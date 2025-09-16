using Metroit.Windows.Forms.ComponentModel;
using System;
using System.ComponentModel;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// 角丸の半径を提供します。
    /// </summary>
    [TypeConverter(typeof(RoundedCornerRadiusConverter))]
    public class RoundedCornerRadius : IEquatable<RoundedCornerRadius>
    {
        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        /// <param name="all">すべての半径の値。</param>
        public RoundedCornerRadius(int all)
        {
            _all = true;
            _topLeft = _topRight = _bottomRight = _bottomLeft = all < 0 ? 0 : all;
        }

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        /// <param name="topLeft">左上の角丸半径。</param>
        /// <param name="topRight">右上の角丸半径。</param>
        /// <param name="bottomRight">右下の角丸半径。</param>
        /// <param name="bottomLeft">左下の角丸半径。</param>
        public RoundedCornerRadius(int topLeft, int topRight, int bottomRight, int bottomLeft)
        {
            _topLeft = topLeft < 0 ? 0 : topLeft;
            _topRight = topRight < 0 ? 0 : topRight;
            _bottomRight = bottomRight < 0 ? 0 : bottomRight;
            _bottomLeft = bottomLeft < 0 ? 0 : bottomLeft;
            _all = _topLeft == _topRight && _topLeft == _bottomRight && _topLeft == _bottomLeft;
        }

        private bool _all;

        /// <summary>
        /// すべての半径を同じ値にする場合、その値を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [MetDescription("RoundedCornerRadiusAll")]
        public int All
        {
            get => _all ? _topLeft : -1;
            set
            {
                if (!_all || _topLeft != value)
                {
                    _all = true;
                    _topLeft = _topRight = _bottomRight = _bottomLeft = value;
                }
            }
        }

        /// <summary>
        /// All プロパティが変更されたかどうかを取得します。
        /// </summary>
        /// <returns></returns>
        internal bool ShouldSerializeAll() => _all;

        private int _topLeft;

        /// <summary>
        /// 左上の半径を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [MetDescription("RoundedCornerRadiusTopLeft")]
        public int TopLeft
        {
            get => _topLeft;
            set
            {
                if (_all || _topLeft != value)
                {
                    _all = false;
                    _topLeft = value < 0 ? 0 : value;
                }
            }
        }

        private int _topRight;

        /// <summary>
        /// 右上の半径を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [MetDescription("RoundedCornerRadiusTopRight")]
        public int TopRight
        {
            get => _topRight;
            set
            {
                if (_all || _topRight != value)
                {
                    _all = false;
                    _topRight = value < 0 ? 0 : value;
                }
            }
        }

        private int _bottomRight;

        /// <summary>
        /// 右下の半径を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [MetDescription("RoundedCornerRadiusBottomRight")]
        public int BottomRight
        {
            get => _bottomRight;
            set
            {
                if (_all || _bottomRight != value)
                {
                    _all = false;
                    _bottomRight = value < 0 ? 0 : value;
                }
            }
        }

        private int _bottomLeft;

        /// <summary>
        /// 左下の半径を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [MetDescription("RoundedCornerRadiusBottomLeft")]
        public int BottomLeft
        {
            get => _bottomLeft;
            set
            {
                if (_all || _bottomLeft != value)
                {
                    _all = false;
                    _bottomLeft = value < 0 ? 0 : value;
                }
            }
        }

        /// <summary>
        /// 角丸の半径を表す文字列を取得します。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{TopLeft}, {TopRight}, {BottomRight}, {BottomLeft}";
        }

        /// <summary>
        /// オブジェクトが等しいかどうかを判定します。
        /// </summary>
        /// <param name="obj">オブジェクト。</param>
        /// <returns>すべて同一の角丸の半径であれば <see langword="true"/>, それ以外は <see langword="false"/> を返却します。</returns>
        public bool Equals(RoundedCornerRadius obj)
        {
            return TopLeft == obj.TopLeft &&
                TopRight == obj.TopRight &&
                BottomRight == obj.BottomRight &&
                BottomLeft == obj.BottomLeft;
        }

        /// <summary>
        /// オブジェクトが等しいかどうかを判定します。
        /// </summary>
        /// <param name="obj">オブジェクト。</param>
        /// <returns>すべて同一の角丸の半径であれば <see langword="true"/>, それ以外は <see langword="false"/> を返却します。</returns>
        public override bool Equals(object obj)
        {
            return obj is RoundedCornerRadius otherRadius && Equals(otherRadius);
        }

        /// <summary>
        /// <see cref="RoundedCornerRadius"/> が等しいかどうかを判定します。
        /// </summary>
        /// <param name="p1">左辺。</param>
        /// <param name="p2">右辺。</param>
        /// <returns>等しい場合は true, それ以外は false を返却します。</returns>
        public static bool operator ==(RoundedCornerRadius p1, RoundedCornerRadius p2)
        {
            if (object.ReferenceEquals(p1, null) && object.ReferenceEquals(p2, null))
            {
                return true;
            }
            if (object.ReferenceEquals(p1, null))
            {
                return false;
            }
            if (object.ReferenceEquals(p2, null))
            {
                return false;
            }

            return p1.TopLeft == p2.TopLeft && p1.TopRight == p2.TopRight && p1.BottomRight == p2.BottomRight && p1.BottomLeft == p2.BottomLeft;
        }

        /// <summary>
        /// <see cref="RoundedCornerRadius"/> が等しくないかどうかを判定します。
        /// </summary>
        /// <returns>等しくない場合は true, それ以外は false を返却します。</returns>
        public static bool operator !=(RoundedCornerRadius p1, RoundedCornerRadius p2)
        {
            return !(p1 == p2);
        }

        /// <summary>
        /// ハッシュコードを取得します。
        /// </summary>
        /// <returns>ハッシュコード。</returns>
        public override int GetHashCode()
        {
            return TopLeft.GetHashCode() ^ TopRight.GetHashCode()
                 ^ BottomRight.GetHashCode() ^ BottomLeft.GetHashCode();
        }
    }
}