using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace VCC.Assignment3.Controllers
{
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
        public IActionResult TriggerMemorySpike()
        {
            try
            {
                var list = new List<byte[]>();
                for (int i = 0; i < 100; i++)
                {
                    list.Add(new byte[10 * 1024 * 1024]); // 10 MB
                }
                return Ok(new { message = "Memory spike triggered" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost("cpu-spike")]
        public IActionResult TriggerCpuSpike()
        {
            try
            {
                var sw = Stopwatch.StartNew();
                while (sw.ElapsedMilliseconds < 5000) // 5 seconds
                {
                    Math.Sqrt(new Random().Next());
                }
                return Ok(new { message = "CPU spike triggered" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("resources")]
        public IActionResult GetSystemResources()
        {
            var process = Process.GetCurrentProcess();
            var totalMemory = GC.GetGCMemoryInfo().TotalAvailableMemoryBytes;
            var usedMemory = process.WorkingSet64;
            var cpuTime = process.TotalProcessorTime.TotalMilliseconds;
            var systemName = RuntimeInformation.OSDescription;
            return Ok(new
            {
                systemName,
                totalMemory,
                usedMemory,
                cpuTime
            });
        }
    }
}
