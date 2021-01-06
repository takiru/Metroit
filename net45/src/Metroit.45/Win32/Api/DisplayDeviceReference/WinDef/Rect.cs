using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Metroit.Win32.Api.DisplayDeviceReference.WinDef
{
    /// <summary>
    /// RECT構造は、左上隅と右下隅の座標によって長方形を定義します。
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Rect
    {
        /// <summary>
        /// 
        /// </summary>
        public int Left, Top, Right, Bottom;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="right"></param>
        /// <param name="bottom"></param>
        public Rect(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="r"></param>
        public Rect(System.Drawing.Rectangle r) : this(r.Left, r.Top, r.Right, r.Bottom) { }

        /// <summary>
        /// 
        /// </summary>
        public int X
        {
            get { return Left; }
            set { Right -= (Left - value); Left = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Y
        {
            get { return Top; }
            set { Bottom -= (Top - value); Top = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Height
        {
            get { return Bottom - Top; }
            set { Bottom = value + Top; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Width
        {
            get { return Right - Left; }
            set { Right = value + Left; }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.Drawing.Point Location
        {
            get { return new System.Drawing.Point(Left, Top); }
            set { X = value.X; Y = value.Y; }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.Drawing.Size Size
        {
            get { return new System.Drawing.Size(Width, Height); }
            set { Width = value.Width; Height = value.Height; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="r"></param>
        public static implicit operator System.Drawing.Rectangle(Rect r)
        {
            return new System.Drawing.Rectangle(r.Left, r.Top, r.Width, r.Height);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="r"></param>
        public static implicit operator Rect(System.Drawing.Rectangle r)
        {
            return new Rect(r);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="r1"></param>
        /// <param name="r2"></param>
        /// <returns></returns>
        public static bool operator ==(Rect r1, Rect r2)
        {
            return r1.Equals(r2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="r1"></param>
        /// <param name="r2"></param>
        /// <returns></returns>
        public static bool operator !=(Rect r1, Rect r2)
        {
            return !r1.Equals(r2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public bool Equals(Rect r)
        {
            return r.Left == Left && r.Top == Top && r.Right == Right && r.Bottom == Bottom;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is Rect)
                return Equals((Rect)obj);
            else if (obj is System.Drawing.Rectangle)
                return Equals(new Rect((System.Drawing.Rectangle)obj));
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return ((System.Drawing.Rectangle)this).GetHashCode();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format(System.Globalization.CultureInfo.CurrentCulture, "{{Left={0},Top={1},Right={2},Bottom={3}}}", Left, Top, Right, Bottom);
        }
    }
}
