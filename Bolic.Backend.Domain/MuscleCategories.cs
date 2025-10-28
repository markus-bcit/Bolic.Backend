namespace Bolic.Backend.Domain;

public readonly record struct MuscleCategory(string Value)
{
    public static readonly MuscleCategory Quads = new("Quads");
    public static readonly MuscleCategory Glutes = new("Glutes");
    public static readonly MuscleCategory Hamstrings = new("Hamstrings");
    public static readonly MuscleCategory Calves = new("Calves");
    public static readonly MuscleCategory Abs = new("Abs");
    public static readonly MuscleCategory Chest = new("Chest");
    public static readonly MuscleCategory Delts = new("Delts");
    public static readonly MuscleCategory Back = new("Back");

    public override string ToString() => Value;

    public static implicit operator string(MuscleCategory c) => c.Value;
    public static implicit operator MuscleCategory(string s) => new(s);
}

public readonly record struct MuscleSubcategory(MuscleCategory Parent, string Name);

public static class MuscleData
{
    public static readonly List<MuscleSubcategory> Subcategories =
    [
        new(MuscleCategory.Delts, "Front"),
        new(MuscleCategory.Delts, "Lateral"),
        new(MuscleCategory.Delts, "Rear"),

        new(MuscleCategory.Back, "Upper Traps"),
        new(MuscleCategory.Back, "Mid Traps"),
        new(MuscleCategory.Back, "Lower Traps"),
        new(MuscleCategory.Back, "Upper Lats"),
        new(MuscleCategory.Back, "Mid Lats"),
        new(MuscleCategory.Back, "Lower Lats")
    ];

    public static bool IsValidSubcategory(MuscleCategory parent, string name) =>
        Subcategories.Any(s =>
            s.Parent.Equals(parent) &&
            s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
}