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
            StartFromBeginning = false)]
        IReadOnlyList<Api.TrainingSession> input,
        FunctionContext context)
    {
        var logger = context.GetLogger<CosmosTrigger>();

        if (input.Count == 0)
        {
            return;
        }

        logger.LogInformation("Processing {Count} changed documents", input.Count);

        var program =
            from docs in input
                .Traverse(doc =>
                    from id in SuccessEff(doc.id ?? "")
                    from userId in SuccessEff(doc.userId ?? "")
                    select (id, userId)
                )
            select docs;

        var result = await program.Run(runtime);

        result.Match(
            docs => docs.Iter(d => logger.LogInformation("Changed document {Id} for user {UserId}", d.id, d.userId)),
            error => logger.LogError(error.ToException(), "Cosmos trigger processing failed")
        );
    }
}
