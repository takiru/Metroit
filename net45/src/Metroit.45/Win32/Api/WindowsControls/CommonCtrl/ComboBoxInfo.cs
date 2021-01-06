using Metroit.Win32.Api.DisplayDeviceReference.WinDef;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Metroit.Win32.Api.WindowsControls.CommonCtrl
{
    /// <summary>
    /// ComboBox の領域情報。
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class ComboBoxInfo
    {
        /// <summary>
        /// 構造体のサイズ（バイト単位）。呼び出し元のアプリケーションは、これをsizeof（COMBOBOXINFO）に設定する必要があります。
        /// </summary>
        public int cbSize;

        /// <summary>
        /// 編集ボックスの座標を指定するRECT構造。
        /// </summary>
        public Rect rcItem;

        /// <summary>
        /// ドロップダウン矢印を含むボタンの座標を指定するRECT構造。
        /// </summary>
        public Rect rcButton;

        /// <summary>
        /// コンボボックスボタンの状態。このパラメーターは、次のいずれかの値になります。
        ///  0                      ボタンが存在し、押されていません。
        ///  STATE_SYSTEM_INVISIBLE ボタンはありません。
        ///  STATE_SYSTEM_PRESSED   ボタンが押されました。
        /// </summary>
        public int stateButton;

        /// <summary>
        /// コンボボックスへのハンドル。
        /// </summary>
        public IntPtr hwndCombo;

        /// <summary>
        /// 編集ボックスへのハンドル。
        /// </summary>
        public IntPtr hwndItem;

        /// <summary>
        /// ドロップダウンリストへのハンドル。
        /// </summary>
        public IntPtr hwndList;

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        public ComboBoxInfo()
        {
            cbSize = Marshal.SizeOf(this);
        }
    }
}
