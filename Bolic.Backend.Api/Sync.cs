namespace Bolic.Backend.Api;

public record SyncRequest
{
    public DateTime? exportDate { get; init; }
    public string? appVersion { get; init; }
    public string? platform { get; init; }
    public required string localUserId { get; init; }
    public Dictionary<string, JsonElement> data { get; init; }
}
