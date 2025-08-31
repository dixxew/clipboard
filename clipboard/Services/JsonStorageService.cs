using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using clipboard.Interfaces;
using clipboard.Models;

namespace clipboard.Services;

public sealed class JsonStorageService : IStorageService
{
    private readonly string _path = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "clipboard", "history.json");

    public void Append(ClipboardEntry e)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_path)!);
        var list = LoadAll().ToList();
        list.Insert(0, e);
        File.WriteAllText(_path, JsonSerializer.Serialize(list.Take(200)));
    }

    public IEnumerable<ClipboardEntry> LoadAll()
        => File.Exists(_path)
            ? JsonSerializer.Deserialize<List<ClipboardEntry>>(File.ReadAllText(_path)) ?? Enumerable.Empty<ClipboardEntry>()
            : [];
    public void Clear()
    {
        if (File.Exists(_path))
            File.Delete(_path);
    }

    public void Remove(ClipboardEntry entry)
    {
        if (!File.Exists(_path))
            return;

        var list = LoadAll().ToList();
        list.RemoveAll(e => e.CreatedAt == entry.CreatedAt && e.Content == entry.Content);
        File.WriteAllText(_path, JsonSerializer.Serialize(list));
    }

    public void Update(ClipboardEntry entry)
    {
        if (!File.Exists(_path))
            return;

        var list = LoadAll().ToList();
        var index = list.FindIndex(e => e.CreatedAt == entry.CreatedAt);
        if (index != -1)
        {
            list[index] = entry;
            File.WriteAllText(_path, JsonSerializer.Serialize(list));
        }
    }

}