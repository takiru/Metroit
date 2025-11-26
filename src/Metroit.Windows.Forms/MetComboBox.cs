using Metroit.Win32.Api;
using Metroit.Windows.Forms.Extensions;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// Windows コンボ ボックス コントロールを表します。
    /// </summary>
    [ToolboxItem(true)]
    public class MetComboBox : ComboBox, IControlRollback, IBorder
    {
        private Color _baseBackColor = SystemColors.Window;
        private Color _baseForeColor = SystemColors.WindowText;
        private Color _baseBorderColor = Color.Transparent;
        private Color _focusBackColor = SystemColors.Window;
        private Color _focusForeColor = SystemColors.WindowText;
        private Color _focusBorderColor = Color.Transparent;
        private Color _errorBorderColor = Color.Red;

        private bool _hasFocus = false;
        private bool _isMouseOverButton = false;
        private bool _isDroppedDown = false;

        private bool _readOnly = false;
        private bool _readOnlyLabel = false;
        private bool _visible = true;
        private bool _error = false;

        private MetTextBox _metTextBox = null;
        private Label _label = null;

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        public MetComboBox()
        {
            // DrawModeはNormalのまま（標準のサイズを維持）
            this.DrawMode = DrawMode.Normal;

            // 初期状態の色を設定
            UpdateColors();
        }

        #region プロパティ

        /// <summary>
        /// TextBox による代替表示を行っているかどうかを取得します。
        /// </summary>
        [Browsable(true)]
        [MetCategory("MetBehavior")]
        [DefaultValue(false)]
        [MetDescription("ControlReadOnly")]
        public bool ReadOnly
        {
            get => _readOnly;
            set
            {
                if (_readOnly != value)
                {
                    _readOnly = value;
                    // ReadOnlyをtrueにする場合、ReadOnlyLabelをfalseにする
                    if (_readOnly && _readOnlyLabel)
                    {
                        _readOnlyLabel = false;
                    }
                    UpdateDisplayMode();
                }
            }
        }

        /// <summary>
        /// Label による代替表示を行っているかどうかを取得します。
        /// </summary>
        [Browsable(true)]
        [MetCategory("MetBehavior")]
        [DefaultValue(false)]
        [MetDescription("ControlReadOnlyLabel")]
        public bool ReadOnlyLabel
        {
            get => _readOnlyLabel;
            set
            {
                if (_readOnlyLabel != value)
                {
                    _readOnlyLabel = value;
                    // ReadOnlyLabelをtrueにする場合、ReadOnlyをfalseにする
                    if (_readOnlyLabel && _readOnly)
                    {
                        _readOnly = false;
                    }
                    UpdateDisplayMode();
                }
            }
        }

        /// <summary>
        /// エラー時のコントロールの枠色を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof(Color), "Red")]
        [MetCategory("MetAppearance")]
        [MetDescription("ControlErrorBorderColor")]
        public Color ErrorBorderColor
        {
            get => _errorBorderColor;
            set
            {
                _errorBorderColor = value;
                if (_metTextBox != null)
                {
                    _metTextBox.ErrorBorderColor = value;
                }
                this.Invalidate();
                CustomPaintBorder();
            }
        }

        /// <summary>
        /// コントロールがエラーかどうかを取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(false)]
        [MetCategory("MetAppearance")]
        [MetDescription("ControlError")]
        public bool Error
        {
            get => _error;
            set
            {
                if (_error != value)
                {
                    _error = value;
                    if (_metTextBox != null)
                    {
                        _metTextBox.Error = value;
                    }
                    this.Invalidate();
                    CustomPaintBorder();
                }
            }
        }

        /// <summary>
        /// コントロールの背景色を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [MetCategory("MetAppearance")]
        [DefaultValue(typeof(Color), "Window")]
        [MetDescription("ControlBaseBackColor")]
        public Color BaseBackColor
        {
            get => _baseBackColor;
            set
            {
                _baseBackColor = value;
                UpdateColors();
                if (_metTextBox != null)
                {
                    _metTextBox.BaseBackColor = value;
                }
                this.Invalidate();
            }
        }

        /// <summary>
        /// コントロールの文字色を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof(Color), "WindowText")]
        [MetCategory("MetAppearance")]
        [MetDescription("ControlBaseForeColor")]
        public Color BaseForeColor
        {
            get => _baseForeColor;
            set
            {
                _baseForeColor = value;
                UpdateColors();
                if (_metTextBox != null)
                {
                    _metTextBox.BaseForeColor = value;
                }
                this.Invalidate();
            }
        }

        /// <summary>
        /// コントロールの枠色を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof(Color), "Transparent")]
        [MetCategory("MetAppearance")]
        [MetDescription("ControlBaseBorderColor")]
        public Color BaseBorderColor
        {
            get => _baseBorderColor;
            set
            {
                _baseBorderColor = value;
                if (_metTextBox != null)
                {
                    _metTextBox.BaseBorderColor = value;
                    _metTextBox.BorderStyle = value == Color.Transparent ? BorderStyle.Fixed3D : BorderStyle.FixedSingle;
                }
                if (_label != null)
                {
                    _label.BorderStyle = value == Color.Transparent ? BorderStyle.Fixed3D : BorderStyle.FixedSingle;
                }
                this.Invalidate();
                CustomPaintBorder();
            }
        }

        /// <summary>
        /// フォーカス時のコントロールの背景色を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof(Color), "Window")]
        [MetCategory("MetAppearance")]
        [MetDescription("ControlFocusBackColor")]
        public Color FocusBackColor
        {
            get => _focusBackColor;
            set
            {
                _focusBackColor = value;
                UpdateColors();
                if (_metTextBox != null)
                {
                    _metTextBox.FocusBackColor = value;
                }
                this.Invalidate();
            }
        }

        /// <summary>
        /// フォーカス時のコントロールの文字色を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof(Color), "WindowText")]
        [MetCategory("MetAppearance")]
        [MetDescription("ControlFocusForeColor")]
        public Color FocusForeColor
        {
            get => _focusForeColor;
            set
            {
                _focusForeColor = value;
                UpdateColors();
                if (_metTextBox != null)
                {
                    _metTextBox.FocusForeColor = value;
                }
                this.Invalidate();
            }
        }

        /// <summary>
        /// フォーカス時のコントロールの枠色を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof(Color), "Transparent")]
        [MetCategory("MetAppearance")]
        [MetDescription("ControlFocusBorderColor")]
        public Color FocusBorderColor
        {
            get => _focusBorderColor;
            set
            {
                _focusBorderColor = value;
                if (_metTextBox != null)
                {
                    _metTextBox.FocusBorderColor = value;
                }
                this.Invalidate();
                CustomPaintBorder();
            }
        }

        /// <summary>
        /// コントロールの背景色を取得または特定のプロパティによって強制的に背景色を設定します。
        /// 意図的に背景色の変更が必要な場合のみ利用してください。
        /// </summary>
        [Browsable(false)]
        [ReadOnly(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Color BackColor
        {
            get => base.BackColor;
            set => base.BackColor = value;
        }

        /// <summary>
        /// コントロールの前景色を取得または特定のプロパティによって強制的に前景色を設定します。
        /// 意図的に前景色の変更が必要な場合のみ利用してください。
        /// </summary>
        [Browsable(false)]
        [ReadOnly(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Color ForeColor
        {
            get => base.ForeColor;
            set => base.ForeColor = value;
        }

        /// <summary>
        /// コントロールとそのすべての子コントロールが表示されているかどうかを示す値を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(true)]
        public new bool Visible
        {
            get => _visible;
            set
            {
                if (_visible != value)
                {
                    _visible = value;
                    UpdateVisibleState();
                }
            }
        }

        #endregion

        #region イベントハンドラ

        /// <summary>
        /// ハンドル作成時に表示モードを更新する。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            UpdateDisplayMode();
        }

        private object enterSelectedItem = null;

        /// <summary>
        /// ロールバック済みかどうかを取得します。
        /// </summary>
        /// <returns>true:ロールバック済み, false:未ロールバック。</returns>
        [Browsable(false)]
        public bool IsRollbacked => this.SelectedItem == this.enterSelectedItem;

        /// <summary>
        /// フォーカスを得た時の値にロールバックを行います。
        /// </summary>
        public virtual void Rollback()
        {
            this.SelectedItem = this.enterSelectedItem;
        }

        /// <summary>
        /// フォーカスを得た時に色を変更する。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            _hasFocus = true;
            if (!IsValidatingCanceled)
            {
                enterSelectedItem = this.SelectedItem;
            }
            UpdateColors();
            // Invalidateではなく直接CustomPaintを呼ぶ
            CustomPaint();
            CustomPaintBorder();
        }

        /// <summary>
        /// フォーカスを失った時に色を変更する。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            _hasFocus = false;
            UpdateColors();
            // Invalidateではなく直接CustomPaintを呼ぶ
            CustomPaint();
            CustomPaintBorder();
        }

        /// <summary>
        /// フォーカスを得た時に色を変更する。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            if (!_hasFocus)
            {
                _hasFocus = true;
                UpdateColors();
                CustomPaint();
                CustomPaintBorder();
            }
        }

        /// <summary>
        /// フォーカスを失った時に色を変更する。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            if (_hasFocus)
            {
                _hasFocus = false;
                UpdateColors();
                CustomPaint();
                CustomPaintBorder();
            }
        }

        /// <summary>
        /// マウス移動時にドロップダウンボタン上かどうかを判定し、必要に応じて再描画を行う。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            // ドロップダウンが開いている場合は何もしない
            if (_isDroppedDown)
            {
                return;
            }

            // ドロップダウンボタンの領域を計算
            int buttonWidth = SystemInformation.VerticalScrollBarWidth;
            Rectangle buttonRect = new Rectangle(
                this.Width - buttonWidth,
                0,
                buttonWidth,
                this.Height
            );

            bool wasOverButton = _isMouseOverButton;
            _isMouseOverButton = buttonRect.Contains(e.Location);

            // マウスがボタン領域に入った、または出た場合に再描画
            if (wasOverButton != _isMouseOverButton)
            {
                CustomPaintBorder();
            }
        }

        /// <summary>
        /// マウスがコントロールから離れたときにドロップダウンボタン上かどうかを判定し、必要に応じて再描画を行う。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            if (_isMouseOverButton)
            {
                _isMouseOverButton = false;

                // ドロップダウンが開いていない場合のみ再描画
                if (!_isDroppedDown)
                {
                    CustomPaintBorder();
                }
            }
        }

        /// <summary>
        /// ドロップダウンが開いたときにボーダーを再描画する。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDropDown(EventArgs e)
        {
            base.OnDropDown(e);
            _isDroppedDown = true;
            _isMouseOverButton = false;
            this.Invalidate();
            CustomPaintBorder();
        }

        /// <summary>
        /// ドロップダウンが閉じたときにボーダーを再描画する。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDropDownClosed(EventArgs e)
        {
            base.OnDropDownClosed(e);
            _isDroppedDown = false;
            this.Invalidate();
            CustomPaintBorder();
        }

        /// <summary>
        /// 選択項目が変更されたときに代替コントロールのテキストを更新する。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);

            // 代替コントロールのテキストも更新
            UpdateAlternativeControlText();

            this.Invalidate();
        }

        /// <summary>
        /// テキストが変更されたときに代替コントロールのテキストを更新する。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);

            // 代替コントロールのテキストも更新
            UpdateAlternativeControlText();
        }

        /// <summary>
        /// Paddingが変更されたときに代替コントロールに反映する。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaddingChanged(EventArgs e)
        {
            base.OnPaddingChanged(e);
            if (_metTextBox != null) _metTextBox.Padding = this.DefaultPadding;
            if (_label != null) _label.Padding = this.DefaultPadding;
        }

        /// <summary>
        /// サイズが変更されたときに代替コントロールに反映する。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (_metTextBox != null) _metTextBox.Size = this.Size;
            if (_label != null) _label.Size = this.Size;
        }

        /// <summary>
        /// 位置が変更されたときに代替コントロールに反映する。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLocationChanged(EventArgs e)
        {
            base.OnLocationChanged(e);
            if (_metTextBox != null) _metTextBox.Location = this.Location;
            if (_label != null) _label.Location = this.Location;
        }

        /// <summary>
        /// マージンが変更されたときに代替コントロールに反映する。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMarginChanged(EventArgs e)
        {
            base.OnMarginChanged(e);
            if (_metTextBox != null) _metTextBox.Margin = this.Margin;
            if (_label != null) _label.Margin = this.Margin;
        }

        /// <summary>
        /// Enabled状態が変更されたときに代替コントロールに反映する。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            if (_metTextBox != null) _metTextBox.Enabled = this.Enabled;
            if (_label != null) _label.Enabled = this.Enabled;
        }

        /// <summary>
        /// Dock状態が変更されたときに代替コントロールに反映する。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDockChanged(EventArgs e)
        {
            base.OnDockChanged(e);
            if (_metTextBox != null) _metTextBox.Dock = this.Dock;
            if (_label != null) _label.Dock = this.Dock;
        }

        /// <summary>
        /// Font状態が変更されたときに代替コントロールに反映する。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            if (_metTextBox != null) _metTextBox.Font = this.Font;
            if (_label != null) _label.Font = this.Font;
        }

        /// <summary>
        /// RightToLeft状態が変更されたときに代替コントロールに反映する。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnRightToLeftChanged(EventArgs e)
        {
            base.OnRightToLeftChanged(e);
            if (_metTextBox != null) _metTextBox.RightToLeft = this.RightToLeft;
            if (_label != null) _label.RightToLeft = this.RightToLeft;
        }

        /// <summary>
        /// Cursor状態が変更されたときに代替コントロールに反映する。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnCursorChanged(EventArgs e)
        {
            base.OnCursorChanged(e);
            if (_metTextBox != null) _metTextBox.Cursor = this.Cursor;
            if (_label != null) _label.Cursor = this.Cursor;
        }

        /// <summary>
        /// TabIndex状態が変更されたときに代替コントロールに反映する。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnTabIndexChanged(EventArgs e)
        {
            base.OnTabIndexChanged(e);
            if (_metTextBox != null) _metTextBox.TabIndex = this.TabIndex;
            if (_label != null) _label.TabIndex = this.TabIndex;
        }

        /// <summary>
        /// TabStop状態が変更されたときに代替コントロールに反映する。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnTabStopChanged(EventArgs e)
        {
            base.OnTabStopChanged(e);
            if (_metTextBox != null) _metTextBox.TabStop = this.TabStop;
        }

        /// <summary>
        /// 親コントロールが変更されたときに代替コントロールに反映する。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);

            if (_metTextBox != null)
            {
                _metTextBox.Parent = Parent;
            }
            if (_label != null)
            {
                _label.Parent = Parent;
            }
        }

        /// <summary>
        /// WndProcオーバーライドによるカスタム描画処理。
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == WindowMessage.WM_PAINT)
            {
                // 常にカスタム描画を実行（ドロップダウンが開いている場合も含む）
                CustomPaint();
                CustomPaintBorder();
            }
            else if (m.Msg == WindowMessage.WM_NCPAINT)
            {
                CustomPaintBorder();
            }
            else if (m.Msg == WindowMessage.WM_SETFOCUS)
            {
                // フォーカスを取得したとき
                _hasFocus = true;
                UpdateColors();
                CustomPaint();
                CustomPaintBorder();
            }
            else if (m.Msg == WindowMessage.WM_KILLFOCUS)
            {
                // フォーカスを失ったとき
                _hasFocus = false;
                UpdateColors();
                CustomPaint();
                CustomPaintBorder();
            }
        }

        #endregion

        #region 描画メソッド

        /// <summary>
        /// フォーカス状態に応じてBackColorとForeColorを更新
        /// </summary>
        private void UpdateColors()
        {
            Color newBackColor = _hasFocus ? _focusBackColor : _baseBackColor;
            Color newForeColor = _hasFocus ? _focusForeColor : _baseForeColor;

            // 色が変わる場合のみ更新（不要な再描画を防ぐ）
            if (base.BackColor != newBackColor)
            {
                base.BackColor = newBackColor;
            }
            if (base.ForeColor != newForeColor)
            {
                base.ForeColor = newForeColor;
            }
        }

        /// <summary>
        /// カスタム描画処理（テキスト領域のみ）
        /// </summary>
        private void CustomPaint()
        {
            using (Graphics g = this.CreateGraphics())
            {
                // テキストレンダリングヒントを標準に設定（アンチエイリアスなし）
                g.TextRenderingHint = TextRenderingHint.SystemDefault;

                // 現在の色を取得
                Color backColor = _hasFocus ? _focusBackColor : _baseBackColor;
                Color foreColor = _hasFocus ? _focusForeColor : _baseForeColor;

                // ドロップダウンボタンの幅を取得
                int buttonWidth = SystemInformation.VerticalScrollBarWidth;

                // テキスト表示領域（ボタン部分を除く）
                // ドロップダウンが開いている場合は、外枠まで塗りつぶす
                Rectangle textRect;
                Rectangle textDrawRect;

                if (_isDroppedDown)
                {
                    textRect = new Rectangle(
                        0,
                        0,
                        this.Width - buttonWidth - 1,
                        this.Height
                    );

                    // ドロップダウンが開いているときも標準のComboBoxと同じ位置
                    textDrawRect = new Rectangle(
                        textRect.X + 1,
                        textRect.Y,
                        textRect.Width - 2,
                        textRect.Height
                    );
                }
                else
                {
                    textRect = new Rectangle(
                        1,
                        1,
                        this.Width - buttonWidth - 2,
                        this.Height - 2
                    );

                    // ドロップダウンが閉じているときも標準のComboBoxと同じ位置
                    textDrawRect = new Rectangle(
                        textRect.X,
                        textRect.Y,
                        textRect.Width - 1,
                        textRect.Height
                    );
                }

                // 背景を塗りつぶし（テキスト領域のみ）
                using (SolidBrush backBrush = new SolidBrush(backColor))
                {
                    g.FillRectangle(backBrush, textRect);
                }

                // テキストを描画（標準のComboBoxと同じ位置に調整）
                if (this.SelectedIndex >= 0 || !string.IsNullOrEmpty(this.Text))
                {
                    string displayText = this.SelectedIndex >= 0
                        ? this.GetItemText(this.Items[this.SelectedIndex])
                        : this.Text;

                    // TextRendererを使用して標準のComboBoxと同じ描画を行う
                    TextFormatFlags flags = TextFormatFlags.Left |
                                           TextFormatFlags.VerticalCenter |
                                           TextFormatFlags.SingleLine |
                                           TextFormatFlags.EndEllipsis |
                                           TextFormatFlags.NoPrefix;

                    TextRenderer.DrawText(g, displayText, this.Font, textDrawRect, foreColor, flags);
                }
            }
        }

        /// <summary>
        /// 外枠のカスタム描画（DropDownStyle, FlatStyleに関係なく常に描画）
        /// </summary>
        private void CustomPaintBorder()
        {
            IntPtr hdc = User32.GetWindowDC(this.Handle);

            if (hdc != IntPtr.Zero)
            {
                try
                {
                    using (Graphics g = Graphics.FromHdc(hdc))
                    {
                        // Error状態の場合はErrorBorderColorを優先
                        Color borderColor;
                        if (_error)
                        {
                            borderColor = _errorBorderColor;
                        }
                        else
                        {
                            borderColor = _hasFocus ? _focusBorderColor : _baseBorderColor;
                        }

                        int buttonWidth = SystemInformation.VerticalScrollBarWidth;

                        using (Pen pen = new Pen(borderColor, 1))
                        {
                            if (_isMouseOverButton || _isDroppedDown)
                            {
                                // マウスがボタン上にある場合、またはドロップダウンが開いている場合
                                // ボタン領域を除いて外枠を描画
                                int buttonLeft = this.Width - buttonWidth - 1;

                                // 左辺
                                g.DrawLine(pen, 0, 0, 0, this.Height - 1);
                                // 上辺（ボタン領域の手前まで）
                                g.DrawLine(pen, 0, 0, buttonLeft, 0);
                                // 下辺（ボタン領域の手前まで）
                                g.DrawLine(pen, 0, this.Height - 1, buttonLeft, this.Height - 1);
                                // テキスト領域とボタン領域の境界線
                                g.DrawLine(pen, buttonLeft, 0, buttonLeft, this.Height - 1);
                            }
                            else
                            {
                                // マウスがボタン上にない場合、かつドロップダウンが閉じている場合
                                // 全体の外枠を描画
                                Rectangle borderRect = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
                                g.DrawRectangle(pen, borderRect);
                            }
                        }
                    }
                }
                finally
                {
                    User32.ReleaseDC(this.Handle, hdc);
                }
            }
        }

        #endregion

        #region ReadOnly/ReadOnlyLabel処理

        /// <summary>
        /// 表示モードを更新する。
        /// </summary>
        private void UpdateDisplayMode()
        {
            // デザインモード中は代替表現を行わない
            if (this.IsDesignMode())
            {
                // 既存の代替コントロールを削除
                RemoveAlternativeControls();
                base.Visible = _visible;
                return;
            }

            // ReadOnlyLabelが優先
            if (_readOnlyLabel)
            {
                ShowAsLabel();
            }
            else if (_readOnly)
            {
                ShowAsMetTextBox();
            }
            else
            {
                ShowAsComboBox();
            }
        }

        /// <summary>
        /// Visible状態を更新する。
        /// </summary>
        private void UpdateVisibleState()
        {
            if (_metTextBox != null && _metTextBox.Parent != null)
            {
                _metTextBox.Visible = _visible;
            }
            else if (_label != null && _label.Parent != null)
            {
                _label.Visible = _visible;
            }
            else
            {
                base.Visible = _visible;
            }
        }

        /// <summary>
        /// ComboBoxとして表示する。
        /// </summary>
        private void ShowAsComboBox()
        {
            RemoveAlternativeControls();
            base.Visible = _visible;
        }

        /// <summary>
        /// MetTextBoxとして表示する。
        /// </summary>
        private void ShowAsMetTextBox()
        {
            if (_metTextBox == null && this.Parent != null)
            {
                _metTextBox = new MetTextBox();
                _metTextBox.BeginInit();
                _metTextBox.EndInit();

                // ComboBoxのプロパティを反映
                _metTextBox.ReadOnly = true;
                _metTextBox.Text = this.SelectedIndex >= 0 ? this.GetItemText(this.Items[this.SelectedIndex]) : this.Text;
                _metTextBox.Size = this.Size;
                _metTextBox.Location = this.Location;
                _metTextBox.Margin = this.Margin;
                _metTextBox.Padding = this.DefaultPadding;
                _metTextBox.Visible = _visible;
                _metTextBox.Enabled = this.Enabled;
                _metTextBox.Dock = this.Dock;
                _metTextBox.Font = this.Font;
                _metTextBox.Anchor = this.Anchor;
                _metTextBox.RightToLeft = this.RightToLeft;
                if (this.BaseBorderColor == Color.Empty)
                {
                    _metTextBox.BorderStyle = BorderStyle.None;
                }
                if (this.BaseBorderColor == Color.Transparent)
                {
                    _metTextBox.BorderStyle = BorderStyle.Fixed3D;
                }
                if (this.BaseBorderColor != Color.Empty && this.BaseBorderColor != Color.Transparent)
                {
                    _metTextBox.BorderStyle = BorderStyle.FixedSingle;
                }

                _metTextBox.Cursor = this.Cursor;
                _metTextBox.TextAlign = HorizontalAlignment.Left;
                _metTextBox.TabIndex = this.TabIndex;
                _metTextBox.TabStop = this.TabStop;
                _metTextBox.BaseBackColor = this.BaseBackColor;
                _metTextBox.BaseForeColor = this.BaseForeColor;
                _metTextBox.FocusBackColor = this.FocusBackColor;
                _metTextBox.FocusForeColor = this.FocusForeColor;
                _metTextBox.BaseBorderColor = this.BaseBorderColor;
                _metTextBox.FocusBorderColor = this.FocusBorderColor;
                _metTextBox.ErrorBorderColor = this.ErrorBorderColor;
                _metTextBox.Error = this.Error;

                this.Parent.Controls.Add(_metTextBox);
            }

            if (_label != null)
            {
                this.Parent.Controls.Remove(_label);
                _label.Dispose();
                _label = null;
            }

            base.Visible = false;
        }

        /// <summary>
        /// Labelとして表示する。
        /// </summary>
        private void ShowAsLabel()
        {
            if (_label == null && this.Parent != null)
            {
                _label = new Label();

                // ComboBoxのプロパティを反映
                _label.Text = this.SelectedIndex >= 0 ? this.GetItemText(this.Items[this.SelectedIndex]) : this.Text;
                _label.AutoSize = false;
                _label.Size = this.Size;
                _label.Location = this.Location;
                _label.Margin = this.Margin;
                _label.Padding = this.DefaultPadding;
                _label.Visible = _visible;
                _label.Enabled = this.Enabled;
                _label.Dock = this.Dock;
                _label.Font = this.Font;
                _label.Anchor = this.Anchor;
                _label.RightToLeft = this.RightToLeft;
                if (this.BaseBorderColor == Color.Empty)
                {
                    _label.BorderStyle = BorderStyle.None;
                }
                if (this.BaseBorderColor == Color.Transparent)
                {
                    _label.BorderStyle = BorderStyle.Fixed3D;
                }
                if (this.BaseBorderColor != Color.Empty && this.BaseBorderColor != Color.Transparent)
                {
                    _label.BorderStyle = BorderStyle.FixedSingle;
                }

                _label.Cursor = this.Cursor;
                _label.TextAlign = ContentAlignment.MiddleLeft;
                _label.BackColor = BaseBackColor;
                _label.ForeColor = BaseForeColor;

                this.Parent.Controls.Add(_label);
            }

            if (_metTextBox != null)
            {
                this.Parent.Controls.Remove(_metTextBox);
                _metTextBox.Dispose();
                _metTextBox = null;
            }

            base.Visible = false;
        }

        /// <summary>
        /// 代替コントロールを削除する。
        /// </summary>
        private void RemoveAlternativeControls()
        {
            if (_metTextBox != null && this.Parent != null)
            {
                this.Parent.Controls.Remove(_metTextBox);
                _metTextBox.Dispose();
                _metTextBox = null;
            }

            if (_label != null && this.Parent != null)
            {
                this.Parent.Controls.Remove(_label);
                _label.Dispose();
                _label = null;
            }
        }

        /// <summary>
        /// 代替コントロールのテキストを更新する。
        /// </summary>
        private void UpdateAlternativeControlText()
        {
            UpdateMetTextBoxText();
            UpdateLabelText();
        }

        /// <summary>
        /// MetTextBoxのテキストを更新する。
        /// </summary>
        private void UpdateMetTextBoxText()
        {
            if (_metTextBox != null)
            {
                if (this.SelectedIndex >= 0)
                {
                    _metTextBox.Text = this.GetItemText(this.Items[this.SelectedIndex]);
                }
                else
                {
                    _metTextBox.Text = this.Text;
                }
            }
        }

        /// <summary>
        /// Labelのテキストを更新する。
        /// </summary>
        private void UpdateLabelText()
        {
            if (_label != null)
            {
                if (this.SelectedIndex >= 0)
                {
                    _label.Text = this.GetItemText(this.Items[this.SelectedIndex]);
                }
                else
                {
                    _label.Text = this.Text;
                }
            }
        }

        #endregion

        /// <summary>
        /// リソースを解放します。
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                RemoveAlternativeControls();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// 値検証がキャンセルされたかどうかを取得します。
        /// </summary>
        protected bool IsValidatingCanceled { get; private set; } = false;

        /// <summary>
        /// 値検証を行っている時の動作。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);
            IsValidatingCanceled = e.Cancel;
        }
    }
}