using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Metroit.Windows.Forms.ComponentModel
{
    /// <summary>
    /// ショートカットキーを設定するためのユーザーコントロールを提供します。
    /// </summary>
    [ToolboxItem(false)]
    internal partial class ShortcutKeysControl : UserControl
    {
        private static readonly MetKeys[] ValidKeys = {
            MetKeys.A, MetKeys.B, MetKeys.Back, MetKeys.C, MetKeys.D, MetKeys.D0, MetKeys.D1, MetKeys.D2,
            MetKeys.D3, MetKeys.D4, MetKeys.D5, MetKeys.D6, MetKeys.D7, MetKeys.D8, MetKeys.D9, MetKeys.Decimal, MetKeys.Delete,
            MetKeys.Divide, MetKeys.Down, MetKeys.E, MetKeys.End, MetKeys.Return, MetKeys.Escape, MetKeys.F, MetKeys.F1, MetKeys.F10,
            MetKeys.F11, MetKeys.F12, MetKeys.F13, MetKeys.F14, MetKeys.F15, MetKeys.F16, MetKeys.F17, MetKeys.F18, MetKeys.F19,
            MetKeys.F2, MetKeys.F20, MetKeys.F21, MetKeys.F22, MetKeys.F23, MetKeys.F24, MetKeys.F3, MetKeys.F4, MetKeys.F5, MetKeys.F6,
            MetKeys.F7, MetKeys.F8, MetKeys.F9, MetKeys.G, MetKeys.H, MetKeys.Home, MetKeys.I, MetKeys.Insert, MetKeys.J, MetKeys.K,
            MetKeys.L, MetKeys.Left, MetKeys.M, MetKeys.Multiply, MetKeys.N, MetKeys.NumLock,
            MetKeys.NumPad0, MetKeys.NumPad1, MetKeys.NumPad2, MetKeys.NumPad3, MetKeys.NumPad4, MetKeys.NumPad5, MetKeys.NumPad6,
            MetKeys.NumPad7, MetKeys.NumPad8, MetKeys.NumPad9, MetKeys.O, MetKeys.OemBackslash, MetKeys.OemClear, MetKeys.OemCloseBrackets,
            MetKeys.Oemcomma, MetKeys.OemMinus, MetKeys.OemOpenBrackets, MetKeys.OemPeriod, MetKeys.OemPipe, MetKeys.Oemplus,
            MetKeys.OemQuestion, MetKeys.OemQuotes, MetKeys.OemSemicolon, MetKeys.Oemtilde, MetKeys.P, MetKeys.Next,
            MetKeys.Prior, MetKeys.Pause, MetKeys.Q, MetKeys.R, MetKeys.Right, MetKeys.S,
            MetKeys.Space, MetKeys.Subtract, MetKeys.T, MetKeys.Tab, MetKeys.U, MetKeys.Up, MetKeys.V, MetKeys.W,
            MetKeys.X, MetKeys.Y, MetKeys.Z
        };

        /// <summary>
        /// 画面で選択されているキー値を取得します。
        /// </summary>
        public object Value { get; private set; }

        /// <summary>
        /// コンボボックスに表示するキー名を求めるためのコンバーター。
        /// </summary>
        private TypeConverter MetKeysConverter => TypeDescriptor.GetConverter(typeof(MetKeys));

        /// <summary>
        /// 呼出が行われたエディタ。
        /// </summary>
        private ShortcutKeysEditor editor;

        /// <summary>
        /// 呼出が行われたIWindowsFormsEditorService。
        /// </summary>
        private IWindowsFormsEditorService editorService;

        /// <summary>
        /// ShortcutKeyControl クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="editor">呼出元のエディタ。</param>
        internal ShortcutKeysControl(ShortcutKeysEditor editor)
        {
            InitializeComponent();

            this.editor = editor;

            // 文字をローカライズ
            systemKeyLabel.Text = DesignResources.GetString("ShortcutKeysControl.SystemKeyText");
            keyLabel.Text = DesignResources.GetString("ShortcutKeysControl.KeyText");
            resetButton.Text = DesignResources.GetString("ResetText");

            Terminate();

            // 設定可能なキー
            foreach (MetKeys validKey in ValidKeys)
            {
                keysComboBox.Items.Add(MetKeysConverter.ConvertToString(validKey));
            }
        }

        /// <summary>
        /// ドロップダウンが開かれる前の準備を行います。
        /// プロパティに設定されている値で画面情報に適用します。
        /// </summary>
        /// <param name="editorService">IWindowsFormsEditorService。</param>
        /// <param name="value">プロパティに設定されている値。</param>
        public void Prepare(IWindowsFormsEditorService editorService, object value)
        {
            this.editorService = editorService;
            Value = value;
            
            MetKeys keys = (MetKeys)value;
            checkCtrl.Checked = (keys & MetKeys.Control) > 0;
            checkShift.Checked = (keys & MetKeys.Shift) > 0;
            checkAlt.Checked = (keys & MetKeys.Alt) > 0;

            MetKeys keyCode = keys & MetKeys.KeyCode;
            if (keyCode == MetKeys.None || !IsValidKeyCode(keyCode))
            {
                keysComboBox.SelectedIndex = -1;
            }
            else
            {
                keysComboBox.SelectedItem = MetKeysConverter.ConvertToString(keyCode);
            }
        }

        /// <summary>
        /// ドロップダウンの操作が終了した時に行います。
        /// 明示的に選択キー値をクリアします。
        /// </summary>
        public void Terminate()
        {
            this.editorService = null;
            Value = null;
        }

        /// <summary>
        /// 選択画面として選択可能なキーコードかどうか。
        /// </summary>
        /// <param name="keyCode">修飾子を除くキーコード。</param>
        /// <returns>true:選択可能, false:選択不可能。</returns>
        private static bool IsValidKeyCode(MetKeys keyCode)
        {
            return ValidKeys.Where(x => x == keyCode).Any();
        }

        /// <summary>
        /// キー設定をリセットする。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void resetButton_Click(object sender, EventArgs e)
        {
            checkCtrl.Checked = false;
            checkShift.Checked = false;
            checkAlt.Checked = false;
            keysComboBox.SelectedIndex = -1;
        }

        /// <summary>
        /// 修飾子チェックの変更。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckStateChanged(object sender, EventArgs e)
        {
            UpdateValue();
        }

        /// <summary>
        /// キーの変更。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void keysComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateValue();
        }

        /// <summary>
        /// 画面で設定されたキー値に更新する。
        /// </summary>
        private void UpdateValue()
        {
            var keyIndex = keysComboBox.SelectedIndex;

            MetKeys keys = MetKeys.None;
            if (keyIndex == -1)
            {
                Value = keys;
                return;
            }

            if (checkCtrl.Checked)
            {
                keys |= MetKeys.Control;
            }
            if (checkShift.Checked)
            {
                keys |= MetKeys.Shift;
            }
            if (checkAlt.Checked)
            {
                keys |= MetKeys.Alt;
            }

            keys |= ValidKeys[keyIndex];

            Value = keys;
        }
    }
}
