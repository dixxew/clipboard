using System;
using clipboard.Models;

namespace clipboard.Services;

public sealed class ClipboardAddedEventArgs(ClipboardEntry e) : EventArgs
{
    public ClipboardEntry Entry { get; } = e;
}