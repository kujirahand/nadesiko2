using System;
using System.Collections.Generic;

using System.Text;
using Libnako.JCompiler.ILWriter;
using Libnako.JCompiler;
using Libnako.NakoAPI;
using Libnako.JCompiler.Function;
using NakoPlugin;

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
        protected Stack<Object> calcStack;
        /// <summary>
        /// 仮想バイトコードの一覧
        /// </summary>
        protected NakoILCodeList list = null;
        /// <summary>
        /// グローバル変数
        /// </summary>
        public NakoVariableManager globalVar { get; set; }
        /// <summary>
        /// ローカル変数
        /// </summary>
        protected NakoVariableManager localVar;
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
		public String PrintLog
		{
			get { return _printLog.ToString(); }
			set { _printLog.Remove(0, _printLog.Length); _printLog.Append(value); }
		}
		public void AddPrintLog(string str)
		{
			_printLog.Append(str);
		}
		private StringBuilder _printLog = new StringBuilder();
		public Boolean UseConsoleOut { get; set; }
		public Boolean debugMode { get; set; }

        public NakoInterpreter(NakoILCodeList list)
        {
			this.UseConsoleOut = false;
			this.debugMode = false;

			this.list = list;
            Reset();
        }
        public NakoInterpreter()
        {
			this.UseConsoleOut = false;
			this.debugMode = false;
			Reset();
        }

        /// <summary>
        /// 環境のリセット
        /// </summary>
        public void Reset()
        {
            calcStack = new Stack<Object>();
            globalVar = new NakoVariableManager(NakoVariableScope.Global);
            localVar = new NakoVariableManager(NakoVariableScope.Local);
            callStack = new Stack<NakoCallStack>();
            PrintLog = "";
        }

        /// <summary>
        /// ILコードを実行する
        /// </summary>
        /// <param name="list">実行するILコードリスト</param>
        /// <returns>実行が成功したかどうか</returns>
        public Boolean Run(NakoILCodeList list)
        {
            if (list != null)
            {
                Reset();
                this.list = list;
            }
            runpos = 0;
            return _run();
        }
        public Boolean Run()
        {
        	return Run(null);
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
                if (calcStack.Count == 0) return null;
                return calcStack.Peek();
            }
        }

        public Object StackPop()
        {
            Object v = calcStack.Pop();
            if (debugMode)
            {
                Console.WriteLine("- POP:" + Convert.ToString(v));
            }
            return v;
        }

        public void StackPush(Object v)
        {
            if (debugMode)
            {
                Console.WriteLine("- PUSH:" + Convert.ToString(v));
            }
            calcStack.Push(v);
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
                    if (code.value is Int64)
                    {
                        s += String.Format("({0,4:X4})", (Int64)code.value);
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
                case NakoILType.POP:            StackPop(); break;
                // 定数をスタックに乗せる
                case NakoILType.LD_CONST_INT:   StackPush(code.value); break;
                case NakoILType.LD_CONST_REAL:  StackPush(code.value); break;
                case NakoILType.LD_CONST_STR:   StackPush(code.value); break;
                // 変数の値をスタックに乗せる
                case NakoILType.LD_GLOBAL:      ld_global((int)code.value); break;
                case NakoILType.LD_LOCAL:       ld_local((int)code.value); break;
                case NakoILType.LD_GLOBAL_REF:  ld_global_ref((int)code.value); break;
                case NakoILType.LD_LOCAL_REF:   ld_local_ref((int)code.value); break;
                case NakoILType.ST_GLOBAL:      st_global((int)code.value); break;
                case NakoILType.ST_LOCAL:       st_local((int)code.value); break;
                case NakoILType.LD_ELEM:        ld_elem(); break;
                case NakoILType.LD_ELEM_REF:    ld_elem_ref(); break;
                case NakoILType.ST_ELEM:        st_elem(); break;
                // 計算処理
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
                // ジャンプ
                case NakoILType.JUMP:           _jump(code); break;
                // 条件ジャンプ
                case NakoILType.BRANCH_TRUE:    _branch_true(code); break;
                case NakoILType.BRANCH_FALSE:   _branch_false(code); break;
                // 関数コール
                case NakoILType.SYSCALL:        exec_syscall(code); break;
                case NakoILType.USRCALL:        exec_usrcall(code); break;
                case NakoILType.RET:            exec_ret(code); break;
                // デバッグ用
                case NakoILType.PRINT:          exec_print(); break;
                default:
                    throw new NakoInterpreterException("未実装のILコード");
            }
        }

        private void exec_usrcall(NakoILCode code)
        {
            NakoCallStack c = new NakoCallStack();
            c.localVar = localVar;
            c.nextpos = runpos + 1;
            this.localVar = new NakoVariableManager(NakoVariableScope.Local);
            callStack.Push(c);
            // JUMP
            autoIncPos = false;
            runpos = Convert.ToInt32((Int64)code.value);
        }

        private void exec_ret(NakoILCode code)
        {
            autoIncPos = false;
            NakoCallStack c = callStack.Pop();
            this.runpos = c.nextpos;
        }

        private void _branch_true(NakoILCode code)
        {
            Object v = calcStack.Pop();
            if (NakoValueConveter.ToLong(v) > 0)
            {
                autoIncPos = false;
                runpos = Convert.ToInt32((Int64)code.value);
            }
        }

        private void _branch_false(NakoILCode code)
        {
            Object v = calcStack.Pop();
            if (NakoValueConveter.ToLong(v) == 0)
            {
                autoIncPos = false;
                runpos = Convert.ToInt32((Int64)code.value);
            }
        }

        private void _jump(NakoILCode code)
        {
            autoIncPos = false;
            runpos = Convert.ToInt32((Int64)(code.value));
        }

        private void _inc()
        {
            Int64 v = (Int64)calcStack.Pop();
            v++;
            StackPush(v);
        }

        private void _dec()
        {
            Int64 v = (Int64)calcStack.Pop();
            v--;
            StackPush(v);
        }

        private void _neg()
        {
            Object v = calcStack.Pop();
            if (v is Int64)
            {
                StackPush((Int64)v * -1);
            }
            if (v is Double)
            {
                StackPush((Double)v * -1);
            }
            throw new NakoInterpreterException("数値以外にマイナスをつけました");
        }

        private void _not()
        {
            Object v = calcStack.Pop();
            if (v is Int64)
            {
                StackPush(((Int64)v == 0) ? 1 : 0);
            }
            if (v is Double)
            {
                StackPush(((Double)v == 0) ? 1 : 0);
            }
            throw new NakoInterpreterException("数値以外にマイナスをつけました");
        }

        private void st_local(int no)
        {
            Object p = calcStack.Pop();
            localVar.SetValue(no, p);
        }

        private void st_global(int no)
        {
            Object p = calcStack.Pop();
            globalVar.SetValue(no, p);
        }

        private void ld_local(int no)
        {
            Object p = localVar.GetValue(no);
            StackPush(p);
        }

        private void ld_global(int no)
        {
            Object p = globalVar.GetValue(no);
            StackPush(p);
        }

        private void ld_local_ref(int no)
        {
            NakoVariable v = localVar.GetVar(no);
            if (v == null)
            {
                v = new NakoVariable();
                localVar.SetVar(no, v);
            }
            StackPush(v);
        }

        private void ld_global_ref(int no)
        {
            NakoVariable v = globalVar.GetVar(no);
            if (v == null)
            {
                v = new NakoVariable();
                v.varNo = no;
                globalVar.SetVar(no, v);
            }
            StackPush(v);
        }

        private void ld_elem()
        {
            Object idx = StackPop();
            Object var = StackPop();
            Object r = null;
            if (var is NakoVarArray)
            {
                r = ((NakoVarArray)var).GetValueFromObj(idx);
            }
            else if (var is NakoVariable)
            {
                if (((NakoVariable)var).Body is NakoVarArray)
                {
                    NakoVarArray ary = (NakoVarArray)((NakoVariable)var).Body;
                    r = ary.GetValueFromObj(idx);
                }
            }
            StackPush(r);
        }

        /// <summary>
        /// 配列要素をスタックに乗せるが、その時、配列オブジェクトへのリンクを乗せる
        /// </summary>
        private void ld_elem_ref()
        {
            Object idx = StackPop();
            Object var = StackPop();
            NakoVarArray var_ary;

            // var が不正なら null を乗せて帰る
            if (!(var is NakoVariable))
            {
                StackPush(null);
                return;
            }

            if (((NakoVariable)var).Body == null)
            {
                ((NakoVariable)var).Body = new NakoVarArray();
                ((NakoVariable)var).Type = NakoVarType.Array;
            }

            if (((NakoVariable)var).Body is NakoVarArray)
            {
                var_ary = (NakoVarArray)((NakoVariable)var).Body;
                NakoVariable elem = var_ary.GetVarFromObj(idx);
                if (elem == null)
                {
                    elem = new NakoVariable();
                    var_ary.SetVarFromObj(idx, elem);
                }
                StackPush(elem);
            }
            else
            {
                StackPush(null);
            }
        }
        private void st_elem()
        {
            Object value = StackPop();
            Object index = StackPop();
            Object var = StackPop();
            if (var is NakoVariable)
            {
                NakoVariable var2 = (NakoVariable)var;
                // null なら NakoArray として生成
                if (var2.Body == null)
                {
                    var2.Body = new NakoVarArray();
                    var2.Type = NakoVarType.Array;
                }
                // NakoArray なら 要素にセット
                if (var2.Body is NakoVarArray)
                {
                    NakoVarArray var3 = (NakoVarArray)(var2.Body);
                    NakoVariable elem = var3.GetVarFromObj(index);
                    if (elem == null)
                    {
                        elem = new NakoVariable();
                        elem.Body = value;
                        if (index is Int64)
                        {
                            elem.varNo = Convert.ToInt32(index);
                        }
                        var3.SetVarFromObj(index, elem);
                    }
                    else
                    {
                        elem.Body = value;
                    }
                }
            }
        }

        private void exec_print()
        {
            Object o = calcStack.Pop();
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
            //PrintLog += s;
			AddPrintLog(s);
        }

        private void exec_syscall(NakoILCode code)
        {
            int funcNo = (int)code.value;
            NakoAPIFunc s = NakoAPIFuncBank.Instance.FuncList[funcNo];
            NakoFuncCallInfo f = new NakoFuncCallInfo(this);
            Object result = s.FuncDl(f);
            globalVar.SetValue(0, result); // 変数「それ」に値をセット
            StackPush(result); // 関数の結果を PUSH する
        }

        private void exec_calc(CalcMethodType f)
        {
            Object b = calcStack.Pop();
            Object a = calcStack.Pop();
            StackPush(f(a, b));
        }

        private Double ToDouble(Object v)
        {
            return NakoValueConveter.ToDouble(v);
        }

        private Boolean IsBothInt(Object a, Object b)
        {
            Boolean r = (a is Int64 && b is Int64);
            return r;
        }

        private Object calc_method_add(Object a, Object b)
        {
            if (IsBothInt(a, b))
            {
                Int64 i = (Int64)a + (Int64)b;
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
                Int64 i = (Int64)a - (Int64)b;
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
                Int64 i = (Int64)a * (Int64)b;
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
            Int64 i = (Int64)a % (Int64)b;
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
            if (a is Int64 && b is Int64)
            {
                return (Int64)a == (Int64)b;
            }
            if (a is String || b is String)
            {
                return NakoValueConveter.ToString(a) == NakoValueConveter.ToString(b);
            }
            if (a is Double || b is Double)
            {
                return NakoValueConveter.ToDouble(a) == NakoValueConveter.ToDouble(b);
            }
            return NakoValueConveter.ToString(a) == NakoValueConveter.ToString(b);
        }
        private Object calc_method_not_eq(Object a, Object b)
        {
            if (a is Int64 && b is Int64)
            {
                return (Int64)a != (Int64)b;
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
            if (a is Int64 && b is Int64)
            {
                return (Int64)a > (Int64)b;
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
            if (a is Int64 && b is Int64)
            {
                return (Int64)a >= (Int64)b;
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
            if (a is Int64 && b is Int64)
            {
                return (Int64)a < (Int64)b;
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
            if (a is Int64 && b is Int64)
            {
                return (Int64)a <= (Int64)b;
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
            if (a is Int64 && b is Int64)
            {
                return (Int64)a & (Int64)b;
            }
            if (a is Double || b is Double)
            {
                return NakoValueConveter.ToLong(a) & NakoValueConveter.ToLong(b);
            }
            throw new NakoInterpreterException("オブジェクトは論理演算できません");
        }
        private Object calc_method_or(Object a, Object b)
        {
            if (a is Int64 && b is Int64)
            {
                return (Int64)a | (Int64)b;
            }
            if (a is Double || b is Double)
            {
                return NakoValueConveter.ToLong(a) | NakoValueConveter.ToLong(b);
            }
            throw new NakoInterpreterException("オブジェクトは論理演算できません");
        }
        private Object calc_method_xor(Object a, Object b)
        {
            if (a is Int64 && b is Int64)
            {
                return (Int64)a ^ (Int64)b;
            }
            if (a is Double || b is Double)
            {
                return NakoValueConveter.ToLong(a) ^ NakoValueConveter.ToLong(b);
            }
            throw new NakoInterpreterException("オブジェクトは論理演算できません");
        }
        
    }

    internal class NakoInterpreterException : ApplicationException
    {
        internal NakoInterpreterException(String message) : base(message)
        {
        }

    }

    public class NakoCallStack
    {
		public NakoVariableManager localVar { get; set; }
		public int nextpos { get; set; }
    }
}
