using System.Collections.ObjectModel;
using System.Threading.Tasks;
using clipboard.Interfaces;
using clipboard.Models;
using clipboard.Services;
using CommunityToolkit.Mvvm.Input;

namespace clipboard.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly IClipboardHistoryService _clip;
    private readonly IWindowService _window;
    private readonly IHotkeyService _hotkeys;
    private readonly IPasswordService _passwords;
    private readonly PresentationService _presentation;

    public MainViewModel(
        IClipboardHistoryService clip,
        IWindowService window,
        IHotkeyService hotkeys,
        IPasswordService passwords, 
        PresentationService presentation)
    {
        _clip = clip;
        _window = window;
        _hotkeys = hotkeys;
        _passwords = passwords;
        _presentation = presentation;

        _hotkeys.Triggered += (_, _) => _window.ToggleVisibility();
        _presentation.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(PresentationService.IsOverlayOpen))
                OnPropertyChanged(nameof(IsOverlayOpen));
            else if (e.PropertyName == nameof(PresentationService.CurrentOverlay))
                OnPropertyChanged(nameof(OverlayContent));
        };
        
        PasteCommand = new RelayCommand<ClipboardEntry>(Paste);
    }

    private string? _query;


    public string? Query
    {
        get => _query;
        set
        {
            SetProperty(ref _query, value);
            ApplyFilter();
        }
    }
    public bool IsOverlayOpen => _presentation.IsOverlayOpen;
    public object? OverlayContent => _presentation.CurrentOverlay;
    public ObservableCollection<ClipboardEntry> Items => _clip.Items;
    public IRelayCommand<ClipboardEntry> PasteCommand { get; }
    

    
    private void ApplyFilter()
    {
        _clip.Filter(Query);
    }
    public Task GeneratePassword()
        => _clip.AddItem(_passwords.GeneratePassword());
    public void ClearItems() => _clip.Clear();
    public void OpenUserWeb()
        => _window.OpenWeb("https://my.software.site/profile");
    public void ShowSettings() => _presentation.OpenSettings();
    public void HideOverlay() => _presentation.CloseOverlay();
    
    public void HideWindow() => _window.Hide();
    
    public void PinItem(ClipboardEntry entry) => _clip.Pin(entry);
    public void DeleteItem(ClipboardEntry entry) => _clip.Remove(entry);
    
    private void Paste(ClipboardEntry? entry)
    {
        if (entry != null)
        {
            _window.Hide();
            _clip.SetClipboard(entry);
        }
    }
}