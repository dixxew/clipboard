using clipboard.Models;

namespace clipboard.Interfaces;

public interface ISettingsService
{
    AppSettings Settings { get; }
    void Save();
    AppSettings Load();
}