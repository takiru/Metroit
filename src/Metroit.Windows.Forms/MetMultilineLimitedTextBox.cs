using System;
using System.ComponentModel;
using System.Linq;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// 複数行の入力を許可するテキストエリアを提供します。
    /// </summary>
    /// <remarks>
    /// [拡張機能]<br />
    /// 　・1行単位の文字数/バイト数の制限。<br />
    /// 　・最大行数の制限。
    /// </remarks>
    [ToolboxItem(true)]
    public class MetMultilineLimitedTextBox : MetLimitedTextBox
    {
        /// <summary>
        /// MetLimitedTextBox の新しいインスタンスを初期化します。
        /// </summary>
        public MetMultilineLimitedTextBox() : base()
        {
            base.Multiline = true;
            base.Height = 50;
            base.MaxLength = 0;
            base.MaxByteLength = 0;
        }

        /// <summary>
        /// テキストボックスコントールは複数行の入力が可能です。
        /// </summary>
        [Browsable(false)]
        [ReadOnly(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new bool Multiline
        {
            get => base.Multiline;
        }

        /// <summary>
        /// テキストボックスコントロールの文字数制限は無制限です。
        /// </summary>
        [Browsable(false)]
        [ReadOnly(true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new int MaxLength
        {
            get => base.MaxLength;
        }

        /// <summary>
        /// テキストボックスコントロールのバイト数制限は無制限です。
        /// </summary>
        [Browsable(false)]
        [ReadOnly(true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new int MaxByteLength
        {
            get => base.MaxByteLength;
        }

        private int maxLineLength = 0;

        /// <summary>
        /// テキストボックスコントロールに入力できる1行あたりの最大文字数を指定します。0を指定した場合、無制限となります。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(0)]
        [MetCategory("MetBehavior")]
        [MetDescription("ControlMaxLineLength")]
        public int MaxLineLength
        {
            get => maxLineLength;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(MaxLineLength));
                }
                maxLineLength = value;
            }
        }

        private int maxLineByteLength = 0;

        /// <summary>
        /// テキストボックスコントロールに入力できる1行あたりの最大バイト数を指定します。0を指定した場合、無制限となります。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(0)]
        [MetCategory("MetBehavior")]
        [MetDescription("ControlMaxLineByteLength")]
        public int MaxLineByteLength
        {
            get => maxLineByteLength;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(MaxLineByteLength));
                }
                maxLineByteLength = value;
            }
        }

        private int maxLineCount = 0;

        /// <summary>
        /// テキストボックスコントロールに入力できる行数を指定します。0を指定した場合、無制限となります。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(0)]
        [MetCategory("MetBehavior")]
        [MetDescription("ControlMaxLineCount")]
        public int MaxLineCount
        {
            get => maxLineCount;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(MaxLineCount));
                }
                maxLineCount = value;
            }
        }

        /// <summary>
        /// 入力後の文字列の長さが有効かチェックします。
        /// </summary>
        /// <param name="value">文字列。</param>
        /// <returns>true:有効, false:無効。</returns>
        protected override bool IsValidTextLength(string value)
        {
            // 改行の制限
            if (MaxLineCount > 0 && value.ToCharArray().Where(x => x == '\r').Count() + 1 > MaxLineCount)
            {
                return false;
            }

            // 1行単位の文字数の制限
            foreach (var lineText in value.Split(new string[] { Environment.NewLine, "\r", "\n" }, StringSplitOptions.None))
            {
                int byteCount = ByteEncoding.GetByteCount(lineText);
                if ((MaxLineLength > 0 && lineText.Length > MaxLineLength) ||
                    (MaxLineByteLength > 0 && byteCount > MaxLineByteLength))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// バイト数を考慮して、オートフォーカスを行うか確認します。
        /// </summary>
        /// <returns>true:オートフォーカス可, false:オートフォーカス不可</returns>
        protected override bool CanAutoFocus()
        {
            // ロールバック中は行わない
            if (Rollbacking)
            {
                return false;
            }

            // 改行制限でない時は行わない
            if (MaxLineCount == 0 || Text.ToCharArray().Where(x => x == '\r').Count() + 1 < MaxLineCount)
            {
                return false;
            }

            // 最終行の文字数制限でない時は行わない
            if (SelectionStart != Text.Length)
            {
                return false;
            }
            var lastLineText = Text.Split(new string[] { Environment.NewLine, "\r", "\n" }, StringSplitOptions.None).Last();
            int byteCount = ByteEncoding.GetByteCount(lastLineText);
            if (!(MaxLineLength > 0 && lastLineText.Length == MaxLineLength) &&
                !(MaxLineByteLength > 0 && byteCount == MaxLineByteLength))
            {
                return false;
            }

            return true;
        }
    }
}
