using Metroit.Win32.Api;
using Metroit.Win32.Api.WindowsControls.CommonCtrl;
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
    /// Windows コンボ ボックス コントロールを表します。
    /// </summary>
    [ToolboxItem(true)]
    public class MetComboBox : ComboBox, IControlRollback, IBorder
    {
        /// <summary>
        /// MetComboBox の新しいインスタンスを初期化します。
        /// </summary>
        public MetComboBox()
            : base()
        {
            // デザイン時は制御しない
            if (this.IsDesignMode())
            {
                return;
            }

            this.Enter += MetComboBox_Enter;
            this.Leave += MetComboBox_Leave;
            this.DrawItem += MetComboBox_DrawItem;
        }

        #region イベント

        private object enterSelectedItem = null;

        /// <summary>
        /// フォーカスを得た時、色の変更とテキストの反転を行う。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MetComboBox_Enter(object sender, EventArgs e)
        {
            if (!IsValidatingCanceled)
            {
                enterSelectedItem = this.SelectedItem;
            }
            ChangeDisplayColor();
        }

        /// <summary>
        /// フォーカスを失った時、色の変更を行う。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MetComboBox_Leave(object sender, EventArgs e)
        {
            ChangeDisplayColor();
        }

        /// <summary>
        /// DrawModeがOwnerDrawFixed, OwnerDrawVariableの時、文字の色を変更してリスト描画する。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MetComboBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            e.DrawBackground();

            // 選択時は青い背景となるので、文字を白くする
            bool selected = DrawItemState.Selected == (e.State & DrawItemState.Selected);
            var brush = (selected) ? Brushes.White : new SolidBrush(this.ForeColor);

            e.Graphics.DrawString(this.Items[e.Index].ToString(), e.Font, brush, e.Bounds, StringFormat.GenericDefault);
            e.DrawFocusRectangle();
        }

        #endregion

        #region プロパティ

        private bool visible = true;

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
                SwitchControl();
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


        /// <summary>
        /// 代替用のTextBox, Labelを親コントロールへ追加する。
        /// </summary>
        /// <param name="control">Control オブジェクト。</param>
        private void AddControl(Control control)
        {
            if (this.Parent.GetType() == typeof(TableLayoutPanel))
            {
                var tlp = (TableLayoutPanel)this.Parent;
                var position = tlp.GetPositionFromControl(this);
                tlp.Controls.Add(control, position.Column, position.Row);
            }

            else if (this.Parent.GetType() == typeof(FlowLayoutPanel))
            {
                var flp = (FlowLayoutPanel)this.Parent;
                flp.Controls.Add(control);
                flp.Controls.SetChildIndex(control, flp.Controls.IndexOf(this));
            }
            else
            {
                this.Parent.Controls.Add(control);
            }
        }

        /// <summary>
        /// 代替用のコントロールを親コントロールから削除する。
        /// </summary>
        /// <param name="control">代替用Controlオブジェクト。</param>
        private void RemoveControl(Control control)
        {
            control.Parent?.Controls.Remove(control);
        }

        /// <summary>
        /// 表示状態に応じて、Visible、テキスト代替を切り替える。
        /// </summary>
        private void switchTextBox()
        {
            // 非表示の時はTextBoxを親コントロールから削除する
            if (!this.Visible)
            {
                this.textBox.Visible = false;
                this.RemoveControl(this.textBox);
                this.ChangeDisplayColor();
                return;
            }

            // 非ReadOnlyの時はComboBox自体を表示し直す
            if (!this.ReadOnly)
            {
                base.Visible = true;
                this.textBox.Visible = false;
                this.RemoveControl(this.textBox);
                this.ChangeDisplayColor();
                return;
            }

            // 代替TextBoxを用意
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

            this.textBox.Enter += MetComboBox_Enter;
            this.textBox.Leave += MetComboBox_Leave;

            // ComboBox自体を非表示にし、ComboBoxが存在する位置に配置する
            base.Visible = false;
            this.AddControl(this.textBox);
            this.ChangeDisplayColor();
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
            // 非表示の時はLabelを親コントロールから削除する
            if (!this.Visible)
            {
                this.RemoveControl(this.label);
                this.ChangeDisplayColor();
                return;
            }

            // 非ReadOnlyの時はComboBox自体を表示し直す
            if (!this.ReadOnlyLabel)
            {
                base.Visible = true;
                this.RemoveControl(this.label);
                this.ChangeDisplayColor();
                return;
            }

            // 代替Labelを用意
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

            // ComboBox自体を非表示にし、ComboBoxが存在する位置に配置する
            base.Visible = false;
            this.AddControl(this.label);
            this.ChangeDisplayColor();
        }

        /// <summary>
        /// TextBoxまたはLabelへ切り替える。
        /// </summary>
        private void SwitchControl()
        {
            // ComboBoxが表示されており、代替ReadOnlyを利用していない時は切り替えない
            if (this.Visible && !this.ReadOnly && !this.ReadOnlyLabel)
            {
                return;
            }

            if (this.ReadOnly)
            {
                this.switchTextBox();
            }
            if (this.ReadOnlyLabel)
            {
                this.switchLabel();
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
            if (!this.IsDesignMode() && form != null && (form.ActiveControl == this || form.ActiveControl == this.textBox))
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
        /// ロールバック済みかどうかを取得します。
        /// </summary>
        /// <returns>true:ロールバック済み, false:未ロールバック。</returns>
        [Browsable(false)]
        public bool IsRollbacked => this.SelectedItem == this.enterSelectedItem;

        /// <summary>
        /// フォーカスを得た時の値にロールバックを行います。
        /// </summary>
        public void Rollback()
        {
            this.SelectedItem = this.enterSelectedItem;
        }

        /// <summary>
        /// コントロールの生成が完了したことを通知し、ReadOnly, ReadOnlyLabelの制御を行います。
        /// </summary>
        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            if (this.IsDesignMode())
            {
                return;
            }

            this.controlCreated = true;
            this.textBox = new MetTextBox();
            this.textBox.BeginInit();
            this.textBox.EndInit();
            this.label = new Label();
            SwitchControl();
        }

        /// <summary>
        /// コントロールの表示文字が変更時、ラベル・テキスト代替をしていたらラベル・テキストの文字も変更します。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnTextChanged(EventArgs e)
        {
            if (this.controlCreated && this.ReadOnlyLabel)
            {
                this.label.Text = this.Text;
            }
            if (this.controlCreated && this.ReadOnly)
            {
                this.textBox.Text = this.Text;
            }
            base.OnTextChanged(e);
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

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, ComboBoxInfo lParam);

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

            var cbi = new ComboBoxInfo();
            SendMessage(this.Handle, ComboBoxControlMessages.CB_GETCOMBOBOXINFO, IntPtr.Zero, cbi);
            var x = bsz.Width;
            var y = bsz.Height;
            var height = cs.Height - bsz.Height * 2;

            var width = cbi.rcButton.Left - bsz.Width;
            return new Rectangle(x, y, width, height);
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
            for (int x = canvas.Left; x <= canvas.Left + canvas.Width; x++)
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
