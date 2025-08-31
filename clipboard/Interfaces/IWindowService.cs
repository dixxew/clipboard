using Avalonia.Controls;

namespace clipboard.Interfaces;

public interface IWindowService
{
    void Show();
    void Hide();
    void RegisterWindow(Window w);
    void ToggleVisibility();
    void OpenWeb(string url);
}