using System;
using System.Collections.Generic;
using System.Text;
using Libnako.JPNCompiler.Function;
using Libnako.JPNCompiler.Parser;
using Libnako.JPNCompiler.ILWriter;
using Libnako.Interpreter.ILCode;

namespace Libnako.JPNCompiler.Node
{
    /// <summary>
    /// 関数定義ノード
    /// </summary>
    public class NakoNodeDefFunction : NakoNode
    {
        /// <summary>
        /// 関数名
        /// </summary>
        public string funcName { get; set; }
        /// <summary>
        /// 関数オブジェクト
        /// </summary>
        public NakoFunc func { get; set; }
        /// <summary>
        /// ローカル変数
        /// </summary>
        public NakoVariableManager localVar { get; set; }
        /// <summary>
        /// 関数本体のノード
        /// </summary>
        public NakoNode funcBody { get; set; }
        /// <summary>
        /// 定義ラベルへのリンクコード
        /// </summary>
        public NakoILCode defLabel { get; set; }
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public NakoNodeDefFunction()
        {
            type = NakoNodeType.DEF_FUNCTION;
            localVar = new NakoVariableManager(NakoVariableScope.Local);
        }

        /// <summary>
        /// 引数をローカル変数として定義する
        /// </summary>
        public void RegistArgsToLocalVar(NakoVariableManager globalVar)
        {
            for (int i = 0; i < func.args.Count; i++)
            {
                NakoFuncArg arg = func.args[i];
                NakoPlugin.NakoVariable var = new NakoPlugin.NakoVariable ();
                if (arg.type != null) {
                    var.SetBody(null, NakoPlugin.NakoVarType.Instance);
                    var.InstanceType = arg.type;
                }
                localVar.CreateVar(arg.name, var);
            }
        }

    }

    /// <summary>
    /// 関数の定義リスト
    /// </summary>
    public class NakoNodeDefFunctionList : IList<NakoNodeDefFunction>
    {
        private List<NakoNodeDefFunction> _list = new List<NakoNodeDefFunction>();

        #region IList<NakoNodeDefFunction> メンバー

        /// <summary>
        /// 検索
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int IndexOf(NakoNodeDefFunction item)
        {
            return _list.IndexOf(item);
        }

        /// <summary>
        /// 挿入
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        public void Insert(int index, NakoNodeDefFunction item)
        {
            _list.Insert(index, item);
        }

        /// <summary>
        /// 削除
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
        }

        /// <summary>
        /// 要素を得る
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public NakoNodeDefFunction this[int index]
        {
            get
            {
                return _list[index];
            }
            set
            {
                _list[index] = value;
            }
        }

        #endregion

        #region ICollection<NakoNodeDefFunction> メンバー

        /// <summary>
        /// 追加
        /// </summary>
        /// <param name="item"></param>
        public void Add(NakoNodeDefFunction item)
        {
            _list.Add(item);
        }
        /// <summary>
        /// 削除
        /// </summary>
        public void Clear()
        {
            _list.Clear();
        }
        /// <summary>
        /// 含む
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(NakoNodeDefFunction item)
        {
            return _list.Contains(item);
        }
        /// <summary>
        /// コピー
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(NakoNodeDefFunction[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }
        /// <summary>
        /// 個数
        /// </summary>
        public int Count
        {
            get { return _list.Count; }
        }

        bool ICollection<NakoNodeDefFunction>.IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }
        /// <summary>
        /// 削除
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(NakoNodeDefFunction item)
        {
            return _list.Remove(item);
        }

        #endregion

        #region IEnumerable<NakoNodeDefFunction> メンバー

        /// <summary>
        /// 列挙
        /// </summary>
        /// <returns></returns>
        public IEnumerator<NakoNodeDefFunction> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        #endregion

        #region IEnumerable メンバー

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        #endregion
    }
}

