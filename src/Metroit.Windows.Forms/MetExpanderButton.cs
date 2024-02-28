using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// 開閉ボタンを提供します。
    /// </summary>
    [ToolboxItem(true)]
    public partial class MetExpanderButton : UserControl
    {
        /// <summary>
        /// 展開時のアイコンイメージ。
        /// </summary>
        private Image expandedImage;

        /// <summary>
        /// 縮小時のアイコンイメージ。
        /// </summary>
        private Image collapsedImage;

        /// <summary>
        /// 捕捉したタイトルの高さ。
        /// </summary>
        private int capturedTitleHeight;

        /// <summary>
        /// 開閉状態。
        /// </summary>
        private ExpandState state = ExpandState.Expanded;

        /// <summary>
        /// 開閉状態を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof(ExpandState), "Expanded")]
        [MetCategory("MetBehavior")]
        [MetDescription("MetExpanderButtonState")]
        public ExpandState State
        {
            get => state;
            set
            {
                state = value;
                SwitchIcon();
                OnExpandStateChanged();
            }
        }

        /// <summary>
        /// SVGイメージを利用する時の設定を取得または設定します。
        /// ImageStyle = Svg の時に利用されます。
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [MetCategory("MetAppearance")]
        [MetDescription("MetExpanderButtonSvg")]
        public SvgConfiguration Svg { get; } = new SvgConfiguration();

        /// <summary>
        /// イメージを利用する時の設定を取得または設定します。
        /// ImageStyle = Image の時に利用されます。
        /// サイズはイメージ領域に合わせて引き伸ばされます。
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [MetCategory("MetAppearance")]
        [MetDescription("MetExpanderButtonImage")]
        public ImageConfiguration Image { get; } = new ImageConfiguration();

        private ExpanderIconStyle iconStyle = ExpanderIconStyle.Svg;

        /// <summary>
        /// アイコンイメージに利用するスタイルを取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof(ExpanderIconStyle), "Svg")]
        [MetCategory("MetAppearance")]
        [MetDescription("MetExpanderButtonIconStyle")]
        public ExpanderIconStyle IconStyle
        {
            get => iconStyle;
            set
            {
                iconStyle = value;
                GenerateImage();
            }
        }

        private bool showIcon = true;

        /// <summary>
        /// アイコンイメージを表示するかどうかを取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(true)]
        [MetCategory("MetAppearance")]
        [MetDescription("MetExpanderButtonShowIcon")]
        public bool ShowIcon
        {
            get => showIcon;
            set
            {
                showIcon = value;
                expanderIcon.Visible = showIcon;
                DrawAndRelayoutWithoutCapture();
            }
        }

        private bool showLine = true;

        /// <summary>
        /// 区切り線を表示するかどうかを取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(true)]
        [MetCategory("MetAppearance")]
        [MetDescription("MetExpanderButtonShowLine")]
        public bool ShowLine
        {
            get => showLine;
            set
            {
                showLine = value;
                DrawAndRelayoutWithoutCapture();
            }
        }

        /// <summary>
        /// タイトルを取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue("")]
        [Localizable(true)]
        [Bindable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [MetCategory("MetAppearance")]
        [MetDescription("MetExpanderButtonText")]
        public override string Text
        {
            get => titleLabel.Text;
            set => titleLabel.Text = value;
        }

        private Color lineColor = SystemColors.WindowFrame;

        /// <summary>
        /// 区切り線の色を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof(Color), "WindowFrame")]
        [MetCategory("MetAppearance")]
        [MetDescription("MetExpanderButtonLineColor")]
        public Color LineColor
        {
            get => lineColor;
            set
            {
                lineColor = value;
                Invalidate();
            }
        }

        private float lineThickness = 1;

        /// <summary>
        /// 区切り線の太さを取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(1)]
        [MetCategory("MetAppearance")]
        [MetDescription("MetExpanderButtonLineThickness")]
        public float LineThickness
        {
            get => lineThickness;
            set
            {
                if (value < 1 || value > 100)
                {
                    throw new ArgumentOutOfRangeException(nameof(LineThickness));
                }
                lineThickness = value;
                DrawAndRelayoutWithoutCapture();
            }
        }

        /// <summary>
        /// マウスがアイコンやタイトルの表示領域に入った時のタイトルの文字色を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof(Color), "ControlText")]
        [MetCategory("MetAppearance")]
        [MetDescription("MetExpanderButtonHoverForeColor")]
        public Color HoverForeColor { get; set; } = SystemColors.ControlText;

        /// <summary>
        /// 展開状態が変更された時に発生します。
        /// </summary>
        [Browsable(true)]
        [MetCategory("MetPropertyChange")]
        [MetDescription("MetExpanderButtonExpandStateChanged")]
        public event ExpandStateEventHandler ExpandStateChanged;

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        public MetExpanderButton()
        {
            InitializeComponent();

            Svg.PropertyChanged += IconConfigurationPropertyChanged;
            Image.PropertyChanged += IconConfigurationPropertyChanged;
        }

        /// <summary>
        /// SVGイメージ, イメージの設定情報が変更された時、アイコンを生成し直す。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IconConfigurationPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            GenerateImage();
        }

        /// <summary>
        /// コンポーネントがフォーム設置されたらレイアウトを生成する。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExpanderButton_ParentChanged(object sender, EventArgs e)
        {
            if (Parent == null)
            {
                return;
            }

            RelayoutAndGenerate();
        }

        /// <summary>
        /// フォントが変更された時、レイアウトを再配置し直す。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void titleLabel_FontChanged(object sender, EventArgs e)
        {
            RelayoutAndGenerate();
        }

        /// <summary>
        /// 任意の高さに調整することを不可とする。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExpanderButton_SizeChanged(object sender, EventArgs e)
        {
            Relayout();
        }

        /// <summary>
        /// マウスホバーする前の既定の文字色。
        /// </summary>
        private Color prevHoverForeColor;

        /// <summary>
        /// マウスがヘッダー情報（アイコン、タイトル）の表示領域に入った時のタイトルの文字色を切り替える。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HeaderMouseEnter(object sender, EventArgs e)
        {
            prevHoverForeColor = titleLabel.ForeColor;
            titleLabel.ForeColor = HoverForeColor;
        }

        /// <summary>
        /// マウスがヘッダー情報（アイコン、タイトル）の表示領域から離れた時のタイトルの文字色を切り替える。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HeaderMouseLeave(object sender, EventArgs e)
        {
            titleLabel.ForeColor = prevHoverForeColor;
        }

        /// <summary>
        /// ヘッダー情報（アイコン、タイトル）をクリックした時、開閉を切り替える。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HeaderMouseClick(object sender, MouseEventArgs e)
        {
            SwitchState();
        }

        /// <summary>
        /// 展開状態が変更された時にイベントを発生させます。
        /// </summary>
        protected virtual void OnExpandStateChanged()
        {
            ExpandStateChanged?.Invoke(this, new ExpandStateEventArgs(State));
        }

        /// <summary>
        /// 描画およびレイアウト再配置を行い、タイトルサイズのキャプチャは行わない。
        /// </summary>
        private void DrawAndRelayoutWithoutCapture()
        {
            Invalidate();
            Relayout(false);
        }

        /// <summary>
        /// レイアウト再配置およびアイコンイメージの再生成を行う。タイトルサイズのキャプチャも行う。
        /// </summary>
        private void RelayoutAndGenerate()
        {
            Relayout(true);
            GenerateImage();
        }

        /// <summary>
        /// フォントに応じて全体レイアウトの再配置を行う。
        /// </summary>
        /// <param name="captureTitleSize">タイトルサイズをキャプチャするかどうか。</param>
        private void Relayout(bool captureTitleSize = true)
        {
            if (captureTitleSize)
            {
                // NOTE: 一旦、タイトルのサイズを把握するためにAutoSize=trueとする。
                titleLabel.AutoSize = true;
                capturedTitleHeight = titleLabel.Height;
                titleLabel.AutoSize = false;
            }

            if (ShowIcon)
            {
                // 画像サイズをフォントの高さに合わせる
                FitIconSize();
            }

            // 文字の位置とサイズ
            FitTitleLocationAndSize();

            // コンポーネント全体の高さ
            FitComponentHeight();
        }

        /// <summary>
        /// アイコンのサイズをフィットさせる。
        /// </summary>
        private void FitIconSize()
        {
            var iconSize = new Size(capturedTitleHeight, capturedTitleHeight);
            expanderIcon.Size = iconSize;
        }

        /// <summary>
        /// タイトルの位置とサイズをフィットさせる。
        /// </summary>
        private void FitTitleLocationAndSize()
        {
            // 文字の位置とサイズ
            if (ShowIcon)
            {
                titleLabel.Location = new Point(expanderIcon.Width + expanderIcon.Margin.Right, 0);
                titleLabel.Size = new Size(Width - expanderIcon.Width - expanderIcon.Margin.Right, capturedTitleHeight);
            }
            else
            {
                titleLabel.Location = new Point(0, 0);
                titleLabel.Size = new Size(Width, capturedTitleHeight);
            }
        }

        /// <summary>
        /// コンポーネント全体の高さをフィットさせる。
        /// </summary>
        private void FitComponentHeight()
        {
            // NOTE: 区切り線の描画太さを考慮して決定する
            var height = titleLabel.Height;
            if (ShowLine)
            {
                height += (int)LineThickness;
            }
            Height = height;
        }

        /// <summary>
        /// アイコンイメージを作り直す。
        /// </summary>
        private void GenerateImage()
        {
            if (IconStyle == ExpanderIconStyle.Svg)
            {
                GenerateSvgImageSet();
            }
            else
            {
                GenerateImageSet();
            }

            SwitchIcon();
        }

        /// <summary>
        /// SVGイメージセットを生成する。
        /// </summary>
        private void GenerateSvgImageSet()
        {
            expanderIcon.SizeMode = PictureBoxSizeMode.Normal;

            expandedImage = Svg.Expanded.Generate(expanderIcon.Width, expanderIcon.Height);
            collapsedImage = Svg.Collapsed.Generate(expanderIcon.Width, expanderIcon.Height);
        }

        /// <summary>
        /// イメージセットを生成する。
        /// </summary>
        private void GenerateImageSet()
        {
            expanderIcon.SizeMode = PictureBoxSizeMode.StretchImage;

            expandedImage = Image.Expanded.Generate();
            collapsedImage = Image.Collapsed.Generate();
        }

        /// <summary>
        /// 区切り線の描画を行います。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            // 区切り線
            DrawLine(e.Graphics);

            base.OnPaint(e);
        }

        /// <summary>
        /// 区切り線を描画する。
        /// </summary>
        /// <param name="g">コントロールグラフィックオブジェクト。</param>
        private void DrawLine(Graphics g)
        {
            if (!ShowLine)
            {
                return;
            }

            // NOTE: DrawLine()は、線の中央から上下に広がって描画されるため、中央位置で描画するようにする。
            var brush = new SolidBrush(lineColor);
            var thicknessHalfHeight = (int)(LineThickness / 2);
            var x = 0;
            if (ShowIcon)
            {
                x = expanderIcon.Width + expanderIcon.Margin.Right;
            }
            var y = titleLabel.Height + thicknessHalfHeight;
            g.DrawLine(new Pen(brush, LineThickness), new Point(x, y), new Point(Width, y));
        }

        /// <summary>
        /// 開閉状態を切り替える。
        /// </summary>
        private void SwitchState()
        {
            State = State == ExpandState.Expanded ? ExpandState.Collapsed : ExpandState.Expanded;
            SwitchIcon();
        }

        /// <summary>
        /// 開閉状態に応じたアイコンイメージに切り替える。
        /// </summary>
        private void SwitchIcon()
        {
            expanderIcon.Image = State == ExpandState.Expanded ? expandedImage : collapsedImage;
        }
    }
}
