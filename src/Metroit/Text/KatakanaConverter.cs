using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Metroit.Text
{
    /// <summary>
    /// カタカナ文字のコンバーターを提供します。
    /// </summary>
    public class KatakanaConverter
    {
        /// <summary>
        /// 通常カナマップ情報。
        /// </summary>
        private static readonly Dictionary<char, char> NormalMap = new Dictionary<char, char>();

        /// <summary>
        /// 濁点カナマップ情報。
        /// </summary>
        private static readonly Dictionary<char, char> VoicedMarkMap = new Dictionary<char, char>();

        /// <summary>
        /// 半濁点カナマップ情報。
        /// </summary>
        private static readonly Dictionary<char, char> SemiVoicedMarkMap = new Dictionary<char, char>();

        /// <summary>
        /// 静的コンストラクタによってマップ情報を構成する。
        /// </summary>
        static KatakanaConverter()
        {
            // 通常カナ
            var normalChars = new Dictionary<char, char>
        {
            {'｡', '。'}, {'｢', '「'}, {'｣', '」'}, {'､', '、'}, {'･', '・'},{'ﾟ', '゜'},{'ﾞ', '゛'},
            {'ｦ', 'ヲ'}, {'ｧ', 'ァ'}, {'ｨ', 'ィ'}, {'ｩ', 'ゥ'}, {'ｪ', 'ェ'}, {'ｫ', 'ォ'},
            {'ｬ', 'ャ'}, {'ｭ', 'ュ'}, {'ｮ', 'ョ'}, {'ｯ', 'ッ'}, {'ｰ', 'ー'},
            {'ｱ', 'ア'}, {'ｲ', 'イ'}, {'ｳ', 'ウ'}, {'ｴ', 'エ'}, {'ｵ', 'オ'},
            {'ｶ', 'カ'}, {'ｷ', 'キ'}, {'ｸ', 'ク'}, {'ｹ', 'ケ'}, {'ｺ', 'コ'},
            {'ｻ', 'サ'}, {'ｼ', 'シ'}, {'ｽ', 'ス'}, {'ｾ', 'セ'}, {'ｿ', 'ソ'},
            {'ﾀ', 'タ'}, {'ﾁ', 'チ'}, {'ﾂ', 'ツ'}, {'ﾃ', 'テ'}, {'ﾄ', 'ト'},
            {'ﾅ', 'ナ'}, {'ﾆ', 'ニ'}, {'ﾇ', 'ヌ'}, {'ﾈ', 'ネ'}, {'ﾉ', 'ノ'},
            {'ﾊ', 'ハ'}, {'ﾋ', 'ヒ'}, {'ﾌ', 'フ'}, {'ﾍ', 'ヘ'}, {'ﾎ', 'ホ'},
            {'ﾏ', 'マ'}, {'ﾐ', 'ミ'}, {'ﾑ', 'ム'}, {'ﾒ', 'メ'}, {'ﾓ', 'モ'},
            {'ﾔ', 'ヤ'}, {'ﾕ', 'ユ'}, {'ﾖ', 'ヨ'},
            {'ﾗ', 'ラ'}, {'ﾘ', 'リ'}, {'ﾙ', 'ル'}, {'ﾚ', 'レ'}, {'ﾛ', 'ロ'},
            {'ﾜ', 'ワ'}, {'ﾝ', 'ン'}
        };

            foreach (var kvp in normalChars)
            {
                NormalMap[kvp.Key] = kvp.Value;
            }

            // 濁点カナ
            var voicedMarks = new Dictionary<char, char>
        {
            {'ｶ', 'ガ'}, {'ｷ', 'ギ'}, {'ｸ', 'グ'}, {'ｹ', 'ゲ'}, {'ｺ', 'ゴ'},
            {'ｻ', 'ザ'}, {'ｼ', 'ジ'}, {'ｽ', 'ズ'}, {'ｾ', 'ゼ'}, {'ｿ', 'ゾ'},
            {'ﾀ', 'ダ'}, {'ﾁ', 'ヂ'}, {'ﾂ', 'ヅ'}, {'ﾃ', 'デ'}, {'ﾄ', 'ド'},
            {'ﾊ', 'バ'}, {'ﾋ', 'ビ'}, {'ﾌ', 'ブ'}, {'ﾍ', 'ベ'}, {'ﾎ', 'ボ'}
        };

            foreach (var kvp in voicedMarks)
            {
                VoicedMarkMap[kvp.Key] = kvp.Value;
            }

            // 半濁点カナ
            var semiVoicedMarks = new Dictionary<char, char>
        {
            {'ﾊ', 'パ'}, {'ﾋ', 'ピ'}, {'ﾌ', 'プ'}, {'ﾍ', 'ペ'}, {'ﾎ', 'ポ'}
        };

            foreach (var kvp in semiVoicedMarks)
            {
                SemiVoicedMarkMap[kvp.Key] = kvp.Value;
            }
        }

        /// <summary>
        /// <paramref name="text"/> に含まれる半角カタカナを全角カタカナに変換します。
        /// </summary>
        /// <param name="text">変換する文字列。</param>
        /// <returns>半角カタカナを全角カタカナに変換した文字列。</returns>
        public static string ToFullWidth(string text)
        {
            var result = new StringBuilder();

            for (int i = 0; i < text.Length; i++)
            {
                char currentChar = text[i];
                char nextChar = i + 1 < text.Length ? text[i + 1] : '\0';

                // 濁点チェック
                if (nextChar == 'ﾞ' && VoicedMarkMap.TryGetValue(currentChar, out char voicedMarkChar))
                {
                    result.Append(voicedMarkChar);
                    i++;
                    continue;
                }

                // 半濁点チェック
                if (nextChar == 'ﾟ' && SemiVoicedMarkMap.TryGetValue(currentChar, out char semiVoicedMarkChar))
                {
                    result.Append(semiVoicedMarkChar);
                    i++;
                    continue;
                }

                // 通常の変換
                if (NormalMap.TryGetValue(currentChar, out char normalChar))
                {
                    result.Append(normalChar);
                    continue;
                }

                result.Append(currentChar);
            }

            return result.ToString();
        }

        /// <summary>
        /// <paramref name="text"/> に含まれる全角カタカナを半角カタカナに変換します。
        /// </summary>
        /// <param name="text">変換する文字列。</param>
        /// <returns>全角カタカナを半角カタカナに変換した文字列。</returns>
        public static string ToHalfWidth(string text)
        {
            var result = new StringBuilder();

            for (var i = 0; i < text.Length; i++)
            {
                if (NormalMap.ContainsValue(text[i]))
                {
                    result.Append(NormalMap
                        .Where(x => x.Value == text[i])
                        .Select(x => x.Key)
                        .Single());
                    continue;
                }

                if (VoicedMarkMap.ContainsValue(text[i]))
                {
                    result.Append(VoicedMarkMap
                        .Where(x => x.Value == text[i])
                        .Select(x => x.Key)
                        .Single());
                    result.Append("ﾞ");
                    continue;
                }

                if (SemiVoicedMarkMap.ContainsValue(text[i]))
                {
                    result.Append(SemiVoicedMarkMap
                        .Where(x => x.Value == text[i])
                        .Select(x => x.Key)
                        .Single());
                    result.Append("ﾟ");
                    continue;
                }

                result.Append(text[i]);
            }

            return result.ToString();
        }
    }
}