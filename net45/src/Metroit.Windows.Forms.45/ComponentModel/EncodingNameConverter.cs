using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metroit.Windows.Forms.ComponentModel
{
    /// <summary>
    /// 文字エンコーディング名を選択可能にするコンバーターを提供します。
    /// </summary>
    public class EncodingNameConverter : TypeConverter
    {
        /// <summary>
        /// 変換可能な文字列かどうかを取得します。
        /// </summary>
        /// <param name="context">コンテキスト情報。</param>
        /// <param name="sourceType">変換元のタイプ。</param>
        /// <returns>true:変換可能, false:変換不可能。</returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                //文字列から変換可能
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        /// <summary>
        /// 文字列から文字エンコーディングに変換します。
        /// </summary>
        /// <param name="context">コンテキスト情報。</param>
        /// <param name="culture">カルチャー情報。</param>
        /// <param name="value">変換元の値。</param>
        /// <returns>文字エンコーディング。</returns>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (!(value is string))
            {
                return base.ConvertFrom(context, culture, value);
            }

            if (context.PropertyDescriptor.PropertyType != typeof(Encoding))
            {
                return null;
            }

            //一覧の文字列と一致するオブジェクトを探して返す
            string s = (string)value;
            var ei = Encoding.GetEncodings()
                    .Select((n) => new { n, encoding = n.GetEncoding() })
                    .FirstOrDefault(x => x.encoding.WebName + " | " + x.encoding.EncodingName == s);

            if (ei == null)
            {
                return null;
            }

            return ei.encoding;
        }

        /// <summary>
        /// 文字列の場合、変換可能を得ます。
        /// </summary>
        /// <param name="context">コンテキスト情報。</param>
        /// <param name="destinationType">変換元のタイプ。</param>
        /// <returns>true:変換可能, false:変換不可能。</returns>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                //文字列に変換可能
                return true;
            }
            return base.CanConvertTo(context, destinationType);
        }

        /// <summary>
        /// 文字エンコーディングから文字列に変換します。
        /// </summary>
        /// <param name="context">コンテキスト情報。</param>
        /// <param name="culture">カルチャー情報。</param>
        /// <param name="value">変換元の値。</param>
        /// <param name="destinationType">変換元のタイプ。</param>
        /// <returns>文字列。</returns>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (value is Encoding && destinationType == typeof(string))
            {
                //文字列化
                var encoding = (Encoding)value;
                return encoding.WebName + " | " + encoding.EncodingName;
            }
            else
            {
                return base.ConvertTo(context, culture, value, destinationType);
            }
        }

        /// <summary>
        /// ドロップダウンで選択可能にします。
        /// </summary>
        /// <param name="context">コンテキスト情報。</param>
        /// <returns>true</returns>
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            // ドロップダウンで選択可能にする
            return true;
        }

        /// <summary>
        /// 文字エンコーディング名のリストを取得します。
        /// </summary>
        /// <param name="context">コンテキスト情報。</param>
        /// <returns>文字エンコーディング名のリスト。</returns>
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            var choices = new List<Encoding>();
            foreach (var ei in Encoding.GetEncodings().OrderBy(_ => _.GetEncoding().WebName))
            {
                choices.Add(ei.GetEncoding());
            }

            return new StandardValuesCollection(choices);
        }

        /// <summary>
        /// 選択肢以外の入力を認めません。
        /// </summary>
        /// <param name="context">コンテキスト情報。</param>
        /// <returns>true</returns>
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            // 選択肢以外は選択不可
            return true;
        }
    }
}
