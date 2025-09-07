using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using clipboard.Enum;
using clipboard.Interfaces;
using clipboard.Interop;
using clipboard.Models;

namespace clipboard.Services;

public sealed class ClipboardHistoryService(IMessageLoopWindow msgWin, IStorageService storage, IPasswordService passwords)
    : IClipboardHistoryService
{
    public ObservableCollection<ClipboardEntry> Items { get; } = new();

    
    private int _ignoreNext;
    private string? _currentFilter;
    

    public void Start()
    {
        msgWin.EnsureStarted(HandleMessage);
        msgWin.AddClipboardListener();
        
        var saved = storage.LoadAll();
        foreach (var e in saved)
            Items.Add(e);
    }

    public void SetClipboard(ClipboardEntry e)
    {
        Interlocked.Exchange(ref _ignoreNext, 1);
        Win32Clipboard.Write(e);
        
        Task.Delay(100).ContinueWith(async _ =>
        {
            await WinApi.SendCtrlV();
        });
    }

    public void Clear()
    {
        Items.Clear();
        storage.Clear();
    }

    public Task AddItem(object item)
    {
        if (item is ClipboardEntry entry)
        {
            Items.Insert(0, entry);
            storage.Append(entry);
        }
        return Task.CompletedTask;
    }

    public void Filter(string? query)
    {
        _currentFilter = query;
        Items.Clear();

        var all = storage.LoadAll();
        if (!string.IsNullOrWhiteSpace(query))
            all = all.Where(e => e.Content.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();

        foreach (var e in all)
            Items.Add(e);
    }

    public void Remove(ClipboardEntry entry)
    {
        if (Items.Remove(entry))
        {
            storage.Remove(entry);
        }
    }

    public void Pin(ClipboardEntry entry)
    {
        entry.IsPinned = !entry.IsPinned;

        var sorted = Items
            .OrderByDescending(e => e.IsPinned)
            .ThenByDescending(e => e.CreatedAt)
            .ToList();

        Items.Clear();
        foreach (var e in sorted)
            Items.Add(e);

        storage.Update(entry);
    }


    private void HandleMessage(uint msg)
    {
        if (msg != WinApi.WmClipboardUpdate) return;

        if (Interlocked.Exchange(ref _ignoreNext, 0) == 1)
            return;

        var entry = Win32Clipboard.Read();
        if (entry == null) return;

        if (passwords.IsProbablyPassword(entry.Content))
        {
            entry.Kind = ClipboardEntryKindEnum.Password;
        }
        
        AddItem(entry);
    }
}