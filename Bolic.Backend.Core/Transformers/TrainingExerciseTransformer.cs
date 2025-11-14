using static Bolic.Backend.Core.Util.DomainExtensions;
namespace Bolic.Backend.Core.Transformers;

public static class TrainingExerciseTransformer
{
    public static Option<Domain.TrainingExercise> ToDt(this Api.TrainingExercise e) =>
        new Domain.TrainingExercise(
            Id: parseGuid(e.Id ?? ""),
            UserId: parseGuid(e.UserId).IfNone(() => throw new Exceptional("Missing UserId", 0000)),
            TrainingDayId: parseGuid(e.TrainingDayId),
            MuscleCategory: parseMuscleCategory(e.MuscleCategory),
            MuscleSubcategory: parseMuscleSubcategory(e.MuscleCategory, e.MuscleSubcategory),
            TargetRepetitions: e.TargetRepetitions,
            TargetRepetitionsInReserve: e.TargetRepetitionsInReserve,
            Name: e.Name,
            TargetPosition: e.TargetPosition,
            Equipment: e.Equipment,
            Notes: e.Notes,
            Sets: e.Sets.Select(TrainingSetTransformer.ToDt).Select(a => a.IfNone(() => throw new Exceptional("Invalid TrainingSet", 0018))).ToList()
        );

    public static Option<Api.TrainingExercise> ToApi(this Domain.TrainingExercise e)
    {
        return new Api.TrainingExercise(){
            Id = e.Id.Match(id => id.ToString(), () => throw new Exceptional("Missing Id", 0015)),
            UserId = e.UserId.Match(id => id.ToString(), () => throw new Exceptional("Invalid UserId", 0017)),
            TrainingDayId = e.TrainingDayId.Match(id => id.ToString(), () => ""),
            MuscleCategory = e.MuscleCategory.Match(mc => mc.Value, () => ""),
            MuscleSubcategory = e.MuscleSubcategory.Match(ms => ms.Name, () => ""),
            TargetRepetitions = e.TargetRepetitions.IfNone(""),
            TargetRepetitionsInReserve = e.TargetRepetitionsInReserve.IfNone(""),
            Name = e.Name.IfNone(""),
            TargetPosition = e.TargetPosition.IfNone(""),
            Equipment = e.Equipment.IfNone(""),
            Notes = e.Notes.IfNone(""),
            Sets = e.Sets.Select(TrainingSetTransformer.ToApi).Select(a => a.IfNone(() => throw new Exceptional("Invalid TrainingSet", 0019))).ToList()
        };
    }
}