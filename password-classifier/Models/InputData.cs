using Microsoft.ML.Data;

namespace password_classifier.Models;

public class InputData
{
    [LoadColumn(0)]
    public required string Text { get; set; }
    [LoadColumn(1)]
    public required bool Label { get; set; }
}

public class OutputPrediction
{
    [ColumnName("PredictedLabel")]
    public bool Prediction { get; set; }

    public float Probability { get; set; }
    public float Score { get; set; }
}