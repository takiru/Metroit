using System.ComponentModel;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// ボタン開閉のSVGイメージ設定を提供します。
    /// </summary>
    [ToolboxItem(true)]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class ImageConfiguration : INotifyPropertyChanged
    {
        /// <summary>
        /// 開いた時のイメージ設定を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ImageSetting Expanded { get; } = new ImageSetting();

        /// <summary>
        /// 閉じた時のイメージ設定を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ImageSetting Collapsed { get; } = new ImageSetting();

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        public ImageConfiguration()
        {
            Expanded.PropertyChanged += Expanded_PropertyChanged;
            Collapsed.PropertyChanged += Collapsed_PropertyChanged;
        }

        /// <summary>
        /// プロパティ値に変更があった場合に発生します。
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 開いた時のイメージ設定の何かが変更された時にイベントを発生させる。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Expanded_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Expanded)));
        }

        /// <summary>
        /// 閉じた時のイメージ設定の何かが変更された時にイベントを発生させる。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Collapsed_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Collapsed)));
        }
    }
}
