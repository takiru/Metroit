namespace Metroit.Windows.Forms.ComponentModel
{
    partial class NumericNegativePatternControl
    {
        /// <summary> 
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナで生成されたコード

        /// <summary> 
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.PercentPatternLabel = new System.Windows.Forms.Label();
            this.comboPPattern = new System.Windows.Forms.ComboBox();
            this.CurrencyPatternLabel = new System.Windows.Forms.Label();
            this.comboCPattern = new System.Windows.Forms.ComboBox();
            this.NumericPatternLabel = new System.Windows.Forms.Label();
            this.comboNPattern = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // PercentPatternLabel
            // 
            this.PercentPatternLabel.AutoSize = true;
            this.PercentPatternLabel.Location = new System.Drawing.Point(3, 82);
            this.PercentPatternLabel.Name = "PercentPatternLabel";
            this.PercentPatternLabel.Size = new System.Drawing.Size(88, 12);
            this.PercentPatternLabel.TabIndex = 7;
            this.PercentPatternLabel.Text = "パーセント表記時";
            // 
            // comboPPattern
            // 
            this.comboPPattern.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboPPattern.FormattingEnabled = true;
            this.comboPPattern.Location = new System.Drawing.Point(5, 97);
            this.comboPPattern.Name = "comboPPattern";
            this.comboPPattern.Size = new System.Drawing.Size(121, 20);
            this.comboPPattern.TabIndex = 30;
            this.comboPPattern.SelectedIndexChanged += new System.EventHandler(this.comboPPattern_SelectedIndexChanged);
            // 
            // CurrencyPatternLabel
            // 
            this.CurrencyPatternLabel.AutoSize = true;
            this.CurrencyPatternLabel.Location = new System.Drawing.Point(3, 41);
            this.CurrencyPatternLabel.Name = "CurrencyPatternLabel";
            this.CurrencyPatternLabel.Size = new System.Drawing.Size(65, 12);
            this.CurrencyPatternLabel.TabIndex = 5;
            this.CurrencyPatternLabel.Text = "通貨表記時";
            // 
            // comboCPattern
            // 
            this.comboCPattern.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboCPattern.FormattingEnabled = true;
            this.comboCPattern.Location = new System.Drawing.Point(5, 56);
            this.comboCPattern.Name = "comboCPattern";
            this.comboCPattern.Size = new System.Drawing.Size(121, 20);
            this.comboCPattern.TabIndex = 20;
            this.comboCPattern.SelectedIndexChanged += new System.EventHandler(this.comboCPattern_SelectedIndexChanged);
            // 
            // NumericPatternLabel
            // 
            this.NumericPatternLabel.AutoSize = true;
            this.NumericPatternLabel.Location = new System.Drawing.Point(3, 0);
            this.NumericPatternLabel.Name = "NumericPatternLabel";
            this.NumericPatternLabel.Size = new System.Drawing.Size(65, 12);
            this.NumericPatternLabel.TabIndex = 9;
            this.NumericPatternLabel.Text = "数値表記時";
            // 
            // comboNPattern
            // 
            this.comboNPattern.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboNPattern.FormattingEnabled = true;
            this.comboNPattern.Location = new System.Drawing.Point(5, 15);
            this.comboNPattern.Name = "comboNPattern";
            this.comboNPattern.Size = new System.Drawing.Size(121, 20);
            this.comboNPattern.TabIndex = 10;
            this.comboNPattern.SelectedIndexChanged += new System.EventHandler(this.comboNPattern_SelectedIndexChanged);
            // 
            // NumericNegativePatternControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.NumericPatternLabel);
            this.Controls.Add(this.comboNPattern);
            this.Controls.Add(this.PercentPatternLabel);
            this.Controls.Add(this.comboPPattern);
            this.Controls.Add(this.CurrencyPatternLabel);
            this.Controls.Add(this.comboCPattern);
            this.Name = "NumericNegativePatternControl";
            this.Size = new System.Drawing.Size(150, 128);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label PercentPatternLabel;
        private System.Windows.Forms.ComboBox comboPPattern;
        private System.Windows.Forms.Label CurrencyPatternLabel;
        private System.Windows.Forms.ComboBox comboCPattern;
        private System.Windows.Forms.Label NumericPatternLabel;
        private System.Windows.Forms.ComboBox comboNPattern;
    }
}
