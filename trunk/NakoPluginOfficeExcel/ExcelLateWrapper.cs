using System;
using System.Collections.Generic;
using System.Text;

using System.Reflection;

namespace NakoPluginOfficeExcel
{
    public class ExcelLateWrapper
    {
        protected object oApp;
        protected object oBooks;
        protected object oBook;
        protected object oSheets;
        protected object oSheet;

        protected static Type ExcelApp;
        protected static Type Workbooks;

        public ExcelLateWrapper()
        {
            //
            ExcelApp = Type.GetTypeFromProgID("Excel.Application");
            if (ExcelApp == null)
            {
                throw new ApplicationException("Excelがインストールされていません。");
            }
            //
            oApp = Activator.CreateInstance(ExcelApp);
            if (oApp == null)
            {
                throw new ApplicationException("Excelのインスタンス作成に失敗しました。");
            }
            oBooks = AppGetProperty("Workbooks");
            if (oBooks == null)
            {
                throw new ApplicationException("Excel.Workbooksの取得に失敗しました。");
            }
            Workbooks = oBooks.GetType();
        }

        public string Version
        {
            get
            {
                return (string)AppGetProperty("Version");
            }
        }

        public bool Visible
        {
            get
            {
                return (bool)AppGetProperty("Visible");
            }
            set
            {
                AppSetProperty1("Visible", value);
            }
        }

        public bool DisplayAlerts
        {
            get
            {
                return (bool)AppGetProperty("DisplayAlerts");
            }
            set
            {
                try
                {
                    AppSetProperty1("DisplayAlerts", value);
                }
                catch
                {
                }
            }
        }

        public XlWindowState WindowState
        {
            get
            {
                return (XlWindowState)AppGetProperty("WindowState");
            }
            set {
                AppSetProperty1("WindowState", (int)value);
            }
        }

        public bool DisplayGridlines
        {
            get
            {
                object window = AppGetProperty("ActiveWindow");
                if (window == null) return false;
                bool v = (bool)window.GetType().InvokeMember("DisplayGridlines", BindingFlags.GetProperty, null, oApp, null);
                ReleaseObj(window);
                return v;
            }
            set
            {
                object window = AppGetProperty("ActiveWindow");
                if (window == null) return;
                window.GetType().InvokeMember("DisplayGridlines", BindingFlags.SetProperty, null, oApp, new object[] { value });
                ReleaseObj(window);
            }
        }

        public string Title
        {
            get
            {
                object window = AppGetProperty("ActiveWindow");
                if (window == null) return null;
                string caption = (string)window.GetType().InvokeMember("Caption", BindingFlags.GetProperty, null, oApp, null);
                ReleaseObj(window);
                return caption;
            }
            set
            {
                object window = AppGetProperty("ActiveWindow");
                if (window == null) return;
                window.GetType().InvokeMember("Caption", BindingFlags.SetProperty, null, oApp, new object[] { value });
                ReleaseObj(window);
            }
        }

        public string StatusBar
        {
            get
            {
                return (string)AppGetProperty("StatusBar");
            }
            set
            {
                AppSetProperty1("StatusBar", value);
            }
        }

        public bool DisplayStatusBar
        {
            get
            {
                return (bool)AppGetProperty("DisplayStatusBar");
            }
            set
            {
                AppSetProperty1("DisplayStatusBar", value);
            }
        }

        public object BookAdd()
        {
            WorkbooksInvokeMethod("Add", null);
            AfterBookOpen();
            return oBook;
        }

        public void Open(string Filename)
        {
            object[] param = new object[] {
                // Filename, UpdateLinks, ReadOnly, Format, Password, WriteResPassword, IgnoreReadOnlyRecommended, Origin, Delimiter, Editable, Notify, Converter, AddToMru, Local, CorruptLoad
                Filename, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing
            };
            WorkbooksInvokeMethod("Open", param);
            AfterBookOpen();
        }

        public void SaveAs(string Filename, XlFileFormat FileFormat = XlFileFormat.xlWorkbookNormal)
        {
            // object Filename = Type.Missing, object FileFormat = Type.Missing, 
            // object Password = Type.Missing, object WriteResPassword = Type.Missing, 
            // object ReadOnlyRecommended = Type.Missing, object CreateBackup = Type.Missing, XlSaveAsAccessMode AccessMode = XlSaveAsAccessMode.xlNoChange, 
            // object ConflictResolution = Type.Missing, object AddToMru = Type.Missing, object TextCodepage = Type.Missing, object TextVisualLayout = Type.Missing, 
            // object Local = Type.Missing
            object[] param = new object[] {
                Filename, FileFormat, 
                Type.Missing, Type.Missing, 
                Type.Missing, Type.Missing,
                XlSaveAsAccessMode.xlNoChange, 
                Type.Missing, 
                Type.Missing, 
                Type.Missing, 
                Type.Missing
                // object Local = Type.Missing
            };
            oSheet.GetType().InvokeMember("SaveAs", BindingFlags.InvokeMethod, null, oSheet, param);
        }

        public void Save()
        {
            oSheet.GetType().InvokeMember("Save", BindingFlags.InvokeMethod, null, oSheet, null);
        }

        public void ExportAsFixedFormat(string Filename, XlFixedFormatType Format)
        {
            // ExportAsFixedFormat(
            //   XlFixedFormatType Type, object Filename = Type.Missing, object Quality = Type.Missing, 
            //   object IncludeDocProperties = Type.Missing, object IgnorePrintAreas = Type.Missing, 
            //   object From = Type.Missing, object To = Type.Missing,
            //   object OpenAfterPublish = Type.Missing, object FixedFormatExtClassPtr = Type.Missing);
        
            // ExportAsFixedFormat
            object[] param = new object[] {
                Format, Filename, Type.Missing, Type.Missing, Type.Missing, 
                Type.Missing, Type.Missing, Type.Missing, Type.Missing
            };
            oBook.GetType().InvokeMember("ExportAsFixedFormat", BindingFlags.InvokeMethod, null, oBook, param);
        }

        protected object getRange(string cell)
        {
            object range = oSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty, null, oSheet, new object[] { cell, Missing.Value });
            if (range == null) throw new ApplicationException("Rangeの取得ができません。");
            return range;
        }

        public void SetCell(string cell, string v)
        {
            if (oSheet == null)
            {
                BookAdd();
            }
            object range = getRange(cell);
            range.GetType().InvokeMember("Value", BindingFlags.SetProperty, null, range, new object[] { v });
            //
            ReleaseObj(range);
        }

        public string GetCell(string cell)
        {
            if (oSheet == null) return null;
            
            object range = getRange(cell);
            object value = range.GetType().InvokeMember("Value", BindingFlags.GetProperty, null, range, null);
            ReleaseObj(range);

            return (string)value;
        }

        protected void AfterBookOpen()
        {
            // 既存のCOMオブジェクトを破棄する
            DisposeSheetObject();

            // オブジェクトを更新する
            // xlBook = xlBooks[xlBooks.Count];
            oBook = Workbooks.InvokeMember("Item", BindingFlags.GetProperty, null, oBooks, new object[] { Workbooks_Count });
            if (oBook == null) throw new ApplicationException("ワークブックの取得に失敗しました。");
            // xlBook.Activate();
            oBook.GetType().InvokeMember("Activate", BindingFlags.InvokeMethod, null, oBook, null);
            // xlSheets = xlBook.Worksheets;
            oSheets = oBook.GetType().InvokeMember("Worksheets", BindingFlags.GetProperty, null, oBook, null);
            if (oSheets == null) throw new ApplicationException("ExcelのWorksheetsオブジェクトの取得に失敗しました。");
            // xlSheet = (NakoExcel._Worksheet)xlSheets[1];
            oSheet = oSheets.GetType().InvokeMember("Item", BindingFlags.GetProperty, null, oSheets, new object[] { 1 });
            // xlSheet.Activate();
            if (oSheet == null) throw new ApplicationException("ExcelのWorksheets[1]オブジェクトの取得に失敗しました。");
            oSheet.GetType().InvokeMember("Activate", BindingFlags.InvokeMethod, null, oSheet, null);
        }

        public int Workbooks_Count
        {
            get
            {
                return (int)WorkbooksGetProperty("Count");
            }
        }

        public void Quit()
        {
            this.DisplayAlerts = false;
            AppInvokeMethod("Quit", null);
        }

        public void ReleaseObj(object o)
        {
            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(o);
        }

        protected void DisposeSheetObject()
        {
            // Disppose Object
            if (oSheet != null) ReleaseObj(oSheet);
            if (oSheets != null) ReleaseObj(oSheets);
            if (oBook != null) ReleaseObj(oBook);
            // set null
            oSheet = null;
            oSheets = null;
            oBook = null;
        }

        public void Close()
        {
            this.DisplayAlerts = false;
            if (oBook != null)
            {
                oBook.GetType().InvokeMember("Close", BindingFlags.InvokeMethod, null, oBook, null);
            }
            if (oBooks != null)
            {
                oBooks.GetType().InvokeMember("Close", BindingFlags.InvokeMethod, null, oBooks, null);
            }
        }

        public void Dispose()
        {
            DisposeSheetObject();

            // Disppose Object
            if (oBooks != null) ReleaseObj(oBooks);
            if (oApp != null)
            {
                Quit();
                ReleaseObj(oApp);
            }
            // set null
            oBooks = null;
            oApp = null;
        }

        #region ExcelApp_Property
        public void AppSetProperty(string name, object[] param)
        {
            ExcelApp.InvokeMember(name, BindingFlags.SetProperty, null, oApp, param);
        }

        public void AppSetProperty1(string name, object param)
        {
            ExcelApp.InvokeMember(name, BindingFlags.SetProperty, null, oApp, new object[] { param });
        }

        public object AppGetProperty(string name)
        {
            return ExcelApp.InvokeMember(name, BindingFlags.GetProperty, null, oApp, null);
        }

        public object AppInvokeMethod(string name, object[] param)
        {
            return ExcelApp.InvokeMember(name, BindingFlags.InvokeMethod, null, oApp, param);
        }
        #endregion

        #region Workbooks Property
        public object WorkbooksGetProperty(string name)
        {
            return Workbooks.InvokeMember(name, BindingFlags.GetProperty, null, oBooks, null);
        }
        public object WorkbooksInvokeMethod(string name, object[] param)
        {
            return Workbooks.InvokeMember(name, BindingFlags.InvokeMethod, null, oBooks, param);
        }
        public void WorkbooksSetProperty(string name, object[] param)
        {
            ExcelApp.InvokeMember(name, BindingFlags.SetProperty, null, oBooks, param);
        }
        public void WorkbooksSetProperty1(string name, object param)
        {
            ExcelApp.InvokeMember(name, BindingFlags.SetProperty, null, oBooks, new object[] { param });
        }
        #endregion

    }

    public enum XlWindowState
    {
        xlNormal = -4143,
        xlMinimized = -4140,
        xlMaximized = -4137,
    }
    public enum XlFileFormat
    {
        xlCurrentPlatformText = -4158,
        xlWorkbookNormal = -4143,
        xlSYLK = 2,
        xlWKS = 4,
        xlWK1 = 5,
        xlCSV = 6,
        xlDBF2 = 7,
        xlDBF3 = 8,
        xlDIF = 9,
        xlDBF4 = 11,
        xlWJ2WD1 = 14,
        xlWK3 = 15,
        xlExcel2 = 16,
        xlTemplate = 17,
        xlTemplate8 = 17,
        xlAddIn = 18,
        xlAddIn8 = 18,
        xlTextMac = 19,
        xlTextWindows = 20,
        xlTextMSDOS = 21,
        xlCSVMac = 22,
        xlCSVWindows = 23,
        xlCSVMSDOS = 24,
        xlIntlMacro = 25,
        xlIntlAddIn = 26,
        xlExcel2FarEast = 27,
        xlWorks2FarEast = 28,
        xlExcel3 = 29,
        xlWK1FMT = 30,
        xlWK1ALL = 31,
        xlWK3FM3 = 32,
        xlExcel4 = 33,
        xlWQ1 = 34,
        xlExcel4Workbook = 35,
        xlTextPrinter = 36,
        xlWK4 = 38,
        xlExcel7 = 39,
        xlExcel5 = 39,
        xlWJ3 = 40,
        xlWJ3FJ3 = 41,
        xlUnicodeText = 42,
        xlExcel9795 = 43,
        xlHtml = 44,
        xlWebArchive = 45,
        xlXMLSpreadsheet = 46,
        xlExcel12 = 50,
        xlOpenXMLWorkbook = 51,
        xlWorkbookDefault = 51,
        xlOpenXMLWorkbookMacroEnabled = 52,
        xlOpenXMLTemplateMacroEnabled = 53,
        xlOpenXMLTemplate = 54,
        xlOpenXMLAddIn = 55,
        xlExcel8 = 56,
        xlOpenDocumentSpreadsheet = 60,
    }
    public enum XlSaveAsAccessMode
    {
        xlNoChange = 1,
        xlShared = 2,
        xlExclusive = 3,
    }
    public enum XlFixedFormatType
    {
        xlTypePDF = 0,
        xlTypeXPS = 1,
    }
}
