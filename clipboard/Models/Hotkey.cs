using System;

namespace clipboard.Models;

public record Hotkey(string Spec)
{
    public bool Alt => Spec.Contains("Alt", StringComparison.OrdinalIgnoreCase);
    public bool Ctrl => Spec.Contains("Ctrl", StringComparison.OrdinalIgnoreCase);
    public bool Shift => Spec.Contains("Shift", StringComparison.OrdinalIgnoreCase);
    public string Key => Spec.Split('+', StringSplitOptions.RemoveEmptyEntries)[^1].Trim().ToUpperInvariant();
}