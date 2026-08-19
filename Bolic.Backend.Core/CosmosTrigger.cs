using Bolic.Backend.Api;
using Bolic.Shared.Database.Implementation;

namespace Bolic.Backend.Core;

public class CosmosTrigger(Runtime runtime)
{
    [Function("CosmosTrigger")]
    public async Task Run(
        [CosmosDBTrigger(
            databaseName: "bolic",
            containerName: "training-sessions",
            Connection = "CosmosConnection",
            LeaseContainerName = "leases",
            CreateLeaseContainerIfNotExists = true,
            StartFromBeginning = false
        )]
            IReadOnlyList<Api.TrainingSession> input,
        FunctionContext context
    )
    {
        var byUser = toSeq(
            input.GroupBy(a => a.userId).Select(g => (UserId: g.Key, Sessions: toSeq(g)))
        );

        var program = byUser.Traverse(a =>
            from user in CosmosDatabase.ReadItem<Api.User>(
                new Shared.Database.Api.ReadRequest("", a.UserId ?? "", "analytics", "bolic")
            )
            select user
        );
    }

    public static Eff<Seq<(string? UserId, Seq<TrainingSession> Sessions)>> SplitByUserId(
        List<Api.TrainingSession> input
    ) =>
        liftEff(() =>
            toSeq(input.GroupBy(a => a.userId).Select(g => (UserId: g.Key, Sessions: toSeq(g))))
        );
}
