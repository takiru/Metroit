using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Metroit.IO;

namespace ConvertTest
{
    /// <summary>
    /// ショートカットを作成するコンバータ。
    /// </summary>
    public class FileShortcut : FileConverterBase
    {
        /// <summary>
        /// Shortcut の新しいインスタンスを初期化します。
        /// </summary>
        public FileShortcut() : base() { }

        /// <summary>
        /// Shortcut の新しいインスタンスを初期化します。
        /// </summary>
        public FileShortcut(FileConvertParameter parameter) : base(parameter) { }

        /// <summary>
        /// ファイルの変換処理を行います。
        /// </summary>
        protected override void ConvertFile(FileConvertParameter parameter)
        {
            //作成するショートカットのパス
            string shortcutPath = parameter.DestConvertFileName;

            //ショートカットのリンク先
            string targetPath = Parent.Parameter.DestFileName;

            //WshShellを作成
            Type t = Type.GetTypeFromCLSID(new Guid("72C24DD5-D70A-438B-8A42-98424B88AFB8"));
            dynamic shell = Activator.CreateInstance(t);

            //WshShortcutを作成
            var shortcut = shell.CreateShortcut(shortcutPath);

            //リンク先
            shortcut.TargetPath = targetPath;
            //アイコンのパス
            shortcut.IconLocation = Parent.Parameter.DestFileName + ",0";

            //作業フォルダ
            shortcut.WorkingDirectory = Path.GetDirectoryName(Parent.Parameter.DestFileName);

            //その他のプロパティも同様に設定できるため、省略

            //ショートカットを作成
            shortcut.Save();

            //後始末
            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(shortcut);
            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(shell);
        }
    }
}
