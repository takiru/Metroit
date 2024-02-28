using System.Data.Common;

namespace Metroit.Data.Extensions
{
    /// <summary>
    /// DbException クラスの拡張メソッドを提供します。
    /// </summary>
    public static class DbExceptionExtensions
    {
        /// <summary>
        /// <para>データベースが検出した Number を取得します。</para>
        /// <para>Number が存在しない場合、int.MinValue を返却します。</para>
        /// </summary>
        /// <param name="exception">DbException オブジェクト。</param>
        /// <returns>Number</returns>
        public static int GetNumber(this DbException exception)
        {
            var prop = exception.GetType().GetProperty("Number");
            if (prop == null)
            {
                return int.MinValue;
            }
            return (int)prop.GetValue(exception);
        }

    }
}
