using Metroit.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var watch = Stopwatch.StartNew();

            Action<FileConvertParameter, ConvertCompleteEventArgs> convertComplete = (p, e) =>
            {
                Console.WriteLine($"{e.Result}");
                Console.WriteLine($"  Original={p.OriginalFileName}");
                Console.WriteLine($"  Source={p.SourceFileName}");
                Console.WriteLine($"  SourceConvert={p.SourceConvertFileName}");
                Console.WriteLine($"  DestConvert={p.DestConvertFileName}");
                Console.WriteLine($"  Dest={p.DestFileName}");
                Console.WriteLine($"  {e.Error?.Message}");
            };

            var files = Directory.GetFiles(@"C:\test");
            List<Task> tasks = new List<Task>();
            foreach (var file in files)
            {
                var topConv = new ExcelToPdf()
                {
                    Parameter = new FileConvertParameter()
                    {
                        SourceFileName = file,
                        DestFileName = Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file) + ".pdf"),
                        UseSourceTemporary = true,
                        SourceTempDirectory = Path.Combine(Path.GetTempPath(), "Test", "TestSource"),
                        UseDestTemporary = true,
                        DestTempDirectory = Path.Combine(Path.GetTempPath(), "Test", "TestDest")
                    },
                    ReactiveConvert = new List<FileConverterBase>()
                    {
                        // オリジナルから変換
                        new ExcelToPdf()
                        {
                            Parameter = new FileConvertParameter()
                            {
                                ReactiveTarget = ReactiveFileTarget.Original,
                                DestFileName = Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file) + "-2.pdf"),
                                UseDestTemporary = true,
                                DestTempDirectory = Path.Combine(Path.GetTempPath(), "Test", "TestDest2")
                            },
                            ReactiveConvert = new List<FileConverterBase>()
                            {
                                // 変換されたファイルからコピー
                                new FileCopy()
                                {
                                    Parameter = new FileConvertParameter()
                                    {
                                        ReactiveTarget = ReactiveFileTarget.RecentConvert,
                                        DestFileName = Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file) + "-2-copied.pdf")
                                    },
                                    Complete = convertComplete
                                },
                                // オリジナルから変換
                                new ExcelToPdf()
                                {
                                    Parameter = new FileConvertParameter()
                                    {
                                        ReactiveTarget = ReactiveFileTarget.Original,
                                        DestFileName = Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file) + "-3.pdf"),
                                        UseDestTemporary = true,
                                        DestTempDirectory = Path.Combine(Path.GetTempPath(), "Test", "TestDest3")
                                    },
                                    Complete = convertComplete
                                }
                            },
                            Complete = convertComplete
                        }
                    },
                    Complete = convertComplete
                };

                //var result = topConv.Convert();
                var result = topConv.ConvertAsync();
                tasks.Add(result);
            }

            Task.WaitAll(tasks.ToArray());
            watch.Stop();
            Console.WriteLine("Finish");
            Console.WriteLine("経過時間：" + watch.Elapsed.TotalSeconds.ToString());
            Console.ReadLine();
        }
    }
}
