using Bolic.Backend.Domain;
using MuscleCategory = Bolic.Backend.Domain.MuscleCategory;

namespace Bolic.Backend.Core.Transformers;

public static class TrainingExerciseTransformer
{
    public static Option<Domain.TrainingExercise> ToDt(this Api.TrainingExercise e) =>
        new Domain.TrainingExercise(
            Id: parseGuid(e.Id),
            UserId: parseGuid(e.UserId).IfNone(() => throw new Exceptional("Missing UserId", 0000)),
            TrainingDayId: parseGuid(e.TrainingDayId),
            MuscleCategory: new MuscleCategory(e.MuscleCategory),
            MuscleSubcategory: new MuscleSubcategory(new MuscleCategory(e.MuscleCategory), e.MuscleSubcategory),
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
            MuscleCategory = e.MuscleCategory.IfNone(string.Empty),
            MuscleSubcategory = e.MuscleSubcategory.Match(ms => ms.Name, () => string.Empty),
            TargetRepetitions = e.TargetRepetitions.IfNone(string.Empty),
            TargetRepetitionsInReserve = e.TargetRepetitionsInReserve.IfNone(string.Empty),
            Name = e.Name.IfNone(string.Empty),
            TargetPosition = e.TargetPosition.IfNone(string.Empty),
            Equipment = e.Equipment.IfNone(string.Empty),
            Notes = e.Notes.IfNone(string.Empty),
            Sets = e.Sets.Select(TrainingSetTransformer.ToApi).Select(a => a.IfNone(() => throw new Exceptional("Invalid TrainingSet", 0019))).ToList()
        };
    }
}