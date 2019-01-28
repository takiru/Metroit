using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Metroit.Windows.Forms.Extensions;
using Metroit.Api.Win32;
using Metroit.Api.Win32.Structures;
using System.Runtime.InteropServices;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// ユーザーが日時を選択し、書式を指定して日時を表示できる Windows コントロールを表します。
    /// </summary>
    [ToolboxItem(true)]
    public class MetDateTimePicker : DateTimePicker, ISupportInitialize, IControlRollback, IBorder
    {
        /// <summary>
        /// MetDateTimePicker の新しいインスタンスを初期化します。
        /// </summary>
        public MetDateTimePicker()
            : base()
        {
            // デザイン時は制御しない
            if (ControlExtensions.IsDesignMode(this))
            {
                return;
            }

            this.KeyDown += MetDateTimePicker_KeyDown;
            this.Enter += MetDateTimePicker_Enter;
            this.Leave += MetDateTimePicker_Leave;
        }

        #region イベント

        private bool isNull = false;
        private DateTime? enterValue = null;

        /// <summary>
        /// Backspace, Deleteキー押下でnull表示にする。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MetDateTimePicker_KeyDown(object sender, KeyEventArgs e)
        {
            //Backspace, Deleteキーが押されたらnull表示
            if (this.AcceptNull && (e.KeyData == Keys.Delete || e.KeyData == Keys.Back) && !this.isNull)
            {
                this.Value = null;
                return;
            }

            // 数字キー、数字テンキー、十字キーが押された時だけnullから値を復帰する
            if (!(e.Alt || e.Control || e.Shift) &&
                    ((e.KeyValue >= 48 && e.KeyValue <= 57) ||
                    (e.KeyValue >= 96 && e.KeyValue <= 105) ||
                    (e.KeyValue >= 37 && e.KeyValue <= 40)))
            {
                if (!this.isNull)
                {
                    return;
                }

                // UIから操作した時、ミリ秒は取り除かれるのが通常仕様
                var recoverDate = new DateTime(base.Value.Year, base.Value.Month, base.Value.Day, base.Value.Hour, base.Value.Minute, base.Value.Second, 0);
                if (base.Value == recoverDate)
                {
                    this.Value = recoverDate;
                    this.OnValueChanged(EventArgs.Empty);
                }
                else
                {
                    this.Value = recoverDate;
                }
            }
        }

        /// <summary>
        /// フォーカスを得た時、色の変更とテキストの反転を行う。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MetDateTimePicker_Enter(object sender, EventArgs e)
        {
            this.enterValue = this.Value;

            // フォーカス取得時の色に変更
            this.changeFocusColor();
        }

        /// <summary>
        /// フォーカスを失った時、色の変更を行う。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MetDateTimePicker_Leave(object sender, EventArgs e)
        {
            this.changeBaseColor();
        }

        private bool formatSwitching = false;
        private bool formatSwitchingByDataBinding = false;

        /// <summary>
        /// 状態に応じてフォーマットの変更を行う。
        /// </summary>
        /// <param name="isNull">null値かどうか。</param>
        private void switchFormat(bool isNull)
        {
            this.formatSwitching = true;
            if (isNull)
            {
                // Valueプロパティがバインドされている場合はValue制御用にフラグを立てる
                var valueBinding = this.DataBindings["Value"];
                if (valueBinding != null)
                {
                    this.formatSwitchingByDataBinding = true;
                    base.Format = DateTimePickerFormat.Custom;
                    base.CustomFormat = " ";
                    this.isNull = true;
                    this.formatSwitchingByDataBinding = false;
                }
                else
                {
                    base.Format = DateTimePickerFormat.Custom;
                    base.CustomFormat = " ";
                    this.isNull = true;
                }
            }
            else
            {
                base.Format = this.innerFormat;
                base.CustomFormat = this.innerCustomFormat;
                this.isNull = false;
            }
            this.formatSwitching = false;
        }

        #endregion

        #region プロパティ

        private bool isValueChanged = false;
        private DateTimePickerFormat innerFormat = DateTimePickerFormat.Long;
        private string innerCustomFormat = "";
        private bool visible = true;

        /// <summary>
        /// コントロールに代入された日付/時刻値を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [Bindable(true)]
        [MetCategory("MetBehavior")]
        [MetDescription("ControlDateTimeValue")]
        public new DateTime? Value
        {
            get
            {
                if (this.isNull)
                {
                    return null;
                }
                return base.Value;
            }
            set
            {
                // Valueがデータバインドしていて、null入力が行われた時にFormat変更によって二度走行するため
                if (this.formatSwitchingByDataBinding)
                {
                    return;
                }

                // Visible=false の時に、nullにしてからTextプロパティで値を変更し、Visible=true にすると、
                // 内部で保持しているであろうnullにする前の日付に変更されて走行してしまうため
                if (this.visibleSwitching)
                {
                    return;
                }

                if (!this.AcceptNull && value == null)
                {
                    throw new ArgumentNullException();
                }

                this.isValueChanged = true;

                // 既にnullが設定されている場合は何もしない
                if (value == null && this.isNull)
                {
                    return;
                }

                // nullの設定時
                if (value == null)
                {
                    this.switchFormat(true);
                    if (ControlExtensions.IsDesignMode(this))
                    {
                        return;
                    }
                    switchControl();
                    this.OnValueChanged(EventArgs.Empty);
                    return;
                }

                // nullからの復帰時
                if (this.Value == null && value != null)
                {
                    this.switchFormat(false);
                    if (ControlExtensions.IsDesignMode(this))
                    {
                        return;
                    }
                    switchControl();
                    if (base.Value == value.Value)
                    {
                        this.OnValueChanged(EventArgs.Empty);
                    }
                    else
                    {
                        base.Value = value.Value;
                    }
                    return;
                }

                base.Value = value.Value;
                if (ControlExtensions.IsDesignMode(this))
                {
                    return;
                }
                switchControl();
            }
        }

        /// <summary>
        /// Value が既定値から変更されたかどうかを返却する。
        /// </summary>
        /// <returns>true:変更された, false:変更されていない</returns>
        internal bool ShouldSerializeValue() => this.isValueChanged;

        /// <summary>
        /// Value のリセット操作を行う。
        /// </summary>
        internal void ResetValue()
        {
            this.Value = DateTime.Now;
            this.Checked = false;
            this.isValueChanged = false;
        }

        /// <summary>
        /// このコントロールに関連付けられているテキストを取得または設定します。
        /// </summary>
        public override string Text
        {
            get
            {
                if (this.isNull)
                {
                    return "";
                }
                return base.Text;
            }
            set
            {
                if (ControlExtensions.IsDesignMode(this))
                {
                    base.Text = value;
                }
                else
                {
                    switchFormat(false);
                    base.Text = value;
                    switchControl();
                }
            }
        }

        /// <summary>
        /// コントロールに表示される日時の書式を取得または設定します。
        /// </summary>
        [DefaultValue(DateTimePickerFormat.Long)]
        public new DateTimePickerFormat Format
        {
            get => base.Format;
            set
            {
                this.innerFormat = value;
                base.Format = value;

                // nullの時は値を復元する
                if (this.isNull)
                {
                    this.switchFormat(false);
                    OnValueChanged(EventArgs.Empty);
                }

                if (!this.controlCreated)
                {
                    return;
                }
                switchControl();
            }
        }

        /// <summary>
        /// 日付/時刻のカスタム書式指定文字列を取得または設定します。
        /// </summary>
        [DefaultValue("")]
        public new string CustomFormat
        {
            get => base.CustomFormat;
            set
            {
                this.innerCustomFormat = value;
                base.CustomFormat = value;

                // nullの時は値を復元する
                if (this.isNull && this.Format == DateTimePickerFormat.Custom && base.CustomFormat != " ")
                {
                    this.switchFormat(false);
                    OnValueChanged(EventArgs.Empty);
                }

                if (!this.controlCreated)
                {
                    return;
                }
                switchControl(true);
            }
        }

        /// <summary>
        /// コントロールとそのすべての子コントロールが表示されているかどうかを示す値を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(true)]
        public new bool Visible
        {
            get => this.visible;
            set
            {
                this.visible = value;
                if (ControlExtensions.IsDesignMode(this))
                {
                    return;
                }
                if (!this.ReadOnly && !this.ReadOnlyLabel)
                {
                    base.Visible = value;
                    return;
                }
                if (!this.controlCreated)
                {
                    return;
                }
                switchControl();
            }
        }

        // FIXED: 例えば特定条件下で色を変えるなどのユーザー実装が行えなくなるため、コード実装は可能とする。
        /// <summary>
        /// コントロールの背景色を取得または特定のプロパティによって強制的に背景色を設定します。
        /// 意図的に背景色の変更が必要な場合のみ利用してください。
        /// </summary>
        [Browsable(false)]
        [ReadOnly(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Color BackColor { get => base.BackColor; set => base.BackColor = value; }

        // FIXED: 例えば特定条件下で色を変えるなどのユーザー実装が行えなくなるため、コード実装は可能とする。
        /// <summary>
        /// コントロールの前景色を取得または特定のプロパティによって強制的に前景色を設定します。
        /// 意図的に前景色の変更が必要な場合のみ利用してください。
        /// </summary>
        [Browsable(false)]
        [ReadOnly(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Color ForeColor { get => base.ForeColor; set => base.ForeColor = value; }

        #endregion

        #region 追加プロパティ

        private bool acceptNull = false;
        private MetTextBox textBox = null;
        private bool readOnlyText = false;
        private Label label = null;
        private bool readOnlyLabel = false;
        private Color defaultBaseBackColor => this.ReadOnly ? SystemColors.Control : SystemColors.Window;   // BaseBackColor の既定値
        private Color? baseBackColor = null;
        private Color defaultFocusBackColor => this.ReadOnly ? SystemColors.Control : SystemColors.Window;  // FocusBackColor の既定値
        private Color? focusBackColor = null;
        private Color defaultBaseForeColor => SystemColors.WindowText;   // BaseForeColor の既定値
        private Color? baseForeColor = null;
        private Color defaultFocusForeColor => SystemColors.WindowText;  // FocusForeColor の既定値
        private Color? focusForeColor = null;

        /// <summary>
        /// NULLの入力を受け入れるかどうかを取得または設定します。
        /// </summary>
        [Browsable(true)]
        [MetCategory("MetBehavior")]
        [DefaultValue(false)]
        [MetDescription("ControlAcceptNull")]
        public bool AcceptNull
        {
            get => this.acceptNull;
            set
            {
                // nullを許可しない場合は、Value実装が動作するため、値を現在日にしてから。
                if (!value && !this.Value.HasValue)
                {
                    this.Value = DateTime.Now;
                    this.acceptNull = value;
                    return;
                }
                this.acceptNull = value;
            }
        }

        /// <summary>
        /// TextBox による代替表示を行っているかどうかを取得します。
        /// </summary>
        [Browsable(true)]
        [MetCategory("MetBehavior")]
        [DefaultValue(false)]
        [MetDescription("ControlReadOnly")]
        public bool ReadOnly
        {
            get
            {
                return this.readOnlyText;
            }
            set
            {
                // ラベルの代替をやめる
                if (value && this.ReadOnlyLabel)
                {
                    this.ReadOnlyLabel = false;
                }

                this.readOnlyText = value;

                if (ControlExtensions.IsDesignMode(this))
                {
                    // 背景色・文字色の変更
                    this.ChangeDisplayColor();
                    return;
                }
                if (!this.controlCreated)
                {
                    return;
                }

                this.switchTextBox();
            }
        }

        private bool visibleSwitching = false;

        /// <summary>
        /// 表示状態に応じて、Visible、テキスト代替を切り替える。
        /// </summary>
        private void switchTextBox()
        {
            if (this.Parent == null)
            {
                return;
            }

            // 非表示
            if (!this.Visible)
            {
                // テキスト代替なしの場合は通常のVisible
                if (!this.ReadOnly)
                {
                    this.ChangeDisplayColor();
                    base.Visible = this.Visible;
                    return;
                }

                // テキストを一旦削除する
                if (this.textBox != null)
                {
                    this.textBox.Visible = false;
                    this.textBox.Parent.Controls.Remove(this.textBox);
                    this.textBox.Dispose();
                    this.textBox = null;
                }
                this.ChangeDisplayColor();
                base.Visible = this.Visible;
                return;
            }

            // 表示でテキスト代替なしの場合はテキストを消す
            if (!this.ReadOnly)
            {
                if (this.textBox != null)
                {
                    this.textBox.Visible = false;
                    this.textBox.Parent.Controls.Remove(this.textBox);
                    this.textBox.Dispose();
                    this.textBox = null;
                }
                this.ChangeDisplayColor();
                if (this.isNull)
                {
                    base.Visible = this.Visible;
                    switchFormat(true);
                }
                else
                {
                    this.visibleSwitching = true;
                    base.Visible = this.Visible;
                    this.visibleSwitching = false;
                }
                return;
            }

            // ReadOnly=trueで既にテキストが存在する場合、一旦テキストを削除する
            if (this.textBox != null)
            {
                this.textBox.Visible = false;
                this.textBox.Parent.Controls.Remove(this.textBox);
                this.textBox.Dispose();
                this.textBox = null;
            }

            // 表示でテキスト代替あり
            this.textBox = new MetTextBox();
            this.textBox.BeginInit();
            this.textBox.ReadOnly = true;
            this.textBox.Text = this.Text;
            this.textBox.Size = this.Size;
            this.textBox.Location = this.Location;
            this.textBox.Margin = this.Margin;
            this.textBox.MaximumSize = this.MaximumSize;
            this.textBox.MinimumSize = this.MinimumSize;
            this.textBox.Padding = this.DefaultPadding;
            this.textBox.Visible = this.Visible;
            this.textBox.Enabled = this.Enabled;
            this.textBox.Dock = this.Dock;
            this.textBox.Font = this.Font;
            this.textBox.Anchor = this.Anchor;
            this.textBox.RightToLeft = this.RightToLeft;
            this.textBox.BorderStyle = this.BaseBorderColor == Color.Transparent ? BorderStyle.Fixed3D : BorderStyle.FixedSingle;
            this.textBox.Cursor = this.Cursor;
            this.textBox.TextAlign = HorizontalAlignment.Left;
            this.textBox.TabIndex = this.TabIndex;
            this.textBox.TabStop = this.TabStop;
            this.textBox.BaseBackColor = this.BaseBackColor;
            this.textBox.BaseForeColor = this.BaseForeColor;
            this.textBox.FocusBackColor = this.FocusBackColor;
            this.textBox.FocusForeColor = this.FocusForeColor;
            this.textBox.BaseBorderColor = this.BaseBorderColor;
            this.textBox.FocusBorderColor = this.FocusBorderColor;
            this.textBox.ErrorBorderColor = this.ErrorBorderColor;
            this.textBox.Error = this.Error;
            this.textBox.EndInit();
            this.ChangeDisplayColor();

            this.textBox.Enter += MetDateTimePicker_Enter;
            this.textBox.Leave += MetDateTimePicker_Leave;

            base.Visible = false;

            if (this.Parent.GetType() == typeof(TableLayoutPanel))
            {
                var tlp = (TableLayoutPanel)this.Parent;
                var position = tlp.GetPositionFromControl(this);
                tlp.Controls.Add(this.textBox, position.Column, position.Row);
            }

            else if (this.Parent.GetType() == typeof(FlowLayoutPanel))
            {
                var flp = (FlowLayoutPanel)this.Parent;
                flp.Controls.Add(this.textBox);
                flp.Controls.SetChildIndex(this.textBox, flp.Controls.IndexOf(this));
            }
            else
            {
                this.Parent.Controls.Add(this.textBox);
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
            get
            {
                return this.readOnlyLabel;
            }
            set
            {
                // テキストの代替をやめる
                if (value && this.ReadOnly)
                {
                    this.ReadOnly = false;
                }

                this.readOnlyLabel = value;

                if (ControlExtensions.IsDesignMode(this))
                {
                    // 背景色・文字色の変更
                    this.ChangeDisplayColor();
                    return;
                }
                if (!this.controlCreated)
                {
                    return;
                }

                this.switchLabel();
            }
        }

        /// <summary>
        /// 表示状態に応じて、Visible、ラベル代替を切り替える。
        /// </summary>
        private void switchLabel()
        {
            if (this.Parent == null)
            {
                return;
            }

            // 非表示
            if (!this.Visible)
            {
                // ラベル代替なしの場合は通常のVisible
                if (!this.ReadOnlyLabel)
                {
                    this.ChangeDisplayColor();
                    base.Visible = this.Visible;
                    return;
                }

                // ラベルを一旦削除する
                if (this.label != null)
                {
                    this.label.Parent.Controls.Remove(this.label);
                    this.label.Dispose();
                    this.label = null;
                }
                this.ChangeDisplayColor();
                base.Visible = this.Visible;
                return;
            }

            // 表示でラベル代替なしの場合はラベルを消す
            if (!this.ReadOnlyLabel)
            {
                if (this.label != null)
                {
                    this.label.Parent.Controls.Remove(this.label);
                    this.label.Dispose();
                    this.label = null;
                }
                this.ChangeDisplayColor();
                if (this.isNull)
                {
                    base.Visible = this.Visible;
                    switchFormat(true);
                }
                else
                {
                    this.visibleSwitching = true;
                    base.Visible = this.Visible;
                    this.visibleSwitching = false;
                }
                return;
            }

            // ReadOnlyLabel=trueで既にラベルが存在する場合、一旦ラベルを削除する
            if (this.label != null)
            {
                this.label.Parent.Controls.Remove(this.label);
                this.label.Dispose();
                this.label = null;
            }

            // 表示でラベル代替あり
            this.label = new Label();
            this.label.Text = this.Text;
            this.label.Size = this.Size;
            this.label.Location = this.Location;
            this.label.Margin = this.Margin;
            this.label.MaximumSize = this.MaximumSize;
            this.label.MinimumSize = this.MinimumSize;
            this.label.Padding = this.DefaultPadding;
            this.label.Visible = this.Visible;
            this.label.Enabled = this.Enabled;
            this.label.Dock = this.Dock;
            this.label.Font = this.Font;
            this.label.Anchor = this.Anchor;
            this.label.RightToLeft = this.RightToLeft;
            this.label.BorderStyle = this.BaseBorderColor == Color.Transparent ? BorderStyle.Fixed3D : BorderStyle.FixedSingle;
            this.label.Cursor = this.Cursor;
            this.label.TextAlign = ContentAlignment.MiddleLeft;
            this.ChangeDisplayColor();

            base.Visible = false;

            if (this.Parent.GetType() == typeof(TableLayoutPanel))
            {
                var tlp = (TableLayoutPanel)this.Parent;
                var position = tlp.GetPositionFromControl(this);
                tlp.Controls.Add(this.label, position.Column, position.Row);
            }

            else if (this.Parent.GetType() == typeof(FlowLayoutPanel))
            {
                var flp = (FlowLayoutPanel)this.Parent;
                flp.Controls.Add(this.label);
                flp.Controls.SetChildIndex(this.label, flp.Controls.IndexOf(this));
            }
            else
            {
                this.Parent.Controls.Add(this.label);
            }
        }

        /// <summary>
        /// TextBoxまたはLabelへ切り替える。
        /// </summary>
        /// <param name="isCustomFormatChange">CustomFormat プロパティを変更したかどうか。</param>
        private void switchControl(bool isCustomFormatChange = false)
        {
            if (isCustomFormatChange)
            {
                if (this.Format == DateTimePickerFormat.Custom && this.ReadOnly)
                {
                    this.switchTextBox();
                }
                if (this.Format == DateTimePickerFormat.Custom && this.ReadOnlyLabel)
                {
                    this.switchLabel();
                }
            }
            else
            {
                if (this.ReadOnly)
                {
                    this.switchTextBox();
                }
                if (this.ReadOnlyLabel)
                {
                    this.switchLabel();
                }
            }
        }

        /// <summary>
        /// コントロールの背景色を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [MetCategory("MetAppearance")]
        [MetDescription("ControlBaseBackColor")]
        public Color BaseBackColor
        {
            get
            {
                this.Refresh();
                return this.baseBackColor ?? this.defaultBaseBackColor;
            }
            set
            {
                this.baseBackColor = value;

                // 背景色・文字色の変更
                this.ChangeDisplayColor();
            }
        }

        /// <summary>
        /// BaseBackColor が既定値から変更されたかどうかを返却します。
        /// </summary>
        /// <returns>true:変更された, false:変更されていない</returns>
        internal bool ShouldSerializeBaseBackColor() => this.baseBackColor != null && this.baseBackColor != this.defaultBaseBackColor;

        /// <summary>
        /// BaseBackColor のリセット操作を行う。
        /// </summary>
        internal void ResetBaseBackColor()
        {
            this.baseBackColor = null;
            // 背景色・文字色の変更
            this.ChangeDisplayColor();

        }

        /// <summary>
        /// フォーカス時のコントロールの背景色を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [MetCategory("MetAppearance")]
        [MetDescription("ControlFocusBackColor")]
        public Color FocusBackColor
        {
            get
            {
                this.Refresh();
                return this.focusBackColor ?? this.defaultFocusBackColor;
            }
            set
            {
                this.focusBackColor = value;

                // 背景色・文字色の変更
                this.ChangeDisplayColor();
            }
        }

        /// <summary>
        /// FocusBackColor が既定値から変更されたかどうかを返却します。
        /// </summary>
        /// <returns>true:変更された, false:変更されていない</returns>
        internal bool ShouldSerializeFocusBackColor() => this.focusBackColor != null && this.focusBackColor != this.defaultFocusBackColor;

        /// <summary>
        /// FocusBackColor のリセット操作を行う。
        /// </summary>
        internal void ResetFocusBackColor()
        {
            this.focusBackColor = null;
            // 背景色・文字色の変更
            this.ChangeDisplayColor();
        }

        /// <summary>
        /// コントロールの文字色を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [MetCategory("MetAppearance")]
        [MetDescription("ControlBaseForeColor")]
        public Color BaseForeColor
        {
            get
            {
                this.Refresh();
                return this.baseForeColor ?? this.defaultBaseForeColor;
            }
            set
            {
                this.baseForeColor = value;

                // 背景色・文字色の変更
                this.ChangeDisplayColor();
            }
        }

        /// <summary>
        /// BaseBackColor が既定値から変更されたかどうかを返却する。
        /// </summary>
        /// <returns>true:変更された, false:変更されていない</returns>
        internal bool ShouldSerializeBaseForeColor() => this.baseForeColor != null && this.baseForeColor != this.defaultBaseForeColor;

        /// <summary>
        /// BaseBackColor のリセット操作を行う。
        /// </summary>
        internal void ResetBaseForeColor()
        {
            this.baseForeColor = null;
            // 背景色・文字色の変更
            this.ChangeDisplayColor();

        }

        /// <summary>
        /// フォーカス時のコントロールの文字色を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [MetCategory("MetAppearance")]
        [MetDescription("ControlFocusForeColor")]
        public Color FocusForeColor
        {
            get
            {
                this.Refresh();
                return this.focusForeColor ?? this.defaultFocusForeColor;
            }
            set
            {
                this.focusForeColor = value;

                // 背景色・文字色の変更
                this.ChangeDisplayColor();
            }
        }

        /// <summary>
        /// FocusBackColor が既定値から変更されたかどうかを返却する。
        /// </summary>
        /// <returns>true:変更された, false:変更されていない</returns>
        internal bool ShouldSerializeFocusForeColor() => this.focusForeColor != null && this.focusForeColor != this.defaultFocusForeColor;

        /// <summary>
        /// FocusBackColor のリセット操作を行う。
        /// </summary>
        internal void ResetFocusForeColor()
        {
            this.focusForeColor = null;
            // 背景色・文字色の変更
            this.ChangeDisplayColor();
        }

        /// <summary>
        /// コントロールのアクティブ状態に応じた表示色に変更します。
        /// </summary>
        protected virtual void ChangeDisplayColor()
        {
            // アクティブの時にアクティブ色に変更
            var form = this.FindForm();
            if (form != null && form.ActiveControl == this)
            {
                this.changeFocusColor();
            }
            else
            {
                this.changeBaseColor();
            }
        }

        /// <summary>
        /// 表示色をフォーカス色に変更する。
        /// </summary>
        private void changeFocusColor()
        {
            if (this.focusBackColor == null)
            {
                base.BackColor = this.defaultFocusBackColor;
            }
            else
            {
                base.BackColor = this.FocusBackColor;
            }
            if (this.focusForeColor == null)
            {
                base.ForeColor = this.defaultFocusForeColor;
            }
            else
            {
                base.ForeColor = this.FocusForeColor;
            }

            // テキストの代替表示を行っている場合はテキストの表示色も変更
            if (this.controlCreated && this.ReadOnly && this.Visible)
            {
                this.textBox.BackColor = this.FocusBackColor;
                this.textBox.ForeColor = this.FocusForeColor;
            }
        }

        /// <summary>
        /// 表示色を非フォーカス色に変更する。
        /// </summary>
        private void changeBaseColor()
        {
            if (this.baseBackColor == null)
            {
                base.BackColor = this.defaultBaseBackColor;
            }
            else
            {
                base.BackColor = this.BaseBackColor;
            }
            if (this.baseForeColor == null)
            {
                base.ForeColor = this.defaultFocusForeColor;
            }
            else
            {
                base.ForeColor = this.BaseForeColor;
            }

            // テキスト・ラベルの代替表示を行っている場合はテキスト・ラベルの表示色も変更
            if (this.controlCreated && this.ReadOnly && this.Visible)
            {
                this.textBox.BackColor = this.BaseBackColor;
                this.textBox.ForeColor = this.BaseForeColor;
            }
            if (this.controlCreated && this.ReadOnlyLabel && this.Visible)
            {
                this.label.BackColor = this.BaseBackColor;
                this.label.ForeColor = this.BaseForeColor;
            }
        }

        #region コントロールの外枠の色

        /// <summary>
        /// コントロールの枠色を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof(Color), "Transparent")]
        [MetCategory("MetAppearance")]
        [MetDescription("ControlBaseBorderColor")]
        public Color BaseBorderColor { get; set; } = Color.Transparent;

        /// <summary>
        /// フォーカス時のコントロールの枠色を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof(Color), "Transparent")]
        [MetCategory("MetAppearance")]
        [MetDescription("ControlFocusBorderColor")]
        public Color FocusBorderColor { get; set; } = Color.Transparent;

        /// <summary>
        /// エラー時のコントロールの枠色を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof(Color), "Red")]
        [MetCategory("MetAppearance")]
        [MetDescription("ControlErrorBorderColor")]
        public Color ErrorBorderColor { get; set; } = Color.Red;

        private bool error = false;

        /// <summary>
        /// コントロールがエラーかどうかを取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(false)]
        [MetCategory("MetAppearance")]
        [MetDescription("ControlError")]
        public bool Error
        {
            get => this.error;
            set
            {
                this.error = value;
                this.Invalidate();
                if (this.ReadOnly)
                {
                    this.textBox.Error = value;
                }
            }
        }

        #endregion

        #endregion

        #region メソッド

        private bool controlCreated = false;

        /// <summary>
        /// InitializeComponent()でコントロールの生成が完了していないことを通知します。
        /// </summary>
        public void BeginInit()
        {
            this.controlCreated = false;
        }

        /// <summary>
        /// 実装はありません。
        /// </summary>
        public void EndInit() { }

        /// <summary>
        /// ロールバック済みかどうかを取得します。
        /// </summary>
        /// <param name="sender">ロールバック指示オブジェクト。</param>
        /// <param name="control">ロールバック対象オブジェクト。</param>
        /// <returns>true:ロールバック済み, false:未ロールバック。</returns>
        public bool IsRollbacked(object sender, Control control)
        {
            return this.Value == this.enterValue;
        }

        /// <summary>
        /// フォーカスを得た時の値にロールバックを行います。
        /// </summary>
        /// <param name="sender">ロールバック指示オブジェクト。</param>
        /// <param name="control">ロールバック対象オブジェクト。</param>
        public void Rollback(object sender, Control control)
        {
            this.Value = this.enterValue;
        }

        /// <summary>
        /// コントロールの生成が完了したことを通知し、ReadOnly, ReadOnlyLabelの制御を行います。
        /// </summary>
        protected override void OnCreateControl()
        {
            // Formatプロパティを変更するとOnCreateControlが走行する。
            // 内部的にFormatプロパティが変更された時は何も処理させない。
            if (this.formatSwitching)
            {
                return;
            }

            base.OnCreateControl();

            if (ControlExtensions.IsDesignMode(this))
            {
                return;
            }

            this.controlCreated = true;
            switchControl();
        }

        /// <summary>
        /// カレンダーを表示した時、日付を復帰します。
        /// </summary>
        /// <param name="eventargs">EventArgs オブジェクト。</param>
        protected override void OnDropDown(EventArgs eventargs)
        {
            if (!this.isNull)
            {
                base.OnDropDown(eventargs);
                return;
            }

            // nullの時、カレンダーを表示後、日付の選択や月の移動ボタンを押すとFormatプロパティ変更直後に
            // 日付が確定してカレンダーが消えてしまうため、直前に指定されていた日付を復帰し、Alt+F4によって
            // カレンダーを表示させ、通常ルートに戻す
            // UIから操作した時、ミリ秒は取り除かれるのが通常仕様
            var recoverDate = new DateTime(base.Value.Year, base.Value.Month, base.Value.Day, base.Value.Hour, base.Value.Minute, base.Value.Second, 0);
            this.Value = recoverDate;
            SendKeys.Send("%{DOWN}");
        }

        /// <summary>
        /// コントロールの配置元が変更時、ラベル・テキスト代替表示をしていたらラベル・テキストの配置元も変更します。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnParentChanged(EventArgs e)
        {
            if (this.controlCreated && this.ReadOnlyLabel)
            {
                this.label.Parent = this.Parent;
            }
            if (this.controlCreated && this.ReadOnly)
            {
                this.textBox.Parent = this.Parent;
            }
            base.OnParentChanged(e);
        }

        /// <summary>
        /// コントロールの表示位置が変更時、ラベル・テキスト代替表示をしていたらラベル・テキストの表示位置も変更します。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLocationChanged(EventArgs e)
        {
            if (this.controlCreated && this.ReadOnlyLabel)
            {
                this.label.Location = this.Location;
            }
            if (this.controlCreated && this.ReadOnly)
            {
                this.textBox.Location = this.Location;
            }
            base.OnLocationChanged(e);
        }

        /// <summary>
        /// コントロールのサイズが変更時、ラベル・テキスト代替表示をしていたらラベル・テキストのサイズも変更します。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSizeChanged(EventArgs e)
        {
            if (this.controlCreated && this.ReadOnlyLabel)
            {
                this.label.Size = this.Size;
            }
            if (this.controlCreated && this.ReadOnly)
            {
                this.textBox.Size = this.Size;
            }
            base.OnSizeChanged(e);
        }

        /// <summary>
        /// コントロールのサイズが変更時、ラベル・テキスト代替表示をしていたらラベル・テキストのサイズも変更します。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(EventArgs e)
        {
            if (this.controlCreated && this.ReadOnlyLabel)
            {
                this.label.Size = this.Size;
            }
            if (this.controlCreated && this.ReadOnly)
            {
                this.textBox.Size = this.Size;
            }
            base.OnResize(e);
        }

        /// <summary>
        /// コントロールのドッキングが変更時、ラベル・テキスト代替表示をしていたらラベル・テキストのドッキングも変更します。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDockChanged(EventArgs e)
        {
            if (this.controlCreated && this.ReadOnlyLabel)
            {
                this.label.Dock = this.Dock;
            }
            if (this.controlCreated && this.ReadOnly)
            {
                this.textBox.Dock = this.Dock;
            }
            base.OnDockChanged(e);
        }

        /// <summary>
        /// コントロールの文字入力方向が変更時、ラベル・テキスト代替表示をしていたらラベル・テキストの文字入力方向も変更します。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnRightToLeftChanged(EventArgs e)
        {
            if (this.controlCreated && this.ReadOnlyLabel)
            {
                this.label.RightToLeft = this.RightToLeft;
            }
            if (this.controlCreated && this.ReadOnly)
            {
                this.textBox.RightToLeft = this.RightToLeft;
            }
            base.OnRightToLeftChanged(e);
        }

        /// <summary>
        /// コントロールのフォントが変更時、ラベル・テキスト代替表示をしていたらラベル・テキストのフォントも変更します。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnFontChanged(EventArgs e)
        {
            if (this.controlCreated && this.ReadOnlyLabel)
            {
                this.label.Font = this.Font;
            }
            if (this.controlCreated && this.ReadOnly)
            {
                this.textBox.Font = this.Font;
            }
            base.OnFontChanged(e);
        }

        /// <summary>
        /// コントロールのマージンが変更時、ラベル・テキスト代替表示をしていたらラベル・テキストのマージンも変更します。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMarginChanged(EventArgs e)
        {
            if (this.controlCreated && this.ReadOnlyLabel)
            {
                this.label.Margin = this.Margin;
            }
            if (this.controlCreated && this.ReadOnly)
            {
                this.textBox.Margin = this.Margin;
            }
            base.OnMarginChanged(e);
        }

        /// <summary>
        /// コントロールのカーソルが変更時、ラベル・テキスト代替表示をしていたらラベル・テキストのカーソルも変更します。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnCursorChanged(EventArgs e)
        {
            if (this.controlCreated && this.ReadOnlyLabel)
            {
                this.label.Cursor = this.Cursor;
            }
            if (this.controlCreated && this.ReadOnly)
            {
                this.textBox.Cursor = this.Cursor;
            }
            base.OnCursorChanged(e);
        }

        /// <summary>
        /// コントロールの活性状態が変更時、テキスト代替表示をしていたらテキストの活性状態も変更します。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnEnabledChanged(EventArgs e)
        {
            if (this.controlCreated && this.ReadOnly)
            {
                this.textBox.Enabled = this.Enabled;
            }
            base.OnEnabledChanged(e);
        }

        /// <summary>
        /// 背景色の変更を行います。
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            // 背景色・文字色、外枠を描画する
            if (m.Msg == WindowMessage.WM_PAINT)
            {
                using (var bmp = new Bitmap(this.ClientRectangle.Width, this.ClientRectangle.Height))
                using (var bmpGraphics = Graphics.FromImage(bmp))
                {
                    // bitmap に描画してもらう
                    var bmphdc = bmpGraphics.GetHdc();
                    var msg = Message.Create(m.HWnd, WindowMessage.WM_PAINT, bmphdc, IntPtr.Zero);
                    base.WndProc(ref msg);
                    bmpGraphics.ReleaseHdc();

                    this.drawBitmap(bmp, bmpGraphics);

                    // コントロールへ描画
                    var hWnd = new HandleRef(this, m.HWnd);
                    var ps = new PAINTSTRUCT();
                    var controlHdc = User32.BeginPaint(hWnd, ref ps);
                    using (var controlGraphics = Graphics.FromHdc(controlHdc))
                    {
                        controlGraphics.DrawImage(bmp, 0, 0);
                    }
                    User32.EndPaint(hWnd, ref ps);
                }
            }
            else
            {
                base.WndProc(ref m);
            }
        }

        /// <summary>
        /// Bitmapオブジェクトにコントロール描画を行う。
        /// </summary>
        private void drawBitmap(Bitmap bmp, Graphics bmpGraphics)
        {
            // 現状のコントロール描画をBitmapにコピー
            this.DrawToBitmap(bmp, new Rectangle(0, 0, this.Width, this.Height));

            System.Drawing.Imaging.ColorMap[] cm = { new System.Drawing.Imaging.ColorMap(), new System.Drawing.Imaging.ColorMap() };

            // 背景色のマッピング
            cm[0].OldColor = SystemColors.Window;
            cm[0].NewColor = this.BackColor;

            // 文字色のマッピング
            cm[1].OldColor = SystemColors.WindowText;
            cm[1].NewColor = this.ForeColor;

            // 背景色・文字色の変更
            var ia = new System.Drawing.Imaging.ImageAttributes();
            ia.SetRemapTable(cm);
            var r = new Rectangle(0, 0, bmp.Width, bmp.Height);
            bmpGraphics.DrawImage(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height),
                        0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, ia);

            // 外枠の変更
            var frameColor = this.BaseBorderColor;
            var form = this.FindForm();
            if (form != null && form.ActiveControl == this)
            {
                frameColor = this.FocusBorderColor;
            }
            if (this.Error)
            {
                frameColor = this.ErrorBorderColor;
            }

            bmpGraphics.DrawRectangle(new Pen(frameColor), new Rectangle(0, 0, bmp.Width - 1, bmp.Height - 1));
        }

        #endregion
    }
}
