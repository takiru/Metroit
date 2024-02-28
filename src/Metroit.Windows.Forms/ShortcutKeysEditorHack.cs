using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// Keys プロパティをデザイナ上で編集するエディタをハックします。
    /// </summary>
    internal class ShortcutKeysEditorHack
    {
        /// <summary>
        /// Keys 内に、指定されたキー情報を追加します。
        /// </summary>
        /// <param name="keys">追加キー情報。</param>
        /// <param name="sort">ソートするかどうか。</param>
        public static void AddKeys(Keys[] keys, bool sort = false)
        {
            var validKeysFi = GetValidKeysField();

            // Keys[]型フィールドが見つからなかった場合は何もしない
            if (validKeysFi == null)
            {
                return;
            }

            var normalKeys = (Keys[])validKeysFi.GetValue(null);

            // 追加対象のKeysを追加
            var keysList = normalKeys.ToList();
            foreach (var key in keys)
            {
                if (!keysList.Contains(key))
                {
                    keysList.Add(key);
                }
            }

            // ソート指定がある時は文字列によるソートを実施
            if (sort)
            {
                keysList.Sort((s, d) => { return SortKeys(s, d); });
            }

            validKeysFi.SetValue(null, keysList.ToArray());
        }

        /// <summary>
        /// ShortcutKeysEditorクラス内のshortcutKeysUIオブジェクト内にあるvalidKeysフィールドを取得する。
        /// </summary>
        /// <returns>validKeysのFieldInfoオブジェクト。見つからない場合はnull。</returns>
        private static FieldInfo GetValidKeysField()
        {
            // VS2017 変数名決め打ちによるフィールド取得
            var shortcutKeysUIFi = typeof(ShortcutKeysEditor).GetField("shortcutKeysUI", BindingFlags.NonPublic | BindingFlags.Instance);
            if (shortcutKeysUIFi != null)
            {

                var validKeysFi = shortcutKeysUIFi.FieldType.GetField("validKeys", BindingFlags.NonPublic | BindingFlags.Static);
                if (validKeysFi != null)
                {
                    return validKeysFi;
                }
            }

            // 見つからない場合、Keys[]型フィールドを取得
            foreach (var fi in typeof(ShortcutKeysEditor).GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
            {
                foreach (var fi2 in fi.FieldType.GetFields(BindingFlags.NonPublic | BindingFlags.Static))
                {
                    if (fi2.FieldType == typeof(Keys[]))
                    {
                        return fi2;
                    }
                }
            }

            // 見つからない場合
            return null;
        }

        /// <summary>
        /// キー情報を文字列で昇順にソートする。
        /// </summary>
        /// <param name="source">ソート対象1。</param>
        /// <param name="dest">ソート対象2。</param>
        /// <returns>ソート結果。</returns>
        private static int SortKeys(Keys source, Keys dest)
        {
            string sValue = source.ToString() == "Return" ? "Enter" : source.ToString();
            string dValue = dest.ToString() == "Return" ? "Enter" : dest.ToString();

            return sValue.CompareTo(dValue);
        }
    }
}
