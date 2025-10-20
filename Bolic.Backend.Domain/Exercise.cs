namespace Bolic.Backend.Models;

public record Exercise(
    Option<Guid> Id,
    Option<Guid> UserId,
    Option<string> Name,
    Option<string> TargetPosition,
    Option<string> Category,
    Option<string> Equipment,
    Option<string> Notes,
    List<TrainingSet> Sets
);