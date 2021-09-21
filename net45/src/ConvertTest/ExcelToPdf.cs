using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ConvertTest.Resources;
using Metroit.IO;

namespace ConvertTest
{
    /// <summary>
    /// excelをpdfに変換するコンバータ。
    /// </summary>
    public class ExcelToPdf : FileConverterBase
    {
        /// <summary>
        /// ExcelToPdf の新しいインスタンスを初期化します。
        /// </summary>
        public ExcelToPdf() : base() { }

        /// <summary>
        /// ExcelToPdf の新しいインスタンスを初期化します。
        /// </summary>
        public ExcelToPdf(FileConvertParameter parameter) : base(parameter) { }

        /// <summary>
        /// ファイルの変換処理を行います。
        /// </summary>
        protected override void ConvertFile(FileConvertParameter parameter)
        {
            dynamic xlApp = null;
            dynamic xlBooks = null;
            dynamic xlBook = null;

            try
            {
                xlApp = Activator.CreateInstance(Type.GetTypeFromProgID("Excel.Application"));
                xlBooks = xlApp.Workbooks;

                try
                {
                    xlBook = xlBooks.Open(parameter.SourceConvertFileName);
                }
                catch
                {
                    throw new FileConvertException(
                            string.Format(MessagesRes.UnreadableSourceFile,
                            parameter.SourceFileName), parameter);
                }

                var directory = System.IO.Path.GetDirectoryName(parameter.DestConvertFileName);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                xlBook.ExportAsFixedFormat(XlFixedFormatType.xlTypePDF, parameter.DestConvertFileName);
            }
            catch (Exception e)
            {
                throw new FileConvertException(
                        string.Format(MessagesRes.ConvertFailed,
                        e.Message), parameter);
            }
            finally
            {
                xlBook?.Close(false);
                xlApp?.Quit();
                if (xlBooks != null)
                {
                    Marshal.FinalReleaseComObject(xlBooks);
                }
                if (xlApp != null)
                {
                    Marshal.FinalReleaseComObject(xlApp);
                }
            }
        }
    }

    internal static class XlFixedFormatType
    {
        public const int xlTypePDF = 0;

    }
}
