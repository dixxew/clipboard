using Microsoft.ML.Data;

namespace clipboard.Models.ML;

public class PasswordPrediction
{
    [ColumnName("PredictedLabel")] public bool IsPassword { get; set; }

    public float Probability { get; set; }
}