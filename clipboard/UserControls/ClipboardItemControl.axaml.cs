using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using clipboard.Models;
using clipboard.ViewModels;

namespace clipboard.UserControls;

public partial class ClipboardItemControl : UserControl
{
    private Point _dragStart;
    private bool _dragStarted;

    public ClipboardItemControl()
    {
        InitializeComponent();
        
        
        // ловим ховер всего UserControl
        this.PointerEntered += (_, _) => OpenActions();
        this.PointerExited += (_, _) => CloseActions();
        
        AddHandler(PointerPressedEvent, OnPointerPressed, RoutingStrategies.Tunnel);
        AddHandler(PointerMovedEvent, OnPointerMoved, RoutingStrategies.Tunnel);
        AddHandler(PointerReleasedEvent, OnPointerReleased, RoutingStrategies.Tunnel);
    }
    private void OpenActions()
    {
        Actions.Width = 36;
        Actions.Opacity = 1;
        Actions.IsHitTestVisible = true;
        (Actions.RenderTransform as TranslateTransform)!.X = 0;
    }

    private void CloseActions()
    {
        Actions.Width = 0;
        Actions.Opacity = 0;
        Actions.IsHitTestVisible = false;
        (Actions.RenderTransform as TranslateTransform)!.X = 12;
    }
    
    private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        _dragStart = e.GetPosition(null);
        _dragStarted = false;
    }

    private async void OnPointerMoved(object? sender, PointerEventArgs e)
    {
        if (_dragStarted)
            return;

        var pos = e.GetPosition(null);
        if (Math.Abs(pos.X - _dragStart.X) > 5 || Math.Abs(pos.Y - _dragStart.Y) > 5)
        {
            _dragStarted = true;

            if (DataContext is ClipboardEntry entry)
            {
                var data = new DataObject();
                data.Set(DataFormats.Text, entry.Content);

                await DragDrop.DoDragDrop(e, data, DragDropEffects.Copy);
            }
        }
    }
    
    private void OnPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        if (_dragStarted)
            return;

        if (DataContext is ClipboardEntry entry &&
            (VisualRoot as Window)?.DataContext is MainViewModel vm &&
            vm.PasteCommand.CanExecute(entry))
        {
            vm.PasteCommand.Execute(entry);
        }
    }
}