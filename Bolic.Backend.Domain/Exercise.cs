
namespace Bolic.Backend.Domain;

public record Exercise(
    Option<Guid> Id,
    Option<Guid> UserId,
    Option<string> Name,
    Option<string> TargetPosition, // TODO: probably better on the set 
    Option<string> Category,
    Option<string> Equipment,
    Option<string> Notes,
    List<TrainingSet> Sets
);