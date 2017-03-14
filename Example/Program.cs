using System;
using System.Runtime.InteropServices;
using QiDiTu.Wrapper.MinHook;

namespace QiDiTu.Example.MinHook
{
    class Example
    {
        static void Main(string[] args)
        {
            // Initialize MinHook.
            MinHookHelper.initialize();

            // Create a hook for MessageBoxW, in disabled state.
            MinHookHelper.createHookApi("user32", "MessageBoxW",
                                new Callback(DetourMessageBoxW), out fpMessageBoxW);


            // Enable the hook for MessageBoxW.
            MinHookHelper.enableHook("user32", "MessageBoxW");

            const int MB_OK = 0;

            // Expected to tell "Hooked!".
            MessageBox(IntPtr.Zero, "Not hooked...", "MinHook Sample", MB_OK);

            // Disable the hook for MessageBoxW.
            MinHookHelper.disableHook("user32", "MessageBoxW");

            // Expected to tell "Not hooked...".
            MessageBox(IntPtr.Zero, "Not hooked...", "MinHook Sample", MB_OK);

            // Uninitialize MinHook.
            MinHookHelper.uninitialize();
        }

        public static Callback fpMessageBoxW;

        public static int DetourMessageBoxW(IntPtr hWnd, string text, string caption, int options)
        {
            return fpMessageBoxW(hWnd, "Hooked!", caption, options);
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public delegate int Callback(IntPtr hWnd, string text, string caption, int options);

        [DllImport("user32.dll", EntryPoint = "MessageBoxW", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern int MessageBox(IntPtr hWnd, string text, string caption, int options);
    }
}
