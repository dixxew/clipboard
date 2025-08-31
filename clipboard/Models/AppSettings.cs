using CommunityToolkit.Mvvm.ComponentModel;

namespace clipboard.Models;

public class AppSettings : ObservableObject
{
    public int PasswordLength { get; set; } = 16;
    public bool PasswordIncludeLetters { get; set; } = true;
    public bool PasswordIncludeDigits { get; set; } = true;
    public bool PasswordIncludeSpecial { get; set; } = true;

    public bool SaveHistory { get; set; } = true;
    public int MaxHistoryItems { get; set; } = 200;

    public string Hotkey { get; set; } = "Alt+V";

    public bool ClearHistoryOnExit { get; set; } = false;
    public bool AutoRemoveAfterPaste { get; set; } = false;
}
