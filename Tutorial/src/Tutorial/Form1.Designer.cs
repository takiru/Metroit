namespace Tutorial
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Metroit.Windows.Forms.AutoCompleteBox autoCompleteBox1 = new Metroit.Windows.Forms.AutoCompleteBox();
            Metroit.Windows.Forms.AutoCompleteBox autoCompleteBox2 = new Metroit.Windows.Forms.AutoCompleteBox();
            Metroit.Windows.Forms.AutoCompleteBox autoCompleteBox3 = new Metroit.Windows.Forms.AutoCompleteBox();
            Metroit.Windows.Forms.NumericNegativePattern numericNegativePattern1 = new Metroit.Windows.Forms.NumericNegativePattern();
            Metroit.Windows.Forms.NumericPositivePattern numericPositivePattern1 = new Metroit.Windows.Forms.NumericPositivePattern();
            this.metTextBox1 = new Metroit.Windows.Forms.MetTextBox();
            this.metLimitedTextBox1 = new Metroit.Windows.Forms.MetLimitedTextBox();
            this.metNumericTextBox1 = new Metroit.Windows.Forms.MetNumericTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.metComboBox1 = new Metroit.Windows.Forms.MetComboBox();
            this.metDateTimePicker1 = new Metroit.Windows.Forms.MetDateTimePicker();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.metTextBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.metLimitedTextBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.metNumericTextBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.metDateTimePicker1)).BeginInit();
            this.SuspendLayout();
            // 
            // metTextBox1
            // 
            this.metTextBox1.CustomAutoCompleteBox = autoCompleteBox1;
            this.metTextBox1.Location = new System.Drawing.Point(135, 37);
            this.metTextBox1.Name = "metTextBox1";
            this.metTextBox1.Size = new System.Drawing.Size(100, 19);
            this.metTextBox1.TabIndex = 0;
            this.metTextBox1.Text = "Sample";
            // 
            // metLimitedTextBox1
            // 
            this.metLimitedTextBox1.CustomAutoCompleteBox = autoCompleteBox2;
            this.metLimitedTextBox1.CustomChars = new string[0];
            this.metLimitedTextBox1.ExcludeChars = new string[0];
            this.metLimitedTextBox1.FullSignSpecialChars = new string[0];
            this.metLimitedTextBox1.Location = new System.Drawing.Point(135, 62);
            this.metLimitedTextBox1.Name = "metLimitedTextBox1";
            this.metLimitedTextBox1.Size = new System.Drawing.Size(100, 19);
            this.metLimitedTextBox1.TabIndex = 1;
            this.metLimitedTextBox1.Text = "Sample";
            // 
            // metNumericTextBox1
            // 
            this.metNumericTextBox1.AutoFocus = true;
            this.metNumericTextBox1.CustomAutoCompleteBox = autoCompleteBox3;
            this.metNumericTextBox1.DecimalDigits = 2;
            this.metNumericTextBox1.GroupSizes = new int[] {
        3};
            this.metNumericTextBox1.Location = new System.Drawing.Point(135, 87);
            this.metNumericTextBox1.MaxValue = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.metNumericTextBox1.Name = "metNumericTextBox1";
            this.metNumericTextBox1.NegativePattern = numericNegativePattern1;
            this.metNumericTextBox1.PositivePattern = numericPositivePattern1;
            this.metNumericTextBox1.Size = new System.Drawing.Size(100, 19);
            this.metNumericTextBox1.TabIndex = 2;
            this.metNumericTextBox1.Value = new decimal(new int[] {
            1234,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "MetTextBox";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "MetLimitedTextBox";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(109, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "MetNumericTextBox";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 115);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = "MetComboBox";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 143);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(105, 12);
            this.label5.TabIndex = 9;
            this.label5.Text = "MetDateTimePicker";
            // 
            // metComboBox1
            // 
            this.metComboBox1.FormattingEnabled = true;
            this.metComboBox1.Location = new System.Drawing.Point(135, 112);
            this.metComboBox1.Name = "metComboBox1";
            this.metComboBox1.Size = new System.Drawing.Size(121, 20);
            this.metComboBox1.TabIndex = 10;
            this.metComboBox1.Text = "test";
            // 
            // metDateTimePicker1
            // 
            this.metDateTimePicker1.CustomFormat = null;
            this.metDateTimePicker1.Location = new System.Drawing.Point(135, 138);
            this.metDateTimePicker1.Name = "metDateTimePicker1";
            this.metDateTimePicker1.Size = new System.Drawing.Size(200, 19);
            this.metDateTimePicker1.TabIndex = 11;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(135, 179);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 12;
            this.button1.Text = "Error";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(362, 214);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.metDateTimePicker1);
            this.Controls.Add(this.metComboBox1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.metNumericTextBox1);
            this.Controls.Add(this.metLimitedTextBox1);
            this.Controls.Add(this.metTextBox1);
            this.KeyPreview = true;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.metTextBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.metLimitedTextBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.metNumericTextBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.metDateTimePicker1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Metroit.Windows.Forms.MetTextBox metTextBox1;
        private Metroit.Windows.Forms.MetLimitedTextBox metLimitedTextBox1;
        private Metroit.Windows.Forms.MetNumericTextBox metNumericTextBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private Metroit.Windows.Forms.MetComboBox metComboBox1;
        private Metroit.Windows.Forms.MetDateTimePicker metDateTimePicker1;
        private System.Windows.Forms.Button button1;
    }
}

