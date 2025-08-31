using System.Collections.Generic;
using clipboard.Models;

namespace clipboard.Interfaces;

public interface IStorageService
{
    void Append(ClipboardEntry e);
    IEnumerable<ClipboardEntry> LoadAll();
    void Clear();
    void Remove(ClipboardEntry entry);
    void Update(ClipboardEntry entry);
}