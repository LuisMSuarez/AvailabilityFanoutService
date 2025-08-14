namespace AvailabilityFanoutSvcApi.Controllers
{
    using AvailabilityFanoutSvcApi.Contracts;
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

            var tasks = urls.Select(async url =>
            {
                try
                {
                    var response = await httpClient.GetAsync(url);
                    var payload = await response.Content.ReadAsStringAsync();
                    return new PingResult(
                        Endpoint: url,
                        StatusCode: (int)response.StatusCode,
                        Payload: payload
                    );
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error pinging {Url}", url);
                    return new PingResult(
                        Endpoint: url,
                        StatusCode: 500,
                        Payload: $"Exception: {ex.Message}"
                    );
                }
            });

            var results = await Task.WhenAll(tasks);
            if (results.Any(ping => !IsSuccessStatusCode(ping.StatusCode)))
            {
                logger.LogWarning("One or more downstream services did not return success.");
                return StatusCode(500, results);
            }

            return Ok(results);
        }
        private static bool IsSuccessStatusCode(int statusCode)
        {
            return statusCode >= 200 && statusCode <= 299;
        }
    }
}
