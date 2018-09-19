namespace Test
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            Metroit.Windows.Forms.AutoCompleteBox autoCompleteBox2 = new Metroit.Windows.Forms.AutoCompleteBox();
            this.metTextBox1 = new Metroit.Windows.Forms.MetTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.metTextBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // metTextBox1
            // 
            this.metTextBox1.AutoFocus = true;
            autoCompleteBox2.CompareOptions = new System.Globalization.CompareOptions[] {
        System.Globalization.CompareOptions.IgnoreCase,
        System.Globalization.CompareOptions.IgnoreWidth,
        System.Globalization.CompareOptions.IgnoreKanaType};
            autoCompleteBox2.DisplayMember = "Column2";
            autoCompleteBox2.MatchPattern = Metroit.Windows.Forms.MatchPatternType.Partial;
            autoCompleteBox2.ValueMember = "Column1";
            this.metTextBox1.CustomAutoCompleteBox = autoCompleteBox2;
            this.metTextBox1.CustomAutoCompleteKeys = new System.Windows.Forms.Keys[] {
        ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Return)))};
            this.metTextBox1.CustomAutoCompleteMode = true;
            this.metTextBox1.FocusBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.metTextBox1.FocusForeColor = System.Drawing.Color.Lime;
            this.metTextBox1.Font = new System.Drawing.Font("メイリオ", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.metTextBox1.Location = new System.Drawing.Point(13, 13);
            this.metTextBox1.MaxLength = 50;
            this.metTextBox1.Name = "metTextBox1";
            this.metTextBox1.Size = new System.Drawing.Size(227, 31);
            this.metTextBox1.TabIndex = 14;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(426, 294);
            this.Controls.Add(this.metTextBox1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.metTextBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Metroit.Windows.Forms.MetTextBox metTextBox1;
    }
}

