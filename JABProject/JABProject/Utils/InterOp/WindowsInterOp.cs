using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsAccessBridgeInterop;
using WindowsAccessBridgeInterop.Win32;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using System.Runtime.Remoting.Contexts;
using static WindowsAccessBridgeInterop.Win32.NativeMethods;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using System.Threading;
using System.Runtime.InteropServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using JABProject.Utils;
using Domain.Entities;
using Domain.Commands;

namespace JABProject.Utils.InterOp
{
    static class WindowsInterOp
    {

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);

        public static AccessBridge jab;
        public static int vmId;

        public static List<JavaApplication> FindJavaWindow(params string[] processNames)
        {
            List<JavaApplication> applications = new List<JavaApplication>();

            EnumWindows((hWnd, lParam) =>
            {
                if (IsWindowVisible(hWnd))
                {
                    GetWindowThreadProcessId(hWnd, out uint processId);
                    Process process = Process.GetProcessById((int)processId);

                    foreach (String processName in processNames)
                        if (process.ProcessName.Equals(processName, StringComparison.OrdinalIgnoreCase))
                        {
                            StringBuilder windowTitle = new StringBuilder(256);
                            GetWindowText(hWnd, windowTitle, windowTitle.Capacity);
                            applications.Add(new JavaApplication(windowTitle.ToString(), hWnd));
                        }
                }
                return true;
            }, IntPtr.Zero);

            return applications;
        }

    }
}
