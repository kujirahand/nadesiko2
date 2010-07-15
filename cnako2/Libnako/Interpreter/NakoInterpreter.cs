using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Libnako.Interpreter
{
    public class NakoInterpreter
    {
        /// <summary>
        /// Singleton method
        /// </summary>
        public static NakoInterpreter Instance
        {
            get {
                if (_instance == null) { _instance = new NakoInterpreter(); }
                return _instance;
            }

        }
        private static NakoInterpreter _instance;

        public Boolean Run()
        {
            return false;
        }
    }
}
