using System;

namespace Metroit.Annotations
{
    /// <summary>
    /// 変更追跡を行わないプロパティまたはフィールドに設定する属性です。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    public class NoTrackingAttribute : Attribute
    {
        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        public NoTrackingAttribute()
        {

        }
    }
}
