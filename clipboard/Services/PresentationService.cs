using System;
using System.Collections.Generic;
using Avalonia.Controls;
using clipboard.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;

namespace clipboard.Services;

public class PresentationService(IServiceProvider services) : ObservableObject
{
    private readonly Dictionary<Type, Type> _viewModelToView = new();

    private bool _isOverlayOpen;
    public bool IsOverlayOpen
    {
        get => _isOverlayOpen;
        private set => SetProperty(ref _isOverlayOpen, value);
    }

    private UserControl? _currentOverlay;
    public UserControl? CurrentOverlay
    {
        get => _currentOverlay;
        private set => SetProperty(ref _currentOverlay, value);
    }

    public void Register<TViewModel, TView>()
    {
        _viewModelToView[typeof(TViewModel)] = typeof(TView);
    }

    private UserControl CreateOverlay<TViewModel>() where TViewModel : class
    {
        if (!_viewModelToView.TryGetValue(typeof(TViewModel), out var viewType))
            throw new InvalidOperationException($"View for {typeof(TViewModel).Name} is not registered");

        var vm = services.GetRequiredService<TViewModel>();

        if (Activator.CreateInstance(viewType) is not UserControl control)
            throw new InvalidOperationException($"{viewType.Name} is not a UserControl");

        control.DataContext = vm;
        return control;
    }

    private void Open<TViewModel>() where TViewModel : class
    {
        CurrentOverlay = CreateOverlay<TViewModel>();
        IsOverlayOpen = true;
    }

    public void CloseOverlay()
    {
        CurrentOverlay = null;
        IsOverlayOpen = false;
    }

    // Публичные методы
    public void OpenSettings() => Open<SettingsViewModel>();
}