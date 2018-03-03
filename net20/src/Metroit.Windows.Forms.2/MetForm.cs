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
        [DefaultValue(FormEscapeBehavior.None)]
        [MetDescription("ControlEscPush")]
        public FormEscapeBehavior EscPush { get; set; } = FormEscapeBehavior.None;

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
            // システムメニューアイテム選択
            const int WM_SYSCOMMAND = 0x0112;

            // 画面を閉じる
            const int SC_CLOSE = 0xF060;

            // Enter
            if (EnterFocus && keyData == Keys.Return)
            {
                // 対象がボタンの時は通常制御とする
                if (this.ActiveControl is Button)
                {
                    return false;
                }
                ControlExtensions.MoveNextControl(this);
                return true;
            }

            // ESC
            if (keyData == Keys.Escape)
            {
                // コントロールのアクティブ状態に応じて動作
                if (this.ActiveControl == null)
                {
                    if (this.EscPush == FormEscapeBehavior.FormClose || this.EscPush == FormEscapeBehavior.Both)
                    {
                        MetForm.SendMessage(this.Handle, WM_SYSCOMMAND,SC_CLOSE, 0);
                        return true;
                    }
                }
                else
                {
                    if (this.EscPush == FormEscapeBehavior.ControlLeave || this.EscPush == FormEscapeBehavior.Both)
                    {
                        this.ActiveControl = null;
                        return true;
                    }
                    if (this.EscPush == FormEscapeBehavior.FormClose)
                    {
                        MetForm.SendMessage(this.Handle, WM_SYSCOMMAND, SC_CLOSE, 0);
                        return true;
                    }
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        #endregion        
    }
}
