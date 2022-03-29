using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace HookKeys;

// Derived from "Hooky," from: https://stackoverflow.com/a/25071571
public class HookKey
{

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

    // =============================================

    // =============================================

    // Bool to use as a flag for control key
    private static bool CONTROL_DOWN = false;

    private static IntPtr _hookID = IntPtr.Zero;

    public static void HookKeyMain()
    {
        // Set our hook
        _hookID = SetHook(_proc);

        // Start a standard application method loop
        Application.Run();
    }

    private static IntPtr SetHook(LowLevelKeyboardProc proc)
    {
        //using (Process curProcess = Process.GetCurrentProcess())
        //using (ProcessModule curModule = curProcess.MainModule)
        //{
        //    return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
        //        GetModuleHandle(curModule.ModuleName), 0);
        //}
        return IntPtr.Zero;
    }

    // =============================================

    // The function called when a key is pressed
    private static LowLevelKeyboardProc _proc = HookCallback;

    private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        // A Key was pressed down
        if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
        {
            // Get the keycode
            int vkCode = Marshal.ReadInt32(lParam);
            // Name of the key
            string theKey = ((Keys)vkCode).ToString();
            // Display the name of the key
            Console.Write(theKey);
            // If they pressed control
            if (theKey.Contains("ControlKey"))
            {
                // Flag control as down
                CONTROL_DOWN = true;                          
            }
            // If they held CTRL and pressed B
            else if (CONTROL_DOWN && theKey == "B")
            {
                // Our hotkey was pressed
                Console.WriteLine("\n***HOTKEY PRESSED***");
            }

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
            if (theKey.Contains("ControlKey"))             // If they let go of control
            {
                CONTROL_DOWN = false;                      // Unflag control
            }
        }
        return CallNextHookEx(_hookID, nCode, wParam, lParam); //Call the next hook
    }

    // =============================================

    // =============================================

    // =============================================

}
