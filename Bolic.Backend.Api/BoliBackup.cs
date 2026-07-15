namespace Bolic.Backend.Api;

public record SyncRequest(
    string UserId,
    DateTime ClientTimestamp,
    Dictionary<string, JsonElement> Data  
);
