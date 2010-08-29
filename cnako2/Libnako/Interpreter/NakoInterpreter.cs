﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Libnako.JCompiler.ILWriter;
using Libnako.JCompiler;
using Libnako.NakoAPI;
using Libnako.JCompiler.Function;

namespace Libnako.Interpreter
{
    delegate Object CalcMethodType(Object a, Object b);

    /// <summary>
    /// なでしこの中間コード（NakoILCode）を実行するインタプリタ
    /// </summary>
    public class NakoInterpreter
    {
        /// <summary>
        /// 計算用のスタック
        /// </summary>
        protected Stack<Object> stack;
        /// <summary>
        /// 仮想バイトコードの一覧
        /// </summary>
        protected NakoILCodeList list = null;
        /// <summary>
        /// グローバル変数
        /// </summary>
        protected NakoVariables globalVar;
        /// <summary>
        /// ローカル変数
        /// </summary>
        protected NakoVariables localVar;
        /// <summary>
        /// ユーザー関数の呼び出し履歴
        /// </summary>
        protected Stack<NakoCallStack> callStack;
        /// <summary>
        /// 現在実行しているリスト中の位置
        /// </summary>
        protected int runpos = 0;
        /// <summary>
        /// 自動的に runpos を進めるかどうか
        /// </summary>
        protected Boolean autoIncPos = true;
        /// <summary>
        /// デバッグ用のログ記録用変数
        /// </summary>
        public String PrintLog { get; set; }
		public Boolean UseConsoleOut { get; set; }
		public Boolean debugMode { get; set; }

        public NakoInterpreter(NakoILCodeList list = null)
        {
			this.UseConsoleOut = false;
			this.debugMode = false;

			this.list = list;
            Reset();
        }

        /// <summary>
        /// 環境のリセット
        /// </summary>
        public void Reset()
        {
            stack = new Stack<Object>();
            globalVar = NakoVariables.Globals;
            localVar = NakoVariables.Locals = new NakoVariables(NakoVariableScope.Local);
            callStack = new Stack<NakoCallStack>();
            PrintLog = "";
        }

        /// <summary>
        /// ILコードを実行する
        /// </summary>
        /// <param name="list">実行するILコードリスト</param>
        /// <returns>実行が成功したかどうか</returns>
        public Boolean Run(NakoILCodeList list = null)
        {
            if (list != null)
            {
                Reset();
                this.list = list;
            }
            runpos = 0;
            return _run();
        }

        protected Boolean _run()
        {
            while (runpos < list.Count)
            {
                NakoILCode code = this.list[runpos];
                Run_NakoIL(code);
                if (autoIncPos)
                {
                    runpos++;
                }
                else
                {
                    autoIncPos = true;
                }
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

        public Object StackPop()
        {
            return stack.Pop();
        }

        public void StackPush(Object v)
        {
            stack.Push(v);
        }

        protected void Run_NakoIL(NakoILCode code)
        {
            if (debugMode)
            {
                int i = runpos;
                string s = "";
                s += String.Format("{0,4:X4}:", i);
                s += String.Format("{0,-14}", code.type.ToString());
                if (code.value != null)
                {
                    if (code.value is int)
                    {
                        s += String.Format("({0,4:X4})", (int)code.value);
                    }
                    else
                    {
                        s += "(" + code.value.ToString() + ")";
                    }
                }
                Console.WriteLine(s);
            }

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
                case NakoILType.LD_GLOBAL_REF:  ld_global_ref((int)code.value); break;
                case NakoILType.LD_LOCAL_REF:   ld_local_ref((int)code.value); break;
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
                case NakoILType.EQ:         exec_calc(calc_method_eq); break;
                case NakoILType.NOT_EQ:     exec_calc(calc_method_not_eq); break;
                case NakoILType.GT:         exec_calc(calc_method_gt); break;
                case NakoILType.GT_EQ:      exec_calc(calc_method_gteq); break;
                case NakoILType.LT:         exec_calc(calc_method_lt); break;
                case NakoILType.LT_EQ:      exec_calc(calc_method_lteq); break;
                case NakoILType.INC:        _inc(); break;
                case NakoILType.DEC:        _dec(); break;
                case NakoILType.NEG:        _neg(); break;
                case NakoILType.AND:        exec_calc(calc_method_and); break;
                case NakoILType.OR:         exec_calc(calc_method_or); break;
                case NakoILType.XOR:        exec_calc(calc_method_xor); break;
                case NakoILType.NOT:        _not(); break;
                case NakoILType.JUMP:           _jump(code); break;
                case NakoILType.BRANCH_TRUE:    _branch_true(code); break;
                case NakoILType.BRANCH_FALSE:   _branch_false(code); break;
                case NakoILType.SYSCALL:        exec_syscall(code); break;
                case NakoILType.USRCALL:        exec_usrcall(code); break;
                case NakoILType.RET:            exec_ret(code); break;
                case NakoILType.PRINT:          exec_print(); break;
            }
        }

        private void exec_usrcall(NakoILCode code)
        {
            NakoCallStack c = new NakoCallStack();
            c.localVar = localVar;
            c.nextpos = runpos + 1;
            this.localVar = new NakoVariables();
            callStack.Push(c);
            // JUMP
            autoIncPos = false;
            runpos = (int)code.value;
        }

        private void exec_ret(NakoILCode code)
        {
            autoIncPos = false;
            NakoCallStack c = callStack.Pop();
            this.runpos = c.nextpos;
        }

        private void _branch_true(NakoILCode code)
        {
            Object v = stack.Pop();
            if (NakoValueConveter.ToInt(v) > 0)
            {
                autoIncPos = false;
                runpos = (Int32)code.value;
            }
        }

        private void _branch_false(NakoILCode code)
        {
            Object v = stack.Pop();
            if (NakoValueConveter.ToInt(v) == 0)
            {
                autoIncPos = false;
                runpos = (Int32)code.value;
            }
        }

        private void _jump(NakoILCode code)
        {
            autoIncPos = false;
            runpos = (Int32)(code.value);
        }

        private void _inc()
        {
            Int32 v = (Int32)stack.Pop();
            v++;
            stack.Push(v);
        }

        private void _dec()
        {
            Int32 v = (Int32)stack.Pop();
            v--;
            stack.Push(v);
        }

        private void _neg()
        {
            Object v = stack.Pop();
            if (v is Int32)
            {
                stack.Push( (Int32)v * -1 );
            }
            if (v is Double)
            {
                stack.Push((Double)v * -1);
            }
            throw new NakoInterpreterException("数値以外にマイナスをつけました");
        }

        private void _not()
        {
            Object v = stack.Pop();
            if (v is Int32)
            {
                stack.Push( ((Int32)v == 0) ? 1 : 0);
            }
            if (v is Double)
            {
                stack.Push(((Double)v == 0) ? 1 : 0);
            }
            throw new NakoInterpreterException("数値以外にマイナスをつけました");
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

        private void ld_local_ref(int no)
        {
            NakoVariable v = localVar.GetVar(no);
            stack.Push(v);
        }

        private void ld_global_ref(int no)
        {
            NakoVariable v = globalVar.GetVar(no);
            stack.Push(v);
        }

        private void exec_print()
        {
            Object o = stack.Pop();
            String s;
            if (o == null) {
                s = "";
            } else {
                s = o.ToString();
            }
            if (UseConsoleOut)
            {
                Console.Write(s);
            }
            PrintLog += s;
        }

        private void exec_syscall(NakoILCode code)
        {
            int funcNo = (int)code.value;
            NakoAPIFunc s = NakoAPIFuncBank.Instance.list[funcNo];
            NakoFuncCallInfo f = new NakoFuncCallInfo(this);
            Object result = s.FuncDl(f);
            globalVar.SetValue(0, result); // 変数「それ」に値をセット
            StackPush(result); // 関数の結果を PUSH する
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
            Boolean r = (a is Int32 && b is Int32);
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
        private Object calc_method_eq(Object a, Object b)
        {
            if (a is Int32 && b is Int32)
            {
                return (Int32)a == (Int32)b;
            }
            if (a is String || b is String)
            {
                return NakoValueConveter.ToString(a) == NakoValueConveter.ToString(b);
            }
            if (a is Double || b is Double)
            {
                return NakoValueConveter.ToDouble(a) == NakoValueConveter.ToDouble(b);
            }
            return a == b;
        }
        private Object calc_method_not_eq(Object a, Object b)
        {
            if (a is Int32 && b is Int32)
            {
                return (Int32)a != (Int32)b;
            }
            if (a is String || b is String)
            {
                return NakoValueConveter.ToString(a) != NakoValueConveter.ToString(b);
            }
            if (a is Double || b is Double)
            {
                return NakoValueConveter.ToDouble(a) != NakoValueConveter.ToDouble(b);
            }
            return a != b;
        }
        private Object calc_method_gt(Object a, Object b)
        {
            if (a is Int32 && b is Int32)
            {
                return (Int32)a > (Int32)b;
            }
            if (a is String || b is String)
            {
                return (String.Compare(NakoValueConveter.ToString(a), NakoValueConveter.ToString(b)) > 0);
            }
            if (a is Double || b is Double)
            {
                return NakoValueConveter.ToDouble(a) > NakoValueConveter.ToDouble(b);
            }
            throw new NakoInterpreterException("オブジェクトは比較できません");
        }
        private Object calc_method_gteq(Object a, Object b)
        {
            if (a is Int32 && b is Int32)
            {
                return (Int32)a >= (Int32)b;
            }
            if (a is String || b is String)
            {
                return (String.Compare(NakoValueConveter.ToString(a), NakoValueConveter.ToString(b)) >= 0);
            }
            if (a is Double || b is Double)
            {
                return NakoValueConveter.ToDouble(a) >= NakoValueConveter.ToDouble(b);
            }
            throw new NakoInterpreterException("オブジェクトは比較できません");
        }
        private Object calc_method_lt(Object a, Object b)
        {
            if (a is Int32 && b is Int32)
            {
                return (Int32)a < (Int32)b;
            }
            if (a is String || b is String)
            {
                return (String.Compare(NakoValueConveter.ToString(a), NakoValueConveter.ToString(b)) < 0);
            }
            if (a is Double || b is Double)
            {
                return NakoValueConveter.ToDouble(a) < NakoValueConveter.ToDouble(b);
            }
            throw new NakoInterpreterException("オブジェクトは比較できません");
        }
        private Object calc_method_lteq(Object a, Object b)
        {
            if (a is Int32 && b is Int32)
            {
                return (Int32)a <= (Int32)b;
            }
            if (a is String || b is String)
            {
                return (String.Compare(NakoValueConveter.ToString(a), NakoValueConveter.ToString(b)) <= 0);
            }
            if (a is Double || b is Double)
            {
                return NakoValueConveter.ToDouble(a) <= NakoValueConveter.ToDouble(b);
            }
            throw new NakoInterpreterException("オブジェクトは比較できません");
        }
        private Object calc_method_and(Object a, Object b)
        {
            if (a is Int32 && b is Int32)
            {
                return (Int32)a & (Int32)b;
            }
            if (a is Double || b is Double)
            {
                return NakoValueConveter.ToInt(a) & NakoValueConveter.ToInt(b);
            }
            throw new NakoInterpreterException("オブジェクトは論理演算できません");
        }
        private Object calc_method_or(Object a, Object b)
        {
            if (a is Int32 && b is Int32)
            {
                return (Int32)a | (Int32)b;
            }
            if (a is Double || b is Double)
            {
                return NakoValueConveter.ToInt(a) | NakoValueConveter.ToInt(b);
            }
            throw new NakoInterpreterException("オブジェクトは論理演算できません");
        }
        private Object calc_method_xor(Object a, Object b)
        {
            if (a is Int32 && b is Int32)
            {
                return (Int32)a ^ (Int32)b;
            }
            if (a is Double || b is Double)
            {
                return NakoValueConveter.ToInt(a) ^ NakoValueConveter.ToInt(b);
            }
            throw new NakoInterpreterException("オブジェクトは論理演算できません");
        }
        
    }

    internal class NakoInterpreterException : Exception
    {
        internal NakoInterpreterException(String message) : base(message)
        {
        }

    }

    public class NakoCallStack
    {
		public NakoVariables localVar { get; set; }
		public int nextpos { get; set; }
    }
}
