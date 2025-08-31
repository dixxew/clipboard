using System;
using clipboard.Interfaces;
using clipboard.Interop;
using clipboard.Models;

namespace clipboard.Services;

public sealed class HotkeyService : IHotkeyService
{
    private readonly IMessageLoopWindow _msg;
    public event EventHandler? Triggered;

    public HotkeyService(IMessageLoopWindow msg)
    {
        _msg = msg;
        _msg.EnsureStarted(OnMsg);
    }

    public void Register(Hotkey hk)
    {
        var (mod, vk) = Parse(hk);
        _msg.RegisterHotKey(mod, vk);
    }

    // HotkeyService.cs — OnMsg
    private void OnMsg(uint msg)
    {
        if (msg == WinApi.WmHotkey)
            Avalonia.Threading.Dispatcher.UIThread.Post(() =>
                Triggered?.Invoke(this, EventArgs.Empty));
    }


    private static (uint mod, uint vk) Parse(Hotkey hk)
    {
        uint mod = 0;
        if (hk.Alt) mod |= WinApi.ModAlt;
        if (hk.Ctrl) mod |= WinApi.ModControl;
        if (hk.Shift) mod |= WinApi.ModShift;

        return hk.Key is [>= 'A' and <= 'Z'] ? (mod, hk.Key[0]) :
            (mod == 0 ? 0x0001u : mod, 0x56u);
    }
}