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

    }
}
