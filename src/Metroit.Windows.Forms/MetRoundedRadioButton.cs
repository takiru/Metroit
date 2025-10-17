using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// Bootstrap4風の角丸ラジオボタンコントロール
    /// 4つの角の丸みを個別に設定可能で、横並び・縦並び両方に対応
    /// カスタム描画による美しい外観を提供
    /// </summary>
    public class MetRoundedRadioButton : RadioButton
    {
        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        public MetRoundedRadioButton() : base()
        {
            // 最高品質の描画設定
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.DoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.SupportsTransparentBackColor, true);

            BackColor = Color.White;
            ForeColor = Color.FromArgb(13, 110, 253);
            ExtendsAppearance.BorderSize = 1;
            ExtendsAppearance.BorderColor = Color.FromArgb(13, 110, 253);
            Size = new Size(100, 35);
            Margin = new Padding(0);
            base.FlatStyle = FlatStyle.Flat;
            base.Appearance = System.Windows.Forms.Appearance.Button;
            TextAlign = ContentAlignment.MiddleCenter;
            Cursor = Cursors.Hand;
        }

        /// <summary>
        /// フォーカス用オーバーレイ操作。
        /// </summary>
        private FocusOverlayController _focusOverlayController;

        /// <summary>
        /// コントロールが配置されたら、フォーカス用オーバーレイを表示できる準備をする。
        /// </summary>
        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            _focusOverlayController = new FocusOverlayController(this, GetCurrentFillColor);
        }

        private RadioButtonExtendsAppearance _extendsAppearance;

        /// <summary>
        /// 外観を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [MetCategory("MetAppearance")]
        [MetDescription("MetRoundedRadioButtonExtendsAppearance")]
        public RadioButtonExtendsAppearance ExtendsAppearance
        {
            get
            {
                if (_extendsAppearance == null)
                {
                    _extendsAppearance = new RadioButtonExtendsAppearance(this);
                }

                return _extendsAppearance;
            }
        }

        /// <summary>
        /// FlatStyleプロパティを隠します（このコントロールでは使用しません）
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Obsolete("FlatStyle cannot be used.", true)]
        public new FlatStyle FlatStyle
        {
            get => base.FlatStyle;
            set { }
        }

        /// <summary>
        /// コントロール間の空白を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof(Padding), "0, 0, 0, 0")]
        public new Padding Margin
        {
            get => base.Margin;
            set => base.Margin = value;
        }

        /// <summary>
        /// コントロールの背景色を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof(Color), "White")]
        public override Color BackColor { get => base.BackColor; set => base.BackColor = value; }

        /// <summary>
        /// コントロールの前景色を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof(Color), "13, 110, 253")]
        public override Color ForeColor { get => base.ForeColor; set => base.ForeColor = value; }

        private Color _checkedBackColor = Color.FromArgb(13, 110, 253);

        /// <summary>
        /// チェック時の背景色を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof(Color), "13, 110, 253")]
        [MetCategory("MetAppearance")]
        [MetDescription("MetRoundedRadioButtonCheckedBackColor")]
        public Color CheckedBackColor
        {
            get => _checkedBackColor;
            set
            {
                if (_checkedBackColor != value)
                {
                    _checkedBackColor = value;
                    Invalidate();
                }
            }
        }

        private Color _checkedForeColor = Color.White;

        /// <summary>
        /// チェック時の前景色を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof(Color), "White")]
        [MetCategory("MetAppearance")]
        [MetDescription("MetRoundedRadioButtonCheckedForeColor")]
        public Color CheckedForeColor
        {
            get => _checkedForeColor;
            set
            {
                if (_checkedForeColor != value)
                {
                    _checkedForeColor = value;
                    Invalidate();
                }
            }
        }

        private static readonly int DefaultRadiusPoint = 5;
        private static readonly RoundedCornerRadius DefaultRadius = new RoundedCornerRadius(DefaultRadiusPoint);
        private RoundedCornerRadius _radius = null;

        /// <summary>
        /// 各角の丸みの半径を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [MetCategory("MetAppearance")]
        [MetDescription("MetRoundedRadioButtonRadius")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public RoundedCornerRadius Radius
        {
            get => _radius == null ? new RoundedCornerRadius(DefaultRadiusPoint) : _radius;
            set
            {
                _radius = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Radius が変更されたかどうか。
        /// </summary>
        /// <returns></returns>
        private bool ShouldSerializeRadius()
        {
            return Radius != DefaultRadius;
        }

        /// <summary>
        /// Radius をリセットする。
        /// </summary>
        private void ResetRadius()
        {
            Radius = new RoundedCornerRadius(DefaultRadius.TopLeft);
        }

        /// <summary>
        /// コントロールの描画を行います。
        /// </summary>
        /// <param name="e">描画イベントの引数</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            //// ラジオボタンの外観を描画
            _focusOverlayController.DrawRoundedShape(e.Graphics, ExtendsAppearance, Radius);

            // テキストを描画
            DrawButtonText(e.Graphics);
        }

        /// <summary>
        /// コントロールが破棄されるときの処理
        /// </summary>
        /// <param name="disposing">マネージリソースを破棄するかどうか</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _focusOverlayController?.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// 現在のマウス状態に応じた塗りつぶし色を取得します。
        /// </summary>
        /// <returns>現在の塗りつぶし色。</returns>
        private Color GetCurrentFillColor()
        {
            if (!Enabled)
            {
                if (Checked)
                {
                    return ControlPaint.LightLight(_checkedBackColor);
                }
                else
                {
                    return ControlPaint.LightLight(BackColor);
                }
            }

            if (Checked)
            {
                switch (_focusOverlayController.MouseState)
                {
                    case MouseState.Hover:
                        return ExtendsAppearance.MouseOverBackColor;
                    case MouseState.Pressed:
                        return ExtendsAppearance.MouseDownBackColor;
                    default:
                        return _checkedBackColor;
                }
            }
            else
            {
                switch (_focusOverlayController.MouseState)
                {
                    case MouseState.Hover:
                        return ExtendsAppearance.MouseOverBackColor;
                    case MouseState.Pressed:
                        return ExtendsAppearance.MouseDownBackColor;
                    default:
                        return BackColor;
                }
            }
        }

        /// <summary>
        /// チェック状態変更時の処理
        /// 再描画を要求
        /// </summary>
        /// <param name="e">イベント引数</param>
        protected override void OnCheckedChanged(EventArgs e)
        {
            Invalidate();
            base.OnCheckedChanged(e);
        }

        /// <summary>
        /// 現在のマウス状態に応じた文字色を取得します。
        /// </summary>
        /// <returns>現在の文字色。</returns>
        private Color GetCurrentTextColor()
        {
            if (!Enabled)
            {
                if (Checked)
                {
                    return ControlPaint.Light(_checkedForeColor);
                }
                else
                {
                    return ControlPaint.Light(ForeColor);
                }
            }

            if (Checked)
            {
                switch (_focusOverlayController.MouseState)
                {
                    case MouseState.Hover:
                        return ExtendsAppearance.MouseOverForeColor;
                    case MouseState.Pressed:
                        return ExtendsAppearance.MouseDownForeColor;
                    default:
                        return _checkedForeColor;
                }
            }
            else
            {
                switch (_focusOverlayController.MouseState)
                {
                    case MouseState.Hover:
                        return ExtendsAppearance.MouseOverForeColor;
                    case MouseState.Pressed:
                        return ExtendsAppearance.MouseDownForeColor;
                    default:
                        return ForeColor;
                }
            }
        }

        /// <summary>
        /// TextAlignの設定値に基づいてStringFormatを作成します。
        /// </summary>
        /// <returns>TextAlignに対応したStringFormat。</returns>
        private StringFormat CreateStringFormat()
        {
            StringFormat stringFormat = new StringFormat();

            // 水平方向の配置
            switch (TextAlign)
            {
                case ContentAlignment.TopLeft:
                case ContentAlignment.MiddleLeft:
                case ContentAlignment.BottomLeft:
                    stringFormat.Alignment = StringAlignment.Near;
                    break;
                case ContentAlignment.TopCenter:
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.BottomCenter:
                    stringFormat.Alignment = StringAlignment.Center;
                    break;
                case ContentAlignment.TopRight:
                case ContentAlignment.MiddleRight:
                case ContentAlignment.BottomRight:
                    stringFormat.Alignment = StringAlignment.Far;
                    break;
            }

            // 垂直方向の配置
            switch (TextAlign)
            {
                case ContentAlignment.TopLeft:
                case ContentAlignment.TopCenter:
                case ContentAlignment.TopRight:
                    stringFormat.LineAlignment = StringAlignment.Near;
                    break;
                case ContentAlignment.MiddleLeft:
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.MiddleRight:
                    stringFormat.LineAlignment = StringAlignment.Center;
                    break;
                case ContentAlignment.BottomLeft:
                case ContentAlignment.BottomCenter:
                case ContentAlignment.BottomRight:
                    stringFormat.LineAlignment = StringAlignment.Far;
                    break;
            }

            stringFormat.Trimming = StringTrimming.EllipsisCharacter;
            stringFormat.FormatFlags = StringFormatFlags.NoWrap;

            return stringFormat;
        }

        /// <summary>
        /// ボタンのテキストを描画します。
        /// </summary>
        /// <param name="graphics">描画に使用するGraphicsオブジェクト。</param>
        private void DrawButtonText(Graphics graphics)
        {
            if (string.IsNullOrEmpty(Text))
                return;

            // テキストの描画色を決定
            Color textColor = GetCurrentTextColor();
            
            // テキストの描画範囲を計算（フォーカス枠は親に描画するため考慮しない）
            Rectangle textRect = ClientRectangle;

            // 境界線分を内側に調整
            if (ExtendsAppearance.BorderSize > 0)
            {
                var borderPadding = Math.Max(2, ExtendsAppearance.BorderSize);
                textRect.Inflate(-borderPadding, -borderPadding);
            }

            // UseMnemonicがtrueの場合は標準のニーモニック処理を使用
            if (UseMnemonic)
            {
                TextFormatFlags flags = CreateTextFormatFlags();
                TextRenderer.DrawText(graphics, Text, Font, textRect, textColor, flags);
            }
            else
            {
                // テキストの高品質描画設定
                graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                // TextAlignに基づいてテキストを描画
                using (StringFormat stringFormat = CreateStringFormat())
                {
                    using (SolidBrush textBrush = new SolidBrush(textColor))
                    {
                        graphics.DrawString(Text, Font, textBrush, textRect, stringFormat);
                    }
                }
            }
        }

        /// <summary>
        /// TextRendererで使用するTextFormatFlagsを作成します。
        /// </summary>
        /// <returns>作成されたTextFormatFlags。</returns>
        private TextFormatFlags CreateTextFormatFlags()
        {
            TextFormatFlags flags = TextFormatFlags.TextBoxControl;

            // 水平方向の配置
            switch (TextAlign)
            {
                case ContentAlignment.TopLeft:
                case ContentAlignment.MiddleLeft:
                case ContentAlignment.BottomLeft:
                    flags |= TextFormatFlags.Left;
                    break;

                case ContentAlignment.TopCenter:
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.BottomCenter:
                    flags |= TextFormatFlags.HorizontalCenter;
                    break;

                case ContentAlignment.TopRight:
                case ContentAlignment.MiddleRight:
                case ContentAlignment.BottomRight:
                    flags |= TextFormatFlags.Right;
                    break;
            }

            // 垂直方向の配置
            switch (TextAlign)
            {
                case ContentAlignment.TopLeft:
                case ContentAlignment.TopCenter:
                case ContentAlignment.TopRight:
                    flags |= TextFormatFlags.Top;
                    break;

                case ContentAlignment.MiddleLeft:
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.MiddleRight:
                    flags |= TextFormatFlags.VerticalCenter;
                    break;

                case ContentAlignment.BottomLeft:
                case ContentAlignment.BottomCenter:
                case ContentAlignment.BottomRight:
                    flags |= TextFormatFlags.Bottom;
                    break;
            }

            // ニーモニック関連のフラグ設定
            if (!UseMnemonic)
            {
                flags |= TextFormatFlags.NoPrefix; // &を通常の文字として表示
            }

            // キーボードキューの表示/非表示
            if (!ShowKeyboardCues)
            {
                flags |= TextFormatFlags.HidePrefix; // アンダーラインを非表示
            }

            // 単一行表示
            flags |= TextFormatFlags.SingleLine;

            return flags;
        }
    }
}