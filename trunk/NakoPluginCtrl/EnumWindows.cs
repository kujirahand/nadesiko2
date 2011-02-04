﻿using System;
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
        [DllImport("user32", EntryPoint = "EnumWindows")]
        public extern static int _EnumWindows(EnumWindowsCallback lpEnumFunc, int lParam);
        [DllImport("user32", EntryPoint = "IsWindowVisible")]
        public extern static int IsWindowVisible(IntPtr hWnd);
        [DllImport("user32.dll",CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        public extern static IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        public extern static bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);
        
        private StringBuilder result;
        
        public String GetWindowTitle()
        {
            result = new StringBuilder();
            _EnumWindows(new EnumWindowsCallback(_EnumWindowsProc), 0);
            String s = result.ToString();
            if (s.Length > 0) {
                s = s.Substring(0, s.Length - 1); // chomp
            }
            return s;
        }
        
        public static String GetTitle()
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
        
        private String Keyword = "";
        private IntPtr FindWindowResult = IntPtr.Zero;
        
        private int _FindWindowProc(IntPtr hWnd, int lParam)
        {
            // Get title
            StringBuilder sb = new StringBuilder(0x1000);
            GetWindowText(hWnd, sb, sb.Capacity);
            String title = sb.ToString();
            
            // Check pattern
            if (Regex.IsMatch(title, Keyword)) {
                FindWindowResult = hWnd;
                return 0;
            } else {
                return 1;
            }
        }
        
        public IntPtr FindRE(String title)
        {
            FindWindowResult = IntPtr.Zero;
            Keyword = title;
            _EnumWindows(new EnumWindowsCallback(_FindWindowProc), 0);
            return FindWindowResult;
        }
        
        public static IntPtr FindWindowRE(String Title)
        {
            EnumWindows e = new EnumWindows();
            return e.FindRE(Title);
        }
        
        public static void ActivateWindow(String title)
        {
            IntPtr h = IntPtr.Zero;
            h = FindWindow(null, title);
            if (h == null) {
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
