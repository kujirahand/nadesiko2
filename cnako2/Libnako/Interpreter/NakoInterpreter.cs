using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Libnako.Parser;

namespace Libnako.Interpreter
{
    delegate Object CalcMethodType(Object a, Object b);

    /// <summary>
    /// なでしこの中間コード（NakoILCode）を実行するインタプリタ
    /// </summary>
    public class NakoInterpreter
    {
        protected Stack<Object> stack;
        protected NakoILCodeList list = null;
        protected NakoVariables globalVar;
        protected NakoVariables localVar;

        public String PrintLog;
        public Boolean UseConsoleOut = false;

        public NakoInterpreter(NakoILCodeList list = null)
        {
            this.list = list;
            Init();
        }

        public void Init()
        {
            stack = new Stack<Object>();
            localVar = new NakoVariables();
            globalVar = new NakoVariables();
            PrintLog = "";
        }

        public Boolean Run(NakoILCodeList list = null)
        {
            if (list != null)
            {
                Init();
                this.list = list;
            }
            for (int i = 0; i < this.list.Count; i++)
            {
                Run_NakoIL(this.list[i]);
            }
            return true;
        }

        public Object StackTop
        {
            get {
                if (stack.Count == 0) return null;
                return stack.Peek();
            }
        }

        protected void Run_NakoIL(NakoILCode code)
        {
            switch (code.type)
            {
                case NakoILType.NOP:
                    /* do nothing */
                    break;
                case NakoILType.LD_CONST_INT:   stack.Push(code.value); break;
                case NakoILType.LD_CONST_REAL:  stack.Push(code.value); break;
                case NakoILType.LD_CONST_STR:   stack.Push(code.value); break;
                case NakoILType.LD_GLOBAL:      ld_global((int)code.value); break;
                case NakoILType.LD_LOCAL:       ld_local((int)code.value); break;
                case NakoILType.ST_GLOBAL:      st_global((int)code.value); break;
                case NakoILType.ST_LOCAL:       st_local((int)code.value); break;
                case NakoILType.NEW_ARR:
                case NakoILType.ST_ARR_ELEM:
                case NakoILType.LD_ARR_ELEM:
                    break;
                case NakoILType.ADD:        exec_calc(calc_method_add); break;
                case NakoILType.SUB:        exec_calc(calc_method_sub); break;
                case NakoILType.MUL:        exec_calc(calc_method_mul); break;
                case NakoILType.DIV:        exec_calc(calc_method_div); break;
                case NakoILType.MOD:        exec_calc(calc_method_mod); break;
                case NakoILType.POWER:      exec_calc(calc_method_power); break;
                case NakoILType.ADD_STR:    exec_calc(calc_method_add_str); break;
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
                case NakoILType.PRINT: exec_print(); break;
            }
        }

        private void st_local(int no)
        {
            Object p = stack.Pop();
            localVar.SetValue(no, p);
        }

        private void st_global(int no)
        {
            Object p = stack.Pop();
            globalVar.SetValue(no, p);
        }

        private void ld_local(int no)
        {
            Object p = localVar.GetValue(no);
            stack.Push(p);
        }

        private void ld_global(int no)
        {
            Object p = globalVar.GetValue(no);
            stack.Push(p);
        }

        private void exec_print()
        {
            Object o = stack.Pop();
            String s = o.ToString();
            if (UseConsoleOut)
            {
                Console.Write(s);
            }
            PrintLog += s;
        }

        private void exec_calc(CalcMethodType f)
        {
            Object b = stack.Pop();
            Object a = stack.Pop();
            stack.Push(f(a, b));
        }

        private Double ToDouble(Object v)
        {
            return NakoValueConveter.ToDouble(v);
        }

        private Boolean IsBothInt(Object a, Object b)
        {
            Boolean r = (a.GetType() == typeof(Int32) && b.GetType() == typeof(Int32));
            return r;
        }

        private Object calc_method_add(Object a, Object b)
        {
            if (IsBothInt(a, b))
            {
                Int32 i = (Int32)a + (Int32)b;
                return (Object)i;
            }
            else
            {
                Double d = ToDouble(a) + ToDouble(b);
                return (Object)d;
            }
        }
        private Object calc_method_sub(Object a, Object b)
        {
            if (IsBothInt(a, b))
            {
                Int32 i = (Int32)a - (Int32)b;
                return (Object)i;
            }
            else
            {
                Double d = ToDouble(a) - ToDouble(b);
                return (Object)d;
            }
        }
        private Object calc_method_mul(Object a, Object b)
        {
            if (IsBothInt(a, b))
            {
                Int32 i = (Int32)a * (Int32)b;
                return (Object)i;
            }
            else
            {
                Double d = ToDouble(a) * ToDouble(b);
                return (Object)d;
            }
        }
        private Object calc_method_div(Object a, Object b)
        {
            // "1 ÷ 2" のような場合を想定して、割り算は常に実数にすることにした
            Double d = ToDouble(a) / ToDouble(b);
            return (Object)d;
        }
        private Object calc_method_mod(Object a, Object b)
        {
            Int32 i = (Int32)a % (Int32)b;
            return (Object)i;
        }
        private Object calc_method_power(Object a, Object b)
        {
            return (Object)
                Math.Pow(ToDouble(a), ToDouble(b));
        }
        private Object calc_method_add_str(Object a, Object b)
        {
            String sa = a.ToString();
            String sb = b.ToString();
            return sa + sb;
        }

    }
}
