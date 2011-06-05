using System;
using System.Collections.Generic;
using System.Text;
using Libnako.JPNCompiler.Tokenizer;

namespace Libnako.JPNCompiler.Node
{
    /// <summary>
    /// なでしこの構文ノードを表わすクラス
    /// </summary>
    public class NakoNode
    {
        /// <summary>
        /// 構文ノードタイプ
        /// </summary>
		public NakoNodeType type { get; set; }
        /// <summary>
        /// ノードの値
        /// </summary>
		public Object value { get; set; }
        private String _josi;
        /// <summary>
        /// 助詞
        /// </summary>
        public String josi
        {
            set { _josi = value; }
            get { return getJosi(); }
        }
        /// <summary>
        /// 子ノード
        /// </summary>
        public NakoNodeList Children
        {
            get { return children; }
        }
        /// <summary>
        /// 子ノード(内部使用)
        /// </summary>
        protected NakoNodeList children = null;
        /// <summary>
        /// 子ノードがあるか
        /// </summary>
        /// <returns></returns>
        public Boolean hasChildren()
        {
            if (children == null) return false;
            if (children.Count == 0) return false;
            return true;
        }
        private NakoToken token = null;
        
        /// <summary>
        /// 助詞を取得する
        /// </summary>
        /// <returns></returns>
        protected String getJosi()
        {
            return _josi;
        }

        /// <summary>
        /// 対応するトークンを返す
        /// </summary>
        public NakoToken Token
        {
            get { return this.token; }
            set
            {
                this.token = value;
                this.josi = token.josi;
            }
        }

        /// <summary>
        /// ノードのコンストラクタ
        /// </summary>
        public NakoNode()
        {
			type = NakoNodeType.NOP;
        }
        /// <summary>
        /// ノードのコンストラクタ
        /// </summary>
        /// <param name="value"></param>
        public NakoNode(Object value)
        {
			type = NakoNodeType.NOP;
			this.value = value;
        }
        /// <summary>
        /// 子ノードを追加
        /// </summary>
        /// <param name="child"></param>
        public void AddChild(NakoNode child)
        {
            if (children == null)
            {
                children = new NakoNodeList();
            }
            children.Add(child);
        }
        /// <summary>
        /// タイプ文字列
        /// </summary>
        /// <returns></returns>
        public virtual String ToTypeString()
        {
            String r = type.ToString();
            if (hasChildren())
            {
                r += "\n";
                for (int i = 0; i < children.Count; i++)
                {
                    r += "  |-- " + children[i].ToTypeString() + "\n";
                }
            }
            return r;
        }

    }
}
