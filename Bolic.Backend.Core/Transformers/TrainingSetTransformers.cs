namespace Bolic.Backend.Core.Transformers;

public static class TrainingSetTransformers
{
    public static Api.TrainingSet ConvertSet(Domain.TrainingSet s) =>
        new()
        {
            id = s.Id.Match(id => id.ToString(), () => Guid.NewGuid().ToString()),
            UserId = s.UserId.Match(id => id.ToString(), () => throw new Exceptional("Invalid UserId", 0004)),
            Type = s.Type.IfNone(string.Empty),
            Weight = s.Weight.IfNone(0),
            WeightType = s.WeightType.IfNone(string.Empty),
            Repetitions = s.Repetitions.IfNone(0),
            RepetitionsInReserve = s.RepetitionsInReserve.IfNone(0),
            RateOfPerceivedExertion = s.RateOfPerceivedExertion.IfNone(0),
            Quality = s.Quality.IfNone(0),
            AverageRepetitionTime = s.AverageRepetitionTime.IfNone(0),
            Notes = s.Notes.IfNone(string.Empty)
        };
}