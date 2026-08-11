using static Bolic.Backend.Core.Util.DomainExtensions;

namespace Bolic.Backend.Core.Transformers;

public static class TrainingExerciseTransformer
{
    public static Eff<Domain.TrainingExercise> ToDt(this Api.TrainingExercise e, string userId) =>
        liftEff(_ => new Domain.TrainingExercise(
            Id: parseGuid(e.id ?? e.exerciseId)
                .IfNone(() =>
                    throw new Exceptional($"Missing both id and exerciseId {e.name}", 0015)
                ),
            UserId: parseGuid(userId),
            TrainingDayIds: e.trainingDayIds.Select(parseGuid)
                .Where(opt => opt.IsSome)
                .Select(opt => opt.IfNone(Guid.Empty))
                .ToList(),
            MuscleCategory: parseMuscleCategory(e.muscleCategory),
            MuscleSubcategory: parseMuscleSubcategory(e.muscleCategory, e.muscleSubcategory),
            Name: e.name ?? e.exerciseName,
            TargetPosition: e.targetPosition,
            TargetRepetitions: e.targetRepetitions,
            TargetRepetitionsInReserve: e.targetRepetitionsInReserve,
            TargetNumberOfSets: e.targetNumberOfSets,
            Equipment: e.equipment,
            Notes: e.notes,
            Version: e.version,
            Sets: e.sets.Select(set => set.ToDt(userId)).Select(a => a.Run().ThrowIfFail()).ToList()
        ));

    public static Eff<Api.TrainingExercise> ToApi(this Domain.TrainingExercise e) =>
        liftEff(() =>
            new Api.TrainingExercise()
            {
                id = e.Id.Match(
                    id => id.ToString(),
                    () => throw new Exceptional("Missing Id", 0015)
                ),
                userId = e.UserId.Match(
                    id => id.ToString(),
                    () => throw new Exceptional("Invalid UserId", 0017)
                ),
                trainingDayIds = e.TrainingDayIds.Match(
                    ids => ids.ConvertAll(id => id.ToString()),
                    () => []
                ),
                muscleCategory = e.MuscleCategory.Match(mc => mc.Value, () => ""),
                muscleSubcategory = e.MuscleSubcategory.Match(ms => ms.Name, () => ""),
                targetRepetitions = e.TargetRepetitions.IfNone(""),
                targetRepetitionsInReserve = e.TargetRepetitionsInReserve.IfNone(""),
                targetNumberOfSets = e.TargetNumberOfSets.IfNone(0),
                name = e.Name.IfNone(""),
                targetPosition = e.TargetPosition.IfNone(""),
                equipment = e.Equipment.IfNone(""),
                notes = e.Notes.IfNone(""),
                version = e.Version.IfNone(0),
                sets = e
                    .Sets.Select(TrainingSetTransformer.ToApi)
                    .Select(a => a.Run().ThrowIfFail())
                    .ToList(),
            }
        );
}
