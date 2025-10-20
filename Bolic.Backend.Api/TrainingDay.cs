namespace Bolic.Backend.Api;

public record TrainingDay
{
    public required string id { get; init; }
    public string name { get; init; }
    public string description { get; init; }
    public DateTime startDate { get; init; }
    public DateTime endDate { get; init; }
    
    public Exercise exercise { get; init; }
}