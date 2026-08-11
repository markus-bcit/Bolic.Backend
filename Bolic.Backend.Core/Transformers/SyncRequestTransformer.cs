using Bolic.Backend.Api;

namespace Bolic.Backend.Core.Transformers;

public static class SyncRequestTransformer
{
    private const string UserPrefix = "@bolic:user";
    private const string SessionsKey = ":sessions:";
    private const string ExercisesKey = ":exercises";

    public static Eff<Domain.Sync> ToDt(SyncRequest request, string userId) =>
        liftEff(() =>
        {
            var prefix = $"{UserPrefix}:{request.localUserId}";

            var exercisesLinq =
                request.data
                    .Where(kvp => kvp.Key.StartsWith($"{prefix}{ExercisesKey}"))
                    .SelectMany(kvp => kvp.Value.Deserialize<List<StorageWrapper<Api.TrainingExercise>>>() ?? [])
                    .Select(wrapper =>
                    {
                        var data = wrapper.data ?? throw new Exceptional("Missing exercise data", 0304);
                        return data with { id = data.id ?? wrapper.id };
                    })
                    .Select(data => data.ToDt(userId).Run().ThrowIfFail());

            var sessionLinq =
                request.data
                    .Where(kvp => kvp.Key.StartsWith($"{prefix}{SessionsKey}"))
                    .SelectMany(kvp => kvp.Value.Deserialize<List<StorageWrapper<Api.TrainingSession>>>() ?? [])
                    .Select(wrapper =>
                    {
                        var data = wrapper.data ?? throw new Exceptional("Missing session data", 0300);
                        return data with { id = data.id ?? wrapper.id };
                    })
                    .Select(data => data.ToDt(userId).Run().ThrowIfFail());

            var exercises = toSeq(exercisesLinq);
            var sessions = toSeq(sessionLinq);

            return new Domain.Sync(
                UserId: userId,
                ExportDate: request.exportDate ?? Option<DateTime>.None,
                AppVersion: request.appVersion,
                Platform: request.platform,
                TrainingSessions: sessions,
                Exercises: exercises
            );
        });
}
