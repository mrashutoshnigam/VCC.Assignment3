using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace VCC.Assignment3.Controllers
{
    public enum MemorySpikeSize
    {
        Small_500MB = 500,
        Medium_1GB = 1024,
        Large_2GB = 2048,
        XLarge_3GB = 3072,
        XXLarge_4GB = 4096
    }

    public enum CpuSpikeDuration
    {
        Short_5Seconds = 5,
        Medium_10Seconds = 10,
        Long_15Seconds = 15,
        XLong_30Seconds = 30,
        XXLong_60Seconds = 60
    }

    [ApiController]
    [Route("api/[controller]")]
    public class SystemController : ControllerBase
    {
        [HttpGet("status")]
        public IActionResult GetStatus()
        {
            return Ok(new { status = "API is running" });
        }

        [HttpPost("memory-spike")]
        public IActionResult TriggerMemorySpike([FromQuery] MemorySpikeSize spikeSize = MemorySpikeSize.Small_500MB)
        {
            try
            {
                var list = new List<byte[]>();
                int sizeMB = (int)spikeSize;
                int chunks = sizeMB / 10;

                for (int i = 0; i < chunks; i++)
                {
                    list.Add(new byte[10 * 1024 * 1024]); // 10 MB chunks
                }

                return Ok(new
                {
                    message = "Memory spike triggered",
                    sizeAllocatedMB = sizeMB,
                    chunks = chunks
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost("cpu-spike")]
        public IActionResult TriggerCpuSpike([FromQuery] CpuSpikeDuration duration = CpuSpikeDuration.Short_5Seconds)
        {
            try
            {
                int durationSeconds = (int)duration;
                int targetMs = durationSeconds * 1000;
                int processorCount = Environment.ProcessorCount;
                int threadsToUse = (int)Math.Ceiling(processorCount * 0.8); // 80% of CPU cores

                var sw = Stopwatch.StartNew();
                var tasks = new List<Task>();

                for (int i = 0; i < threadsToUse; i++)
                {
                    tasks.Add(Task.Run(() =>
                    {
                        var localSw = Stopwatch.StartNew();
                        while (localSw.ElapsedMilliseconds < targetMs)
                        {
                            // Perform CPU-intensive operations
                            for (int j = 0; j < 1000; j++)
                            {
                                Math.Sqrt(Math.Pow(j, 2));
                            }
                        }
                    }));
                }

                Task.WaitAll(tasks.ToArray());

                return Ok(new
                {
                    message = "CPU spike triggered",
                    durationSeconds = durationSeconds,
                    actualDurationMs = sw.ElapsedMilliseconds,
                    cpuUtilizationTarget = "80%",
                    threadsUsed = threadsToUse,
                    totalProcessors = processorCount
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("resources")]
        public IActionResult GetSystemResources()
        {
            var gcInfo = GC.GetGCMemoryInfo();
            var totalMemoryBytes = gcInfo.TotalAvailableMemoryBytes;
            var memoryLoadBytes = gcInfo.MemoryLoadBytes;

            var systemName = RuntimeInformation.OSDescription;
            var osArchitecture = RuntimeInformation.OSArchitecture.ToString();
            var frameworkDescription = RuntimeInformation.FrameworkDescription;
            var processorCount = Environment.ProcessorCount;
            var systemUptime = TimeSpan.FromMilliseconds(Environment.TickCount64).ToString(@"dd\.hh\:mm\:ss");

            double totalMemoryGB = Math.Round(totalMemoryBytes / (1024.0 * 1024.0 * 1024.0), 2);
            double usedMemoryGB = Math.Round(memoryLoadBytes / (1024.0 * 1024.0 * 1024.0), 2);
            double memoryUsagePercent = Math.Round((double)memoryLoadBytes / totalMemoryBytes * 100, 2);

            return Ok(new
            {
                systemName,
                osArchitecture,
                frameworkDescription,
                processorCount,
                systemUptime,
                totalMemoryGB,
                usedMemoryGB,
                memoryUsagePercent
            });
        }
    }
}