using Metroit.Win32.Api;
using Metroit.Win32.DesktopAppUi.MenuRc;
using Metroit.Windows.Forms.Extensions;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// 標準Formを拡張し、新たにいくつかの機能を設けたフォームを提供します。
    /// </summary>
    public partial class MetForm : Form
    {
        /// <summary>
        /// MetForm の新しいインスタンスを初期化します。
        /// </summary>
        public MetForm()
            : base()
        {
            InitializeComponent();
        }

        #region 追加イベント

        /// <summary>
        /// ESCキーによってロールバックを行う時に発生するイベントです。
        /// </summary>
        [MetCategory("MetBehavior")]
        [MetDescription("MetFormControlRollbacking")]
        public event CancelEventHandler ControlRollbacking;

        /// <summary>
        /// ESCキーによってフォーカスを失う時に発生するイベントです。
        /// </summary>
        [MetCategory("MetBehavior")]
        [MetDescription("MetFormControlLeaving")]
        public event CancelEventHandler ControlLeaving;

        #endregion

        #region 追加プロパティ

        /// <summary>
        /// Enterキーが押された時にフォーカスを遷移するかを取得または設定します。
        /// </summary>
        /// <remarks>
        /// TextBox の AcceptsTabがtrueの時はフォーカス遷移をせず、 TextBox にタブが入力されます。
        /// </remarks>
        [Browsable(true)]
        [MetCategory("MetBehavior")]
        [DefaultValue(false)]
        [MetDescription("ControlEnterFocus")]
        public bool EnterFocus { get; set; } = false;

        /// <summary>
        /// ESCキーが押された時の動作を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [MetCategory("MetBehavior")]
        [MetDescription("ControlEscPush")]
        public FormEscapeBehavior EscPush { get; set; } = new FormEscapeBehavior();

        /// <summary>
        /// EscPush が既定値から変更されたかどうかを返却する。
        /// </summary>
        /// <returns>true:変更された, false:変更されていない</returns>
        private bool ShouldSerializeEscPush()
        {
            return this.EscPush.ControlRollback | this.EscPush.ControlLeave | this.EscPush.FormClose;
        }

        /// <summary>
        /// EscPush のリセット操作を行う。
        /// </summary>
        private void ResetEscPush()
        {
            this.EscPush = new FormEscapeBehavior();
        }

        /// <summary>
        /// 呼び元からのリクエストパラメータを取得または設定します。
        /// </summary>
        [Browsable(false)]
        protected object Request { get; private set; } = null;

        /// <summary>
        /// 呼び元へのレスポンスパラメータを設定し、呼び元で取得可能にします。
        /// </summary>
        [Browsable(false)]
        public object Response { get; protected set; } = null;

        #endregion

        #region 追加メソッド

        /// <summary>
        /// リクエストパラメーターを送り、コントロールをユーザーに対して表示します。
        /// </summary>
        /// <param name="request">リクエストパラメーター。</param>
        public void Show(object request)
        {
            this.Request = request;
            base.Show();
        }

        /// <summary>
        /// リクエストパラメーターを送り、所有側フォームを指定してフォームをユーザーに表示します。
        /// </summary>
        /// <param name="owner">親ウィンドウオブジェクト</param>
        /// <param name="request">リクエストパラメータ</param>
        /// <remarks>パラメータを渡し、モードレスウィンドウを表示する。</remarks>
        public void Show(IWin32Window owner, object request)
        {
            this.Request = request;
            base.Show(owner);
        }

        /// <summary>
        /// リクエストパラメーターを送り、フォームをモーダル ダイアログ ボックスとして表示します。
        /// </summary>
        /// <param name="request">リクエストパラメータ</param>
        /// <returns>パラメータを渡し、モーダルウィンドウを表示する。</returns>
        public DialogResult ShowDialog(object request)
        {
            this.Request = request;
            return base.ShowDialog();
        }

        /// <summary>
        /// リクエストパラメーターを送り、指定した所有者を持つモーダル ダイアログ ボックスとしてフォームを表示します。
        /// </summary>
        /// <param name="owner">親ウィンドウオブジェクト</param>
        /// <param name="request">リクエストパラメータ</param>
        /// <returns>パラメータを渡し、モーダルウィンドウを表示する。</returns>
        public DialogResult ShowDialog(IWin32Window owner, object request)
        {
            this.Request = request;
            return base.ShowDialog(owner);
        }

        #endregion

        #region メソッド

        /// <summary>
        /// Enterキー、ESCキーの制御を行います。
        /// </summary>
        /// <param name="msg">Messageオブジェクト</param>
        /// <param name="keyData">Keysオブジェクト</param>
        /// <returns>true:以降のイベントハンドラを制御しない, false:以降のイベントハンドラを制御する</returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // Enter
            if (EnterFocus && (keyData == Keys.Return || keyData == (Keys.Shift | Keys.Return)))
            {
                // 対象がボタンの時は通常制御とする
                if (this.ActiveControl is Button)
                {
                    return base.ProcessCmdKey(ref msg, keyData);
                }
                this.MoveNextControl();
                return true;
            }

            // ESC
            if (keyData == Keys.Escape)
            {
                var rollback = GetControlRollbackObject();
                var isRollbacked = IsRollbacked(rollback);

                // ControlRollback
                if (this.IsRollbackControl(rollback, isRollbacked))
                {
                    var e = new CancelEventArgs();
                    this.OnControlRollbacking(e);
                    if (e.Cancel)
                    {
                        return false;
                    }
                    rollback.Rollback();
                    return true;
                }

                // ControlLeave
                if (this.IsLeaveControl(isRollbacked))
                {
                    var e = new CancelEventArgs();
                    this.OnControlLeaving(e);
                    if (e.Cancel)
                    {
                        return false;
                    }
                    this.LeaveControl();
                    return true;
                }

                // FormClose
                if (this.CloseForm())
                {
                    return true;
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        /// <summary>
        /// IControlRollback オブジェクトを取得する。
        /// </summary>
        /// <returns>IControlRollback オブジェクト。</returns>
        private IControlRollback GetControlRollbackObject()
        {
            // 対象コントロールにIControlRollback が実装されていない場合は、フォームに実装されているかまで見る
            var rollback = this.ActiveControl as IControlRollback;
            if (rollback == null)
            {
                rollback = this as IControlRollback;
            }
            return rollback;
        }

        /// <summary>
        /// 対象コントロールのロールバックが済んでいるかどうかを取得する。
        /// </summary>
        /// <param name="leaveRollback">ILeaveRollback オブジェクト</param>
        /// <returns>true:ロールバック済み, false:未ロールバック。</returns>
        private bool IsRollbacked(IControlRollback leaveRollback)
        {
            // ILeaveRollback インターフェースを実装していなかったらロールバック済みとみなす
            if (leaveRollback == null)
            {
                return true;
            }

            return leaveRollback.IsRollbacked;
        }

        /// <summary>
        /// ロールバック対象かどうかを取得する。
        /// </summary>
        /// <param name="leaveRollback">ILeaveRollback オブジェクト。</param>
        /// <param name="isRollbacked">ロールバック済みかどうか。</param>
        /// <returns>true:対象, false:対象外。</returns>
        private bool IsRollbackControl(IControlRollback leaveRollback, bool isRollbacked)
        {
            // ロールバックを実施しない場合は対象外
            if (!this.EscPush.ControlRollback)
            {
                return false;
            }

            // コントロールを有していない場合は対象外
            if (this.ActiveControl == null)
            {
                return false;
            }

            // ロールバック済みの場合は対象外
            if (isRollbacked)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// コントロールのロールバックを行う前のイベントを発生させます。
        /// </summary>
        /// <param name="e">CancelEventArgs オブジェクト。</param>
        protected virtual void OnControlRollbacking(CancelEventArgs e)
        {
            this.ControlRollbacking?.Invoke(this, e);
        }

        /// <summary>
        /// フォーカスアウト対象かどうかを取得する。
        /// </summary>
        /// <param name="isRollbacked">ロールバック済みかどうか。</param>
        /// <returns>true:対象, false:対象外。</returns>
        private bool IsLeaveControl(bool isRollbacked)
        {
            // フォーカスアウトを実施しない場合は対象外
            if (!this.EscPush.ControlLeave)
            {
                return false;
            }

            // コントロールを有していない場合は対象外
            if (this.ActiveControl == null)
            {
                return false;
            }

            // ロールバック済みでない場合は対象外
            if (this.EscPush.ControlRollback && !isRollbacked)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// コントロールのフォーカスを行う前のイベントを発生させます。
        /// </summary>
        /// <param name="e">CancelEventArgs オブジェクト。</param>
        protected virtual void OnControlLeaving(CancelEventArgs e)
        {
            this.ControlLeaving?.Invoke(this, e);
        }

        /// <summary>
        /// コントロールのフォーカスアウトを行う。
        /// </summary>
        private void LeaveControl()
        {
            if (!this.ActiveControl.CausesValidation)
            {
                this.ActiveControl = null;
                return;
            }

            if (this.Validate())
            {
                this.ActiveControl = null;
            }
        }

        /// <summary>
        /// フォームを閉じる。
        /// </summary>
        /// <returns>true:フォーム終了の実施, false:フォーム終了の未実施。</returns>
        private bool CloseForm()
        {
            if (!this.EscPush.FormClose)
            {
                return false;
            }

            User32.SendMessage(this.Handle, WindowMessage.WM_SYSCOMMAND, new IntPtr(SystemCommandTypes.SC_CLOSE), IntPtr.Zero);
            return true;
        }

        #endregion        
    }
}
