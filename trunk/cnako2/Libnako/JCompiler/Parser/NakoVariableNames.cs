using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Libnako.JCompiler.Parser
{
    public class NakoVarialbeNamesValue
    {
        public int no;
        public NakoVariableType type = NakoVariableType.Object;
    }

    public class NakoVariableNames : Dictionary<String, NakoVarialbeNamesValue>
    {
        /// <summary>
        /// 名前より変数を生成して変数番号を返す
        /// </summary>
        /// <param name="name"></param>
        /// <returns>変数番号</returns>
        public NakoVarialbeNamesValue createName(String name)
        {
            if (!this.ContainsKey(name))
            {
                NakoVarialbeNamesValue v = new NakoVarialbeNamesValue();
                int i = this.Count + 1;
                this[name] = v;
                v.no = i;
                return v;
            }
            return this[name];
        }
        
        public int createNameGetNo(String name, NakoVariableType type = NakoVariableType.Object)
        {
            NakoVarialbeNamesValue v = createName(name);
            v.type = type;
            return v.no;
        }

        /// <summary>
        /// 無名変数を生成して変数番号を返す
        /// </summary>
        /// <returns>変数番号</returns>
        public NakoVarialbeNamesValue createNameless()
        {
            int i = Count + 1;
            NakoVarialbeNamesValue v = new NakoVarialbeNamesValue();
            v.no = i;
            this["?_" + i] = v;
            return v;
        }

        public int createNamelessGetNo(NakoVariableType type = NakoVariableType.Object)
        {
            NakoVarialbeNamesValue v = createNameless();
            v.type = type;
            return v.no;
        }
    }
}
