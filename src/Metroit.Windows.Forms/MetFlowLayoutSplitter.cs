using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// <see cref="FlowLayoutPanel"/> 内で使用する、サイズ変更用のスプリッターコントロールを提供します。
    /// </summary>
    public class MetFlowLayoutSplitter : Panel
    {
        private int _minSize = 25;

        /// <summary>
        /// サイズを変更するコントロールの最小サイズを指定します。
        /// </summary>
        [Browsable(true)]
        [Localizable(true)]
        [DefaultValue(25)]
        [MetCategory("MetBehavior")]
        [MetDescription("MetFlowLayoutSplitterMinSize")]
        public int MinSize
        {
            get => _minSize;
            set
            {
                if (value < 0)
                {
                    value = 0;
                }

                _minSize = value;
            }
        }

        private bool _isDragging = false;
        private Control _previousControl;
        private DockStyle _previousDock;
        private int _savedWidth;
        private int _savedHeight;

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        public MetFlowLayoutSplitter()
        {
            Width = 3;
            _savedWidth = Width;
            _savedHeight = Height;
            _previousDock = Dock;

            MouseDown += OnMouseDown;
            MouseMove += OnMouseMove;
            MouseUp += OnMouseUp;
            MouseLeave += OnMouseLeave;

            UpdateCursor();
        }

        /// <summary>
        /// 描画時の幅／高さを保持する。
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="specified"></param>
        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            // 現在のサイズを保存
            _savedWidth = Width;
            _savedHeight = Height;

            base.SetBoundsCore(x, y, width, height, specified);
        }

        /// <summary>
        /// Dock プロパティの状態に応じて高さ／幅を切り替える。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDockChanged(EventArgs e)
        {
            base.OnDockChanged(e);

            // 以前のDockから新しいDockへの変更を検出
            bool wasHorizontal = (_previousDock == DockStyle.Top || _previousDock == DockStyle.Bottom);
            bool isHorizontal = (Dock == DockStyle.Top || Dock == DockStyle.Bottom);

            // Top/Bottom ⇔ Left/Right の変更の場合、保存していたサイズを入れ替える
            if (wasHorizontal != isHorizontal)
            {
                if (isHorizontal)
                {
                    // Left/Right → Top/Bottom: 保存していたWidth → Height
                    Height = _savedWidth;
                }
                else
                {
                    // Top/Bottom → Left/Right: 保存していたHeight → Width
                    Width = _savedHeight;
                }
            }

            _previousDock = Dock;
            UpdateCursor();
        }

        /// <summary>
        /// カーソルアイコンを変更する。
        /// </summary>
        private void UpdateCursor()
        {
            if (Dock == DockStyle.Top || Dock == DockStyle.Bottom)
            {
                Cursor = Cursors.HSplit;
            }
            else if (Dock == DockStyle.Left || Dock == DockStyle.Right)
            {
                Cursor = Cursors.VSplit;
            }
        }

        /// <summary>
        /// ドラッグを捕捉する。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _isDragging = true;

                // マウスをキャプチャして、マウスがコントロール外に出ても追跡する
                this.Capture = true;

                // FlowLayoutPanel内の前のコントロールを取得
                var parent = this.Parent as FlowLayoutPanel;
                if (parent != null)
                {
                    int index = parent.Controls.GetChildIndex(this);
                    if (index > 0)
                    {
                        _previousControl = parent.Controls[index - 1];
                    }
                }
            }
        }

        /// <summary>
        /// ドラッグによって自身の直前にあるコントロールのサイズを変更する。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (_isDragging && _previousControl != null)
            {
                if (Dock == DockStyle.Top || Dock == DockStyle.Bottom)
                {
                    // スクリーン座標からの相対位置を計算
                    Point screenPoint = PointToScreen(new Point(e.X, e.Y));
                    Point parentPoint = Parent.PointToClient(screenPoint);

                    int deltaY = parentPoint.Y - (_previousControl.Top + _previousControl.Height + _previousControl.Margin.Bottom + Margin.Top);
                    int newHeight = _previousControl.Height + deltaY;

                    // 最大値を制限
                    int maxHeight = Parent.Height - _previousControl.Top - _previousControl.Margin.Bottom - Height - Margin.Top - Margin.Bottom;
                    newHeight = Math.Min(newHeight, maxHeight);
                    if (newHeight > MinSize)
                    {
                        _previousControl.Height = newHeight;
                    }
                }
                else if (Dock == DockStyle.Left || Dock == DockStyle.Right)
                {
                    // スクリーン座標からの相対位置を計算
                    Point screenPoint = PointToScreen(new Point(e.X, e.Y));
                    Point parentPoint = Parent.PointToClient(screenPoint);

                    int deltaX = parentPoint.X - (_previousControl.Left + _previousControl.Width + _previousControl.Margin.Right + Margin.Left);
                    int newWidth = _previousControl.Width + deltaX;

                    // 最大値を制限
                    int maxWidth = Parent.Width - _previousControl.Left - _previousControl.Margin.Right - Width - Margin.Left - Margin.Right;
                    newWidth = Math.Min(newWidth, maxWidth);
                    if (newWidth > MinSize)
                    {
                        _previousControl.Width = newWidth;
                    }
                }
            }
        }

        /// <summary>
        /// ドラッグを終了する。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (_isDragging)
            {
                _isDragging = false;
                _previousControl = null;
                Capture = false;
            }
        }

        /// <summary>
        /// マウスがコントロールから離れてもドラッグ中は処理を継続するようにする。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseLeave(object sender, EventArgs e)
        {
            // NOTE: Capture = true の場合、MouseMoveイベントは継続して発生する
        }
    }
}