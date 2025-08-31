using System.Runtime.InteropServices;
using Avalonia;
using clipboard.Interop;

namespace clipboard.Helpers;

public static class CursorHelper
{
    [DllImport("user32.dll")]
    private static extern bool GetCursorPos(out WinApi.Point lpPoint);

    public static PixelPoint GetCursorPosition()
    {
        GetCursorPos(out var point);
        return new PixelPoint(point.x, point.y);
    }
}