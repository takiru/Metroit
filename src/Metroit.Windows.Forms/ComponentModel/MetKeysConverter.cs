using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Metroit.Windows.Forms.ComponentModel
{
    /// <summary>
    /// MetKeys を扱うコンバーターを提供します。
    /// </summary>
    public class MetKeysConverter : TypeConverter, IComparer
    {
        /// <summary>
        /// デザイナで表示するときのマッピングされた修飾子キーの順序
        /// </summary>
        private static readonly List<string> OrderModifierNames = new List<string> { "Ctrl", "Alt", "Shift" };

        /// <summary>
        /// キーと文字列のマッピング
        /// </summary>
        private Dictionary<string, MetKeys> keyNameMap;

        private StandardValuesCollection standardValues;

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        public MetKeysConverter()
        {
            RegisterKeyNameMaps();
        }

        /// <summary>
        /// キーコードごとに、プロパティデザイナ上で表現する文字列を登録する。
        /// </summary>
        private void RegisterKeyNameMaps()
        {
            keyNameMap = new Dictionary<string, MetKeys>(14);

            RegisterKeyNameMap("Enter", MetKeys.Return);
            RegisterKeyNameMap("Ctrl", MetKeys.Control);
            RegisterKeyNameMap("Alt", MetKeys.Alt);
            RegisterKeyNameMap("Shift", MetKeys.Shift);
            RegisterKeyNameMap("PageDown", MetKeys.Next);
            RegisterKeyNameMap("PageUp", MetKeys.Prior);
            RegisterKeyNameMap("0", MetKeys.D0);
            RegisterKeyNameMap("1", MetKeys.D1);
            RegisterKeyNameMap("2", MetKeys.D2);
            RegisterKeyNameMap("3", MetKeys.D3);
            RegisterKeyNameMap("4", MetKeys.D4);
            RegisterKeyNameMap("5", MetKeys.D5);
            RegisterKeyNameMap("6", MetKeys.D6);
            RegisterKeyNameMap("7", MetKeys.D7);
            RegisterKeyNameMap("8", MetKeys.D8);
            RegisterKeyNameMap("9", MetKeys.D9);
        }

        /// <summary>
        /// 指定したキーコードの、プロパティデザイナ上で表現する文字列を登録する。
        /// </summary>
        /// <param name="displayText">デザイナ上で表現する文字列。</param>
        /// <param name="value">実際のキーコード。</param>
        private void RegisterKeyNameMap(string displayText, MetKeys value)
        {
            keyNameMap[displayText] = value;
        }

        /// <summary>
        /// キーコードに変換可能かどうかを取得します。
        /// sourceType が string か Enum[] の場合に変換可能です。
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sourceType">変換対象となる Type。</param>
        /// <returns>true:変換可能, false:変換不可能。</returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || sourceType == typeof(Enum[]) || base.CanConvertFrom(context, sourceType);
        }

        /// <summary>
        /// 文字列に変換可能かどうかを取得します。
        /// destinationType が Enum[] の場合に変換可能です。
        /// </summary>
        /// <param name="context"></param>
        /// <param name="destinationType">変換対象となる Type。</param>
        /// <returns>true:変換可能, false:変換不可能。</returns>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(Enum[]) || base.CanConvertTo(context, destinationType);
        }

        /// <summary>
        /// キーコードと表現文字列の変換マップをソートする時の比較を行います。
        /// </summary>
        /// <param name="x">比較元の値。</param>
        /// <param name="y">比較先の値。</param>
        /// <returns></returns>
        public int Compare(object x, object y)
        {
            return string.Compare(ConvertToString(x), ConvertToString(y), false, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// デザインの値からキーコードを求める。
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="FormatException"></exception>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            bool isStringValue = value is string;
            bool isEnumValue = value is Enum[];
            if (!(isStringValue | isEnumValue))
            {
                return base.ConvertFrom(context, culture, value);
            }

            if (isStringValue)
            {
                var keyElements = value.ToString().Split('+').Select(x => x.Trim()).ToArray();
                if (keyElements.Length == 0)
                {
                    return null;
                }

                MetKeys resultKey = MetKeys.None;
                bool hasKeyCode = false;

                foreach (var keyElement in keyElements)
                {
                    MetKeys key = MetKeys.None;
                    if (keyNameMap.ContainsKey(keyElement))
                    {
                        key = keyNameMap[keyElement];
                    }
                    else
                    {
                        key = (MetKeys)Enum.Parse(typeof(MetKeys), keyElement);
                    }

                    // 既にキーコードが設定されているにも関わらず、更にキーコードがきた場合はエラー
                    if ((key & MetKeys.KeyCode) != MetKeys.None)
                    {
                        hasKeyCode = !hasKeyCode ? true : throw new FormatException(ExceptionResources.GetString("KeysConverterInvalidKeyCombination"));
                    }
                    resultKey |= key;
                }

                return resultKey;
            }

            long num = 0;
            foreach (Enum enumValue in (Enum[])value)
            {
                num |= Convert.ToInt64(enumValue, CultureInfo.InvariantCulture);
            }
            return Enum.ToObject(typeof(MetKeys), num);
        }

        /// <summary>
        /// キーコードから、デザインに表示する文字列を求める。
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == null)
            {
                throw new ArgumentNullException(nameof(destinationType));
            }

            if (!(value is MetKeys || value is int))
            {
                return base.ConvertTo(context, culture, value, destinationType);
            }

            bool isStringDestinate = destinationType == typeof(string);
            bool isEnumDestinate = destinationType == typeof(Enum[]);
            if (!(isStringDestinate | isEnumDestinate))
            {
                return base.ConvertTo(context, culture, value, destinationType);
            }

            var keys = (MetKeys)value;

            var textResult = new StringBuilder();
            var enumResult = new List<Enum>();

            // 修飾子を表示順を考慮して書き出す
            var modifiers = keys & MetKeys.Modifiers;
            foreach (var modifiersKeyName in OrderModifierNames)
            {
                if ((modifiers & keyNameMap[modifiersKeyName]) > 0)
                {
                    textResult.Append(modifiersKeyName).Append("+");
                    enumResult.Add(keyNameMap[modifiersKeyName]);
                }
            }

            // キーコードを書き出す
            var keyCode = keys & MetKeys.KeyCode;
            var keyName = keyNameMap.Where(x => (MetKeys)x.Value == keyCode).Select(x => x.Key).FirstOrDefault();
            if (keyName == null)
            {
                textResult.Append(keyCode.ToString());
                enumResult.Add(keyCode);
            }
            else
            {
                textResult.Append(keyName);
                enumResult.Add((MetKeys)Enum.Parse(typeof(MetKeys), keyName.ToString()));
            }

            // Designer コード反映のために Enum[] が求められたら Enum[] を返却する
            if (isEnumDestinate)
            {
                return enumResult.ToArray();
            }

            return textResult.ToString();
        }

        /// <summary>
        /// フォーマット コンテキストが提供されている場合、この型コンバーターが対象とするデータ型の標準値のコレクションを返します。
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            if (standardValues == null)
            {
                ArrayList arrayList = new ArrayList();
                foreach (object obj in (IEnumerable)keyNameMap.Values)
                {
                    arrayList.Add(obj);
                }
                arrayList.Sort(this);
                standardValues = new StandardValuesCollection((ICollection)arrayList.ToArray());
            }
            return standardValues;
        }

        /// <summary>
        /// 指定したコンテキストを使用して、GetStandardValues() から返された標準値のコレクションが有効値の排他的なリストかどうかを示す値を返します。
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }

        /// <summary>
        /// 指定したコンテキストを使用して、リストから選択できる標準値セットをオブジェクトがサポートするかどうかを示す値を返します。
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
    }
}
