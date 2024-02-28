using Metroit.Windows.Forms.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// オートコンプリートのボックスを表示する命令を提供します。
    /// </summary>
    [ToolboxItem(true)]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [DefaultBindingProperty("DataSource")]
    public class AutoCompleteBox : Component, IBindableComponent
    {
        private bool isAssociatedControl = false;    // Controlを指定したインスタンス化を実施したかどうか

        /// <summary>
        /// 新しい AutoCompleteBox インスタンスを生成します。
        /// </summary>
        public AutoCompleteBox()
        {
            this.CompareOptions = this.defaultCompareOptions;

            if (this.IsDesignMode())
            {
                return;
            }

            // 必要なイベントを設定
            this.candidateBox.SelectedValueChanged += CandidateBox_SelectedValueChanged;
            this.candidateBox.CandidateBoxClosed += CandidateBox_CandidateBoxClosed;
        }

        /// <summary>
        /// 新しい AutoCompleteBox インスタンスを生成します。
        /// </summary>
        /// <param name="control">利用するコントロール。</param>
        public AutoCompleteBox(Control control)
        {
            this.CompareOptions = this.defaultCompareOptions;

            // 紐づくコントロールを保持
            this.TargetControl = control;
            this.isAssociatedControl = true;

            if (this.IsDesignMode())
            {
                return;
            }

            // 必要なイベントを設定
            this.candidateBox.SelectedValueChanged += CandidateBox_SelectedValueChanged;
            this.candidateBox.CandidateBoxOpened += CandidateBox_CandidateBoxOpened;
            this.candidateBox.CandidateBoxClosed += CandidateBox_CandidateBoxClosed;
        }

        #region イベント

        /// <summary>
        /// ドロップダウンを開いた時のイベントを発生させる。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CandidateBox_CandidateBoxOpened(object sender, EventArgs e)
        {
            this.OnCandidateBoxOpened(sender, e);
        }

        /// <summary>
        /// ドロップダウンを閉じた時のイベントを発生させる。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CandidateBox_CandidateBoxClosed(object sender, EventArgs e)
        {
            // プルダウンの表示件数初期化
            this.preCandidateItemCount = -1;
            this.OnCandidateBoxClosed(sender, e);
        }

        // IListの実装を行う
        /// <summary>
        /// ドロップダウンを選択した時に選択値を設定してイベントを発生させる。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CandidateBox_SelectedValueChanged(object sender, EventArgs e)
        {
            // リストの初期表示で値が選択された時は走行させない, 候補が表示中に手入力で候補を決定した時は走行させない
            if (this.innerSelectedChanging)
            {
                return;
            }

            // DataSet または DataTable の場合
            if (this.DataSource is DataSet || this.DataSource is DataTable)
            {
                var dt = (this.DataSource as DataTable) ?? (this.DataSource as DataSet)?.Tables[0];

                var item = ((DataRowView)this.candidateBox.SelectedItem).Row;
                this.SelectedItem = item;
                this.SelectedValue = this.GetValue(item);
                this.SelectedIndex = dt.Rows.IndexOf(item);
                var cse = new CandidateSelectedEventArgs(this.SelectedItem, this.SelectedValue, this.candidateBox.Text, this.SelectedIndex);
                this.OnCandidateSelected(this, cse);
                return;
            }

            // IList の場合
            var list = this.DataSource as IList;
            if (list != null)
            {
                var item = this.candidateBox.SelectedItem;
                this.SelectedItem = item;
                this.SelectedValue = this.GetValue(item);
                this.SelectedIndex = list.IndexOf(item);
                var cse = new CandidateSelectedEventArgs(this.SelectedItem, this.SelectedValue, this.candidateBox.Text, this.SelectedIndex);
                this.OnCandidateSelected(this, cse);
                return;
            }
        }

        #endregion

        #region 追加イベント

        /// <summary>
        /// 候補ドロップダウンを開く前に発生するイベントです。
        /// </summary>
        [MetCategory("MetPropertyChange")]
        [MetDescription("AutoCompleteBoxCandidateBoxOpening")]
        public event EventHandler CandidateBoxOpening;

        /// <summary>
        /// 候補ドロップダウンを開く前に発生します。
        /// </summary>
        /// <param name="sender">呼出元オブジェクト。</param>
        /// <param name="e">EventArgs オブジェクト。</param>
        protected virtual void OnCandidateBoxOpening(object sender, EventArgs e)
        {
            this.CandidateBoxOpening?.Invoke(sender, e);
        }

        /// <summary>
        /// 候補ドロップダウンを開いた時に発生するイベントです。
        /// </summary>
        [MetCategory("MetPropertyChange")]
        [MetDescription("AutoCompleteBoxCandidateBoxOpened")]
        public event EventHandler CandidateBoxOpened;

        /// <summary>
        /// 候補ドロップダウンを開いた時に発生します。
        /// </summary>
        /// <param name="sender">呼出元オブジェクト。</param>
        /// <param name="e">EventArgs オブジェクト。</param>
        protected virtual void OnCandidateBoxOpened(object sender, EventArgs e)
        {
            this.CandidateBoxOpened?.Invoke(sender, e);
        }

        /// <summary>
        /// 候補ドロップダウンを閉じた時に発生するイベントです。
        /// </summary>
        [MetCategory("MetPropertyChange")]
        [MetDescription("AutoCompleteBoxCandidateBoxClosed")]
        public event EventHandler CandidateBoxClosed;

        /// <summary>
        /// 候補ドロップダウンを閉じた時に発生します。
        /// </summary>
        /// <param name="sender">呼出元オブジェクト。</param>
        /// <param name="e">EventArgs オブジェクト。</param>
        protected virtual void OnCandidateBoxClosed(object sender, EventArgs e)
        {
            this.CandidateBoxClosed?.Invoke(sender, e);
        }

        /// <summary>
        /// 候補の値が選択された時に発生するイベントです。
        /// </summary>
        [MetCategory("MetPropertyChange")]
        [MetDescription("AutoCompleteBoxCandidateSelected")]
        public event CandidateSelectedEventHandler CandidateSelected;

        /// <summary>
        /// 候補の値が選択された時に発生します。
        /// </summary>
        /// <param name="sender">呼出元オブジェクト。</param>
        /// <param name="e">CandidateSelectedEventArgs オブジェクト。</param>
        protected virtual void OnCandidateSelected(object sender, CandidateSelectedEventArgs e)
        {
            this.CandidateSelected?.Invoke(sender, e);
        }

        #endregion

        #region プロパティ

        private object dataSource = null;

        // DataSet, DataTable, IList 以外のオブジェクトが指定されたらエラーにする。
        /// <summary>
        /// オートコンプリートに利用される全データを含むデータソースを取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(null)]
        [Bindable(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [AttributeProvider(typeof(IListSource))]
        [MetDescription("AutoCompleteBoxDataSource")]
        public object DataSource
        {
            get => this.dataSource;
            set
            {
                if (value != null && !(value is DataSet) && !(value is DataTable) && !(value is IList))
                {
                    //throw new ArgumentException("Complex DataBinding はIList または IListSource のどちらかをデータソースとして受け入れます。");
                    throw new ArgumentException("IList または DataSet, DataTable のいずれかをデータソースとして受け入れます。");
                }
                this.dataSource = value;
            }
        }

        /// <summary>
        /// このコントロール内の項目に対して表示するプロパティを示します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue("")]
        [MetDescription("AutoCompleteBoxDisplayMember")]
        public string DisplayMember { get; set; } = "";

        /// <summary>
        /// コントロール内のアイテムに対して、実際の値として使用するプロパティを示します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue("")]
        [MetDescription("AutoCompleteBoxValueMember")]
        public string ValueMember { get; set; } = "";

        /// <summary>
        /// オートコンプリートの表示するアイテム数を設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(8)]
        [MetDescription("AutoCompleteBoxMaxDropDownItems")]
        public int MaxDropDownItems { get => this.candidateBox.MaxDropDownItems; set => this.candidateBox.MaxDropDownItems = value; }

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
        /// CompareOptions が既定値から変更されたかどうかを返却する。
        /// </summary>
        /// <returns>true:変更された, false:変更されていない</returns>
        private bool ShouldSerializeCompareOptions() => this.CompareOptions != null && this.CompareOptions != this.defaultCompareOptions;

        /// <summary>
        /// CompareOptions のリセット操作を行う。
        /// </summary>
        private void ResetCompareOptions() => this.CompareOptions = defaultCompareOptions;

        private Control targetControl = null;

        /// <summary>
        /// オートコンプリートの利用を行うコントロールを取得します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(null)]
        public Control TargetControl
        {
            get => this.targetControl;
            set
            {
                // インスタンス生成時に紐づけを行った場合は変更不可
                if (isAssociatedControl)
                {
                    return;
                }
                this.targetControl = value;
            }
        }

        /// <summary>
        /// 実際に画面に表示するコンボボックスの取得を行います。
        /// </summary>
        [Browsable(false)]
        private AutoCompleteCandidateBox candidateBox { get; } = new AutoCompleteCandidateBox();

        /// <summary>
        /// 現在選択中の候補インデックスを取得します。
        /// </summary>
        [Browsable(false)]
        public int SelectedIndex { get; private set; } = -1;

        /// <summary>
        /// 現在選択中の ValueMember プロパティで指定したメンバー プロパティの値を取得します。
        /// </summary>
        [Browsable(false)]
        public object SelectedValue { get; private set; } = null;

        /// <summary>
        /// 現在選択中の候補アイテムを取得します。
        /// </summary>
        [Browsable(false)]
        public object SelectedItem { get; private set; } = null;

        /// <summary>
        /// オートコンプリートのボックスが開かれているかどうかを取得します。
        /// </summary>
        [Browsable(false)]
        public bool IsOpened { get => this.candidateBox.IsOpened; }

        #endregion

        #region メソッド

        // 直前の候補件数
        private int preCandidateItemCount = -1;

        // 内部的に候補値が設定され、SelectedValueChanged イベントを走行させないためのフラグ
        private bool innerSelectedChanging = false;

        /// <summary>
        /// オートコンプリートのボックスを開きます。
        /// </summary>
        /// <param name="text">候補の検索に利用するテキスト。</param>
        public void Open(string text = "")
        {
            this.OnCandidateBoxOpening(this, EventArgs.Empty);

            // 最初に初期化しないとDataSourceの反映ができないため
            this.candidateBox.InitializeControl(this.targetControl);

            // 既に入力されている値での絞り込み
            this.Extract(text);

            // 候補がある時だけ表示する
            if (this.GetCandidateCount() > 0)
            {
                this.candidateBox.Open();
            }
        }

        /// <summary>
        /// オートコンプリートのボックスを閉じます。
        /// </summary>
        public void Close()
        {
            this.candidateBox.Close();
        }

        /// <summary>
        /// 対象アイテムからデータソースのインデックスを取得する。
        /// </summary>
        /// <param name="item">アイテム。</param>
        /// <returns>インデックス。</returns>
        private int GetIndex(object item)
        {
            // DataSet または DataTable の場合
            if (this.DataSource is DataSet || this.DataSource is DataTable)
            {
                var sourceDt = (this.DataSource as DataTable) ?? (this.DataSource as DataSet)?.Tables[0];
                foreach (var row in sourceDt.AsEnumerable())
                {
                    if (row.Equals(item))
                    {
                        return sourceDt.Rows.IndexOf(row);
                    }
                }
                return -1;
            }

            // IList(List<T>)の場合の処理
            var sourceList = this.DataSource as IList;
            if (sourceList != null)
            {
                if (sourceList.Contains(item))
                {
                    return sourceList.IndexOf(item);
                }
                return -1;
            }

            return -1;
        }

        /// <summary>
        /// 対象インデックスからデータソースのアイテムを取得する。
        /// </summary>
        /// <param name="index">インデックス。</param>
        /// <returns>アイテム。</returns>
        private object GetItem(int index)
        {
            // DataSet または DataTable の場合
            if (this.DataSource is DataSet || this.DataSource is DataTable)
            {
                var sourceDt = (this.DataSource as DataTable) ?? (this.DataSource as DataSet)?.Tables[0];
                if (sourceDt != null)
                {
                    return sourceDt.Rows[index];
                }
            }

            // IList(List<T>)の場合の処理
            var sourceList = this.DataSource as IList;
            if (sourceList != null)
            {
                return sourceList[index];
            }

            return null;
        }

        /// <summary>
        /// アイテムの値を取得する。
        /// </summary>
        /// <param name="item">アイテム。</param>
        /// <returns>実際の値。</returns>
        private object GetDisplay(object item)
        {
            if (this.DisplayMember == "" && this.ValueMember == "")
            {
                return item;
            }

            string member = this.DisplayMember;
            if (member == "")
            {
                member = this.ValueMember;
            }

            // DataSet または DataTable の場合
            if (this.DataSource is DataSet || this.DataSource is DataTable)
            {

                var row = item as DataRow;
                if (row == null)
                {
                    return item;
                }
                return row[member];
            }

            // IListの場合の処理
            var sourceList = this.DataSource as IList;
            if (sourceList != null)
            {
                PropertyDescriptor descriptor = TypeDescriptor.GetProperties(item).Find(member, true);
                if (descriptor == null)
                {
                    return item;
                }
                return descriptor.GetValue(item);
            }
            return item;
        }

        /// <summary>
        /// アイテムの実際の値を取得する。
        /// </summary>
        /// <param name="item">アイテム。</param>
        /// <returns>実際の値。</returns>
        private object GetValue(object item)
        {
            if (this.ValueMember == "")
            {
                return item;
            }

            // DataSet または DataTable の場合
            if (this.DataSource is DataSet || this.DataSource is DataTable)
            {

                var row = item as DataRow;
                if (row == null)
                {
                    return item;
                }
                return row[this.ValueMember];
            }

            // IListの場合の処理
            var sourceList = this.DataSource as IList;
            if (sourceList != null)
            {
                PropertyDescriptor descriptor = TypeDescriptor.GetProperties(item).Find(this.ValueMember, true);
                if (descriptor == null)
                {
                    return item;
                }
                return descriptor.GetValue(item);
            }
            return item;
        }

        /// <summary>
        /// テキストからデータソースのアイテムを決定します。
        /// </summary>
        /// <param name="text">テキスト。</param>
        public void SelectItem(string text)
        {
            this.SelectedItem = null;
            this.SelectedValue = null;
            this.SelectedIndex = -1;

            var index = this.IndexOfAllItem(text.ToString());
            if (index == -1)
            {
                return;
            }

            // 入力値が全体候補に存在する場合は選択状態とする
            this.SelectedItem = this.GetItem(index);
            this.SelectedValue = this.GetValue(this.SelectedItem);
            this.SelectedIndex = index;
            var cse = new CandidateSelectedEventArgs(this.SelectedItem, this.SelectedValue, text, this.SelectedIndex);
            this.OnCandidateSelected(this, cse);
        }

        /// <summary>
        /// アイテムからデータソースのアイテムを決定します。
        /// </summary>
        /// <param name="item">選択オブジェクト。</param>
        public void SelectItem(object item)
        {
            this.SelectedItem = null;
            this.SelectedValue = null;
            this.SelectedIndex = -1;

            var index = GetIndex(item);
            if (index == -1)
            {
                return;
            }

            // 入力値が全体候補に存在する場合は選択状態とする
            this.SelectedItem = item;
            this.SelectedValue = this.GetValue(this.SelectedItem);
            this.SelectedIndex = index;
            var cse = new CandidateSelectedEventArgs(this.SelectedItem, this.SelectedValue, GetDisplay(item).ToString(), this.SelectedIndex);
            this.OnCandidateSelected(this, cse);
        }

        /// <summary>
        /// 全体の入力候補より絞り込みを行います。
        /// </summary>
        /// <param name="text">候補を絞り込む文字列。</param>
        public void Extract(string text)
        {
            // データソースを再設定する前に選択済みだったアイテムを保持
            var preItem = this.SelectedItem;

            this.innerSelectedChanging = true;
            this.candidateBox.DisplayMember = "";
            this.candidateBox.ValueMember = "";
            this.candidateBox.DataSource = null;

            this.candidateBox.DisplayMember = this.DisplayMember;
            this.candidateBox.ValueMember = this.ValueMember;
            this.candidateBox.DataSource = this.ExtractObject(text);

            this.candidateBox.SelectedIndex = -1;
            this.innerSelectedChanging = false;

            // 選択済みのアイテムがない状態でドロップダウンを開いた時
            // ドロップダウンが開いている状態で、選択済みのアイテムがなく、手入力によって選択済みアイテムが決定した時
            if (preItem == null)
            {
                var i = this.IndexOfCandidate(text);
                if (i > -1)
                {
                    this.candidateBox.SelectedIndex = i;
                }
                this.ReOpenDropdown();
                return;
            }

            // 入力された値で設定値を決定する
            this.DecideItem(preItem, text);

            this.ReOpenDropdown();
        }

        // IListの実装を行う。
        /// <summary>
        /// ドロップダウンからアイテムを決定する。
        /// </summary>
        /// <param name="preItem">直前の選択アイテムオブジェクト。</param>
        /// <param name="text">テキスト。</param>
        private void DecideItem(object preItem, string text)
        {
            if (this.DataSource is DataSet || this.DataSource is DataTable)
            {
                this.DecideItemForDataTable((DataRow)preItem, text);
                return;
            }

            if (this.DataSource is IList)
            {
                this.DecideItemForLIst(preItem, text);
                return;
            }
        }

        /// <summary>
        /// データソースが DataSet または DataTable だった時に、ドロップダウンからアイテムを決定する。
        /// </summary>
        /// <param name="preItem">直前の選択アイテムオブジェクト。</param>
        /// <param name="text">テキスト。</param>
        private void DecideItemForDataTable(DataRow preItem, string text)
        {
            // 値が確定していて、ドロップダウンを開いた時に、内部の SelectedItem を設定し直す
            if (text == this.GetDisplay(preItem).ToString())
            {
                var source = (DataView)this.candidateBox.DataSource;
                foreach (DataRowView row in source)
                {
                    if (row.Row == preItem)
                    {
                        this.innerSelectedChanging = true;
                        this.candidateBox.SelectedItem = row;
                        this.innerSelectedChanging = false;
                        break;
                    }
                }
                return;
            }

            // ドロップダウンが表示されている状態で、削除などにより、確定した値から確定した値に変更された時
            var index = this.IndexOfCandidate(text);
            if (index > -1)
            {
                this.candidateBox.SelectedIndex = index;
                return;
            }

            // ドロップダウンが表示されている状態で、削除などにより、確定した値から未確定の値に変更された時
            this.SelectedItem = null;
            this.SelectedValue = null;
            this.SelectedIndex = -1;
        }

        /// <summary>
        /// データソースが IList だった時に、ドロップダウンからアイテムを決定する。
        /// </summary>
        /// <param name="preItem">直前の選択アイテムオブジェクト。</param>
        /// <param name="text">テキスト。</param>
        private void DecideItemForLIst(object preItem, string text)
        {
            // 値が確定していて、ドロップダウンを開いた時に、内部の SelectedItem を設定し直す
            if (text == this.GetDisplay(preItem).ToString())
            {
                var source = (IList)this.candidateBox.DataSource;
                foreach (var item in source)
                {
                    if (item == preItem)
                    {
                        this.innerSelectedChanging = true;
                        this.candidateBox.SelectedItem = item;
                        this.innerSelectedChanging = false;
                        break;
                    }
                }
                return;
            }

            // ドロップダウンが表示されている状態で、削除などにより、確定した値から確定した値に変更された時
            var index = this.IndexOfCandidate(text);
            if (index > -1)
            {
                this.candidateBox.SelectedIndex = index;
                return;
            }

            // ドロップダウンが表示されている状態で、削除などにより、確定した値から未確定の値に変更された時
            this.SelectedItem = null;
            this.SelectedValue = null;
            this.SelectedIndex = -1;
        }

        /// <summary>
        /// ドロップダウンを開き直す必要がある場合に開き直す。
        /// </summary>
        private void ReOpenDropdown()
        {
            // ドロップダウンの件数が、直前のドロップダウンの件数より多く、ドロップダウンが開いていた時に開き直す
            var candidateItemCount = this.GetCandidateCount();
            if (this.candidateBox.IsOpened && candidateItemCount > 0 && candidateItemCount > this.preCandidateItemCount)
            {
                this.candidateBox.ReOpen();
            }
            this.preCandidateItemCount = candidateItemCount;
        }

        /// <summary>
        /// 現在のドロップダウン候補数を取得します。
        /// </summary>
        /// <returns>ドロップダウン候補数。</returns>
        public int GetCandidateCount()
        {
            if (this.candidateBox.DataSource is IList)
            {
                return ((IList)this.candidateBox.DataSource).Count;
            }
            return 0;
        }

        /// <summary>
        /// データソースからテキストを検索し、アイテムインデックスを取得する。
        /// </summary>
        /// <param name="text">検索する文字列。</param>
        /// <returns>アイテムインデックス。</returns>
        private int IndexOfAllItem(string text)
        {
            // DataSet または DataTable の場合
            if (this.DataSource is DataSet || this.DataSource is DataTable)
            {
                var sourceDt = (this.DataSource as DataTable) ?? (this.DataSource as DataSet)?.Tables[0];
                if (sourceDt != null)
                {
                    return this.IndexOfItemForDataTable(sourceDt, text);
                }
            }

            // IList の場合の処理
            var sourceList = this.DataSource as IList;
            if (sourceList != null)
            {
                return this.IndexOfItemForIList(sourceList, text);
            }

            return -1;
        }

        /// <summary>
        /// 現在の候補からテキストを検索し、アイテムインデックスを取得する。
        /// </summary>
        /// <param name="text">検索するテキスト。</param>
        /// <returns>現在の入力候補アイテムのインデックス。</returns>
        private int IndexOfCandidate(string text)
        {
            // DataView も IList もいける
            var sourceList = this.candidateBox.DataSource as IList;
            if (sourceList != null)
            {
                return this.IndexOfItemForIList(sourceList, text);
            }

            return -1;
        }

        /// <summary>
        /// IList オブジェクト内のプロパティで、テキストと合致するインデックスを取得する。
        /// </summary>
        /// <param name="list">IList オブジェクト。</param>
        /// <param name="text">検索するテキスト。</param>
        /// <returns>アイテムのインデックス。</returns>
        private int IndexOfItemForIList(IList list, string text)
        {
            if (this.DisplayMember == "" && this.ValueMember == "")
            {
                return -1;
            }

            string member = this.DisplayMember;
            if (member == "")
            {
                member = this.ValueMember;
            }

            // 対象プロパティに入っている文字列が合致する行を候補とする
            var i = 0;
            foreach (var item in list)
            {
                PropertyDescriptor descriptor = TypeDescriptor.GetProperties(item).Find(member, true);
                if (descriptor == null)
                {
                    return -1;
                }
                var displayText = descriptor.GetValue(item).ToString();
                if (displayText == text)
                {
                    return i;
                }
                i++;
            }
            return -1;
        }

        /// <summary>
        /// DataTable オブジェクト内のプロパティで、テキストと合致するインデックスを取得する。
        /// </summary>
        /// <param name="dt">DataTable オブジェクト。</param>
        /// <param name="text">検索するテキスト。</param>
        /// <returns>アイテムのインデックス。</returns>
        private int IndexOfItemForDataTable(DataTable dt, string text)
        {
            // 対象プロパティに入っている文字列が合致する行を候補とする
            var i = 0;
            foreach (var row in dt.AsEnumerable())
            {
                var displayText = this.GetDisplay(row).ToString();
                if (displayText == text)
                {
                    return i;
                }
                i++;
            }
            return -1;
        }

        /// <summary>
        /// 候補の絞り込みを行う。
        /// </summary>
        /// <param name="text">テキスト。</param>
        /// <returns>絞り込みデータオブジェクト。</returns>
        private object ExtractObject(string text)
        {
            var compareOptions = this.GetCompareOptions();

            var dataSource = this.DataSource;

            // DataBindings に DataSource が設定されている場合、そちらを優先する
            var dataSourceBinding = this.DataBindings.OfType<Binding>().Where(binding => binding.PropertyName == "DataSource").FirstOrDefault();
            if (dataSourceBinding != null)
            {
                var bindingFieldProperty = dataSourceBinding.DataSource.GetType().GetProperty(dataSourceBinding.BindingMemberInfo.BindingField);
                if (bindingFieldProperty != null)
                {
                    dataSource = bindingFieldProperty.GetValue(dataSourceBinding.DataSource);
                    candidateBox.DisplayMember = this.DisplayMember;
                    candidateBox.ValueMember = this.ValueMember;
                }
            }

            if (dataSource is DataSet)
            {
                return this.ExtractDataSet((DataSet)dataSource, text, compareOptions);
            }

            if (dataSource is DataTable)
            {
                return this.ExtractDataTable((DataTable)dataSource, text, compareOptions);
            }

            if (dataSource is IList)
            {
                return this.ExtractList((IList)dataSource, text, compareOptions);
            }

            return null;
        }

        /// <summary>
        /// 候補判断を行うオプションを取得する。
        /// </summary>
        /// <returns>CompareOptions オブジェクト。</returns>
        private CompareOptions GetCompareOptions()
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
        /// <param name="dataSource">処理対象のデータソース。</param>
        /// <param name="text">テキスト。</param>
        /// <param name="compareOptions">候補判断。</param>
        /// <returns>DataView オブジェクト。</returns>
        private DataView ExtractDataSet(DataSet dataSource, string text, CompareOptions compareOptions)
        {
            var dt = dataSource.Tables[0];
            return this.ExtractDataTable(dt, text, compareOptions);
        }

        /// <summary>
        /// DataTable オブジェクトのリストデータを取得する。
        /// </summary>
        /// <param name="dataSource">処理対象のデータソース。</param>
        /// <param name="text">テキスト。</param>
        /// <param name="compareOptions">候補判断。</param>
        /// <returns>DataView オブジェクト。</returns>
        private DataView ExtractDataTable(DataTable dataSource, string text, CompareOptions compareOptions)
        {
            EnumerableRowCollection<DataRow> query = from order in dataSource.AsEnumerable()
                                                     where IsMatch(this.GetDisplay(order).ToString(), text, compareOptions)
                                                     select order;
            return query.AsDataView();
        }

        /// <summary>
        /// IList オブジェクトのリストデータを取得する。
        /// </summary>
        /// <param name="dataSource">処理対象のデータソース。</param>
        /// <param name="text">テキスト。</param>
        /// <param name="compareOptions">候補判断。</param>
        /// <returns>IList オブジェクト。</returns>
        private IList ExtractList(IList dataSource, string text, CompareOptions compareOptions)
        {
            var result = new List<object>();

            foreach (var value in dataSource)
            {
                if (this.IsMatch(this.GetDisplay(value).ToString(), text, compareOptions))
                {
                    result.Add(value);
                }
            }
            return result;
        }

        /// <summary>
        /// 対象のテキストが候補対象となるかを確認する。
        /// </summary>
        /// <param name="candidateText">候補文字列。</param>
        /// <param name="searchText">検索文字列。</param>
        /// <param name="compareOptions">CompareOptions オブジェクト。</param>
        /// <returns>true:候補対象, false:候補対象でない。</returns>
        private bool IsMatch(string candidateText, string searchText, CompareOptions compareOptions)
        {
            // 検索文字列が空の場合は候補とする
            if (string.IsNullOrEmpty(searchText))
            {
                return true;
            }

            var ci = CultureInfo.CurrentCulture.CompareInfo;
            switch (MatchPattern)
            {
                case MatchPatternType.StartsWith:
                    if (ci.IndexOf(candidateText, searchText, compareOptions) != 0)
                    {
                        return false;
                    }
                    break;
                case MatchPatternType.EndsWith:
                    if (ci.LastIndexOf(candidateText, searchText, compareOptions) != candidateText.Length - searchText.Length)
                    {
                        return false;
                    }
                    break;
                case MatchPatternType.Partial:
                    if (ci.IndexOf(candidateText, searchText, compareOptions) < 0)
                    {
                        return false;
                    }
                    break;
                case MatchPatternType.Equal:
                    if (ci.IndexOf(candidateText, searchText, compareOptions) < 0 || candidateText.Length != searchText.Length)
                    {
                        return false;
                    }
                    break;
            }
            return true;
        }

        #endregion

        #region IBindableComponent 実装

        private BindingContext bindingContext;
        private ControlBindingsCollection dataBindings;

        /// <summary>
        /// 使わない。
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public BindingContext BindingContext
        {
            get
            {
                if (bindingContext == null)
                {
                    bindingContext = new BindingContext();
                }
                return bindingContext;
            }
            set
            {
                bindingContext = value;
            }
        }

        /// <summary>
        /// コントロールのデータバインドです。DataSource をバインドすることができます。
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [MetDescription("AutoCompleteBoxDataBindings")]
        public ControlBindingsCollection DataBindings
        {
            get
            {
                if (dataBindings == null)
                {
                    dataBindings = new ControlBindingsCollection(this);
                }
                return dataBindings;
            }
        }

        #endregion
    }
}
