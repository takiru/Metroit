using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metroit.IO
{
    /// <summary>
    /// 反応変換を行うターゲットとなるファイルを定義します。
    /// </summary>
    public enum ReactiveFileTarget
    {
        /// <summary>
        /// 変換実行を行ったオリジナルのファイルを定義します。
        /// 一時ディレクトリを利用している場合、一時ディレクトリにコピーしたオリジナルのファイルが対象となります。
        /// </summary>
        Original,

        /// <summary>
        /// 直近の変換実行で利用された変換元ファイルを定義します。
        /// 多階層による変換を実行している場合、直近の階層で利用した変換元ファイルが対象となります。
        /// 一時ディレクトリを利用している場合、一時ディレクトリにコピーした変換元ファイルが対象となります。
        /// </summary>
        RecentOriginal,

        /// <summary>
        /// 直近の変換実行で変換された変換先ファイルを定義します。
        /// 多階層による変換を実行している場合、直近の階層で変換された変換先ファイルが対象となります。
        /// 一時ディレクトリを利用している場合、一時ディレクトリに変換された変換先ファイルが対象となります。
        /// </summary>
        RecentConvert
    }
}
