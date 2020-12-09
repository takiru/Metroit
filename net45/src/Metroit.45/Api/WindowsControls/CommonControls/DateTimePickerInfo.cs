using Metroit.Api.Win32.Structures;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Metroit.Api.WindowsControls.CommonControls
{
    /// <summary>
    /// DateTimePicker の領域情報。
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class DateTimePickerInfo
    {
        /// <summary>
        /// 自身のアンマネージサイズ。
        /// </summary>
        public int cbSize;

        /// <summary>
        /// RECTのチェックボックスの位置を記述する構造。チェックボックスが表示されてチェックされている場合は、選択した日時値を更新するための編集コントロールを使用できる必要があります。
        /// </summary>
        public RECT rcCheck;

        /// <summary>
        /// 状態rcCheck用のオンオブジェクト状態定数など、STATE_SYSTEM_CHECKED又はSTATE_SYSTEM_INVISIBLE。
        /// </summary>
        public int stateCheck;

        /// <summary>
        /// RECTの制御下のドロップダウングリッドまたはアップ/の位置を記述する構造。
        /// </summary>
        public RECT rcButton;

        /// <summary>
        /// 状態 rcButton - 1つのビット単位の組み合わせオブジェクトの状態の定数など、STATE_SYSTEM_UNAVAILABLE、STATE_SYSTEM_INVISIBLE、又はSTATE_SYSTEM_PRESSED。アップ/ダウンコントロールが使用されている場合、ボタンの状態は STATE_SYSTEM_INVISIBLEです。
        /// </summary>
        public int stateButton;

        /// <summary>
        /// 編集コントロールへのハンドル。
        /// </summary>
        public IntPtr hwndEdit;

        /// <summary>
        /// アップ/ダウンコントロールへのハンドル ドロップダウングリッドを使用する代わりに（月のカレンダーコントロールのように見えます）。
        /// </summary>
        public IntPtr hwndUD;

        /// <summary>
        /// ドロップダウングリッドへのハンドル。
        /// </summary>
        public IntPtr hwndDropDown;

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        public DateTimePickerInfo()
        {
            cbSize = Marshal.SizeOf(this);
        }
    }
}
