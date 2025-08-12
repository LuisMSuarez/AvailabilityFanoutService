namespace AvailabilityFanoutSvcApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    [Route("v1/[controller]")]
    [ApiController]
    public class PingController(
        ILogger<PingController> logger,
        HttpClient httpClient,
        IConfiguration configuration) : ControllerBase
    {
        private readonly IEnumerable<string> urls = configuration.GetSection("PingUrls").Get<IEnumerable<string>>() ?? [];

        [HttpGet(Name = "Ping")]
        public async Task<IActionResult> Ping()
        {
            logger.LogInformation("Ping endpoint called");

            var responses = await Task.WhenAll(this.urls.Select(url => httpClient.GetAsync(url)));
            if (responses.Any(r => !r.IsSuccessStatusCode))
            {
                logger.LogWarning("One or more downstream services did not return success.");
                return StatusCode(500); // Bad Gateway or another appropriate status
            }
            return Ok();
        }
    }
}
