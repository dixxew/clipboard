using Microsoft.ML;
using password_classifier.Models;

internal class Program
{
    public static void Main(string[] args)
    {
        var mlContext = new MLContext();
        var path = Path.Combine(Directory.GetCurrentDirectory(), "password_dataset.csv");
        var data = mlContext.Data.LoadFromTextFile<InputData>(
            path, hasHeader: true, separatorChar: ',', allowQuoting: true);


        // Фичи: преобразуем текст в числовой вектор
        var pipeline = mlContext.Transforms.Text
            .FeaturizeText("Features", nameof(InputData.Text))
            .Append(mlContext.BinaryClassification.Trainers.SdcaLogisticRegression());

        // Обучаем
        var model = pipeline.Fit(data);

        // Сохраняем модель
        mlContext.Model.Save(model, data.Schema, "PasswordModel.zip");

        Console.WriteLine("Модель обучена и сохранена.");
    }
}