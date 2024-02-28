using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// イメージの設定を提供します。
    /// </summary>
    [ToolboxItem(true)]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class ImageSetting : INotifyPropertyChanged
    {
        private Image image = null;

        /// <summary>
        /// イメージを取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(null)]
        public Image Image
        {
            get => image;
            set => SetProperty(ref image, value);
        }

        private RotateFlipType rotation = RotateFlipType.RotateNoneFlipNone;

        /// <summary>
        /// イメージの回転角度を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof(RotateFlipType), "RotateNoneFlipNone")]
        public RotateFlipType Rotation
        {
            get => rotation;
            set => SetProperty(ref rotation, value);
        }

        /// <summary>
        /// プロパティ値に変更があった場合に発生します。
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// プロパティ値に変更があった場合にイベントを発生させます。
        /// </summary>
        /// <param name="propertyName">プロパティ名。</param>
        protected void OnNotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 値の設定を行い、変更を通知します。
        /// </summary>
        /// <typeparam name="T">設定するプロパティ情報</typeparam>
        /// <param name="field">値を設定する変数。</param>
        /// <param name="value">値。</param>
        /// <param name="propertyName">プロパティ名。</param>
        /// <returns>true:値の設定を行った, false:値の設定を行わなかった。</returns>
        private bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            // 値に変更がない場合は処理しない
            if (Equals(field, value))
            {
                return false;
            }

            field = value;

            OnNotifyPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// 設定に応じたイメージを生成する。
        /// </summary>
        /// <returns>イメージ。</returns>
        public Image Generate()
        {
            if (Image == null)
            {
                return Image;
            }

            var image = (Image)Image.Clone();

            // 回転
            image.RotateFlip(Rotation);

            return Image;
        }
    }
}
