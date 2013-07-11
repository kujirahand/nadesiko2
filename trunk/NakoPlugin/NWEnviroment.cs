using System;
using System.IO;

namespace NakoPlugin
{
    /// <summary>
    /// OSや各種環境を判別するクラス
    /// </summary>
    public class NWEnviroment
    {
        /// <summary>
        /// OSのバージョンを文字列として返す
        /// </summary>
        /// <returns>OS名</returns>
        public static string osVersionStr()
        {
            string res = "";
            System.OperatingSystem os = System.Environment.OSVersion;
            switch (os.Platform)
            {
                case System.PlatformID.Win32Windows:
                    if (os.Version.Major >= 4)
                    {
                        switch (os.Version.Major)
                        {
                            case 0: res = "Windows 95"; break;
                            case 10: res = "Windows 98"; break;
                            case 90: res = "Windows Me"; break;
                        }
                    }
                    break;
                case System.PlatformID.Win32NT:
                    switch (os.Version.Major)
                    {
                        case 3:
                            switch (os.Version.Minor)
                            {
                                case 0: res = "Windows NT 3"; break;
                                case 1: res = "Windows NT 3.1"; break;
                                case 5: res = "Windows NT 3.5"; break;
                                case 51: res = "Windows NT 3.51"; break;
                            }
                            break;
                        case 4: res = "Windows NT 4.0"; break;
                        case 5:
                            switch (os.Version.Minor)
                            {
                                case 0: res = "Windows 2000"; break;
                                case 1: res = "Windows XP"; break;
                                case 2: res = "Windows Server 2003"; break;
                            }
                            break;
                        case 6:
                            switch (os.Version.Minor)
                            {
                                case 0: res = "Windows Vista"; break;
                                case 1: res = "Windows 7"; break;
                            }
                            break;
                    }
                    break;
                case System.PlatformID.Unix:
                    res = "Unix"; break;
                case System.PlatformID.Xbox:
                    res = "Xbox"; break;
                case System.PlatformID.MacOSX:
                    res = "Mac OS X"; break;
                case System.PlatformID.WinCE:
                    res = "Windows CE"; break;

            }
            if (res == "")
            {
                res = os.VersionString;
            }
            return res;
        }
        /// <summary>
        /// 実行ファイルのパス
        /// </summary>
        public static string ExePath
        {
            get
            {
                return System.Reflection.Assembly.GetExecutingAssembly().Location;
            }
        }
        /// <summary>
        /// 実行ファイルのあるフォルダのパス
        /// </summary>
        public static string AppPath
        {
            get
            {
                string path = System.IO.Path.GetDirectoryName(ExePath);
                return AppendLastPathFlag(path);
            }
        }
        
        /// <summary>
        /// パスの最後に区切り記号(\)を追加する
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static string AppendLastPathFlag(string dir)
        {
        	if (dir.EndsWith(Path.DirectorySeparatorChar.ToString()))
        	{
        		return dir;
        	}
        	return dir + Path.DirectorySeparatorChar;
        }
    }
}
