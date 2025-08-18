using Metroit.Windows.Forms.Extensions;
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
    public class MetToggleSwitch : Control, IControlRollback
    {
        private Timer _animationTimer;
        private float _animationProgress = 0f;
        private readonly int AnimationDuration = 150;
        private readonly int AnimationFps = 60;
        private DateTime _animationStartTime;
        private bool _isAnimating = false;

        private readonly int SwitchWidth = 40;
        private readonly int SiwtchHeight = 20;
        private readonly int BorderWidth = 1;

        private readonly Color DefaultOffThumbColor = Color.FromArgb(51, 51, 51);
        private readonly Color DefaultOnThumbColor = Color.FromArgb(255, 255, 255);
        private readonly Color HoverOverlayColor = Color.FromArgb(20, 0, 0, 0);
        private readonly Color DisabledBorderColor = Color.FromArgb(200, 200, 200);
        private readonly Color DisabledTextColor = Color.FromArgb(150, 150, 150);

        private bool _isHovered = false;

        /// <summary>
        /// トグルスイッチの状態が変更されたときに発生します。
        /// </summary>
        public event EventHandler CheckedChanged;

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

            Size = new Size(SwitchWidth, SiwtchHeight);
            BackColor = Color.Transparent;
            Cursor = Cursors.Hand;

            _animationTimer = new Timer();
            _animationTimer.Interval = 1000 / AnimationFps;
            _animationTimer.Tick += AnimationTimer_Tick;

            UpdateControlSize();

            Enter += MetToggleSwitch_Enter;
        }

        private bool _checked = false;

        /// <summary>
        /// トグルスイッチの状態を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(false)]
        [MetCategory("MetAppearance")]
        [MetDescription("MetToggleSwitchChecked")]
        public bool Checked
        {
            get { return _checked; }
            set
            {
                if (_checked != value)
                {
                    _checked = value;
                    StartAnimation();
                    CheckedChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private string _onText = "ON";

        /// <summary>
        /// トグルスイッチのON状態を示すテキストを取得または設定します。
        /// </summary>
        [Browsable(true)]
        [MetCategory("MetAppearance")]
        [MetDescription("MetToggleSwitchOnText")]
        public string OnText
        {
            get { return _onText; }
            set
            {
                if (_onText != value)
                {
                    _onText = value;
                    if (_autoSize)
                    {
                        UpdateControlSize();
                    }
                    Invalidate();
                }
            }
        }

        private string _offText = "OFF";

        /// <summary>
        /// トグルスイッチのOFF状態を示すテキストを取得または設定します。
        /// </summary>
        [Browsable(true)]
        [MetCategory("MetAppearance")]
        [MetDescription("MetToggleSwitchOffText")]
        public string OffText
        {
            get { return _offText; }
            set
            {
                if (_offText != value)
                {
                    _offText = value;
                    if (_autoSize)
                    {
                        UpdateControlSize();
                    }
                    Invalidate();
                }
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
                    if (_autoSize)
                    {
                        UpdateControlSize();
                    }
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
                    if (_autoSize)
                    {
                        UpdateControlSize();
                    }
                    Invalidate();
                }
            }
        }

        private Color _onTextForeColor = SystemColors.ControlText;

        /// <summary>
        /// トグルスイッチのON状態のテキストの前景色を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [MetCategory("MetAppearance")]
        [MetDescription("MetToggleSwitchOnTextForeColor")]
        public Color OnTextForeColor
        {
            get { return _onTextForeColor; }
            set
            {
                if (_onTextForeColor != value)
                {
                    _onTextForeColor = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// OnTextForeColor プロパティがデフォルト値でない場合に true を返します。
        /// </summary>
        /// <returns>変更があった場合は true, それ以外は false を返却する。</returns>
        private bool ShouldSerializeOnTextForeColor()
        {
            return _onTextForeColor != SystemColors.ControlText;
        }

        /// <summary>
        /// OnTextForeColor プロパティをデフォルト値にリセットします。
        /// </summary>
        private void ResetOnTextForeColor()
        {
            OnTextForeColor = SystemColors.ControlText;
        }

        private Color _offTextForeColor = SystemColors.ControlText;

        /// <summary>
        /// トグルスイッチのOFF状態のテキストの前景色を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [MetCategory("MetAppearance")]
        [MetDescription("MetToggleSwitchOffTextForeColor")]
        public Color OffTextForeColor
        {
            get { return _offTextForeColor; }
            set
            {
                if (_offTextForeColor != value)
                {
                    _offTextForeColor = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// OffTextForeColor プロパティがデフォルト値でない場合に true を返します。
        /// </summary>
        /// <returns>変更がある場合は true, それ以外は false を返却すうｒ。</returns>
        private bool ShouldSerializeOffTextForeColor()
        {
            return _offTextForeColor != SystemColors.ControlText;
        }

        /// <summary>
        /// OffTextForeColor プロパティをデフォルト値にリセットします。
        /// </summary>
        private void ResetOffTextForeColor()
        {
            OffTextForeColor = SystemColors.ControlText;
        }

        private Color _onSwitchBorderColor = Color.FromArgb(0, 120, 212);

        /// <summary>
        /// トグルスイッチのON状態のボーダー色を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof(Color), "0, 120, 212")]
        [MetCategory("MetAppearance")]
        [MetDescription("MetToggleSwitchOnSwitchBorderColor")]
        public Color OnSwitchBorderColor
        {
            get { return _onSwitchBorderColor; }
            set
            {
                if (_onSwitchBorderColor != value)
                {
                    _onSwitchBorderColor = value;
                    Invalidate();
                }
            }
        }

        private Color _offSwitchBorderColor = Color.FromArgb(118, 118, 118);

        /// <summary>
        /// トグルスイッチのOFF状態のボーダー色を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof(Color), "118, 118, 118")]
        [MetCategory("MetAppearance")]
        [MetDescription("MetToggleSwitchOffSwitchBorderColor")]
        public Color OffSwitchBorderColor
        {
            get { return _offSwitchBorderColor; }
            set
            {
                if (_offSwitchBorderColor != value)
                {
                    _offSwitchBorderColor = value;
                    Invalidate();
                }
            }
        }

        private Color _onBackColor = Color.FromArgb(0, 120, 212);

        /// <summary>
        /// トグルスイッチのON状態の背景色を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof(Color), "0, 120, 212")]
        [MetCategory("MetAppearance")]
        [MetDescription("MetToggleSwitchOnBackColor")]
        public Color OnBackColor
        {
            get { return _onBackColor; }
            set
            {
                if (_onBackColor != value)
                {
                    _onBackColor = value;
                    Invalidate();
                }
            }
        }

        private Color _onDisabledBackColor = Color.FromArgb(191, 191, 191);

        /// <summary>
        /// トグルスイッチが無効でON状態の背景色を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof(Color), "191, 191, 191")]
        [MetCategory("MetAppearance")]
        [MetDescription("MetToggleSwitchOnDisabledBackColor")]
        public Color OnDisabledBackColor
        {
            get { return _onDisabledBackColor; }
            set
            {
                if (_onDisabledBackColor != value)
                {
                    _onDisabledBackColor = value;
                    Invalidate();
                }
            }
        }

        private Color _offBackColor = Color.FromArgb(255, 255, 255);

        /// <summary>
        /// トグルスイッチのOFF状態の背景色を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof(Color), "255, 255, 255")]
        [MetCategory("MetAppearance")]
        [MetDescription("MetToggleSwitchOffBackColor")]
        public Color OffBackColor
        {
            get { return _offBackColor; }
            set
            {
                if (_offBackColor != value)
                {
                    _offBackColor = value;
                    Invalidate();
                }
            }
        }

        private Color _offDisabledBackColor = Color.FromArgb(243, 243, 243);

        /// <summary>
        /// トグルスイッチが無効でOFF状態の背景色を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof(Color), "243, 243, 243")]
        [MetCategory("MetAppearance")]
        [MetDescription("MetToggleSwitchOffDisabledBackColor")]
        public Color OffDisabledBackColor
        {
            get { return _offDisabledBackColor; }
            set
            {
                if (_offDisabledBackColor != value)
                {
                    _offDisabledBackColor = value;
                    Invalidate();
                }
            }
        }

        // Empty の場合は自動色
        private Color _onThumbColor = Color.Empty;

        /// <summary>
        /// トグルスイッチのON状態のサムの色を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof(Color), "Empty")]
        [MetCategory("MetAppearance")]
        [MetDescription("MetToggleSwitchOnThumbColor")]
        public Color OnThumbColor
        {
            get { return _onThumbColor; }
            set
            {
                if (_onThumbColor != value)
                {
                    _onThumbColor = value;
                    Invalidate();
                }
            }
        }

        private Color _onDisabledThumbColor = Color.FromArgb(255, 255, 255);

        /// <summary>
        /// トグルスイッチが無効でON状態のサムの色を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof(Color), "255, 255,255")]
        [MetCategory("MetAppearance")]
        [MetDescription("MetToggleSwitchOnDisabledThumbColor")]
        public Color OnDisabledThumbColor
        {
            get { return _onDisabledThumbColor; }
            set
            {
                if (_onDisabledThumbColor != value)
                {
                    _onDisabledThumbColor = value;
                    Invalidate();
                }
            }
        }

        // Empty の場合は自動色
        private Color _offThumbColor = Color.Empty;

        /// <summary>
        /// トグルスイッチのOFF状態のサムの色を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof(Color), "Empty")]
        [MetCategory("MetAppearance")]
        [MetDescription("MetToggleSwitchOffThumbColor")]
        public Color OffThumbColor
        {
            get { return _offThumbColor; }
            set
            {
                if (_offThumbColor != value)
                {
                    _offThumbColor = value;
                    Invalidate();
                }
            }
        }

        private Color _offDisabledThumbColor = Color.FromArgb(155, 155, 155);

        /// <summary>
        /// トグルスイッチが無効でOFF状態のサムの色を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof(Color), "155, 155, 155")]
        [MetCategory("MetAppearance")]
        [MetDescription("MetToggleSwitchOffDisabledThumbColor")]
        public Color OffDisabledThumbColor
        {
            get { return _offDisabledThumbColor; }
            set
            {
                if (_offDisabledThumbColor != value)
                {
                    _offDisabledThumbColor = value;
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
                    if (_autoSize)
                    {
                        UpdateControlSize();
                    }
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
        /// アニメーションタイマーのティックイベントハンドラーです。
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
        /// イージング関数：EaseOutCubic
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private float EaseOutCubic(float t)
        {
            return 1f - (float)Math.Pow(1 - t, 3);
        }

        /// <summary>
        /// コントロールのサイズを自動的に更新します。
        /// </summary>
        private void UpdateControlSize()
        {
            if (!_autoSize)
            {
                return; // AutoSize = false の場合はサイズ変更しない
            }

            int baseHeight = SiwtchHeight;

            if (!_showState)
            {
                Size = new Size(SwitchWidth, baseHeight);
                return;
            }

            // テキストサイズを測定
            using (var g = CreateGraphics())
            {
                var textFont = Font;
                var onTextSize = g.MeasureString(_onText, textFont);
                var offTextSize = g.MeasureString(_offText, textFont);
                var maxTextWidth = Math.Max(onTextSize.Width, offTextSize.Width);
                var maxTextHeight = Math.Max(onTextSize.Height, offTextSize.Height);

                if (_statePosition == ToggleSwitchStatePosition.Inside)
                {
                    // Inner表示の場合：OnText, OffTextの文字長を考慮した幅を計算
                    // サム円のサイズ（高さ-6px）とテキスト幅とマージンを考慮
                    var thumbSize = baseHeight - 6;
                    var requiredWidth = thumbSize + 6 + (int)Math.Ceiling(maxTextWidth) + 10; // 余白10px
                    var totalWidth = Math.Max(SwitchWidth, requiredWidth);
                    var totalHeight = Math.Max(baseHeight, (int)Math.Ceiling(maxTextHeight));
                    Size = new Size(totalWidth, totalHeight);
                }
                else
                {
                    // Left/Right表示の場合：従来通り
                    var totalWidth = SwitchWidth + (int)Math.Ceiling(maxTextWidth) + 8; // 8px間隔
                    var totalHeight = Math.Max(baseHeight, (int)Math.Ceiling(maxTextHeight));
                    Size = new Size(totalWidth, totalHeight);
                }
            }
        }

        /// <summary>
        /// コントロールのサイズが変更されたときに呼び出されます。
        /// </summary>
        private void UpdateSwitchSizeForCustomSize()
        {
            if (!_showState || _statePosition == ToggleSwitchStatePosition.Inside || !_autoSize)
            {
                return; // テキスト表示なし、内部表示、またはAutoSize = false の場合はスイッチサイズ固定
            }

            // 現在のコントロールサイズからテキスト幅を逆算してスイッチサイズを調整
            using (var g = CreateGraphics())
            {
                var textFont = Font;
                var onTextSize = g.MeasureString(_onText, textFont);
                var offTextSize = g.MeasureString(_offText, textFont);
                var maxTextWidth = Math.Max(onTextSize.Width, offTextSize.Width);

                var availableWidthForSwitch = Width - (int)Math.Ceiling(maxTextWidth) - 8;

                if (availableWidthForSwitch > 20) // 最小スイッチ幅を確保
                {
                    // 実際のスイッチ幅を動的に設定（ただし、この実装では比率調整のみ）
                    // より高度な実装では、SWITCH_WIDTH を動的に変更することも可能
                }
            }
        }

        /// <summary>
        /// スイッチの矩形を取得します。
        /// </summary>
        /// <returns>スイッチの矩形。</returns>
        private Rectangle GetSwitchRectangle()
        {
            int switchWidth = SwitchWidth;
            int switchHeight = SiwtchHeight;

            // カスタムサイズの場合は、コントロールサイズに基づいてスイッチサイズを調整
            if (!_showState || _statePosition == ToggleSwitchStatePosition.Inside)
            {
                // テキスト表示なしの場合は、コントロール全体をスイッチとして使用
                switchWidth = Math.Max(20, Width - 1);
                switchHeight = Math.Max(10, Height - 1);
                return new Rectangle(0, 0, switchWidth, switchHeight);
            }

            // テキスト表示ありの場合の動的スイッチサイズ計算
            using (var g = CreateGraphics())
            {
                var textFont = Font;
                var onTextSize = g.MeasureString(_onText, textFont);
                var offTextSize = g.MeasureString(_offText, textFont);
                var maxTextWidth = Math.Max(onTextSize.Width, offTextSize.Width);

                var availableWidthForSwitch = Width - (int)Math.Ceiling(maxTextWidth) - 8;
                if (availableWidthForSwitch > 20)
                {
                    switchWidth = Math.Max(20, Math.Min(availableWidthForSwitch, Width - 8));
                }
                switchHeight = Math.Max(10, Height - 1);
            }

            // ShowState = true かつ StatePosition != Inner のとき、スイッチ幅を1ピクセル縮小
            if (_showState && _statePosition != ToggleSwitchStatePosition.Inside)
            {
                switchWidth = Math.Max(20, switchWidth - 1);
            }

            var switchY = (Height - switchHeight) / 2;

            if (_statePosition == ToggleSwitchStatePosition.Left)
            {
                using (var g = CreateGraphics())
                {
                    var textFont = Font;
                    var onTextSize = g.MeasureString(_onText, textFont);
                    var offTextSize = g.MeasureString(_offText, textFont);
                    var maxTextWidth = Math.Max(onTextSize.Width, offTextSize.Width);
                    var textWidth = (int)Math.Ceiling(maxTextWidth);
                    return new Rectangle(textWidth + 8, switchY, switchWidth, switchHeight);
                }
            }
            else // Right
            {
                return new Rectangle(0, switchY, switchWidth, switchHeight);
            }
        }

        /// <summary>
        /// テキストの描画領域を取得します。
        /// </summary>
        /// <returns>テキストの描画領域。</returns>
        private Rectangle GetTextRectangle()
        {
            if (!_showState || _statePosition == ToggleSwitchStatePosition.Inside)
            {
                return Rectangle.Empty;
            }

            var switchRect = GetSwitchRectangle();
            var textFont = Font;
            var textY = (Height - (int)Math.Ceiling(textFont.GetHeight())) / 2;

            if (_statePosition == ToggleSwitchStatePosition.Left)
            {
                return new Rectangle(0, textY, switchRect.X - 8, (int)Math.Ceiling(textFont.GetHeight()));
            }
            else // Right
            {
                return new Rectangle(switchRect.Right + 8, textY, Width - switchRect.Right - 8, (int)Math.Ceiling(textFont.GetHeight()));
            }
        }

        /// <summary>
        /// コントロールの描画を行います。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            var switchRect = GetSwitchRectangle();
            var rect = new RectangleF(switchRect.X, switchRect.Y, switchRect.Width, switchRect.Height);

            // 背景とボーダーの色を計算
            Color backgroundColor, borderColor, thumbColor;

            if (!Enabled)
            {
                if (Checked)
                {
                    backgroundColor = OnDisabledBackColor;
                    thumbColor = OnDisabledThumbColor;
                }
                else
                {
                    backgroundColor = OffDisabledBackColor;
                    thumbColor = OffDisabledThumbColor;
                }
                borderColor = DisabledBorderColor;
            }
            else
            {
                // カスタム色またはデフォルト色を使用
                var offBgColor = _offBackColor;
                var onBgColor = _onBackColor;

                // アニメーション中の色の補間
                backgroundColor = InterpolateColor(offBgColor, onBgColor, _animationProgress);
                borderColor = InterpolateColor(_offSwitchBorderColor, _onSwitchBorderColor, _animationProgress);

                // ThumbColor の設定
                if (_onThumbColor == Color.Empty || _offThumbColor == Color.Empty)
                {
                    // 自動色：OFF時は暗い色、ON時は白
                    var autoOffThumbColor = _offThumbColor == Color.Empty ? DefaultOffThumbColor : _offThumbColor;
                    var autoOnThumbColor = _onThumbColor == Color.Empty ? DefaultOnThumbColor : _onThumbColor;
                    thumbColor = InterpolateColor(autoOffThumbColor, autoOnThumbColor, _animationProgress);
                }
                else
                {
                    thumbColor = InterpolateColor(_offThumbColor, _onThumbColor, _animationProgress);
                }
            }

            // 角丸半径の計算
            var cornerRadius = _cornerRadius == -1 ? rect.Height / 2 : Math.Max(0, _cornerRadius);

            // 背景の描画
            using (var brush = new SolidBrush(backgroundColor))
            {
                g.FillRoundedRectangle(brush, rect, cornerRadius);
            }

            // ボーダーの描画
            using (var pen = new Pen(borderColor, BorderWidth))
            {
                var borderRect = new RectangleF(
                    rect.X + BorderWidth / 2f,
                    rect.Y + BorderWidth / 2f,
                    rect.Width - BorderWidth,
                    rect.Height - BorderWidth);
                var borderCornerRadius = Math.Max(0, cornerRadius - BorderWidth / 2f);
                g.DrawRoundedRectangle(pen, borderRect, borderCornerRadius);
            }

            // ホバー効果
            if (_isHovered && Enabled)
            {
                using (var brush = new SolidBrush(HoverOverlayColor))
                {
                    g.FillRoundedRectangle(brush, rect, cornerRadius);
                }
            }

            // サムの描画
            using (var brush = new SolidBrush(thumbColor))
            {
                // CornerRadius が 0 の場合は縦長の長方形、それ以外は円形
                if (_cornerRadius == 0)
                {
                    // 縦長の長方形のサムを描画（上下左右3pxの余白）
                    var thumbWidth = (switchRect.Height - 6) * 0.6f; // 高さの60%程度の幅
                    var thumbHeight = switchRect.Height - 6; // 上下3pxずつの余白
                    var thumbX = rect.X + (switchRect.Width - thumbWidth - 6) * _animationProgress + 3;
                    var thumbY = rect.Y + 3; // 上側3pxの余白
                    var thumbRect = new RectangleF(thumbX, thumbY, thumbWidth, thumbHeight);

                    g.FillRectangle(brush, thumbRect);
                }
                else
                {
                    float thumbSize = Math.Min(switchRect.Height - 6, switchRect.Width / 3);
                    var thumbX = rect.X + (switchRect.Width - thumbSize - 6) * _animationProgress + 3;
                    var thumbY = rect.Y + (rect.Height - thumbSize) / 2f;
                    var thumbRect = new RectangleF(thumbX, thumbY, thumbSize, thumbSize);

                    g.FillEllipse(brush, thumbRect);
                }
            }

            // テキストの描画
            if (_showState)
            {
                DrawStateText(g, switchRect);
            }

            base.OnPaint(e);
        }

        /// <summary>
        /// トグルスイッチの状態テキストを描画します。
        /// </summary>
        /// <param name="g">グラフィックス。</param>
        /// <param name="switchRect">サム円の描画サイズ。</param>
        private void DrawStateText(Graphics g, Rectangle switchRect)
        {
            var currentText = _checked ? _onText : _offText;
            if (string.IsNullOrEmpty(currentText)) return;

            var textFont = Font;
            Color textColor;

            if (!Enabled)
            {
                textColor = DisabledTextColor;
            }
            else if (_statePosition == ToggleSwitchStatePosition.Inside)
            {
                // Inner表示時もカスタム色を使用
                textColor = _checked ? _onTextForeColor : _offTextForeColor;
            }
            else
            {
                // カスタム色を使用
                textColor = _checked ? _onTextForeColor : _offTextForeColor;
            }

            using (var brush = new SolidBrush(textColor))
            {
                if (_statePosition == ToggleSwitchStatePosition.Inside)
                {
                    // スイッチ内部にテキストを描画（サム円を除いた領域で左右中央揃え）
                    var textSize = g.MeasureString(currentText, textFont);

                    // サム円のサイズと位置を計算
                    var thumbSize = switchRect.Height - 6;
                    var thumbX = (switchRect.Width - thumbSize - 6) * _animationProgress + 3;

                    // テキスト表示可能領域を計算
                    float availableStartX, availableEndX;
                    if (_checked)
                    {
                        // ON状態：サム円の左側の領域
                        availableStartX = switchRect.X + 3;
                        availableEndX = switchRect.X + thumbX - 2;
                    }
                    else
                    {
                        // OFF状態：サム円の右側の領域
                        availableStartX = switchRect.X + thumbX + thumbSize + 2;
                        availableEndX = switchRect.X + switchRect.Width - 3;
                    }

                    var availableWidth = availableEndX - availableStartX;

                    // テキストが表示可能領域に収まる場合のみ描画
                    if (textSize.Width <= availableWidth && availableWidth > 10)
                    {
                        var textX = availableStartX + (availableWidth - textSize.Width) / 2;
                        var textY = switchRect.Y + (switchRect.Height - textSize.Height) / 2;

                        g.DrawString(currentText, textFont, brush, textX, textY);
                    }
                }
                else
                {
                    // 外部のテキスト領域に描画
                    var textRect = GetTextRectangle();
                    if (!textRect.IsEmpty)
                    {
                        var format = new StringFormat();
                        format.Alignment = _statePosition == ToggleSwitchStatePosition.Left ? StringAlignment.Far : StringAlignment.Near;
                        format.LineAlignment = StringAlignment.Center;

                        g.DrawString(currentText, textFont, brush, textRect, format);
                    }
                }
            }
        }

        /// <summary>
        /// 2つの色を指定された比率で補間します。
        /// </summary>
        /// <param name="color1"></param>
        /// <param name="color2"></param>
        /// <param name="ratio"></param>
        /// <returns></returns>
        private Color InterpolateColor(Color color1, Color color2, float ratio)
        {
            ratio = Math.Max(0, Math.Min(1, ratio));

            int r = (int)(color1.R + (color2.R - color1.R) * ratio);
            int g = (int)(color1.G + (color2.G - color1.G) * ratio);
            int b = (int)(color1.B + (color2.B - color1.B) * ratio);

            return Color.FromArgb(r, g, b);
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
            if (Enabled)
            {
                var switchRect = GetSwitchRectangle();
                var mousePos = PointToClient(Cursor.Position);

                // スイッチ部分またはテキスト部分のクリックで状態を切り替え
                if (switchRect.Contains(mousePos) ||
                    (_showState && _statePosition != ToggleSwitchStatePosition.Inside && GetTextRectangle().Contains(mousePos)))
                {
                    Checked = !Checked;
                }
            }
            base.OnClick(e);
        }

        /// <summary>
        /// マウスがコントロールに入ったときの処理を行います。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseEnter(EventArgs e)
        {
            _isHovered = true;
            Invalidate();
            base.OnMouseEnter(e);
        }

        /// <summary>
        /// マウスがコントロールから離れたときの処理を行います。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeave(EventArgs e)
        {
            _isHovered = false;
            Invalidate();
            base.OnMouseLeave(e);
        }

        /// <summary>
        /// マウスダウンイベントを処理します。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            Invalidate();
            base.OnMouseDown(e);
        }

        /// <summary>
        /// マウスアップイベントを処理します。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            Invalidate();
            base.OnMouseUp(e);
        }

        /// <summary>
        /// コントロールの有効状態が変更されたときの処理を行います。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnEnabledChanged(EventArgs e)
        {
            Invalidate();
            base.OnEnabledChanged(e);
        }

        /// <summary>
        /// コントロールのサイズが変更されたときの処理を行います。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSizeChanged(EventArgs e)
        {
            if (_showState && _statePosition != ToggleSwitchStatePosition.Inside && _autoSize)
            {
                UpdateSwitchSizeForCustomSize();
            }
            Invalidate();
            base.OnSizeChanged(e);
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
        public void Rollback()
        {
            Rollbacking = true;
            Checked = _enterChecked;
            Rollbacking = false;
        }
    }
}