using System.Security.Policy;
using Bolic.Backend.Api;

namespace Bolic.Backend.Core.Transformers;

public class SyncRequestTransformer
{
    private const string UserPrefix = "@bolic:user";
    private const string SessionsKey = ":sessions:";
    private const string ExercisesKey = ":exercises";
    
    public static Eff<Domain.Sync> ToDt(SyncRequest request, string userId) =>
        liftEff(() =>
        {
            var prefix = $"{UserPrefix}:{request.localUserId}";

            var sessionLinq =
                request.data
                    .Where(kvp => kvp.Key.StartsWith($"{prefix}{SessionsKey}"))
                    .SelectMany(kvp => kvp.Value.Deserialize<List<Api.TrainingSession>>() ?? [])
                    .Select(api => api.ToDt(userId).IfNone(() => throw new Exceptional("Invalid session", 0002)));

            var exercisesLinq =
                request.data
                    .Where(kvp => kvp.Key.StartsWith($"{prefix}{ExercisesKey}"))
                    .SelectMany(kvp => kvp.Value.Deserialize<List<Api.TrainingExercise>>() ?? [])
                    .Select(item => item.ToDt(userId).IfNone(() => throw new Exceptional("Invalid exercise", 0003)));
            
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