using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Libnako.Parser
{
    public class NakoRunner
    {
        // Singleton method
        private static NakoRunner instance = null;
        public static NakoRunner Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new NakoRunner();
                }
                return instance;
            }
        }

        // property


        // constructor
        public NakoRunner()
        {
        }

        public void Run()
        {
        }

    }
}
