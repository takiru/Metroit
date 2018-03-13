using Metroit.Windows.Forms.Extensions;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// 標準Formを拡張し、新たにいくつかの機能を設けたフォームを提供します。
    /// </summary>
    public partial class MetForm : Form
    {
        // Closingイベントを発生させるための画面終了用のSendMessage
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        /// <summary>
        /// MetForm の新しいインスタンスを初期化します。
        /// </summary>
        public MetForm()
            : base()
        {
            InitializeComponent();
        }

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
            this.EscPush.ControlRollback = false;
            this.EscPush.ControlLeave = false;
            this.EscPush.FormClose = false;
        }
        
        /// <summary>
        /// 呼び元からのリクエストパラメータを取得または設定します。
        /// </summary>
        protected object Request { get; private set; } = null;

        /// <summary>
        /// 呼び元へのレスポンスパラメータを設定し、呼び元で取得可能にします。
        /// </summary>
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
            if (EnterFocus && keyData == Keys.Return)
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
                var rollback = GetLeaveRollbackObject();
                var isRollbacked = IsRollbacked(rollback);

                // ControlRollback
                if (this.RollbackControl(rollback, isRollbacked))
                {
                    return true;
                }

                // ControlLeave
                if (this.LeaveControl(isRollbacked))
                {
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
        /// ILeaveRollback オブジェクトを取得する。
        /// </summary>
        /// <returns>ILeaveRollback オブジェクト。</returns>
        private IControlRollback GetLeaveRollbackObject()
        {
            // 対象コントロールにILeaveRollback が実装されていない場合は、フォームに実装されているかまで見る
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

            return leaveRollback.IsRollbacked(this, this.ActiveControl);
        }

        /// <summary>
        /// コントロールのロールバックを行う。
        /// </summary>
        /// <param name="leaveRollback">ILeaveRollback オブジェクト。</param>
        /// <param name="isRollbacked">ロールバック済みかどうか。</param>
        /// <returns>true:ロールバックの実施, false:ロールバックの未実施。</returns>
        private bool RollbackControl(IControlRollback leaveRollback, bool isRollbacked)
        {
            if (!this.EscPush.ControlRollback)
            {
                return false;
            }
            if (this.ActiveControl == null)
            {
                return false;
            }

            // ロールバック済みなら処理しない
            if (isRollbacked)
            {
                return false;
            }
            leaveRollback.Rollback(this, this.ActiveControl);
            return true;
        }

        /// <summary>
        /// コントロールのフォーカスアウトを行う。
        /// </summary>
        /// <param name="isRollbacked">ロールバック済みかどうか。</param>
        /// <returns>true:フォーカスアウトの実施, false:フォーカスアウトの未実施。</returns>
        private bool LeaveControl(bool isRollbacked)
        {
            if (!this.EscPush.ControlLeave)
            {
                return false;
            }
            if (this.ActiveControl == null)
            {
                return false;
            }

            // ロールバック済みでない場合は処理しない
            if (this.EscPush.ControlRollback && !isRollbacked)
            {
                return false;
            }

            // 一時的にボタンを用意して、ボタンにフォーカス遷移させる
            using (var hiddenButton = new Button())
            {
                hiddenButton.Name = Guid.NewGuid().ToString("N").Substring(0, 10);
                hiddenButton.Size = new System.Drawing.Size(0, 0);
                this.Controls.Add(hiddenButton);

                hiddenButton.Focus();

                this.ActiveControl = null;
                this.Controls.Remove(hiddenButton);
            }
            return true;
        }

        /// <summary>
        /// フォームを閉じる。
        /// </summary>
        /// <returns>true:フォーム終了の実施, false:フォーム終了の未実施。</returns>
        private bool CloseForm()
        {
            // システムメニューアイテム選択
            const int WM_SYSCOMMAND = 0x0112;

            // 画面を閉じる
            const int SC_CLOSE = 0xF060;

            if (!this.EscPush.FormClose)
            {
                return false;
            }

            MetForm.SendMessage(this.Handle, WM_SYSCOMMAND, SC_CLOSE, 0);
            return true;
        }

        #endregion        
    }
}
