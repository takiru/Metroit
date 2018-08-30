using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Metroit.Windows.Forms.Extensions;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// Windows コンボ ボックス コントロールを表します。
    /// </summary>
    [ToolboxItem(true)]
    public class MetComboBox : ComboBox, IControlRollback
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
            enterSelectedItem = this.SelectedItem;
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

        /// <summary>
        /// ComboBox コントロールの背景色を示す値を取得または設定します。
        /// </summary>
        [Browsable(false)]
        public override Color BackColor { get => base.BackColor; set => base.BackColor = value; }

        /// <summary>
        /// ComboBox コントロールの文字色を示す値を取得または設定します。
        /// </summary>
        [Browsable(false)]
        public override Color ForeColor { get => base.ForeColor; set => base.ForeColor = value; }

        #endregion

        #region 追加プロパティ

        private TextBox textBox = null;
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
                this.RemoveControl(this.textBox);
                this.ChangeDisplayColor();
                return;
            }

            // 非ReadOnlyの時はComboBox自体を表示し直す
            if (!this.ReadOnly)
            {
                base.Visible = true;
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
            this.textBox.BorderStyle = BorderStyle.Fixed3D;
            this.textBox.Cursor = this.Cursor;
            this.textBox.TextAlign = HorizontalAlignment.Left;
            this.textBox.TabIndex = this.TabIndex;
            this.textBox.TabStop = this.TabStop;
            this.ChangeDisplayColor();

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
            this.label.BorderStyle = BorderStyle.Fixed3D;
            this.label.Cursor = this.Cursor;
            this.label.TextAlign = ContentAlignment.MiddleLeft;
            this.ChangeDisplayColor();

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

        #endregion

        #region メソッド

        private bool controlCreated = false;

        /// <summary>
        /// ロールバック済みかどうかを取得します。
        /// </summary>
        /// <param name="sender">ロールバック指示オブジェクト。</param>
        /// <param name="control">ロールバック対象オブジェクト。</param>
        /// <returns>true:ロールバック済み, false:未ロールバック。</returns>
        public bool IsRollbacked(object sender, Control control)
        {
            return this.SelectedItem == this.enterSelectedItem;
        }

        /// <summary>
        /// フォーカスを得た時の値にロールバックを行います。
        /// </summary>
        /// <param name="sender">ロールバック指示オブジェクト。</param>
        /// <param name="control">ロールバック対象オブジェクト。</param>
        public void Rollback(object sender, Control control)
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
            this.textBox = new TextBox();
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

        #endregion
    }
}
