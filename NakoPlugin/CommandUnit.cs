using System;
using System.Diagnostics;

namespace NakoPlugin
{
    /// <summary>
    /// Linux Command
    /// </summary>
	public class LinuxCommand
	{
        /// <summary>
        /// LinuxCommand Initializer
        /// </summary>
		public LinuxCommand ()
		{
		}
        /// <summary>
        /// execute
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
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
    /// <summary>
    /// WindowsCommand
    /// </summary>
	public class WindowsCommand
	{
        /// <summary>
        /// Initializer
        /// </summary>
		public WindowsCommand ()
		{
		}
        /// <summary>
        /// execute
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
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

