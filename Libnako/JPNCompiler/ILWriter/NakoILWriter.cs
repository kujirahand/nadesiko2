using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using Libnako.JPNCompiler.Node;
using Libnako.JPNCompiler.Parser;
using Libnako.JPNCompiler.Function;
using Libnako.NakoAPI;
using Libnako.Interpreter.ILCode;

namespace Libnako.JPNCompiler.ILWriter
{
    /// <summary>
    /// 構文ノードから、なでしこ仮想バイトコードを書き出すクラス
    /// </summary>
    public class NakoILWriter
    {
        /// <summary>
        /// 構文ノードのトップ
        /// </summary>
        protected NakoNode topNode = null;
        /// <summary>
        /// 書き出した結果(内部で利用)
        /// </summary>
        protected NakoILCodeList result = null;
        /// <summary>
        /// 書き出した結果
        /// </summary>
        public NakoILCodeList Result
        {
            get { return result; }
        }
        /// <summary>
        /// ラベル一覧
        /// </summary>
        protected Dictionary<NakoILCode, long> labels = null;
		protected Dictionary<String,NakoILCode> exceptions = null;
		private int _labelId = 0;
		private int GetLableId() { ++_labelId; return _labelId; }
        
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="topNode"></param>
        public NakoILWriter(NakoNode topNode)
        {
            this.topNode = topNode;
            Init();
        }
        /// <summary>
        /// constructor
        /// </summary>
        public NakoILWriter()
        {
            Init();
        }
        /// <summary>
        /// 初期化
        /// </summary>
        public void Init()
        {
            this.result = new NakoILCodeList();
            this.labels = new Dictionary<NakoILCode, long>();
			this.exceptions = new Dictionary<String,NakoILCode>();
        }
        /// <summary>
        /// 中間コードを書き出す
        /// </summary>
        /// <param name="node"></param>
        protected void Write_r(NakoNode node)
        {
            if (node == null) return;
            switch (node.type)
            {
                case NakoNodeType.NOP:
                    result.Add(NakoILCode.newNop());
                    break;
                case NakoNodeType.CALC:
                    newCalc((NakoNodeCalc)node);
                    return;
                case NakoNodeType.INT:
                    result.Add(new NakoILCode(NakoILType.LD_CONST_INT, node.value));
                    return;
                case NakoNodeType.NUMBER:
                    result.Add(new NakoILCode(NakoILType.LD_CONST_REAL, node.value));
                    return;
                case NakoNodeType.STRING:
                    result.Add(new NakoILCode(NakoILType.LD_CONST_STR, node.value));
                    return;
                case NakoNodeType.POP:
                    result.Add(new NakoILCode(NakoILType.POP));
                    return;
                case NakoNodeType.PRINT:
                    _print(node);
                    return;
                case NakoNodeType.ST_VARIABLE:
                    _setVariable((NakoNodeVariable)node);
                    return;
                case NakoNodeType.LET:
                    _let((NakoNodeLet)node);
                    return;
                case NakoNodeType.LD_VARIABLE:
                    _getVariable((NakoNodeVariable)node);
                    return;
                case NakoNodeType.IF:
                    _if((NakoNodeIf)node);
                    return;
                case NakoNodeType.WHILE:
                    _while((NakoNodeWhile)node);
                    return;
                case NakoNodeType.FOR:
                    _for((NakoNodeFor)node);
                    return;
                case NakoNodeType.BREAK:
                    addNewILCode(NakoILType.NOP, "BREAK");
                    return;
                case NakoNodeType.CONTINUE:
                    addNewILCode(NakoILType.NOP, "CONTINUE");
                    return;
                case NakoNodeType.RETURN:
                    addNewILCode(NakoILType.NOP, "RETURN");
                    return;
                case NakoNodeType.REPEAT_TIMES:
                    _repeat_times((NakoNodeRepeatTimes)node);
                    return;
                case NakoNodeType.FOREACH:
                    _foreachUseIterator((NakoNodeForeach)node);
                    //_foreach((NakoNodeForeach)node);
                    return;
                case NakoNodeType.CALL_FUNCTION:
                    _call_function((NakoNodeCallFunction)node);
                    return;
                case NakoNodeType.DEF_FUNCTION:
                    _def_function((NakoNodeDefFunction)node);
                    return;
                case NakoNodeType.JUMP:
                    _jump((NakoNodeJump)node);
                    return;
                //TODO
                case NakoNodeType.LET_VALUE:
                    addNewILCode(NakoILType.NOP, "LET_VALUE");
                    break;
				case NakoNodeType.TRY:
					_try((NakoNodeTry)node);
					return;
				case NakoNodeType.THROW:
					_throw((NakoNodeThrow)node);
					return;
                default:
                    throw new NakoCompilerException("未定義のノードタイプ: " + node.type.ToString());
            }
            // ---
            if (!node.hasChildren()) return;
            Write_list(node.Children);
        }
        /// <summary>
        /// 中間コードを書き出す
        /// </summary>
        /// <param name="topNode"></param>
        public void Write(NakoNode topNode)
        {
            if (topNode != null) { this.topNode = topNode; }
            Write_r(this.topNode);
            FixLabel();
			SetExceptionTables ();
        }
        /// <summary>
        /// ラベルを解決する
        /// </summary>
        public void FixLabel()
        {
            // 現在のラベル位置を調べる
            for (int i = 0; i < result.Count; i++)
            {
                NakoILCode code = result[i];
                if (code.type != NakoILType.NOP) continue;
                if (labels.ContainsKey(code))
                {
                    labels[code] = i;
                }
            }
            // JUMP/BRANCH_TRUE/BRANCH_FALSE/CALL/USRCALL を解決する
            for (int i = 0; i < result.Count; i++)
            {
                NakoILCode code = result[i];
                switch (code.type)
                {
                    case NakoILType.JUMP:
                    case NakoILType.BRANCH_TRUE:
                    case NakoILType.BRANCH_FALSE:
                    case NakoILType.USRCALL:
                        break;
                    default:
                        continue;
                }
                if (!(code.value is NakoILCode)) continue;
                if (code.value is NakoILCode)
                {
                    if (labels.ContainsKey( (NakoILCode)code.value) )
                    {
                        code.value = (object)labels[(NakoILCode)code.value];
                        continue;
                    }
                    throw new NakoILWriterException("ラベルが解決できません");
                }
            }
            
        }
		/// <summary>
		/// Sets the exception tables.
		/// </summary>
		public void SetExceptionTables()
		{
			for (int i = 0; i < result.Count; i++)
			{
				NakoILCode code = result[i];
				if (code.type != NakoILType.EXCEPTIONTABLE)
					continue;
				if (!(code.value is NakoException)) continue;
				if (code.value is NakoException)
				{
					NakoException e = (NakoException)code.value;
					if (labels.ContainsKey( e.fromLabel) )
					{
						e.from = (int)labels[e.fromLabel];
						e.to = (int)labels[e.targetLabel]-1;//endTry?
						e.target = (int)labels [e.targetLabel];
						continue;
					}
					throw new NakoILWriterException("ExceptionTableが解決できません");
				}
			}

		}

        /// <summary>
        /// 指定したリストを書く
        /// </summary>
        /// <param name="list"></param>
        protected void Write_list(NakoNodeList list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                NakoNode node = list[i];
                Write_r(node);
            }
        }
        /// <summary>
        /// ラベルを生成する
        /// </summary>
        /// <param name="labelName"></param>
        /// <returns></returns>
        protected NakoILCode createLABEL(string labelName)
        {
            NakoILCode r = NakoILCode.newNop();
            r.value = labelName;
            labels[r] = -1;
            return r;
        }

        /// <summary>
        /// ラベルジャンプを生成する
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        protected NakoILCode createJUMP(NakoILCode label)
        {
            NakoILCode r = new NakoILCode(NakoILType.JUMP, label);
            return r;
        }

        private void _insertTemp(NakoILType type)
        {
            addNewILCode(type);
        }

//TODO:まだ実装できてない
        private void _try(NakoNodeTry node)
        {
			int labelId = GetLableId();
			NakoILCode label_try = createLABEL("TRY" + labelId.ToString());
			NakoILCode label_catch = createLABEL("CATCH" + labelId.ToString());
			NakoILCode label_finally = createLABEL("FINALLY" + labelId.ToString());
            // (0) Tryを埋め込む
			result.Add(label_try);
            // (1) 通常実行をコードにする
            Write_r(node.nodeTry);
			result.Add(createJUMP(label_finally));
            // (2) 例外処理
			result.Add(label_catch);
			//TODO ○○のエラーならば〜☆☆のエラーならば〜への対応
			//TODO ○○や☆☆のエラーならばへの対応
           if (node.nodeCatch != null)
            {
                Write_r(node.nodeCatch);
				result.Add(createJUMP(label_finally));
				NakoILCode c = new NakoILCode ();
				c.type = NakoILType.EXCEPTIONTABLE;
				c.value = new NakoException (label_try,label_catch,new Exception());
				result.Insert (0, c);
           }
            // (3) 最後に必ず実行する処理
			result.Add(label_finally);
            if (node.nodeFinally != null)
            {
                Write_r(node.nodeFinally);
            }
        }

		private void _throw(NakoNodeThrow node){
			//int errorVarNo = node.errorVarNo;
			//Write_r(node.exceptionNode);
			//addNewILCode(NakoILType.LD_LOCAL_REF, node.exceptionNode.value);
			//addNewILCode(NakoILType.ST_LOCAL, errorVarNo);
			//addNewILCode(NakoILType.LD_LOCAL_REF, errorVarNo);
			addNewILCode (NakoILType.THROW,node.exceptionNode.value);
		}

        private void _if(NakoNodeIf node)
        {
         int labelId = GetLableId();
            // (1) 条件文をコードにする
            Write_r(node.nodeCond);
            // (2) コードの結果により分岐する
            // 分岐先をラベルとして作成
         NakoILCode label_endif = createLABEL("ENDIF" + labelId.ToString());
         NakoILCode label_else = createLABEL("ELSE" + labelId.ToString());
            result.Add(new NakoILCode(NakoILType.BRANCH_FALSE, label_else));
            // (3) TRUE
            if (node.nodeTrue != null)
            {
                Write_r(node.nodeTrue);
                result.Add(createJUMP(label_endif));
            }
            // (4) FALSE
            result.Add(label_else);
            if (node.nodeFalse != null)
            {
                Write_r(node.nodeFalse);
            }
            result.Add(label_endif);
        }

        private void _while(NakoNodeWhile node)
        {
			int labelId = GetLableId();
			// (1) 条件をコードにする
			NakoILCode label_while_begin = createLABEL("WHILE_BEGIN" + labelId.ToString());
            result.Add(label_while_begin);
            Write_r(node.nodeCond);
            // (2) コードの結果により分岐する
            // 分岐先をラベルとして作成
			NakoILCode label_while_end = createLABEL("WHILE_END" + labelId.ToString());
            addNewILCode(NakoILType.BRANCH_FALSE, label_while_end);
            // (3) ループブロックを書き込む
            _loop_check_break_continue(node.nodeBlocks, label_while_end, label_while_begin);
            Write_r(node.nodeBlocks);
            result.Add(createJUMP(label_while_begin));
            result.Add(label_while_end);
        }

//Iterator実装テスト
//TODO:try finallyを入れる必要がある
        private void _foreachUseIterator(NakoNodeForeach node)
        {
            int labelId = GetLableId();
            int loopVarNo = node.loopVarNo;
            int valueVarNo = node.valueVarNo;
            int taisyouVarNo = node.taisyouVarNo;
            int kaisuVarNo = node.kaisuVarNo;
            int enumeratorVarNo = node.enumeratorVarNo;
            int enumeratorFuncNo = node.enumeratorFuncNo;
            int moveresultFuncNo = node.moveresultFuncNo;
            int getcurrentFuncNo = node.getcurrentFuncNo;
            int getdisposeFuncNo = node.getdisposeFuncNo;

            // (0)
            NakoILCode label_for_begin = createLABEL("ITERATOR_BEGIN" + labelId.ToString());
            NakoILCode label_for_end = createLABEL("ITERATOR_END" + labelId.ToString());

            // (1) 変数を初期化する
            result.Add(label_for_begin);
            // カウンタ変数を初期化
            addNewILCode(NakoILType.LD_CONST_INT, 0L);
            addNewILCode(NakoILType.ST_LOCAL, loopVarNo);
            // 反復要素の評価
            //TODO:反復要素のGetEnumerator()を評価
            //GetEnumerator(); addNewILCode(NakoILType.CALL,"GetEnumerator")?
            //addNewILCode(NakoILType.ST_LOCAL, valueVarNo);
            Write_r(node.nodeValue); // 値を評価
            addNewILCode(NakoILType.ST_LOCAL, valueVarNo);
            addNewILCode(NakoILType.LD_LOCAL_REF, valueVarNo);
            addNewILCode(NakoILType.SYSCALL, enumeratorFuncNo);
            addNewILCode(NakoILType.ST_LOCAL, enumeratorVarNo);

            // (2) 条件をコードにする
            //TODO: MoveNext()=true
            NakoILCode label_for_cond = createLABEL("ITERATOR_COND" + labelId.ToString());
            result.Add(label_for_cond);
            // L
            //MoveNext()に変更
            addNewILCode(NakoILType.LD_LOCAL_REF, enumeratorVarNo);
            addNewILCode(NakoILType.SYSCALL, moveresultFuncNo);
            // R
            addNewILCode(NakoILType.LD_CONST_INT, true);//TODO:実際にコンパイルする時にこれはまずいはずなので、何か考える
            // LT
            addNewILCode(NakoILType.EQ);
            // IF BRANCH FALSE
            addNewILCode(NakoILType.BRANCH_FALSE, label_for_end);
            // 反復する値を変数「対象」にセット
            // ** 対象=値\(ループカウンタ)
            //TODO:対象＝オブジェクト.Currentに変更
            addNewILCode(NakoILType.LD_LOCAL_REF, enumeratorVarNo);
            addNewILCode(NakoILType.SYSCALL,getcurrentFuncNo);
            //addNewILCode(NakoILType.NOP, "let-taisyou"); // for DEBUG
            addNewILCode(NakoILType.ST_LOCAL, taisyouVarNo);
            //TODO:key
            // ** 回数=ループカウンタ+1
            addNewILCode(NakoILType.LD_LOCAL, loopVarNo);
            addNewILCode(NakoILType.INC);
            addNewILCode(NakoILType.ST_LOCAL, kaisuVarNo);

            // (3) 繰り返し文を実行する
            _loop_check_break_continue(node.nodeBlocks, label_for_end, label_for_begin);
            Write_r(node.nodeBlocks);

            // (4) 変数を加算する
            addNewILCode(NakoILType.INC_LOCAL, loopVarNo);

            // (5) 手順2に戻る
            result.Add(createJUMP(label_for_cond));
            result.Add(label_for_end);
            addNewILCode(NakoILType.LD_LOCAL_REF, enumeratorVarNo);
            addNewILCode(NakoILType.SYSCALL,getdisposeFuncNo);
            //TODO: EnumeratorをDisposeする
        }

        private void addNewILCode(NakoILType type, object value)
        {
            result.Add(new NakoILCode(type, value));
        }
        private void addNewILCode(NakoILType type)
        {
        	addNewILCode(type, null);
        }

        private void _for(NakoNodeFor node)
        {
            int loopVarNo = node.loopVar.varNo;
			int labelId = GetLableId();

            // (0)
			NakoILCode label_for_begin = createLABEL("FOR_BEGIN" + labelId.ToString());
			NakoILCode label_for_end = createLABEL("FOR_END" + labelId.ToString());

            // (1) 変数を初期化する
            result.Add(label_for_begin);
            Write_r(node.nodeFrom);
            addNewILCode(NakoILType.ST_LOCAL, loopVarNo);

            // (2) 条件をコードにする
            // i <= iTo
			NakoILCode label_for_cond = createLABEL("FOR_COND" + labelId.ToString());
            result.Add(label_for_cond);
            // L
            addNewILCode(NakoILType.LD_LOCAL, loopVarNo);
            // R
            Write_r(node.nodeTo); // TODO:最適化
            // LT_EQ
            addNewILCode(NakoILType.LT_EQ);
            // IF BRANCH FALSE
            addNewILCode(NakoILType.BRANCH_FALSE, label_for_end);

            // (3) 繰り返し文を実行する
            _loop_check_break_continue(node.nodeBlocks, label_for_end, label_for_cond);
            Write_r(node.nodeBlocks);

            // (4) 変数を加算する (ここ最適化できそう)
            addNewILCode(NakoILType.LD_LOCAL, loopVarNo);
            addNewILCode(NakoILType.INC);
            addNewILCode(NakoILType.ST_LOCAL, loopVarNo);

            // (5) 手順2に戻る
            result.Add(createJUMP(label_for_cond));
            result.Add(label_for_end);
        }

        // BREAK/CONTINUE文を解決する
        private void _loop_check_break_continue(NakoNode block, NakoILCode break_label, NakoILCode continue_label)
        {
            if (block == null) return;
            if (!block.hasChildren()) return;
            for (int i = 0; i < block.Children.Count; i++)
            {
                NakoNode item = block.Children[i];

                switch (item.type)
                {
                    case NakoNodeType.BREAK:
                        item.type = NakoNodeType.JUMP;
                        ((NakoNodeBreak)item).label = break_label;
                        break;
                    case NakoNodeType.CONTINUE:
                        item.type = NakoNodeType.JUMP;
                        ((NakoNodeBreak)item).label = continue_label;
                        break;

                    // ジャンプポイントが変わる構文があれば、その下層ブロックは処理しない
                    case NakoNodeType.FOR: break;
                    case NakoNodeType.WHILE: break;
                    case NakoNodeType.REPEAT_TIMES: break;
                    case NakoNodeType.FOREACH: break;
                    case NakoNodeType.IF:
                        NakoNodeIf ifnode = (NakoNodeIf)item;
                        _loop_check_break_continue(ifnode.nodeTrue,  break_label, continue_label);
                        _loop_check_break_continue(ifnode.nodeFalse, break_label, continue_label);
                        break;
                    default:
                        if (item.hasChildren())
                        {
                            _loop_check_break_continue(item, break_label, continue_label);
                        }
                        break;
                }
            }
        }

        // RETURN文を解決する
        private void _func_check_return(NakoNode block, NakoILCode return_label)
        {
            if (block == null) return;
            if (!block.hasChildren()) return;
            for (int i = 0; i < block.Children.Count; i++)
            {
                NakoNode item = block.Children[i];

                switch (item.type)
                {
                    case NakoNodeType.RETURN:
                       item.type = NakoNodeType.JUMP;
                        ((NakoNodeReturn)item).label = return_label;
                        break;

                    // ジャンプポイントが変わる構文があれば、その下層ブロックは処理しない
                    case NakoNodeType.FOR: break;
                    case NakoNodeType.WHILE: break;
                    case NakoNodeType.REPEAT_TIMES: break;
                    case NakoNodeType.FOREACH: break;
                    case NakoNodeType.IF:
                        NakoNodeIf ifnode = (NakoNodeIf)item;
                        _func_check_return(ifnode.nodeTrue, return_label);
                        _func_check_return(ifnode.nodeFalse, return_label);
                        break;
                    default:
                        if (item.hasChildren())
                        {
                            _func_check_return(item, return_label);
                        }
                        break;
                }
            }
        }

        private void _jump(NakoNodeJump node)
        {
            NakoILCode jmp = new NakoILCode(NakoILType.JUMP, node.label);
            result.Add(jmp);
        }

        private void _repeat_times(NakoNodeRepeatTimes node)
        {
			int labelId = GetLableId();

            // (0)
			NakoILCode label_for_begin = createLABEL("TIMES_BEGIN" + labelId.ToString());
			NakoILCode label_for_end = createLABEL("TIMES_END" + labelId.ToString());

            // (1) 変数を初期化する
            result.Add(label_for_begin);
            addNewILCode(NakoILType.LD_CONST_INT, 1L);
            addNewILCode(NakoILType.ST_LOCAL, node.loopVarNo);
            // 何回実行するか回数を評価する
            Write_r(node.nodeTimes);
            addNewILCode(NakoILType.ST_LOCAL, node.timesVarNo);

            // (2) 条件をコードにする
            // i <= iTo
			NakoILCode label_for_cond = createLABEL("TIMES_COND" + labelId.ToString());
            result.Add(label_for_cond);
            // L
            addNewILCode(NakoILType.LD_LOCAL, node.loopVarNo);
            // R
            addNewILCode(NakoILType.LD_LOCAL, node.timesVarNo);
            // LT_EQ
            addNewILCode(NakoILType.LT_EQ);
            // IF BRANCH FALSE
            addNewILCode(NakoILType.BRANCH_FALSE, label_for_end);

            // (3) 繰り返し文を実行する
            _loop_check_break_continue(node.nodeBlocks, label_for_end, label_for_begin);
            Write_r(node.nodeBlocks);

            // (4) 変数を加算する (ここ最適化できそう)
            addNewILCode(NakoILType.INC_LOCAL, node.loopVarNo);

            // (5) 手順2に戻る
            result.Add(createJUMP(label_for_cond));
            result.Add(label_for_end);
        }

//TODO:_foreachUseIteratorがOKならば消す
        private void _foreach(NakoNodeForeach node)
        {
            int labelId = GetLableId();
            int loopVarNo = node.loopVarNo;
            int lenVarNo = node.lenVarNo;
            int valueVarNo = node.valueVarNo;
            int taisyouVarNo = node.taisyouVarNo;
            int kaisuVarNo = node.kaisuVarNo;

            // (0)
            NakoILCode label_for_begin = createLABEL("FOREACH_BEGIN" + labelId.ToString());
            NakoILCode label_for_end = createLABEL("FOREACH_END" + labelId.ToString());

            // (1) 変数を初期化する
            result.Add(label_for_begin);
            // カウンタ変数を初期化
            addNewILCode(NakoILType.LD_CONST_INT, 0L);
            addNewILCode(NakoILType.ST_LOCAL, loopVarNo);
            // 反復要素の評価
            Write_r(node.nodeValue); // 値を評価
            addNewILCode(NakoILType.ST_LOCAL, valueVarNo);
            // 要素数を得る
            addNewILCode(NakoILType.LD_LOCAL, valueVarNo);
            addNewILCode(NakoILType.ARR_LENGTH);
            addNewILCode(NakoILType.ST_LOCAL, lenVarNo);
            
            // (2) 条件をコードにする
            // i < iTo
            NakoILCode label_for_cond = createLABEL("FOREACH_COND" + labelId.ToString());
            result.Add(label_for_cond);
            // L
            addNewILCode(NakoILType.LD_LOCAL, loopVarNo);
            // R
            addNewILCode(NakoILType.LD_LOCAL, lenVarNo);
            // LT
            addNewILCode(NakoILType.LT);
            // IF BRANCH FALSE
            addNewILCode(NakoILType.BRANCH_FALSE, label_for_end);
            // 反復する値を変数「対象」にセット
            // ** 対象=値\(ループカウンタ)
            addNewILCode(NakoILType.LD_LOCAL, valueVarNo);
            addNewILCode(NakoILType.LD_LOCAL, loopVarNo);
            addNewILCode(NakoILType.LD_ELEM_REF);
            //addNewILCode(NakoILType.NOP, "let-taisyou"); // for DEBUG
            addNewILCode(NakoILType.ST_LOCAL, taisyouVarNo);
            // ** 回数=ループカウンタ+1
            addNewILCode(NakoILType.LD_LOCAL, loopVarNo);
            addNewILCode(NakoILType.INC);
            addNewILCode(NakoILType.ST_LOCAL, kaisuVarNo);

            // (3) 繰り返し文を実行する
            _loop_check_break_continue(node.nodeBlocks, label_for_end, label_for_begin);
            Write_r(node.nodeBlocks);

            // (4) 変数を加算する
            addNewILCode(NakoILType.INC_LOCAL, loopVarNo);

            // (5) 手順2に戻る
            result.Add(createJUMP(label_for_cond));
            result.Add(label_for_end);
        }

        private void _let(NakoNodeLet node)
        {
            NakoNodeVariable varNode = node.VarNode;
            NakoNode valueNode = node.ValueNode;
            
            // 配列要素があるか確認
            if (!varNode.useElement)
            {
                // + 要素なしの代入処理
                // - 代入する値を書き込んで...
                Write_r(valueNode);
                // - セットする
                NakoILCode st = new NakoILCode();
                st.value = varNode.varNo;
                st.type = (varNode.scope == NakoVariableScope.Global)
                    ? NakoILType.ST_GLOBAL
                    : NakoILType.ST_LOCAL;
                result.Add(st);
            }
            else // 配列要素があるとき 
            {
                // + 配列への代入処理
                // - 基本となる変数をセット
                NakoILCode ldvar = new NakoILCode();
                ldvar.value = varNode.varNo;
                ldvar.type = (varNode.scope == NakoVariableScope.Global)
                    ? NakoILType.LD_GLOBAL_REF
                    : NakoILType.LD_LOCAL_REF;
                result.Add(ldvar);
                // - アクセス要素をセット
                int count = varNode.Children.Count;
                for (int i = 0; i < count; i++)
                {
                    NakoNode n = varNode.Children[i];
                    Write_r(n); // ノードの値

                    if (i < count - 1)
                    {
                        result.Add(new NakoILCode(NakoILType.LD_ELEM_REF)); // 要素
                    }
                    else
                    {
                        // 値ノード
                        Write_r(valueNode);
                        addNewILCode(NakoILType.ST_ELEM);
                    }
                }
            }
        }

        private void _setVariable(NakoNodeVariable node)
        {
            // _let() で処理されるのでここでは何もしない
        }

        /// <summary>
        /// ILコードのタイプを参照型に直す
        /// </summary>
        /// <param name="c">変更したいコード</param>
        private void _varBy_change_ref(NakoILCode c)
        {
            switch (c.type)
            {
                case NakoILType.LD_LOCAL: c.type = NakoILType.LD_LOCAL_REF; break;
                case NakoILType.LD_GLOBAL: c.type = NakoILType.LD_GLOBAL_REF; break;
                case NakoILType.LD_ELEM: c.type = NakoILType.LD_ELEM_REF; break;
            }
        }


        private void _getVariable(NakoNodeVariable node)
        {
            NakoILCode ld = new NakoILCode();
            if (!node.useElement)
            {
                // + 変数アクセス
                ld.type = (node.scope == NakoVariableScope.Global)
                    ? NakoILType.LD_GLOBAL
                    : NakoILType.LD_LOCAL;
                if (node.varBy == VarByType.ByRef) _varBy_change_ref(ld);
                ld.value = node.varNo;
                result.Add(ld);
            }
            else
            {
                // + 配列変数アクセス
                // - 変数
                ld.type = (node.scope == NakoVariableScope.Global)
                    ? NakoILType.LD_GLOBAL
                    : NakoILType.LD_LOCAL;
                ld.value = node.varNo;
                result.Add(ld);
                // - 要素
                NakoNodeList c = node.Children;
                for (int i = 0; i < c.Count; i++)
                {
                    Write_r(c[i]);
                    NakoILCode code = new NakoILCode();
                    /*
                    code.type = ((c.Count - 1) == i)
                        ? NakoILType.LD_ELEM
                        : NakoILType.LD_ELEM_REF;
                     */
                    code.type = NakoILType.LD_ELEM;
                    result.Add(code);
                }
            }
        }

        private void _print(NakoNode node)
        {
            NakoNode v = node.Children[0];
            Write_r(v);
            result.Add(new NakoILCode(NakoILType.PRINT, null));
        }

        private void newCalc(NakoNodeCalc node)
        {
            NakoILCode c = new NakoILCode();
            // 
            Write_r(node.nodeL);
            Write_r(node.nodeR);
            //
            switch (node.calc_type)
            {
                case CalcType.NOP: c.type = NakoILType.NOP; break; // ( ... )
                case CalcType.ADD: c.type = NakoILType.ADD; break;
                case CalcType.SUB: c.type = NakoILType.SUB; break;
                case CalcType.MUL: c.type = NakoILType.MUL; break;
                case CalcType.DIV: c.type = NakoILType.DIV; break;
                case CalcType.MOD: c.type = NakoILType.MOD; break;
                case CalcType.ADD_STR: c.type = NakoILType.ADD_STR; break;
                case CalcType.POWER: c.type = NakoILType.POWER; break;
                case CalcType.EQ: c.type = NakoILType.EQ; break;
                case CalcType.NOT_EQ: c.type = NakoILType.NOT_EQ; break;
                case CalcType.GT: c.type = NakoILType.GT; break;
                case CalcType.GT_EQ: c.type = NakoILType.GT_EQ; break;
                case CalcType.LT: c.type = NakoILType.LT; break;
                case CalcType.LT_EQ: c.type = NakoILType.LT_EQ; break;
                case CalcType.AND: c.type = NakoILType.AND; break;
                case CalcType.OR: c.type = NakoILType.OR; break;
                case CalcType.XOR: c.type = NakoILType.XOR; break;
                case CalcType.NEG: c.type = NakoILType.NEG; break;
            }
            result.Add(c);
        }

        private void _call_function(NakoNodeCallFunction node)
        {
            // push args values
            if (node.argNodes != null)
            {
                for (int i = 0; i < node.argNodes.Count; i++)
                {
                    Write_r(node.argNodes[i]);
                }
            }
            NakoILCode code = new NakoILCode();
            if (node.func.funcType == Function.NakoFuncType.SysCall)
            {
                // NakoAPIFunc f = (NakoAPIFunc)node.func;
                code.type = NakoILType.SYSCALL;
                code.value = node.func.varNo;
                result.Add(code);
            }
            else // UserCall
            {
                NakoILCode defLabel = ((NakoNodeDefFunction)node.value).defLabel;
                code.type = NakoILType.USRCALL;
                code.value = defLabel;
                result.Add(code);
            }
        }

        private void _def_function(NakoNodeDefFunction node)
        {
            // 必要なラベルを定義
			NakoILCode end_of_def_func = createLABEL("END_OF_DEF_FUNC_" + node.func.name);
			NakoILCode ret_of_def_func = createLABEL("RETURN_OF_DEF_FUNC_" + node.func.name);
            NakoILCode begin_def_func = createLABEL("FUNC_" + node.func.name);
            node.defLabel = begin_def_func;
            
            // 関数の定義は実行しないので、end_of_def_func へ飛ぶ
            result.Add(createJUMP(end_of_def_func));
            // 関数の始まりラベルを定義
            result.Add(begin_def_func);
            // 引数をPOPする処理
            NakoFunc func = node.func;
            for (int i = 0; i < func.ArgCount; i++)
            {
                NakoFuncArg arg = func.args[i];
                NakoILCode c = new NakoILCode(NakoILType.ST_LOCAL, i);
                result.Add(c);
            }
            // 本文を定義
            _func_check_return(node.funcBody, ret_of_def_func);
            Write_r(node.funcBody);
            // 戻り値(変数「それ」)をスタックに載せる
            result.Add(ret_of_def_func);
            result.Add(new NakoILCode(NakoILType.LD_GLOBAL, (int)0));
			result.Add(new NakoILCode(NakoILType.RET, func.updateSore));
            // 関数の終わりを定義
            result.Add(end_of_def_func);
        }
    }
    
    /// <summary>
    /// 中間コードの書き出しエラークラス
    /// </summary>
    public class NakoILWriterException : ApplicationException
    {
        /// <summary>
        /// 中間コードの書き出しエラーを出す
        /// </summary>
        /// <param name="message"></param>
        public NakoILWriterException(string message) : base(message) { }
    }
}
