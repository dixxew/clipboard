using System.Collections.ObjectModel;
using System.Threading.Tasks;
using clipboard.Interfaces;
using clipboard.Models;
using clipboard.Services;
using CommunityToolkit.Mvvm.Input;

namespace clipboard.ViewModels;

public class SettingsViewModel(ISettingsService settingsService) : ViewModelBase
{
    public AppSettings Settings => settingsService.Settings;


    public void Save()
    {
        settingsService.Save();
    }
}