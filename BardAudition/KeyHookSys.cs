using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BardAudition;

// Derived from "Hooky," from: https://stackoverflow.com/a/25071571
public static class KeyHookSys
{
    // ============================================
    #region KeyHooks
    // ============================================

    // ============================================
    // Delegate for the low-level keyboard hook.
    // ============================================

    private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

    // ============================================
    // DLL imports for the low-level keyboard hook.
    // ============================================

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int idHook,
        LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
        IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string lpModuleName);

    // ============================================
    // Constants
    // ============================================

    // <summary> Type of Hook - Low Level Keyboard </summary>
    private const int WH_KEYBOARD_LL = 13;

    // <summary> Value passed on KeyDown </summary>
    private const int WM_KEYDOWN = 0x0100;

    // <summary> Value passed on KeyUp </summary>
    private const int WM_KEYUP = 0x0101;

    // ============================================
    // State
    // ============================================

    // Bool to use as a flag for control key
    //private static bool CONTROL_DOWN = false;

    private static IntPtr _hookID = IntPtr.Zero;

    private static bool _initialized = false;

    // ============================================
    // Key Hook Callback
    // ============================================
    static KeyHookSys()
    {
        if (_initialized) { throw new Exception("Init must be called once"); }
        else { _initialized = true; }

        var proc = _proc;
        IntPtr? hookId = null;

        using (Process curProcess = Process.GetCurrentProcess())
        {
            var mainModule = curProcess.MainModule;
            if (mainModule == null) { throw new Exception($"mainModule was null"); }

            using (ProcessModule curModule = mainModule)
            {
                if (curModule.ModuleName == null) { throw new Exception($"ModuleName was null"); }
                hookId = SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        if (hookId == null) { throw new Exception($"Failed to init key hooks"); }
        _hookID = hookId.Value;
    }

    private static readonly LowLevelKeyboardProc _proc = KeyHook_Callback;
    private static IntPtr KeyHook_Callback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        // A Key was pressed down
        if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
        {
            // Get the keycode
            var vkCode = Marshal.ReadInt32(lParam);

            // Name of the key
            var formsKeyName = ((Keys)vkCode).ToString();

            // Set key state.
            //
            // Allows us to ignore represses from holding the button down.
            var wasAlreadyPressed = keyState.Contains(vkCode);
            keyState.Add(vkCode);

            if (!wasAlreadyPressed)
            {
                // Fire key down callback.
                if (keyDownCbs.TryGetValue(formsKeyName, out var cb))
                {
                    cb?.Invoke(null);
                }
            }

            //// Display the name of the key
            //Console.Write(theKey);

            //// If they pressed control
            //if (theKey.Contains("ControlKey"))
            //{
            //    // Flag control as down
            //    CONTROL_DOWN = true;
            //}

            //// If they held CTRL and pressed B
            //else if (CONTROL_DOWN && theKey == "B")
            //{
            //    // Our hotkey was pressed
            //    Console.WriteLine("\n***HOTKEY PRESSED***");
            //}

            //// If they press escape
            //else if (theKey == "Escape")
            //{
            //    // Release our hook
            //    UnhookWindowsHookEx(_hookID);
            //    // Exit our program
            //    Environment.Exit(0);
            //}
        }
        else if (nCode >= 0 && wParam == (IntPtr)WM_KEYUP) // Key Up
        {
            int vkCode = Marshal.ReadInt32(lParam);        // Get Keycode
            string theKey = ((Keys)vkCode).ToString();     // Get Key name

            //if (theKey.Contains("ControlKey"))             // If they let go of control
            //{
            //    CONTROL_DOWN = false;                      // Unflag control
            //}
            keyState.Remove(vkCode);
        }
        return CallNextHookEx(_hookID, nCode, wParam, lParam); //Call the next hook
    }

    public static HashSet<int> keyState = new HashSet<int>();
    public static Dictionary<string, Action<object?>> keyDownCbs = new Dictionary<string, Action<object?>>();
    public static Dictionary<string, Action<object?>> keyUpCbs = new Dictionary<string, Action<object?>>();

    public static void RegisterKeyDown(string winFormsKey, Action<object?> keyDownCb)
    {
        keyDownCbs[winFormsKey] = keyDownCb;

        Console.Write($"InputSys Registered '{winFormsKey}' down; ");
    }

    public static void RegisterKeyUp(string winFormsKey, Action<object?> keyUpCb)
    {
        keyUpCbs[winFormsKey] = keyUpCb;

        Console.Write($"InputSys Registered '{winFormsKey}' up; ");
    }

    #endregion
    // ============================================

}
