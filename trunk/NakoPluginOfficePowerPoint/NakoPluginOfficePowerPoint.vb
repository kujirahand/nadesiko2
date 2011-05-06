'
' なでしこプラグイン (NakoPluginOfficePowerPoint)
'
' ---------------------------------------------------------------
''' <summary>
''' PowerPoint用のなでしこプラグイン(NakoPluginOfficePowerPoint)
''' </summary>
Public Class NakoPluginOfficePowerPoint
    Implements NakoPlugin.INakoPlugin

    ''' <summary>
    ''' プラグインのフルネーム、これは変更不要
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
            Return "PowerPoint 操作プラグイン"
        End Get
    End Property

    ''' <summary>
    ''' プラグインのバージョン
    ''' </summary>
    Public ReadOnly Property PluginVersion As Double Implements NakoPlugin.INakoPlugin.PluginVersion
        Get
            Return 1.0
        End Get
    End Property

    ''' <summary>
    ''' ターゲットとするなでしこのバージョン
    ''' </summary>
    Public ReadOnly Property TargetNakoVersion As Double Implements NakoPlugin.INakoPlugin.TargetNakoVersion
        Get
            Return 2.0
        End Get
    End Property

    ''' <summary>
    ''' Usedの実装が必要
    ''' </summary>
    Private _userd As Boolean = False
    Public Property Used As Boolean Implements NakoPlugin.INakoPlugin.Used
        Get
            Return _userd
        End Get
        Set(ByVal value As Boolean)
            _userd = value
        End Set
    End Property

    ' ---------------------------------------------------------------
    ' DLL内で使う各種変数
    ' ---------------------------------------------------------------
    Dim oPPT As PowerPointLateWrapper

    ''' <summary>
    ''' なでしこメソッドの定義
    ''' </summary>
    ''' <param name="bank">定義用引数</param>
    Public Sub DefineFunction(ByVal bank As NakoPlugin.INakoPluginBank) Implements NakoPlugin.INakoPlugin.DefineFunction
        ' なでしこ関数の定義
        bank.AddFunc("パワポ起動", "Vで", NakoPlugin.NakoVarType.Void, AddressOf _ppt_new, "可視状態V(オン/オフ)でPowerPointを起動する", "ぱわぽきどう")
        bank.AddFunc("パワポ開く", "FILEを|FILEの", NakoPlugin.NakoVarType.Void, AddressOf _ppt_open, "PowerPointのファイルを開く", "ぱわぽひらく")
        bank.AddFunc("パワポPNG出力", "DIRへ", NakoPlugin.NakoVarType.Void, AddressOf _ppt_saveAsPNG, "PowerPointをPNG形式でFILEという名前で出力する", "ぱわぽPNGしゅつりょく")
        bank.AddFunc("パワポ終了", "", NakoPlugin.NakoVarType.Void, AddressOf _ppt_quit, "PowerPointを終了する", "ぱわぽしゅうりょう")
    End Sub

    Public Function _ppt_new(ByVal info As NakoPlugin.INakoFuncCallInfo) As Object
        Dim i As Integer
        i = info.StackPopAsInt()
        oPPT = New PowerPointLateWrapper()
        oPPT.SetVisible(i <> 0)
        Return (Nothing)
    End Function

    Private Sub CheckPPT()
        If (oPPT Is Nothing) Then
            Throw New NakoPlugin.NakoPluginRuntimeException("先に『パワポ起動』でPowerPointを起動してください。")
        End If
    End Sub


    Public Function _ppt_open(ByVal info As NakoPlugin.INakoFuncCallInfo) As Object
        Dim f As String
        f = info.StackPopAsString()
        CheckPPT()
        oPPT.Open(f)
        Return (Nothing)
    End Function

    Public Function _ppt_saveAsPNG(ByVal info As NakoPlugin.INakoFuncCallInfo) As Object
        Dim dir As String
        dir = info.StackPopAsString()
        CheckPPT()
        oPPT.SaveToPngDir(dir)
        Return (Nothing)
    End Function

    Public Function _ppt_quit(ByVal info As NakoPlugin.INakoFuncCallInfo) As Object
        CheckPPT()
        oPPT.Quit()
        Return (Nothing)
    End Function

    Public Sub PluginFin(ByVal runner As NakoPlugin.INakoInterpreter) Implements NakoPlugin.INakoPlugin.PluginFin
        If Not (oPPT Is Nothing) Then
            oPPT.Quit()
            oPPT.Dispose()
            oPPT = Nothing
        End If
    End Sub

    Public Sub PluginInit(ByVal runner As NakoPlugin.INakoInterpreter) Implements NakoPlugin.INakoPlugin.PluginInit
        oPPT = Nothing
    End Sub

End Class
