using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// 開閉ボタンを有したパネルを提供します。
    /// </summary>
    [ToolboxItem(true)]
    public partial class MetExpanderPanel : Panel
    {
        /// <summary>
        /// 開閉状態を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof(ExpandState), "Expanded")]
        [MetCategory("MetBehavior")]
        [MetDescription("MetExpanderPanelState")]
        public ExpandState State
        {
            get => expanderButton.State;
            set => expanderButton.State = value;
        }

        /// <summary>
        /// SVGイメージを利用する時の設定を取得します。
        /// ImageStyle = Svg の時に利用されます。
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [MetCategory("MetAppearance")]
        [MetDescription("MetExpanderPanelSvg")]
        public SvgConfiguration Svg => expanderButton.Svg;

        /// <summary>
        /// イメージを利用する時の設定を取得します。
        /// ImageStyle = Image の時に利用されます。
        /// サイズはイメージ領域に合わせて引き伸ばされます。
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [MetCategory("MetAppearance")]
        [MetDescription("MetExpanderPanelImage")]
        public ImageConfiguration Image => expanderButton.Image;

        /// <summary>
        /// アイコンイメージに利用するスタイルを取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof(ExpanderIconStyle), "Svg")]
        [MetCategory("MetAppearance")]
        [MetDescription("MetExpanderPanelIconStyle")]
        public ExpanderIconStyle IconStyle
        {
            get => expanderButton.IconStyle;
            set => expanderButton.IconStyle = value;
        }

        /// <summary>
        /// アイコンイメージを表示するかどうかを取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(true)]
        [MetCategory("MetAppearance")]
        [MetDescription("MetExpanderPanelShowIcon")]
        public bool ShowIcon
        {
            get => expanderButton.ShowIcon;
            set => expanderButton.ShowIcon = value;
        }

        /// <summary>
        /// 区切り線を表示するかどうかを取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(true)]
        [MetCategory("MetAppearance")]
        [MetDescription("MetExpanderPanelShowLine")]
        public bool ShowLine
        {
            get => expanderButton.ShowLine;
            set => expanderButton.ShowLine = value;
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
        [MetDescription("MetExpanderPanelText")]
        public override string Text
        {
            get => expanderButton.Text;
            set => expanderButton.Text = value;
        }

        /// <summary>
        /// 区切り線の色を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof(Color), "WindowFrame")]
        [MetCategory("MetAppearance")]
        [MetDescription("MetExpanderPanelLineColor")]
        public Color LineColor
        {
            get => expanderButton.LineColor;
            set => expanderButton.LineColor = value;
        }

        /// <summary>
        /// 区切り線の太さを取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(1)]
        [MetCategory("MetAppearance")]
        [MetDescription("MetExpanderPanelLineThickness")]
        public float LineThickness
        {
            get => expanderButton.LineThickness;
            set => expanderButton.LineThickness = value;
        }

        /// <summary>
        /// マウスがアイコンやタイトルの表示領域に入った時のタイトルの文字色を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof(Color), "ControlText")]
        [MetCategory("MetAppearance")]
        [MetDescription("MetExpanderPanelHoverForeColor")]
        public Color HoverForeColor
        {
            get => expanderButton.HoverForeColor;
            set => expanderButton.HoverForeColor = value;
        }

        /// <summary>
        /// タイトルのフォントを取得または設定します。
        /// </summary>
        [Browsable(true)]
        [MetCategory("MetAppearance")]
        [MetDescription("MetExpanderPanelHeaderFont")]
        public Font HeaderFont
        {
            get => expanderButton.Font;
            set => expanderButton.Font = value;
        }

        /// <summary>
        /// HeaderFontを設置しているコントロールのFontにリセットする。
        /// </summary>
        private void ResetHeaderFont()
        {
            if (Parent == null)
            {
                return;
            }

            expanderButton.Font = Parent.Font;
        }

        /// <summary>
        /// HeaderFontが設置しているコントロールのFontと異なる場合、変更ありとみなす。
        /// </summary>
        /// <returns></returns>
        private bool ShouldSerializeHeaderFont()
        {
            if (Parent == null)
            {
                return false;
            }

            if (Parent.Font == expanderButton.Font)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// タイトルの文字色を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [MetCategory("MetAppearance")]
        [MetDescription("MetExpanderPanelHeaderForeColor")]
        public Color HeaderForeColor
        {
            get => expanderButton.ForeColor;
            set => expanderButton.ForeColor = value;
        }

        /// <summary>
        /// タイトルのパディングを取得または設定します。
        /// </summary>
        [Browsable(true)]
        [MetCategory("MetArrangement")]
        [MetDescription("MetExpanderPanelHeaderPadding")]
        public Padding HeaderPadding
        {
            get => wrapPanel.Padding;
            set => wrapPanel.Padding = value;
        }

        /// <summary>
        /// パネルの高さを取得または設定する。
        /// </summary>
        private int PanelHeight { get; set; }

        /// <summary>
        /// ボタン開閉にアニメーションを利用するかどうかを取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(false)]
        [MetCategory("MetBehavior")]
        [MetDescription("MetExpanderPanelUseAnimation")]
        public bool UseAnimation { get; set; } = false;

        /// <summary>
        /// アニメーション加速度を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [MetCategory("MetBehavior")]
        [MetDescription("MetExpanderPanelAcceleration")]
        public int Acceleration { get; set; } = 2;

        /// <summary>
        /// 閉じた時にヘッダーラインを表示するかどうかを取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(true)]
        [MetCategory("MetAppearance")]
        [MetDescription("MetExpanderPanelCollapsedHeaderLineVisibled")]
        public bool CollapsedHeaderLineVisibled { get; set; } = true;

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        public MetExpanderPanel()
        {
            InitializeComponent();

            PanelHeight = Height;
        }

        /// <summary>
        /// とにかく開閉ボタンを最上部に位置させる。
        /// </summary>
        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            wrapPanel.SendToBack();
        }

        /// <summary>
        /// 閉じてる時に高さは変更できないようにする。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSizeChanged(EventArgs e)
        {
            // 展開制御中に限り高さ変更可能とする
            if (animationEnded)
            {
                if (State == ExpandState.Expanded)
                {
                    PanelHeight = Height;
                }
                else
                {
                    // 展開制御が終わっていても、パネルを閉じていたら高さ変更不可
                    Height = wrapPanel.Height;
                }
            }

            base.OnSizeChanged(e);
        }

        /// <summary>
        /// 開閉状態が変化した時、パネルを開閉する。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void expanderButton_ExpandStateChanged(object sender, ExpandStateEventArgs e)
        {
            OpenAndClosePanel(OnceAt);
        }

        /// <summary>
        /// 開閉の動作を即座に行うかどうか。
        /// </summary>
        private bool OnceAt { get; set; } = false;

        /// <summary>
        /// パネルを開きます。
        /// </summary>
        /// <param name="onceAt">即座に開閉するかどうか。</param>
        public void Expand(bool onceAt = false)
        {
            OnceAt = onceAt;
            expanderButton.State = ExpandState.Expanded;
            OnceAt = false;
        }

        /// <summary>
        /// パネルを閉じます。
        /// </summary>
        /// <param name="onceAt">即座に開閉するかどうか。</param>
        public void Collapse(bool onceAt = false)
        {
            OnceAt = onceAt;
            expanderButton.State = ExpandState.Collapsed;
            OnceAt = false;
        }

        /// <summary>
        /// パネルを開閉する。
        /// </summary>
        /// <param name="onceAt">即座に開閉するかどうか。</param>
        private void OpenAndClosePanel(bool onceAt = false)
        {
            if (onceAt || !UseAnimation)
            {
                if (State == ExpandState.Expanded)
                {
                    if (!CollapsedHeaderLineVisibled)
                    {
                        expanderButton.ShowLine = true;
                    }
                    Height = PanelHeight;
                }
                else
                {
                    if (!CollapsedHeaderLineVisibled)
                    {
                        expanderButton.ShowLine = false;
                    }
                    Height = wrapPanel.Height;
                }
                OnExpandStateChanged();
                return;
            }

            animationEnded = false;
            heightExpandVolume = 1;
            animationTimer.Enabled = true;
        }

        private bool animationEnded = true;
        private int heightExpandVolume = 0;

        /// <summary>
        /// 開閉アニメーションを行う。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void animationTimer_Tick(object sender, EventArgs e)
        {
            heightExpandVolume += Acceleration;

            if (State == ExpandState.Expanded)
            {
                if (!CollapsedHeaderLineVisibled)
                {
                    expanderButton.ShowLine = true;
                }
                if ((Height + heightExpandVolume) < PanelHeight)
                {
                    Height += heightExpandVolume;
                }
                else
                {
                    Height = PanelHeight;
                    animationEnded = true;
                }
            }
            else
            {
                if ((Height - heightExpandVolume) > wrapPanel.Height)
                {
                    Height -= heightExpandVolume;
                }
                else
                {
                    if (!CollapsedHeaderLineVisibled)
                    {
                        expanderButton.ShowLine = false;
                    }
                    Height = wrapPanel.Height;
                    animationEnded = true;
                }
            }

            animationTimer.Enabled = !animationEnded;
            if (animationEnded)
            {
                OnExpandStateChanged();
            }
        }

        /// <summary>
        /// ラップパネルのサイズを調整する。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void expanderButton_SizeChanged(object sender, EventArgs e)
        {
            wrapPanel.Height = expanderButton.Height + wrapPanel.Padding.Top + wrapPanel.Padding.Bottom;
        }

        /// <summary>
        /// 展開状態が変更された時に発生します。
        /// </summary>
        [Browsable(true)]
        [MetCategory("MetPropertyChange")]
        [MetDescription("MetExpanderPanelExpandStateChanged")]
        public event ExpandStateEventHandler ExpandStateChanged;

        /// <summary>
        /// 展開状態が変更された時にイベントを発生させます。
        /// </summary>
        protected virtual void OnExpandStateChanged()
        {
            ExpandStateChanged?.Invoke(this, new ExpandStateEventArgs(State));
        }
    }
}
