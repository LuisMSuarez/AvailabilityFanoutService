using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace AvailabilityFanoutSvcApi.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class PingController : ControllerBase
    {
        private readonly ILogger<PingController> logger;
        private readonly HttpClient httpClient;
        private readonly IEnumerable<string> urls =
        [
            "https://gamers-hub.azurewebsites.net/",
            "https://gamers-hub-api.azurewebsites.net/v1/ping"
        ];


        public PingController(
            ILogger<PingController> logger,
            HttpClient httpClient)
        {
            this.logger = logger;
            this.httpClient = httpClient;
        }

        [HttpGet(Name = "Ping")]
        public async Task<IActionResult> Ping()
        {
            this.logger.LogInformation("Ping endpoint called");

            var tasks = this.urls.Select(url => this.httpClient.GetAsync(url));
            await Task.WhenAll(tasks);
            return Ok();
        }
    }
}
