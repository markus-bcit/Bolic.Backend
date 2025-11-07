namespace Bolic.Backend.Core.PatchOperations;

public static class TrainingDay
{
    public static List<PatchOperation> GetPatchOperations(this Domain.TrainingDay td)
    {
        var po = new List<PatchOperation>();

        td.Name.IfSome(value =>
            po.Add(PatchOperation.Replace("/Name", value)));

        td.Description.IfSome(value =>
            po.Add(PatchOperation.Replace("/Description", value)));

        td.StartDate.IfSome(value =>
            po.Add(PatchOperation.Replace("/StartDate", value)));

        td.EndDate.IfSome(value =>
            po.Add(PatchOperation.Replace("/EndDate", value)));

        td.MicrocycleId.IfSome(value =>
            po.Add(PatchOperation.Replace("/MicrocycleId", value)));

        if (td.Exercises is { Count: > 0 })
        {
            po.Add(PatchOperation.Replace("/Exercises", td.Exercises));
        }
        
        return po;
    }
}