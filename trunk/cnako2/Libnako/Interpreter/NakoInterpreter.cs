using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Libnako.Parser;

namespace Libnako.Interpreter
{
    delegate Object CalcMethodType(Object a, Object b);

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

        protected Stack<Object> stack;


        protected void Init()
        {
            stack = new Stack<Object>();
        }

        public Boolean Run()
        {
            return false;
        }

        public Boolean Run_NakoIL(NakoILCodeList list)
        {
            Init();
            for (int i = 0; i < list.Count; i++)
            {
                ExecNakoIL(list[i]);
            }
            return true;
        }

        protected void ExecNakoIL(NakoILCode code)
        {
            switch (code.type)
            {
                case NakoILType.NOP:
                    /* do nothing */
                    break;
                case NakoILType.LD_CONST_INT: stack.Push(code.value); break;
                case NakoILType.LD_CONST_REAL: stack.Push(code.value); break;
                case NakoILType.LD_CONST_STR: stack.Push(code.value); break;
                case NakoILType.LD_GLOBAL:
                case NakoILType.LD_LOCAL:
                case NakoILType.ST_GLOBAL:
                case NakoILType.ST_LOCAL:
                case NakoILType.NEW_ARR:
                case NakoILType.ST_ARR_ELEM:
                case NakoILType.LD_ARR_ELEM:
                    break;
                case NakoILType.ADD: exec_calc(calc_method_add); break;
                case NakoILType.SUB: exec_calc(calc_method_sub); break;
                case NakoILType.MUL: exec_calc(calc_method_mul); break;
                case NakoILType.DIV: exec_calc(calc_method_div); break;
                case NakoILType.MOD: exec_calc(calc_method_mod); break;
                case NakoILType.POWER: exec_calc(calc_method_power); break;
                case NakoILType.EQ:
                case NakoILType.NOT_EQ:
                case NakoILType.GT:
                case NakoILType.GT_EQ:
                case NakoILType.LT:
                case NakoILType.LT_EQ:
                case NakoILType.INC:
                case NakoILType.DEC:
                case NakoILType.NEG:
                case NakoILType.AND:
                case NakoILType.OR:
                case NakoILType.XOR:
                case NakoILType.NOT:
                case NakoILType.JUMP:
                case NakoILType.CALL:
                case NakoILType.RET:
                    break;
            }
        }

        private void exec_calc(CalcMethodType f)
        {
            Object a = stack.Pop();
            Object b = stack.Pop();
            stack.Push(f(a, b));
        }

        private Object calc_method_add(Object a, Object b)
        {
            return (Object)((float)a + (float)b);
        }
        private Object calc_method_sub(Object a, Object b)
        {
            return (Object)((float)a - (float)b);
        }
        private Object calc_method_mul(Object a, Object b)
        {
            return (Object)((float)a * (float)b);
        }
        private Object calc_method_div(Object a, Object b)
        {
            return (Object)((float)a / (float)b);
        }
        private Object calc_method_mod(Object a, Object b)
        {
            return (Object)((int)a % (int)b);
        }
        private Object calc_method_power(Object a, Object b)
        {
            return Math.Pow((float)a, (float)b);
        }

    }
}
