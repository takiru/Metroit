using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// 角丸のボタンコントロールを提供します。
    /// </summary>
    public class MetRoundedButton : Button
    {
        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        public MetRoundedButton() : base()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.DoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.SupportsTransparentBackColor, true);

            // 既定値
            BackColor = Color.FromArgb(13, 110, 253);
            ForeColor = Color.White;
            Size = new Size(128, 40);
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

            _focusOverlayController = new FocusOverlayController(this);
        }

        /// <summary>
        /// コントロールが破棄されるときにオーバーレイも破棄する。
        /// </summary>
        /// <param name="disposing">マネージリソースを破棄するかどうか。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _focusOverlayController?.Dispose();
            }
            base.Dispose(disposing);
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
        /// コントロールの背景色を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof(Color), "13, 110, 253")]
        public override Color BackColor { get => base.BackColor; set => base.BackColor = value; }

        /// <summary>
        /// コントロールの前景色を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof(Color), "White")]
        public override Color ForeColor { get => base.ForeColor; set => base.ForeColor = value; }

        private static readonly int DefaultRadiusPoint = 5;
        private static readonly RoundedCornerRadius DefaultRadius = new RoundedCornerRadius(DefaultRadiusPoint);
        private RoundedCornerRadius _radius = null;

        /// <summary>
        /// 各角の丸みの半径を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [MetCategory("MetAppearance")]
        [MetDescription("MetRoundedButtonRadius")]
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

        private ButtonExtendsAppearance _extendsAppearance;

        /// <summary>
        /// 拡張された外観を決定します。
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [MetCategory("MetAppearance")]
        [MetDescription("MetRoundedButtonExtendsAppearance")]
        public ButtonExtendsAppearance ExtendsAppearance
        {
            get
            {
                if (_extendsAppearance == null)
                {
                    _extendsAppearance = new ButtonExtendsAppearance(this);
                }

                return _extendsAppearance;
            }
        }

        /// <summary>
        /// コントロールの描画を行います。
        /// </summary>
        /// <param name="e">描画イベントの引数</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            //// ボタンの外観を描画
            _focusOverlayController.DrawRoundedShape(e.Graphics, ExtendsAppearance, Radius);

            // テキストを描画
            DrawText(e.Graphics);
        }

        /// <summary>
        /// テキストを描画する。
        /// </summary>
        /// <param name="g">描画に使用するGraphicsオブジェクト。</param>
        private void DrawText(Graphics g)
        {
            if (string.IsNullOrEmpty(Text))
            {
                return;
            }

            var textColor = GetCurrentTextColor();

            var textRect = ClientRectangle;
            if (ExtendsAppearance.BorderSize > 0)
            {
                var borderPadding = Math.Max(2, ExtendsAppearance.BorderSize);
                textRect.Inflate(-borderPadding, -borderPadding);
            }

            // UseMnemonicがtrueの場合は標準のニーモニック処理を使用
            if (UseMnemonic)
            {
                var flags = CreateTextFormatFlags();
                TextRenderer.DrawText(g, Text, Font, textRect, textColor, flags);
            }
            else
            {
                g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

                using (StringFormat stringFormat = CreateStringFormat())
                {
                    using (SolidBrush textBrush = new SolidBrush(textColor))
                    {
                        g.DrawString(Text, Font, textBrush, textRect, stringFormat);
                    }
                }
            }
        }

        /// <summary>
        /// テキストを描画するためのフラグを作成する。
        /// </summary>
        /// <returns>テキスト描画フラグ。</returns>
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

            // その他のフラグ
            if (UseMnemonic)
            {
                // ニーモニックを有効化（&を解釈してアンダーラインを表示）
                // このフラグを指定しない場合、デフォルトでニーモニックが有効
            }
            else
            {
                flags |= TextFormatFlags.NoPrefix; // &を通常の文字として表示
            }

            if (!ShowKeyboardCues)
            {
                flags |= TextFormatFlags.HidePrefix; // アンダーラインを非表示
            }

            return flags;
        }

        /// <summary>
        /// TextAlign の設定値に基づいて StringFormat を作成する。
        /// </summary>
        /// <returns>TextAlign に対応した StringFormat。</returns>
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
        /// 現在のコントロールとマウス状態に応じた文字色を取得する。
        /// </summary>
        /// <returns>現在の文字色。</returns>
        private Color GetCurrentTextColor()
        {
            if (!Enabled)
            {
                return ControlPaint.Light(ForeColor);
            }

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
}