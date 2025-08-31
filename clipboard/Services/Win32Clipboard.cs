using System;
using System.Runtime.InteropServices;
using System.Text;
using clipboard.Enum;
using clipboard.Interop;
using clipboard.Models;

namespace clipboard.Services;

internal static class Win32Clipboard
{

    public static ClipboardEntry? Read()
    {
        if (!WinApi.OpenClipboard(IntPtr.Zero)) return null;
        try
        {
            if (!WinApi.IsClipboardFormatAvailable(WinApi.CfUnicodeText)) return null;

            var h = WinApi.GetClipboardData(WinApi.CfUnicodeText);
            if (h == IntPtr.Zero) return null;

            var p = WinApi.GlobalLock(h);
            if (p == IntPtr.Zero) return null;

            try
            {
                var text = Marshal.PtrToStringUni(p);
                if (string.IsNullOrEmpty(text)) return null;
                
                text = text.TrimEnd('\0', '\r', '\n');
                return new ClipboardEntry
                {
                    Content = text,
                    CreatedAt = DateTime.Now
                };
            }
            finally
            {
                WinApi.GlobalUnlock(h);
            }
        }
        finally
        {
            WinApi.CloseClipboard();
        }
    }

    public static void Write(ClipboardEntry e)
    {
        if (e.Kind != ClipboardEntryKindEnum.Plain) return;

        var bytes = Encoding.Unicode.GetBytes(e.Content + "\0");

        if (!WinApi.OpenClipboard(IntPtr.Zero)) return;
        try
        {
            WinApi.EmptyClipboard();

            var hMem = WinApi.GlobalAlloc(WinApi.GMemMoveable | WinApi.GMemZeroInit, (uint)bytes.Length);
            if (hMem == IntPtr.Zero) return;

            var ptr = WinApi.GlobalLock(hMem);
            if (ptr == IntPtr.Zero) return;

            try
            {
                Marshal.Copy(bytes, 0, ptr, bytes.Length);
            }
            finally
            {
                WinApi.GlobalUnlock(hMem);
            }

            // ВАЖНО: не освобождаем hMem — теперь им владеет буфер обмена
            WinApi.SetClipboardData(WinApi.CfUnicodeText, hMem);
        }
        finally
        {
            WinApi.CloseClipboard();
        }
    }
}