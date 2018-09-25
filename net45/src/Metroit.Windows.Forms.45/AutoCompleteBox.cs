using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using Metroit.Api.Win32;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// オートコンプリートのボックスを表示する命令を提供します。
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class AutoCompleteBox
    {
        // 全データを含むデータソースを管理するためのコンボボックス
        private MetComboBox innerCandidateBox = new MetComboBox();

        /// <summary>
        /// 新しい AutoCompleteBox インスタンスを生成します。
        /// </summary>
        public AutoCompleteBox()
        {
            this.CandidateBox.DropDownClosed += CandidateBox_DropDownClosed;

            this.CompareOptions = this.defaultCompareOptions;
            this.CandidateBox.IntegralHeight = false;
        }

        /// <summary>
        /// 新しい AutoCompleteBox インスタンスを生成します。
        /// </summary>
        /// <param name="control">利用するコントロール。</param>
        public AutoCompleteBox(Control control) : base()
        {
            this.SetTarget(control);
        }

        #region プロパティ

        /// <summary>
        /// オートコンプリートに利用される全データを含むデータソースを取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(null)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [AttributeProvider(typeof(IListSource))]
        [MetDescription("AutoCompleteBoxDataSource")]
        public object DataSource
        {
            get => innerCandidateBox.DataSource;
            set
            {
                innerCandidateBox.DataSource = value;

                // 本体のデータソースを初期化
                this.DataSourceChanging = true;
                CandidateBox.DataSource = innerCandidateBox.DataSource;
                CandidateBox.DisplayMember = innerCandidateBox.DisplayMember;
                CandidateBox.ValueMember = innerCandidateBox.ValueMember;
                this.ResetSelectedIndex();
                this.DataSourceChanging = false;
            }
        }

        /// <summary>
        /// このコントロール内の項目に対して表示するプロパティを示します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue("")]
        [MetDescription("AutoCompleteBoxDisplayMember")]
        public string DisplayMember { get => innerCandidateBox.DisplayMember; set => innerCandidateBox.DisplayMember = value; }

        /// <summary>
        /// コントロール内のアイテムに対して、実際の値として使用するプロパティを示します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue("")]
        [MetDescription("AutoCompleteBoxValueMember")]
        public string ValueMember { get => innerCandidateBox.ValueMember; set => innerCandidateBox.ValueMember = value; }

        /// <summary>
        /// オートコンプリートの表示するアイテム数を設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(8)]
        [MetDescription("AutoCompleteBoxMaxDropDownItems")]
        public int MaxDropDownItems { get => CandidateBox.MaxDropDownItems; set => CandidateBox.MaxDropDownItems = value; }

        /// <summary>
        /// 候補の絞込みを行うパターンを取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof(MatchPatternType), "StartsWith")]
        [MetDescription("AutoCompleteBoxMatchPattern")]
        public MatchPatternType MatchPattern { get; set; } = MatchPatternType.StartsWith;

        private CompareOptions[] defaultCompareOptions = new CompareOptions[] { System.Globalization.CompareOptions.None };

        /// <summary>
        /// 候補の絞込みに判断するオプションを取得または設定します。
        /// </summary>
        [Browsable(true)]
        [MetDescription("AutoCompleteBoxCompareOptions")]
        public CompareOptions[] CompareOptions { get; set; }

        /// <summary>
        /// BaseBackColor が既定値から変更されたかどうかを返却する。
        /// </summary>
        /// <returns>true:変更された, false:変更されていない</returns>
        private bool ShouldSerializeCompareOptions() => this.CompareOptions != null && this.CompareOptions != this.defaultCompareOptions;

        /// <summary>
        /// BaseBackColor のリセット操作を行う。
        /// </summary>
        private void ResetCompareOptions() => this.CompareOptions = defaultCompareOptions;

        /// <summary>
        /// オートコンプリートの利用を行うコントロールを取得します。
        /// </summary>
        [Browsable(false)]
        public Control Target { get; private set; } = null;

        /// <summary>
        /// 実際に画面に表示するコンボボックスの取得を行います。
        /// </summary>
        [Browsable(false)]
        public MetComboBox CandidateBox { get; } = new MetComboBox();

        /// <summary>
        /// ValueMember プロパティで指定したメンバー プロパティの値を取得します。
        /// </summary>
        [Browsable(false)]
        public object SelectedValue
        {
            get
            {
                // テキスト値に合致する値がない場合はnullとする
                if (!this.Contains(this.Target.Text))
                {
                    return null;
                }
                return this.CandidateBox.SelectedValue;
            }
        }

        /// <summary>
        /// DataSource プロパティを変更中かどうかを取得します。
        /// </summary>
        [Browsable(false)]
        public bool DataSourceChanging { get; private set; } = false;

        /// <summary>
        /// オートコンプリートのボックスを閉じようとしているかどうかを取得します。
        /// </summary>
        [Browsable(false)]
        public bool BoxClosing { get; private set; } = false;

        /// <summary>
        /// オートコンプリートのボックスを開こうとしているかどうかを取得します。
        /// </summary>
        [Browsable(false)]
        public bool BoxOpening { get; private set; } = false;

        /// <summary>
        /// オートコンプリートのボックスが開かれているかどうかを取得します。
        /// </summary>
        [Browsable(false)]
        public bool IsOpen { get => this.CandidateBox.DroppedDown; }

        /// <summary>
        /// オートコンプリートのボックスがアクティブフォーカスかどうかを取得します。
        /// </summary>
        [Browsable(false)]
        public bool IsActive { get => this.Target.FindForm().ActiveControl == this.CandidateBox; }

        #endregion

        #region イベント

        /// <summary>
        /// 候補をマウスクリックで選択してコンボボックスが閉じられた時、正しく終了させ、
        /// 利用コントロールをフォーカスする。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CandidateBox_DropDownClosed(object sender, EventArgs e)
        {
            this.BoxClosing = true;

            // クリックによって選択した時、ドロップダウンが非表示されないので非表示とする
            if (this.CandidateBox.Visible)
            {
                this.CandidateBox.Visible = false;
            }

            // ドロップダウンを表示している状態で候補が表示されており、
            // クリックによって選択、またはテキスト値をリスト候補の文字列としてフォーカス遷移した時、
            // リスト候補にない場合は、テキスト値をリストのテキストとする
            if (this.CandidateBox.Items.Count >= 1 && this.CandidateBox.Text != this.Target.Text &&
                !this.Contains(this.Target.Text))
            {
                this.CandidateBox.Text = this.Target.Text;
            }

            // マウスクリックで選択してリストが閉じられた時は元コントロールへフォーカスを戻す
            var activeControl = this.Target.FindForm().ActiveControl;
            if (activeControl == this.CandidateBox)
            {
                this.Target.Focus();
            }
            this.Target.Cursor = Cursors.IBeam;

            this.BoxClosing = false;
        }

        #endregion

        #region メソッド

        private int preItemCount = -1;

        /// <summary>
        /// オートコンプリートのボックスを開きます。
        /// </summary>
        public void Open()
        {
            this.SetupControlProperties();

            this.BoxOpening = true;

            this.CandidateBox.Visible = true;

            // プルダウンの表示件数初期化
            this.preItemCount = -1;

            // 既に入力されている値での絞り込み
            this.Extract(this.Target.Text);

            this.CandidateBox.DroppedDown = true;

            // TextBoxのテキストに差し替える
            this.CandidateBox.Text = this.Target.Text;
            this.Target.Cursor = Cursors.Arrow;

            this.BoxOpening = false;
        }

        /// <summary>
        /// オートコンプリートのボックスを閉じます。
        /// </summary>
        public void Close()
        {
            this.CandidateBox.DroppedDown = false;
            this.CandidateBox.Visible = false;
        }

        /// <summary>
        /// オートコンプリートを利用するコントロールを設定します。
        /// </summary>
        /// <param name="target">オートコンプリートを利用するコントロール。</param>
        internal void SetTarget(Control target)
        {
            this.Target = target;

            if (this.Target.Parent == null)
            {
                return;
            }

            this.SetupControlProperties();
            this.Target.Parent.Controls.Add(this.CandidateBox);
        }

        /// <summary>
        /// 全体の入力候補より絞り込みを行います。
        /// </summary>
        /// <param name="value">候補を絞り込む文字列。</param>
        internal void Extract(string value)
        {
            this.CandidateBox.DataSource = this.ExtractObject(value);
            this.ResetSelectedIndex();

            // 初回ドロップダウン表示以外で、前回の表示件数より多い場合はドロップダウンを開き直す
            if (!this.BoxOpening && this.CandidateBox.Items.Count > this.preItemCount)
            {
                this.Redraw();
            }
            this.preItemCount = this.CandidateBox.Items.Count;
        }

        /// <summary>
        /// 現在の入力候補より、指定した値が存在するかどうかを取得します。
        /// </summary>
        /// <param name="value">検索する文字列。</param>
        internal bool Contains(string value)
        {
            // DataSet または DataTable の場合
            if (this.CandidateBox.DataSource is DataSet || this.CandidateBox.DataSource is DataTable)
            {
                var sourceDt = (this.CandidateBox.DataSource as DataTable) ?? (this.CandidateBox.DataSource as DataSet).Tables[0];

                // 対象列に入っている文字列が合致する行を候補とする
                foreach (var row in sourceDt.AsEnumerable())
                {
                    var displayText = row[DisplayMember].ToString();
                    if (displayText == value)
                    {
                        return true;
                    }
                }
                return false;
            }

            // IList(List<T>)の場合の処理
            if (this.CandidateBox.DataSource is IList)
            {
                var sourceList = this.CandidateBox.DataSource as IList;

                // 対象プロパティに入っている文字列が合致する行を候補とする
                foreach (var sourceItem in sourceList)
                {
                    PropertyDescriptor descriptor = TypeDescriptor.GetProperties(sourceItem).Find(innerCandidateBox.DisplayMember, true);
                    var displayText = descriptor.GetValue(sourceItem).ToString();
                    if (displayText == value)
                    {
                        return true;
                    }
                }
                return false;
            }
            return false;
        }

        /// <summary>
        /// コンボボックスのプロパティを設定する。
        /// </summary>
        private void SetupControlProperties()
        {
            this.CandidateBox.Name = Guid.NewGuid().ToString("N").Substring(0, 10);
            this.CandidateBox.BaseBackColor = this.Target.BackColor;
            this.CandidateBox.BaseForeColor = this.Target.ForeColor;
            this.CandidateBox.FocusBackColor = this.Target.BackColor;
            this.CandidateBox.FocusForeColor = this.Target.ForeColor;
            this.CandidateBox.FlatStyle = FlatStyle.Flat;
            this.CandidateBox.Font = this.Target.Font;
            this.CandidateBox.Location = this.Target.Location;
            this.CandidateBox.Size = Target.Size;

            // コンボボックス自体の高さを、テキストと一緒にする
            // WParam: -1 = コンボボックス自体の高さ
            // Height: TextBoxの高さ - 6 = TextBoxより+6大きい数値が設定されるため
            var height = this.Target.Height - 6;
            User32.SendMessage(this.CandidateBox.Handle, ComboBoxCommand.CB_SETITEMHEIGHT, -1, height);

            this.CandidateBox.Visible = false;
        }
       
        /// <summary>
        /// オートコンプリートのボックス内容を再描画する。
        /// </summary>
        private void Redraw()
        {
            this.Close();
            this.BoxOpening = true;
            this.CandidateBox.Visible = true;
            this.CandidateBox.DroppedDown = true;
            this.Target.Cursor = Cursors.Arrow;
            this.BoxOpening = false;
        }

        /// <summary>
        /// ボックス候補の絞り込みを行う。
        /// </summary>
        /// <param name="value">絞り込み文字列。</param>
        /// <returns>絞り込みデータ。</returns>
        private object ExtractObject(string value)
        {
            var compareOptions = this.GetExecuteCompareOptions();

            if (this.DataSource is DataSet)
            {
                return this.ExtractDataSet(value, compareOptions);
            }

            if (this.DataSource is DataTable)
            {
                return this.ExtractDataTable(value, compareOptions);
            }

            if (this.DataSource is IList)
            {
                return this.ExtractList(value, compareOptions);
            }

            return null;
        }

        /// <summary>
        /// 判断実行を行うオプションを取得する。
        /// </summary>
        /// <returns>CompareOptions オブジェクト。</returns>
        private CompareOptions GetExecuteCompareOptions()
        {
            var executeCompareOptions = System.Globalization.CompareOptions.None;
            foreach (var compareOption in this.CompareOptions)
            {
                executeCompareOptions = executeCompareOptions | compareOption;
            }
            return executeCompareOptions;
        }

        /// <summary>
        /// DataSet オブジェクトのリストデータを取得する。
        /// </summary>
        /// <param name="value">絞り込み文字列。</param>
        /// <param name="compareOptions">絞り込み条件。</param>
        /// <returns>DataSet オブジェクト。</returns>
        private DataSet ExtractDataSet(string value, CompareOptions compareOptions)
        {
            var sourceDt = (this.DataSource as DataSet).Tables[0];
            var destDt = sourceDt.Clone();

            // 対象列に入っている文字列が合致する行を候補とする
            foreach (var row in sourceDt.AsEnumerable())
            {
                var displayText = row[DisplayMember].ToString();
                if (!IsMatch(displayText, value, compareOptions))
                {
                    continue;
                }
                destDt.ImportRow(row);
            }

            var destDs = new DataSet();
            destDs.Tables.Add(destDt);
            return destDs;
        }

        /// <summary>
        /// DataTable オブジェクトのリストデータを取得する。
        /// </summary>
        /// <param name="value">絞り込み文字列。</param>
        /// <param name="compareOptions">絞り込み条件。</param>
        /// <returns>DataTable オブジェクト。</returns>
        private DataTable ExtractDataTable(string value, CompareOptions compareOptions)
        {
            var sourceDt = (this.DataSource as DataTable);
            var destDt = sourceDt.Clone();

            // 対象列に入っている文字列が合致する行を候補とする
            foreach (var row in sourceDt.AsEnumerable())
            {
                var displayText = row[DisplayMember].ToString();
                if (!IsMatch(displayText, value, compareOptions))
                {
                    continue;
                }
                destDt.ImportRow(row);
            }

            return destDt;
        }

        /// <summary>
        /// IList オブジェクトのリストデータを取得する。
        /// </summary>
        /// <param name="value">絞り込み文字列。</param>
        /// <param name="compareOptions">絞り込み条件。</param>
        /// <returns>IList オブジェクト。</returns>
        private IList ExtractList(string value, CompareOptions compareOptions)
        {
            var sourceList = this.DataSource as IList;
            var destList = Activator.CreateInstance(this.DataSource.GetType()) as IList;

            // 対象プロパティに入っている文字列が合致する行を候補とする
            foreach (var sourceItem in sourceList)
            {
                PropertyDescriptor descriptor = TypeDescriptor.GetProperties(sourceItem).Find(innerCandidateBox.DisplayMember, true);
                var displayText = descriptor.GetValue(sourceItem).ToString();
                if (!IsMatch(displayText, value, compareOptions))
                {
                    continue;
                }
                destList.Add(sourceItem);
            }
            return destList;
        }

        /// <summary>
        /// 対象のテキストが候補対象となるかを確認する。
        /// </summary>
        /// <param name="sourceText">候補文字列。</param>
        /// <param name="destText">検索文字列。</param>
        /// <param name="compareOptions">CompareOptions オブジェクト。</param>
        /// <returns>true:候補対象, false:候補対象でない。</returns>
        private bool IsMatch(string sourceText, string destText, CompareOptions compareOptions)
        {
            // 検索文字列が空の場合は候補とする
            if (string.IsNullOrEmpty(destText))
            {
                return true;
            }

            var ci = CultureInfo.CurrentCulture.CompareInfo;
            switch (MatchPattern)
            {
                case MatchPatternType.StartsWith:
                    if (ci.IndexOf(sourceText, destText, compareOptions) != 0)
                    {
                        return false;
                    }
                    break;
                case MatchPatternType.EndsWith:
                    if (ci.LastIndexOf(sourceText, destText, compareOptions) != sourceText.Length - destText.Length)
                    {
                        return false;
                    }
                    break;
                case MatchPatternType.Partial:
                    if (ci.IndexOf(sourceText, destText, compareOptions) < 0)
                    {
                        return false;
                    }
                    break;
                case MatchPatternType.Equal:
                    if (ci.IndexOf(sourceText, destText, compareOptions) < 0 || sourceText.Length != destText.Length)
                    {
                        return false;
                    }
                    break;
            }
            return true;
        }

        /// <summary>
        /// リストのテキスト値とターゲットのテキスト値が異なる時、リストの選択状態を初期化する。
        /// </summary>
        private void ResetSelectedIndex()
        {
            if (this.CandidateBox.Text != this.Target.Text)
            {
                this.CandidateBox.SelectedIndex = -1;
            }
        }

        #endregion
    }
}
