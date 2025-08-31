using Microsoft.ML.Data;

namespace clipboard.Models.ML;

public class PasswordInput
{
    [LoadColumn(0)] public string Text { get; set; } = string.Empty;
}