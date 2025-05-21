using System;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// ローディングアニメーションの構成を提供します。
    /// </summary>
    public class LoadingAnimationSetting
    {
        /// <summary>
        /// ローディングアニメーションの拡大割合種類を取得または設定します。
        /// </summary>
        public LoadingAnimationSizeUnit SizeUnit { get; set; } = LoadingAnimationSizeUnit.Pixel;

        private int _size = 100;

        /// <summary>
        /// ローディングアニメーションの拡大割合を取得または設定します。
        /// <see cref="SizeUnit"/> が <see cref="LoadingAnimationSizeUnit.Contain"/> のときは無視されます。
        /// </summary>
        public int Size
        {
            get => _size;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(Size));
                }

                _size = value;
            }
        }

        /// <summary>
        /// ローディングアニメーションを取得または設定します。
        /// </summary>
        public LoadingAnimationImage Image { get; set; } = LoadingAnimationImage.Spin;

        /// <summary>
        /// カスタムのローディングアニメーションとなるSVGデータのバイトを取得または設定します。
        /// </summary>
        public byte[] CustomImageBytes { get; set; } = null;
    }
}
