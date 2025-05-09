namespace Metroit.Windows.FormsTest
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
            Forms.NumericNegativePattern numericNegativePattern1 = new Forms.NumericNegativePattern();
            Forms.NumericPositivePattern numericPositivePattern1 = new Forms.NumericPositivePattern();
            metLimitedTextBox1 = new Metroit.Windows.Forms.MetLimitedTextBox();
            metMultilineLimitedTextBox1 = new Metroit.Windows.Forms.MetMultilineLimitedTextBox();
            label2 = new Label();
            groupBox1 = new GroupBox();
            label3 = new Label();
            metNumericTextBox1 = new Metroit.Windows.Forms.MetNumericTextBox();
            checkBox2 = new CheckBox();
            checkBox1 = new CheckBox();
            ((System.ComponentModel.ISupportInitialize)metLimitedTextBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)metMultilineLimitedTextBox1).BeginInit();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)metNumericTextBox1).BeginInit();
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
            metLimitedTextBox1.TextChanged += metLimitedTextBox1_TextChanged;
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
            metMultilineLimitedTextBox1.Location = new Point(257, 187);
            metMultilineLimitedTextBox1.MaxLineCount = 2;
            metMultilineLimitedTextBox1.MaxLineLength = 6;
            metMultilineLimitedTextBox1.Name = "metMultilineLimitedTextBox1";
            metMultilineLimitedTextBox1.Size = new Size(100, 50);
            metMultilineLimitedTextBox1.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(96, 187);
            label2.Name = "label2";
            label2.Size = new Size(155, 15);
            label2.TabIndex = 2;
            label2.Text = "MetMultilineLimitedTextBox";
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
            metNumericTextBox1.NegativePattern = numericNegativePattern1;
            metNumericTextBox1.PositivePattern = numericPositivePattern1;
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
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(598, 270);
            Controls.Add(groupBox1);
            Controls.Add(label2);
            Controls.Add(metMultilineLimitedTextBox1);
            MaximizeBox = false;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)metLimitedTextBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)metMultilineLimitedTextBox1).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)metNumericTextBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Forms.MetLimitedTextBox metLimitedTextBox1;
        private Forms.MetMultilineLimitedTextBox metMultilineLimitedTextBox1;
        private Label label2;
        private GroupBox groupBox1;
        private Forms.MetNumericTextBox metNumericTextBox1;
        private CheckBox checkBox2;
        private CheckBox checkBox1;
        private Label label3;
    }
}
