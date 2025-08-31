using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using clipboard.Interfaces;
using clipboard.Interop;

namespace clipboard.Services;

public sealed class MessageLoopWindow : IMessageLoopWindow
{
    private Thread? _thread;
    private IntPtr _hwnd;
    private Action<uint>? _cb;
    private readonly ManualResetEventSlim _ready = new(false);
    private WinApi.WndProc? _wndProc;
    private readonly ConcurrentQueue<(Func<bool> action, TaskCompletionSource<bool> tcs)> _queue = new();
   
    
    public void EnsureStarted(Action<uint> onMessage)
    {
        if (!OperatingSystem.IsWindows())
            throw new PlatformNotSupportedException("MessageLoopWindow only works on Windows");
        
        _cb += onMessage;
        if (_thread != null) { _ready.Wait(); return; }

        _thread = new Thread(MessageThread) { IsBackground = true };
        _thread.SetApartmentState(ApartmentState.STA);
        _thread.Start();
        _ready.Wait();
    }


    // эти методы теперь через InvokeOnWndThread
    public void AddClipboardListener()
    {
        _ready.Wait();
        InvokeOnWndThread(() =>
        {
            if (!WinApi.AddClipboardFormatListener(_hwnd))
                throw new Win32Exception(Marshal.GetLastWin32Error(), "AddClipboardFormatListener failed");
            return true;
        });
    }

    public void RegisterHotKey(uint modifiers, uint vk)
    {
        _ready.Wait();
        InvokeOnWndThread(() =>
        {
            if (!WinApi.RegisterHotKey(_hwnd, 1, modifiers, vk))
                throw new Win32Exception(Marshal.GetLastWin32Error(), "RegisterHotKey failed");
            return true;
        });
    }
    
    
    private void InvokeOnWndThread(Func<bool> action)
    {
        var tcs = new TaskCompletionSource<bool>();
        _queue.Enqueue((action, tcs));
        if (!WinApi.PostMessage(_hwnd, WinApi.WmAppInvoke, IntPtr.Zero, IntPtr.Zero))
            throw new Win32Exception(Marshal.GetLastWin32Error(), "PostMessage failed");
    }

    private void MessageThread()
    {
        _hwnd = CreateMessageWindow();
        if (_hwnd == IntPtr.Zero) throw new Exception("Failed to create message window");
        _ready.Set();

        while (WinApi.GetMessage(out var msg, IntPtr.Zero, 0, 0))
        {
            WinApi.TranslateMessage(ref msg);
            WinApi.DispatchMessage(ref msg);
        }
    }

    private IntPtr CreateMessageWindow()
    {
        _wndProc = WndProcImpl;

        var cls = new WinApi.WndClassW()
        {
            lpszClassName = "MsgOnlyWnd_" + Guid.NewGuid().ToString("N"),
            lpfnWndProc = _wndProc,
            hInstance = WinApi.GetModuleHandle(null)
        };

        var atom = WinApi.RegisterClassW(ref cls);
        if (atom == 0)
            throw new Win32Exception(Marshal.GetLastWin32Error(), "RegisterClassW failed");

        // HWND_MESSAGE = (IntPtr)(-3)
        var hwnd = WinApi.CreateWindowExW(0, cls.lpszClassName, "",
            0, 0, 0, 0, 0, new IntPtr(-3), IntPtr.Zero, cls.hInstance, IntPtr.Zero);

        return hwnd;
    }

    // WndProcImpl — добавь обработку нашего WM_APP_INVOKE
    private IntPtr WndProcImpl(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
    {
        if (msg == WinApi.WmAppInvoke)
        {
            while (_queue.TryDequeue(out var item))
            {
                try { var ok = item.action(); item.tcs.SetResult(ok); }
                catch (Exception ex) { item.tcs.SetException(ex); }
            }
            return IntPtr.Zero;
        }

        if (msg == WinApi.WmClipboardUpdate || msg == WinApi.WmHotkey)
        {
            _cb?.Invoke(msg);
            return IntPtr.Zero;
        }

        return WinApi.DefWindowProcW(hWnd, msg, wParam, lParam);
    }
}