using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// <see cref="Panel">Panel</see> を拡張した制御を提供します。
    /// </summary>
    public class MetPanel : Panel
    {
        private Point _savedScrollPosition;

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        public MetPanel() : base() { }

        /// <summary>
        /// <see cref="Control.Controls">Controls</see> にコントロールが追加または削除されたとき、スクロールバーの位置を保持するかどうかを取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(false)]
        [MetCategory("MetBehavior")]
        [Description("Controls にコントロールが追加または削除されたとき、スクロールバーの位置を保持するかどうかを取得または設定します。")]
        public bool ScrollPreserve { get; set; } = false;

        /// <summary>
        /// <see cref="Control.ControlAdded">ControlAdded</see> イベントを発生させます。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnControlAdded(ControlEventArgs e)
        {
            // スクロール位置を保存
            if (ScrollPreserve)
            {
                _savedScrollPosition = new Point(
                    Math.Abs(AutoScrollPosition.X),
                    Math.Abs(AutoScrollPosition.Y)
                );
            }

            base.OnControlAdded(e);

            // スクロール位置を復元
            if (ScrollPreserve)
            {
                BeginInvoke(new Action(() =>
                {
                    AutoScrollPosition = _savedScrollPosition;
                }));
            }
        }

        /// <summary>
        /// <see cref="Control.OnControlRemoved">OnControlRemoved</see> イベントを発生させます。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnControlRemoved(ControlEventArgs e)
        {
            // スクロール位置を保存
            if (ScrollPreserve)
            {
                _savedScrollPosition = new Point(
                    Math.Abs(AutoScrollPosition.X),
                    Math.Abs(AutoScrollPosition.Y)
                );
            }

            base.OnControlRemoved(e);

            // スクロール位置を復元
            if (ScrollPreserve)
            {
                BeginInvoke(new Action(() =>
                {
                    AutoScrollPosition = _savedScrollPosition;
                }));
            }
        }
    }
}
