' +------------------------------+
' | なでしこプラグイン           |
' | Excel(NakoPluginOfficeExcel) |
' +------------------------------+

''' <summary>
''' Excel用のなでしこプラグイン
''' </summary>
Public Class NakoPluginOfficeExcel
    Implements NakoPlugin.INakoPlugin

    ' ===== プラグインの宣言 =====
    ''' <summary>
    ''' プラグインのフルネーム
    ''' </summary>
    Public ReadOnly Property Name As String Implements NakoPlugin.INakoPlugin.Name
        Get
            Return Me.GetType().FullName
        End Get
    End Property
    ''' <summary>
    ''' プラグインの説明 
    ''' </summary>
    Public ReadOnly Property Description As String Implements NakoPlugin.INakoPlugin.Description
        Get
            Return "Excel 操作プラグイン"
        End Get
    End Property
    ''' <summary>
    ''' プラグインのバージョン
    ''' </summary>
    Public ReadOnly Property PluginVersion As Version Implements NakoPlugin.INakoPlugin.PluginVersion
        Get
            Return New Version(1, 0)
        End Get
    End Property
    ''' <summary>
    ''' ターゲットとするなでしこのバージョン
    ''' </summary>
    Public ReadOnly Property TargetNakoVersion As Version Implements NakoPlugin.INakoPlugin.TargetNakoVersion
        Get
            Return New Version(2, 0)
        End Get
    End Property
    ''' <summary>
    ''' Usedの実装
    ''' </summary>
    Private _used As Boolean = False
    Public Property Used As Boolean Implements NakoPlugin.INakoPlugin.Used
        Get
            Return _used
        End Get
        Set(ByVal value As Boolean)
            _used = value
        End Set
    End Property

    ' ===== 変数の定義 =====
    Private oXLS As ExcelLateWrapper ' 遅延バインディング

    ' ===== なでしこ関数の定義 =====
    ''' <summary>
    ''' なでしこ関数の定義
    ''' </summary>
    ''' <param name="bank">定義用引数</param>
    Public Sub DefineFunction(ByVal bank As NakoPlugin.INakoPluginBank) Implements NakoPlugin.INakoPlugin.DefineFunction
        ' アプリケーション
        bank.AddFunc("エクセルインストールチェック", "", NakoPlugin.NakoVarType.Int, AddressOf _xlInstallCheck, "Excelがインストールされているか調べる。(1:インストールされている、0:インストールされていない)", "えくせるいんすとーるちぇっく")
        bank.AddFunc("エクセル起動", "FLAGで|FLAGに|FLAGへ", NakoPlugin.NakoVarType.Void, AddressOf _xlNew, "Excelを起動する。(FLAG=1:可視、FLAG=0:不可視で起動)", "えくせるきどう")
        bank.AddFunc("エクセル終了", "", NakoPlugin.NakoVarType.Void, AddressOf _xlClose, "Excelを終了する。ブックは保存しない。", "えくせるしゅうりょう")
        bank.AddFunc("エクセル起動取得", "", NakoPlugin.NakoVarType.Int, AddressOf _xlStarted, "Excelが起動しているか調べる。(1:起動している、0:起動していない)", "えくせるきどうしゅとく")
        bank.AddFunc("エクセルバージョン取得", "", NakoPlugin.NakoVarType.Int, AddressOf _xlVersion, "Excelのバージョンを取得する。", "えくせるばーじょんしゅとく")
        bank.AddFunc("エクセル可視設定", "FLAGで|FLAGに|FLAGへ", NakoPlugin.NakoVarType.Void, AddressOf _xlVisibleSet, "Excelの可視を設定する。(FLAG=1:可視、FLAG=0:不可視)", "えくせるかしせってい")
        bank.AddFunc("エクセル可視取得", "", NakoPlugin.NakoVarType.Int, AddressOf _xlVisibleGet, "Excelの可視を取得する。(1:可視、0:不可視で起動)", "えくせるかししゅとく")
        bank.AddFunc("エクセル警告設定", "FLAGで|FLAGに|FLAGへ", NakoPlugin.NakoVarType.Void, AddressOf _xlDisplayAlertsSet, "Excelの警告表示を設定する。(FLAG=1:警告あり、FLAG=0:警告なし)", "えくせるけいこくせってい")
        bank.AddFunc("エクセル警告取得", "", NakoPlugin.NakoVarType.Int, AddressOf _xlDisplayAlertsGet, "Excelの警告表示を取得する。(1:警告あり、0:警告なし)", "えくせるけいこくしゅとく")
        bank.AddFunc("エクセル窓設定", "STATEで|STATEに|STATEへ", NakoPlugin.NakoVarType.Void, AddressOf _xlWindowStateSet, "Excelのウィンドウ状態を設定する。(STATE=『最大化|最小化|標準』)", "えくせるまどせってい")
        bank.AddFunc("エクセル窓取得", "", NakoPlugin.NakoVarType.String, AddressOf _xlWindowStateGet, "Excelのウィンドウ状態を取得する。(『最大化|最小化|標準』)", "えくせるまどしゅとく")
        bank.AddFunc("エクセルタイトル設定", "TITLEで|TITLEに|TITLEへ", NakoPlugin.NakoVarType.Void, AddressOf _xlApplicationCaptionSet, "ExcelのウィンドウタイトルをTITLEに設定する。", "えくせるたいとるせってい")
        bank.AddFunc("エクセルタイトル取得", "", NakoPlugin.NakoVarType.String, AddressOf _xlApplicationCaptionGet, "Excelのウィンドウタイトルを取得する。", "えくせるたいとるしゅとく")
        bank.AddFunc("エクセルステータスバー設定", "STRINGで|STRINGに|STRINGへ", NakoPlugin.NakoVarType.Void, AddressOf _xlStatusBarSet, "ExcelのステータスバーをSTRINGに設定する。STRING=「」の時、ステータスバーを標準にする。", "えくせるすてーたすばーせってい")
        bank.AddFunc("エクセルステータスバー取得", "", NakoPlugin.NakoVarType.String, AddressOf _xlStatusBarGet, "Excelのステータスバーを取得する。", "えくせるすてーたすばーしゅとく")
        bank.AddFunc("エクセルステータスバー可視設定", "FLAGで|FLAGに|FLAGへ", NakoPlugin.NakoVarType.Void, AddressOf _xlDisplayStatusBarSet, "Excelのステータスバーの可視を設定する。(FLAG=1:可視、FLAG=0:不可視)", "えくせるすてーたすばーかしせってい")
        bank.AddFunc("エクセルステータスバー可視取得", "", NakoPlugin.NakoVarType.String, AddressOf _xlDisplayStatusBarGet, "Excelのステータスバーの可視を取得する。", "えくせるすてーたすばーかししゅとく")
        bank.AddFunc("エクセル画面更新設定", "FLAGで|FLAGに|FLAGへ", NakoPlugin.NakoVarType.Void, AddressOf _xlScreenUpdatingSet, "Excelの画面更新を設定する。(FLAG=1:画面更新あり、FLAG=0:画面更新なし)", "えくせるがめんこうしんせってい")
        bank.AddFunc("エクセル画面更新取得", "", NakoPlugin.NakoVarType.Int, AddressOf _xlScreenUpdatingGet, "Excelの画面更新を取得する。(1:画面更新あり、0:画面更新なし)", "えくせるがめんこうしんしゅとく")

        ' マクロ作成
        ' マクロ実行
        ' キー送信
        ' キー送信待機



        ' ブック
        bank.AddFunc("エクセルブック追加", "", NakoPlugin.NakoVarType.Void, AddressOf _xlBookAdd, "Excelに新規ブックを追加する。", "えくせるぶっくついか")
        bank.AddFunc("エクセルブック開く", "FILEを|FILEの|FILEで|FILEから", NakoPlugin.NakoVarType.Void, AddressOf _xlBookOpen, "Excelにファイル名を指定してブックを開く。", "えくせるぶっくひらく")
        bank.AddFunc("エクセルブック保存", "FILEを|FILEの|FILEで|FILEへ|FILEに", NakoPlugin.NakoVarType.Void, AddressOf _xlBookSaveAs, "ファイル名を指定して現在のブックを保存する。FILEを省略すると現在のブックを上書き保存する。", "えくせるぶっくほぞん")
        bank.AddFunc("エクセルブックマクロ可能取得", "", NakoPlugin.NakoVarType.Int, AddressOf _xlBookVbaAvailable, "アクティブブックでなでしこからマクロ(VBA)が利用できるか調べる。(1:利用可能、0:利用不可)", "えくせるぶっくまくろかのうしゅとく")
        bank.AddFunc("エクセルブック閉じる", "", NakoPlugin.NakoVarType.Void, AddressOf _xlBookClose, "アクティブブックを保存しないで閉じる。", "えくせるぶっくとじる")
        bank.AddFunc("エクセルブック保存後閉じる", "", NakoPlugin.NakoVarType.Void, AddressOf _xlBookCloseSave, "アクティブブックを上書き保存して閉じる。", "えくせるぶっくほぞんごとじる")
        bank.AddFunc("エクセルブック変更保存設定", "FLAGへ|FLAGに|FLAGで", NakoPlugin.NakoVarType.Void, AddressOf _xlBookSavedSet, "アクティブブックを変更後保存したか設定する。(FLAG=1:変更していないか変更して保存した、FLAG=0:変更したが保存していない)", "えくせるぶっくへんこうほぞんせってい")
        bank.AddFunc("エクセルブック変更保存取得", "", NakoPlugin.NakoVarType.Int, AddressOf _xlBookSavedGet, "アクティブブックを変更後保存したか取得する。(1:変更していないか変更して保存した、0:変更したが保存していない)", "えくせるぶっくへんこうほぞんしゅとく")
        bank.AddFunc("エクセルブック列挙", "", NakoPlugin.NakoVarType.String, AddressOf _xlBookListGet, "開いているブック名を列挙する。", "えくせるぶっくれっきょ")
        bank.AddFunc("エクセルブックアクティブ設定", "BOOKを|BOOKの|BOOKへ|BOOKで", NakoPlugin.NakoVarType.Void, AddressOf _xlBookActiveSet, "ブックをアクティブにする。(BOOK:ブック名)", "えくせるぶっくあくてぃぶせってい")
        bank.AddFunc("エクセルブックアクティブ取得", "", NakoPlugin.NakoVarType.String, AddressOf _xlBookActiveGet, "アクティブなブック名を取得する。", "えくせるぶっくあくてぃぶしゅとく")

        ' BOOKをエクセルブック検索
        ' PASSでFLAGにエクセルブック保護設定
        ' エクセルブック保護取得
        ' バックアップ


        ' シート
        bank.AddFunc("エクセルシート枠線可視設定", "FLAGで|FLAGに|FLAGへ", NakoPlugin.NakoVarType.Void, AddressOf _xlSheetDisplayGridlinesSet, "Excelのシートの枠線の可視を設定する。(FLAG=1:可視、FLAG=0:不可視)", "えくせるしーとわくせんかしせってい")
        bank.AddFunc("エクセルシート枠線可視取得", "", NakoPlugin.NakoVarType.Int, AddressOf _xlSheetDisplayGridLinesGet, "Excelのシートの枠線の可視を取得する。(1:可視、0:不可視)", "えくせるしーとわくせんかししゅとく")

        ' エクセルシート追加
        ' エクセルシート列挙
        ' SHEETへエクセルシートアクティブ設定
        ' エクセルシートアクティブ取得
        ' SHEETをエクセルシート検索

        ' セル
        bank.AddFunc("エクセルセルA1形式変換", "CELLSの|CELLSを|CELLSで|CELLSから", NakoPlugin.NakoVarType.String, AddressOf _xlCellA1Conversion, "「row,col」をA1形式のセル番地に変換する。(row:行番号、col:列番号)", "えくせるせるえーわんけいしきへんかん")
        bank.AddFunc("エクセルセル行番号取得", "RANGEの|RANGEを|RANGEで|RANGEから", NakoPlugin.NakoVarType.String, AddressOf _xlCellGetRow, "セル番地「RANGE」の行番号を取得する。(RANGE:A1形式のセル番地)", "えくせるせるぎょうばんこうしゅとく")
        bank.AddFunc("エクセルセル列番号取得", "RANGEの|RANGEを|RANGEで|RANGEから", NakoPlugin.NakoVarType.String, AddressOf _xlCellGetCol, "セル番地「RANGE」の列番号を取得する。(RANGE:A1形式のセル番地)", "えくせるせるれつばんこうしゅとく")
        bank.AddFunc("エクセルセル値取得", "RANGEの|RANGEを|RANGEで|RANGEから", NakoPlugin.NakoVarType.String, AddressOf _xlCellGetValue, "セル番地「RANGE」の値を取得する。範囲指定しても左上のセルのみが対象。(RANGE:A1形式のセル番地)", "えくせるせるあたいしゅとく")
        bank.AddFunc("エクセルセル値設定", "RANGEへ|RANGEにVALUEを|VALUEで", NakoPlugin.NakoVarType.Void, AddressOf _xlCellSetValue, "セル番地「RANGE」にVALUEを設定する。範囲指定しても左上のセルのみが対象。(RANGE:A1形式のセル番地)", "えくせるせるあたいせってい")
        bank.AddFunc("エクセルセル式取得", "RANGEの|RANGEを|RANGEで|RANGEから", NakoPlugin.NakoVarType.String, AddressOf _xlCellGetFormula, "セル番地「RANGE」の式を取得する。範囲指定しても左上のセルのみが対象。(RANGE:A1形式のセル番地)", "えくせるせるしきしゅとく")
        bank.AddFunc("エクセルセル式設定", "RANGEへ|RANGEにEXPを|EXPで", NakoPlugin.NakoVarType.Void, AddressOf _xlCellSetFormula, "セル番地「RANGE」に式「EXP」を設定する。範囲指定しても左上のセルのみが対象。(RANGE:A1形式のセル番地)", "えくせるせるしきせってい")
        bank.AddFunc("エクセルセル文字取得", "RANGEの|RANGEを|RANGEで|RANGEから", NakoPlugin.NakoVarType.String, AddressOf _xlCellGetText, "セル番地「RANGE」の値を文字列として取得する。値を表示形式のまま取得する。範囲指定しても左上のセルのみが対象。(RANGE:A1形式のセル番地)", "えくせるせるもじしゅとく")
        bank.AddFunc("エクセルセル文字設定", "RANGEへ|RANGEにTEXTを|TEXTで", NakoPlugin.NakoVarType.String, AddressOf _xlCellSetText, "セル番地「RANGE」に文字列「TEXT」を設定する。文字列の前に「'」が付く。範囲指定しても左上のセルのみが対象。(RANGE:A1形式のセル番地)", "えくせるせるもじせってい")
        bank.AddFunc("エクセルセルシリアル値取得", "RANGEの|RANGEを|RANGEで|RANGEから", NakoPlugin.NakoVarType.String, AddressOf _xlCellGetSerial, "セル番地「RANGE」のシリアル値を取得する。範囲指定しても左上のセルのみが対象。(RANGE:A1形式のセル番地)", "えくせるせるしりあるちしゅとく")
        bank.AddFunc("エクセルセルシリアル値設定", "RANGEへ|RANGEにVALUEを|VALUEで", NakoPlugin.NakoVarType.Void, AddressOf _xlCellSetSerial, "セル番地「RANGE」にシリアル値「VALUE」を設定する。範囲指定しても左上のセルのみが対象。(RANGE:A1形式のセル番地)", "えくせるせるしりあるちせってい")
        bank.AddFunc("エクセルセル値範囲取得", "RANGE_LTから|RANGE_LTよりRANGE_RBまで|RANGE_RBへ|RANGE_RBを|RANGE_RBの|RANGE_RBで", NakoPlugin.NakoVarType.String, AddressOf _xlCellGetValueEx, "セル番地「RANGE_LT:RANGE_RB」の値を範囲取得する。(RANGE_LT:A1形式の左上セル番地、RANGE_RB:A1形式の右下セル番地)", "えくせるせるあたいはんいしゅとく")
        bank.AddFunc("エクセルセル値範囲設定", "RANGEへ|RANGEにVALUEを|VALUEで", NakoPlugin.NakoVarType.Void, AddressOf _xlCellSetValueEx, "セル番地「RANGE」にVALUEを範囲設定する。(RANGE:A1形式のセル番地、VALUE:配列)", "えくせるせるあたいはんいせってい")
        bank.AddFunc("エクセルセル式範囲取得", "RANGE_LTから|RANGE_LTよりRANGE_RBまで|RANGE_RBへ|RANGE_RBを|RANGE_RBの|RANGE_RBで", NakoPlugin.NakoVarType.String, AddressOf _xlCellGetFormulaEx, "セル番地「RANGE_LT:RANGE_RB」の式を取得する。(RANGE_LT:A1形式の左上セル番地、RANGE_RB:A1形式の右下セル番地)", "えくせるせるしきはんいしゅとく")
        bank.AddFunc("エクセルセル式範囲設定", "RANGEへ|RANGEにEXPを|EXPで", NakoPlugin.NakoVarType.Void, AddressOf _xlCellSetFormulaEx, "セル番地「RANGE」に式「EXP」を設定する。(RANGE:A1形式のセル番地、EXP:配列)", "えくせるせるしきはんいせってい")
        bank.AddFunc("エクセルセル文字範囲取得", "RANGE_LTから|RANGE_LTよりRANGE_RBまで|RANGE_RBへ|RANGE_RBを|RANGE_RBの|RANGE_RBで", NakoPlugin.NakoVarType.String, AddressOf _xlCellGetTextEx, "セル番地「RANGE_LT:RANGE_RB」の式を取得する。(RANGE_LT:A1形式の左上セル番地、RANGE_RB:A1形式の右下セル番地)", "えくせるせるもじはんいしゅとく")
        bank.AddFunc("エクセルセル文字範囲設定", "RANGEへ|RANGEにTEXTを|TEXT", NakoPlugin.NakoVarType.Void, AddressOf _xlCellSetTextEx, "セル番地「RANGE」に文字列「TEXT」を設定する。(RANGE:A1形式のセル番地、TEXT:配列)", "えくせるせるもじはんいせってい")

        '最上行取得
        '最下行取得
        '最左列取得
        '最右列取得
        '表上端行取得
        '表下端行取得
        '表左端列取得
        '表右端列取得
        '表エリア取得
        '表サイズ取得

        'おなじ命令を定義すると、最後に定義した命令が有効となる
        'bank.AddFunc("エクセルテスト", "MSGを", NakoPlugin.NakoVarType.Void, AddressOf _xlTestSet, "", "えくせるてすと")
        'bank.AddFunc("エクセルテスト", "", NakoPlugin.NakoVarType.String, AddressOf _xlTestGet, "", "えくせるてすと")

    End Sub

    ' ===== メソッドの定義 =====
#Region "アプリケーション関係"

    Public Function _xlInstallCheck(ByVal info As NakoPlugin.INakoFuncCallInfo) As Object
        Dim rkey As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.ClassesRoot
        Dim names As String() = rkey.GetSubKeyNames()
        Dim s As String
        For Each s In names
            If s = "Excel.Application" Then Return 1
        Next
        Return 0 ' 0ならばFalse
    End Function

    Private Sub CheckExcel()
        If (oXLS Is Nothing) Then
            Throw New NakoPlugin.NakoPluginRuntimeException("Excelが起動していません。『エクセル起動』でExcelを起動して下さい。")
        End If
    End Sub

    Public Function _xlNew(ByVal info As NakoPlugin.INakoFuncCallInfo) As Object
        Dim FLAG As Integer
        FLAG = info.StackPopAsInt()
        If (oXLS Is Nothing) Then
            oXLS = New ExcelLateWrapper()
        End If
        oXLS.Visible = ItoB(FLAG) ' 0ならばFalse
        Return (Nothing)
    End Function

    Public Function _xlClose(ByVal info As NakoPlugin.INakoFuncCallInfo) As Object
        _xlEnd()
        Return Nothing
    End Function

    Public Sub _xlEnd()
        If (oXLS IsNot Nothing) Then
            oXLS.BooksClose()
            oXLS.Dispose()
        End If
        oXLS = Nothing
    End Sub

    Public Function _xlStarted(ByVal info As NakoPlugin.INakoFuncCallInfo) As Object
        Return BtoI(oXLS IsNot Nothing)
    End Function

    Public Function _xlVersion(ByVal info As NakoPlugin.INakoFuncCallInfo) As Object
        CheckExcel()
        Return oXLS.Version
    End Function

    Public Function _xlVisibleSet(ByVal info As NakoPlugin.INakoFuncCallInfo) As Object
        Dim arg As Integer = info.StackPopAsInt
        CheckExcel()
        oXLS.Visible = ItoB(arg)
        Return Nothing
    End Function

    Public Function _xlVisibleGet(ByVal info As NakoPlugin.INakoFuncCallInfo) As Object
        CheckExcel()
        Return BtoI(oXLS.Visible)
    End Function

    Public Function _xlDisplayAlertsSet(ByVal info As NakoPlugin.INakoFuncCallInfo) As Object
        Dim arg As Integer = info.StackPopAsInt
        CheckExcel()
        oXLS.DisplayAlerts = ItoB(arg)
        Return Nothing
    End Function

    Public Function _xlDisplayAlertsGet(ByVal info As NakoPlugin.INakoFuncCallInfo) As Object
        CheckExcel()
        Return BtoI(oXLS.DisplayAlerts)
    End Function

    Public Function _xlWindowStateSet(ByVal info As NakoPlugin.INakoFuncCallInfo) As Object
        Dim arg As String = info.StackPopAsString
        CheckExcel()
        Dim state As Integer
        Select Case arg
            Case "最大化"
                state = XlWindowState.xlMaximized
            Case "最小化"
                state = XlWindowState.xlMinimized
            Case "元通り"
                state = XlWindowState.xlNormal
            Case Else
                Throw New NakoPlugin.NakoPluginRuntimeException("『エクセル窓設定』命令の引数が不正です。『最大化|最小化|元通り』のいずれかを指定して下さい。")
        End Select
        If (state <> oXLS.WindowState) Then oXLS.WindowState = state
        Return Nothing
    End Function

    Public Function _xlWindowStateGet(ByVal info As NakoPlugin.INakoFuncCallInfo) As Object
        CheckExcel()
        Dim state As Integer = oXLS.WindowState
        Select Case state
            Case XlWindowState.xlMaximized
                Return "最大化"
            Case XlWindowState.xlMinimized
                Return "最小化"
            Case XlWindowState.xlNormal
                Return "元通り"
            Case Else
                Throw New NakoPlugin.NakoPluginRuntimeException("『エクセル窓取得』に失敗しました。")
        End Select
    End Function

    Public Function _xlApplicationCaptionSet(ByVal info As NakoPlugin.INakoFuncCallInfo) As Object
        Dim str As String = info.StackPopAsString()
        CheckExcel()
        oXLS.Caption = str
        Return Nothing
    End Function

    Public Function _xlApplicationCaptionGet(ByVal info As NakoPlugin.INakoFuncCallInfo) As Object
        CheckExcel()
        Return oXLS.Caption
    End Function

    Public Function _xlStatusBarSet(ByVal info As NakoPlugin.INakoFuncCallInfo) As Object
        Dim str As String = info.StackPopAsString
        CheckExcel()
        oXLS.StatusBar = str
        Return Nothing
    End Function

    Public Function _xlStatusBarGet(ByVal info As NakoPlugin.INakoFuncCallInfo) As Object
        CheckExcel()
        Return oXLS.StatusBar
    End Function

    Public Function _xlDisplayStatusBarSet(ByVal info As NakoPlugin.INakoFuncCallInfo) As Object
        Dim flag As Integer = info.StackPopAsInt
        CheckExcel()
        oXLS.DisplayStatusBar = ItoB(flag)
        Return Nothing
    End Function

    Public Function _xlDisplayStatusBarGet(ByVal info As NakoPlugin.INakoFuncCallInfo) As Object
        CheckExcel()
        Return BtoI(oXLS.DisplayStatusBar)
    End Function

    Public Function _xlScreenUpdatingSet(ByVal info As NakoPlugin.INakoFuncCallInfo) As Object
        Dim flag As Integer = info.StackPopAsInt
        CheckExcel()
        oXLS.ScreenUpdating = ItoB(flag)
        Return Nothing
    End Function

    Public Function _xlScreenUpdatingGet(ByVal info As NakoPlugin.INakoFuncCallInfo) As Object
        CheckExcel()
        Return BtoI(oXLS.ScreenUpdating)
    End Function

#End Region

#Region "ブック関係"

    Private Sub CheckBook()
        CheckExcel()
        'ブックがなければエラー
        If (oXLS.Workbooks_Count = 0) Then
            Throw New NakoPlugin.NakoPluginRuntimeException("Bookがありません。")
        End If
    End Sub

    Public Function _xlBookAdd(ByVal info As NakoPlugin.INakoFuncCallInfo) As Object
        CheckExcel()
        oXLS.BookAdd()
        Return Nothing
    End Function

    Public Function _xlBookOpen(ByVal info As NakoPlugin.INakoFuncCallInfo) As Object
        Dim Filename As String = info.StackPopAsString
        CheckExcel()
        ' 開こうとしているファイルが存在するか確認してから開く
        If (System.IO.File.Exists(Filename)) Then
            oXLS.Open(Filename)
        Else
            Throw New NakoPlugin.NakoPluginRuntimeException("『エクセルブック開く』で指定したファイル[" & Filename & "]が存在しません。")
        End If
        Return Nothing
    End Function

    Public Function _xlBookSaveAs(ByVal info As NakoPlugin.INakoFuncCallInfo) As Object
        Dim Filename As String = info.StackPopAsString
        CheckBook()
        Dim Fileformat As XlFileFormat = XlFileFormat.xlWorkbookNormal
        ' 現在の警告表示状態を取得
        Dim flag As Boolean = oXLS.DisplayAlerts
        ' 警告表示を解除する
        oXLS.DisplayAlerts = False
        ' ファイル名が指定されなければ上書き保存
        If (Filename = "") Then
            oXLS.Save()
            ' 警告表示状態を戻す
            oXLS.DisplayAlerts = flag
            Return Nothing
        End If
        ' 拡張子を調べる
        Dim ver As Double = Double.Parse(oXLS.Version)
        Dim ext As String = System.IO.Path.GetExtension(Filename)
        ext = ext.ToLower
        Select Case True
            Case ext = ".xlsx" And ver < 12.0
                ' 拡張子:xlsx Excel:2007以前
                ext = ".xls"
                Fileformat = XlFileFormat.xlExcel9795
            Case ext = ".xlsx"
                ' 拡張子:xlsx Excel:2007以降
                ' マクロを含んだBookを"xlsx"形式で保存すれば、マクロを削除して保存する仕様です。
                Fileformat = XlFileFormat.xlOpenXMLWorkbook
            Case ext = ".xls" And ver < 12.0
                ' 拡張子:xls Excel:2007以前
                Fileformat = XlFileFormat.xlExcel9795
            Case ext = ".xls"
                ' 拡張子:xls Excel:2007以降
                Fileformat = XlFileFormat.xlExcel8
            Case ext = ".xlsm" And ver < 12.0
                ' 拡張子:xlsm Excel:2007以前
                ext = ".xls"
                Fileformat = XlFileFormat.xlExcel9795
            Case ext = ".xlsm"
                ' 拡張子:xlsm Excel:2007以降
                Fileformat = XlFileFormat.xlOpenXMLWorkbookMacroEnabled
            Case ext = ".csv"
                ' 拡張子:csv
                Fileformat = XlFileFormat.xlCSV
            Case ext = ".txt"
                ' 拡張子:txt
                Fileformat = XlFileFormat.xlUnicodeText
            Case ext = ".pdf" And ver < 12.0
                ' 拡張子:pdf Excel:2007以前
                Throw New NakoPlugin.NakoPluginRuntimeException("『エクセル保存』で拡張子" & ext & "はExcel2007以降でないと指定できません。")
            Case ext = ".xps" And ver < 12.0
                ' 拡張子:xps Excel:2007以前
                Throw New NakoPlugin.NakoPluginRuntimeException("『エクセル保存』で拡張子" & ext & "はExcel2007以降でないと指定できません。")
            Case Else
                Throw New NakoPlugin.NakoPluginRuntimeException("『エクセル保存』で拡張子" & ext & "は指定できません。")
        End Select
        Filename = System.IO.Path.ChangeExtension(Filename, ext)
        If ext = ".pdf" Then
            oXLS.ExportAsFixedFormat(Filename, XlFixedFormatType.xlTypePDF)
        ElseIf ext = ".xps" Then
            oXLS.ExportAsFixedFormat(Filename, XlFixedFormatType.xlTypeXPS)
        Else
            oXLS.SaveAs(Filename, Fileformat)
        End If
        ' 警告表示状態を戻す
        oXLS.DisplayAlerts = flag
        Return Nothing
    End Function

    Public Function _xlBookVbaAvailable(ByVal info As NakoPlugin.INakoFuncCallInfo) As Object
        CheckBook()
        Return BtoI(oXLS.GetModules)
    End Function

    Public Function _xlBookClose(ByVal info As NakoPlugin.INakoFuncCallInfo) As Object
        CheckBook()
        oXLS.Close()
        Return Nothing
    End Function

    Public Function _xlBookCloseSave(ByVal info As NakoPlugin.INakoFuncCallInfo) As Object
        CheckBook()
        oXLS.CloseSave()
        Return Nothing
    End Function

    Public Function _xlBookSavedSet(ByVal info As NakoPlugin.INakoFuncCallInfo) As Object
        Dim Flag As Integer = info.StackPopAsInt
        CheckBook()
        oXLS.Saved = ItoB(Flag)
        Return Nothing
    End Function

    Public Function _xlBookSavedGet(ByVal info As NakoPlugin.INakoFuncCallInfo) As Object
        CheckBook()
        Return BtoI(oXLS.Saved)
    End Function

    Public Function _xlBookListGet(ByVal info As NakoPlugin.INakoFuncCallInfo) As Object
        CheckBook()
        Return oXLS.BookList
    End Function

    Public Function _xlBookActiveSet(ByVal info As NakoPlugin.INakoFuncCallInfo) As Object
        Dim book As String = info.StackPopAsString
        CheckBook()
        oXLS.ActiveBook = book
        Return Nothing
    End Function

    Public Function _xlBookActiveGet(ByVal info As NakoPlugin.INakoFuncCallInfo) As Object
        CheckBook()
        Return oXLS.ActiveBook
    End Function



#End Region

#Region "シート関係"
    Public Function _xlSheetDisplayGridlinesSet(ByVal info As NakoPlugin.INakoFuncCallInfo) As Object
        Dim flag As Integer = info.StackPopAsInt
        CheckBook()
        oXLS.DisplayGridlines = ItoB(flag)
        Return Nothing
    End Function

    Public Function _xlSheetDisplayGridLinesGet(ByVal info As NakoPlugin.INakoFuncCallInfo) As Object
        CheckBook()
        Return BtoI(oXLS.DisplayGridlines)
    End Function


#End Region

#Region "セル関係"
    Public Function _xlCellA1Conversion(ByVal info As NakoPlugin.INakoFuncCallInfo) As Object
        Dim cells() As String = Split(info.StackPopAsString, ",")
        CheckBook()
        Return oXLS.GetA1Adress(CLng(cells(0)), CLng(cells(1)))
    End Function

    Public Function _xlCellGetRow(ByVal info As NakoPlugin.INakoFuncCallInfo) As Object
        Dim range As String = info.StackPopAsString
        CheckBook()
        Return oXLS.GetCellRow(range)
    End Function

    Public Function _xlCellGetCol(ByVal info As NakoPlugin.INakoFuncCallInfo) As Object
        Dim range As String = info.StackPopAsString
        CheckBook()
        Return oXLS.GetCellCol(range)
    End Function

    Public Function _xlCellGetValue(ByVal info As NakoPlugin.INakoFuncCallInfo) As Object
        Dim range As String = info.StackPopAsString
        CheckBook()
        Return oXLS.GetCell(range, 1)
    End Function

    Public Function _xlCellSetValue(ByVal info As NakoPlugin.INakoFuncCallInfo) As Object
        Dim range As String = info.StackPopAsString
        Dim value As String = info.StackPopAsString
        CheckBook()
        oXLS.SetCell(range, value, 1)
        Return Nothing
    End Function

    Public Function _xlCellGetFormula(ByVal info As NakoPlugin.INakoFuncCallInfo) As Object
        Dim range As String = info.StackPopAsString
        CheckBook()
        Return oXLS.GetCell(range, 2)
    End Function

    Public Function _xlCellSetFormula(ByVal info As NakoPlugin.INakoFuncCallInfo) As Object
        Dim range As String = info.StackPopAsString
        Dim exp As String = info.StackPopAsString
        CheckBook()
        oXLS.SetCell(range, exp, 2)
        Return Nothing
    End Function

    Public Function _xlCellGetText(ByVal info As NakoPlugin.INakoFuncCallInfo) As Object
        Dim range As String = info.StackPopAsString
        CheckBook()
        Return oXLS.GetCell(range, 3)
    End Function

    Public Function _xlCellSetText(ByVal info As NakoPlugin.INakoFuncCallInfo) As Object
        Dim range As String = info.StackPopAsString
        Dim text As String = info.StackPopAsString
        CheckBook()
        oXLS.SetCell(range, text, 3)
        Return Nothing
    End Function

    Public Function _xlCellGetSerial(ByVal info As NakoPlugin.INakoFuncCallInfo) As Object
        Dim range As String = info.StackPopAsString
        CheckBook()
        Return oXLS.GetCell(range, 4)
    End Function

    Public Function _xlCellSetSerial(ByVal info As NakoPlugin.INakoFuncCallInfo) As Object
        Dim range As String = info.StackPopAsString
        Dim value As String = info.StackPopAsString
        CheckBook()
        oXLS.SetCell(range, value, 4)
        Return Nothing
    End Function

    Public Function _xlCellGetValueEx(ByVal info As NakoPlugin.INakoFuncCallInfo) As Object
        Dim range_LT As String = info.StackPopAsString
        Dim range_RB As String = info.StackPopAsString
        CheckBook()
        Return oXLS.GetCellEx(range_LT, range_RB, 1)
    End Function

    Public Function _xlCellSetValueEx(ByVal info As NakoPlugin.INakoFuncCallInfo) As Object
        Dim range As String = info.StackPopAsString
        Dim value As String = info.StackPopAsString
        CheckBook()
        oXLS.SetCellEx(range, value, 1)
        Return Nothing
    End Function

    Public Function _xlCellGetFormulaEx(ByVal info As NakoPlugin.INakoFuncCallInfo) As Object
        Dim range_LT As String = info.StackPopAsString
        Dim range_RB As String = info.StackPopAsString
        CheckBook()
        Return oXLS.GetCellEx(range_LT, range_RB, 2)
    End Function

    Public Function _xlCellSetFormulaEx(ByVal info As NakoPlugin.INakoFuncCallInfo) As Object
        Dim range As String = info.StackPopAsString
        Dim exp As String = info.StackPopAsString
        CheckBook()
        oXLS.SetCellEx(range, exp, 2)
        Return Nothing
    End Function

    Public Function _xlCellGetTextEx(ByVal info As NakoPlugin.INakoFuncCallInfo) As Object
        Dim range_LT As String = info.StackPopAsString
        Dim range_RB As String = info.StackPopAsString
        CheckBook()
        Return oXLS.GetCellEx(range_LT, range_RB, 3)
    End Function

    Public Function _xlCellSetTextEx(ByVal info As NakoPlugin.INakoFuncCallInfo) As Object
        Dim range As String = info.StackPopAsString
        Dim text As String = info.StackPopAsString
        CheckBook()
        oXLS.SetCellEx(range, text, 3)
        Return Nothing
    End Function

    Public Function _xlCellGetSerialEx(ByVal info As NakoPlugin.INakoFuncCallInfo) As Object
        Dim range_LT As String = info.StackPopAsString
        Dim range_RB As String = info.StackPopAsString
        CheckBook()
        Return oXLS.GetCellEx(range_LT, range_RB, 4)
    End Function

    Public Function _xlCellSetSerialEx(ByVal info As NakoPlugin.INakoFuncCallInfo) As Object
        Dim range As String = info.StackPopAsString
        Dim value As String = info.StackPopAsString
        CheckBook()
        oXLS.SetCellEx(range, value, 4)
        Return Nothing
    End Function



#End Region


    ''' <summary>
    ''' プラグインの初期化処理
    ''' </summary>
    Public Sub PluginInit(ByVal runner As NakoPlugin.INakoInterpreter) Implements NakoPlugin.INakoPlugin.PluginInit
        oXLS = Nothing
    End Sub
    ''' <summary>
    ''' プラグインの終了処理
    ''' </summary>
    Public Sub PluginFin(ByVal runner As NakoPlugin.INakoInterpreter) Implements NakoPlugin.INakoPlugin.PluginFin
        If Not (oXLS Is Nothing) Then
            _xlEnd()
            oXLS = Nothing
        End If
    End Sub

    Private Function BtoI(ByVal bln As Boolean) As Integer
        ' CIntより確実に変換したい
        ' True->1 False->0
        If (bln = True) Then Return 1
        Return 0
    End Function
    Private Function ItoB(ByVal int As Integer) As Boolean
        ' CBoolより確実に変換したい
        ' 0->False その他->True
        If (int = 0) Then Return False
        Return True
    End Function

End Class
