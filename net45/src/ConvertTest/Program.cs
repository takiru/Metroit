using Metroit.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConvertTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var watch = Stopwatch.StartNew();

            Action<FileConvertParameter> convertSucceed = (p) =>
            {
                Console.WriteLine($"ConvertSucceed!");
                Console.WriteLine($"  Source={p.SourceFileName}");
                Console.WriteLine($"  Dest={p.DestFileName}");
                Console.WriteLine($"  OriginalFileName={p.OriginalFileName}");
                Console.WriteLine($"  SourceConvertFileName={p.SourceConvertFileName}");
                Console.WriteLine($"  DestConvertFileName={p.DestConvertFileName}");
            };

            Action<FileConvertParameter, Exception> convertFailed = (p, ex) =>
            {
                Console.WriteLine($"ConvertFailed!");
                Console.WriteLine($"  Source={p.SourceFileName}");
                Console.WriteLine($"  Dest={p.DestFileName}");
                Console.WriteLine($"  OriginalFileName={p.OriginalFileName}");
                Console.WriteLine($"  SourceConvertFileName={p.SourceConvertFileName}");
                Console.WriteLine($"  DestConvertFileName={p.DestConvertFileName}");
                Console.WriteLine($"  {ex.Message}");
            };

            Action<FileConvertParameter, ConvertCompleteEventArgs> completed = (p, e) =>
            {
                Console.WriteLine($"Completed!");
                Console.WriteLine($"  Status={e.Result}");
                Console.WriteLine($"  Source={p.SourceFileName}");
                Console.WriteLine($"  Dest={p.DestFileName}");
                Console.WriteLine($"  OriginalFileName={p.OriginalFileName}");
                Console.WriteLine($"  SourceConvertFileName={p.SourceConvertFileName}");
                Console.WriteLine($"  DestConvertFileName={p.DestConvertFileName}");
                Console.WriteLine($"  {e.Error?.Message}");
            };

            var files = Directory.GetFiles(@"C:\test");
            List<Task> tasks = new List<Task>();
            foreach (var file in files)
            {
                // ex.) Plain
                var conv1 = new FileCopy()
                {
                    //   Source=C:\test\Hoge.txt
                    //   Dest=C:\test\Hoge-copy1.txt
                    //   OriginalFileName=C:\test\Hoge.txt
                    //   SourceConvertFileName=C:\test\Hoge.txt
                    //   DestConvertFileName=C:\test\Hoge-copy1.txt
                    Parameter = new FileConvertParameter()
                    {
                        SourceFileName = file,
                        DestFileName = Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file) + "-copy1.txt"),
                        Overwrite = true
                    },
                    ConvertSucceed = convertSucceed,
                    ConvertFailed = convertFailed,
                    Completed = completed
                };
                tasks.Add(conv1.ConvertAsync());

                //// ex.) Use temporary
                //var conv2 = new FileCopy()
                //{
                //    //   Source=C:\test\Hoge.txt
                //    //   Dest=C:\test\Hoge-copy2.txt
                //    //   OriginalFileName=C:\Users\Metroit\AppData\Local\Temp\p1uluhp4.myl.txt
                //    //   SourceConvertFileName=C:\Users\Metroit\AppData\Local\Temp\p1uluhp4.myl.txt
                //    //   DestConvertFileName=C:\Users\Metroit\AppData\Local\Temp\peozivhh.lh0.txt
                //    Parameter = new FileConvertParameter()
                //    {
                //        SourceFileName = file,
                //        DestFileName = Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file) + "-copy2.txt"),
                //        Overwrite = true,
                //        UseSourceTemporary = true,
                //        UseDestTemporary = true
                //    },
                //    ConvertSucceed = convertSucceed,
                //    ConvertFailed = convertFailed,
                //    Completed = completed
                //};
                //tasks.Add(conv2.ConvertAsync());

                //// ex.) Reactive plain and reactive use temporary by original
                //var conv3 = new FileCopy()
                //{
                //    //   Source=C:\test\Hoge.txt
                //    //   Dest=C:\test\Hoge-copy3.txt
                //    //   OriginalFileName=C:\Users\Metroit\AppData\Local\Temp\5le3wpjj.i4m.txt
                //    //   SourceConvertFileName=C:\Users\Metroit\AppData\Local\Temp\5le3wpjj.i4m.txt
                //    //   DestConvertFileName=C:\Users\Metroit\AppData\Local\Temp\ndvbdq32.qqk.txt
                //    Parameter = new FileConvertParameter()
                //    {
                //        SourceFileName = file,
                //        DestFileName = Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file) + "-copy3.txt"),
                //        Overwrite = true,
                //        UseSourceTemporary = true,
                //        UseDestTemporary = true
                //    },
                //    ConvertSucceed = convertSucceed,
                //    ConvertFailed = convertFailed,
                //    Completed = completed,
                //    ReactiveConvert = new List<FileConverterBase>()
                //    {
                //        new FileCopy()
                //        {
                //            //   Source=C:\Users\Metroit\AppData\Local\Temp\5le3wpjj.i4m.txt
                //            //   Dest=C:\test\Hoge-copy3-reactive-original1.txt
                //            //   OriginalFileName=C:\Users\Metroit\AppData\Local\Temp\5le3wpjj.i4m.txt
                //            //   SourceConvertFileName=C:\Users\Metroit\AppData\Local\Temp\5le3wpjj.i4m.txt
                //            //   DestConvertFileName=C:\Users\Metroit\AppData\Local\Temp\pcexhiej.ess.txt
                //            Parameter = new FileConvertParameter()
                //            {
                //                ReactiveTarget = ReactiveFileTarget.Original,
                //                DestFileName = Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file) + "-copy3-reactive-original1.txt"),
                //                UseDestTemporary = true
                //            },
                //            ConvertSucceed = convertSucceed,
                //            ConvertFailed = convertFailed
                //        },
                //        new FileCopy()
                //        {
                //            //   Source=C:\Users\Metroit\AppData\Local\Temp\5le3wpjj.i4m.txt
                //            //   Dest=C:\test\Hoge-copy3-reactive-original2.txt
                //            //   OriginalFileName=C:\Users\Metroit\AppData\Local\Temp\5le3wpjj.i4m.txt
                //            //   SourceConvertFileName=C:\Users\Metroit\AppData\Local\Temp\rbvj1lpg.nay.txt
                //            //   DestConvertFileName=C:\Users\Metroit\AppData\Local\Temp\zy5jja2v.zcn.txt
                //            Parameter = new FileConvertParameter()
                //            {
                //                ReactiveTarget = ReactiveFileTarget.Original,
                //                DestFileName = Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file) + "-copy3-reactive-original2.txt"),
                //                UseSourceTemporary = true,
                //                UseDestTemporary = true
                //            },
                //            ConvertSucceed = convertSucceed,
                //            ConvertFailed = convertFailed,
                //        }
                //    }
                //};
                //tasks.Add(conv3.ConvertAsync());

                //// ex.) Reactive plain and reactive use temporary by RecentConvert
                //var conv4 = new FileCopy()
                //{
                //    //   Source=C:\test\Hoge.txt
                //    //   Dest=C:\test\Hoge-copy4.txt
                //    //   OriginalFileName=C:\Users\Metroit\AppData\Local\Temp\qdsgt1ve.3t0.txt
                //    //   SourceConvertFileName=C:\Users\Metroit\AppData\Local\Temp\qdsgt1ve.3t0.txt
                //    //   DestConvertFileName=C:\Users\Metroit\AppData\Local\Temp\zhulc1e1.rsp.txt
                //    Parameter = new FileConvertParameter()
                //    {
                //        SourceFileName = file,
                //        DestFileName = Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file) + "-copy4.txt"),
                //        Overwrite = true,
                //        UseSourceTemporary = true,
                //        UseDestTemporary = true
                //    },
                //    ConvertSucceed = convertSucceed,
                //    ConvertFailed = convertFailed,
                //    Completed = completed,
                //    ReactiveConvert = new List<FileConverterBase>()
                //    {
                //        new FileCopy()
                //        {
                //            //   Source=C:\Users\Metroit\AppData\Local\Temp\zhulc1e1.rsp.txt
                //            //   Dest=C:\test\Hoge-copy4-reactive-RecentConvert1.txt
                //            //   OriginalFileName=C:\Users\Metroit\AppData\Local\Temp\qdsgt1ve.3t0.txt
                //            //   SourceConvertFileName=C:\Users\Metroit\AppData\Local\Temp\zhulc1e1.rsp.txt
                //            //   DestConvertFileName=C:\Users\Metroit\AppData\Local\Temp\uzg35gal.tl2.txt
                //            Parameter = new FileConvertParameter()
                //            {
                //                ReactiveTarget = ReactiveFileTarget.RecentConvert,
                //                DestFileName = Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file) + "-copy4-reactive-RecentConvert1.txt"),
                //                UseDestTemporary = true
                //            },
                //            ConvertSucceed = convertSucceed,
                //            ConvertFailed = convertFailed
                //        },
                //        new FileCopy()
                //        {
                //            //   Source=C:\Users\Metroit\AppData\Local\Temp\zhulc1e1.rsp.txt
                //            //   Dest=C:\test\Hoge-copy4-reactive-RecentConvert2.txt
                //            //   OriginalFileName=C:\Users\Metroit\AppData\Local\Temp\qdsgt1ve.3t0.txt
                //            //   SourceConvertFileName=C:\Users\Metroit\AppData\Local\Temp\enbqkszf.mxv.txt
                //            //   DestConvertFileName=C:\Users\Metroit\AppData\Local\Temp\ikn4oltw.b0e.txt
                //            Parameter = new FileConvertParameter()
                //            {
                //                ReactiveTarget = ReactiveFileTarget.RecentConvert,
                //                DestFileName = Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file) + "-copy4-reactive-RecentConvert2.txt"),
                //                UseSourceTemporary = true,
                //                UseDestTemporary = true
                //            },
                //            ConvertSucceed = convertSucceed,
                //            ConvertFailed = convertFailed
                //        }
                //    }
                //};
                //tasks.Add(conv4.ConvertAsync());

                //// ex.) Reactive plain and reactive use temporary by RecentDest
                //var conv5 = new FileCopy()
                //{
                //    //   Source=C:\test\Hoge.txt
                //    //   Dest=C:\test\Hoge-copy5.txt
                //    //   OriginalFileName=C:\Users\Metroit\AppData\Local\Temp\vmui3s5f.rfl.txt
                //    //   SourceConvertFileName=C:\Users\Metroit\AppData\Local\Temp\vmui3s5f.rfl.txt
                //    //   DestConvertFileName=C:\Users\Metroit\AppData\Local\Temp\focnebiz.fnl.txt
                //    Parameter = new FileConvertParameter()
                //    {
                //        SourceFileName = file,
                //        DestFileName = Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file) + "-copy5.txt"),
                //        Overwrite = true,
                //        UseSourceTemporary = true,
                //        UseDestTemporary = true
                //    },
                //    ConvertSucceed = convertSucceed,
                //    ConvertFailed = convertFailed,
                //    Completed = completed,
                //    ReactiveConvert = new List<FileConverterBase>()
                //    {
                //        new FileCopy()
                //        {
                //            //   Source=C:\test\Hoge-copy5.txt
                //            //   Dest=C:\test\Hoge-copy5-reactive-RecentDest1.txt
                //            //   OriginalFileName=C:\Users\Metroit\AppData\Local\Temp\vmui3s5f.rfl.txt
                //            //   SourceConvertFileName=C:\test\Hoge-copy5.txt
                //            //   DestConvertFileName=C:\Users\Metroit\AppData\Local\Temp\acjxxzpv.1id.txt
                //            Parameter = new FileConvertParameter()
                //            {
                //                ReactiveTarget = ReactiveFileTarget.RecentDest,
                //                DestFileName = Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file) + "-copy5-reactive-RecentDest1.txt"),
                //                UseDestTemporary = true
                //            },
                //            ConvertSucceed = convertSucceed,
                //            ConvertFailed = convertFailed
                //        },
                //        new FileCopy()
                //        {
                //            //   Source=C:\test\Hoge-copy5.txt
                //            //   Dest=C:\test\Hoge-copy5-reactive-RecentDest2.txt
                //            //   OriginalFileName=C:\Users\Metroit\AppData\Local\Temp\vmui3s5f.rfl.txt
                //            //   SourceConvertFileName=C:\Users\Metroit\AppData\Local\Temp\oyjqmbuk.wls.txt
                //            //   DestConvertFileName=C:\Users\Metroit\AppData\Local\Temp\rtk5whpp.osu.txt
                //            Parameter = new FileConvertParameter()
                //            {
                //                ReactiveTarget = ReactiveFileTarget.RecentDest,
                //                DestFileName = Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file) + "-copy5-reactive-RecentDest2.txt"),
                //                UseSourceTemporary = true,
                //                UseDestTemporary = true
                //            },
                //            ConvertSucceed = convertSucceed,
                //            ConvertFailed = convertFailed
                //        }
                //    }
                //};
                //tasks.Add(conv5.ConvertAsync());

                //// ex.) Reactive plain and reactive use temporary by RecentOriginal
                //var conv6 = new FileCopy()
                //{
                //    //   Source=C:\test\Hoge.txtMetroit
                //    //   Dest=C:\test\Hoge-copy6.txt
                //    //   OriginalFileName=C:\Users\Metroit\AppData\Local\Temp\zr2cncil.o3b.txt
                //    //   SourceConvertFileName=C:\Users\Metroit\AppData\Local\Temp\zr2cncil.o3b.txt
                //    //   DestConvertFileName=C:\Users\Metroit\AppData\Local\Temp\loms5wsk.kd3.txt
                //    Parameter = new FileConvertParameter()
                //    {
                //        SourceFileName = file,
                //        DestFileName = Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file) + "-copy6.txt"),
                //        Overwrite = true,
                //        UseSourceTemporary = true,
                //        UseDestTemporary = true
                //    },
                //    ConvertSucceed = convertSucceed,
                //    ConvertFailed = convertFailed,
                //    Completed = completed,
                //    ReactiveConvert = new List<FileConverterBase>()
                //    {
                //        new FileCopy()
                //        {
                //            //   Source=C:\Users\Metroit\AppData\Local\Temp\zr2cncil.o3b.txt
                //            //   Dest=C:\test\Hoge-copy6-reactive-RecentOriginal1.txt
                //            //   OriginalFileName=C:\Users\Metroit\AppData\Local\Temp\zr2cncil.o3b.txt
                //            //   SourceConvertFileName=C:\Users\Metroit\AppData\Local\Temp\zr2cncil.o3b.txt
                //            //   DestConvertFileName=C:\Users\Metroit\AppData\Local\Temp\aosqino0.ig3.txt
                //            Parameter = new FileConvertParameter()
                //            {
                //                ReactiveTarget = ReactiveFileTarget.RecentOriginal,
                //                DestFileName = Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file) + "-copy6-reactive-RecentOriginal1.txt"),
                //                UseDestTemporary = true
                //            },
                //            ConvertSucceed = convertSucceed,
                //            ConvertFailed = convertFailed,
                //            ReactiveConvert = new List<FileConverterBase>()
                //            {
                //                new FileCopy()
                //                {
                //                    //   Source=C:\Users\Metroit\AppData\Local\Temp\zr2cncil.o3b.txt
                //                    //   Dest=C:\test\Hoge-copy6-reactive-RecentOriginal3.txt
                //                    //   OriginalFileName=C:\Users\Metroit\AppData\Local\Temp\zr2cncil.o3b.txt
                //                    //   SourceConvertFileName=C:\Users\Metroit\AppData\Local\Temp\zr2cncil.o3b.txt
                //                    //   DestConvertFileName=C:\Users\Metroit\AppData\Local\Temp\evc2oz2h.ib0.txt
                //                    Parameter = new FileConvertParameter()
                //                    {
                //                        ReactiveTarget = ReactiveFileTarget.RecentOriginal,
                //                        DestFileName = Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file) + "-copy6-reactive-RecentOriginal3.txt"),
                //                        UseDestTemporary = true
                //                    },
                //                    ConvertSucceed = convertSucceed,
                //                    ConvertFailed = convertFailed
                //                }
                //            }
                //        },
                //        new FileCopy()
                //        {
                //            //   Source=C:\Users\Metroit\AppData\Local\Temp\zr2cncil.o3b.txt
                //            //   Dest=C:\test\Hoge-copy6-reactive-RecentOriginal2.txt
                //            //   OriginalFileName=C:\Users\Metroit\AppData\Local\Temp\zr2cncil.o3b.txt
                //            //   SourceConvertFileName=C:\Users\Metroit\AppData\Local\Temp\pprqadn3.nbe.txt
                //            //   DestConvertFileName=C:\Users\Metroit\AppData\Local\Temp\nrabzpdj.3rb.txt
                //            Parameter = new FileConvertParameter()
                //            {
                //                ReactiveTarget = ReactiveFileTarget.RecentOriginal,
                //                DestFileName = Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file) + "-copy6-reactive-RecentOriginal2.txt"),
                //                UseSourceTemporary = true,
                //                UseDestTemporary = true
                //            },
                //            ConvertSucceed = convertSucceed,
                //            ConvertFailed = convertFailed
                //        }
                //    }
                //};
                //tasks.Add(conv6.ConvertAsync());

                //// ex.) Reactive DestThrough
                //var conv7 = new FileCopy()
                //{
                //    //   Source=C:\test\Hoge.txt
                //    //   Dest=C:\test\Hoge-copy7.txt
                //    //   OriginalFileName=C:\Users\Metroit\AppData\Local\Temp\hw4bsv3k.zgi.txt
                //    //   SourceConvertFileName=C:\Users\Metroit\AppData\Local\Temp\hw4bsv3k.zgi.txt
                //    //   DestConvertFileName=C:\Users\Metroit\AppData\Local\Temp\jql12o0j.t4x.txt
                //    Parameter = new FileConvertParameter()
                //    {
                //        SourceFileName = file,
                //        DestFileName = Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file) + "-copy7.txt"),
                //        Overwrite = true,
                //        UseSourceTemporary = true,
                //        UseDestTemporary = true,
                //        DestThrough = true
                //    },
                //    ConvertSucceed = convertSucceed,
                //    ConvertFailed = convertFailed,
                //    Completed = completed,
                //    ReactiveConvert = new List<FileConverterBase>()
                //    {
                //        new FileCopy()
                //        {
                //            //   Source=C:\Users\Metroit\AppData\Local\Temp\hw4bsv3k.zgi.txt
                //            //   Dest=C:\test\Hoge-copy7-DestThrough-reactive1.txt
                //            //   OriginalFileName=C:\Users\Metroit\AppData\Local\Temp\hw4bsv3k.zgi.txt
                //            //   SourceConvertFileName=C:\Users\Metroit\AppData\Local\Temp\hw4bsv3k.zgi.txt
                //            //   DestConvertFileName=C:\Users\Metroit\AppData\Local\Temp\mg2udfu4.icu.txt
                //            Parameter = new FileConvertParameter()
                //            {
                //                ReactiveTarget = ReactiveFileTarget.Original,
                //                DestFileName = Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file) + "-copy7-DestThrough-reactive1.txt"),
                //                UseDestTemporary = true
                //            },
                //            ConvertSucceed = convertSucceed,
                //            ConvertFailed = convertFailed
                //        },
                //        new FileCopy()
                //        {
                //            //   Source=C:\Users\Metroit\AppData\Local\Temp\hw4bsv3k.zgi.txt
                //            //   Dest=C:\test\Hoge-copy7-DestThrough-reactive2.txt
                //            //   OriginalFileName=C:\Users\Metroit\AppData\Local\Temp\hw4bsv3k.zgi.txt
                //            //   SourceConvertFileName=C:\Users\Metroit\AppData\Local\Temp\up5f1jdb.4gz.txt
                //            //   DestConvertFileName=C:\Users\Metroit\AppData\Local\Temp\12w2y1gq.yr5.txt
                //            Parameter = new FileConvertParameter()
                //            {
                //                ReactiveTarget = ReactiveFileTarget.Original,
                //                DestFileName = Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file) + "-copy7-DestThrough-reactive2.txt"),
                //                UseSourceTemporary = true,
                //                UseDestTemporary = true
                //            },
                //            ConvertSucceed = convertSucceed,
                //            ConvertFailed = convertFailed
                //        }
                //    }
                //};
                //tasks.Add(conv7.ConvertAsync());

            }

            Task.WaitAll(tasks.ToArray());
            watch.Stop();
            Console.WriteLine("Finish");
            Console.WriteLine("経過時間：" + watch.Elapsed.TotalSeconds.ToString());
            Console.ReadLine();
        }
    }
}
