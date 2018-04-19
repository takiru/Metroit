using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Metroit.Windows.Forms.Extensions;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// 標準TextBoxを拡張し、新たにいくつかの機能を設けたテキストエリアを提供します。
    /// </summary>
    /// <remarks>
    /// [拡張機能]<br />
    /// 　・コピー、貼り付け、切り取り個別制御。<br />
    /// 　・TextChangeValidation イベントの実装による入力値の可否制御。<br />
    /// 　・ラベルによる代替表示。<br />
    /// 　・最大入力文字後、次コントロールへの自動遷移(ペースト時、コードによる直接入力は行わない)。<br />
    /// 　・フォーカス取得時の全選択。<br />
    /// 　・フォーカス取得時の背景色、文字色の変更。<br />
    /// 　・Multiline時、Ctrl+Aによるテキストの全選択。<br />
    /// <br />
    /// TextChangeValidation イベントは、削除／切り取り／元に戻すなど、一度入力が許容された値に対しての操作や、オートコンプリートの候補値による値設定では発生しません。<br />
    /// そのため、TextChangeValidation イベント内で、状態変更・保持を行うべきではありません。<br />
    /// 状態変更・保持などを行う場合はTextChanged イベント、またはValidated イベントで行うべきです。<br />
    /// TextChangeValidation イベントが発生しない操作<br />
    /// 　・AutoCompleteMode プロパティによって、入力値の補完が行われた時<br />
    /// 　・BackSpace、Deleteキーなどによって、入力値が削除された時<br />
    /// 　・切り取り、Ctrl+Xによって、入力値が切り取られた時<br />
    /// 　・元に戻す、Ctrl+Zによって、入力値が直前の内容に戻された時<br />
    /// </remarks>
    /// <exception cref="Metroit.Windows.Forms.DeniedTextException">
    /// Textプロパティをコード上で書き換え、入力値が拒否された場合に発生します。
    /// </exception>
    [ToolboxItem(true)]
    public class MetTextBox : TextBox, ISupportInitialize, IControlRollback
    {
        /// <summary>
        /// MetTextBox の新しいインスタンスを初期化します。
        /// </summary>
        public MetTextBox()
            : base()
        {
            // デザイン時は制御しない
            if (this.IsDesignMode())
            {
                return;
            }

            // イベントハンドラの追加
            this.Enter += MetTextBox_Enter;
            this.Leave += MetTextBox_Leave;
            this.KeyDown += MetTextBox_KeyDown;
            this.KeyPress += MetTextBox_KeyPress;
            this.KeyUp += MetTextBox_KeyUp;
            this.MouseDown += MetTextBox_MouseDown;
            this.TextChanged += MetTextBox_TextChanged;
        }

        /// <summary>
        /// MetTextBox のインスタンスを破棄します。
        /// </summary>
        ~MetTextBox()
        {
            this.Dispose(true);
        }

        #region イベント

        // マウスクリックによるフォーカス取得かどうか
        private bool isMouseClickFocus = false;

        // IME入力時、何か1文字でも入力値を拒否したかどうか
        private bool isDenyImeKeyChar = false;

        private string enterText = "";

        /// <summary>
        /// フォーカスを得た時、色の変更とテキストの反転を行う。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MetTextBox_Enter(object sender, EventArgs e)
        {
            this.enterText = this.Text;

            // フォーカス取得時の色に変更
            this.changeFocusColor();

            // テキストの反転
            // マウスクリック時は、MouseDownイベントが走行したタイミングで選択が解除されてしまうため、MouseDownイベントで実施する
            if (this.FocusSelect && MouseButtons == MouseButtons.None)
            {
                this.SelectAll();
            }
            if (this.FocusSelect && MouseButtons != MouseButtons.None)
            {
                this.isMouseClickFocus = true;
            }
        }

        /// <summary>
        /// フォーカスを失った時、色の変更を行う。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MetTextBox_Leave(object sender, EventArgs e)
        {
            this.changeBaseColor();
        }

        /// <summary>
        /// キーが離れた時の動作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MetTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            // 入力文字確定時は入力文字拒否フラグは初期化する
            this.isDenyImeKeyChar = false;
        }

        /// <summary>
        /// キーが押された時の動作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MetTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            // Multiline時のCtrl+A
            if (this.Multiline && this.MultilineSelectAll && e.KeyData == (Keys.Control | Keys.A))
            {
                this.SelectAll();
                e.SuppressKeyPress = true;
                return;
            }
        }

        /// <summary>
        /// キーが押された時の動作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MetTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // 文字キー押下時、TextChangingを走行させる。
            // BackSpace, Ctrl+X, Ctrl+Z, Ctrl+Vは何もしない
            if (e.KeyChar == '\b' || e.KeyChar == '\u0018' || e.KeyChar == '\u001a' || e.KeyChar == '\u0016')
            {
                return;
            }

            // Multiline でない時にEnter, Ctrl+Enter, Shift+Enter押下時は何もしない
            if (!this.Multiline && (e.KeyChar == '\r' || e.KeyChar == '\n'))
            {
                return;
            }

            // IME入力時、WM_CHAR, WM_IME_ENDCOMPOSITIONあたりでそれぞれ1文字ずつKeyPressが走行する
            // いずれかの文字が拒否されたら無条件に全文字を拒否する
            if (this.isDenyImeKeyChar)
            {
                e.Handled = true;
                return;
            }

            var inputText = this.createInputString(e.KeyChar.ToString());

            // 入力後のテキストを取得
            var afterText = this.createTextAfterInput(e);

            // TextChangingイベントの発行
            var args = new TextChangeValidationEventArgs();
            args.Cancel = false;
            args.Before = base.Text;
            args.Input = inputText;
            args.After = afterText;
            this.OnTextChangeValidation(args);

            // キャンセルされたら入力を拒否する
            if (args.Cancel)
            {
                this.isDenyImeKeyChar = true;
                e.Handled = true;
            }
        }

        /// <summary>
        /// マウスがクリックされてフォーカスを得た時、テキストの反転を行う。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MetTextBox_MouseDown(object sender, MouseEventArgs e)
        {
            // テキストの反転
            if (this.FocusSelect && this.isMouseClickFocus)
            {
                this.SelectAll();
                this.isMouseClickFocus = false;
            }
        }

        /// <summary>
        /// テキストが変更された時の動作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MetTextBox_TextChanged(object sender, EventArgs e)
        {
            // コードによるText変更は何も制御しない
            if (this.isTextDirect)
            {
                this.isTextDirect = false;
                return;
            }

            // 貼り付けによるText変更は何も制御しない
            if (this.isTextPate)
            {
                this.isTextPate = false;
                return;
            }
        }

        /// <summary>
        /// 文字キーによってテキスト入力が行われた際の、入力後テキストを取得する。
        /// </summary>
        /// <param name="e">キーイベント</param>
        /// <returns>入力後テキスト</returns>
        private string createTextAfterInput(KeyPressEventArgs e)
        {
            var headText = (this.SelectionStart == 0 ? "" : base.Text.Substring(0, SelectionStart));
            var footText = base.Text.Substring(SelectionStart + SelectionLength);

            // Enterが押されたらCRLFに変換
            var inputText = this.createInputString(e.KeyChar.ToString());

            if (e.KeyChar == '\r' || e.KeyChar == '\n')
            {
                inputText = Environment.NewLine;
            }
            return headText + inputText + footText;
        }

        /// <summary>
        /// オートフォーカスを行う。
        /// </summary>
        private void autoFocus()
        {
            if (this.AutoFocus && this.Focused && this.CanAutoFocus())
            {
                this.MoveNextControl();
            }
        }

        /// <summary>
        /// オートフォーカスが実行可能な状態かどうかを取得します。
        /// </summary>
        /// <returns>true:実行可能, false:実行不可能</returns>
        protected virtual bool CanAutoFocus()
        {
            if (this.MaxLength != this.Text.Length)
            {
                return false;
            }

            return true;
        }

        #endregion

        #region 追加イベント

        /// <summary>
        /// <para>Text プロパティの値が追加／挿入されているときに発生するイベントです。値を検証し、変更を拒否することができます。</para>
        /// <para>値の削除や元に戻す操作では発生しないことに注意してください。</para>
        /// </summary>
        [MetCategory("MetPropertyChange")]
        [MetDescription("MetTextBoxTextChangeValidationEventHandler")]
        public event TextChangeValidationEventHandler TextChangeValidation;

        /// <summary>
        /// 入力値を変更しているときに走行します。
        /// </summary>
        /// <param name="e">入力値許可イベントオブジェクト。</param>
        protected virtual void OnTextChangeValidation(TextChangeValidationEventArgs e)
        {
            if (this.TextChangeValidation != null)
            {
                this.TextChangeValidation(this, e);
            }
        }

        #endregion

        #region プロパティ

        private bool visible = true;

        /// <summary>
        /// コントロールがバインドされるコンテナーの端を取得または設定し、親のサイズ変更時に、コントロールのサイズがどのように変化するかを決定します。
        /// </summary>
        public override AnchorStyles Anchor
        {
            get => base.Anchor;
            set
            {
                base.Anchor = value;
                if (this.initialized && this.ReadOnlyLabel)
                {
                    this.label.Anchor = this.Anchor;
                }
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
                if (!this.initialized)
                {
                    return;
                }
                this.switchLabel();
            }
        }

        /// <summary>
        /// コントロールに関連付けられたテキストです。
        /// </summary>
        public override string Text
        {
            get => base.Text;
            set
            {
                // 設定値に変更がない場合は処理しない
                if (this.Text == value)
                {
                    return;
                }

                // 入力値チェックイベント
                var inputText = this.createInputString(value);
                var e = new TextChangeValidationEventArgs();
                e.Cancel = false;
                e.Before = base.Text;
                e.Input = inputText;
                e.After = inputText;
                this.OnTextChangeValidation(e);
                if (e.Cancel)
                {
                    throw new DeniedTextException(
                            string.Format(ExceptionResources.GetString("InvalidValue"), inputText), inputText);
                }

                base.Text = value;

                if (this.IsDesignMode())
                {
                    return;
                }
                if (this.ReadOnlyLabel)
                {
                    this.label.Text = base.Text;
                }
            }
        }

        /// <summary>
        /// 取得またはテキスト ボックスにテキストが読み取り専用かどうかを示す値を設定します。
        /// </summary>
        public new bool ReadOnly
        {
            get => base.ReadOnly;
            set
            {
                if (value && this.ReadOnlyLabel)
                {
                    this.ReadOnlyLabel = false;
                }
                base.ReadOnly = value;

                // 背景色・文字色の変更
                this.ChangeDisplayColor();
            }
        }

        // FIXED: 例えば特定条件下で色を変えるなどのユーザー実装が行えなくなるため、コード実装は可能とする。
        /// <summary>
        /// コントロールの前景色を取得または特定のプロパティによって強制的に前景色を設定します。
        /// 意図的に前景色の変更が必要な場合のみ利用してください。
        /// </summary>
        [Browsable(false)]
        [ReadOnly(true)]
        //[EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Color ForeColor
        {
            get => base.ForeColor;
            set => base.ForeColor = value;
        }

        /// <summary>
        /// コントロールの背景色を取得または特定のプロパティによって強制的に背景色を設定します。
        /// 意図的に背景色の変更が必要な場合のみ利用してください。
        /// </summary>
        [Browsable(false)]
        [ReadOnly(true)]
        //[EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Color BackColor
        {
            get => base.BackColor;
            set => base.ForeColor = value;
        }

        #endregion

        #region 追加プロパティ

        private bool initialized = false;
        private Color baseForeColor = SystemColors.WindowText;
        private Color defaultBaseBackColor => this.ReadOnly ? SystemColors.Control : SystemColors.Window;   // BaseBackColor の既定値
        private Color? baseBackColor = null;
        private Color focusForeColor = SystemColors.WindowText;
        private Color defaultFocusBackColor => this.ReadOnly ? SystemColors.Control : SystemColors.Window;  // FocusBackColor の既定値
        private Color? focusBackColor = null;
        private bool readOnlyLabel = false;
        private Label label = null;

        /// <summary>
        /// InitializeComponent() によってコントロールの準備が完了したかどうかを取得します。
        /// </summary>
        protected bool Initialized { get => this.initialized; }

        /// <summary>
        /// Label による代替表示を行うかどうかを取得または設定します。
        /// </summary>
        [Browsable(true)]
        [MetCategory("MetAppearance")]
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
                if (value && this.ReadOnly)
                {
                    this.ReadOnly = false;
                }
                this.readOnlyLabel = value;

                if (this.IsDesignMode())
                {
                    return;
                }
                if (!this.initialized)
                {
                    return;
                }

                this.switchLabel();
            }
        }

        /// <summary>
        /// コントロールの前景色を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [MetCategory("MetAppearance")]
        [DefaultValue(typeof(Color), "WindowText")]
        [MetDescription("ControlBaseForeColor")]
        public Color BaseForeColor
        {
            get { return this.baseForeColor; }
            set
            {
                this.baseForeColor = value;

                // 背景色・文字色の変更
                this.ChangeDisplayColor();
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
        /// BaseBackColor が既定値から変更されたかどうかを返却する。
        /// </summary>
        /// <returns>true:変更された, false:変更されていない</returns>
        private bool ShouldSerializeBaseBackColor() => this.baseBackColor != null && this.baseBackColor != this.defaultBaseBackColor;

        /// <summary>
        /// BaseBackColor のリセット操作を行う。
        /// </summary>
        private void ResetBaseBackColor() => this.baseBackColor = null;

        /// <summary>
        /// フォーカス時のコントロールの前景色を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [MetCategory("MetAppearance")]
        [DefaultValue(typeof(Color), "WindowText")]
        [MetDescription("ControlFocusForeColor")]
        public Color FocusForeColor
        {
            get { return this.focusForeColor; }
            set
            {
                this.focusForeColor = value;

                // 背景色・文字色の変更
                this.ChangeDisplayColor();
            }

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
        /// FocusBackColor が既定値から変更されたかどうかを返却する。
        /// </summary>
        /// <returns>true:変更された, false:変更されていない</returns>
        private bool ShouldSerializeFocusBackColor() => this.focusBackColor != null && this.focusBackColor != this.defaultFocusBackColor;

        /// <summary>
        /// FocusBackColor のリセット操作を行う。
        /// </summary>
        private void ResetFocusBackColor() => this.focusBackColor = null;

        /// <summary>
        /// フォーカスを得た時、テキストを反転させるかを設定します。
        /// </summary>
        [Browsable(true)]
        [MetCategory("MetBehavior")]
        [DefaultValue(false)]
        [MetDescription("ControlFocusSelect")]
        public bool FocusSelect { get; set; } = false;

        /// <summary>
        /// 複数行テキストエディットの時、Ctrl+Aを押すことでテキストを全選択するかを設定します。
        /// </summary>
        [Browsable(true)]
        [MetCategory("MetBehavior")]
        [DefaultValue(false)]
        [MetDescription("MetTextBoxMultilineSelectAll")]
        public bool MultilineSelectAll { get; set; } = false;

        /// <summary>
        /// 最大入力まで行われた時、次のコントロールへフォーカスを遷移するかを設定します。
        /// </summary>
        [Browsable(true)]
        [MetCategory("MetBehavior")]
        [DefaultValue(false)]
        [MetDescription("ControlAutoFocus")]
        public bool AutoFocus { get; set; } = false;

        /// <summary>
        /// 表示色をフォーカス色に変更する。
        /// </summary>
        private void changeFocusColor()
        {
            base.ForeColor = this.FocusForeColor;
            base.BackColor = this.FocusBackColor;
        }

        /// <summary>
        /// 表示色を非フォーカス色に変更する。
        /// </summary>
        private void changeBaseColor()
        {
            base.ForeColor = this.BaseForeColor;
            base.BackColor = this.BaseBackColor;

            // ラベルの代替表示を行っている場合はラベルの表示色も変更
            if (this.ReadOnlyLabel)
            {
                this.label.ForeColor = this.BaseForeColor;
                this.label.BackColor = this.BaseBackColor;
            }
        }

        /// <summary>
        /// コントロールのアクティブ状態に応じた表示色に変更する。
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
        /// 表示状態に応じて、Visible、ラベル代替を切り替える。
        /// </summary>
        private void switchLabel()
        {
            // 非表示
            if (!this.Visible)
            {
                // ラベル代替なしの場合は通常のVisible
                if (!this.ReadOnlyLabel)
                {
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
                base.Visible = this.Visible;
                return;
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
            this.label.BackColor = this.BaseBackColor;
            this.label.ForeColor = this.BaseForeColor;
            this.label.Font = this.Font;
            this.label.Anchor = this.Anchor;
            this.label.RightToLeft = this.RightToLeft;
            this.label.BorderStyle = this.BorderStyle;
            switch (this.TextAlign)
            {
                case HorizontalAlignment.Center:
                    this.label.TextAlign = Multiline ?
                            ContentAlignment.TopCenter : ContentAlignment.MiddleCenter;
                    break;

                case HorizontalAlignment.Left:
                    this.label.TextAlign = Multiline ?
                            ContentAlignment.TopLeft : ContentAlignment.MiddleLeft;
                    break;

                case HorizontalAlignment.Right:
                    this.label.TextAlign = Multiline ?
                            ContentAlignment.TopRight : ContentAlignment.MiddleRight;
                    break;
            }

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

        #endregion

        #region メソッド

        private bool isTextPate = false;
        private bool isTextDirect = false;

        /// <summary>
        /// InitializeComponent()でコントロールの初期化が完了していないことを通知します。
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void BeginInit()
        {
            this.initialized = false;
        }

        /// <summary>
        /// InitializeComponent()でコントロールの初期化が完了したことを通知し、ReadOnlyLabelの制御を行います。
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void EndInit()
        {
            if (this.IsDesignMode())
            {
                return;
            }
            this.initialized = true;

            this.switchLabel();

        }

        /// <summary>
        /// ロールバック済みかどうかを取得します。
        /// </summary>
        /// <param name="sender">ロールバック指示オブジェクト。</param>
        /// <param name="control">ロールバック対象オブジェクト。</param>
        /// <returns>true:ロールバック済み, false:未ロールバック。</returns>
        public virtual bool IsRollbacked(object sender, Control control)
        {
            return this.Text == this.enterText;
        }

        /// <summary>
        /// フォーカスを得た時の値にロールバックを行います。
        /// </summary>
        /// <param name="sender">ロールバック指示オブジェクト。</param>
        /// <param name="control">ロールバック対象オブジェクト。</param>
        public virtual void Rollback(object sender, Control control)
        {
            this.Text = this.enterText;
        }

        /// <summary>
        /// テキストが変更後、可能の場合にフォーカス遷移を行います。
        /// </summary>
        /// <param name="e">イベントオブジェクト。</param>
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);

            // オートコンプリート指定がある場合はオートフォーカスしない
            if (this.AutoCompleteMode != AutoCompleteMode.None)
            {
                return;
            }
            this.autoFocus();
        }

        /// <summary>
        /// コントロールの配置元が変更時、ラベル代替表示をしていたらラベルの配置元も変更します。
        /// </summary>
        /// <param name="e">イベントオブジェクト。</param>
        protected override void OnParentChanged(EventArgs e)
        {
            if (this.initialized && this.ReadOnlyLabel)
            {
                this.label.Parent = this.Parent;
            }

            base.OnParentChanged(e);
        }

        /// <summary>
        /// コントロールの表示位置が変更時、ラベル代替表示をしていたらラベルの表示位置も変更します。
        /// </summary>
        /// <param name="e">イベントオブジェクト。</param>
        protected override void OnLocationChanged(EventArgs e)
        {
            if (this.initialized && this.ReadOnlyLabel)
            {
                this.label.Location = this.Location;
            }
            base.OnLocationChanged(e);
        }

        /// <summary>
        /// コントロールのサイズが変更時、ラベル代替表示をしていたらラベルのサイズも変更します。
        /// </summary>
        /// <param name="e">イベントオブジェクト。</param>
        protected override void OnSizeChanged(EventArgs e)
        {
            if (this.initialized && this.ReadOnlyLabel)
            {
                this.label.Size = this.Size;
            }
            base.OnSizeChanged(e);
        }

        /// <summary>
        /// コントロールのサイズが変更時、ラベル代替表示をしていたらラベルのサイズも変更します。
        /// </summary>
        /// <param name="e">イベントオブジェクト。</param>
        protected override void OnResize(EventArgs e)
        {
            if (this.initialized && this.ReadOnlyLabel)
            {
                this.label.Size = this.Size;
            }
            base.OnResize(e);
        }

        /// <summary>
        /// コントロールのドッキングが変更時、ラベル代替表示をしていたらラベルのドッキングも変更します。
        /// </summary>
        /// <param name="e">イベントオブジェクト。</param>
        protected override void OnDockChanged(EventArgs e)
        {
            if (this.initialized && this.ReadOnlyLabel)
            {
                this.label.Dock = this.Dock;
            }
            base.OnDockChanged(e);
        }

        /// <summary>
        /// コントロールの文字入力方向が変更時、ラベル代替表示をしていたらラベルの文字入力方向も変更します。
        /// </summary>
        /// <param name="e">イベントオブジェクト。</param>
        protected override void OnRightToLeftChanged(EventArgs e)
        {
            if (this.initialized && this.ReadOnlyLabel)
            {
                this.label.RightToLeft = this.RightToLeft;
            }
            base.OnRightToLeftChanged(e);
        }

        /// <summary>
        /// コントロールのフォントが変更時、ラベル代替表示をしていたらラベルのフォントも変更します。
        /// </summary>
        /// <param name="e">イベントオブジェクト。</param>
        protected override void OnFontChanged(EventArgs e)
        {
            if (this.initialized && this.ReadOnlyLabel)
            {
                this.label.Font = this.Font;
            }
            base.OnFontChanged(e);
        }

        /// <summary>
        /// コントロールのマージンが変更時、ラベル代替表示をしていたらラベルのマージンも変更します。
        /// </summary>
        /// <param name="e">イベントオブジェクト。</param>
        protected override void OnMarginChanged(EventArgs e)
        {
            if (this.initialized && this.ReadOnlyLabel)
            {
                this.label.Margin = this.Margin;
            }
            base.OnMarginChanged(e);
        }

        /// <summary>
        /// コントロールのボーダーが変更時、ラベル代替表示をしていたらラベルのボーダーも変更します。
        /// </summary>
        /// <param name="e">イベントオブジェクト。</param>
        protected override void OnBorderStyleChanged(EventArgs e)
        {
            if (this.initialized && this.ReadOnlyLabel)
            {
                this.label.BorderStyle = this.BorderStyle;
            }
            base.OnBorderStyleChanged(e);
        }

        /// <summary>
        /// コントロールの文字表示位置が変更時、ラベル代替表示をしていたらラベルの文字表示位置も変更します。
        /// </summary>
        /// <param name="e">イベントオブジェクト。</param>
        protected override void OnTextAlignChanged(EventArgs e)
        {
            if (this.initialized && this.ReadOnlyLabel)
            {
                switch (this.TextAlign)
                {
                    case HorizontalAlignment.Center:
                        this.label.TextAlign = Multiline ?
                                ContentAlignment.TopCenter : ContentAlignment.MiddleCenter;
                        break;

                    case HorizontalAlignment.Left:
                        this.label.TextAlign = Multiline ?
                                ContentAlignment.TopLeft : ContentAlignment.MiddleLeft;
                        break;

                    case HorizontalAlignment.Right:
                        this.label.TextAlign = Multiline ?
                                ContentAlignment.TopRight : ContentAlignment.MiddleRight;
                        break;
                }
            }
            base.OnTextAlignChanged(e);
        }

        /// <summary>
        /// ウィンドウメッセージを捕捉し、コードによる値の設定および貼り付け時の制御を行います。
        /// </summary>
        /// <param name="m">ウィンドウメッセージ。</param>
        protected override void WndProc(ref Message m)
        {
            // ウィンドウタイトルやコントロールのテキストを設定
            const int WM_SETTEXT = 0x000C;

            // エディットボックスまたはコンボボックスの現在のキャレット位置にクリップボードのテキストを挿入する時のメッセージ。
            const int WM_PASTE = 0x0302;

            // コードによるText値変更
            if (m.Msg == WM_SETTEXT)
            {
                this.isTextDirect = true;
            }

            // 貼り付け
            if (m.Msg == WM_PASTE)
            {
                // 拒否されたら貼り付けを行わない
                if (!this.canPaste())
                {
                    return;
                }
                this.isTextPate = true;
            }

            base.WndProc(ref m);
        }

        /// <summary>
        /// 貼り付けが可能かどうかを取得する。
        /// </summary>
        /// <returns>true:貼付成功, false:貼付失敗</returns>
        private bool canPaste()
        {
            // 貼り付け可能なクリップボードが存在しない場合は処理しない
            var inputText = Clipboard.GetText();
            if (string.IsNullOrWhiteSpace(inputText))
            {
                return false;
            }

            // 貼り付け値の入力検証処理
            // マルチラインじゃない場合はクリップボードの1行目のみを処理する
            if (!Multiline)
            {
                int newLinePosition = inputText.IndexOf(Environment.NewLine);
                if (newLinePosition > -1)
                {
                    inputText = inputText.Substring(0, newLinePosition);
                }
            }

            inputText = this.createInputString(inputText);
            var afterText = this.createTextAfterPasted(inputText);
            var e = new TextChangeValidationEventArgs();
            e.Cancel = false;
            e.Before = base.Text;
            e.Input = inputText;
            e.After = afterText;
            this.OnTextChangeValidation(e);

            // 入力を拒否された場合は処理しない
            if (e.Cancel)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 入力文字を生成する。
        /// </summary>
        /// <param name="inputText">入力文字</param>
        /// <returns>生成後の入力文字</returns>
        private string createInputString(string inputText)
        {
            if (this.CharacterCasing == CharacterCasing.Lower)
            {
                return inputText.ToLower();
            }
            if (this.CharacterCasing == CharacterCasing.Upper)
            {
                return inputText.ToUpper();
            }

            return inputText;
        }

        /// <summary>
        /// ペースト操作によってテキストペーストが行われた後のテキストを取得する。
        /// </summary>
        /// <param name="pasteText">ペースト文字列</param>
        /// <returns>ペースト後テキスト</returns>
        private string createTextAfterPasted(string pasteText)
        {
            var afterText = new StringBuilder();

            afterText.Append((this.SelectionStart == 0 ? "" : base.Text.Substring(0, this.SelectionStart)));
            afterText.Append(pasteText);
            afterText.Append(base.Text.Substring(this.SelectionStart + this.SelectionLength));

            return afterText.ToString();
        }

        /// <summary>
        /// オブジェクトの破棄を行います。
        /// </summary>
        /// <param name="disposing">マネージリソースの破棄を行うかどうか。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.label != null)
                {
                    this.label.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        #endregion

    }

}
