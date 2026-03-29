# VCC Assignment 3: Hybrid Cloud Auto-Scaling System

**Student Name:** Ashutosh Nigam  
**Roll Number:** M25AI2006  
**Course:** Virtual Cloud Computing (VCC)

---

## Problem Statement

Create a local VM and implement a mechanism to monitor resource usage. Configure it to auto-scale to a public cloud (e.g., GCP, AWS, or Azure) when resource usage exceeds 75%.

### Objectives

1. Set up a local virtual machine environment
2. Implement real-time resource monitoring for CPU and Memory
3. Configure auto-scaling policies that trigger cloud deployment when local resources exceed 75% utilization
4. Demonstrate the complete workflow from local deployment to cloud migration
5. Provide comprehensive documentation and demonstration of the system

---

## Project Overview

This ASP.NET Core Web API application serves as a **System Resource Monitor and Load Generator** designed to test and demonstrate hybrid cloud auto-scaling capabilities. The application provides endpoints to:

- Monitor system resources (CPU, Memory)
- Trigger controlled CPU spikes
- Trigger controlled Memory spikes
- Provide real-time system status

This API acts as the core application deployed on a local VM that can be monitored by tools like Prometheus/Grafana, which then trigger auto-scaling to cloud platforms when resource thresholds are exceeded.

---

## Technology Stack

- **Framework:** ASP.NET Core 8.0
- **Language:** C# with .NET 8.0
- **API Documentation:** Swagger/OpenAPI
- **Architecture:** RESTful Web API

---

## Prerequisites

Before running this application, ensure you have:

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) installed
- Windows, Linux, or macOS operating system
- A code editor (Visual Studio, VS Code, or Rider recommended)
- Postman or similar tool for API testing (optional, Swagger UI included)

---

## How to Run the Application

### Method 1: Using .NET CLI (Recommended)

1. **Clone or navigate to the project directory:**
   ```bash
   cd VCC.Assignment3
   ```

2. **Restore dependencies:**
   ```bash
   dotnet restore
   ```

3. **Build the project:**
   ```bash
   dotnet build
   ```

4. **Run the application:**
   ```bash
   dotnet run
   ```

5. **Access the application:**
   - **Swagger UI:** http://localhost:5000/swagger or https://localhost:5001/swagger
   - **API Base URL:** http://localhost:5000/api or https://localhost:5001/api

### Method 2: Using Visual Studio

1. Open `VCC.Assignment3.sln` in Visual Studio
2. Press `F5` to run with debugging or `Ctrl+F5` to run without debugging
3. Browser will automatically open with Swagger UI

## API Endpoints

### 1. **GET** `/api/system/status`
Check if the API is running.

**Response:**
```json
{
  "status": "API is running"
}
```

---

### 2. **GET** `/api/system/resources`
Get current system resource information.

**Response:**
```json
{
  "systemName": "Microsoft Windows 10...",
  "osArchitecture": "X64",
  "frameworkDescription": ".NET 8.0.0",
  "processorCount": 8,
  "systemUptime": "01.12:34:56",
  "totalMemoryGB": 16.0,
  "usedMemoryGB": 8.5,
  "memoryUsagePercent": 53.12
}
```

---

### 3. **POST** `/api/system/memory-spike?spikeSize=<size>`
Trigger a controlled memory spike for testing auto-scaling.

**Query Parameters:**
- `spikeSize` (optional): Size of memory to allocate
  - `Small_500MB` (default): 500 MB
  - `Medium_1GB`: 1 GB
  - `Large_2GB`: 2 GB
  - `XLarge_3GB`: 3 GB
  - `XXLarge_4GB`: 4 GB

**Example:**
```
POST http://localhost:5000/api/system/memory-spike?spikeSize=Medium_1GB
```

**Response:**
```json
{
  "message": "Memory spike triggered",
  "sizeAllocatedMB": 1024,
  "chunks": 102
}
```

---

### 4. **POST** `/api/system/cpu-spike?duration=<duration>`
Trigger a controlled CPU spike for testing auto-scaling.

**Query Parameters:**
- `duration` (optional): Duration of CPU spike
  - `Short_5Seconds` (default): 5 seconds
  - `Medium_10Seconds`: 10 seconds
  - `Long_15Seconds`: 15 seconds
  - `XLong_30Seconds`: 30 seconds
  - `XXLong_60Seconds`: 60 seconds

**Example:**
```
POST http://localhost:5000/api/system/cpu-spike?duration=Medium_10Seconds
```

**Response:**
```json
{
  "message": "CPU spike triggered",
  "durationSeconds": 10,
  "actualDurationMs": 10005,
  "cpuUtilizationTarget": "80%",
  "threadsUsed": 6,
  "totalProcessors": 8
}
```

---

## Testing the Application

### Using Swagger UI (Easiest Method)

1. Run the application
2. Navigate to http://localhost:5000/swagger
3. Expand any endpoint and click "Try it out"
4. Modify parameters if needed and click "Execute"
5. View the response below

### Using curl

```bash
# Check status
curl -X GET http://localhost:5000/api/system/status

# Get system resources
curl -X GET http://localhost:5000/api/system/resources

# Trigger memory spike
curl -X POST "http://localhost:5000/api/system/memory-spike?spikeSize=Medium_1GB"

# Trigger CPU spike
curl -X POST "http://localhost:5000/api/system/cpu-spike?duration=Long_15Seconds"
```

### Using PowerShell

```powershell
# Check status
Invoke-RestMethod -Uri "http://localhost:5000/api/system/status" -Method Get

# Get system resources
Invoke-RestMethod -Uri "http://localhost:5000/api/system/resources" -Method Get

# Trigger memory spike
Invoke-RestMethod -Uri "http://localhost:5000/api/system/memory-spike?spikeSize=Large_2GB" -Method Post

# Trigger CPU spike
Invoke-RestMethod -Uri "http://localhost:5000/api/system/cpu-spike?duration=XLong_30Seconds" -Method Post
```

---

## Project Structure

```
VCC.Assignment3/
│
├── Controllers/
│   └── SystemController.cs        # API endpoints for system monitoring and load generation
│
├── Properties/
│   └── launchSettings.json         # Application launch configuration
│
├── appsettings.json                # Application configuration
├── appsettings.Development.json    # Development environment settings
├── Program.cs                      # Application entry point and configuration
├── VCC.Assignment3.csproj          # Project file
└── README.md                       # This file
```

---

## Deliverables

### 1. Document Report
Step-by-step instructions covering:
- ✅ Creation of a local VM (VirtualBox/VMware/Hyper-V)
- ✅ Implementation of resource monitoring (Prometheus + Grafana)
- ✅ Configuration of cloud auto-scaling policies
- ✅ Deployment of this sample application

### 2. Architecture Design
Diagram illustrating:
- Local VM resource monitoring
- Threshold detection (>75% utilization)
- Auto-scaling trigger to cloud (GCP/AWS/Azure)
- Cloud deployment and load balancing

### 3. Source Code Repository
- ✅ This repository contains the monitoring API
- ✅ Deployment configurations
- ✅ Application code

### 4. Recorded Video Demo
Video demonstration showing:
- Setup process
- Resource monitoring in action
- Auto-scaling process
- Detailed voice-over explanation

---

## How This Application Supports Auto-Scaling Testing

This application is specifically designed to test auto-scaling mechanisms:

1. **Baseline Monitoring:** The `/api/system/resources` endpoint provides real-time metrics that can be scraped by Prometheus

2. **Load Generation:** The CPU and Memory spike endpoints allow controlled testing of auto-scaling triggers

3. **Threshold Testing:** By triggering spikes, you can simulate scenarios where local VM resources exceed 75% utilization

4. **Integration Ready:** The API can be integrated with:
   - Prometheus for metrics collection
   - Grafana for visualization
   - Cloud-native monitoring solutions (CloudWatch, Azure Monitor, GCP Monitoring)
   - Custom auto-scaling scripts

---

## Example Auto-Scaling Workflow

1. **Deploy application on local VM** (VirtualBox/VMware)
2. **Set up Prometheus** to scrape `/api/system/resources` endpoint
3. **Configure Grafana dashboards** to visualize CPU and Memory usage
4. **Create alerting rules** when usage exceeds 75%
5. **Trigger auto-scaling script** that:
   - Provisions cloud VM (AWS EC2, GCP Compute Engine, Azure VM)
   - Deploys this application to cloud
   - Configures load balancer to distribute traffic
6. **Test by calling** `/api/system/cpu-spike` or `/api/system/memory-spike`
7. **Observe** automatic migration to cloud when thresholds are exceeded

---

## Monitoring Integration Examples

### Prometheus Configuration (prometheus.yml)
```yaml
scrape_configs:
  - job_name: 'vcc-assignment3'
    metrics_path: '/api/system/resources'
    scrape_interval: 15s
    static_configs:
      - targets: ['localhost:5000']
```

### Alert Rule Example
```yaml
groups:
  - name: resource_alerts
    rules:
      - alert: HighCPUUsage
        expr: cpu_usage_percent > 75
        for: 2m
        annotations:
          summary: "CPU usage exceeded 75%"
      
      - alert: HighMemoryUsage
        expr: memory_usage_percent > 75
        for: 2m
        annotations:
          summary: "Memory usage exceeded 75%"
```

---

## Troubleshooting

### Port Already in Use
If port 5000 or 5001 is already in use, modify `Properties/launchSettings.json` to use different ports.

### Memory Spike Fails
Ensure your system has enough available memory. Start with smaller spike sizes and gradually increase.

### CPU Spike Doesn't Show High Usage
The application uses 80% of available cores. On systems with many cores, this may not show as 100% CPU in task manager.

---

## Future Enhancements

- [ ] Add metrics exporter for Prometheus format
- [ ] Implement WebSocket for real-time monitoring
- [ ] Add database connectivity spike testing
- [ ] Include network bandwidth testing
- [ ] Add disk I/O spike testing
- [ ] Implement health check endpoints
- [ ] Add authentication and rate limiting

---

## License

This project is created for educational purposes as part of VCC Assignment 3.

---

## Contact

**Student:** Ashutosh Nigam  
**Roll Number:** M25AI2006  

For questions or issues related to this assignment, please contact through the official course channels.

---

## Acknowledgments

- ASP.NET Core Documentation
- Prometheus & Grafana Community
- Virtual Cloud Computing Course Materials

---

**Last Updated:** March 2026