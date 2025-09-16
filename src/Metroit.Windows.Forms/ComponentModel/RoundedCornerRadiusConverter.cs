using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace Metroit.Windows.Forms.ComponentModel
{
    /// <summary>
    /// <see cref="RoundedCornerRadius"/> 型のコンバーターを提供します。
    /// </summary>
    public class RoundedCornerRadiusConverter : TypeConverter
    {
        /// <summary>
        /// コンバーターが特定の型のオブジェクトをコンバーターの型に変換できるかどうかを示す値を返します。
        /// </summary>
        /// <param name="context">コンテキスト。</param>
        /// <param name="sourceType">変換元の型を表す <see cref="Type"/>。</param>
        /// <returns>コンバーターが変換を実行できる場合は <see langword="true"/>。それ以外の場合は <see langword="false"/>。</returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        /// <summary>
        ///  コンバーターがオブジェクトを指定した型に変換できるかどうかを示す値を返します。
        /// </summary>
        /// <param name="context">コンテキスト。</param>
        /// <param name="destinationType">変換先の型を表す <see cref="Type"/>。</param>
        /// <returns>コンバーターが変換を実行できる場合は <see langword="true"/>。それ以外の場合は <see langword="false"/>。</returns>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(string) ||
                   destinationType == typeof(InstanceDescriptor) ||
                   base.CanConvertTo(context, destinationType);
        }

        /// <summary>
        /// 指定したコンテキストとカルチャ情報を使用して、指定したオブジェクトをこのコンバーターの型に変換します。
        /// </summary>
        /// <param name="context">コンテキスト。</param>
        /// <param name="culture">カルチャ情報。</param>
        /// <param name="value">変換対象の <see cref="object"/>。</param>
        /// <returns>変換後の値を表す <see cref="object"/>。</returns>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string str)
            {
                str = str.Trim();
                string[] parts = str.Split(',');

                if (parts.Length != 4)
                {
                    return base.ConvertFrom(context, culture, value);
                }

                if (int.TryParse(parts[0].Trim(), out int topLeft) &&
                    int.TryParse(parts[1].Trim(), out int topRight) &&
                    int.TryParse(parts[3].Trim(), out int bottomRight) &&
                    int.TryParse(parts[2].Trim(), out int bottomLeft))
                {
                    return new RoundedCornerRadius(topLeft, topRight, bottomRight, bottomLeft);
                }
            }

            return base.ConvertFrom(context, culture, value);
        }

        /// <summary>
        /// <see cref="RoundedCornerRadius"/> インスタンスを作成するための必要な情報を生成します。
        /// </summary>
        /// <param name="context">コンテキスト。</param>
        /// <param name="culture">カルチャ情報。</param>
        /// <param name="value">変換対象のオブジェクト。</param>
        /// <param name="destinationType"><paramref name="value"/> パラメーターの変換後の <see cref="Type"/>。</param>
        /// <returns>変換後の値を表す <see cref="object"/>。</returns>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            var radius = (RoundedCornerRadius)value;

            if (destinationType == typeof(string))
            {
                return radius.ToString();
            }

            if (destinationType != typeof(InstanceDescriptor))
            {
                return base.ConvertTo(context, culture, value, destinationType);
            }

            if (radius.ShouldSerializeAll())
            {
                var ctor = typeof(RoundedCornerRadius).GetConstructor(
                    BindingFlags.Public | BindingFlags.Instance,
                    null,
                    new Type[] { typeof(int) },
                    null);
                var args = new object[] { radius.All };
                return new InstanceDescriptor(ctor, args, true);
            }
            else
            {
                var ctor = typeof(RoundedCornerRadius).GetConstructor(
                BindingFlags.Public | BindingFlags.Instance,
                null,
                new Type[] { typeof(int), typeof(int), typeof(int), typeof(int) },
                null);
                var args = new object[] { radius.TopLeft, radius.TopRight, radius.BottomRight, radius.BottomLeft };
                return new InstanceDescriptor(ctor, args, true);
            }
        }

        /// <summary>
        /// <see cref="RoundedCornerRadius"/> のインスタンスを生成できることを示します。
        /// </summary>
        /// <param name="context">コンテキスト。</param>
        /// <returns><see langword="true"/> を返却します。</returns>
        public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        /// <summary>
        /// 変更前と変更後のプロパティ値を比較して、新しいインスタンスを生成します。
        /// </summary>
        /// <param name="context">コンテキスト。</param>
        /// <param name="propertyValues">変更後プロパティのコレクション。</param>
        /// <returns>新しい <see cref="RoundedCornerRadius"/> オブジェクト。</returns>
        public override object CreateInstance(ITypeDescriptorContext context, System.Collections.IDictionary propertyValues)
        {
            RoundedCornerRadius original = (RoundedCornerRadius)context.PropertyDescriptor.GetValue(context.Instance);
            var all = (int)propertyValues[nameof(RoundedCornerRadius.All)];

            // All が変更されたらすべてのRadiusを変更する
            if (all != original.All)
            {
                return new RoundedCornerRadius(all);
            }

            return new RoundedCornerRadius(
                (int)propertyValues[nameof(RoundedCornerRadius.TopLeft)],
                (int)propertyValues[nameof(RoundedCornerRadius.TopRight)],
                (int)propertyValues[nameof(RoundedCornerRadius.BottomRight)],
                (int)propertyValues[nameof(RoundedCornerRadius.BottomLeft)]);
        }

        /// <summary>
        /// <see cref="RoundedCornerRadius"/> のプロパティをサポートします。
        /// </summary>
        /// <param name="context">コンテキスト。</param>
        /// <returns><see langword="true"/> を返却します。</returns>
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        /// <summary>
        /// <see cref="RoundedCornerRadius"/> のプロパティを所定の順序で取得します。
        /// </summary>
        /// <param name="context">コンテキスト。</param>
        /// <param name="value">プロパティを取得する配列の型。</param>
        /// <param name="attributes">フィルターとして使用される <see cref="Attribute"/> 型の配列。</param>
        /// <returns></returns>
        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            return TypeDescriptor.GetProperties(typeof(RoundedCornerRadius), attributes)
                .Sort(new string[] {
                    nameof(RoundedCornerRadius.All),
                    nameof(RoundedCornerRadius.TopLeft),
                    nameof(RoundedCornerRadius.TopRight),
                    nameof(RoundedCornerRadius.BottomRight),
                    nameof(RoundedCornerRadius.BottomLeft)
                });
        }
    }
}