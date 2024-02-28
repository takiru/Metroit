using Metroit.Windows.Forms.Properties;
using Svg;
using Svg.Transforms;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// SVGイメージの設定を提供します。
    /// </summary>
    [ToolboxItem(true)]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class SvgSetting : INotifyPropertyChanged
    {
        private SvgImages image = SvgImages.ArrowCircleDownFilled;

        /// <summary>
        /// SVGイメージを取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof(SvgImages), "ArrowCircleDownFilled")]
        [MetCategory("MetAppearance")]
        [MetDescription("SvgSettingImage")]
        public SvgImages Image
        {
            get => image;
            set => SetProperty(ref image, value);
        }

        private bool useColor = false;

        /// <summary>
        /// SVGイメージのFillカラーを塗りつぶすかどうかを取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(false)]
        [MetCategory("MetAppearance")]
        [MetDescription("SvgSettingUseColor")]
        public bool UseColor
        {
            get => useColor;
            set => SetProperty(ref useColor, value);
        }

        private Color color = SystemColors.Control;

        /// <summary>
        /// SVGイメージの塗りつぶすFillカラーを取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof(Color), "Control")]
        [MetCategory("MetAppearance")]
        [MetDescription("SvgSettingColor")]
        public Color Color
        {
            get => color;
            set => SetProperty(ref color, value);
        }

        private float rotation = 0;

        /// <summary>
        /// SVGイメージの回転角度を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(0)]
        [MetCategory("MetAppearance")]
        [MetDescription("SvgSettingRotation")]
        public float Rotation
        {
            get => rotation;
            set => SetProperty(ref rotation, value);
        }

        private MemoryStream svgStream = null;

        /// <summary>
        /// Image = Custom の場合に利用するSVGイメージを取得または設定します。
        /// </summary>
        [Browsable(false)]
        public MemoryStream SvgStream
        {
            get => svgStream;
            set => SetProperty(ref svgStream, value);
        }

        /// <summary>
        /// プロパティ値に変更があった場合に発生します。
        /// </summary>
        [Browsable(true)]
        [MetCategory("MetPropertyChange")]
        [MetDescription("SvgSettingPropertyChanged")]
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
        /// 設定に応じたSVGイメージを生成する。
        /// </summary>
        /// <param name="width">生成するイメージの幅。</param>
        /// <param name="height">生成するイメージの高さ。</param>
        /// <returns>SVGイメージ。</returns>
        public Image Generate(int width, int height)
        {
            MemoryStream stream;
            if (Image == SvgImages.Custom)
            {
                stream = SvgStream ?? new MemoryStream(Resources.ArrowCircleDownFilled);
            }
            else
            {
                stream = new MemoryStream((byte[])Resources.ResourceManager.GetObject(Image.ToString()));
            }

            var svgDoc = SvgDocument.Open<SvgDocument>(stream);

            // 回転
            if (Rotation != 0)
            {
                var tc = new SvgTransformCollection()
                {
                    new SvgRotate(Rotation, svgDoc.Width / 2, svgDoc.Height / 2)
                };
                svgDoc.Transforms = tc;
            }

            // 色
            if (UseColor)
            {
                ChangeSvgFillColor(svgDoc.Descendants(), new SvgColourServer(Color));
            }

            return svgDoc.Draw(width, height);
        }

        /// <summary>
        /// SVGイメージのFillカラーを変更する。
        /// </summary>
        /// <param name="nodes">SVGイメージノード。</param>
        /// <param name="colorServer">変更カラー。</param>
        private void ChangeSvgFillColor(IEnumerable<SvgElement> nodes, SvgPaintServer colorServer)
        {
            foreach (var node in nodes)
            {
                if (node.Fill != SvgPaintServer.None)
                {
                    node.Fill = colorServer;
                }

                ChangeSvgFillColor(node.Descendants(), colorServer);
            }
        }

        /// <summary>
        /// イメージ設定を返却します。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Image.ToString();
        }
    }
}
