using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Metroit.Windows.Forms.Extensions;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// 標準CheckBoxを拡張し、新たにいくつかの機能を設けたテキストエリアを提供します。
    /// </summary>
    public class MetCheckBox : CheckBox, IControlRollback
    {
        private int _focusWidth = 4;

        /// <summary>
        /// フォーカスを得た時に表示されるフレームのサイズを取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(4)]
        [MetCategory("MetAppearance")]
        [MetDescription("MetCheckBoxFocusWidth")]
        public int FocusWidth
        {
            get => _focusWidth;
            set
            {
                if (_focusWidth != value)
                {
                    _focusWidth = value;
                    if (AutoSize)
                    {
                        Size = GetPreferredSize(Size.Empty);
                    }
                    Invalidate();
                }
            }
        }

        private Color _focusColor = Color.FromArgb(152, 193, 254);

        /// <summary>
        /// フォーカスを得たときに表示されるフレームの境界線色を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof(Color), "152, 193, 254")]
        [MetCategory("MetAppearance")]
        [MetDescription("MetCheckBoxFocusColor")]
        public Color FocusColor
        {
            get => _focusColor;
            set
            {
                if (_focusColor != value)
                {
                    _focusColor = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// 既定のチェックボックス描画の角丸半径。
        /// </summary>
        private static readonly int DefaultCheckBoxRadius = 3;

        private int _checkBoxRadius = -1;

        /// <summary>
        /// チェックボックス描画の角丸半径を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(-1)]
        [MetCategory("MetAppearance")]
        [MetDescription("MetCheckBoxCheckBoxRadius")]
        public int CheckBoxRadius
        {
            get => _checkBoxRadius;
            set
            {
                if (_checkBoxRadius != value)
                {
                    _checkBoxRadius = value;
                    if (_checkBoxRadius < -1)
                    {
                        _checkBoxRadius = -1;
                    }
                    Invalidate();
                }
            }
        }

        private Color _checkColor = Color.Empty;

        /// <summary>
        /// チェックの色を取得または設定します。
        /// </summary>
        [MetCategory("MetAppearance")]
        [DefaultValue(typeof(Color), "Empty")]
        [MetDescription("MetCheckBoxCheckColor")]
        public Color CheckColor
        {
            get => _checkColor;
            set
            {
                if (_checkColor != value)
                {
                    _checkColor = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// コンポーネントがチェックされた状態かどうかを示します。
        /// </summary>
        [Browsable(true)]
        [MetCategory("MetAppearance")]
        [MetDescription("MetCheckBoxChecked")]
        public new bool Checked
        {
            get => base.Checked;
            set
            {
                if (Checked != value)
                {
                    var checkedCancelArgs = new CheckedChangingEventArgs(Checked, value);
                    OnCheckedChanging(checkedCancelArgs);
                    if (checkedCancelArgs.Cancel)
                    {
                        return;
                    }
                    var checkStateCancelArgs = new CheckStateChangingEventArgs(CheckState,
                        value ? CheckState.Checked : CheckState.Unchecked);
                    OnCheckStateChanging(checkStateCancelArgs);
                    if (checkStateCancelArgs.Cancel)
                    {
                        return;
                    }

                    base.Checked = value;
                }
            }
        }

        /// <summary>
        /// コンポーネントの状態を示します。
        /// </summary>
        [Browsable(true)]
        [MetCategory("MetAppearance")]
        [MetDescription("MetCheckBoxCheckState")]
        public new CheckState CheckState
        {
            get => base.CheckState;
            set
            {
                if (CheckState != value)
                {
                    var checkedCancelArgs = new CheckedChangingEventArgs(Checked, GetCheckedByState(value));
                    OnCheckedChanging(checkedCancelArgs);
                    if (checkedCancelArgs.Cancel)
                    {
                        return;
                    }
                    var checkStateCancelArgs = new CheckStateChangingEventArgs(CheckState, value);
                    OnCheckStateChanging(checkStateCancelArgs);
                    if (checkStateCancelArgs.Cancel)
                    {
                        return;
                    }

                    base.CheckState = value;
                }
            }
        }

        /// <summary>
        /// CheckState から、Checked となる値を求める。
        /// </summary>
        /// <param name="checkState">ステート値。</param>
        /// <returns>Checked となる値。</returns>
        private bool GetCheckedByState(CheckState checkState)
        {
            switch (checkState)
            {
                case CheckState.Checked:
                    return true;

                case CheckState.Indeterminate:
                    return true;

                default:
                    return false;
            }
        }

        /// <summary>
        /// 既定のボーダー色が設定されていなかったときの既定値。
        /// </summary>
        private readonly static Color DefaultBorderColor = Color.FromArgb(137, 137, 137);

        /// <summary>
        /// 既定の背景色が設定されていなかったときの既定値。
        /// </summary>
        private new readonly static Color DefaultBackColor = Color.FromArgb(245, 245, 245);


        private CheckBoxAppearance _checkedAppearance;

        /// <summary>
        /// チェックされたときの外観を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [MetCategory("MetAppearance")]
        [MetDescription("MetCheckBoxCheckedAppearance")]
        public CheckBoxAppearance CheckedAppearance
        {
            get
            {
                if (_checkedAppearance == null)
                {
                    _checkedAppearance = new CheckBoxAppearance(this, "Checked", DefaultControllers);
                }

                return _checkedAppearance;
            }
        }

        private CheckBoxAppearance _uncheckedAppearance;

        /// <summary>
        /// チェックされていないときの外観を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [MetCategory("MetAppearance")]
        [MetDescription("MetCheckBoxUncheckedAppearance")]
        public CheckBoxAppearance UncheckedAppearance
        {
            get
            {
                if (_uncheckedAppearance == null)
                {
                    _uncheckedAppearance = new CheckBoxAppearance(this, "Unchecked", DefaultControllers);
                }

                return _uncheckedAppearance;
            }
        }

        /// <summary>
        /// Checked プロパティが変更されるときに発生します。
        /// </summary>
        [Browsable(true)]
        [MetCategory("MetOther")]
        [MetDescription("MetCheckBoxCheckedChanging")]
        public event CheckedChangingEventHandler CheckedChanging;

        /// <summary>
        /// <see cref="CheckedChanging"/> イベントを発生させます。
        /// </summary>
        /// <param name="e">イベントデータ。</param>
        protected virtual void OnCheckedChanging(CheckedChangingEventArgs e)
        {
            CheckedChanging?.Invoke(this, e);
        }

        /// <summary>
        /// CheckState プロパティが変更されたときの発生します。
        /// </summary>
        [Browsable(true)]
        [MetCategory("MetOther")]
        [MetDescription("MetCheckBoxCheckStateChanging")]
        public event CheckStateChangingEventHandler CheckStateChanging;

        /// <summary>
        /// <see cref="CheckStateChanging"/> イベントを発生させます。
        /// </summary>
        /// <param name="e">イベントデータ。</param>
        protected virtual void OnCheckStateChanging(CheckStateChangingEventArgs e)
        {
            if (_internalStateChange)
            {
                return;
            }

            CheckStateChanging?.Invoke(this, e);
        }

        /// <summary>
        /// デザイナによる色設定制御。
        /// </summary>
        private List<PropertyDefaultController> DefaultControllers = null;

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        public MetCheckBox()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.DoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.SupportsTransparentBackColor, true);

            // デザイン時は プロパティリセット制御の設定
            if (this.IsDesignMode())
            {
                // Bootstrap5 に近しいデフォルトの色設定
                DefaultControllers = new List<PropertyDefaultController>()
                {
                    new PropertyDefaultController("Checked.Default.BorderColor",
                        () => CheckedAppearance.Default.BorderColor != Color.FromArgb(13, 110, 253),
                        () => CheckedAppearance.Default.BorderColor = Color.FromArgb(13, 110, 253)),
                    new PropertyDefaultController("Checked.Default.BackColor",
                        () => CheckedAppearance.Default.BackColor != Color.FromArgb(13, 110, 253),
                        () => CheckedAppearance.Default.BackColor = Color.FromArgb(13, 110, 253)),
                    new PropertyDefaultController("Checked.MouseOver.BorderColor",
                        () => CheckedAppearance.MouseOver.BorderColor != Color.FromArgb(13, 110, 253),
                        () => CheckedAppearance.MouseOver.BorderColor = Color.FromArgb(13, 110, 253)),
                    new PropertyDefaultController("Checked.MouseOver.BackColor",
                        () => CheckedAppearance.MouseOver.BackColor != Color.FromArgb(13, 110, 253),
                        () => CheckedAppearance.MouseOver.BackColor = Color.FromArgb(13, 110, 253)),
                    new PropertyDefaultController("Checked.MouseDown.BorderColor",
                        () => CheckedAppearance.MouseDown.BorderColor != Color.FromArgb(12, 99, 228),
                        () => CheckedAppearance.MouseDown.BorderColor = Color.FromArgb(12, 99, 228)),
                    new PropertyDefaultController("Checked.MouseDown.BackColor",
                        () => CheckedAppearance.MouseDown.BackColor != Color.FromArgb(12, 99, 228),
                        () => CheckedAppearance.MouseDown.BackColor = Color.FromArgb(12, 99, 228)),
                    new PropertyDefaultController("Unchecked.Default.BorderColor",
                        () => UncheckedAppearance.Default.BorderColor != Color.FromArgb(222, 226, 230),
                        () => UncheckedAppearance.Default.BorderColor = Color.FromArgb(222, 226, 230)),
                    new PropertyDefaultController("Unchecked.Default.BackColor",
                        () => UncheckedAppearance.Default.BackColor !=  Color.White,
                        () => UncheckedAppearance.Default.BackColor =  Color.White),
                    new PropertyDefaultController("Unchecked.MouseOver.BorderColor",
                        () => UncheckedAppearance.MouseOver.BorderColor != Color.FromArgb(222, 226, 230),
                        () => UncheckedAppearance.MouseOver.BorderColor = Color.FromArgb(222, 226, 230)),
                    new PropertyDefaultController("Unchecked.MouseOver.BackColor",
                        () => UncheckedAppearance.MouseOver.BackColor != Color.White,
                        () => UncheckedAppearance.MouseOver.BackColor = Color.White),
                    new PropertyDefaultController("Unchecked.MouseDown.BorderColor",
                        () => UncheckedAppearance.MouseDown.BorderColor != Color.FromArgb(121, 165, 229),
                        () => UncheckedAppearance.MouseDown.BorderColor = Color.FromArgb(121, 165, 229)),
                    new PropertyDefaultController("Unchecked.MouseDown.BackColor",
                        () => UncheckedAppearance.MouseDown.BackColor != Color.FromArgb(230, 230, 230),
                        () => UncheckedAppearance.MouseDown.BackColor = Color.FromArgb(230, 230, 230))
                };
            }

            // 既定値
            CheckedAppearance.Default.BorderColor = Color.FromArgb(13, 110, 253);
            CheckedAppearance.Default.BackColor = Color.FromArgb(13, 110, 253);
            CheckedAppearance.MouseOver.BorderColor = Color.FromArgb(13, 110, 253);
            CheckedAppearance.MouseOver.BackColor = Color.FromArgb(13, 110, 253);
            CheckedAppearance.MouseDown.BorderColor = Color.FromArgb(12, 99, 228);
            CheckedAppearance.MouseDown.BackColor = Color.FromArgb(12, 99, 228);
            UncheckedAppearance.Default.BorderColor = Color.FromArgb(222, 226, 230);
            UncheckedAppearance.Default.BackColor = Color.White;
            UncheckedAppearance.MouseOver.BorderColor = Color.FromArgb(222, 226, 230);
            UncheckedAppearance.MouseOver.BackColor = Color.White;
            UncheckedAppearance.MouseDown.BorderColor = Color.FromArgb(121, 165, 229);
            UncheckedAppearance.MouseDown.BackColor = Color.FromArgb(230, 230, 230);

            Enter += MetCheckBox_Enter;
        }

        private const int WM_LBUTTONUP = 0x0202;
        private const int WM_KEYUP = 0x0101;
        private const int VK_SPACE = 0x20;

        /// <summary>
        /// クリックまたはスペースキーが押されたとき、OnCheckedChanging, OnCheckStateChanging を発生させる。
        /// </summary>
        /// <param name="m">メッセージ。</param>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_LBUTTONUP)
            {
                // マウスアップ位置をチェック
                int upX = (int)(m.LParam.ToInt64() & 0xFFFF);
                int upY = (int)((m.LParam.ToInt64() >> 16) & 0xFFFF);
                bool mouseUpInside = this.ClientRectangle.Contains(upX, upY);

                // マウスアップがコントロール外の場合は処理しない
                if (!mouseUpInside)
                {
                    base.WndProc(ref m);
                    return;
                }

                var originalState = CheckState;

                var checkedCancelArgs = new CheckedChangingEventArgs(Checked, GetNextChecked());
                OnCheckedChanging(checkedCancelArgs);
                if (checkedCancelArgs.Cancel)
                {
                    // NOTE: メッセージを送信しないと、何らかの操作を行った後でないと OnMouseLeave() などが発生しなくなるため
                    _internalStateChange = true;
                    base.WndProc(ref m);
                    base.CheckState = originalState;
                    _internalStateChange = false;

                    _isPressed = false;
                    Invalidate();
                    return;
                }

                var checkStateCancelArgs = new CheckStateChangingEventArgs(CheckState, GetNextCheckState());
                OnCheckStateChanging(checkStateCancelArgs);
                if (checkStateCancelArgs.Cancel)
                {
                    // NOTE: メッセージを送信しないと、何らかの操作を行った後でないと OnMouseLeave() などが発生しなくなるため
                    _internalStateChange = true;
                    base.WndProc(ref m);
                    base.CheckState = originalState;
                    _internalStateChange = false;

                    _isPressed = false;
                    Invalidate();
                    return;
                }
            }

            if (m.Msg == WM_KEYUP)
            {
                if (m.WParam.ToInt32() == VK_SPACE)
                {
                    var checkedCancelArgs = new CheckedChangingEventArgs(Checked, GetNextChecked());
                    OnCheckedChanging(checkedCancelArgs);
                    if (checkedCancelArgs.Cancel)
                    {
                        _isPressed = false;
                        Invalidate();
                        return;
                    }

                    var checkStateCancelArgs = new CheckStateChangingEventArgs(CheckState, GetNextCheckState());
                    OnCheckStateChanging(checkStateCancelArgs);
                    if (checkStateCancelArgs.Cancel)
                    {
                        _isPressed = false;
                        Invalidate();
                        return;
                    }
                }
            }

            base.WndProc(ref m);
        }

        /// <summary>
        /// チェック操作が行われるとき、次の変化となるCheckStateを求める。
        /// </summary>
        /// <returns>次の変化となるCheckState。</returns>
        private CheckState GetNextCheckState()
        {
            if (ThreeState)
            {
                switch (CheckState)
                {
                    case CheckState.Checked:
                        return CheckState.Indeterminate;

                    case CheckState.Indeterminate:
                        return CheckState.Unchecked;

                    default:
                        return CheckState.Checked;
                }
            }

            switch (CheckState)
            {
                case CheckState.Checked:
                    return CheckState.Unchecked;

                default:
                    return CheckState.Checked;
            }
        }

        /// <summary>
        /// チェック操作が行われるとき、次の変化となるCheckedを求める。
        /// </summary>
        /// <returns>次の変化となるChecked。</returns>
        private bool GetNextChecked()
        {
            if (ThreeState)
            {
                switch (CheckState)
                {
                    case CheckState.Checked:
                        return true;

                    case CheckState.Indeterminate:
                        return false;

                    default:
                        return true;
                }
            }

            switch (CheckState)
            {
                case CheckState.Checked:
                    return false;

                default:
                    return true;
            }
        }

        private bool _internalStateChange = false;

        /// <summary>
        /// チェック操作が拒否されたときにイベント走行しないようにする。
        /// </summary>
        /// <param name="e">イベントデータ。</param>
        protected override void OnCheckStateChanged(EventArgs e)
        {
            if (_internalStateChange)
            {
                return;
            }

            base.OnCheckStateChanged(e);
        }

        /// <summary>
        /// チェック操作が拒否されたときにイベント走行しないようにする。
        /// </summary>
        /// <param name="e">イベントデータ。</param>
        protected override void OnCheckedChanged(EventArgs e)
        {
            if (_internalStateChange)
            {
                return;
            }

            base.OnCheckedChanged(e);
        }

        private bool _isMouseOver = false;

        /// <summary>
        /// マウスカーソルがエリア内に入ったときに描画し直す。
        /// </summary>
        /// <param name="e">イベントデータ。</param>
        protected override void OnMouseEnter(EventArgs e)
        {
            _isMouseOver = true;
            Invalidate();
            base.OnMouseEnter(e);
        }

        /// <summary>
        /// マウスカーソルがエリア内から出たときに描画し直す。
        /// </summary>
        /// <param name="e">イベントデータ。</param>
        protected override void OnMouseLeave(EventArgs e)
        {
            _isMouseOver = false;
            Invalidate();
            base.OnMouseLeave(e);
        }

        private bool _isPressed = false;

        /// <summary>
        /// マウスダウンされたときに描画し直す。
        /// </summary>
        /// <param name="e">イベントデータ。</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _isPressed = true;
                Focus();
                Invalidate();
            }
            base.OnMouseDown(e);
        }

        /// <summary>
        /// マウスアップされたときに描画し直す。
        /// </summary>
        /// <param name="e">イベントデータ。</param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _isPressed = false;
                Invalidate();
            }
            base.OnMouseUp(e);
        }

        /// <summary>
        /// スペースキーがキーダウンされたときに描画し直す。
        /// </summary>
        /// <param name="e">イベントデータ。</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                _isPressed = true;
                Invalidate();
            }
            base.OnKeyDown(e);
        }

        /// <summary>
        /// スペースキーがキーアップされたときに描画し直す。
        /// </summary>
        /// <param name="e">イベントデータ。</param>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                _isPressed = false;
                Invalidate();
            }
            base.OnKeyUp(e);
        }

        /// <summary>
        /// フォーカスを得たときに描画し直す。
        /// </summary>
        /// <param name="e">イベントデータ。</param>
        protected override void OnGotFocus(EventArgs e)
        {
            Invalidate();
            base.OnGotFocus(e);
        }

        /// <summary>
        /// フォーカスを失ったときに描画し直す。
        /// </summary>
        /// <param name="e">イベントデータ。</param>
        protected override void OnLostFocus(EventArgs e)
        {
            Invalidate();
            base.OnLostFocus(e);
        }

        /// <summary>
        /// テキストが変化したときに描画し直す。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            if (AutoSize)
            {
                Size = GetPreferredSize(Size.Empty);
            }
            Invalidate();
        }

        /// <summary>
        /// フォントが変化したときに描画し直す。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            if (AutoSize)
            {
                Size = GetPreferredSize(Size.Empty);
            }
            Invalidate();
        }

        /// <summary>
        /// 描画に必要なサイズを求める。
        /// </summary>
        /// <param name="proposedSize">コントロールのカスタム サイズ領域。</param>
        /// <returns>四角形の幅と高さを表す、順序を付与した Size 型のペア。</returns>
        public override Size GetPreferredSize(Size proposedSize)
        {
            Size textSize = Size.Empty;
            if (!string.IsNullOrEmpty(Text))
            {
                textSize = TextRenderer.MeasureText(Text, Font);
            }

            int checkBoxSize = Font.Height;
            int width = (FocusWidth * 2) + checkBoxSize + textSize.Width;
            int height = Math.Max(checkBoxSize + (FocusWidth * 2), textSize.Height);

            return new Size(width, height);
        }

        /// <summary>
        /// AutoSize のときに描画に必要なサイズに設定する。
        /// </summary>
        /// <param name="x">新しいX座標。</param>
        /// <param name="y">新しいY座標。</param>
        /// <param name="width">新しい幅。</param>
        /// <param name="height">新しい高さ。</param>
        /// <param name="specified">BoundsSpecified 値のビット単位の組み合わせ。</param>
        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            if (AutoSize && (specified & BoundsSpecified.Size) == BoundsSpecified.Size)
            {
                Size preferredSize = GetPreferredSize(new Size(width, height));
                width = preferredSize.Width;
                height = preferredSize.Height;
            }
            base.SetBoundsCore(x, y, width, height, specified);
        }

        /// <summary>
        /// コントロールの描画を行う。
        /// </summary>
        /// <param name="e">グラフィック。</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            // 描画品質を設定
            var oldSmoothing = g.SmoothingMode;
            var oldInterpolation = g.InterpolationMode;
            var oldCompositing = g.CompositingQuality;
            var oldPixelOffset = g.PixelOffsetMode;

            try
            {
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                // 全体の透明背景を描画
                DrawTransparentBackground(g, new Region(new Rectangle(0, 0, Width, Height)));

                // コントロール全体の背景を描画
                DrawControlBackground(g);

                // フォーカス枠を描画
                if (Focused)
                {
                    DrawFocusFrame(g);
                }

                Rectangle checkBoxRect = GetCheckBoxRectangle();
                Rectangle textRect = GetTextRectangle();

                // チェックボックスを描画
                DrawCheckBox(g, checkBoxRect);

                // テキストを描画
                if (!string.IsNullOrEmpty(Text))
                {
                    DrawText(g, textRect);
                }
            }
            finally
            {
                g.SmoothingMode = oldSmoothing;
                g.InterpolationMode = oldInterpolation;
                g.CompositingQuality = oldCompositing;
                g.PixelOffsetMode = oldPixelOffset;
            }
        }

        /// <summary>
        /// 透明な背景をクリッピングによって描画します。
        /// </summary>
        /// <param name="g">グラフィックス。</param>
        /// <param name="clipRegion">クリッピング領域。</param>
        private void DrawTransparentBackground(Graphics g, Region clipRegion)
        {
            if (Parent == null)
            {
                return;
            }

            // クリッピング領域を設定
            Region oldClip = g.Clip;
            try
            {
                g.Clip = clipRegion;

                // 親の背景を描画するために、座標を親の座標系に変換
                g.TranslateTransform(-Location.X, -Location.Y);

                // 親コントロールの背景描画メソッドを呼び出し
                InvokePaintBackground(Parent, new PaintEventArgs(g, Bounds));

                // 親コントロール上の他の子コントロールも描画（この CheckBox より前面にあるもの以外）
                InvokeParentPaint(Parent, g);
            }
            finally
            {
                g.TranslateTransform(Location.X, Location.Y);
                g.Clip = oldClip;
            }
        }

        /// <summary>
        /// 親コントロールの描画を呼び出します。
        /// </summary>
        /// <param name="parent">親オブジェクト。</param>
        /// <param name="g">グラフィックス。</param>
        private void InvokeParentPaint(Control parent, Graphics g)
        {
            try
            {
                // 親の Paint イベントを発生させる
                var paintEventArgs = new PaintEventArgs(g, parent.ClientRectangle);
                typeof(Control).GetMethod("OnPaint", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    ?.Invoke(parent, new object[] { paintEventArgs });
            }
            catch
            {
                // エラーが発生した場合は親の背景色で塗りつぶし
                using (var brush = new SolidBrush(parent.BackColor))
                {
                    g.FillRectangle(brush, parent.ClientRectangle);
                }
            }
        }

        /// <summary>
        /// コントロール全体の背景を描画します。
        /// </summary>
        /// <param name="g">グラフィックス</param>
        private void DrawControlBackground(Graphics g)
        {
            // BackColorが設定されている場合は全体をBackColorで塗りつぶし
            if (BackColor != SystemColors.Control && BackColor != Color.Empty)
            {
                Rectangle backgroundRect;
                Rectangle checkBoxRect = GetCheckBoxRectangle();

                backgroundRect = new Rectangle(
                    0,
                    0,
                    Width,
                    Height
                );

                using (SolidBrush backBrush = new SolidBrush(BackColor))
                {
                    g.FillRectangle(backBrush, backgroundRect);
                }
            }
        }

        /// <summary>
        /// チェックボックスの描画領域を取得する。
        /// </summary>
        /// <returns>チェックボックスの描画領域。</returns>
        private Rectangle GetCheckBoxRectangle()
        {
            int checkBoxSize = Font.Height;
            Rectangle rect = new Rectangle();

            // 横方向の配置
            switch (CheckAlign)
            {
                case ContentAlignment.TopLeft:
                case ContentAlignment.MiddleLeft:
                case ContentAlignment.BottomLeft:
                    rect.X = FocusWidth;
                    break;

                case ContentAlignment.TopCenter:
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.BottomCenter:
                    rect.X = (Width - checkBoxSize) / 2;
                    break;

                case ContentAlignment.TopRight:
                case ContentAlignment.MiddleRight:
                case ContentAlignment.BottomRight:
                    rect.X = Width - checkBoxSize - FocusWidth;
                    break;
            }

            // 縦方向の配置
            switch (CheckAlign)
            {
                case ContentAlignment.TopLeft:
                case ContentAlignment.TopCenter:
                case ContentAlignment.TopRight:
                    rect.Y = FocusWidth;
                    break;

                case ContentAlignment.MiddleLeft:
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.MiddleRight:
                    rect.Y = Math.Max(FocusWidth, (Height - checkBoxSize) / 2);
                    break;

                case ContentAlignment.BottomLeft:
                case ContentAlignment.BottomCenter:
                case ContentAlignment.BottomRight:
                    rect.Y = Height - checkBoxSize - FocusWidth;
                    break;
            }

            rect.Width = checkBoxSize;
            rect.Height = checkBoxSize;

            // 最小サイズを保証
            rect.Width = Math.Max(8, rect.Width);
            rect.Height = Math.Max(8, rect.Height);

            return rect;
        }

        /// <summary>
        /// テキストの描画領域を求める。
        /// </summary>
        /// <returns>テキストの描画領域。</returns>
        private Rectangle GetTextRectangle()
        {
            if (string.IsNullOrEmpty(Text))
                return Rectangle.Empty;

            Rectangle checkBoxRect = GetCheckBoxRectangle();
            Rectangle textRect = new Rectangle();

            Size fontSize = TextRenderer.MeasureText(Text, Font);

            // 横方向の配置
            switch (TextAlign)
            {
                case ContentAlignment.TopLeft:
                case ContentAlignment.MiddleLeft:
                case ContentAlignment.BottomLeft:
                    textRect.X = checkBoxRect.Right + FocusWidth;
                    break;

                case ContentAlignment.TopCenter:
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.BottomCenter:
                    textRect.X = (Width - checkBoxRect.Width - FocusWidth) / 2 - fontSize.Width / 2;
                    break;

                case ContentAlignment.TopRight:
                case ContentAlignment.MiddleRight:
                case ContentAlignment.BottomRight:
                    textRect.X = Width - fontSize.Width - (FocusWidth * 2) - checkBoxRect.Width;
                    break;
            }

            // 縦方向の配置
            // NOTE: 縦中央位置からの増分を指定する。
            switch (TextAlign)
            {
                case ContentAlignment.TopLeft:
                case ContentAlignment.TopCenter:
                case ContentAlignment.TopRight:
                    textRect.Y = -(Height / 2 - Font.Height / 2 - FocusWidth);
                    break;

                case ContentAlignment.MiddleLeft:
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.MiddleRight:
                    textRect.Y = 0;
                    break;
                case ContentAlignment.BottomLeft:
                case ContentAlignment.BottomCenter:
                case ContentAlignment.BottomRight:
                    textRect.Y = Height / 2 - Font.Height / 2 - FocusWidth;
                    break;
            }

            textRect.Width = fontSize.Width;
            textRect.Height = Height;

            return textRect;
        }

        /// <summary>
        /// フォーカス枠を描画します。
        /// </summary>
        /// <param name="g">グラフィック。</param>
        private void DrawFocusFrame(Graphics g)
        {
            Rectangle checkBoxRect = GetCheckBoxRectangle();
            if (!checkBoxRect.IsEmpty)
            {
                // フォーカス枠の外側矩形（チェックボックスより4px大きく）
                Rectangle focusOuterRect = new Rectangle(
                    checkBoxRect.X - FocusWidth,
                    checkBoxRect.Y - FocusWidth,
                    checkBoxRect.Width + (FocusWidth * 2),
                    checkBoxRect.Height + (FocusWidth * 2)
                );

                // フォーカス枠の内側矩形（チェックボックスと同じサイズ）
                Rectangle focusInnerRect = checkBoxRect;

                // フォーカス枠を描画（外側パス - 内側パス）
                using (GraphicsPath outerPath = CreateRoundedRectanglePath(focusOuterRect,
                    GetDrawableCheckBoxRadius() + FocusWidth))
                using (GraphicsPath innerPath = CreateRoundedRectanglePath(focusInnerRect,
                    GetDrawableCheckBoxRadius()))
                using (Region focusRegion = new Region(outerPath))
                {
                    focusRegion.Exclude(innerPath);

                    using (SolidBrush focusBrush = new SolidBrush(FocusColor))
                    {
                        g.FillRegion(focusBrush, focusRegion);
                    }
                }
            }
        }

        /// <summary>
        /// 描画可能な範囲のチェックボックス角丸半径を求める。
        /// </summary>
        /// <returns>描画可能な範囲のチェックボックス角丸半径。</returns>
        private int GetDrawableCheckBoxRadius()
        {
            if (CheckBoxRadius == -1)
            {
                return DefaultCheckBoxRadius;
            }

            var checkBoxRectangle = GetCheckBoxRectangle();
            return Math.Min((int)Math.Floor(checkBoxRectangle.Width / 2d), CheckBoxRadius);
        }

        /// <summary>
        /// 角丸のパスを作成する。
        /// </summary>
        /// <param name="rect">描画領域。</param>
        /// <param name="radius">角丸半径。</param>
        /// <returns>角丸パス。</returns>
        private GraphicsPath CreateRoundedRectanglePath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();

            if (CheckBoxRadius == 0)
            {
                path.AddRectangle(rect);
                return path;
            }

            int diameter = radius * 2;

            // 角丸四角形のパスを作成
            Rectangle arc = new Rectangle(rect.X, rect.Y, diameter, diameter);
            path.AddArc(arc, 180, 90); // 左上

            arc.X = rect.Right - diameter;
            path.AddArc(arc, 270, 90); // 右上

            arc.Y = rect.Bottom - diameter;
            path.AddArc(arc, 0, 90); // 右下

            arc.X = rect.Left;
            path.AddArc(arc, 90, 90); // 左下

            path.CloseFigure();
            return path;
        }

        /// <summary>
        /// チェックボックスを描画する。
        /// </summary>
        /// <param name="g">グラフィック。</param>
        /// <param name="rect">チェックボックスの描画情報。</param>
        private void DrawCheckBox(Graphics g, Rectangle rect)
        {
            Color checkMarkColor = CheckColor != Color.Empty ? CheckColor : Color.White;
            ElementColorCombination colorCombinatin;
            if (Enabled)
            {
                if (Checked)
                {
                    colorCombinatin = GetCheckedColorCombination();
                }
                else
                {
                    colorCombinatin = GetUncheckedColorCombination();
                }
            }
            else
            {
                colorCombinatin = GetDisableColorCombination();
            }

            // 背景を描画（角丸四角形）
            using (SolidBrush bgBrush = new SolidBrush(colorCombinatin.BackColor))
            {
                FillRoundedRectangle(g, bgBrush, rect, GetDrawableCheckBoxRadius());
            }

            // 境界線を描画
            DrawCheckBoxBorder(g, rect, colorCombinatin.BorderColor);

            // チェックマークまたは不確定マークを描画
            if (CheckState == CheckState.Checked)
            {
                DrawCheckMark(g, rect, checkMarkColor);
            }
            else if (CheckState == CheckState.Indeterminate)
            {
                DrawIndeterminateMark(g, rect, checkMarkColor);
            }
        }

        /// <summary>
        /// チェックされているときの色の組み合わせを求める。
        /// </summary>
        /// <returns>色の組み合わせ。</returns>
        private ElementColorCombination GetCheckedColorCombination()
        {
            var borderColor = CheckedAppearance.Default.BorderColor != Color.Empty ?
                CheckedAppearance.Default.BorderColor : DefaultBorderColor;
            var fillColor = CheckedAppearance.Default.BackColor != Color.Empty ?
                CheckedAppearance.Default.BackColor : DefaultBackColor;

            if (_isMouseOver)
            {
                borderColor = CheckedAppearance.MouseOver.BorderColor != Color.Empty ?
                    CheckedAppearance.MouseOver.BorderColor : DefaultBorderColor; ;
                fillColor = CheckedAppearance.MouseOver.BackColor != Color.Empty ?
                    CheckedAppearance.MouseOver.BackColor : DefaultBorderColor; ;
            }

            if (_isPressed)
            {
                borderColor = CheckedAppearance.MouseDown.BorderColor != Color.Empty ?
                    CheckedAppearance.MouseDown.BorderColor : DefaultBorderColor; ;
                fillColor = CheckedAppearance.MouseDown.BackColor != Color.Empty ?
                    CheckedAppearance.MouseDown.BackColor : DefaultBorderColor; ;
            }

            return new ElementColorCombination(borderColor, fillColor, Color.Empty);
        }

        /// <summary>
        /// チェックされていないときの色の組み合わせを求める。
        /// </summary>
        /// <returns>色の組み合わせ。</returns>
        private ElementColorCombination GetUncheckedColorCombination()
        {
            var borderColor = UncheckedAppearance.Default.BorderColor != Color.Empty ?
                UncheckedAppearance.Default.BorderColor : DefaultBorderColor;
            var fillColor = UncheckedAppearance.Default.BackColor != Color.Empty ?
                UncheckedAppearance.Default.BackColor : DefaultBackColor;

            if (_isMouseOver)
            {
                borderColor = UncheckedAppearance.MouseOver.BorderColor != Color.Empty ?
                    UncheckedAppearance.MouseOver.BorderColor : DefaultBorderColor;
                fillColor = UncheckedAppearance.MouseOver.BackColor != Color.Empty ?
                    UncheckedAppearance.MouseOver.BackColor : DefaultBorderColor; ;
            }

            if (_isPressed)
            {
                borderColor = UncheckedAppearance.MouseDown.BorderColor != Color.Empty ?
                    UncheckedAppearance.MouseDown.BorderColor : DefaultBorderColor; ;
                fillColor = UncheckedAppearance.MouseDown.BackColor != Color.Empty ?
                    UncheckedAppearance.MouseDown.BackColor : DefaultBorderColor; ;
            }

            return new ElementColorCombination(borderColor, fillColor, Color.Empty);
        }

        /// <summary>
        /// 非活性のときの色の組み合わせを求める。
        /// </summary>
        /// <returns>色の組み合わせ。</returns>
        private ElementColorCombination GetDisableColorCombination()
        {
            Color borderColor;
            Color fillColor;
            if (Checked)
            {
                borderColor = CheckedAppearance.Default.BorderColor != Color.Empty ?
                    CheckedAppearance.Default.BorderColor : DefaultBorderColor;
                fillColor = CheckedAppearance.Default.BackColor != Color.Empty ?
                    CheckedAppearance.Default.BackColor : DefaultBackColor;
            }
            else
            {
                borderColor = UncheckedAppearance.Default.BorderColor != Color.Empty ?
                    UncheckedAppearance.Default.BorderColor : DefaultBorderColor;
                fillColor = UncheckedAppearance.Default.BackColor != Color.Empty ?
                    UncheckedAppearance.Default.BackColor : DefaultBackColor;
            }

            return new ElementColorCombination(
                ControlPaint.LightLight(borderColor),
                ControlPaint.LightLight(fillColor),
                Color.Empty);
        }

        /// <summary>
        /// 角丸の塗りつぶし四角形を描画する。
        /// </summary>
        /// <param name="g">グラフィック。</param>
        /// <param name="brush">ブラシ。</param>
        /// <param name="rect">描画領域。</param>
        /// <param name="radius">角丸半径。</param>
        private void FillRoundedRectangle(Graphics g, Brush brush, Rectangle rect, int radius)
        {
            using (GraphicsPath path = CreateRoundedRectanglePath(rect, radius))
            {
                g.FillPath(brush, path);
            }
        }

        /// <summary>
        /// チェックボックスの境界ボーダーを描画する。
        /// フォーカスがある場合はフォーカス枠に対してアンチエイリアスを適用する。
        /// </summary>
        /// <param name="g">グラフィックス。</param>
        /// <param name="rect">描画領域。</param>
        /// <param name="borderColor">境界線色。</param>
        private void DrawCheckBoxBorder(Graphics g, Rectangle rect, Color borderColor)
        {
            using (Pen borderPen = new Pen(borderColor, 1))
            {
                var oldSmoothing = g.SmoothingMode;
                var oldPixelOffset = g.PixelOffsetMode;

                try
                {
                    var drawableCheckBoxRadius = GetDrawableCheckBoxRadius();

                    if (Focused)
                    {
                        // フォーカスがある時は、フォーカス枠に対してアンチエイリアスを適用して描画
                        g.SmoothingMode = SmoothingMode.AntiAlias;
                        g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                        // 角丸四角形として全体を描画（フォーカス枠との調和を重視）
                        using (GraphicsPath borderPath = CreateRoundedRectanglePath(rect, drawableCheckBoxRadius))
                        {
                            g.DrawPath(borderPen, borderPath);
                        }
                        return;
                    }

                    // フォーカスがない時は、はっきりとした線で描画
                    // 上辺（直線部分）
                    if (CheckBoxRadius == 0)
                    {
                        g.DrawRectangle(borderPen, rect);
                    }
                    else
                    {
                        var minDrawableCheckBoxRadius = (drawableCheckBoxRadius == 0 ? 0 : drawableCheckBoxRadius - 1);
                        g.SmoothingMode = SmoothingMode.None;
                        g.PixelOffsetMode = PixelOffsetMode.None;
                        g.DrawLine(borderPen,
                            rect.X + drawableCheckBoxRadius,
                            rect.Y,
                            rect.Right - minDrawableCheckBoxRadius,
                            rect.Y);

                        // 右辺（直線部分）
                        g.DrawLine(borderPen,
                            rect.Right - 1,
                            rect.Y + drawableCheckBoxRadius,
                            rect.Right - 1,
                            rect.Bottom - minDrawableCheckBoxRadius);

                        // 下辺（直線部分）
                        g.DrawLine(borderPen,
                            rect.Right - minDrawableCheckBoxRadius,
                            rect.Bottom - 1,
                            rect.X + drawableCheckBoxRadius,
                            rect.Bottom - 1);

                        // 左辺（直線部分）
                        g.DrawLine(borderPen,
                            rect.X,
                            rect.Bottom - minDrawableCheckBoxRadius,
                            rect.X,
                            rect.Y + drawableCheckBoxRadius);

                        // 角丸部分のみアンチエイリアスを有効にして描画
                        g.SmoothingMode = SmoothingMode.AntiAlias;
                        g.PixelOffsetMode = PixelOffsetMode.None;

                        int diameter = drawableCheckBoxRadius * 2;
                        var minDiameter = (diameter == 0 ? 0 : diameter - 1);
                        float offset = 0.5f;

                        // 左上角（矩形内に収める）
                        RectangleF arcRect = new RectangleF(
                            rect.X + offset,
                            rect.Y + offset,
                            minDiameter,
                            minDiameter);
                        g.DrawArc(borderPen, arcRect, 180, 90);

                        // 右上角（矩形内に収める）
                        arcRect = new RectangleF(
                            rect.Right - diameter - offset,
                            rect.Y + offset,
                            minDiameter,
                            minDiameter);
                        g.DrawArc(borderPen, arcRect, 270, 90);

                        // 右下角（矩形内に収める）
                        arcRect = new RectangleF(
                            rect.Right - diameter - offset,
                            rect.Bottom - diameter - offset,
                            minDiameter,
                            minDiameter);
                        g.DrawArc(borderPen, arcRect, 0, 90);

                        // 左下角（矩形内に収める）
                        arcRect = new RectangleF(
                            rect.X + offset,
                            rect.Bottom - diameter - offset,
                            minDiameter,
                            minDiameter);
                        g.DrawArc(borderPen, arcRect, 90, 90);
                    }
                }
                finally
                {
                    g.SmoothingMode = oldSmoothing;
                    g.PixelOffsetMode = oldPixelOffset;
                }
            }
        }

        /// <summary>
        /// チェックされたマークを描画する。
        /// </summary>
        /// <param name="g">グラフィック。</param>
        /// <param name="rect">描画領域。</param>
        /// <param name="color">マークの色。</param>
        private void DrawCheckMark(Graphics g, Rectangle rect, Color color)
        {
            using (Pen checkPen = new Pen(color, Math.Max(rect.Width / 6, 2.0f)))
            {
                checkPen.StartCap = LineCap.Round;
                checkPen.EndCap = LineCap.Round;
                checkPen.LineJoin = LineJoin.Round;

                // チェックマークのパス
                Point[] checkPoints = new Point[]
                {
                    new Point(rect.X + rect.Width * 25 / 100, rect.Y + rect.Height * 50 / 100),
                    new Point(rect.X + rect.Width * 45 / 100, rect.Y + rect.Height * 70 / 100),
                    new Point(rect.X + rect.Width * 75 / 100, rect.Y + rect.Height * 35 / 100)
                };

                g.DrawLines(checkPen, checkPoints);
            }
        }

        /// <summary>
        /// Indeterminateチェックのマークを描画する。
        /// </summary>
        /// <param name="g">グラフィック。</param>
        /// <param name="rect">描画領域。</param>
        /// <param name="color">マークの色。</param>
        private void DrawIndeterminateMark(Graphics g, Rectangle rect, Color color)
        {
            using (SolidBrush brush = new SolidBrush(color))
            {
                int padding = rect.Width / 4;
                int thickness = Math.Max(2, rect.Height / 6);
                Rectangle indRect = new Rectangle(
                    rect.X + padding,
                    rect.Y + (rect.Height - thickness) / 2,
                    rect.Width - (padding * 2),
                    thickness);

                // 角丸の四角形として描画
                FillRoundedRectangle(g, brush, indRect, 1);
            }
        }

        /// <summary>
        /// テキストを描画する。
        /// </summary>
        /// <param name="g">グラフィック。</param>
        /// <param name="rect">描画領域。</param>
        private void DrawText(Graphics g, Rectangle rect)
        {
            if (string.IsNullOrEmpty(Text) || rect.IsEmpty)
            {
                return;
            }

            TextFormatFlags flags = TextFormatFlags.Left | TextFormatFlags.VerticalCenter |
                TextFormatFlags.EndEllipsis;

            if (!UseMnemonic)
            {
                flags |= TextFormatFlags.NoPrefix;
            }

            Color textColor = Enabled ? ForeColor : ControlPaint.Light(ForeColor);
            TextRenderer.DrawText(g, Text, Font, rect, textColor, flags);
        }

        private CheckState _enterCheckState = CheckState.Checked;

        /// <summary>
        /// フォーカスを得たときに、現在のチェック状態を保存します。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MetCheckBox_Enter(object sender, EventArgs e)
        {
            _enterCheckState = CheckState;
        }

        /// <summary>
        /// ロールバック済みかどうかを取得します。
        /// </summary>
        /// <returns>true:ロールバック済み, false:未ロールバック。</returns>
        [Browsable(false)]
        public virtual bool IsRollbacked => CheckState == _enterCheckState;

        /// <summary>
        /// ロールバック中かどうかを取得または設定します。
        /// </summary>
        [Browsable(false)]
        protected bool Rollbacking { get; set; } = false;

        /// <summary>
        /// フォーカスを得た時の値にロールバックを行います。
        /// </summary>
        public virtual void Rollback()
        {
            Rollbacking = true;

            var cancelArgs = new CheckStateChangingEventArgs(CheckState, GetNextCheckState());
            OnCheckStateChanging(cancelArgs);
            if (cancelArgs.Cancel)
            {
                Rollbacking = false;
                return;
            }

            CheckState = _enterCheckState;
            Rollbacking = false;
        }
    }
}