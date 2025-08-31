using System.Collections.ObjectModel;
using System.Threading.Tasks;
using clipboard.Models;

namespace clipboard.Interfaces;

public interface IClipboardHistoryService
{
    ObservableCollection<ClipboardEntry> Items { get; }
    void Start();
    void SetClipboard(ClipboardEntry e);
    void Clear();
    Task AddItem(object item);
    void Filter(string? query);
    void Remove(ClipboardEntry entry);
    void Pin(ClipboardEntry entry);

}