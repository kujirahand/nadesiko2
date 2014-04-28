' +------------------------------+
' | Excel用ラッパー              |
' +------------------------------+
Option Explicit Off
''' <summary>
''' Excel用ラッパークラス
''' </summary>
Public Class ExcelLateWrapper

    Protected oApp As Object
    Protected oBooks As Object
    Protected oBook As Object
    Protected oSheets As Object
    Protected oSheet As Object
    Protected oRange As Object
    Protected oModules As Object

#Region "アプリケーション関係"
    ''' <summary>
    ''' Excel起動
    ''' Excelアプリケーションのインスタンスを作成する
    ''' </summary>
    Public Sub New()
        ' Excelオブジェクトの作成
        Try
            oApp = CreateObject("Excel.Application")
        Catch ex As Exception
            Throw New ApplicationException("『エクセル起動』でExcelのApplicationの取得に失敗しました。Excelがインストールされているか確認して下さい。")
        End Try
        ' Workbooksの取得
        Try
            oBooks = oApp.Workbooks
        Catch ex As Exception
            Throw New ApplicationException("『エクセル起動』でExcelのWorkbooksの取得に失敗しました。")
        End Try
    End Sub

    ''' <summary>
    ''' ブック終了
    ''' </summary>
    Public Sub BooksClose()
        DisplayAlerts = False
        If (oBook IsNot Nothing) Then oBook.Close()
        If (oBooks IsNot Nothing) Then oBooks.Close()
    End Sub

    ''' <summary>
    ''' Excelアプリケーション終了
    ''' </summary>
    Public Sub Quit()
        DisplayAlerts = False
        oApp.Quit()
    End Sub

    ''' <summary>
    ''' アプリケーション解放
    ''' </summary>
    Public Sub Dispose()
        DisposeSheets()
        If (oBooks IsNot Nothing) Then ReleaseObj(oBooks)
        If (oApp IsNot Nothing) Then
            Quit()
            ReleaseObj(oApp)
        End If
        oBooks = Nothing
        oApp = Nothing
    End Sub

    ''' <summary>
    ''' シート解放
    ''' </summary>
    Public Sub DisposeSheets()
        If (oSheet IsNot Nothing) Then ReleaseObj(oSheet)
        If (oSheets IsNot Nothing) Then ReleaseObj(oSheets)
        If (oBook IsNot Nothing) Then ReleaseObj(oBook)
        oSheet = Nothing
        oSheets = Nothing
        oBook = Nothing
    End Sub

    ''' <summary>
    '''  オブジェクト解放
    ''' </summary>
    ''' <param name="obj">解放するオブジェクト</param>
    Public Sub ReleaseObj(ByRef obj As Object)
        System.Runtime.InteropServices.Marshal.FinalReleaseComObject(obj)
    End Sub

    ''' <summary>
    ''' Excelのバージョンを返す
    ''' </summary>
    Public ReadOnly Property Version() As String
        Get
            Return oApp.Version
        End Get
    End Property

    ''' <summary>
    ''' Excelの可視
    ''' 1=True,0=False
    ''' </summary>
    Public Property Visible As Boolean
        Get
            Return oApp.Visible
        End Get
        Set(ByVal value As Boolean)
            oApp.Visible = value
        End Set
    End Property

    ''' <summary>
    ''' Excelの警告表示
    ''' 1=True,0=Fasle
    ''' </summary>
    Public Property DisplayAlerts As Boolean
        Get
            Return oApp.DisplayAlerts
        End Get
        Set(ByVal value As Boolean)
            oApp.DisplayAlerts = value
        End Set
    End Property

    ''' <summary>
    ''' Excelの窓サイズ
    ''' 「最大化|最小化|元通り」
    ''' </summary>
    Public Property WindowState As Integer
        Get
            Return oApp.WindowState
        End Get
        Set(ByVal value As Integer)
            oApp.WindowState = value
        End Set
    End Property

    ''' <summary>
    ''' Excelアプリケーションのタイトル
    ''' </summary>
    Public Property Caption As String
        Get
            Return CStr(oApp.Caption)
        End Get
        Set(ByVal value As String)
            oApp.Caption = value
        End Set
    End Property

    ''' <summary>
    ''' Excelアプリケーションのステータスバー
    ''' </summary>
    Public Property StatusBar As String
        Get
            Return oApp.StatusBar
        End Get
        Set(ByVal value As String)
            oApp.StatusBar = value
        End Set
    End Property

    ''' <summary>
    ''' Excelアプリケーションのステータスバーの可視
    ''' 1=True,0=Fasle
    ''' </summary>
    Public Property DisplayStatusBar As Boolean
        Get
            Return oApp.DisplayStatusBar
        End Get
        Set(ByVal value As Boolean)
            oApp.DisplayStatusBar = value
        End Set
    End Property

    ''' <summary>
    ''' Excelアプリケーションの画面更新
    ''' 1=True,0=Fasle
    ''' </summary>
    Public Property ScreenUpdating As Boolean
        Get
            Return oApp.ScreenUpdating
        End Get
        Set(ByVal value As Boolean)
            oApp.ScreenUpdating = value
        End Set
    End Property

#End Region

#Region "ブック関係"
    ' --- Book ---
    ''' <summary>
    ''' Bookを追加する
    ''' </summary>
    Public Sub BookAdd()
        oBooks.Add()
        AfterBookOpen()
    End Sub

    ''' <summary>
    ''' Bookを開く
    ''' </summary>
    Public Sub Open(ByVal Filename As String)
        Try
            oBooks.Open(Filename)
        Catch ex As Exception
            Throw New ApplicationException("『エクセルブック開く』で" & Filename & "を開くことができませんでした。")
        End Try
        AfterBookOpen()
    End Sub

    ''' <summary>
    ''' Bookを保存しないで閉じる
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Close()
        Try
            oBook.Close(False)
        Catch ex As Exception
            Throw New ApplicationException("『エクセルブック閉じる』または『エクセルブック保存後閉じる』でエラーが発生しました。")
        End Try
        AfterBookOpen()
    End Sub

    ''' <summary>
    ''' Bookを上書き保存して閉じる
    ''' </summary>
    Public Sub CloseSave()
        Try
            Save()
            Close()
        Catch ex As Exception
            Throw New ApplicationException("『エクセルブック保存後閉じる』でエラーが発生しました。")
        End Try
    End Sub

    ''' <summary>
    ''' Bookを開いたり閉じた後の処理
    ''' Book、Sheetオブジェクトの更新
    ''' </summary>
    Public Sub AfterBookOpen()
        ' 既存のオブジェクト破棄
        DisposeSheets()
        If (oBooks IsNot Nothing) Then ReleaseObj(oBooks)
        ' オブジェクトの再取得
        Try
            oBooks = oApp.Workbooks
        Catch ex As Exception
            Throw New ApplicationException("『エクセルブック追加』または『エクセルブック開く』でExcelのWorkbooksの取得に失敗しました。")
        End Try
        If (Workbooks_Count = 0) Then Exit Sub
        oBook = oBooks(Workbooks_Count)
        If oBook Is Nothing Then Throw New ApplicationException("『エクセルブック追加』または『エクセルブック開く』でExcelのWorkbookの取得に失敗しました。")
        oBook.Activate()
        oSheets = oBook.Worksheets
        If oSheets Is Nothing Then Throw New ApplicationException("『エクセルブック追加』または『エクセルブック開く』でExcelのWorksheetsの取得に失敗しました。")
        oSheet = oBook.Activesheet
        If oSheet Is Nothing Then Throw New ApplicationException("『エクセルブック追加』または『エクセルブック開く』でExcelのWorksheetの取得に失敗しました。")
        GetModules()
    End Sub

    ''' <summary>
    ''' Bookの数を返す
    ''' </summary>
    Public ReadOnly Property Workbooks_Count As Integer
        Get
            Return oBooks.Count()
        End Get
    End Property

    ''' <summary>
    ''' VBAアクセスができるか
    ''' </summary>
    Public Function GetModules() As Boolean
        Try
            oModules = oBook.VBProject.VBComponents
        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function

    ''' <summary>
    ''' Bookを上書き保存
    ''' </summary>
    Public Sub Save()
        ' 現在の警告表示状態を取得
        Dim flag As Boolean = DisplayAlerts
        DisplayAlerts = False
        If oBook Is Nothing Then Throw New ApplicationException("『エクセルブック保存』または『エクセルブック保存後閉じる』でExcelのWorkbookの取得に失敗しました。")
        Try
            oBook.Save()
        Catch ex As Exception
            Throw New ApplicationException("『エクセルブック保存』または『エクセルブック保存後閉じる』でエラーが発生しました。")
        End Try
        ' 警告表示状態を戻す
        DisplayAlerts = flag
    End Sub

    ''' <summary>
    ''' Bookを名前を付けて保存
    ''' </summary>
    Public Sub SaveAs(ByVal Filename As String, ByVal Fileformat As XlFileFormat)
        If oBook Is Nothing Then Throw New ApplicationException("『エクセルブック保存』でExcelのWorkbookの取得に失敗しました。")
        Try
            oBook.SaveAs(Filename, Fileformat)
        Catch ex As Exception
            Throw New ApplicationException("『エクセルブック保存』でファイル名が不正です。またはその他のエラーが発生しました。")
        End Try
    End Sub

    ''' <summary>
    ''' BookをPDF等の形式で保存
    ''' </summary>
    Public Sub ExportAsFixedFormat(ByVal Filename As String, ByVal Fileformat As XlFixedFormatType)
        Try
            oBook.ExportAsFixedFormat(Filename, Fileformat)
        Catch ex As Exception
            Throw New ApplicationException("『エクセルブック保存』でファイル名が不正です。またはその他のエラーが発生しました。")
        End Try
    End Sub

    ''' <summary>
    ''' Bookの変更が保存されているか
    ''' </summary>
    Public Property Saved As Boolean
        Get
            Return oBook.Saved
        End Get
        Set(ByVal value As Boolean)
            oBook.Saved = value
        End Set
    End Property

    ''' <summary>
    ''' 開いているBookの名前を列挙
    ''' </summary>
    Public Function BookList() As String
        Dim Workbooks As String = ""
        Dim Workbook As Object
        For Each Workbook In oBooks
            If Workbooks = "" Then
                Workbooks = Workbook.Name
            Else
                Workbooks = Workbooks + vbCrLf + Workbook.Name
            End If
        Next
        Return Workbooks
    End Function

    ''' <summary>
    ''' ActiveBook名
    ''' </summary>
    Public Property ActiveBook As String
        Get
            Return oApp.ActiveWorkbook.Name
        End Get
        Set(ByVal value As String)
            Try
                oApp.Workbooks(value).Activate()
            Catch ex As Exception
                Throw New ApplicationException("『エクセルブックアクティブ設定』でExcelのBookの取得に失敗しました。")
            End Try
        End Set
    End Property

#End Region

#Region "シート関係"
    ' --- Sheet ---
    ''' <summary>
    ''' Sheetの枠線の可視
    ''' </summary>
    Public Property DisplayGridlines As Boolean
        Get
            Dim window As Object
            Dim bool As Boolean
            window = oApp.ActiveWindow
            If window Is Nothing Then Throw New ApplicationException("『エクセルシート枠線可視取得』でExcelのWorksheetの取得に失敗しました。")
            bool = window.DisplayGridlines
            ReleaseObj(window)
            Return bool
        End Get
        Set(ByVal value As Boolean)
            Dim window As Object
            window = oApp.ActiveWindow
            If window Is Nothing Then Throw New ApplicationException("『エクセルシート枠線可視設定』でExcelのWorksheetの取得に失敗しました。")
            window.DisplayGridlines = value
            ReleaseObj(window)
        End Set
    End Property

#End Region

#Region "セル関係"
    ' --- Range ---
    ''' <summary>
    ''' Cell(A1形式)からRangeオブジェクトを取得
    ''' </summary>
    Public Function GetRangeObj(ByVal cell As String) As Object
        Dim range As Object
        Try
            range = oSheet.Range(cell)
        Catch ex As Exception
            Throw New ApplicationException("ExcelのRangeの取得に失敗しました。セル範囲の指定が不正です。")
        End Try
        Return range
    End Function

    ''' <summary>
    ''' R1C1形式セル番地からA1形式セル番地へ変換
    ''' </summary>
    Public Function GetA1Adress(ByVal row As Long, ByVal col As Long) As String
        Dim range As Object, addr As String
        range = oSheet.Cells(row, col)
        If range Is Nothing Then Throw New ApplicationException("ExcelのRangeの取得に失敗しました。")
        addr = range.Address
        Return addr.Replace("$", "")
    End Function

    ''' <summary>
    ''' Cell(A1形式)から行番号rowを取得
    ''' </summary>
    Public Function GetCellRow(ByVal cell As String) As Long
        oRange = GetRangeObj(cell)
        Return oRange(1, 1).Row
    End Function

    ''' <summary>
    ''' Cell(A1形式)から列番号colを取得
    ''' </summary>
    Public Function GetCellCol(ByVal cell As String) As Long
        oRange = GetRangeObj(cell)
        Return oRange(1, 1).Column
    End Function

    ''' <summary>
    ''' Cellを取得
    ''' </summary>
    Public Function GetCell(ByVal range As String, ByVal mode As Integer) As String
        oRange = GetRangeObj(range)
        Dim result As String = ""
        Select Case mode
            Case 1
                result = oRange(1, 1).Value   '値
            Case 2
                result = oRange(1, 1).Formula '式
            Case 3
                result = oRange(1, 1).Text    '文字列
            Case 4
                result = oRange(1, 1).Value2  'シリアル値
        End Select
        Return result
    End Function

    ''' <summary>
    ''' Cellを範囲取得
    ''' </summary>
    Public Function GetCellEx(ByVal range_LT As String, ByVal range_RB As String, ByVal mode As Integer) As String
        ' ********** 本来は二次元配列でデータ取得すべき。(現在はStringに変換して取得)
        ' ********** まだ二次元配列が実装されていないため、CSVと配列の変換はクジラさんにお願いする。
        oRange = GetRangeObj(range_LT & ":" & range_RB)
        Dim row As Long = oRange.Rows.Count - 1
        Dim col As Long = oRange.Columns.Count - 1
        Dim result As String = ""
        If row = 0 And col = 0 Then
            Return GetCell(range_LT & ":" & range_RB, mode)
        End If
        Dim str_r(row) As String
        Dim str_c(col) As String
        Dim value As Object = Nothing
        For i As Long = 0 To row
            For j As Long = 0 To col
                Select Case mode
                    Case 1
                        value = oRange(i + 1, j + 1).Value      '値
                    Case 2
                        value = oRange(i + 1, j + 1).Formula    '式
                    Case 3
                        value = oRange(i + 1, j + 1).Text       '文字列
                    Case 4
                        value = oRange(i + 1, j + 1).Value2     'シリアル値
                End Select
                str_c(j) = value
            Next j
            str_r(i) = String.Join(",", str_c)
        Next i
        result = String.Join(vbCrLf, str_r)
        Return result
    End Function

    ''' <summary>
    ''' Cellに設定
    ''' </summary>
    Public Sub SetCell(ByVal range As String, ByVal value As String, ByVal mode As Integer)
        oRange = GetRangeObj(range)
        Select Case mode
            Case 1
                oRange(1, 1).Value = value          '値
            Case 2
                oRange(1, 1).Formula = value        '式
            Case 3
                oRange(1, 1).Value = "'" & value    '文字列
            Case 4
                oRange(1, 1).Value2 = value         'シリアル値
        End Select
    End Sub

    ''' <summary>
    ''' Cellに範囲設定
    ''' </summary>
    Public Sub SetCellEx(ByVal range As String, ByVal value As String, ByVal mode As Integer)
        ' ********** 本来は二次元配列でデータ設定すべき。(現在はStringから変換して取得)
        ' ********** まだ二次元配列が実装されていないため、CSVと配列の変換はクジラさんにお願いする。
        oRange = GetRangeObj(range)
        Dim array(,) As String = CSVToArray(value)  'CSV形式を二次元配列に変換
        Dim row As Long = array.GetLongLength(0) - 1
        Dim col As Long = array.GetLongLength(1) - 1
        For i = 0 To row
            For j = 0 To col
                Select Case mode
                    Case 1
                        oRange.Offset(i, j).Value = array(i, j)
                    Case 2
                        oRange.Offset(i, j).Formula = array(i, j)
                    Case 3
                        oRange.Offset(i, j).Value = "'" & array(i, j)
                    Case 4
                        oRange.Offset(i, j).Value2 = array(i, j)
                End Select
            Next j
        Next i
    End Sub







    'セルクリア
    'セル値クリア
    'セル書式クリア

#End Region

    ' --- Utilitys ---
    ''' <summary>
    ''' 二次元配列をCSV形式文字列に変換
    ''' </summary>
    Protected Function ArrayToCSV(ByVal array As Object(,)) As String
        Dim result As String = ""
        Dim row As Long = array.GetLongLength(0) - 1
        Dim col As Long = array.GetLongLength(1) - 1
        Dim str_r(row) As String
        Dim str_c(col) As String
        For i As Long = 0 To row
            For j As Long = 0 To col
                str_c(j) = array(i + 1, j + 1)  'Rangeは(1,1)からなので+1必要
            Next j
            str_r(i) = String.Join(",", str_c)
        Next i
        result = String.Join(vbCrLf, str_r)
        Return result
    End Function

    ''' <summary>
    ''' CSV形式文字列を二次元配列に変換
    ''' </summary>
    Protected Function CSVToArray(ByVal csv As String) As String(,)
        Dim str_r() As String = csv.Split(vbCrLf)
        Dim row As Long = str_r.GetLongLength(0) - 1
        Dim str_c() As String = str_r(0).Split(",")
        Dim col As Long = str_c.GetLongLength(0) - 1
        Dim result(row, col) As String
        For i = 0 To row
            str_c = str_r(i).Split(",")
            For j = 0 To col
                result(i, j) = str_c(j).Replace(vbLf, "")   '不要な改行を削除
            Next j
        Next i
        Return result
    End Function

End Class



#Region "===== 定数 ====="
' ウィンドウサイズ
Public Enum XlWindowState
    xlNormal = -4143 ' 元通り
    xlMinimized = -4140 ' 最小化
    xlMaximized = -4137 ' 最大化
End Enum

' ファイル形式
Public Enum XlFileFormat
    xlCurrentPlatformText = -4158 ' テキスト形式の種類を指定(使用中のシステム)
    xlWorkbookNormal = -4143 ' Excelブック形式
    xlSYLK = 2 ' シンボリックリンク形式
    xlWKS = 4 ' Lotus1-2-3形式
    xlWK1 = 5 ' Lotus1-2-3形式
    xlCSV = 6 ' コンマ区切りの値
    xlDBF2 = 7 ' Dbase2形式
    xlDBF3 = 8 ' Dbase3形式
    xlDIF = 9 ' データ交換形式
    xlDBF4 = 11 ' Dbase4形式
    xlWJ2WD1 = 14 ' 不適切な形式
    xlWK3 = 15 ' Lotus1-2-3形式
    xlExcel2 = 16 ' Excel2.0
    xlTemplate = 17 ' Excelテンプレート形式
    xlTemplate8 = 17 ' Excelテンプレート形式
    xlAddIn = 18 ' Microsoft Office Excelアドイン
    xlAddIn8 = 18 ' Excel2007アドイン
    xlTextMac = 19 ' テキスト形式の種類を指定(Mac)
    xlTextWindows = 20 ' テキスト形式の種類を指定(Windows)
    xlTextMSDOS = 21 ' テキスト形式の種類を指定(MS-DOS)
    xlCSVMac = 22 ' コンマ区切りの値
    xlCSVWindows = 23 ' コンマ区切りの値
    xlCSVMSDOS = 24 ' コンマ区切りの値
    xlIntlMacro = 25 ' マクロのインターナショナル形式
    xlIntlAddIn = 26 ' Microsoft Office Excelアドインのインターナショナル形式
    xlExcel2FarEast = 27 ' Excel2.0(東アジア)
    xlWorks2FarEast = 28 ' Microsoft Works2.0形式
    xlExcel3 = 29 ' Excel3.0
    xlWK1FMT = 30 ' Lotus1-2-3形式
    xlWK1ALL = 31 ' Lotus1-2-3形式
    xlWK3FM3 = 32 ' Lotus1-2-3形式
    xlExcel4 = 33 ' Excel4.0
    xlWQ1 = 34 ' Quattro Pro形式
    xlExcel4Workbook = 35 ' Excel4.0、ワークブック形式
    xlTextPrinter = 36 ' テキスト形式の種類を指定(プリンター)
    xlWK4 = 38 ' Lotus1-2-3形式
    xlExcel7 = 39 ' Excel95
    xlExcel5 = 39 ' Excel5.0
    xlWJ3 = 40 ' 不適切な形式
    xlWJ3FJ3 = 41 ' 不適切な形式
    xlUnicodeText = 42 ' テキスト形式の種類を指定(Unicode)
    xlExcel9795 = 43 ' Excel5.0,95,97,2000
    xlHtml = 44 ' Webページ形式
    xlWebArchive = 45 ' MHT形式
    xlXMLSpreadsheet = 46 ' Excelシート形式
    xlExcel12 = 50 ' Excel12.0
    xlOpenXMLWorkbook = 51 ' XMLブック
    xlWorkbookDefault = 51 ' 規程のブック
    xlOpenXMLWorkbookMacroEnabled = 52 ' マクロを有効にしたXMLブック
    xlOpenXMLTemplateMacroEnabled = 53 ' マクロを有効にしたXMLテンプレート
    xlOpenXMLTemplate = 54 ' XMLテンプレート
    xlOpenXMLAddIn = 55 ' XMLアドイン
    xlExcel8 = 56 ' Excel8.0(Excel12.0以降でxls形式)
    xlOpenDocumentSpreadsheet = 60 ' Open Document形式
    ' いくつかの定数は、日本語環境で使用できない
End Enum

' 名前を付けて保存
Public Enum XlSaveAsAccessMode
    xlNoChange = 1 ' モード変更なし
    xlShared = 2 ' 共有モード
    xlExclusive = 3 ' 排他モード
End Enum

' 保存形式
Public Enum XlFixedFormatType
    xlTypePDF = 0 ' PDF
    xlTypeXPS = 1 ' XPS
End Enum

' 下線スタイル
Public Enum XlUnderlineStyle
    xlUnderlineStyleNone = -4142 ' なし
    xlUnderlineStyleSingle = 2 ' 下線
    xlUnderlineStyleDouble = -4119 ' 二重下線
    xlUnderlineStyleSingleAccounting = 4 ' 下線会計
    xlUnderlineStyleDoubleAccounting = 5 ' 二重下線会計
End Enum

' 網掛けパターン
Public Enum XlPattern
    xlPatternNone = -4142 ' なし
    xlPatternSolid = 1 ' ベタ
    xlPatternChecker = 9 ' 市松
    xlPatternGrid = 15 ' 格子
    xlPatternCrissCross = 16 ' 網目
    xlPatternLightHorizontal = 11 ' 薄横線
    xlPatternLightVertical = 12 ' 薄縦線
    xlPatternLightDown = 13 ' 右下薄対角
    xlPatternLightUp = 14 ' 右上薄対角
    xlPatternHorizontal = -4128 ' 濃横線
    xlPatternVertical = -4166 ' 濃縦線
    xlPatternDown = -4121 ' 右下濃対角
    xlPatternUp = -4162 ' 右上濃対角
    xlPatternGray8 = 18 ' 8%灰色
    xlPatternGray16 = 17 ' 16%灰色
    xlPatternGray25 = -4124 ' 25%灰色
    xlPatternGray50 = -4125 ' 50%灰色
    xlPatternGray75 = -4126 ' 75%灰色
    xlPatternSemiGray75 = 10 ' 75%濃モアレ
    xlPatternAutomatic = -4105 ' Excel制御
End Enum

' 罫線位置
Public Enum XlBordersIndex
    xlEdgeTop = 8 ' 上
    xlEdgeBottom = 9 ' 下
    xlEdgeLeft = 7 ' 左
    xlEdgeRight = 10 ' 右
    xlDiagonalDown = 5 ' ＼
    xlDiagonalUp = 6 ' ／
    xlInsideHorizontal = 12 ' 内横
    xlInsideVertical = 11 ' 内縦
End Enum

' 罫線太さ
Public Enum XlBorderWeight
    xlHairline = 1 ' 細線(最も細い罫線)
    xlMedium = -4138 ' 普通
    xlThick = 4 ' 太線(最も太い罫線)
    xlThin = 2 ' 極細
End Enum

' 罫線種類
Public Enum XlLineStyle
    xlContinuous = 1 ' 実線
    xlDash = -4115 ' 破線
    xlDashDot = 4 ' 一点鎖線
    xlDashDotDot = 5 ' ニ点鎖線
    xlDot = -4118 ' 点線
    xlDouble = -4119 ' 二重線
    xlLineStyleNone = -4142 ' 線なし
    xlSlantDashDot = 13 ' 斜破線
End Enum

' ふりがな種類
Public Enum XlPhoneticCharacterType
    xlKatakanaHalf = 0 ' 半角カタカナ
    xlKatakana = 1 ' カタカナ
    xlHiragana = 2 ' ひらがな
    xlNoConversion = 3 ' なし
End Enum

' ふりがな配置
Public Enum XlPhoneticAlignment
    XlPhoneticAlignNoControl = 0 ' 自動
    XlPhoneticAlignLeft = 1 ' 左
    XlPhoneticAlignCenter = 2 ' 中央
    XlPhoneticAlignDistributed = 3 ' 均等
End Enum

' 形式を指定して貼り付け
Public Enum XlPasteType
    xlPasteAll = -4104 ' すべてを貼り付け
    xlPasteAllExceptBorders = 7 ' 輪郭以外のすべてを貼り付け
    xlPasteAllMergingConditionalFormats = 14 ' すべてを貼り付け、条件付き書式をマージ
    xlPasteAllUsingSourceTheme = 13 ' ソースのテーマを使用してすべてを貼り付け
    xlPasteColumnWidths = 8 ' コピーした列の幅を貼り付け
    xlPasteComments = -4144 ' コメントを貼り付け
    xlPasteFormats = -4122 ' コピーしたソースの形式を貼り付け
    xlPasteFormulas = -4123 ' 数式を貼り付け
    xlPasteFormulasAndNumberFormats = 11 ' 数式と数値の書式を貼り付け
    xlPasteValidation = 6 ' 入力規則を貼り付け
    xlPasteValues = -4163 ' 値を貼り付け
    xlPasteValuesAndNumberFormats = 12 ' 値と数値の書式を貼り付け
End Enum

' セル内の横位置
Public Enum XlHAlign
    xlHAlignGeneral = 1 ' 標準
    xlHAlignLeft = -4131 ' 左詰め
    xlHAlignCenter = -4108 '中央揃え
    xlHAlignRight = -4152 ' 右詰め
    xlHAlignFill = 5 ' 繰り返し
    xlHAlignJustify = -4130 ' 両端揃え
    xlHAlignCenterAcrossSelection = 7 ' 選択範囲内で中央
    xlHAlignDistributed = -4117 ' 均等割り付け
End Enum

' セル内の縦位置
Public Enum XlVAlign
    xlVAlignTop = -4160 ' 上詰め
    xlVAlignCenter = -4108 ' 中央揃え
    xlVAlignBottom = -4107 ' 下詰め
    xlVAlignDistributed = -4117 ' 均等割り付け
    xlVAlignJustify = -4130 ' 両端揃え
End Enum

' セル種類
Public Enum XlCellType
    xlCellTypeAllFormatConditions = -4172 ' 表示形式
    xlCellTypeAllValidation = -4174 ' 条件
    xlCellTypeBlanks = 4 ' 空白
    xlCellTypeComments = -4144 ' コメント
    xlCellTypeConstants = 2 ' 定数
    xlCellTypeFormulas = -4123 ' 数式
    xlCellTypeLastCell = 11 ' 最後
    xlCellTypeSameFormatConditions = -4173 ' 同じ表示形式
    xlCellTypeSameValidation = -4175 ' 同じ条件
    xlCellTypeVisible = 12 ' 可視
End Enum

' 用紙サイズ
Public Enum XlPaperSize
    xlPaper10x14 = 16 ' 10 x 14 インチ
    xlPaper11x17 = 17 ' 11 x 17 インチ
    xlPaperA3 = 8 ' A3 (297 mm x 420 mm)
    xlPaperA4 = 9 ' A4 (210 mm x 297 mm)
    xlPaperA4Small = 10 ' A4 (小型) (210 mm x 297 mm)
    xlPaperA5 = 11 ' A5 (148 mm x 210 mm)
    xlPaperA6 = 70 ' A6 (105 mm x 148 mm)
    xlPaperB4 = 12 ' B4 (250 mm x 354 mm)
    xlPaperB5 = 13 ' B5 (182 mm x 257 mm)
    xlPaperB6 = 88 ' B6 (128 mm x 182 mm)
    xlPaperCsheet = 24 ' C (17 インチ x 22 インチ)
    xlPaperDsheet = 25 ' D (22 インチ x 34 インチ)
    xlPaperEnvelope10 = 20 ' 封筒 10 号 (4 1/8 x 9 1/2 インチ)
    xlPaperEnvelope11 = 21 ' 封筒 11 号 (4 1/2 x 10 3/8 インチ)
    xlPaperEnvelope12 = 22 ' 封筒 12 号 (4 1/2 x 11 インチ)
    xlPaperEnvelope14 = 23 ' 封筒 14 号 (5 x 11 1/2 インチ)
    xlPaperEnvelope9 = 19 ' 封筒 9 号 (3 7/8 x 8 7/8 インチ)
    xlPaperEnvelopeB4 = 33 ' 封筒 B4 (250 mm x 353 mm)
    xlPaperEnvelopeB5 = 34 ' 封筒 B5 (176 mm x 250 mm)
    xlPaperEnvelopeB6 = 35 ' 封筒 B6 (176 mm x 125 mm)
    xlPaperEnvelopeC3 = 29 ' 封筒 C3 (324 mm x 458 mm)
    xlPaperEnvelopeC4 = 30 ' 封筒 C4 (229 mm x 324 mm)
    xlPaperEnvelopeC5 = 28 ' 封筒 C5 (162 mm x 229 mm)
    xlPaperEnvelopeC6 = 31 ' 封筒 C6 (114 mm x 162 mm)
    xlPaperEnvelopeC65 = 32 ' 封筒 C65 (114 mm x 229 mm)
    xlPaperEnvelopeDL = 27 ' 封筒 DL (110 x 220 mm)
    xlPaperEnvelopeItaly = 36 ' 封筒 (110 mm x 230 mm)
    xlPaperEnvelopeMonarch = 37 ' 封筒モナーク (3 7/8 x 7 1/2 インチ)
    xlPaperEnvelopePersonal = 38 ' 封筒 (3 5/8 x 6 1/2 インチ)
    xlPaperEsheet = 26 ' E (34 インチ x 44 インチ)
    xlPaperExecutive = 7 ' エグゼクティブ (7 1/2 x 10 1/2 インチ)
    xlPaperFanfoldLegalGerman = 41 ' ドイツ リーガル複写紙 (8 1/2 x 13 インチ)
    xlPaperFanfoldStdGerman = 40 ' ドイツ リーガル複写紙 (8 1/2 x 13 インチ)
    xlPaperFanfoldUS = 39 ' 米国標準ファンフォールド (14 7/8 x 11 インチ)
    xlPaperFolio = 14 ' フォリオ (8 1/2 x 13 インチ)
    xlPaperHagaki = 43 ' はがき (100 mm x 148 mm)
    xlPaperHagaki2 = 69 ' 往復はがき (148 mm x 200 mm)
    xlPaperL = 123 ' L版
    xlPaperLedger = 4 ' Ledger (17 x 11 インチ)
    xlPaperLegal = 5 ' リーガル (8 1/2 x 14 インチ)
    xlPaperLetter = 1 ' レター (8 1/2 x 11 インチ)
    xlPaperLetterSmall = 2 ' レター (小型) (8 1/2 x 11 インチ)
    xlPaperMeishi = 127 ' 名刺
    xlPaperNote = 18 ' ノート (8 1/2 x 11 インチ)
    xlPaperQuarto = 15 ' カート (215 mm x 275 mm)
    xlPaperStatement = 6 ' ステートメント (5 1/2 x 8 1/2 インチ)
    xlPaperTabloid = 3 ' タブロイド (11 x 17 インチ)
    xlPaperUser = 256 ' ユーザー設定
    ' いくつかの定数は、日本語環境またはプリンタにより使用できない
End Enum

' 印刷方向
Public Enum XlPageOrientation
    xlLandscape = 2 ' 横向き
    xlPortrait = 1 ' 縦向き
End Enum

#End Region

