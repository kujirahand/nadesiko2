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
            System.Text.StringBuilder o = new System.Text.StringBuilder ();
            Process proc = new System.Diagnostics.Process ();
            proc.StartInfo = oInfo;
            proc.OutputDataReceived += new DataReceivedEventHandler (delegate(object sender, DataReceivedEventArgs args) {
                o.AppendLine (args.Data);
            });
            proc.Start ();
            proc.BeginOutputReadLine ();
            proc.WaitForExit ();
            proc.Close();
            proc.Dispose();
            System.GC.Collect();
            System.GC.WaitForPendingFinalizers();
            string output = o.ToString ();//proc.StandardOutput.ReadToEnd();
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
		proc.WaitForExit ();
		string output = proc.StandardOutput.ReadToEnd();
        proc.Close();
		return output.Replace("\r\r\n","\n");
			
		}
	}
}

