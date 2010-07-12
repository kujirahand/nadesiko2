using System;
using System.Collections.Generic;
using System.Text;

namespace Libnako.Parser
{
    public class NakoJosi : List<String>
    {
        private static NakoJosi instance = null;
        public static NakoJosi GetInstance()
        {
            if (instance == null)
            {
                instance = new NakoJosi();
            }
            return instance;
        }

        private NakoJosi()
        {
            Init();
        }

        protected void Init()
        {
            Add("から");
            Add("まで");
            Add("とは");
            Add("は");
            Add("の");
            Add("が");
            Add("を");
            Add("に");
            Add("へ");
            //
            SortAsLength();
        }

        protected void SortAsLength()
        {
            this.Sort(
                delegate (String a, String b) {
                    if (a.Length == b.Length)
                    {
                        return String.Compare(a, b);
                    }
                    return b.Length - a.Length;
                }
            );
        }

    }
}
