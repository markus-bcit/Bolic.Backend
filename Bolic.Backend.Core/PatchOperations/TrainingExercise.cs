using Bolic.Backend.Core.Transformers;

namespace Bolic.Backend.Core.PatchOperations;

public static class TrainingExercise
{
    public static List<PatchOperation> GetPatchOperations(this Domain.TrainingExercise td)
    {
        var po = new List<PatchOperation>();

        td.TrainingDayId.IfSome(v => po.Add(PatchOperation.Replace("/TrainingDayId", v.ToString())));
        td.MuscleCategory.IfSome(v => po.Add(PatchOperation.Replace("/MuscleCategory", v.ToString())));
        td.MuscleSubcategory.IfSome(v => po.Add(PatchOperation.Replace("/MuscleSubcategory", v.ToString())));
        td.Name.IfSome(v => po.Add(PatchOperation.Replace("/Name", v)));
        td.TargetPosition.IfSome(v => po.Add(PatchOperation.Replace("/TargetPosition", v)));
        td.TargetRepetitions.IfSome(v => po.Add(PatchOperation.Replace("/TargetRepetitions", v)));
        td.TargetRepetitionsInReserve.IfSome(v => po.Add(PatchOperation.Replace("/TargetRepetitionsInReserve", v)));
        td.Equipment.IfSome(v => po.Add(PatchOperation.Replace("/Equipment", v)));
        td.Notes.IfSome(v => po.Add(PatchOperation.Replace("/Notes", v)));

        if (td.Sets is { Count: > 0 })
        {
            po.Add(PatchOperation.Replace("/Sets",
                td.Sets.Select(a =>
                    a.ToApi().Match(b => b, () => throw new Exceptional("Bad set list", 0203)))));
        }

        return po;
    }
}