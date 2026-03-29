import boto3
import requests
import time
import socket
import logging

logging.basicConfig(
    level=logging.INFO,
    format='%(asctime)s %(levelname)s %(message)s'
)
log = logging.getLogger(__name__)


PROMETHEUS_URL = "http://localhost:9090"
REGION         = "ap-south-2"          
NAMESPACE      = "Ubuntu/Prometheus-Local-VM"  
INTERVAL       = 60                   
HOST           = socket.gethostname() 


METRICS = [
    {
        "name":  "CPUUsagePercent",
        "unit":  "Percent",
        "query": "100 - (avg(rate(node_cpu_seconds_total{mode='idle'}[2m])) * 100)"
    },
    {
        "name":  "MemoryUsagePercent",
        "unit":  "Percent",
        "query": "(1 - (node_memory_MemAvailable_bytes / node_memory_MemTotal_bytes)) * 100"
    },
    {
        "name":  "DiskUsagePercent",
        "unit":  "Percent",
        "query": "(node_filesystem_size_bytes{mountpoint='/',fstype!='tmpfs'} - node_filesystem_free_bytes{mountpoint='/',fstype!='tmpfs'}) / node_filesystem_size_bytes{mountpoint='/',fstype!='tmpfs'} * 100"
    },
    {
        "name":  "LoadAverage1m",
        "unit":  "Count",
        "query": "node_load1"
    },
    {
        "name":  "NetworkRxBytesPerSec",
        "unit":  "Bytes/Second",
        "query": "rate(node_network_receive_bytes_total{device!='lo'}[2m])"
    },
    {
        "name":  "NetworkTxBytesPerSec",
        "unit":  "Bytes/Second",
        "query": "rate(node_network_transmit_bytes_total{device!='lo'}[2m])"
    },
]

cw = boto3.client("cloudwatch", region_name=REGION)


def query_prometheus(promql):
    """Query Prometheus instant query API, return float value or None."""
    try:
        r = requests.get(
            f"{PROMETHEUS_URL}/api/v1/query",
            params={"query": promql},
            timeout=10
        )
        r.raise_for_status()
        results = r.json().get("data", {}).get("result", [])
        if not results:
            log.warning(f"No data for query: {promql[:60]}")
            return None
        # Sum all results (handles multi-device metrics like network)
        total = sum(float(r["value"][1]) for r in results)
        return total
    except requests.exceptions.ConnectionError:
        log.error("Cannot connect to Prometheus — is it running on :9090?")
    except Exception as e:
        log.error(f"Query error: {e}")
    return None


def push_metric(name, value, unit):
    """Push a single metric to CloudWatch."""
    try:
        cw.put_metric_data(
            Namespace=NAMESPACE,
            MetricData=[{
                "MetricName": name,
                "Value":      value,
                "Unit":       unit,
                "Dimensions": [
                    {"Name": "Host",   "Value": HOST},
                    {"Name": "Source", "Value": "Prometheus"}
                ]
            }]
        )
        log.info(f"  OK  {name:<30} = {value:.4f} {unit}")
    except cw.exceptions.ClientError as e:
        log.error(f"  FAIL {name}: {e.response['Error']['Message']}")
    except Exception as e:
        log.error(f"  FAIL {name}: {e}")


def main():
    log.info(f"Prometheus → CloudWatch pusher started")
    log.info(f"  Host:      {HOST}")
    log.info(f"  Namespace: {NAMESPACE}")
    log.info(f"  Region:    {REGION}")
    log.info(f"  Interval:  {INTERVAL}s")
    log.info(f"  Metrics:   {len(METRICS)}")

    while True:
        log.info("── pushing batch ──────────────────────────")
        for m in METRICS:
            val = query_prometheus(m["query"])
            if val is not None:
                push_metric(m["name"], val, m["unit"])
        log.info(f"── done, sleeping {INTERVAL}s ──────────────")
        time.sleep(INTERVAL)


if __name__ == "__main__":
    main()