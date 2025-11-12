using Bolic.Backend.Domain;

namespace Bolic.Backend.Core.Util;

public static class DomainExtensions
{
    public static Option<MuscleCategory> parseMuscleCategory(string? value) =>
        !string.IsNullOrEmpty(value) && MuscleCategory.All().Contains(value)
            ? new MuscleCategory(value)
            : Option<MuscleCategory>.None;

    public static Option<MuscleSubcategory> parseMuscleSubcategory(string? parent, string? value)
    {
        return !string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(parent) && MuscleSubcategory.All().Contains((parent, value))
            ? new MuscleSubcategory(parent, value)
            : Option<MuscleSubcategory>.None;
    }
}