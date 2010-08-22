using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Libnako.JCompiler
{
    public class NakoVariableNames : Dictionary<String, int>
    {
        /// <summary>
        /// 名前より変数を生成して変数番号を返す
        /// </summary>
        /// <param name="name"></param>
        /// <returns>変数番号</returns>
        public int createName(String name)
        {
            if (!this.ContainsKey(name))
            {
                int i = this.Count + 1;
                this[name] = i;
                return i;
            }
            return this[name];
        }

        /// <summary>
        /// 無名変数を生成して変数番号を返す
        /// </summary>
        /// <returns>変数番号</returns>
        public int createNameless()
        {
            int i = Count + 1;
            this["??___" + i] = i;
            return i;
        }
    }
}
