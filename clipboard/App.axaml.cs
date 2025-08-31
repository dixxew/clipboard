using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using clipboard.Interfaces;
using clipboard.Models;
using clipboard.Services;
using clipboard.UserControls;
using clipboard.ViewModels;
using clipboard.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace clipboard;

public partial class App : Application
{
    private static IHost? _host;
    private static IServiceProvider? _sp;

    public App()
    {
    }

    public override void OnFrameworkInitializationCompleted()
    {
        _host = Program.BuildHost(Array.Empty<string>());
        _sp = _host.Services;

        var presentation = _sp.GetRequiredService<PresentationService>();
        presentation.Register<SettingsViewModel, SettingsControl>();
        
        var vm = _sp.GetRequiredService<MainViewModel>();
        var win = new MainWindow { DataContext = vm };

        _sp.GetRequiredService<IWindowService>().RegisterWindow(win);

        EventHandler? openedHandler = null;
        openedHandler = (sender, args) =>
        {
            _sp.GetRequiredService<IClipboardHistoryService>().Start();
            _sp.GetRequiredService<IHotkeyService>().Register(new Hotkey("Alt+V"));
            win.Opened -= openedHandler!;
        };
        win.Opened += openedHandler;

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
            desktopLifetime.MainWindow = win;

        base.OnFrameworkInitializationCompleted();
    }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}