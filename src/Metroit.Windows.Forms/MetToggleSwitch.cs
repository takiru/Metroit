using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// トグルスイッチを提供します。
    /// </summary>
    [DefaultEvent("CheckedChanged")]
    public class MetToggleSwitch : Control, IControlRollback
    {
        private Timer _animationTimer;
        private float _animationProgress = 0f;
        private readonly int AnimationDuration = 150;
        private readonly int AnimationFps = 60;
        private DateTime _animationStartTime;
        private bool _isAnimating = false;

        /// <summary>
        /// 最低トグルスイッチ幅。
        /// </summary>
        private readonly int MinSwitchWidth = 40;

        /// <summary>
        /// 最低トグルスイッチ高さ。
        /// </summary>
        private readonly int MinSwitchHeight = 20;

        /// <summary>
        /// サム円のマージン。
        /// </summary>
        private readonly int ThumbMargin = 1;

        /// <summary>
        /// フォーカス枠の幅
        /// </summary>
        private static readonly int FocusWidth = 4;

        /// <summary>
        /// トグルスイッチの枠幅。
        /// </summary>
        private static readonly int SwitchBorderWidth = 1;

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        public MetToggleSwitch()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.DoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.SupportsTransparentBackColor, true);

            Size = new Size(MinSwitchWidth, MinSwitchHeight);
            base.BackColor = Color.Transparent;
            Cursor = Cursors.Hand;

            _animationTimer = new Timer();
            _animationTimer.Interval = 1000 / AnimationFps;
            _animationTimer.Tick += AnimationTimer_Tick;

            Enter += MetToggleSwitch_Enter;
        }

        /// <summary>
        /// BackColorプロパティを隠します（このコントロールでは使用しません）
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Obsolete("BackColorは使用できません。", true)]
#pragma warning disable CS0809
        public override Color BackColor
#pragma warning restore CS0809
        {
            get => base.BackColor;
            set { }
        }

        /// <summary>
        /// トグルスイッチの状態が変更されるときに発生します。
        /// </summary>
        [Browsable(true)]
        [MetCategory("MetOther")]
        [MetDescription("MetToggleSwitchCheckedChanging")]
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
        /// トグルスイッチの状態が変更されたときに発生します。
        /// </summary>
        [Browsable(true)]
        [MetCategory("MetOther")]
        [MetDescription("MetToggleSwitchCheckedChanged")]
        public event EventHandler CheckedChanged;

        /// <summary>
        /// <see cref="CheckedChanged"/> イベントを発生させます。
        /// </summary>
        /// <param name="e">イベントデータ。</param>
        protected virtual void OnCheckedChanged(EventArgs e)
        {
            CheckedChanged?.Invoke(this, e);
        }

        private bool _checked = false;

        /// <summary>
        /// トグルスイッチの状態を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(false)]
        [Bindable(true)]
        [MetCategory("MetAppearance")]
        [MetDescription("MetToggleSwitchChecked")]
        public bool Checked
        {
            get { return _checked; }
            set
            {
                if (_checked != value)
                {
                    var checkedCancelArgs = new CheckedChangingEventArgs(Checked, !Checked);
                    OnCheckedChanging(checkedCancelArgs);
                    if (checkedCancelArgs.Cancel)
                    {
                        return;
                    }

                    _checked = value;
                    StartAnimation();
                    OnCheckedChanged(EventArgs.Empty);
                }
            }
        }

        private OnToggleButtonAppearance _onToggleButtonAppearance;

        /// <summary>
        /// ONのときの外観を決定します。
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [MetCategory("MetAppearance")]
        [MetDescription("MetToggleSwitchOnAppearance")]
        public OnToggleButtonAppearance OnAppearance
        {
            get
            {
                if (_onToggleButtonAppearance == null)
                {
                    _onToggleButtonAppearance = new OnToggleButtonAppearance(this);
                }

                return _onToggleButtonAppearance;
            }
        }

        private OffToggleButtonAppearance _offToggleButtonAppearance;

        /// <summary>
        /// OFFのときの外観を決定します。
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [MetCategory("MetAppearance")]
        [MetDescription("MetToggleSwitchOffAppearance")]
        public OffToggleButtonAppearance OffAppearance
        {
            get
            {
                if (_offToggleButtonAppearance == null)
                {
                    _offToggleButtonAppearance = new OffToggleButtonAppearance(this);
                }

                return _offToggleButtonAppearance;
            }
        }

        private bool _showState = false;

        /// <summary>
        /// トグルスイッチの状態を表示するかどうかを示す値を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(false)]
        [MetCategory("MetAppearance")]
        [MetDescription("MetToggleSwitchShowState")]
        public bool ShowState
        {
            get { return _showState; }
            set
            {
                if (_showState != value)
                {
                    _showState = value;
                    Invalidate();
                }
            }
        }

        private ToggleSwitchStatePosition _statePosition = ToggleSwitchStatePosition.Left;

        /// <summary>
        /// トグルスイッチの状態を表示する位置を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof(ToggleSwitchStatePosition), "Left")]
        [MetCategory("MetAppearance")]
        [MetDescription("MetToggleSwitchStatePosition")]
        public ToggleSwitchStatePosition StatePosition
        {
            get { return _statePosition; }
            set
            {
                if (_statePosition != value)
                {
                    _statePosition = value;
                    Invalidate();
                }
            }
        }

        private bool _autoSize = true;

        /// <summary>
        /// トグルスイッチの自動サイズ調整を有効または無効にします。
        /// </summary>
        [DefaultValue(true)]
        [Browsable(true)]
        [MetDescription("MetToggleSwitchAutoSize")]
        [RefreshProperties(RefreshProperties.All)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public new bool AutoSize
        {
            get { return _autoSize; }
            set
            {
                if (_autoSize != value)
                {
                    _autoSize = value;
                    Invalidate();
                }
            }
        }

        // -1 の場合は自動計算
        private int _cornerRadius = -1;

        /// <summary>
        /// トグルスイッチの角丸半径を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(-1)]
        [MetCategory("MetAppearance")]
        [MetDescription("MetToggleSwitchCornerRadius")]
        public int CornerRadius
        {
            get { return _cornerRadius; }
            set
            {
                if (_cornerRadius != value)
                {
                    _cornerRadius = Math.Max(-1, value);
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
        [MetDescription("MetToggleSwitchFocusColor")]
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
        /// アニメーションを開始します。
        /// </summary>
        private void StartAnimation()
        {
            if (!_isAnimating)
            {
                _animationStartTime = DateTime.Now;
                _isAnimating = true;
                _animationTimer.Start();
            }
        }

        /// <summary>
        /// アニメーションしてトグルスイッチを切り替える。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            var elapsed = (DateTime.Now - _animationStartTime).TotalMilliseconds;
            _animationProgress = Math.Min(1.0f, (float)(elapsed / AnimationDuration));

            // イージング関数（スムーズなアニメーション）
            var easedProgress = EaseOutCubic(_animationProgress);

            if (_animationProgress >= 1.0f)
            {
                _animationTimer.Stop();
                _isAnimating = false;
                _animationProgress = _checked ? 1.0f : 0.0f;
            }
            else
            {
                _animationProgress = _checked ? easedProgress : 1.0f - easedProgress;
            }

            Invalidate();
        }

        /// <summary>
        /// アニメーションのイージング位置を求める。
        /// </summary>
        /// <param name="progress">現在の描画位置。</param>
        /// <returns>描画する位置。</returns>
        private float EaseOutCubic(float progress)
        {
            return 1f - (float)Math.Pow(1 - progress, 3);
        }

        /// <summary>
        /// コントロールのサイズを自動的に更新します。
        /// </summary>
        internal void UpdateControlSize(Graphics g)
        {
            if (!AutoSize)
            {
                return;
            }

            var textSize = GetTextSize(g);
            var thumbSize = GetThumbSize(textSize.Height);
            var switchBorderSize = GetSwitchBorderSize(textSize, thumbSize);
            var focusSize = GetFocusSize(switchBorderSize);

            if (ShowState && StatePosition == ToggleSwitchStatePosition.Inside)
            {
                Size = new Size(
                    (int)Math.Ceiling(focusSize.Width),
                    (int)Math.Ceiling(focusSize.Height));
                return;
            }

            Size = new Size(
                (int)Math.Ceiling((ShowState ? textSize.Width : 0) + focusSize.Width),
                (int)Math.Ceiling(focusSize.Height));
        }

        /// <summary>
        /// サム円が左にあるときと右にあるときの中間間隔。
        /// </summary>
        private static readonly int ThumbInterval = 4;

        /// <summary>
        /// スイッチボーダー領域のサイズを求める。
        /// </summary>
        /// <param name="textSize">テキストサイズ。</param>
        /// <param name="thumbSize">サム円サイズ。</param>
        /// <returns>スイッチボーダーサイズ。</returns>
        private SizeF GetSwitchBorderSize(SizeF textSize, SizeF thumbSize)
        {
            // 高さはテキストの高さと一緒
            var height = textSize.Height;

            float width;
            // 幅はサム円のサイズと中間間隔, サム円のマージン, スイッチボーダーの合算
            if (StatePosition == ToggleSwitchStatePosition.Left || StatePosition == ToggleSwitchStatePosition.Right)
            {
                width = (thumbSize.Width * 2 / (CornerRadius == 0 ? 0.6f : 1)) +
                    ThumbInterval + (ThumbMargin * 2) + (SwitchBorderWidth * 2);
            }
            else
            {
                // 内部のときは幅全体
                if (AutoSize)
                {
                    width = textSize.Width +
                        (thumbSize.Width * 2 / (CornerRadius == 0 ? 0.6f : 1)) +
                        (ThumbMargin * 2) + (SwitchBorderWidth * 2);
                }
                else
                {
                    width = Width - (FocusWidth * 2);
                }
            }

            return new SizeF(width, height);
        }

        /// <summary>
        /// ボーダーも含めたスイッチの矩形を取得する。
        /// テキストサイズの高さに合わせたスイッチとする。
        /// </summary>
        /// <returns>スイッチの矩形。</returns>
        private RectangleF GetSwitchBorderRectangle(SizeF switchBorderSize)
        {
            var y = (Height - switchBorderSize.Height) / 2f;

            float x = 0;
            if (StatePosition == ToggleSwitchStatePosition.Left)
            {
                x = Width - FocusWidth - switchBorderSize.Width;
            }
            if (StatePosition == ToggleSwitchStatePosition.Right)
            {
                x = FocusWidth;
            }
            if (StatePosition == ToggleSwitchStatePosition.Inside)
            {
                x = FocusWidth;
            }

            return new RectangleF(x, y, switchBorderSize.Width, switchBorderSize.Height);
        }

        /// <summary>
        /// コントロールの描画を行います。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (AutoSize)
            {
                UpdateControlSize(e.Graphics);
            }

            DrawRoundedShape(e.Graphics);
        }

        /// <summary>
        /// 角丸図形を描画する。
        /// フォーカス時の枠線、境界線、背景色を描画する。
        /// </summary>
        /// <param name="g">グラフィックス。</param>
        public void DrawRoundedShape(Graphics g)
        {
            if (Width <= 0 || Height <= 0)
            {
                return;
            }

            var oldSmoothing = g.SmoothingMode;
            var oldInterpolation = g.InterpolationMode;
            var oldCompositing = g.CompositingQuality;
            var oldPixelOffset = g.PixelOffsetMode;
            var oldTextRenderingHint = g.TextRenderingHint;

            try
            {
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                var contentRect = new RectangleF(0, 0, Width, Height);

                // 描画領域を親の背景で塗りつぶす
                DrawTransparentBackground(g, new Region(contentRect));

                // コンテンツ領域
                using (GraphicsPath contentPath = CreateRoundedPath(contentRect, 0, 0, 0, 0))
                {
                    var textSize = GetTextSize(g);
                    var thumbSize = GetThumbSize(textSize.Height);
                    var switchBorderSize = GetSwitchBorderSize(textSize, thumbSize);
                    var switchBorderRect = GetSwitchBorderRectangle(switchBorderSize);
                    var cornerRadius = CornerRadius == -1 ? Height / 2 : CornerRadius;

                    // フォーカス枠の描画
                    var focusSize = GetFocusSize(switchBorderSize);
                    var focusRect = GetFocusRect(switchBorderRect, focusSize);
                    if (Focused)
                    {
                        using (GraphicsPath borderPath = CreateRoundedPath(focusRect,
                            cornerRadius, cornerRadius, cornerRadius, cornerRadius))
                        {
                            using (SolidBrush borderBrush = new SolidBrush(FocusColor))
                            {
                                g.FillPath(borderBrush, borderPath);
                            }
                        }
                    }

                    // トグルスイッチ枠の描画
                    using (GraphicsPath borderPath = CreateRoundedPath(switchBorderRect,
                        cornerRadius, cornerRadius, cornerRadius, cornerRadius))
                    {

                        using (SolidBrush borderBrush = new SolidBrush(GetCurrentBorderColor()))
                        {
                            g.FillPath(borderBrush, borderPath);
                        }
                    }

                    // トグルスイッチのボーダーを除いたトグルスイッチ背景の描画
                    var switchBackgroundRect = new RectangleF(
                        switchBorderRect.X + SwitchBorderWidth,
                        switchBorderRect.Y + SwitchBorderWidth,
                        switchBorderRect.Width - (SwitchBorderWidth * 2),
                        switchBorderRect.Height - (SwitchBorderWidth * 2));
                    using (GraphicsPath borderPath = CreateRoundedPath(
                        switchBackgroundRect, cornerRadius, cornerRadius, cornerRadius, cornerRadius))
                    {
                        using (SolidBrush borderBrush = new SolidBrush(GetCurrentBackColor()))
                        {
                            g.FillPath(borderBrush, borderPath);
                        }
                    }

                    // サムの描画
                    var thumbDrawingAreaRect = new RectangleF(
                        switchBackgroundRect.X + ThumbMargin,
                        switchBackgroundRect.Y + ThumbMargin,
                        switchBackgroundRect.Width - (ThumbMargin * 2),
                        switchBackgroundRect.Height - (ThumbMargin * 2));
                    using (var brush = new SolidBrush(GetCurrentThumbColor()))
                    {
                        var thumbX = thumbDrawingAreaRect.X +
                            (thumbDrawingAreaRect.Width - thumbSize.Width) * _animationProgress;
                        if (CornerRadius == 0)
                        {
                            g.FillRectangle(brush, new RectangleF(
                                thumbX, thumbDrawingAreaRect.Y, thumbSize.Width, thumbSize.Height));
                        }
                        else
                        {
                            g.FillEllipse(brush, new RectangleF(
                                thumbX, thumbDrawingAreaRect.Y, thumbSize.Width, thumbSize.Height));
                        }
                    }

                    // テキストの描画
                    if (ShowState)
                    {
                        var textRect = GetTextRect(focusRect, textSize);
                        DrawStateText(g, textRect);
                    }
                }
            }
            finally
            {
                g.SmoothingMode = oldSmoothing;
                g.InterpolationMode = oldInterpolation;
                g.CompositingQuality = oldCompositing;
                g.PixelOffsetMode = oldPixelOffset;
                g.TextRenderingHint = oldTextRenderingHint;
            }
        }

        /// <summary>
        /// フォーカスサイズを求める。
        /// </summary>
        /// <param name="switchBorderSize">スイッチのボーダーサイズ。</param>
        /// <returns>フォーカスサイズ。</returns>
        private static SizeF GetFocusSize(SizeF switchBorderSize)
        {
            return new SizeF(switchBorderSize.Width + (FocusWidth * 2), switchBorderSize.Height + (FocusWidth * 2));
        }

        /// <summary>
        /// フォーカス枠描画位置を求める
        /// </summary>
        /// <param name="switchBorderRect">スイッチボーダーの描画領域。</param>
        /// <param name="focusSize">フォーカスサイズ。</param>
        /// <returns>フォーカス描画領域。</returns>
        private static RectangleF GetFocusRect(RectangleF switchBorderRect, SizeF focusSize)
        {
            return new RectangleF(
                switchBorderRect.X - FocusWidth,
                switchBorderRect.Y - FocusWidth,
                focusSize.Width,
                focusSize.Height);
        }

        /// <summary>
        /// ON/OFFテキストの表示に必要なサイズを求める。
        /// </summary>
        /// <param name="g">グラフィックス。</param>
        /// <returns>テキストの表示に必要なサイズ。</returns>
        private SizeF GetTextSize(Graphics g)
        {
            var onTextSize = g.MeasureString(OnAppearance.Text, Font);
            var offTextSize = g.MeasureString(OffAppearance.Text, Font);
            var textWidth = Math.Max(onTextSize.Width, offTextSize.Width);
            var textHeight = Math.Max(onTextSize.Height, offTextSize.Height);

            // ON/OFFともにテキストがないとき、全体の描画のために高さを確保する
            if (textHeight == 0)
            {
                var HeightOnlySize = g.MeasureString("A", Font);
                return new SizeF(0, HeightOnlySize.Height);
            }

            return new SizeF(textWidth, textHeight);
        }

        /// <summary>
        /// トグルスイッチの高さに合わせて、サム円のサイズを求める。
        /// </summary>
        /// <param name="textHeight">テキストの高さ。</param>
        /// <returns>サム円のサイズ。</returns>
        private SizeF GetThumbSize(float textHeight)
        {
            var thumbHeight = textHeight - (SwitchBorderWidth * 2) - (ThumbMargin * 2);

            if (CornerRadius == 0)
            {
                return new SizeF(thumbHeight * 0.6f, thumbHeight);
            }
            else
            {
                return new SizeF(thumbHeight, thumbHeight);
            }
        }

        /// <summary>
        /// 親背景を正確に描画して、透明な背景をクリッピングによって描画する。
        /// </summary>
        /// <param name="g">グラフィックス。</param>
        /// <param name="clipRegion">クリッピング領域。</param>
        private void DrawTransparentBackground(Graphics g, Region clipRegion)
        {
            Region oldClip = g.Clip;
            try
            {
                g.Clip = clipRegion;

                // 親の背景を描画するために、座標を親の座標系に変換
                g.TranslateTransform(-Location.X, -Location.Y);

                // 親コントロールの背景描画を行う
                var e = new PaintEventArgs(g, Bounds);
                InvokePaintBackground(Parent, e);
                InvokePaint(Parent, e);
            }
            finally
            {
                g.TranslateTransform(Location.X, Location.Y);
                g.Clip = oldClip;
            }
        }

        /// <summary>
        /// 角丸パスを作成する。
        /// </summary>
        /// <param name="rect">描画領域。</param>
        /// <param name="topLeftRadius">左上角の角丸半径。</param>
        /// <param name="topRightRadius">右上角の角丸半径。</param>
        /// <param name="bottomLeftRadius">左下角の角丸半径。</param>
        /// <param name="bottomRightRadius">右下角の角丸半径。</param>
        /// <returns>角丸パス。</returns>
        private GraphicsPath CreateRoundedPath(RectangleF rect, float topLeftRadius, float topRightRadius,
            float bottomLeftRadius, float bottomRightRadius)
        {
            GraphicsPath path = new GraphicsPath();

            if (rect.Width <= 0 || rect.Height <= 0)
            {
                return path;
            }

            if (topLeftRadius <= 0 && topRightRadius <= 0 && bottomLeftRadius <= 0 && bottomRightRadius <= 0)
            {
                path.AddRectangle(rect);
                return path;
            }

            // 半径の境界チェック
            float maxRadius = Math.Min(rect.Width, rect.Height) / 2f;
            var drawTopLeftRadius = Math.Min(topLeftRadius, maxRadius);
            var drawTopRightRadius = Math.Min(topRightRadius, maxRadius);
            var drawBottomLeftRadius = Math.Min(bottomLeftRadius, maxRadius);
            var drawBottomRightRadius = Math.Min(bottomRightRadius, maxRadius);

            float x = rect.X;
            float y = rect.Y;
            float width = rect.Width;
            float height = rect.Height;

            // 角丸の直径
            float topLeftDiameter = drawTopLeftRadius * 2f;
            float topRightDiameter = drawTopRightRadius * 2f;
            float bottomLeftDiameter = drawBottomLeftRadius * 2f;
            float bottomRightDiameter = drawBottomRightRadius * 2f;

            // 左上角から開始
            if (drawTopLeftRadius > 0)
            {
                // 左上の円弧
                path.AddArc(x, y,
                    topLeftDiameter, topLeftDiameter, 180f, 90f);
                // 上辺へ移動
                path.AddLine(x + drawTopLeftRadius, y,
                    x + width - drawTopRightRadius, y);
            }
            else
            {
                // 角丸なしの場合は左上角から上辺
                path.AddLine(x, y,
                    x + width - drawTopRightRadius, y);
            }

            // 右上角
            if (drawTopRightRadius > 0)
            {
                // 右上の円弧
                path.AddArc(x + width - topRightDiameter, y, topRightDiameter,
                    topRightDiameter, 270f, 90f);
                // 右辺へ移動
                path.AddLine(x + width, y + drawTopRightRadius,
                    x + width, y + height - drawBottomRightRadius);
            }
            else
            {
                // 角丸なしの場合は右上角から右辺
                path.AddLine(x + width, y, x + width,
                    y + height - drawBottomRightRadius);
            }

            // 右下角
            if (drawBottomRightRadius > 0)
            {
                // 右下の円弧
                path.AddArc(x + width - bottomRightDiameter, y + height - bottomRightDiameter,
                    bottomRightDiameter, bottomRightDiameter, 0f, 90f);
                // 下辺へ移動
                path.AddLine(x + width - drawBottomRightRadius, y + height,
                    x + drawBottomLeftRadius, y + height);
            }
            else
            {
                // 角丸なしの場合は右下角から下辺
                path.AddLine(x + width, y + height,
                    x + drawBottomLeftRadius, y + height);
            }

            // 左下角
            if (drawBottomLeftRadius > 0)
            {
                // 左下の円弧
                path.AddArc(x, y + height - bottomLeftDiameter,
                    bottomLeftDiameter, bottomLeftDiameter, 90f, 90f);
                // 左辺へ移動（開始点まで戻る）
                path.AddLine(x, y + height - drawBottomLeftRadius,
                    x, y + drawTopLeftRadius);
            }
            else
            {
                // 角丸なしの場合は左下角から左辺
                path.AddLine(x, y + height,
                    x, y + drawTopLeftRadius);
            }

            path.CloseFigure();
            return path;
        }


        /// <summary>
        /// テキストの描画位置を求める。
        /// </summary>
        /// <param name="focusRect">フォーカス描画領域。</param>
        /// <param name="textSize">テキストサイズ。</param>
        /// <returns>テキストの描画領域。</returns>
        private RectangleF GetTextRect(RectangleF focusRect, SizeF textSize)
        {
            if (StatePosition == ToggleSwitchStatePosition.Left)
            {
                return new RectangleF(focusRect.X - textSize.Width, (Height - textSize.Height) / 2, textSize.Width, textSize.Height);
            }

            if (StatePosition == ToggleSwitchStatePosition.Right)
            {
                return new RectangleF(focusRect.X + focusRect.Width, (Height - textSize.Height) / 2, textSize.Width, textSize.Height);
            }

            return new RectangleF(focusRect.X + FocusWidth, (Height - textSize.Height) / 2, focusRect.Width - FocusWidth, textSize.Height);
        }

        /// <summary>
        /// テキストを描画する。
        /// </summary>
        /// <param name="g">グラフィックス。</param>
        /// <param name="textRect">テキストの描画位置。</param>
        private void DrawStateText(Graphics g, RectangleF textRect)
        {
            var currentText = Checked ? OnAppearance.Text : OffAppearance.Text;
            if (string.IsNullOrEmpty(currentText))
            {
                return;
            }

            var textColor = Checked ? OnAppearance.TextForeColor : OffAppearance.TextForeColor;
            using (StringFormat stringFormat = CreateStringFormat())
            {
                using (SolidBrush textBrush = new SolidBrush(GetCurrentTextColor()))
                {
                    g.DrawString(currentText, Font, textBrush, textRect, stringFormat);
                }
            }
        }


        /// <summary>
        /// StatePosition の設定値に基づいて StringFormat を作成する。
        /// </summary>
        /// <returns>StatePosition に対応した StringFormat。</returns>
        private StringFormat CreateStringFormat()
        {
            StringFormat stringFormat = new StringFormat();

            // 水平方向の位置
            switch (StatePosition)
            {
                case ToggleSwitchStatePosition.Left:
                    stringFormat.Alignment = StringAlignment.Far;
                    break;
                case ToggleSwitchStatePosition.Inside:
                    stringFormat.Alignment = StringAlignment.Center;
                    break;
                case ToggleSwitchStatePosition.Right:
                    stringFormat.Alignment = StringAlignment.Near;
                    break;
            }

            // 垂直方向の配置
            stringFormat.LineAlignment = StringAlignment.Center;

            stringFormat.Trimming = StringTrimming.EllipsisCharacter;
            stringFormat.FormatFlags = StringFormatFlags.NoWrap;

            return stringFormat;
        }

        /// <summary>
        /// 現在のコントロールとマウス状態に応じたボーダー色を取得する。
        /// </summary>
        /// <returns>現在のボーダー色。</returns>
        private Color GetCurrentBorderColor()
        {
            if (Enabled)
            {
                if (Checked)
                {
                    return OnAppearance.BorderColor;
                }
                else
                {
                    return OffAppearance.BorderColor;
                }
            }

            if (Checked)
            {
                return ControlPaint.Light(OnAppearance.BorderColor);
            }
            else
            {
                return ControlPaint.Light(OffAppearance.BorderColor);
            }
        }

        /// <summary>
        /// 現在のコントロールとマウス状態に応じた背景色を取得する。
        /// </summary>
        /// <returns>現在の背景色。</returns>
        private Color GetCurrentBackColor()
        {
            if (Enabled)
            {
                if (Checked)
                {
                    return OnAppearance.BackColor;
                }
                else
                {
                    return OffAppearance.BackColor;
                }
            }

            if (Checked)
            {
                return OnAppearance.DisabledBackColor;
            }
            else
            {
                return OffAppearance.DisabledBackColor;
            }
        }

        /// <summary>
        /// 現在のコントロールとマウス状態に応じたサム円色を取得する。
        /// </summary>
        /// <returns>現在のサム円色。</returns>
        private Color GetCurrentThumbColor()
        {
            if (Enabled)
            {
                if (Checked)
                {
                    return OnAppearance.ThumbColor;
                }
                else
                {
                    return OffAppearance.ThumbColor;
                }
            }

            if (Checked)
            {
                return OnAppearance.DisabledThumbColor;
            }
            else
            {
                return OffAppearance.DisabledThumbColor;
            }
        }

        /// <summary>
        /// 現在のコントロールとマウス状態に応じた文字色を取得する。
        /// </summary>
        /// <returns>現在の文字色。</returns>
        private Color GetCurrentTextColor()
        {
            if (Enabled)
            {
                if (Checked)
                {
                    return OnAppearance.TextForeColor;
                }
                else
                {
                    return OffAppearance.TextForeColor;
                }
            }

            if (Checked)
            {
                return ControlPaint.Light(OnAppearance.TextForeColor);
            }
            else
            {
                return ControlPaint.Light(OffAppearance.TextForeColor);
            }
        }

        /// <summary>
        /// スペースキーでトグルの切り替えを可能にする。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                Checked = !Checked;
                e.SuppressKeyPress = true;
                return;
            }

            base.OnKeyUp(e);
        }

        /// <summary>
        /// クリックイベントを処理します。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClick(EventArgs e)
        {
            Focus();
            if (Enabled)
            {
                Checked = !Checked;
            }
            base.OnClick(e);
        }

        /// <summary>
        /// フォーカスを得たときに再描画します。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnGotFocus(EventArgs e)
        {
            Invalidate();
            base.OnGotFocus(e);
        }

        /// <summary>
        /// フォーカスを失ったときに再描画します。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLostFocus(EventArgs e)
        {
            Invalidate();
            base.OnLostFocus(e);
        }

        /// <summary>
        /// コントロールの破棄時にリソースを解放します。
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _animationTimer?.Stop();
                _animationTimer?.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// フォーカスを得たときに、現在のチェック状態を保存します。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MetToggleSwitch_Enter(object sender, EventArgs e)
        {
            _enterChecked = Checked;
        }

        private bool _enterChecked = false;

        /// <summary>
        /// ロールバック済みかどうかを取得します。
        /// </summary>
        /// <returns>true:ロールバック済み, false:未ロールバック。</returns>
        [Browsable(false)]
        public virtual bool IsRollbacked => Checked == _enterChecked;

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
            Checked = _enterChecked;
            Rollbacking = false;
        }
    }
}