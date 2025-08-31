using System;
using clipboard.Models;

namespace clipboard.Interfaces;

public interface IHotkeyService
{
    event EventHandler? Triggered;
    void Register(Hotkey hk);
}