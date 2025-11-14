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
    public static List<string> All() => [Quads, Glutes, Hamstrings, Calves, Abs, Chest, Delts, Back];
}

public readonly record struct MuscleSubcategory(MuscleCategory Parent, string Name)
{
    public override string ToString() => Name;

    public static readonly List<MuscleSubcategory> Subcategories =
    [
        new(MuscleCategory.Chest, "Upper"),
        new(MuscleCategory.Chest, "Middle"),
        new(MuscleCategory.Chest, "Lower"),

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
    public static List<(string, string)> All() => Subcategories.Select(subcategory => (subcategory.Parent.Value, subcategory.Name)).ToList();
}