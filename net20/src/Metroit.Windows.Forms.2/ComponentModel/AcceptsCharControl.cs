using Metroit.Windows.Forms;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Metroit.Windows.Forms.ComponentModel
{
    /// <summary>
    /// 許可文字を設定するためのユーザーコントロールを提供します。
    /// </summary>
    [ToolboxItem(false)]
    public partial class AcceptsCharControl : UserControl
    {
        // 各種類チェックによる状態保持
        private enum CheckedConditionType : int
        {
            AroundImpact = 0,
            NumericEitherChecked = 1,
            AlphaEitherChecked = 2,
            SignEitherChecked = 3
        }

        private AcceptsCharType acceptsCharType;
        private bool[] checkedCondition = new bool[] { false, false, false, false };

        /// <summary>
        /// ApprovalCharControl クラスの新しいインスタンスを初期化します。
        /// </summary>
        internal AcceptsCharControl()
        {
            InitializeComponent();

            // 文字をローカライズ
            checkAll.Text = DesignResources.GetString("AcceptsAll");
            checkNumeric.Text = DesignResources.GetString("AcceptsNum");
            checkHalfNumeric.Text = DesignResources.GetString("AcceptsHalf");
            checkFullNumeric.Text = DesignResources.GetString("AcceptsFull");
            checkAlpha.Text = DesignResources.GetString("AcceptsAlpha");
            checkHalfAlpha.Text = DesignResources.GetString("AcceptsHalf");
            checkFullAlpha.Text = DesignResources.GetString("AcceptsFull");
            checkSign.Text = DesignResources.GetString("AcceptsSign");
            checkHalfSign.Text = DesignResources.GetString("AcceptsHalf");
            checkFullSign.Text = DesignResources.GetString("AcceptsFull");
            checkCustom.Text = DesignResources.GetString("Custom");

            // 共通チェック時イベント追加
            checkNumeric.CheckedChanged += new EventHandler(checkType_CheckedChanged);
            checkHalfNumeric.CheckedChanged += new EventHandler(checkTypeParcelling_CheckedChanged);
            checkFullNumeric.CheckedChanged += new EventHandler(checkTypeParcelling_CheckedChanged);

            checkAlpha.CheckedChanged += new EventHandler(checkType_CheckedChanged);
            checkHalfAlpha.CheckedChanged += new EventHandler(checkTypeParcelling_CheckedChanged);
            checkFullAlpha.CheckedChanged += new EventHandler(checkTypeParcelling_CheckedChanged);

            checkSign.CheckedChanged += new EventHandler(checkType_CheckedChanged);
            checkHalfSign.CheckedChanged += new EventHandler(checkTypeParcelling_CheckedChanged);
            checkFullSign.CheckedChanged += new EventHandler(checkTypeParcelling_CheckedChanged);

            // 対応する列挙値を格納
            checkAll.Tag = AcceptsCharType.All;
            checkHalfNumeric.Tag = AcceptsCharType.HalfNumeric;
            checkFullNumeric.Tag = AcceptsCharType.FullNumeric;
            checkHalfAlpha.Tag = AcceptsCharType.HalfAlpha;
            checkFullAlpha.Tag = AcceptsCharType.FullAlpha;
            checkHalfSign.Tag = AcceptsCharType.HalfSign;
            checkFullSign.Tag = AcceptsCharType.FullSign;
            checkCustom.Tag = AcceptsCharType.Custom;
        }

        /// <summary>
        /// ApprovalCharControl クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="approvalCharacters">許可文字。</param>
        internal AcceptsCharControl(AcceptsCharType approvalCharacters)
            : this()
        {
            ApprovalCharacters = approvalCharacters;
        }

        /// <summary>
        /// 許可文字の取得または設定します。
        /// </summary>
        internal AcceptsCharType ApprovalCharacters
        {
            get { return acceptsCharType; }
            set
            {
                acceptsCharType = value;
                checkAll.Checked = ((value & AcceptsCharType.All) != 0);
                checkHalfNumeric.Checked = ((value & AcceptsCharType.HalfNumeric) != 0);
                checkFullNumeric.Checked = ((value & AcceptsCharType.FullNumeric) != 0);
                checkHalfAlpha.Checked = ((value & AcceptsCharType.HalfAlpha) != 0);
                checkFullAlpha.Checked = ((value & AcceptsCharType.FullAlpha) != 0);
                checkHalfSign.Checked = ((value & AcceptsCharType.HalfSign) != 0);
                checkFullSign.Checked = ((value & AcceptsCharType.FullSign) != 0);
                checkCustom.Checked = ((value & AcceptsCharType.Custom) != 0);
            }
        }

        /// <summary>
        /// 全て許可以外のチェックボックスを外します。
        /// </summary>
        /// <param name="sender">操作チェックボックス</param>
        /// <param name="e">空イベントオブジェクト</param>
        private void checkAll_CheckedChanged(object sender, EventArgs e)
        {
            if (checkAll.Checked)
            {
                checkedCondition[(int)CheckedConditionType.AroundImpact] = true;
                checkNumeric.Checked = false;
                checkAlpha.Checked = false;
                checkSign.Checked = false;
                checkCustom.Checked = false;
                acceptsCharType |= (AcceptsCharType)checkAll.Tag;
                checkedCondition[(int)CheckedConditionType.AroundImpact] = false;
            }
            else
            {
                acceptsCharType &= ~(AcceptsCharType)checkAll.Tag;
            }
        }

        /// <summary>
        /// 数字チェックボックスの操作時、中間状態チェックを行いません。
        /// </summary>
        /// <param name="sender">操作チェックボックス</param>
        /// <param name="e">空イベントオブジェクト</param>
        private void checkNumeric_CheckStateChanged(object sender, EventArgs e)
        {
            // 1クリックで中間状態になった場合はチェックを外す
            if (checkNumeric.CheckState == CheckState.Indeterminate &&
                    !checkedCondition[(int)CheckedConditionType.NumericEitherChecked])
            {
                checkNumeric.Checked = false;
            }
        }

        /// <summary>
        /// 英字チェックボックスの操作時、中間状態チェックを行いません。
        /// </summary>
        /// <param name="sender">操作チェックボックス</param>
        /// <param name="e">空イベントオブジェクト</param>
        private void checkAlpha_CheckStateChanged(object sender, EventArgs e)
        {
            // 1クリックで中間状態になった場合はチェックを外す
            if (checkAlpha.CheckState == CheckState.Indeterminate &&
                    !checkedCondition[(int)CheckedConditionType.AlphaEitherChecked])
            {
                checkAlpha.Checked = false;
            }
        }

        /// <summary>
        /// 記号チェックボックスの操作時、中間状態チェックを行いません。
        /// </summary>
        /// <param name="sender">操作チェックボックス</param>
        /// <param name="e">空イベントオブジェクト</param>
        private void checkSign_CheckStateChanged(object sender, EventArgs e)
        {
            // 1クリックで中間状態になった場合はチェックを外す
            if (checkSign.CheckState == CheckState.Indeterminate &&
                    !checkedCondition[(int)CheckedConditionType.SignEitherChecked])
            {
                checkSign.Checked = false;
            }
        }

        /// <summary>
        /// カスタム以外のチェックボックスを外します。
        /// </summary>
        /// <param name="sender">操作チェックボックス</param>
        /// <param name="e">空イベントオブジェクト</param>
        private void checkCustom_CheckedChanged(object sender, EventArgs e)
        {
            if (checkCustom.Checked)
            {
                checkedCondition[(int)CheckedConditionType.AroundImpact] = true;
                checkAll.Checked = false;
                checkNumeric.Checked = false;
                checkAlpha.Checked = false;
                checkSign.Checked = false;
                acceptsCharType |= (AcceptsCharType)checkCustom.Tag;
                checkedCondition[(int)CheckedConditionType.AroundImpact] = false;
            }
            else
            {
                acceptsCharType &= ~(AcceptsCharType)checkCustom.Tag;
            }
        }

        /// <summary>
        /// 個別種類チェックボックスが操作された際に、全体設定及び、
        /// 個別種類設定のチェックボックスの状態を変化させます。
        /// </summary>
        /// <param name="sender">操作チェックボックス</param>
        /// <param name="e">空イベントオブジェクト</param>
        private void checkType_CheckedChanged(object sender, EventArgs e)
        {
            // 対象チェックボックスを決定
            CheckBox targetCheckBox = (CheckBox)sender;
            CheckBox targetHalfCheckBox = null;
            CheckBox targetFullCheckBox = null;
            if (targetCheckBox == checkNumeric)
            {
                targetHalfCheckBox = checkHalfNumeric;
                targetFullCheckBox = checkFullNumeric;
            }
            if (targetCheckBox == checkAlpha)
            {
                targetHalfCheckBox = checkHalfAlpha;
                targetFullCheckBox = checkFullAlpha;
            }

            if (targetCheckBox == checkSign)
            {
                targetHalfCheckBox = checkHalfSign;
                targetFullCheckBox = checkFullSign;
            }

            // 全チェックが行われた時
            if (targetCheckBox.CheckState == CheckState.Checked &&
                    !targetHalfCheckBox.Checked && !targetFullCheckBox.Checked)
            {
                targetHalfCheckBox.Checked = true;
                targetFullCheckBox.Checked = true;
            }

            // 全チェックなしが行われた時
            if (targetCheckBox.CheckState == CheckState.Unchecked &&
                    (targetHalfCheckBox.Checked || targetFullCheckBox.Checked))
            {
                targetHalfCheckBox.Checked = false;
                targetFullCheckBox.Checked = false;
            }
        }

        /// <summary>
        /// 半角／全角チェックボックスが操作された際に、全体設定及び、
        /// 個別種類設定のチェックボックスの状態を変化させます。
        /// </summary>
        /// <param name="sender">操作チェックボックス</param>
        /// <param name="e">空イベントオブジェクト</param>
        private void checkTypeParcelling_CheckedChanged(object sender, EventArgs e)
        {
            // 対象チェックボックスと状態列挙を決定
            CheckBox focusCheckBox = (CheckBox)sender;
            CheckBox targetCheckBox = null;
            CheckBox targetHalfCheckBox = null;
            CheckBox targetFullCheckBox = null;
            CheckedConditionType conditionType = CheckedConditionType.NumericEitherChecked;
            if (focusCheckBox == checkHalfNumeric || focusCheckBox == checkFullNumeric)
            {
                targetCheckBox = checkNumeric;
                targetHalfCheckBox = checkHalfNumeric;
                targetFullCheckBox = checkFullNumeric;
                conditionType = CheckedConditionType.NumericEitherChecked;
            }
            if (focusCheckBox == checkHalfAlpha || focusCheckBox == checkFullAlpha)
            {
                targetCheckBox = checkAlpha;
                targetHalfCheckBox = checkHalfAlpha;
                targetFullCheckBox = checkFullAlpha;
                conditionType = CheckedConditionType.AlphaEitherChecked;
            }

            if (focusCheckBox == checkHalfSign || focusCheckBox == checkFullSign)
            {
                targetCheckBox = checkSign;
                targetHalfCheckBox = checkHalfSign;
                targetFullCheckBox = checkFullSign;
                conditionType = CheckedConditionType.SignEitherChecked;
            }

            // 半角／全角チェックボックスが操作された時
            if (!checkedCondition[(int)CheckedConditionType.AroundImpact])
            {
                checkAll.Checked = false;
                checkCustom.Checked = false;
            }

            // 半角／全角の両方がチェックされた時
            if (targetHalfCheckBox.Checked && targetFullCheckBox.Checked)
            {
                checkedCondition[(int)conditionType] = false;
                acceptsCharType |= (AcceptsCharType)targetHalfCheckBox.Tag;
                acceptsCharType |= (AcceptsCharType)targetFullCheckBox.Tag;
                targetCheckBox.CheckState = CheckState.Checked;
            }

            // 半角／全角のいずれかがチェックされた時 
            else if (targetHalfCheckBox.Checked || targetFullCheckBox.Checked)
            {
                checkedCondition[(int)conditionType] = true;
                if (targetHalfCheckBox.Checked)
                {
                    acceptsCharType |= (AcceptsCharType)targetHalfCheckBox.Tag;
                }
                else
                    acceptsCharType &= ~(AcceptsCharType)targetHalfCheckBox.Tag;
                {
                }
                if (targetFullCheckBox.Checked)
                {
                    acceptsCharType |= (AcceptsCharType)targetFullCheckBox.Tag;
                }
                else
                {
                    acceptsCharType &= ~(AcceptsCharType)targetFullCheckBox.Tag;
                }
                targetCheckBox.CheckState = CheckState.Indeterminate;
            }

            // 半角／全角の両方のチェックが外された時
            else
            {
                checkedCondition[(int)conditionType] = false;
                acceptsCharType &= ~(AcceptsCharType)targetHalfCheckBox.Tag;
                acceptsCharType &= ~(AcceptsCharType)targetFullCheckBox.Tag;
                targetCheckBox.CheckState = CheckState.Unchecked;
            }
        }
    }
}
