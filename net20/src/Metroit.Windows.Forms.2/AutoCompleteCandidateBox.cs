using Metroit.Win32.Api;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// オートコンプリートの実際に表示するドロップダウンを提供します。
    /// </summary>
    internal class AutoCompleteCandidateBox : MetComboBox
    {
        #region 追加イベント

        /// <summary>
        /// ドロップダウンが開いた時に発生するイベントです。
        /// </summary>
        public event EventHandler CandidateBoxOpened;

        /// <summary>
        /// ドロップダウンが開いた時に走行します。
        /// </summary>
        /// <param name="sender">発生元オブジェクト。</param>
        /// <param name="e">EventArgs オブジェクト。</param>
        protected virtual void OnCandidateBoxOpened(object sender, EventArgs e)
        {
            this.CandidateBoxOpened?.Invoke(sender, e);
        }

        /// <summary>
        /// ドロップダウンが閉じた時に発生するイベントです。
        /// </summary>
        public event EventHandler CandidateBoxClosed;

        /// <summary>
        /// ドロップダウンが閉じた時に走行します。
        /// </summary>
        /// <param name="sender">発生元オブジェクト。</param>
        /// <param name="e">EventArgs オブジェクト。</param>
        protected virtual void OnCandidateBoxClosed(object sender, EventArgs e)
        {
            this.CandidateBoxClosed?.Invoke(sender, e);
        }

        #endregion

        #region プロパティ

        private bool propertyChanging = false;

        /// <summary>
        /// データ ソースを取得または設定します。
        /// </summary>
        public new object DataSource
        {
            get => base.DataSource;
            set
            {
                this.propertyChanging = true;
                base.DataSource = value;
                this.propertyChanging = false;
            }
        }

        /// <summary>
        /// 表示するプロパティを取得または設定します。
        /// </summary>
        public new string DisplayMember
        {
            get => base.DisplayMember;
            set
            {
                this.propertyChanging = true;
                base.DisplayMember = value;
                this.propertyChanging = false;
            }
        }

        /// <summary>
        /// 項目の実際の値として使用するプロパティのパスを取得または設定します。
        /// </summary>
        public new string ValueMember
        {
            get => base.ValueMember;
            set
            {
                this.propertyChanging = true;
                base.ValueMember = value;
                this.propertyChanging = false;
            }
        }

        #endregion

        #region 追加プロパティ

        /// <summary>
        /// オートコンプリートを利用するコントロールを取得または設定します。
        /// </summary>
        public Control TargetControl { get; set; } = null;

        /// <summary>
        /// ドロップダウンが開いているかどうかを取得します。
        /// </summary>
        public bool IsOpened { get; private set; } = false;

        #endregion

        #region メソッド

        private bool isClosing = false;
        private bool isClickSelectionCommitted = false;
        private List<IMessageFilter> messageFilters = new List<IMessageFilter>();

        /// <summary>
        /// ドロップダウンコントロールの初期化を行います。
        /// </summary>
        public void InitializeControl(Control targetControl)
        {
            this.isClosing = false;
            this.isClickSelectionCommitted = false;

            this.TargetControl = targetControl;

            this.Name = Guid.NewGuid().ToString("N").Substring(0, 10);
            this.IntegralHeight = false;       // これがないとMaxDropDownItems が効かない
            this.BaseBackColor = this.TargetControl.BackColor;
            this.BaseForeColor = this.TargetControl.ForeColor;
            this.FocusBackColor = this.TargetControl.BackColor;
            this.FocusForeColor = this.TargetControl.ForeColor;
            this.FlatStyle = FlatStyle.Flat;
            this.Font = this.TargetControl.Font;
            this.Location = this.TargetControl.Location;
            this.Size = TargetControl.Size;
            this.Visible = false;

            // 初回にコントロールを追加
            if (!this.TargetControl.Parent.Controls.Contains(this))
            {
                this.TargetControl.Parent.Controls.Add(this);
                this.InitializeMessageFiliter();
            }
        }

        /// <summary>
        /// ウィンドウメッセージフィルタを初期化する。
        /// </summary>
        private void InitializeMessageFiliter()
        {
            // 下キーの動作は、ドロップダウンに通知する
            this.messageFilters.Add(new MessageFilter(
                new MessageFilterParams(WindowMessage.WM_KEYDOWN)
                {
                    Controls = new List<Control>() { this.TargetControl },
                    WParam = new List<int>() { VirtualKey.VK_DOWN },
                    Action = (e) =>
                    {
                        User32.SendMessage(this.Handle, WindowMessage.WM_KEYDOWN, new IntPtr(VirtualKey.VK_DOWN), IntPtr.Zero);
                        e.StopMessage = true;
                    }
                }
            ));

            // 上キーの動作は、ドロップダウンに通知する
            this.messageFilters.Add(new MessageFilter(
                new MessageFilterParams(WindowMessage.WM_KEYDOWN)
                {
                    Controls = new List<Control>() { this.TargetControl },
                    WParam = new List<int>() { VirtualKey.VK_UP },
                    Action = (e) =>
                    {
                        User32.SendMessage(this.Handle, WindowMessage.WM_KEYDOWN, new IntPtr(VirtualKey.VK_UP), IntPtr.Zero);
                        e.StopMessage = true;
                    }
                }
            ));

            // ESCキーの動作は、ドロップダウンを閉じるようにする
            this.messageFilters.Add(new MessageFilter(
                new MessageFilterParams(WindowMessage.WM_KEYDOWN)
                {
                    Controls = new List<Control>() { this.TargetControl },
                    WParam = new List<int>() { VirtualKey.VK_ESCAPE },
                    Action = (e) =>
                    {
                        this.Close();
                        e.StopMessage = true;
                    }
                }
            ));

            // Tabキーの動作は、ドロップダウン閉じてから元コントロールの動作をする
            this.messageFilters.Add(new MessageFilter(
                new MessageFilterParams(WindowMessage.WM_KEYDOWN)
                {
                    Controls = new List<Control>() { this.TargetControl },
                    WParam = new List<int>() { VirtualKey.VK_TAB },
                    Action = (e) =>
                    {
                        this.Close();
                        e.StopMessage = false;
                    }
                }
            ));

            // Enterキーの動作は、ドロップダウンを閉じるようにする
            this.messageFilters.Add(new MessageFilter(
                new MessageFilterParams(WindowMessage.WM_KEYDOWN)
                {
                    Controls = new List<Control>() { this.TargetControl },
                    WParam = new List<int>() { VirtualKey.VK_RETURN },
                    Action = (e) =>
                    {
                        this.Close();
                        e.StopMessage = true;
                    }
                }
            ));

            // マウスホイールの動作は、ドロップダウンに通知する
            this.messageFilters.Add(new MessageFilter(
                new MessageFilterParams(WindowMessage.WM_MOUSEWHEEL)
                {
                    Controls = new List<Control>() { this.TargetControl },
                    Action = (e) =>
                    {
                        User32.SendMessage(this.Handle, e.Message.Msg, e.Message.WParam, e.Message.LParam);
                        e.StopMessage = true;
                    }
                }
            ));
        }

        /// <summary>
        /// <para>ドロップダウンを開きます。</para>
        /// <para>事前に InitializeControl() を実施している必要があります。</para>
        /// </summary>
        public void Open()
        {
            this.OpenDropdown();
            this.AddMessageFilter();
            this.OnCandidateBoxOpened(this, EventArgs.Empty);
        }

        /// <summary>
        /// ウィンドウメッセージフィルタを追加する。
        /// </summary>
        private void AddMessageFilter()
        {
            foreach (var messageFiliter in this.messageFilters)
            {
                Application.AddMessageFilter(messageFiliter);
            }
        }

        /// <summary>
        /// ドロップダウンを開く。
        /// </summary>
        private void OpenDropdown()
        {
            Cursor.Current = Cursors.Arrow;
            this.IsOpened = true;
            this.DroppedDown = true;
        }

        /// <summary>
        /// ドロップダウンを閉じます。
        /// </summary>
        public void Close()
        {
            this.RemoveMessageFilter();
            this.CloseDropdown();
            this.OnCandidateBoxClosed(this, EventArgs.Empty);
        }

        /// <summary>
        /// ウィンドウメッセージフィルタを削除する。
        /// </summary>
        private void RemoveMessageFilter()
        {
            foreach (var messageFiliter in this.messageFilters)
            {
                Application.RemoveMessageFilter(messageFiliter);
            }
        }

        /// <summary>
        /// ドロップダウンを閉じる。
        /// </summary>
        private void CloseDropdown()
        {
            this.isClosing = true;

            // アイテムが決定されてされていない状態でTabで遷移した時に、SelectedItem は null の状態にある
            var isNull = false;
            if (this.SelectedItem == null)
            {
                isNull = true;
            }

            this.IsOpened = false;
            this.DroppedDown = false;

            if (isNull)
            {
                // アイテムが選択されなかった時は DroppedDown = false によって、SelectedIndex=0になり、
                // SelectedItem, SelectedValue, SelectedText が IndexOutOfRangeException となるため
                // 値の初期化を行う
                this.ResetSelectedIndex();
            }
        }

        /// <summary>
        /// <para>ドロップダウンを開きなおします。</para>
        /// <para>ドロップダウンのアイテム量が変化した時に有効です。</para>
        /// </summary>
        public void ReOpen()
        {
            this.CloseDropdown();
            this.isClosing = false;
            this.isClickSelectionCommitted = false;
            this.OpenDropdown();
        }

        /// <summary>
        /// ドロップダウンの選択状態をリセットします。
        /// </summary>
        public void ResetSelectedIndex()
        {
            this.propertyChanging = true;
            this.SelectedIndex = -1;
            this.propertyChanging = false;
        }

        /// <summary>
        /// フォーカスを得たことを認識させません。
        /// </summary>
        /// <param name="e">EventArgs オブジェクト。</param>
        protected override sealed void OnGotFocus(EventArgs e)
        {
            return;
        }

        /// <summary>
        /// フォーカスを得たことを認識させません。
        /// </summary>
        /// <param name="e">EventArgs オブジェクト。</param>
        protected override sealed void OnEnter(EventArgs e)
        {
            return;
        }

        /// <summary>
        /// ドロップダウンが閉じた時に、明示的に終了処理を行います。
        /// </summary>
        /// <param name="e">EventArgs オブジェクト。</param>
        protected override sealed void OnDropDownClosed(EventArgs e)
        {
            base.OnDropDownClosed(e);

            // フォーカス変更またはAlt+Tabでドロップダウンが閉じられた時、明示的に終了処理を実行
            if (this.IsOpened)
            {
                this.Close();
            }
        }

        /// <summary>
        /// ドロップダウンのアイテムをクリックで決定したことを保持します。
        /// </summary>
        /// <param name="e"></param>
        protected override sealed void OnSelectionChangeCommitted(EventArgs e)
        {
            base.OnSelectionChangeCommitted(e);

            // 候補をクリックで決めた時だけフラグを立てる
            if (this.Focused)
            {
                this.isClickSelectionCommitted = true;
            }
        }

        /// <summary>
        /// アイテムが選択された時の動作を行います。
        /// </summary>
        /// <param name="e">EventArgs オブジェクト。</param>
        protected override sealed void OnSelectedIndexChanged(EventArgs e)
        {
            // ドロップダウン表示中にAlt+Tabすると走行してしまうのを止める
            // 入力値がアイテムの一部の時、勝手にアイテムが確定してしまうことも止める
            if (this.isClosing && !this.isClickSelectionCommitted)
            {
                return;
            }

            // ResetSelectedIndex(), DataSource の設定中は処理しない
            if (this.propertyChanging)
            {
                return;
            }

            base.OnSelectedIndexChanged(e);

            // クリックによって候補が選択されてドロップダウンが閉じた場合は、
            // 元のコントロールにフォーカスを戻し、オープン状態を初期値に戻す
            if (!this.DroppedDown)
            {
                this.TargetControl.Focus();
                this.IsOpened = false;
            }
        }

        /// <summary>
        /// DisplayMember, ValueMember の設定中に値が変更された時は処理しないようにします。
        /// </summary>
        /// <param name="e"></param>
        protected override sealed void OnSelectedValueChanged(EventArgs e)
        {
            // DisplayMember, ValueMember の設定中は処理しない
            if (this.propertyChanging)
            {
                return;
            }

            base.OnSelectedValueChanged(e);
        }

        #endregion
    }
}
