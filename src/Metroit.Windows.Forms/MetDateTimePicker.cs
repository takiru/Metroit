﻿using Metroit.Win32.Api;
using Metroit.Win32.Api.DisplayDeviceReference.WinDef;
using Metroit.Win32.Api.WindowsControls.CommonCtrl;
using Metroit.Win32.Api.WindowsControls.WinUser;
using Metroit.Win32.Api.WindowsGdi;
using Metroit.Win32.Api.WindowsGdi.WinUser;
using Metroit.Win32.DesktopAppUi.WindowsControls;
using Metroit.Windows.Forms.Extensions;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;

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
            if (this.IsDesignMode())
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
                this.Value = this.GetRestoreValue(base.Value, true);
            }
        }

        /// <summary>
        /// フォーカスを得た時、色の変更とテキストの反転を行う。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MetDateTimePicker_Enter(object sender, EventArgs e)
        {
            if (!IsValidatingCanceled)
            {
                this.enterValue = this.Value;
            }

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
                    if (this.IsDesignMode())
                    {
                        return;
                    }
                    if (!this.controlCreated)
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
                    if (this.IsDesignMode())
                    {
                        return;
                    }
                    if (base.Value == value.Value)
                    {
                        // Value と Text の内容が合致しないため、一度 Value を変更させて整合させる
                        this.InnerValueChanged = true;
                        base.Value = DateTime.Now;
                        this.InnerValueChanged = false;
                        base.Value = value.Value;
                    }
                    else
                    {
                        base.Value = value.Value;
                    }
                    switchControl();
                    return;
                }

                base.Value = this.GetValueByMinCalendarType(value.Value);
                if (this.IsDesignMode())
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
        private bool ShouldSerializeValue() => this.isValueChanged;

        /// <summary>
        /// Value のリセット操作を行う。
        /// </summary>
        private void ResetValue()
        {
            this.isValueChanged = true;
            this.Value = this.GetRestoreValue(DateTime.Now);

            // カレンダーが日まで指定する場合は既定と同じ動作とする
            if (this.MinCalendarType == CalendarType.Day)
            {
                this.Checked = false;
                this.isValueChanged = false;
            }
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
                if (this.IsDesignMode())
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
            get => this.innerFormat;
            set
            {
                this.innerFormat = value;
                base.Format = value;

                // nullの時は値を復元する
                if (this.isNull)
                {
                    this.switchFormat(false);

                    // Value と Text の内容が合致しないため、一度 Value を変更させて整合させる
                    this.InnerValueChanged = true;
                    var restoreValue = base.Value;
                    base.Value = DateTime.Now;
                    this.InnerValueChanged = false;
                    base.Value = restoreValue;
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
            get => this.innerCustomFormat;
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
                if (this.IsDesignMode())
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

                if (this.IsDesignMode())
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

                if (this.IsDesignMode())
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
        private bool ShouldSerializeBaseBackColor() => this.baseBackColor != null && this.baseBackColor != this.defaultBaseBackColor;

        /// <summary>
        /// BaseBackColor のリセット操作を行う。
        /// </summary>
        private void ResetBaseBackColor()
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
        private bool ShouldSerializeFocusBackColor() => this.focusBackColor != null && this.focusBackColor != this.defaultFocusBackColor;

        /// <summary>
        /// FocusBackColor のリセット操作を行う。
        /// </summary>
        private void ResetFocusBackColor()
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
        private bool ShouldSerializeBaseForeColor() => this.baseForeColor != null && this.baseForeColor != this.defaultBaseForeColor;

        /// <summary>
        /// BaseBackColor のリセット操作を行う。
        /// </summary>
        private void ResetBaseForeColor()
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
        private bool ShouldSerializeFocusForeColor() => this.focusForeColor != null && this.focusForeColor != this.defaultFocusForeColor;

        /// <summary>
        /// FocusBackColor のリセット操作を行う。
        /// </summary>
        private void ResetFocusForeColor()
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
            if (this.ReadOnly && this.Visible && this.textBox != null)
            {
                this.textBox.FocusBackColor = this.FocusBackColor;
                this.textBox.FocusForeColor = this.FocusForeColor;
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
            if (this.ReadOnly && this.Visible && this.textBox != null)
            {
                this.textBox.BaseBackColor = this.BaseBackColor;
                this.textBox.BaseForeColor = this.BaseForeColor;
            }
            if (this.ReadOnlyLabel && this.Visible && this.label != null)
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

        #region カレンダー

        private CalendarType minCalendarType = CalendarType.Day;

        /// <summary>
        /// カレンダーコントロールで選べる年月日タイプと、実際に設定できる年月日タイプを取得または設定します。
        /// </summary>
        [Browsable(true)]
        [Bindable(true)]
        [DefaultValue(CalendarType.Day)]
        [MetCategory("MetBehavior")]
        [MetDescription("ControlDateTimeMinCalendarType")]
        public CalendarType MinCalendarType
        {
            get
            {
                return this.minCalendarType;
            }
            set
            {
                this.minCalendarType = value;

                // 月日を固定値に切り替える
                if (this.MinCalendarType != CalendarType.Day)
                {
                    this.Value = this.GetValueByMinCalendarType(this.Value.Value);
                }
            }
        }

        private bool showToday = true;

        /// <summary>
        /// カレンダーコントロールの下部に「今日」の日付を表示するかどうかを設定します。
        /// </summary>
        [Browsable(true)]
        [Bindable(true)]
        [DefaultValue(true)]
        [MetCategory("MetBehavior")]
        [MetDescription("ControlDateTimeShowToday")]
        public bool ShowToday
        {
            get => this.showToday;
            set
            {
                this.showToday = value;
                this.RefreshCalendarLayout();
            }
        }

        private bool showTodayCircle = true;

        /// <summary>
        /// カレンダーコントロールの「今日」の日付を丸で囲むかどうかを設定します。
        /// </summary>
        [Browsable(true)]
        [Bindable(true)]
        [DefaultValue(true)]
        [MetCategory("MetBehavior")]
        [MetDescription("ControlDateTimeShowTodayCircle")]
        public bool ShowTodayCircle
        {
            get => this.showTodayCircle;
            set
            {
                this.showTodayCircle = value;
                this.RefreshCalendarLayout();
            }
        }

        private bool showTorailingDates = true;

        /// <summary>
        /// カレンダーコントロールで、前月と翌月の日付は、当月のカレンダーには表示するかどうかを設定します。
        /// </summary>
        [Browsable(true)]
        [Bindable(true)]
        [DefaultValue(true)]
        [MetCategory("MetBehavior")]
        [MetDescription("ControlDateTimeShowTorailingDates")]
        public bool ShowTorailingDates
        {
            get => this.showTorailingDates;
            set
            {
                this.showTorailingDates = value;
                this.RefreshCalendarLayout();
            }
        }

        #endregion

        #endregion

        #region メソッド

        private bool controlCreated = false;
        private IntPtr initialCalendarLayoutValue;

        /// <summary>
        /// 内部的なValueの変更を実施中かどうかを取得します。
        /// 内部的なValueの変更であってもOnValueChangedが発生するため、このプロパティで判定を行う必要があります。
        /// </summary>
        protected bool InnerValueChanged { get; private set; } = false;

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
        /// <returns>true:ロールバック済み, false:未ロールバック。</returns>
        [Browsable(false)]
        public bool IsRollbacked => this.Value == this.enterValue;

        /// <summary>
        /// フォーカスを得た時の値にロールバックを行います。
        /// </summary>
        public void Rollback()
        {
            this.Value = this.enterValue;
        }

        /// <summary>
        /// ハンドルが作成された時に、カレンダーレイアウトの初期状態の把握と、カレンダーレイアウトの反映を行います。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnHandleCreated(EventArgs e)
        {
            this.initialCalendarLayoutValue = User32.SendMessage(this.Handle, DateTimePickerMessages.DTM_GETMCSTYLE, IntPtr.Zero, IntPtr.Zero);
            this.RefreshCalendarLayout();

            base.OnHandleCreated(e);
        }

        /// <summary>
        /// コントロールの生成が完了したことを通知し、ReadOnly, ReadOnlyLabelの制御を行います。
        /// </summary>
        protected override void OnCreateControl()
        {
            // null変換、null復帰のためにFormatプロパティを変更するとOnCreateControlが走行する。
            // 内部的にFormatプロパティが変更された時は何も処理させない。
            if (this.formatSwitching)
            {
                return;
            }

            base.OnCreateControl();

            if (this.IsDesignMode())
            {
                return;
            }
            switchControl();
            this.controlCreated = true;
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
            // 日付が確定してカレンダーが消えてしまうため、直前に指定されていた日付を復帰し、Alt+下キーによって
            // カレンダーを表示させ、通常ルートに戻す
            // UIから操作した時、ミリ秒は取り除かれるのが通常仕様
            this.Value = this.GetRestoreValue(base.Value, true);
            SendKeys.Send("%{DOWN}");
        }

        /// <summary>
        /// 値が変更された時、内部的な値変更では処理しないようにします。
        /// InnerValueChanged = true の場合は走行させないようにしてください。
        /// </summary>
        /// <param name="eventargs"></param>
        protected override void OnValueChanged(EventArgs eventargs)
        {
            // 内部的にValueを強制変更時は処理させない
            if (this.InnerValueChanged)
            {
                return;
            }
            base.OnValueChanged(eventargs);
        }

        /// <summary>
        /// コントロールの配置元が変更時、ラベル・テキスト代替表示をしていたらラベル・テキストの配置元も変更します。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnParentChanged(EventArgs e)
        {
            if (this.controlCreated && this.ReadOnlyLabel && this.label != null)
            {
                this.label.Parent = this.Parent;
            }
            if (this.controlCreated && this.ReadOnly && this.textBox != null)
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
            if (this.controlCreated && this.ReadOnlyLabel && this.label != null)
            {
                this.label.Location = this.Location;
            }
            if (this.controlCreated && this.ReadOnly && this.textBox != null)
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
            if (this.controlCreated && this.ReadOnlyLabel && this.label != null)
            {
                this.label.Size = this.Size;
            }
            if (this.controlCreated && this.ReadOnly && this.textBox != null)
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
            if (this.controlCreated && this.ReadOnlyLabel && this.label != null)
            {
                this.label.Size = this.Size;
            }
            if (this.controlCreated && this.ReadOnly && this.textBox != null)
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
            if (this.controlCreated && this.ReadOnlyLabel && this.label != null)
            {
                this.label.Dock = this.Dock;
            }
            if (this.controlCreated && this.ReadOnly && this.textBox != null)
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
            if (this.controlCreated && this.ReadOnlyLabel && this.label != null)
            {
                this.label.RightToLeft = this.RightToLeft;
            }
            if (this.controlCreated && this.ReadOnly && this.textBox != null)
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
            if (this.controlCreated && this.ReadOnlyLabel && this.label != null)
            {
                this.label.Font = this.Font;
            }
            if (this.controlCreated && this.ReadOnly && this.textBox != null)
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
            if (this.controlCreated && this.ReadOnlyLabel && this.label != null)
            {
                this.label.Margin = this.Margin;
            }
            if (this.controlCreated && this.ReadOnly && this.textBox != null)
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
            if (this.controlCreated && this.ReadOnlyLabel && this.label != null)
            {
                this.label.Cursor = this.Cursor;
            }
            if (this.controlCreated && this.ReadOnly && this.textBox != null)
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
            if (this.controlCreated && this.ReadOnly && this.textBox != null)
            {
                this.textBox.Enabled = this.Enabled;
            }
            base.OnEnabledChanged(e);
        }

        /// <summary>
        /// カレンダーレイアウトを反映する。
        /// </summary>
        private void RefreshCalendarLayout()
        {
            var layoutValue = this.initialCalendarLayoutValue.ToInt32();
            if (!ShowToday)
            {
                layoutValue |= MonthCalendarControlStyles.MCS_NOTODAY;
            }
            if (!ShowTodayCircle)
            {
                layoutValue |= MonthCalendarControlStyles.MCS_NOTODAYCIRCLE;
            }
            if (!ShowTorailingDates)
            {
                layoutValue |= MonthCalendarControlStyles.MCS_NOTRAILINGDATES;
            }
            User32.SendMessage(this.Handle, DateTimePickerMessages.DTM_SETMCSTYLE, IntPtr.Zero, new IntPtr(layoutValue));
        }

        /// <summary>
        /// 固有のメッセージ制御を行います。
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WindowMessage.WM_PAINT:
                    WmPaint(ref m);
                    break;

                case WindowMessage.WM_KEYDOWN:
                    WmKeyDown(ref m);
                    break;

                case WindowMessage.WM_NOTIFY:
                    WmNotify(ref m);
                    break;

                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        /// <summary>
        /// WM_PAINTにより、背景色・文字色・外枠色の変更を行う。
        /// </summary>
        /// <param name="m"></param>
        private void WmPaint(ref Message m)
        {
            var cs = this.ClientSize;
            using (var bmp = new Bitmap(cs.Width, cs.Height))
            {
                BitBltGraphics(bmp, cs);

                if (m.WParam == IntPtr.Zero)
                {
                    // コントロールに描画
                    var ps = new PaintStruct();
                    var controlHdc = User32.BeginPaint(m.HWnd, ref ps);
                    using (var controlGraphics = Graphics.FromHdc(controlHdc))
                    {
                        controlGraphics.DrawImage(bmp, 0, 0);
                    }
                    User32.EndPaint(m.HWnd, ref ps);
                }
                else
                {
                    // hdc に描画(WM_PAINTの動作は、意図的にWParamにHDCを設定した時は、そのHDCに描画する仕様)
                    using (var controlGraphics = Graphics.FromHdc(m.WParam))
                    {
                        controlGraphics.DrawImage(bmp, 0, 0);
                    }
                }
            }
        }

        /// <summary>
        /// WM_KEYDOWNにより、上下キーによって日付を変更時の制御を行う。
        /// </summary>
        /// <param name="m"></param>
        private void WmKeyDown(ref Message m)
        {
            // 上、下キー以外の押下は既定動作
            if (m.WParam.ToInt32() != VirtualKey.VK_UP && m.WParam.ToInt32() != VirtualKey.VK_DOWN)
            {
                base.WndProc(ref m);
                return;
            }
            // nullからの復帰が行われる場合は既定動作
            if (this.isNull)
            {
                base.WndProc(ref m);
                return;
            }
            // カレンダータイプが日の場合は既定動作
            if (this.MinCalendarType == CalendarType.Day)
            {
                base.WndProc(ref m);
                return;
            }

            // まず一度WndProcによって値を書き換える
            this.InnerValueChanged = true;
            base.WndProc(ref m);
            this.InnerValueChanged = false;

            var newValue = this.GetValueByMinCalendarType(this.Value.Value);
            // 書き換えた値が一緒の場合はカレンダーのタイプによって固定になる月日以外の変更であるため、イベント発行
            if (this.Value.Value == newValue)
            {
                OnValueChanged(EventArgs.Empty);
                return;
            }

            // 書き換えた値が異なる場合は固定の日付に戻し、イベント発行はしない
            this.InnerValueChanged = true;
            this.Value = newValue;
            this.InnerValueChanged = false;
        }

        /// <summary>
        /// WM_NOTIFYにより、カレンダー操作を制御する。
        /// </summary>
        /// <param name="m"></param>
        private void WmNotify(ref Message m)
        {
            // カレンダー利用ではない場合
            if (this.ShowUpDown)
            {
                if (!this.isNull)
                {
                    base.WndProc(ref m);
                    return;
                }
                this.Value = this.GetRestoreValue(base.Value, true);
                return;
            }

            // カレンダータイプが日の場合は制御しない
            if (this.MinCalendarType == CalendarType.Day)
            {
                base.WndProc(ref m);
                return;
            }

            var nmhdr = (NmHdr)Marshal.PtrToStructure(m.LParam, typeof(NmHdr));

            if (nmhdr.code != MonthCalendarNotifications.MCN_SELCHANGE && nmhdr.code != -950 &&
                nmhdr.code != MonthCalendarNotifications.MCN_VIEWCHANGE)
            {
                base.WndProc(ref m);
                return;
            }

            switch (nmhdr.code)
            {
                case MonthCalendarNotifications.MCN_SELCHANGE:
                    // 十字キーでカレンダーを選択時
                    // 表示の日付を正しく切り替わるようにするため、WndProcを実施する
                    // base.WndProc()によってカレンダーを表示した時の日付に戻ってしまうので、強制的に書き換えてOnValueChangedを発行させる
                    this.InnerValueChanged = true;
                    base.WndProc(ref m);

                    if (this.MinCalendarType != CalendarType.Day)
                    {
                        this.Value = this.GetValueByMinCalendarType(this.Value.Value);
                    }
                    this.InnerValueChanged = false;
                    OnValueChanged(EventArgs.Empty);
                    break;

                case -950:
                    // カレンダーを開いた時、カレンダータイプのカレンダーを表示する
                    var calendar = User32.SendMessage(this.Handle, DateTimePickerMessages.DTM_GETMONTHCAL, IntPtr.Zero, IntPtr.Zero);
                    User32.SendMessage(calendar, MonthCalendarMessages.MCM_SETCURRENTVIEW, IntPtr.Zero, (IntPtr)this.MinCalendarType);
                    break;

                case MonthCalendarNotifications.MCN_VIEWCHANGE:
                    // カレンダータイプより下位のカレンダータイプに切り替えた場合、カレンダーを閉じる
                    var nmviewchange = (NmViewChange)Marshal.PtrToStructure(m.LParam, typeof(NmViewChange));
                    if (nmviewchange.dwOldView == (int)this.MinCalendarType && nmviewchange.dwNewView == (int)this.MinCalendarType - 1)
                    {
                        User32.SendMessage(this.Handle, DateTimePickerMessages.DTM_CLOSEMONTHCAL, IntPtr.Zero, IntPtr.Zero);
                    }
                    break;
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, DateTimePickerInfo lParam);

        /// <summary>
        /// 背景色、文字色、キャレット範囲をビットブロック転送し、外枠を描画する。
        /// </summary>
        /// <param name="bmp">Bitmap オブジェクト。</param>
        /// <param name="cs">クライアントサイズ。</param>
        private void BitBltGraphics(Bitmap bmp, Size cs)
        {
            using (var g = Graphics.FromImage(bmp))
            using (var bmpBack = CreateNativeBitmap(cs))
            using (var bmpFore = CreateNegativeBitmap(bmpBack))
            {
                IntPtr hdc = g.GetHdc();
                var canvas = GetCanvasRectangle(cs);

                // ベースとなる背景色を変更した全体イメージを生成し、適用する
                BitBlt(bmpBack, this.BackColor, canvas, RasterOperations.SRCAND);
                BitBlt(hdc, bmpBack, RasterOperations.SRCCOPY);

                // 文字色を変更したイメージを生成し、入力領域のみを再描画する
                BitBlt(bmpFore, this.ForeColor, canvas, RasterOperations.SRCAND);
                BitBlt(hdc, bmpFore, canvas, canvas.Location, RasterOperations.SRCPAINT);

                if (this.ContainsFocus)
                {
                    // 選択領域を再描画する
                    using (var bmpOrg = CreateNativeBitmap(cs))
                    {
                        Rectangle caret = GetCaretRectangle(bmpOrg, canvas);
                        BitBlt(hdc, bmpOrg, caret, caret.Location, RasterOperations.SRCCOPY);
                    }
                }
                g.ReleaseHdc();

                // 外枠の変更
                var frameColor = this.BaseBorderColor;
                if (this.ContainsFocus)
                {
                    frameColor = this.FocusBorderColor;
                }
                if (this.Error)
                {
                    frameColor = this.ErrorBorderColor;
                }
                g.DrawRectangle(new Pen(frameColor), new Rectangle(0, 0, bmp.Width - 1, bmp.Height - 1));
            }
        }

        /// <summary>
        /// 描画変更される前のBitmapオブジェクトを取得する。
        /// </summary>
        /// <param name="cs">クライアントサイズ。</param>
        /// <returns>Bitmap オブジェクト。</returns>
        private Bitmap CreateNativeBitmap(Size cs)
        {
            var bmp = new Bitmap(cs.Width, cs.Height);
            using (var g = Graphics.FromImage(bmp))
            {
                var hdc = g.GetHdc();
                var pm = Message.Create(this.Handle, WindowMessage.WM_PAINT, hdc, IntPtr.Zero);
                base.DefWndProc(ref pm);
                g.ReleaseHdc();
            }
            return bmp;
        }

        /// <summary>
        /// 対象Bitmapオブジェクトの色を反転し、背景を白、文字を黒としたBitmapオブジェクトを取得する。
        /// </summary>
        /// <param name="bmp">Bitmap オブジェクト。</param>
        /// <returns>Bitmap オブジェクト。</returns>
        private static Bitmap CreateNegativeBitmap(Bitmap bmp)
        {
            var bmpDest = new Bitmap(bmp.Width, bmp.Height);
            using (var g = Graphics.FromImage(bmpDest))
            {
                var hdc = g.GetHdc();
                BitBlt(hdc, bmp, RasterOperations.NOTSRCCOPY);
                g.ReleaseHdc();
            }
            return bmpDest;
        }

        /// <summary>
        /// 入力エリア領域を求める。
        /// </summary>
        /// <returns>Rectangle オブジェクト。</returns>
        private Rectangle GetCanvasRectangle(Size cs)
        {
            var bsz = SystemInformation.Border3DSize;

            var dti = new DateTimePickerInfo();
            SendMessage(this.Handle, DateTimePickerMessages.DTM_GETDATETIMEPICKERINFO, IntPtr.Zero, dti);
            var x = bsz.Width + (dti.rcCheck.Right != 0 ? dti.rcCheck.Right - 1 : 0);
            var y = bsz.Height;
            var height = cs.Height - bsz.Height * 2;

            if (ShowUpDown)
            {
                Rect rc;
                User32.GetWindowRect(dti.hwndUD, out rc);
                Point point = PointToClient(new Point(rc.Left, rc.Top));
                var width = point.X - bsz.Width - (dti.rcCheck.Right != 0 ? dti.rcCheck.Right - 1 : 0);
                return new Rectangle(x, y, width, height);
            }
            else
            {
                var width = dti.rcButton.Left - bsz.Width - (dti.rcCheck.Right != 0 ? dti.rcCheck.Right - 1 : 0);
                return new Rectangle(x, y, width, height);
            }
        }

        /// <summary>
        /// 選択領域の座標を求める。
        /// </summary>
        /// <param name="bmp">Bitmap オブジェクト。</param>
        /// <param name="canvas">Rectangle オブジェクト。</param>
        /// <returns>選択領域座標の Rectangle オブジェクト。</returns>
        private static Rectangle GetCaretRectangle(Bitmap bmp, Rectangle canvas)
        {
            var baseColor = SystemColors.Highlight;
            int pixelSize = Image.GetPixelFormatSize(bmp.PixelFormat) / 8;
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
                ImageLockMode.ReadOnly, bmp.PixelFormat);

            byte[] pixels = new byte[bmpData.Stride * bmp.Height];
            Marshal.Copy(bmpData.Scan0, pixels, 0, pixels.Length);

            var rangeStartX = -1;
            var rangeEndX = -1;
            var rangeStartY = canvas.Top + 2;   // Yの開始座標を、とりあえず4ピクセル目とする
            var rangeEndY = bmpData.Height - 4; // Yの終了座標は、どの環境でも全体レイアウトの高さ-4

            // X方向へ選択領域座標を取得
            for (int x = canvas.Left; x <= canvas.Left + canvas.Width - 1; x++)
            {
                int pos = rangeStartY * bmpData.Stride + x * pixelSize;
                if (!(pixels[pos] == baseColor.B && pixels[pos + 1] == baseColor.G && pixels[pos + 2] == baseColor.R))
                {
                    continue;
                }
                if (rangeStartX == -1)
                {
                    rangeStartX = x;
                }
                else
                {
                    rangeEndX = x;
                }
            }
            // Y開始座標が3ピクセル目の場合があるので、開始座標を求め直す
            for (int y = rangeStartY; y >= canvas.Top; y--)
            {
                int pos = y * bmpData.Stride + rangeStartX * pixelSize;
                if (!(pixels[pos] == baseColor.B && pixels[pos + 1] == baseColor.G && pixels[pos + 2] == baseColor.R))
                {
                    continue;
                }
                rangeStartY = y;
            }
            bmp.UnlockBits(bmpData);
            return Rectangle.FromLTRB(rangeStartX, rangeStartY, rangeEndX + 1, rangeEndY);
        }

        /// <summary>
        /// Bitmapの入力領域に対して、背景色・文字色だけを変更し、それ以外を黒とする。
        /// </summary>
        /// <param name="bmp">Bitmap オブジェクト。</param>
        /// <param name="color">変更する色。</param>
        /// <param name="rectangle"></param>
        /// <param name="dwRop"></param>
        private static void BitBlt(Bitmap bmp, Color color, Rectangle rectangle, int dwRop)
        {
            using (var g = Graphics.FromImage(bmp))
            {
                IntPtr hdc = g.GetHdc();
                BitBlt(hdc, bmp, new Rectangle(0, 0, bmp.Width, bmp.Height), new Point(0, 0), RasterOperations.SRCCOPY);

                using (var fillBmp = new Bitmap(rectangle.Width, rectangle.Height))
                using (var fillGraphics = Graphics.FromImage(fillBmp))
                {
                    fillGraphics.Clear(color);
                    BitBlt(hdc, fillBmp, rectangle, new Point(0, 0), dwRop);
                }

                g.ReleaseHdc();
            }
        }

        /// <summary>
        /// 対象Bitmapを指定したラスター操作コードで色変換する。
        /// </summary>
        /// <param name="hdc">IntPtr。</param>
        /// <param name="bmp">Bitmapオブジェクト。</param>
        /// <param name="dwRop">ラスター操作コード。</param>
        private static void BitBlt(IntPtr hdc, Bitmap bmp, int dwRop)
        {
            BitBlt(hdc, bmp, new Rectangle(0, 0, bmp.Width, bmp.Height), new Point(0, 0), dwRop);
        }

        /// <summary>
        /// 対象領域を指定したラスター操作コードで色変換する。
        /// </summary>
        /// <param name="hdc">IntPtr。</param>
        /// <param name="bmp">Bitmap オブジェクト。</param>
        /// <param name="rectangle">Rectangle オブジェクト。</param>
        /// <param name="point">Point オブジェクト。</param>
        /// <param name="dwRop">ラスター操作コード。</param>
        private static void BitBlt(IntPtr hdc, Bitmap bmp, Rectangle rectangle, Point point, int dwRop)
        {
            var hdcSrc = Gdi32.CreateCompatibleDC(hdc);
            var hBitmap = bmp.GetHbitmap();
            var hbmpOld = Gdi32.SelectObject(hdcSrc, hBitmap);

            Gdi32.BitBlt(hdc, rectangle.Left, rectangle.Top, rectangle.Width, rectangle.Height,
                hdcSrc, point.X, point.Y, dwRop);

            Gdi32.SelectObject(hdcSrc, hbmpOld);
            Gdi32.DeleteObject(hBitmap);
            Gdi32.DeleteDC(hdcSrc);
        }

        /// <summary>
        /// 対象の日付で、カレンダータイプに応じた復元用の日付を取得する。
        /// </summary>
        /// <param name="value">日付値。</param>
        /// <param name="resetMilliSecond">ミリ秒をリセットするかどうか。</param>
        /// <returns>カレンダータイプに応じた復元用の日付。</returns>
        private DateTime GetRestoreValue(DateTime value, bool resetMilliSecond = false)
        {
            DateTime result;
            switch (this.MinCalendarType)
            {
                case CalendarType.Month:
                    result = new DateTime(value.Year, value.Month, 1, value.Hour, value.Minute, value.Second, resetMilliSecond ? 0 : value.Millisecond);
                    break;
                case CalendarType.Year:
                    result = new DateTime(value.Year, 1, 1, value.Hour, value.Minute, value.Second, resetMilliSecond ? 0 : value.Millisecond);
                    break;
                default:
                    result = new DateTime(value.Year, value.Month, value.Day, value.Hour, value.Minute, value.Second, resetMilliSecond ? 0 : value.Millisecond);
                    break;
            }
            return result;
        }

        /// <summary>
        /// 対象の日付で、カレンダータイプに応じた日付を取得する。
        /// </summary>
        /// <param name="value">日付値。</param>
        /// <returns>カレンダータイプに応じた日付。</returns>
        private DateTime GetValueByMinCalendarType(DateTime value)
        {
            DateTime result;
            switch (this.MinCalendarType)
            {
                case CalendarType.Month:
                    result = value.AddDays(-(value.Day - 1));
                    break;

                case CalendarType.Year:
                    result = value.AddMonths(-(value.Month - 1)).AddDays(-(value.Day - 1));
                    break;
                default:
                    result = value;
                    break;
            }
            return result;
        }

        #endregion


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
