using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using NakoPlugin;
using System.Management;

namespace NakoPluginCtrl
{
    public class NakoPluginMemory : INakoPlugin
    {
        //--- プラグインの宣言 ---
        string _description = "メモリの情報を表示するプラグイン";
        Version _version = new Version(1, 0);
        //--- プラグイン共通の部分 ---
        public Version TargetNakoVersion { get { return new Version(2, 0); } }
        public bool Used { get; set; }
        public string Name { get { return this.GetType().FullName; } }
        public Version PluginVersion { get { return _version; } }
        public string Description { get { return _description; } }
        //--- 関数の定義 ---
        public void DefineFunction(INakoPluginBank bank)
        {
            bank.AddFunc("メモリ使用率取得", "", NakoVarType.Int, _usagePercentage, "メモリの使用率を取得して返す。", "めもりしようりつしゅとく");
            bank.AddFunc("メモリ空き容量取得", "", NakoVarType.Int, _available, "メモリの空き容量をMB単位で取得して返す。", "めもりあきようりょうしゅとく");
            bank.AddFunc("メモリ総容量取得", "", NakoVarType.Int, _total, "メモリの総容量をMB単位で取得して返す。", "めもりそうようりょうしゅとく");
        }
        // プラグインの初期化処理
        public void PluginInit(INakoInterpreter runner)
        {
        }
        // プラグインの終了処理
        public void PluginFin(INakoInterpreter runner)
        {
        }
        
        // Define Method
        public object _usagePercentage(INakoFuncCallInfo info)
        {
			return (__total()>0)? (int)(100 - (100*__available()/__total())) : -1;
        }

        public object _available(INakoFuncCallInfo info)
        {
            return __available();
        }

        public object _total(INakoFuncCallInfo info)
        {
            return __total();
        }

        private int __available(){//MB単位
            if (NWEnviroment.isWindows ()) {
                string mem = "Memory";
                string countMem = "Available Mbytes";
                System.Diagnostics.PerformanceCounter pcMem = new System.Diagnostics.PerformanceCounter (mem, countMem);
                float available = pcMem.NextValue ();
                pcMem.Close ();
                pcMem.Dispose ();
                return (int)available;
            }else{
                string free =LinuxCommand.execute("free -m");
                using(StringReader sr = new StringReader(free)){
                    string line = "";
                    while((line=sr.ReadLine())!=null){
                        if(line.Contains("-/+")){
                            string[] parts = Regex.Split(line,@"\s+");
                            int available = int.Parse(parts[parts.Length-1]);
                            sr.Close();
                            sr.Dispose();
                            return available;
//                            Console.WriteLine("rate:{0}",(int)(100*int.Parse(parts[2])/(int.Parse(parts[3])+int.Parse(parts[2]))));
                        }
                    }
                }
                free = LinuxCommand.execute ("top -l 1 -s 0");
                    using(StringReader sr = new StringReader (free)){
                    string line = "";
                    while((line=sr.ReadLine())!=null){
                        if(line.Contains("PhysMem")){
                            string [] parts = Regex.Split (line, @"\s+");
                            for (int i = 0; i < parts.Length; i++) {
                                if (parts [i].Contains ("unused")) {
                                    int available = int.Parse (parts [i-1].Replace ("M",""));
                                    sr.Close ();
                                    sr.Dispose ();
                                    return available;
                                }
                            }
                        }
                    }
                }
            }
            return 0;//TODO: Exception?
        }
        private int __total(){//MB単位
            if(NWEnviroment.isWindows()){
                float total=0;
                ManagementClass mc = new ManagementClass("Win32_OperatingSystem");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach(ManagementObject mo in moc){
                    total = (int.Parse (mo["TotalVisibleMemorySize"].ToString()) + int.Parse(mo["TotalVirtualMemorySize"].ToString()))/1000;
                }
                moc.Dispose();
                mc.Dispose();
                return (int)total;
            }else{
                string free =LinuxCommand.execute("free -m");
                using(StringReader sr = new StringReader(free)){
                    string line = "";
                    while((line=sr.ReadLine())!=null){
                        if(line.Contains("-/+")){
                            string[] parts = Regex.Split(line,@"\s+");
                            int total = int.Parse(parts[parts.Length-1])+int.Parse(parts[parts.Length-2]);
                            sr.Close();
                            sr.Dispose();
                            return total;
                        }
                    }
                }
                free = LinuxCommand.execute ("sysctl hw.memsize");
                    using(StringReader sr = new StringReader (free)){
                    string line = "";
                    while((line=sr.ReadLine())!=null){
                        if(line.Contains("hw.memsize")){
                            string [] parts = Regex.Split (line, @"\s+");
                            int total = int.Parse (parts [1].Remove(parts[1].Length-6));
                            sr.Close();
                            sr.Dispose();
                            return total;
                        }
                    }
                }

            }
            return 0;//TODO: Exception?
        }
    }
}
