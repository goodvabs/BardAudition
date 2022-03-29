using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;

namespace BardAudition.Hotkeys;
public static class HotkeyManager
{
    public static event EventHandler<HotkeyEventArgs>? HotkeyPressed;

    public static int RegisterHotkey(Keys key, KeyModifiers modifiers)
    {
        _windowReadyEvent.WaitOne();
        if (_wnd == null) return 0;
        int id = System.Threading.Interlocked.Increment(ref _id);
        _wnd.Invoke(new RegisterHotkeyDelegate(RegisterHotkeyInternal), _hwnd, id, (uint)modifiers, (uint)key);
        return id;
    }

    public static void UnregisterHotkey(int id)
    {
        if (_wnd == null) return;
        _wnd.Invoke(new UnRegisterHotkeyDelegate(UnRegisterHotkeyInternal), _hwnd, id);
    }

    delegate void RegisterHotkeyDelegate(IntPtr hwnd, int id, uint modifiers, uint key);
    delegate void UnRegisterHotkeyDelegate(IntPtr hwnd, int id);

    private static void RegisterHotkeyInternal(IntPtr hwnd, int id, uint modifiers, uint key)
    {
        RegisterHotKey(hwnd, id, modifiers, key);
    }

    private static void UnRegisterHotkeyInternal(IntPtr hwnd, int id)
    {
        UnregisterHotKey(_hwnd, id);
    }

    private static void OnHotkeyPressed(HotkeyEventArgs e)
    {
        if (HotkeyManager.HotkeyPressed != null)
        {
            HotkeyManager.HotkeyPressed(null, e);
        }
    }
    private static volatile MessageWindow? _wnd;
    private static volatile IntPtr _hwnd;
    private static ManualResetEvent _windowReadyEvent = new ManualResetEvent(false);
    static HotkeyManager()
    {
        Thread messageLoop = new Thread(delegate ()
        {
            Application.Run(new MessageWindow());
        });
        messageLoop.Name = "MessageLoopThread";
        messageLoop.IsBackground = true;
        messageLoop.Start();
    }

    private class MessageWindow : Form
    {
        public MessageWindow()
        {
            _wnd = this;
            _hwnd = this.Handle;
            _windowReadyEvent.Set();
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_HOTKEY)
            {
                HotkeyEventArgs e = new HotkeyEventArgs(m.LParam);
                HotkeyManager.OnHotkeyPressed(e);
            }

            base.WndProc(ref m);
        }

        protected override void SetVisibleCore(bool value)
        {
            // Ensure the window never becomes visible
            base.SetVisibleCore(false);
        }

        private const int WM_HOTKEY = 0x312;
    }

    [DllImport("user32", SetLastError = true)]
    private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

    [DllImport("user32", SetLastError = true)]
    private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

    private static int _id = 0;
}


public class HotkeyEventArgs : EventArgs
{
    public readonly Keys Key;
    public readonly KeyModifiers Modifiers;

    public HotkeyEventArgs(Keys key, KeyModifiers modifiers)
    {
        this.Key = key;
        this.Modifiers = modifiers;
    }

    public HotkeyEventArgs(IntPtr hotKeyParam)
    {
        uint param = (uint)hotKeyParam.ToInt64();
        Key = (Keys)((param & 0xffff0000) >> 16);
        Modifiers = (KeyModifiers)(param & 0x0000ffff);
    }
}

[Flags]
public enum KeyModifiers
{
    None = 0,
    Alt = 1,
    Control = 2,
    Shift = 4,
    Windows = 8,
    NoRepeat = 0x4000
}
