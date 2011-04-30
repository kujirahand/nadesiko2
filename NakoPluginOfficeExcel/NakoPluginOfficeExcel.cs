using System;
using System.Collections.Generic;
using System.Text;

using NakoPlugin;
using NakoExcel = Microsoft.Office.Interop.Excel;
using VBIDE = Microsoft.Vbe.Interop;

using System.Windows.Forms; //動作チェック用のダイアログ表示に使用

namespace NakoPluginOfficeExcel
{
    public class NakoPluginOfficeExcel : INakoPlugin
    {
        //--- プラグインの宣言 ---
        string _description = "エクセル命令 by 粗茶";
        double _version = 1.0;  //2011.1.7より作業開始
        //--- プラグイン共通の部分 ---
        public double TargetNakoVersion { get { return 2.0; } }
        public bool Used { get; set; }
        public string Name { get { return this.GetType().FullName; } }
        public double PluginVersion { get { return _version; } }
        public string Description { get { return _description; } }
        //--- プラグインの初期化処理 ---
        public void PluginInit(INakoInterpreter runner) { }
        //--- プラグインの終了処理 ---
        public void PluginFin(INakoInterpreter runner) { NakoExcelEnd(); }
        //--- 変数の定義 ---
        NakoExcel._Application xlApp;    //Excelアプリケーション
        NakoExcel.Workbooks xlBooks;
        NakoExcel._Workbook xlBook;
        NakoExcel.Sheets xlSheets;
        NakoExcel._Worksheet xlSheet;
        //NakoExcel.Range xlRange;
        //VBIDE.VBComponents xlModules;

        //--- 関数の定義 ---
        public void DefineFunction(INakoPluginBank bank)
        {
            //エクセル
            #region アプリケーション関係
            //+アプリケーション
            bank.AddFunc("エクセル起動", "FLAGで|FLAGに|FLAGへ", NakoVarType.Void, _xlStart, "エクセルを起動する。FLAG=1:可視、FLAG=0:不可視で起動。", "えくせるきどう");
            bank.AddFunc("エクセル終了", "", NakoVarType.Void, _xlEnd, "エクセルを終了する。", "えくせるしゅうりょう");
            bank.AddFunc("エクセル起動状態", "", NakoVarType.Int, _xlStarted, "エクセルが起動しているか確認する。1:起動している。0:起動していない。", "えくせるきどうじょうたい");
            bank.AddFunc("エクセルバージョン", "", NakoVarType.String, _xlVer, "エクセルのバージョンを取得して返す。", "えくせるばーじょん");
            bank.AddFunc("エクセル可視変更", "FLAGで|FLAGに|FLAGへ", NakoVarType.Void, _xlVisibleSet, "エクセルの可視を変更する。FLAG=1:可視、FLAG=0:不可視", "えくせるかしへんこう");
            bank.AddFunc("エクセル可視状態", "", NakoVarType.Int, _xlVisibleGet, "エクセルの可視を取得して返す。1:可視、0:不可視。", "えくせるかしじょうたい");
            bank.AddFunc("エクセル警告変更", "FLAGで|FLAGに|FLAGへ", NakoVarType.Void, _xlAlertSet, "エクセルの警告を変更する。FLAG=1:警告有り、FLAG=0:警告無視。", "えくせるけいこくへんこう");
            bank.AddFunc("エクセル警告状態", "", NakoVarType.Int, _xlAlertGet, "エクセルの警告を取得して返す。1:警告有り、0:警告無視。", "えくせるけいこくじょうたい");
            bank.AddFunc("エクセル窓変更", "STATEで|STATEに|STATEへ", NakoVarType.Void, _xlWindowStateSet, "エクセルのウインドウを最大化・最小化する。STATE=「最大化|最小化|標準」", "えくせるまどへんこう");
            bank.AddFunc("エクセル窓状態", "", NakoVarType.String, _xlWindowStateGet, "エクセルのウインドウ状態を取得して返す。「最大化|最小化|標準」", "えくせるまどじょうたい");
            bank.AddFunc("エクセルタイトル変更", "TITLEで|TITLEに|TITLEへ", NakoVarType.Void, _xlTitleSet, "エクセルのウインドウタイトルをTITLEに変更する。", "えくせるたいとるへんこう");
            bank.AddFunc("エクセルタイトル状態", "", NakoVarType.String, _xlTitleGet, "エクセルのウインドウタイトルを取得して返す。", "えくせるたいとるじょうたい");
            bank.AddFunc("エクセルステータスバー変更", "MSGで|MSGに|MSGへ", NakoVarType.Void, _xlStatusbarTextSet, "エクセルのステータスバーの内容をMSGに変更する。", "えくせるすてーたすばーへんこう");
            bank.AddFunc("エクセルステータスバー状態", "", NakoVarType.String, _xlStatusbarTextGet, "エクセルのステータスバーの内容を取得して返す。", "えくせるすてーたすばーじょうたい");
            bank.AddFunc("エクセルステータスバー標準", "", NakoVarType.Void, _xlStatusbarTextNormal, "エクセルのステータスバーの内容を元に戻す。", "えくせるすてーたすばーひょうじゅん");
            bank.AddFunc("エクセルステータスバー可視変更", "FLAGで|FLAGに|FLAGへ", NakoVarType.Void, _xlStatusbarVisibleSet, "エクセルのステータスバーの表示を変更する。FLAG=1:表示、FLAG=0:非表示。", "えくせるすてーたすばーかしへんこう");
            bank.AddFunc("エクセルステータスバー可視状態", "", NakoVarType.Int, _xlStatusbarVisibleGet, "エクセルのステータスバーの表示状態を取得して返す。1:表示、0:非表示。", "えくせるすてーたすばーかしじょうたい");
            bank.AddFunc("エクセル枠線表示変更", "FLAGで|FLAGに|FLAGへ", NakoVarType.Void, _xlGridlineVisibleSet, "エクセルのセル枠線の表示を変更する。FLAG=1:表示、FLAG=0:非表示。", "えくせるわくせんひょうじへんこう");
            bank.AddFunc("エクセル枠線表示状態", "", NakoVarType.Int, _xlGridlineVisibleGet, "エクセルのセル枠線の表示を取得して返す。1:表示、0:非表示。", "えくせるわくせんひょうじじょうたい");

            #endregion
            #region ブック関係
            //+ブック
            bank.AddFunc("エクセルブック追加", "", NakoVarType.Void, _xlBookAdd, "エクセルに新規ブックを追加。", "えくせるぶっくついか");
            bank.AddFunc("エクセルブック開く", "FILEを|FILEで|FILEから", NakoVarType.Void, _xlBookOpen, "エクセルにFILE(ブックのパス)を開く。", "えくせるぶっくひらく");
            bank.AddFunc("エクセルブック保存", "FILEを|FILEで|FILEに|FILEへ", NakoVarType.Void, _xlBookSaveAs, "", "");

            #endregion
            /*--- todo ---
             * ブック一覧
             * 現在ブック
             * ブック保護設定
             * ブック保護解除
             * バックアップ */
        }
        //--- Define Method ---
        #region アプリケーション関係
        //エクセル起動
        public Object _xlStart(INakoFuncCallInfo info)
        {
            if (xlApp == null)
            {
                long arg = info.StackPopAsInt();
                xlApp = new NakoExcel.Application { Visible = (arg != 0) }; //arg:0以外はtrue
                xlBooks = xlApp.Workbooks;
            }
            return null;
        }
        //エクセル終了
        public Object _xlEnd(INakoFuncCallInfo info)
        {
            NakoExcelEnd();
            return null;
        }
        void NakoExcelEnd()
        {
            if (xlApp != null)
            {
                xlApp.DisplayAlerts = false;
                xlApp.Quit();   //Excel終了
                xlApp = null;
                //GC.Collect();
            }
        }
        //エクセル起動状態
        public Object _xlStarted(INakoFuncCallInfo info)
        {
            int i = (xlApp != null) ? 1 : 0;
            return i;
        }
        //エクセルバージョン
        public Object _xlVer(INakoFuncCallInfo info)
        {
            if (xlApp != null) return xlApp.Version;
            return "-1";    //Excelが起動していない
        }
        //エクセル可視変更
        public Object _xlVisibleSet(INakoFuncCallInfo info)
        {
            if (xlApp != null)
            {
                long arg = info.StackPopAsInt();
                xlApp.Visible = (arg != 0); //arg:0以外はtrue
            }
            return null;
        }
        //エクセル可視状態
        public Object _xlVisibleGet(INakoFuncCallInfo info)
        {
            int i = (xlApp != null && xlApp.Visible == true) ? 1 : 0;
            return i;
        }
        //エクセル警告変更
        public Object _xlAlertSet(INakoFuncCallInfo info)
        {
            if (xlApp != null)
            {
                long arg = info.StackPopAsInt();
                xlApp.DisplayAlerts = (arg != 0);   //arg:0以外はtrue
            }
            return null;
        }
        //エクセル警告状態
        public Object _xlAlertGet(INakoFuncCallInfo info)
        {
            int i = (xlApp != null && xlApp.DisplayAlerts == true) ? 1 : 0;
            return i;
        }
        //エクセル窓変更
        public Object _xlWindowStateSet(INakoFuncCallInfo info)
        {
            if (xlApp != null)
            {
                string arg = info.StackPopAsString();
                NakoExcel.XlWindowState state = NakoExcel.XlWindowState.xlNormal;
                switch (arg)
                {
                    case "最大化":
                        state = NakoExcel.XlWindowState.xlMaximized;
                        break;
                    case "最小化":
                        state = NakoExcel.XlWindowState.xlMinimized;
                        break;
                    case "標準":
                        state = NakoExcel.XlWindowState.xlNormal;
                        break;
                    default:
                        state = xlApp.WindowState;  //現在の状態
                        //throw new NakoPluginRuntimeException("エクセル窓変更の引数に「最大化|最小化|標準」以外の文字列が指定されました。");
                        break;
                }
                xlApp.WindowState = state;
            }
            return null;
        }
        //エクセル窓状態
        public Object _xlWindowStateGet(INakoFuncCallInfo info)
        {
            if (xlApp != null)
            {
                NakoExcel.XlWindowState state = xlApp.WindowState;
                switch (state)
                {
                    case NakoExcel.XlWindowState.xlMaximized:
                        return "最大化";
                    case NakoExcel.XlWindowState.xlMinimized:
                        return "最小化";
                    case NakoExcel.XlWindowState.xlNormal:
                        return "標準";
                }
            }
            return "-1";  //Excelが起動していない
        }
        //タイトル変更
        public Object _xlTitleSet(INakoFuncCallInfo info)
        {
            if (xlApp != null && xlBooks != null)
            {
                xlApp.ActiveWindow.Caption = info.StackPopAsString();
            }
            return null;
        }
        //タイトル状態
        public Object _xlTitleGet(INakoFuncCallInfo info)
        {
            if (xlApp != null && xlBooks != null)
            {
                return xlApp.ActiveWindow.Caption;
            }
            return "-1";    //Excelが起動していない or Bookがひとつもない
        }
        //ステータスバー変更
        public Object _xlStatusbarTextSet(INakoFuncCallInfo info)
        {
            if (xlApp != null)
            {
                string arg = info.StackPopAsString();
                xlApp.StatusBar = arg;
            }
            return null;
        }
        //ステータスバー状態
        public Object _xlStatusbarTextGet(INakoFuncCallInfo info)
        {
            if (xlApp != null) { return xlApp.StatusBar; }
            return "";
        }
        //ステータスバー標準
        public Object _xlStatusbarTextNormal(INakoFuncCallInfo info)
        {
            if (xlApp != null)
            {
                xlApp.StatusBar = false;
            }
            return null;
        }
        //ステータスバー可視変更
        public Object _xlStatusbarVisibleSet(INakoFuncCallInfo info)
        {
            if (xlApp != null)
            {
                long arg = info.StackPopAsInt();
                xlApp.DisplayStatusBar = (arg != 0);    //arg:0以外はtrue
            }
            return null;
        }
        //ステータスバー可視状態
        public Object _xlStatusbarVisibleGet(INakoFuncCallInfo info)
        {
            int i = (xlApp != null && xlApp.DisplayStatusBar == true) ? 1 : 0;
            return i;
        }
        //枠線表示変更
        public Object _xlGridlineVisibleSet(INakoFuncCallInfo info)
        {
            if (xlApp != null && xlBooks != null)
            {
                long arg = info.StackPopAsInt();
                xlApp.ActiveWindow.DisplayGridlines = (arg != 0);   //arg:0以外はtrue
            }
            return null;
        }
        //枠線表示状態
        public Object _xlGridlineVisibleGet(INakoFuncCallInfo info)
        {
            int i = (xlApp != null && xlBooks != null && xlApp.ActiveWindow.DisplayGridlines == true) ? 1 : 0;
            return i;
        }
        //マクロ実行
        //キー送信
        //キー送信待機
        #endregion
        #region ブック関係
        //ブック追加
        public Object _xlBookAdd(INakoFuncCallInfo info)
        {
            if (xlApp != null)
            {
                xlBooks.Add();
                AfterBookOpen();
            }
            return null;
        }
        //ブック開く
        public Object _xlBookOpen(INakoFuncCallInfo info)
        {
            if (xlApp != null)
            {
                string filename = info.StackPopAsString();
                if (!System.IO.File.Exists(filename))
                { throw new NakoPluginRuntimeException("ファイル『" + filename + "』は存在しません。"); }
                xlBooks.Open(filename);
                AfterBookOpen();
            }
            return null;
        }
        void AfterBookOpen()
        {
            xlBook = xlBooks[xlBooks.Count];
            xlBook.Activate();
            xlSheets = xlBook.Worksheets;
            xlSheet = (NakoExcel._Worksheet)xlSheets[1];
            xlSheet.Activate();
        }
        //ブック保存
        public Object _xlBookSaveAs(INakoFuncCallInfo info)
        {
            if (xlApp != null && xlBook != null)
            {
                bool flag = xlApp.DisplayAlerts;
                string filename = info.StackPopAsString();
                object format = NakoExcel.XlFileFormat.xlWorkbookNormal;
                xlApp.DisplayAlerts = false;
                if (filename == "")
                {
                    xlBook.Save();  //ファイル名指定なければ上書き保存
                    xlApp.DisplayAlerts = flag;
                    return null;
                }
                string ext = System.IO.Path.GetExtension(filename);
                ext = ext.ToLower();
                switch (ext)
                {
                    case ".xlsx":
                        if (double.Parse(xlApp.Version) < 12.0)
                        { throw new NakoPluginRuntimeException("Excel2007以上でなければ拡張子.xlsx形式で保存できません。"); }
                        //マクロを含んだBookをxlsx形式で保存すれば、マクロを削除して保存する仕様です。
                        //次の2行のコメントを外すと、自動的にxlsm形式(マクロあり)で保存します。
                        //xlModules = xlBook.VBProject.VBComponents;
                        //if (xlModules != null) goto case ".xlsm";   //マクロを含んでいれば.xlsm
                        format = NakoExcel.XlFileFormat.xlOpenXMLWorkbook;
                        break;
                    case ".xls":
                        if (double.Parse(xlApp.Version) < 12.0) { format = NakoExcel.XlFileFormat.xlExcel9795; }
                        else { format = NakoExcel.XlFileFormat.xlExcel8; }  //Excel2007以上の場合
                        break;
                    case ".xlsm":
                        if (double.Parse(xlApp.Version) < 12.0)
                        { throw new NakoPluginRuntimeException("Excel2007以上でなければ拡張子.xlsm形式で保存できません。"); }
                        format = NakoExcel.XlFileFormat.xlOpenXMLWorkbookMacroEnabled;
                        ext = ".xlsm";
                        break;
                    case ".csv":
                        format = NakoExcel.XlFileFormat.xlCSV;
                        break;
                    case ".txt":
                        format = NakoExcel.XlFileFormat.xlUnicodeText;
                        break;
                    case ".pdf":
                        if (double.Parse(xlApp.Version) < 12.0)
                        { throw new NakoPluginRuntimeException("Excel2007以上でなければ拡張子.pdf形式で保存できません。"); }
                        break;
                    default:
                        throw new NakoPluginRuntimeException("保存形式が無効です。");
                }
                filename = System.IO.Path.ChangeExtension(filename, ext);
                if (ext == ".pdf") { xlSheet.ExportAsFixedFormat(NakoExcel.XlFixedFormatType.xlTypePDF, filename); }
                else { xlBook.SaveAs(filename, format); }
                xlApp.DisplayAlerts = flag;
            }
            return null;
        }

        //ブックを閉じる時には、xlBooks[1]をアクティブにする
        //ブックが何もなければ、xlBookはnull
        #endregion

        /*
         * --- エラー処理はexceptionを投げること ---
         * エラーでも実行し続けるのは危険！なので必ずエラーメッセージを表示すること。
         * --- マクロを扱う命令は、ユーザー側がExcelで次の設定が必要 ---
         * (Excel2002、2003の場合)
         * ツール＞マクロ＞セキュリティ＞信頼できる発行元＞Visual Basicプロジェクトへのアクセスを信頼するをオン
         * (Excel2007、2010の場合)
         * リボンの「開発」タブ＞マクロのセキュリティ＞VBAプロジェクトオブジェクトモデルへのアクセスを信頼するをオン
         * ※「プログラミングによるVisual Basicプロジェクトへのアクセスは信頼性に欠けます」というエラーメッセージが出ないようになる。
        */
    }
}
