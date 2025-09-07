using Avalonia;
using System;
using clipboard.Interfaces;
using clipboard.Services;
using clipboard.ViewModels;
using clipboard.Views;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Projektanker.Icons.Avalonia;
using Projektanker.Icons.Avalonia.FontAwesome;

namespace clipboard;

sealed class Program
{
    [STAThread]
    public static void Main(string[] args)
        => BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    

    public static AppBuilder BuildAvaloniaApp()
    {
        IconProvider.Current
            .Register<FontAwesomeIconProvider>();

        return AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
    }
    
     public static IHost BuildHost(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices(s =>
                {
                    // Services
                    s.AddSingleton<IClipboardHistoryService, ClipboardHistoryService>();
                    s.AddSingleton<IHotkeyService, HotkeyService>();
                    s.AddSingleton<IWindowService, WindowService>();
                    s.AddSingleton<IStorageService, JsonStorageService>();
                    s.AddSingleton<IMessageLoopWindow, MessageLoopWindow>();
                    s.AddSingleton<ISettingsService, SettingsService>();
                    s.AddSingleton<IPasswordService, PasswordService>();
                    s.AddSingleton<PresentationService>();
                    s.AddSingleton<ISettingsService, SettingsService>();
                    // ViewModels
                    s.AddSingleton<MainViewModel>();
                    s.AddSingleton<SettingsViewModel>();
                    
                    // Views
                    s.AddSingleton<MainWindow>();
                })
                .Build();
        }
     
}