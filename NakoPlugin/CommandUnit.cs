using System;
using System.Diagnostics;

namespace NakoPlugin
{
	public class LinuxCommand
	{
		public LinuxCommand ()
		{
		}
		public static string execute(string cmd){
        ProcessStartInfo oInfo = new ProcessStartInfo();
        oInfo.UseShellExecute = false;
        oInfo.CreateNoWindow = true;
		oInfo.FileName = "sh";
		oInfo.Arguments = "-c '" + cmd + "'";

        oInfo.RedirectStandardOutput = true;
        oInfo.RedirectStandardError = true;

        Process proc = System.Diagnostics.Process.Start(oInfo);
		string output = proc.StandardOutput.ReadToEnd();
        proc.Close();
		return output.Replace("\r\r\n","\n");
			
		}
	}
	public class WindowsCommand
	{
		public WindowsCommand ()
		{
		}
		public static string execute(string cmd){
        ProcessStartInfo oInfo = new ProcessStartInfo();
        oInfo.UseShellExecute = false;
        oInfo.CreateNoWindow = true;
		oInfo.FileName = System.Environment.GetEnvironmentVariable("ComSpec");
		oInfo.Arguments = "/c " + cmd;

        oInfo.RedirectStandardOutput = true;
        oInfo.RedirectStandardError = true;

        Process proc = System.Diagnostics.Process.Start(oInfo);
		string output = proc.StandardOutput.ReadToEnd();
        proc.Close();
		return output.Replace("\r\r\n","\n");
			
		}
	}
}

