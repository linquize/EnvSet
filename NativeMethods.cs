using System;
using System.Runtime.InteropServices;

namespace EnvSet
{
    static class NativeMethods
    {
        public const int WM_SETTINGCHANGE = 0x1a;


        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SendMessageTimeout(IntPtr hWnd, int Msg, IntPtr wParam, string lParam, uint fuFlags, uint uTimeout, IntPtr lpdwResult);
    }
}
