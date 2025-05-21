using System.Drawing;
using System.Windows.Forms;

namespace Metroit.Windows.FormsTestNet462
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Forms.NumericNegativePattern numericNegativePattern4 = new Forms.NumericNegativePattern();
            Forms.NumericPositivePattern numericPositivePattern4 = new Forms.NumericPositivePattern();
            Forms.NumericNegativePattern numericNegativePattern1 = new Forms.NumericNegativePattern();
            Forms.NumericPositivePattern numericPositivePattern1 = new Forms.NumericPositivePattern();
            Forms.NumericNegativePattern numericNegativePattern2 = new Forms.NumericNegativePattern();
            Forms.NumericPositivePattern numericPositivePattern2 = new Forms.NumericPositivePattern();
            metLimitedTextBox1 = new Metroit.Windows.Forms.MetLimitedTextBox();
            metMultilineLimitedTextBox1 = new Metroit.Windows.Forms.MetMultilineLimitedTextBox();
            groupBox1 = new GroupBox();
            label3 = new Label();
            metNumericTextBox1 = new Metroit.Windows.Forms.MetNumericTextBox();
            checkBox2 = new CheckBox();
            checkBox1 = new CheckBox();
            groupBox2 = new GroupBox();
            label2 = new Label();
            metNumericTextBox3 = new Metroit.Windows.Forms.MetNumericTextBox();
            label1 = new Label();
            metNumericTextBox2 = new Metroit.Windows.Forms.MetNumericTextBox();
            checkBox3 = new CheckBox();
            checkBox4 = new CheckBox();
            OverlayShowButton = new Button();
            ((System.ComponentModel.ISupportInitialize)metLimitedTextBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)metMultilineLimitedTextBox1).BeginInit();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)metNumericTextBox1).BeginInit();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)metNumericTextBox3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)metNumericTextBox2).BeginInit();
            SuspendLayout();
            // 
            // metLimitedTextBox1
            // 
            metLimitedTextBox1.AutoFocus = true;
            // 
            // 
            // 
            metLimitedTextBox1.CustomAutoCompleteBox.TargetControl = metLimitedTextBox1;
            metLimitedTextBox1.Font = new Font("ＭＳ ゴシック", 9F);
            metLimitedTextBox1.FullWidthCharTwo = true;
            metLimitedTextBox1.Location = new Point(6, 47);
            metLimitedTextBox1.MaxLength = 6;
            metLimitedTextBox1.Name = "metLimitedTextBox1";
            metLimitedTextBox1.Size = new Size(100, 19);
            metLimitedTextBox1.TabIndex = 0;
            // 
            // metMultilineLimitedTextBox1
            // 
            metMultilineLimitedTextBox1.AutoFocus = true;
            // 
            // 
            // 
            metMultilineLimitedTextBox1.CustomAutoCompleteBox.TargetControl = metMultilineLimitedTextBox1;
            metMultilineLimitedTextBox1.Font = new Font("ＭＳ ゴシック", 9F, FontStyle.Regular, GraphicsUnit.Point, 128);
            metMultilineLimitedTextBox1.FullWidthCharTwo = true;
            metMultilineLimitedTextBox1.Location = new Point(6, 47);
            metMultilineLimitedTextBox1.MaxLineCount = 2;
            metMultilineLimitedTextBox1.MaxLineLength = 6;
            metMultilineLimitedTextBox1.Name = "metMultilineLimitedTextBox1";
            metMultilineLimitedTextBox1.ScrollBars = ScrollBars.Both;
            metMultilineLimitedTextBox1.Size = new Size(468, 64);
            metMultilineLimitedTextBox1.TabIndex = 1;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(metNumericTextBox1);
            groupBox1.Controls.Add(checkBox2);
            groupBox1.Controls.Add(checkBox1);
            groupBox1.Controls.Add(metLimitedTextBox1);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(345, 78);
            groupBox1.TabIndex = 3;
            groupBox1.TabStop = false;
            groupBox1.Text = "MetLimitedTextBox";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(222, 23);
            label3.Name = "label3";
            label3.Size = new Size(67, 15);
            label3.TabIndex = 3;
            label3.Text = "MaxLength";
            // 
            // metNumericTextBox1
            // 
            metNumericTextBox1.AcceptNegative = false;
            // 
            // 
            // 
            metNumericTextBox1.CustomAutoCompleteBox.TargetControl = metNumericTextBox1;
            metNumericTextBox1.GroupSizes = new int[]
    {
    3
    };
            metNumericTextBox1.Location = new Point(295, 20);
            metNumericTextBox1.MaxValue = new decimal(new int[] { 32767, 0, 0, 0 });
            metNumericTextBox1.MinValue = new decimal(new int[] { 0, 0, 0, 0 });
            metNumericTextBox1.Name = "metNumericTextBox1";
            metNumericTextBox1.NegativePattern = numericNegativePattern4;
            metNumericTextBox1.PositivePattern = numericPositivePattern4;
            metNumericTextBox1.Size = new Size(33, 23);
            metNumericTextBox1.TabIndex = 2;
            metNumericTextBox1.Value = new decimal(new int[] { 6, 0, 0, 0 });
            metNumericTextBox1.TextChanged += metNumericTextBox1_TextChanged;
            // 
            // checkBox2
            // 
            checkBox2.AutoSize = true;
            checkBox2.Checked = true;
            checkBox2.CheckState = CheckState.Checked;
            checkBox2.Location = new Point(95, 22);
            checkBox2.Name = "checkBox2";
            checkBox2.Size = new Size(121, 19);
            checkBox2.TabIndex = 1;
            checkBox2.Text = "FullWidthCharTwo";
            checkBox2.UseVisualStyleBackColor = true;
            checkBox2.CheckedChanged += checkBox2_CheckedChanged;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Checked = true;
            checkBox1.CheckState = CheckState.Checked;
            checkBox1.Location = new Point(6, 22);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(83, 19);
            checkBox1.TabIndex = 1;
            checkBox1.Text = "AutoFocus";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(label2);
            groupBox2.Controls.Add(metNumericTextBox3);
            groupBox2.Controls.Add(label1);
            groupBox2.Controls.Add(metNumericTextBox2);
            groupBox2.Controls.Add(checkBox3);
            groupBox2.Controls.Add(checkBox4);
            groupBox2.Controls.Add(metMultilineLimitedTextBox1);
            groupBox2.Location = new Point(12, 96);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(482, 120);
            groupBox2.TabIndex = 4;
            groupBox2.TabStop = false;
            groupBox2.Text = "MetMultilineLimitedTextBox";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(222, 23);
            label2.Name = "label2";
            label2.Size = new Size(84, 15);
            label2.TabIndex = 9;
            label2.Text = "MaxLineCount";
            // 
            // metNumericTextBox3
            // 
            metNumericTextBox3.AcceptNegative = false;
            // 
            // 
            // 
            metNumericTextBox3.CustomAutoCompleteBox.TargetControl = metNumericTextBox3;
            metNumericTextBox3.GroupSizes = new int[]
    {
    3
    };
            metNumericTextBox3.Location = new Point(312, 20);
            metNumericTextBox3.MaxValue = new decimal(new int[] { 32767, 0, 0, 0 });
            metNumericTextBox3.MinValue = new decimal(new int[] { 0, 0, 0, 0 });
            metNumericTextBox3.Name = "metNumericTextBox3";
            metNumericTextBox3.NegativePattern = numericNegativePattern1;
            metNumericTextBox3.PositivePattern = numericPositivePattern1;
            metNumericTextBox3.Size = new Size(33, 23);
            metNumericTextBox3.TabIndex = 8;
            metNumericTextBox3.Value = new decimal(new int[] { 2, 0, 0, 0 });
            metNumericTextBox3.TextChanged += metNumericTextBox3_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(351, 22);
            label1.Name = "label1";
            label1.Size = new Size(89, 15);
            label1.TabIndex = 7;
            label1.Text = "MaxLineLength";
            // 
            // metNumericTextBox2
            // 
            metNumericTextBox2.AcceptNegative = false;
            // 
            // 
            // 
            metNumericTextBox2.CustomAutoCompleteBox.TargetControl = metNumericTextBox2;
            metNumericTextBox2.GroupSizes = new int[]
    {
    3
    };
            metNumericTextBox2.Location = new Point(441, 19);
            metNumericTextBox2.MaxValue = new decimal(new int[] { 32767, 0, 0, 0 });
            metNumericTextBox2.MinValue = new decimal(new int[] { 0, 0, 0, 0 });
            metNumericTextBox2.Name = "metNumericTextBox2";
            metNumericTextBox2.NegativePattern = numericNegativePattern2;
            metNumericTextBox2.PositivePattern = numericPositivePattern2;
            metNumericTextBox2.Size = new Size(33, 23);
            metNumericTextBox2.TabIndex = 6;
            metNumericTextBox2.Value = new decimal(new int[] { 6, 0, 0, 0 });
            metNumericTextBox2.TextChanged += metNumericTextBox2_TextChanged;
            // 
            // checkBox3
            // 
            checkBox3.AutoSize = true;
            checkBox3.Checked = true;
            checkBox3.CheckState = CheckState.Checked;
            checkBox3.Location = new Point(95, 22);
            checkBox3.Name = "checkBox3";
            checkBox3.Size = new Size(121, 19);
            checkBox3.TabIndex = 4;
            checkBox3.Text = "FullWidthCharTwo";
            checkBox3.UseVisualStyleBackColor = true;
            checkBox3.CheckedChanged += checkBox3_CheckedChanged;
            // 
            // checkBox4
            // 
            checkBox4.AutoSize = true;
            checkBox4.Checked = true;
            checkBox4.CheckState = CheckState.Checked;
            checkBox4.Location = new Point(6, 22);
            checkBox4.Name = "checkBox4";
            checkBox4.Size = new Size(83, 19);
            checkBox4.TabIndex = 5;
            checkBox4.Text = "AutoFocus";
            checkBox4.UseVisualStyleBackColor = true;
            checkBox4.CheckedChanged += checkBox4_CheckedChanged;
            // 
            // OverlayShowButton
            // 
            OverlayShowButton.Location = new Point(12, 222);
            OverlayShowButton.Name = "OverlayShowButton";
            OverlayShowButton.Size = new Size(93, 23);
            OverlayShowButton.TabIndex = 5;
            OverlayShowButton.Text = "Show Overlay";
            OverlayShowButton.UseVisualStyleBackColor = true;
            OverlayShowButton.Click += OverlayShowButton_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(598, 270);
            Controls.Add(OverlayShowButton);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            MaximizeBox = false;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Metroit.WindowsForms Sample";
            ((System.ComponentModel.ISupportInitialize)metLimitedTextBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)metMultilineLimitedTextBox1).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)metNumericTextBox1).EndInit();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)metNumericTextBox3).EndInit();
            ((System.ComponentModel.ISupportInitialize)metNumericTextBox2).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Forms.MetLimitedTextBox metLimitedTextBox1;
        private Forms.MetMultilineLimitedTextBox metMultilineLimitedTextBox1;
        private GroupBox groupBox1;
        private Forms.MetNumericTextBox metNumericTextBox1;
        private CheckBox checkBox2;
        private CheckBox checkBox1;
        private Label label3;
        private GroupBox groupBox2;
        private Label label2;
        private Forms.MetNumericTextBox metNumericTextBox3;
        private Label label1;
        private Forms.MetNumericTextBox metNumericTextBox2;
        private CheckBox checkBox3;
        private CheckBox checkBox4;
        private Button OverlayShowButton;
    }
}
