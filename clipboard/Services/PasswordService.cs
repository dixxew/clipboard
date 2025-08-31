using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using clipboard.Interfaces;
using clipboard.Models;
using clipboard.Models.ML;
using Microsoft.ML;

namespace clipboard.Services;

public class PasswordService : IPasswordService
{
    private static readonly string Letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private static readonly string Digits = "0123456789";
    private static readonly string Special = "!@#$%^&*()-_=+[]{};:,.<>?";

    private readonly PredictionEngine<PasswordInput, PasswordPrediction> _predictor;
    
    public PasswordService()
    {
        var mlContext = new MLContext();
        var modelPath = Path.Combine(AppContext.BaseDirectory, "PasswordModel.zip");
        using var fileStream = new FileStream(modelPath, FileMode.Open, FileAccess.Read, FileShare.Read);
        var model = mlContext.Model.Load(fileStream, out _);
        _predictor = mlContext.Model.CreatePredictionEngine<PasswordInput, PasswordPrediction>(model);
    }
    
    public bool IsProbablyPassword(string s)
    {
        if (string.IsNullOrWhiteSpace(s)) return false;
        if (s.Length < 6 || s.Length > 64) return false;

        if (s.Contains(' ')) return false;
        if (s.All(char.IsLetter)) return false;
        
        var prediction = _predictor.Predict(new PasswordInput { Text = s });

        // Можно поиграться с порогом, 0.7–0.8 — норм
        return prediction.Probability >= 0.75f;
    }

    public string Encrypt(string plain)
    {
        using var aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes("supersecretkeysupersecretkey12"); // 32 байта
        aes.IV = new byte[16];
        using var enc = aes.CreateEncryptor();
        var bytes = Encoding.UTF8.GetBytes(plain);
        var encBytes = enc.TransformFinalBlock(bytes, 0, bytes.Length);
        return Convert.ToBase64String(encBytes);
    }
    
    public object GeneratePassword()
    {
        var allChars = Letters + Digits + Special;
        var rnd = new Random();

        var password = new string(Enumerable.Range(0, 16) // длина 16 символов
            .Select(_ => allChars[rnd.Next(allChars.Length)])
            .ToArray());

        var res = new ClipboardEntry
        {
            Content = password,
            CreatedAt = DateTime.Now
        };
        
        res.SetKindPassword();
        
        return res;
    }
}