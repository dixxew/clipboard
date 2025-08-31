using Avalonia;
using Avalonia.Controls;
using clipboard.Helpers;
using clipboard.Interfaces;

namespace clipboard.Services;

public sealed class WindowService : IWindowService
{
    private Window? _window;

    public void RegisterWindow(Window window) => _window = window;

    public void Show()
    {
        var w = _window;
        Avalonia.Threading.Dispatcher.UIThread.Post(() =>
        {
            if (w == null) return;
            w.Show();
            w.Activate();
        });
    }

    public void Hide()
    {
        var w = _window;
        Avalonia.Threading.Dispatcher.UIThread.Post(() =>
        {
            w?.Hide();
        });
    }

    public void ToggleVisibility()
    {
        var w = _window;
        Avalonia.Threading.Dispatcher.UIThread.Post(() =>
        {
            if (w == null) return;

            if (w.IsVisible)
            {
                w.Hide();
            }
            else
            {
                var cursorPos = CursorHelper.GetCursorPosition();
                w.Position = new PixelPoint(cursorPos.X, cursorPos.Y + 10); // +10 чтобы окно не перекрывало курсор
                w.Show();
                w.Activate();
            }
        });
    }

    public void OpenWeb(string url)
    {
        try
        {
            using var p = new System.Diagnostics.Process();
            p.StartInfo.UseShellExecute = true;
            p.StartInfo.FileName = url;
            p.Start();
        }
        catch
        {
            // ignored
        }
    }
}