using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace NakoPluginCtrl
{
    /// <summary>
    /// Enumurate Window List
    /// </summary>
    public class EnumWindows
    {
        public delegate int EnumWindowsCallback(IntPtr hWnd, int lParam);
        
        [DllImport("user32.dll")]
        public extern static IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        public extern static int GetWindowText(IntPtr hWnd, StringBuilder text, int length);
        [DllImport("user32.dll")]
        public extern static int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);
        [DllImport("user32.dll", EntryPoint = "EnumWindows")]
        public extern static int _EnumWindows(EnumWindowsCallback lpEnumFunc, int lParam);
        [DllImport("user32.dll", EntryPoint = "IsWindowVisible")]
        public extern static int IsWindowVisible(IntPtr hWnd);
        [DllImport("user32.dll",CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        public extern static IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        public extern static bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);
        
        private StringBuilder result;
        
        public string GetWindowTitle()
        {
            result = new StringBuilder();
            _EnumWindows(new EnumWindowsCallback(_EnumWindowsProc), 0);
            string s = result.ToString();
            if (s.Length > 0) {
                s = s.Substring(0, s.Length - 1); // chomp
            }
            return s;
        }
        
        public static string GetTitle()
        {
            EnumWindows e = new EnumWindows();
            return e.GetWindowTitle();
        }
        
        private int _EnumWindowsProc(IntPtr hWnd, int lParam)
        {
            // Window Handle
            result.Append(hWnd.ToString() + ",");
            // Title
            StringBuilder sb = new StringBuilder(0x1000);
            int len = GetWindowText(hWnd, sb, sb.Capacity);
            if (len > 0) {
                sb.Replace("\"", "\"\"");
                result.Append("\"" + sb.ToString() + "\",");
            } else {
                result.Append("\"\",");
            }
            // Class
            sb = new StringBuilder(0x1000);
            GetClassName(hWnd, sb, sb.Capacity);
            result.Append(sb);
            result.Append(",");
            // Visible
            bool b = (IsWindowVisible(hWnd) > 0);
            result.Append(b ? "true" : "false");
            result.Append("\n");
            return 1; // CONTINUE
        }
        
        private string Keyword = "";
        private IntPtr FindWindowResult = IntPtr.Zero;
        
        private int _FindWindowProc(IntPtr hWnd, int lParam)
        {
            // Get title
            StringBuilder sb = new StringBuilder(0x1000);
            GetWindowText(hWnd, sb, sb.Capacity);
            string title = sb.ToString();
            
            // Check pattern
            if (Regex.IsMatch(title, Keyword)) {
                FindWindowResult = hWnd;
                return 0;
            } else {
                return 1;
            }
        }
        
        public IntPtr FindRE(string title)
        {
            FindWindowResult = IntPtr.Zero;
            Keyword = title;
            _EnumWindows(new EnumWindowsCallback(_FindWindowProc), 0);
            return FindWindowResult;
        }
        
        public static IntPtr FindWindowRE(string Title)
        {
            EnumWindows e = new EnumWindows();
            return e.FindRE(Title);
        }
        
        public static void ActivateWindow(string title)
        {
            IntPtr h = IntPtr.Zero;
            h = FindWindow(null, title);
            if (h == (IntPtr)null) {
                h = EnumWindows.FindWindowRE(title);
            }
            if (h != IntPtr.Zero) {
                SetForegroundWindow(h);
            }
        }
        
        public EnumWindows()
        {
        }
    }
}
