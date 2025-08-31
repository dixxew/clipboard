using System;

namespace clipboard.Interfaces;

public interface IMessageLoopWindow
{
    void EnsureStarted(Action<uint> onMessage);
    void AddClipboardListener();
    void RegisterHotKey(uint modifiers, uint vk);
}