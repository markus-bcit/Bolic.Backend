namespace Bolic.Backend.Api;

public record StorageWrapper<T>
{
    public string? id { get; init; }
    public string? userId { get; init; }
    public T? data { get; init; }
}