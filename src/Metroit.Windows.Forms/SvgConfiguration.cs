using System.ComponentModel;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// ボタン開閉のイメージ設定を提供します。
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class SvgConfiguration : INotifyPropertyChanged
    {
        /// <summary>
        /// 開いた時のSVGイメージ設定を取得します。
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [MetCategory("MetAppearance")]
        [MetDescription("SvgConfigurationExpanded")]
        public SvgSetting Expanded { get; } = new SvgSetting();

        /// <summary>
        /// 閉じた時のSVGイメージ設定を取得します。
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [MetCategory("MetAppearance")]
        [MetDescription("SvgConfigurationCollapsed")]
        public SvgSetting Collapsed { get; } = new SvgSetting();

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        public SvgConfiguration()
        {
            Expanded.PropertyChanged += Expanded_PropertyChanged;
            Collapsed.PropertyChanged += Collapsed_PropertyChanged;
        }

        /// <summary>
        /// プロパティ値に変更があった場合に発生します。
        /// </summary>
        [MetCategory("MetPropertyChange")]
        [MetDescription("SvgConfigurationPropertyChanged")]
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 開いた時のSVGイメージ設定の何かが変更された時にイベントを発生させる。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Expanded_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Expanded)));
        }

        /// <summary>
        /// 閉じた時のSVGイメージ設定の何かが変更された時にイベントを発生させる。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Collapsed_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Collapsed)));
        }
    }
}
