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
            OperatingSystem os = Environment.OSVersion;
            switch (os.Platform)
            {
                case PlatformID.Win32Windows:
                    if (os.Version.Major >= 4)
                    {
                        switch (os.Version.Major)
                        {
                            case 0: return "Windows 95";
                            case 10: return "Windows 98";
                            case 90: return "Windows Me";
                        }
                    }
                    break;
                case PlatformID.Win32NT:
                    switch (os.Version.Major)
                    {
                        case 3:
                            switch (os.Version.Minor)
                            {
                                case 0: return "Windows NT 3";
                                case 1: return "Windows NT 3.1";
                                case 5: return "Windows NT 3.5";
                                case 51: return "Windows NT 3.51";
                            }
                            break;
                        case 4: return "Windows NT 4.0";
                        case 5:
                            switch (os.Version.Minor)
                            {
                                case 0: return "Windows 2000";
                                case 1: return "Windows XP";
                                case 2: return "Windows Server 2003";
                            }
                            break;
                        case 6:
                            switch (os.Version.Minor)
                            {
                                case 0: return "Windows Vista";
                                case 1: return "Windows 7";
                            }
                            break;
                    }
                    break;
                case PlatformID.Unix: return "Unix";
                case PlatformID.Xbox: return "Xbox";
                case PlatformID.MacOSX: return "Mac OS X";
                case PlatformID.WinCE: return "Windows CE";

            }
            return os.VersionString;
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
