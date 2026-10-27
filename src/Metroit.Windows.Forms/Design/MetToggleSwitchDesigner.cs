#if NETFRAMEWORK
using System.Collections;
using System.Windows.Forms.Design;
using System.Windows.Forms.Design.Behavior;

namespace Metroit.Windows.Forms.Design
{
    /// <summary>
    /// MetToggleSwitch で SnapToLine を機能させる機能を提供します。
    /// </summary>
    internal class MetToggleSwitchDesigner : ControlDesigner
    {
        public override IList SnapLines
        {
            get
            {
                var snapLines = base.SnapLines;
                var toggle = (MetToggleSwitch)Control;

                // テキストを表示しているときだけ、ベースラインを追加する
                if (toggle.ShowState)
                {
                    snapLines.Add(new SnapLine(
                        SnapLineType.Baseline,
                        toggle.GetTextBaseline(),
                        SnapLinePriority.Medium));
                }

                return snapLines;
            }
        }
    }
}
#elif NET6_0_OR_GREATER
using Microsoft.DotNet.DesignTools.Designers;
using Microsoft.DotNet.DesignTools.Designers.Behaviors;
using System.Collections.Generic;

namespace Metroit.Windows.Forms.Design
{
    /// <summary>
    /// MetToggleSwitch で SnapToLine を機能させる機能を提供します。
    /// </summary>
    internal class MetToggleSwitchDesigner : ControlDesigner
    {
        public override IList<SnapLine> SnapLines
        {
            get
            {
                var snapLines = base.SnapLines;
                var toggle = (MetToggleSwitch)Control;

                if (toggle.ShowState)
                {
                    snapLines.Add(new SnapLine(
                        SnapLineType.Baseline,
                        toggle.GetTextBaseline(),
                        SnapLinePriority.Medium));
                }

                return snapLines;
            }
        }
    }
}
#endif