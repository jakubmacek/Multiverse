using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Multiverse.SimulationSandbox
{
    public static class ConsoleWindow
    {
        [DllImport("user32.dll")]
        private static extern bool ShowWindow(System.IntPtr hWnd, int cmdShow);

        public static void Maximize()
        {
            Process p = Process.GetCurrentProcess();
            ShowWindow(p.MainWindowHandle, 3); //SW_MAXIMIZE = 3
        }
    }
}
