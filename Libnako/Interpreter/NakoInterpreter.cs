﻿using System;
using System.Collections.Generic;

using System.Text;
using Libnako.JPNCompiler.ILWriter;
using Libnako.JPNCompiler;
using Libnako.NakoAPI;
using Libnako.JPNCompiler.Function;
using Libnako.Interpreter.ILCode;
using NakoPlugin;

namespace Libnako.Interpreter
{
    /// <summary>
    /// 計算用デリゲート
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    delegate object CalcMethodType(object a, object b);

    /// <summary>
    /// なでしこの中間コード（NakoILCode）を実行するインタプリタ
    /// </summary>
    public class NakoInterpreter : INakoInterpreter
    {
        /// <summary>
        /// 計算用のスタック
        /// </summary>
        protected Stack<object> calcStack;
        /// <summary>
        /// 仮想バイトコードの一覧
        /// </summary>
        protected NakoILCodeList list = null;
        /// <summary>
        /// グローバル変数
        /// </summary>
        public NakoVariableManager globalVar { get; set; }
		/// <summary>
		/// the exception table.
		/// </summary>
		/// <value>The exception table.</value>
		public NakoExceptionTable exceptionTable { get; set; }
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
        protected bool autoIncPos = true;
		/// <summary>
		/// The thrown code.
		/// </summary>
		private NakoILCode thrownCode = null;
        /// <summary>
        /// デバッグ用のログ記録用変数
        /// </summary>
        public string PrintLog
        {
            get { return _printLog.ToString(); }
            set { _printLog.Remove(0, _printLog.Length); _printLog.Append(value); }
        }
        /// <summary>
        /// ログに文字列を追加する
        /// </summary>
        /// <param name="str"></param>
        public void AddPrintLog(string str)
        {
            _printLog.Append(str);
        }
        private StringBuilder _printLog = new StringBuilder();
        /// <summary>
        /// コンソールに出力するか
        /// </summary>
        public bool UseConsoleOut { get; set; }
        /// <summary>
        /// デバッグモード
        /// </summary>
        public bool debugMode { get; set; }
        
        /// <summary>
        /// インタプリタ識別用ID
        /// </summary>
        public int InterpreterId { get { return _interpreter_id; } }
        private int _interpreter_id;
        private static int _interpreter_id_count = 0;
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="list"></param>
        public NakoInterpreter(NakoILCodeList list)
        {
            this.UseConsoleOut = false;
            this.debugMode = false;
            Reset();
            this.list = list;
            if (list.globalVar != null) {
            	this.globalVar = list.globalVar;
            }
			this.exceptionTable = new NakoExceptionTable ();
        }
        /// <summary>
        /// constructor
        /// </summary>
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
            // インタプリタ識別番号の設定
            _interpreter_id = _interpreter_id_count;
            _interpreter_id_count++;
            // スタックや変数などを生成
            calcStack = new Stack<object>();
			globalVar = new NakoVariableManager(NakoVariableScope.Global);
            localVar = new NakoVariableManager(NakoVariableScope.Local);
            callStack = new Stack<NakoCallStack>();
            PrintLog = "";
            InitPlugins();
        }
        
        /// <summary>
        /// プラグインの初期化
        /// </summary>
        void InitPlugins()
        {
            NakoAPIFuncBank bank = NakoAPIFuncBank.Instance;
            foreach (INakoPlugin plugin in bank.PluginList.Values)
            {
                plugin.PluginInit(this);
            }
        }
        /// <summary>
        /// プラグインの終了処理
        /// </summary>
        void FinPlugins()
        {
            NakoAPIFuncBank bank = NakoAPIFuncBank.Instance;
            foreach (INakoPlugin plugin in bank.PluginList.Values)
            {
                plugin.PluginFin(this);
            }
        }
        
        /// <summary>
        /// ILコードを実行する
        /// </summary>
        /// <param name="list">実行するILコードリスト</param>
        /// <returns>実行が成功したかどうか</returns>
        public bool Run(NakoILCodeList list)
        {
            if (list != null)
            {
                Reset();
                this.list = list;
                if (list.globalVar != null) {
                	this.globalVar = list.globalVar;
                }
				this.exceptionTable = new NakoExceptionTable ();
            }
            runpos = 0;
            bool result = _run();
            
            FinPlugins();
            return result;
        }
        /// <summary>
        /// 実行
        /// </summary>
        /// <returns></returns>
        public bool Run()
        {
        	return Run(null);
        }
        /// <summary>
        /// 実行
        /// </summary>
        /// <returns></returns>
        protected bool _run()
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
        /// <summary>
        /// スタックトップを調べる
        /// </summary>
        public object StackTop
        {
            get {
                if (calcStack.Count == 0) return null;
                return calcStack.Peek();
            }
        }
        /// <summary>
        /// スタックから末尾要素を取り出す
        /// </summary>
        /// <returns></returns>
        public object StackPop()
        {
            object v = calcStack.Pop();
            if (debugMode)
            {
                Console.WriteLine("- POP:" + Convert.ToString(v));
            }
            return v;
        }
        /// <summary>
        /// スタックに値をプッシュ
        /// </summary>
        /// <param name="v"></param>
        public void StackPush(object v)
        {
            if (debugMode)
            {
                Console.WriteLine("- PUSH:" + Convert.ToString(v));
            }
            calcStack.Push(v);
        }
        /// <summary>
        /// ILを実行する
        /// </summary>
        /// <param name="code"></param>
        protected void Run_NakoIL(NakoILCode code)
        {
            if (debugMode)
            {
                int i = runpos;
                string s = "";
                s += String.Format("{0,4:X4}:", i);
                s += String.Format("{0,-14}", code.type.ToString());
                s += code.GetDescription();
                Console.WriteLine(s);
            }

			if (thrownCode != null) {
				if (code.type == NakoILType.JUMP) {
					_jump (code);
					_throw (thrownCode);
				} else if (code.type == NakoILType.RET) {
					exec_ret (code);
					_throw (thrownCode);
				} else if (code.type == NakoILType.LD_GLOBAL) {
					ld_global((int)code.value);
				}
				return;
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
                case NakoILType.ARR_LENGTH:     arr_length(); break;
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
                // ローカル変数に対する操作
                case NakoILType.INC_LOCAL: inc_local(code); break;
                case NakoILType.DEC_LOCAL: dec_local(code); break;
                case NakoILType.DUP: dup(); break;
				//exception
				case NakoILType.THROW:            _throw(code); break;
				//exceptionTable
				case NakoILType.EXCEPTIONTABLE:		_exceptionTable(code); break;
                //
                default:
                    throw new NakoInterpreterException("未実装のILコード");
            }
        }

        private void dup()
        {
            // スタックトップの値を複製
            object o = calcStack.Peek();
            calcStack.Push(o);
        }

        private void inc_local(NakoILCode code)
        {
            NakoVariable v = localVar.GetVar((int)code.value);
            v.AsInt++;
        }
        private void dec_local(NakoILCode code)
        {
            NakoVariable v = localVar.GetVar((int)code.value);
            v.AsInt--;
        }

        private void exec_usrcall(NakoILCode code)
        {
            NakoCallStack c = new NakoCallStack();
            c.localVar = localVar;
            c.nextpos = runpos + 1;
			c.sore = globalVar.GetValue(0);
            this.localVar = new NakoVariableManager(NakoVariableScope.Local);
            if (code.value == null) {//引数に関数定義がある場合
                code.value = StackPop ();
            }
            callStack.Push(c);
            // JUMP
            autoIncPos = false;
            runpos = Convert.ToInt32((long)code.value);
        }

        private void exec_ret(NakoILCode code)
        {
            autoIncPos = false;
            NakoCallStack c = callStack.Pop();
            this.runpos = c.nextpos;
			if (((bool)code.value) == false)
				globalVar.SetValue(0, c.sore);// "それ"を関数実行前に戻す
			this.localVar = c.localVar;
        }

		private void _exceptionTable(NakoILCode code){
			exceptionTable.Add ((NakoException)code.value);
		}

		private void _throw(NakoILCode code){
			//TODO:check exception table which include runpos and jump if not throw NakoException
			int catchpos = (exceptionTable==null)? -1 : exceptionTable.GetCatchLine (runpos, code.value);
			if (catchpos == this.runpos) {
				thrownCode = code;
			} else {
				thrownCode = null;
				this.runpos = catchpos;
			}
			if (this.runpos == -1) {
				throw new NakoInterpreterException ("例外");
			}
		}

        private void _branch_true(NakoILCode code)
        {
            object v = calcStack.Pop();
            if (NakoValueConveter.ToLong(v) > 0)
            {
                autoIncPos = false;
                runpos = Convert.ToInt32((long)code.value);
            }
        }

        private void _branch_false(NakoILCode code)
        {
            object v = calcStack.Pop();
            if (NakoValueConveter.ToLong(v) == 0)
            {
                autoIncPos = false;
                runpos = Convert.ToInt32((long)code.value);
            }
        }

        private void _jump(NakoILCode code)
        {
            autoIncPos = false;
            runpos = Convert.ToInt32((long)(code.value));
        }

        private void _inc()
        {
            long v = (long)calcStack.Pop();
            v++;
            StackPush(v);
        }

        private void _dec()
        {
            long v = (long)calcStack.Pop();
            v--;
            StackPush(v);
        }

        private void _neg()
        {
            object v = calcStack.Pop();
            if (v is long)
            {
                StackPush((long)v * -1);
            }
            if (v is double)
            {
                StackPush((double)v * -1);
            }
            throw new NakoInterpreterException("数値以外にマイナスをつけました");
        }

        private void _not()
        {
            object v = calcStack.Pop();
            if (v is long)
            {
                StackPush(((long)v == 0) ? 1 : 0);
            }
            if (v is double)
            {
                StackPush(((double)v == 0) ? 1 : 0);
            }
            throw new NakoInterpreterException("数値以外にマイナスをつけました");
        }

        private void st_local(int no)
        {
            object p = calcStack.Pop();
            localVar.SetValue(no, p);
        }

        private void st_global(int no)
        {
            object p = calcStack.Pop();
            globalVar.SetValue(no, p);
        }

        private void ld_local(int no)
        {
            object p = localVar.GetValue(no);
            StackPush(p);
        }

        private void ld_global(int no)
        {
            object p = globalVar.GetValue(no);
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

        private string ld_elem_slice(string s, object index)
        {
            if (!(index is long))
            {
                return null;
            }
            long idx = (long)index;
            string[] a = s.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            if (a.Length > idx)
            {
                return a[(long)idx];
            }
            return null;
        }

        private void ld_elem()
        {
            object idx = StackPop();
            object var = StackPop();
            object r = null;
            if (var is NakoVarArray)
            {
                r = ((NakoVarArray)var).GetValueFromObj(idx);
            }
            else if (var is NakoVariable)
            {
                // 変数は配列か?
                if (((NakoVariable)var).Body is NakoVarArray)
                {
                    NakoVarArray ary = (NakoVarArray)((NakoVariable)var).Body;
                    r = ary.GetValueFromObj(idx);
                }
                else
                {
                    r = ld_elem_slice(var.ToString(), idx);
                }
            }
            else
            {
                string vs = var.ToString();
                r = ld_elem_slice(vs, idx);
            }
            StackPush(r);
        }

        /// <summary>
        /// 配列要素をスタックに乗せるが、その時、配列オブジェクトへのリンクを乗せる
        /// </summary>
        private void ld_elem_ref()
        {
            object idx = StackPop();
            object var = StackPop();
            NakoVarArray var_ary;

            // var が不正なら null を乗せて帰る
            if (!(var is NakoVariable))
            {
                StackPush(null);
                return;
            }
            if (var is NakoVarArray)
            {
                NakoVarArray a = (NakoVarArray)var;
                NakoVariable elem = a.GetVarFromObj(idx);
                StackPush(elem);
                return;
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

            if (((NakoVariable)var).Body == null)
            {
                ((NakoVariable)var).SetBody(new NakoVarArray(), NakoVarType.Array);
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
            object value = StackPop();
            object index = StackPop();
            object var = StackPop();
            if (var is NakoVariable)
            {
                NakoVariable var2 = (NakoVariable)var;
                // null か空の文字列なら NakoArray として生成
                if (var2.Body == null || (var2.Type==NakoVarType.String && (string)var2.Body==""))
                {
                    var2.SetBody(new NakoVarArray(), NakoVarType.Array);
                }
                if (!(var2.Body is NakoVarArray))
                {
                    string s = "";
                    if (var2.Body != null) s = var2.Body.ToString();
                    var2.SetBody(new NakoVarArray(), NakoVarType.Array);
                    ((NakoVarArray)var2.Body).SetValuesFromString(s);
                    
                }
                // NakoArray なら 要素にセット
                if (var2.Body is NakoVarArray)
                {
                    NakoVarArray var3 = (NakoVarArray)(var2.Body);
                    NakoVariable elem = var3.GetVarFromObj(index);
                    if (elem == null)
                    {
                        elem = new NakoVariable();
                        elem.SetBodyAutoType(value);
                        if (index is long)
                        {
                            elem.varNo = Convert.ToInt32(index);
                        }
                        var3.SetVarFromObj(index, elem);
                    }
                    else
                    {
                        elem.SetBodyAutoType(value);
                    }
                }
            }
        }

        private void arr_length()
        {
            object value = StackPop();
            
            // 配列要素を取り出す
            NakoVarArray arr = null;
            if (value is NakoVarArray)
            {
                arr = (NakoVarArray)value;
            }
            else if (value is NakoVariable)
            {
                NakoVariable v = (NakoVariable)value;
                if (v.Type == NakoVarType.Array)
                {
                    arr = (NakoVarArray)v.Body;
                }
            }
            if (arr != null)
            {
                StackPush(arr.Count);
            }
            else
            {
                StackPush(0L);
            }
            return;
        }

        private void exec_print()
        {
            object o = calcStack.Pop();
            string s;
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
            object result = s.FuncDl(f);
			if (s.updateSore)
				globalVar.SetValue(0, result); // 変数「それ」に値をセット
            StackPush(result); // 関数の結果を PUSH する
        }

        private void exec_calc(CalcMethodType f)
        {
            object b = calcStack.Pop();
            object a = calcStack.Pop();
            StackPush(f(a, b));
        }

        private double ToDouble(object v)
        {
            return NakoValueConveter.ToDouble(v);
        }

        private bool IsBothInt(object a, object b)
        {
            bool r = (a is long && b is long);
            return r;
        }

        private object calc_method_add(object a, object b)
        {
            if (IsBothInt(a, b))
            {
                long i = (long)a + (long)b;
                return (object)i;
            }
            else
            {
                double d = ToDouble(a) + ToDouble(b);
                return (object)d;
            }
        }
        private object calc_method_sub(object a, object b)
        {
            if (IsBothInt(a, b))
            {
                long i = (long)a - (long)b;
                return (object)i;
            }
            else
            {
                double d = ToDouble(a) - ToDouble(b);
                return (object)d;
            }
        }
        private object calc_method_mul(object a, object b)
        {
            if (IsBothInt(a, b))
            {
                long i = (long)a * (long)b;
                return (object)i;
            }
            else
            {
                double d = ToDouble(a) * ToDouble(b);
                return (object)d;
            }
        }
        private object calc_method_div(object a, object b)
        {
            // "1 ÷ 2" のような場合を想定して、割り算は常に実数にすることにした
            double d = ToDouble(a) / ToDouble(b);
            return (object)d;
        }
        private object calc_method_mod(object a, object b)
        {
            long i = (long)a % (long)b;
            return (object)i;
        }
        private object calc_method_power(object a, object b)
        {
            return (object)
                Math.Pow(ToDouble(a), ToDouble(b));
        }
        private object calc_method_add_str(object a, object b)
        {
            if (a == null) a = "";
            if (b == null) b = "";
            string sa = a.ToString();
            string sb = b.ToString();
            return sa + sb;
        }
        private object calc_method_eq(object a, object b)
        {
            if (a is long && b is long)
            {
                return (long)a == (long)b;
            }
            if (a is string || b is string)
            {
                return NakoValueConveter.ToString(a) == NakoValueConveter.ToString(b);
            }
            if (a is double || b is double)
            {
                return NakoValueConveter.ToDouble(a) == NakoValueConveter.ToDouble(b);
            }
            return NakoValueConveter.ToString(a) == NakoValueConveter.ToString(b);
        }
        private object calc_method_not_eq(object a, object b)
        {
            if (a is long && b is long)
            {
                return (long)a != (long)b;
            }
            if (a is string || b is string)
            {
                return NakoValueConveter.ToString(a) != NakoValueConveter.ToString(b);
            }
            if (a is double || b is double)
            {
                return NakoValueConveter.ToDouble(a) != NakoValueConveter.ToDouble(b);
            }
            return a != b;
        }
        private object calc_method_gt(object a, object b)
        {
            if (a is long && b is long)
            {
                return (long)a > (long)b;
            }
            if (a is string || b is string)
            {
                return (string.Compare(NakoValueConveter.ToString(a), NakoValueConveter.ToString(b)) > 0);
            }
            if (a is double || b is double)
            {
                return NakoValueConveter.ToDouble(a) > NakoValueConveter.ToDouble(b);
            }
            throw new NakoInterpreterException("オブジェクトは比較できません");
        }
        private object calc_method_gteq(object a, object b)
        {
            if (a is long && b is long)
            {
                return (long)a >= (long)b;
            }
            if (a is string || b is string)
            {
                return (string.Compare(NakoValueConveter.ToString(a), NakoValueConveter.ToString(b)) >= 0);
            }
            if (a is double || b is double)
            {
                return NakoValueConveter.ToDouble(a) >= NakoValueConveter.ToDouble(b);
            }
            throw new NakoInterpreterException("オブジェクトは比較できません");
        }
        private object calc_method_lt(object a, object b)
        {
            if (a is long && b is long)
            {
                return (long)a < (long)b;
            }
            if (a is string || b is string)
            {
                return (string.Compare(NakoValueConveter.ToString(a), NakoValueConveter.ToString(b)) < 0);
            }
            if (a is double || b is double)
            {
                return NakoValueConveter.ToDouble(a) < NakoValueConveter.ToDouble(b);
            }
            throw new NakoInterpreterException("オブジェクトは比較できません");
        }
        private object calc_method_lteq(object a, object b)
        {
            if (a is long && b is long)
            {
                return (long)a <= (long)b;
            }
            if (a is string || b is string)
            {
                return (string.Compare(NakoValueConveter.ToString(a), NakoValueConveter.ToString(b)) <= 0);
            }
            if (a is double || b is double)
            {
                return NakoValueConveter.ToDouble(a) <= NakoValueConveter.ToDouble(b);
            }
            throw new NakoInterpreterException("オブジェクトは比較できません");
        }
        private object calc_method_and(object a, object b)
        {
            if (a is long && b is long)
            {
                return (long)a & (long)b;
            }
            if (a is double || b is double)
            {
                return NakoValueConveter.ToLong(a) & NakoValueConveter.ToLong(b);
            }
			if (a is bool && b is bool) {
				return (bool)a && (bool)b;
			}
			throw new NakoInterpreterException("オブジェクトは論理演算できません");
        }
        private object calc_method_or(object a, object b)
        {
            if (a is long && b is long)
            {
                return (long)a | (long)b;
            }
            if (a is double || b is double)
            {
                return NakoValueConveter.ToLong(a) | NakoValueConveter.ToLong(b);
            }
			if (a is bool && b is bool) {
				return (bool)a || (bool)b;
			}
            throw new NakoInterpreterException("オブジェクトは論理演算できません");
        }
        private object calc_method_xor(object a, object b)
        {
            if (a is long && b is long)
            {
                return (long)a ^ (long)b;
            }
            if (a is double || b is double)
            {
                return NakoValueConveter.ToLong(a) ^ NakoValueConveter.ToLong(b);
            }
			if (a is bool && b is bool) {
				return (bool)a ^ (bool)b;
			}
            throw new NakoInterpreterException("オブジェクトは論理演算できません");
        } 
        private NakoILCode currentIL = null;
        /// <summary>
        /// Callback the specified sender, func_name and args.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="func_name">Func name.</param>
        /// <param name="args">Arguments.</param>
        public void CallUserFunc(string func_name, params object[] args){
            NakoILCode c = null;
            foreach (NakoILCode tmp in this.list) {
                if (tmp.type == NakoILType.NOP && (string)tmp.value == "FUNC_" + func_name) {
                    c = tmp;
                    break;
                }
            }
            if (c == null)
                return;// _nop;
            currentIL = c;
            //StackPush (sender);
            foreach (object o in args) {
                StackPush (o);
            }
            NakoCallStack stack = new NakoCallStack();
            //escape
            int currentPos = runpos;
            bool currentAutoIncPos = autoIncPos;
            object currentSore = globalVar.GetValue(0);
            stack.localVar = localVar;
            stack.nextpos = list.Count+1; // ストッパー
            stack.sore = globalVar.GetValue(0);
            //prepare
            this.localVar = new NakoVariableManager(NakoVariableScope.Local);
            autoIncPos = true;
            runpos = this.list.IndexOf(currentIL);
            callStack.Push(stack);
            //int baseStackCount=callStack.Count;
            _run ();
            runpos = currentPos;
            autoIncPos = currentAutoIncPos;
            globalVar.SetValue(0, currentSore);
        }

    }
    /// <summary>
    /// インタプリタの例外クラス
    /// </summary>
    internal class NakoInterpreterException : ApplicationException
    {
        /// <summary>
        /// インタプリタクラスの例外を出す
        /// </summary>
        /// <param name="message"></param>
        internal NakoInterpreterException(string message) : base(message)
        {
        }

    }

    /// <summary>
    /// 呼び出しスタック
    /// </summary>
    public class NakoCallStack
    {
        /// <summary>
        /// ローカル変数
        /// </summary>
		public NakoVariableManager localVar { get; set; }
        /// <summary>
        /// 次の位置
        /// </summary>
		public int nextpos { get; set; }
        /// <summary>
        /// それ
        /// </summary>
		public object sore { get; set; }
    }
}
