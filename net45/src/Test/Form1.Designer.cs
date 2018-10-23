namespace Test
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.metTextBox3 = new Metroit.Windows.Forms.MetTextBox();
            this.metComboBox1 = new Metroit.Windows.Forms.MetComboBox();
            this.metDateTimePicker1 = new Metroit.Windows.Forms.MetDateTimePicker();
            this.metTextBox2 = new Metroit.Windows.Forms.MetTextBox();
            this.metTextBox1 = new Metroit.Windows.Forms.MetTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.metTextBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.metDateTimePicker1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.metTextBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.metTextBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(216, 17);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(297, 17);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(216, 46);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 5;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(297, 46);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 6;
            this.button4.Text = "button4";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(62, 222);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 8;
            this.button5.Text = "Error";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(143, 222);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(93, 23);
            this.button6.TabIndex = 9;
            this.button6.Text = "ReadOnlyLabel";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(242, 222);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(70, 23);
            this.button7.TabIndex = 10;
            this.button7.Text = "ReadOnly";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(318, 222);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(70, 23);
            this.button8.TabIndex = 12;
            this.button8.Text = "Visible";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // textBox1
            // 
            this.textBox1.AutoCompleteCustomSource.AddRange(new string[] {
            "aaa",
            "aab",
            "aac",
            "aad"});
            this.textBox1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.textBox1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.textBox1.Location = new System.Drawing.Point(272, 156);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 19);
            this.textBox1.TabIndex = 15;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // metTextBox3
            // 
            this.metTextBox3.CustomAutoCompleteBox = autoCompleteBox1;
            this.metTextBox3.CustomAutoCompleteMode = Metroit.Windows.Forms.CustomAutoCompleteMode.Keys;
            this.metTextBox3.Location = new System.Drawing.Point(272, 119);
            this.metTextBox3.Name = "metTextBox3";
            this.metTextBox3.Size = new System.Drawing.Size(100, 19);
            this.metTextBox3.TabIndex = 14;
            this.metTextBox3.TextChangeValidation += new Metroit.Windows.Forms.TextChangeValidationEventHandler(this.metTextBox3_TextChangeValidation);
            this.metTextBox3.TextChanged += new System.EventHandler(this.metTextBox3_TextChanged);
            // 
            // metComboBox1
            // 
            this.metComboBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.metComboBox1.BaseBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.metComboBox1.BaseOuterFrameColor = System.Drawing.Color.Lime;
            this.metComboBox1.FocusBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.metComboBox1.FocusForeColor = System.Drawing.Color.Red;
            this.metComboBox1.FocusOuterFrameColor = System.Drawing.Color.Fuchsia;
            this.metComboBox1.Font = new System.Drawing.Font("メイリオ", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.metComboBox1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.metComboBox1.FormattingEnabled = true;
            this.metComboBox1.Location = new System.Drawing.Point(89, 251);
            this.metComboBox1.Name = "metComboBox1";
            this.metComboBox1.Size = new System.Drawing.Size(121, 32);
            this.metComboBox1.TabIndex = 13;
            // 
            // metDateTimePicker1
            // 
            this.metDateTimePicker1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.metDateTimePicker1.BaseBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.metDateTimePicker1.BaseOuterFrameColor = System.Drawing.Color.Lime;
            this.metDateTimePicker1.CalendarFont = new System.Drawing.Font("メイリオ", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.metDateTimePicker1.CustomFormat = null;
            this.metDateTimePicker1.FocusBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.metDateTimePicker1.FocusForeColor = System.Drawing.Color.Red;
            this.metDateTimePicker1.FocusOuterFrameColor = System.Drawing.Color.Blue;
            this.metDateTimePicker1.Font = new System.Drawing.Font("メイリオ", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.metDateTimePicker1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.metDateTimePicker1.Location = new System.Drawing.Point(62, 185);
            this.metDateTimePicker1.Name = "metDateTimePicker1";
            this.metDateTimePicker1.Size = new System.Drawing.Size(200, 31);
            this.metDateTimePicker1.TabIndex = 7;
            // 
            // metTextBox2
            // 
            this.metTextBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.metTextBox2.CustomAutoCompleteBox = autoCompleteBox2;
            this.metTextBox2.Font = new System.Drawing.Font("メイリオ", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.metTextBox2.Location = new System.Drawing.Point(47, 88);
            this.metTextBox2.Name = "metTextBox2";
            this.metTextBox2.Size = new System.Drawing.Size(100, 24);
            this.metTextBox2.TabIndex = 3;
            // 
            // metTextBox1
            // 
            this.metTextBox1.BaseOuterFrameColor = System.Drawing.Color.Lime;
            this.metTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            autoCompleteBox3.CompareOptions = new System.Globalization.CompareOptions[] {
        System.Globalization.CompareOptions.IgnoreCase,
        System.Globalization.CompareOptions.IgnoreKanaType,
        System.Globalization.CompareOptions.IgnoreWidth};
            autoCompleteBox3.DisplayMember = "Column2";
            autoCompleteBox3.MatchPattern = Metroit.Windows.Forms.MatchPatternType.Partial;
            autoCompleteBox3.ValueMember = "Column1";
            this.metTextBox1.CustomAutoCompleteBox = autoCompleteBox3;
            this.metTextBox1.CustomAutoCompleteKeys = new System.Windows.Forms.Keys[] {
        ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Return)))};
            this.metTextBox1.CustomAutoCompleteMode = Metroit.Windows.Forms.CustomAutoCompleteMode.KeysSuggest;
            this.metTextBox1.FocusBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.metTextBox1.FocusForeColor = System.Drawing.Color.Blue;
            this.metTextBox1.FocusOuterFrameColor = System.Drawing.Color.Blue;
            this.metTextBox1.Font = new System.Drawing.Font("メイリオ", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.metTextBox1.Location = new System.Drawing.Point(12, 12);
            this.metTextBox1.Name = "metTextBox1";
            this.metTextBox1.Size = new System.Drawing.Size(198, 24);
            this.metTextBox1.TabIndex = 0;
            this.metTextBox1.TextChangeValidation += new Metroit.Windows.Forms.TextChangeValidationEventHandler(this.metTextBox1_TextChangeValidation);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(402, 341);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.metTextBox3);
            this.Controls.Add(this.metComboBox1);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.metDateTimePicker1);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.metTextBox2);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.metTextBox1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.metTextBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.metDateTimePicker1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.metTextBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.metTextBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Metroit.Windows.Forms.MetTextBox metTextBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private Metroit.Windows.Forms.MetTextBox metTextBox2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private Metroit.Windows.Forms.MetDateTimePicker metDateTimePicker1;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private Metroit.Windows.Forms.MetComboBox metComboBox1;
        private Metroit.Windows.Forms.MetTextBox metTextBox3;
        private System.Windows.Forms.TextBox textBox1;
    }
}