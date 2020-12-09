using System;
using System.Collections.Generic;
using System.Text;

namespace Metroit.Api.WindowsControls.CommonControls
{
    /// <summary>
    /// MonthCalendarのビュー種類を提供します。
    /// </summary>
    public static class MonthCalendarView
    {
        /// <summary>
        /// 月単位のビュー。
        /// </summary>
        public const int MCMV_MONTH = 0;

        /// <summary>
        /// 年間ビュー。
        /// </summary>
        public const int MCMV_YEAR = 1;

        /// <summary>
        /// 10年表示ビュー。
        /// </summary>
        public const int MCMV_DECADE = 2;

        /// <summary>
        /// 世紀表示ビュー。
        /// </summary>
        public const int MCMV_CENTURY = 3;
    }
}
