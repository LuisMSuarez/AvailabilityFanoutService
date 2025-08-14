namespace AvailabilityFanoutSvcApi.Contracts
{
    public record PingResult(
        string Endpoint,
        int StatusCode,
        string Payload
    );
}
