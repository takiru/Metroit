using System;

namespace Metroit
{
    /// <summary>
    /// 数値の算出を提供します。
    /// </summary>
    public static class MetMath
    {
        /// <summary>
        /// 10進値を、指定した小数部の桁数に丸めます。パラメーターは、値が2つの数値の中間にある場合にその値を丸める方法を指定します。
        /// 数値を指定した小数位まで丸めます。
        /// </summary>
        /// <param name="d">丸め対象数値。</param>
        /// <param name="dec">有効小数桁数。</param>
        /// <param name="roundingNum">丸め値。四捨五入であれば5。</param>
        /// <param name="mode">dが2つの数値の中間にある場合にその値を丸める方法を指定します。</param>
        /// <returns>指定した小数部まで丸めた数値。</returns>
        public static decimal Round(decimal d, int dec,
                int roundingNum = 5, MidpointRounding mode = MidpointRounding.ToEven)
        {
            // 有効桁数まで整数化
            var workValue = d;
            workValue *= (decimal)Math.Pow(10, dec);

            // n捨m入の値を決定する
            var direction = 1;
            if (workValue < 0)
            {
                direction = -1;
            }
            var remainValue = Math.Abs(workValue - Math.Truncate(workValue));
            if (remainValue * 10 >= roundingNum)
            {
                workValue += (decimal)(direction * 0.5);
            }

            workValue = Math.Round(workValue, 0, mode);

            // 有効桁数を小数化
            workValue /= (decimal)Math.Pow(10, dec);

            return workValue;
        }

        /// <summary>
        /// 倍精度浮動小数点を、指定した小数部の桁数に丸めます。パラメーターは、値が2つの数値の中間にある場合にその値を丸める方法を指定します。
        /// 数値を指定した小数位まで丸めます。
        /// </summary>
        /// <param name="value">丸め対象数値。</param>
        /// <param name="dec">有効小数桁数。</param>
        /// <param name="roundingNum">丸め値。四捨五入であれば5。</param>
        /// <param name="mode">dが2つの数値の中間にある場合にその値を丸める方法を指定します。</param>
        /// <returns>指定した小数部まで丸めた数値。</returns>
        public static double Round(double value, int dec,
                int roundingNum = 5, MidpointRounding mode = MidpointRounding.ToEven)
        {
            // 有効桁数まで整数化
            var workValue = value;
            workValue *= Math.Pow(10, dec);

            // n捨m入の値を決定する
            var direction = 1;
            if (workValue < 0)
            {
                direction = -1;
            }
            var remainValue = Math.Abs(workValue - Math.Truncate(workValue));
            if (remainValue * 10 >= roundingNum)
            {
                workValue += (direction * 0.5);
            }

            workValue = Math.Round(workValue, 0, mode);

            // 有効桁数を小数化
            workValue /= Math.Pow(10, dec);

            return workValue;
        }

        /// <summary>
        /// 指定した10進数以上の数のうち、最小の整数値を返します。
        /// </summary>
        /// <param name="d">丸め対象数値。</param>
        /// <param name="dec">有効小数桁数。</param>
        /// <returns>指定した小数部まで切り上げた数値。</returns>
        public static decimal Ceiling(decimal d, int dec = 0)
        {
            // 有効桁数まで整数化
            var workValue = d;
            workValue *= (decimal)Math.Pow(10, dec);

            workValue = Math.Ceiling(workValue);

            // 有効桁数を小数化
            workValue /= (decimal)Math.Pow(10, dec);

            return workValue;
        }

        /// <summary>
        /// 指定した倍精度浮動小数点以上の数のうち、最小の整数値を返します。
        /// </summary>
        /// <param name="a">丸め対象数値。</param>
        /// <param name="dec">有効小数桁数。</param>
        /// <returns>指定した小数部まで切り上げた数値。</returns>
        public static double Ceiling(double a, int dec = 0)
        {
            // 有効桁数まで整数化
            var workValue = a;
            workValue *= Math.Pow(10, dec);

            workValue = Math.Ceiling(workValue);

            // 有効桁数を小数化
            workValue /= Math.Pow(10, dec);

            return workValue;
        }

        /// <summary>
        /// 指定した10進数以下の数のうち、最大の整数値を返します。
        /// </summary>
        /// <param name="d">丸め対象数値。</param>
        /// <param name="dec">有効小数桁数。</param>
        /// <returns>指定した小数部まで切り捨てた数値。</returns>
        public static decimal Floor(decimal d, int dec = 0)
        {
            // 有効桁数まで整数化
            var workValue = d;
            workValue *= (decimal)Math.Pow(10, dec);

            workValue = Math.Floor(workValue);

            // 有効桁数を小数化
            workValue /= (decimal)Math.Pow(10, dec);

            return workValue;
        }

        /// <summary>
        /// 指定した倍精度浮動小数点以下の数のうち、最大の整数値を返します。
        /// </summary>
        /// <param name="d">丸め対象数値。</param>
        /// <param name="dec">有効小数桁数。</param>
        /// <returns>指定した小数部まで切り捨てた数値。</returns>
        public static double Floor(double d, int dec = 0)
        {
            // 有効桁数まで整数化
            var workValue = d;
            workValue *= Math.Pow(10, dec);

            workValue = Math.Floor(workValue);

            // 有効桁数を小数化
            workValue /= Math.Pow(10, dec);

            return workValue;
        }

        /// <summary>
        /// 指定した10進数の整数部を計算します。
        /// </summary>
        /// <param name="d">丸め対象数値。</param>
        /// <param name="dec">有効小数桁数。</param>
        /// <returns>指定した小数位まで丸めた数値。</returns>
        public static decimal Truncate(decimal d, int dec = 0)
        {
            // 有効桁数まで整数化
            var workValue = d;
            workValue *= (decimal)Math.Pow(10, dec);

            workValue = Math.Truncate(workValue);

            // 有効桁数を小数化
            workValue /= (decimal)Math.Pow(10, dec);

            return workValue;
        }

        /// <summary>
        /// 指定した倍精度浮動小数の整数部を計算します。
        /// </summary>
        /// <param name="d">丸め対象数値。</param>
        /// <param name="dec">有効小数桁数。</param>
        /// <returns>指定した小数位まで丸めた数値。</returns>
        public static double Truncate(double d, int dec = 0)
        {
            // 有効桁数まで整数化
            var workValue = d;
            workValue *= Math.Pow(10, dec);

            workValue = Math.Truncate(workValue);

            // 有効桁数を小数化
            workValue /= Math.Pow(10, dec);

            return workValue;
        }
    }
}
