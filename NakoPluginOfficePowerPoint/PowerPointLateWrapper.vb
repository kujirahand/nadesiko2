Option Explicit Off

Public Class PowerPointLateWrapper

    Private oApp As Object
    Private ReadOnly ppSaveAsJPG = 17
    Private ReadOnly ppSaveAsPNG = 18
    Private ReadOnly ppSaveAsPDF = 32


    Public Sub New()
        Try
            oApp = CreateObject("PowerPoint.Application")
        Catch ex As Exception
            Throw New ApplicationException("PowerPointの起動ができません。インストールされているかチェックしてください。")
        End Try
    End Sub

    Public Sub Dispose()
        If Not (oApp Is Nothing) Then
            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oApp)
            oApp = Nothing
        End If
    End Sub

    Public Sub Quit()
        If Not (oApp Is Nothing) Then
            oApp.Quit()
            Me.Dispose()
        End If
    End Sub

    Public Sub Open(ByVal FName As String)
        oApp.Presentations.Open(FName)
    End Sub

    Public Sub Save(ByVal FName As String)
        oApp.ActivePresentation.SaveAs(FName)
    End Sub

    Public Function ppt_checkDir(ByVal dir As String)
        If Right(dir, 1) = "\" Then
            dir = Left(dir, Len(dir) - 1)
        End If
        Return dir
    End Function

    Public Sub SaveToPngDir(ByVal dir As String)
        System.IO.Directory.CreateDirectory(dir)
        oApp.ActivePresentation.SaveAs(ppt_checkDir(dir), ppSaveAsPNG)
    End Sub

    Public Sub SaveToJpegDir(ByVal dir As String)
        System.IO.Directory.CreateDirectory(dir)
        oApp.ActivePresentation.SaveAs(ppt_checkDir(dir), ppSaveAsJPG)
    End Sub

    Public Sub SaveToPDFFile(ByVal f As String)
        oApp.ActivePresentation.SaveAs(f, ppSaveAsPDF)
    End Sub

    Public Sub GoSlide(ByVal Index As Integer)
        oApp.ActiveWindow.View.GotoSlide(Index)
    End Sub

    Public Sub SlideExit()
        oApp.ActivePresentation.SlideShowWindow.View.Exit()
    End Sub

    Public Sub SlideNext()
        oApp.ActivePresentation.SlideShowWindow.View.Next()
    End Sub

    Public Sub SlidePrev()
        oApp.ActivePresentation.SlideShowWindow.View.Previous()
    End Sub

    Public Sub SlideRun()
        oApp.FPp.ActivePresentation.SlideShowSettings.Run()
    End Sub

    Public Sub SetVisible(ByVal Value As Boolean)
        Try
            oApp.Visible = Value
        Catch
        End Try
    End Sub

    Public Function MacroExec(ByVal s As String, ByVal arg As String) As String
        If arg = "" Then
            Return oApp.Run(s)
        Else
            Return oApp.Run(s, arg)
        End If
    End Function

    Public Sub PrintOut()
        oApp.ActivePresentation.PrintOut()
    End Sub

End Class
