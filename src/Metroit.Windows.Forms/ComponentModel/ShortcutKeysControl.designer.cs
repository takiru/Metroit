namespace Metroit.Windows.Forms.ComponentModel
{
    partial class ShortcutKeysControl
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
            checkCtrl = new System.Windows.Forms.CheckBox();
            checkShift = new System.Windows.Forms.CheckBox();
            checkAlt = new System.Windows.Forms.CheckBox();
            keyLabel = new System.Windows.Forms.Label();
            systemKeyLabel = new System.Windows.Forms.Label();
            keysComboBox = new System.Windows.Forms.ComboBox();
            resetButton = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // checkCtrl
            // 
            checkCtrl.AutoSize = true;
            checkCtrl.Location = new System.Drawing.Point(24, 25);
            checkCtrl.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            checkCtrl.Name = "checkCtrl";
            checkCtrl.Size = new System.Drawing.Size(44, 19);
            checkCtrl.TabIndex = 1;
            checkCtrl.Text = "&Ctrl";
            checkCtrl.UseVisualStyleBackColor = true;
            checkCtrl.CheckStateChanged += CheckStateChanged;
            // 
            // checkShift
            // 
            checkShift.AutoSize = true;
            checkShift.Location = new System.Drawing.Point(82, 25);
            checkShift.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            checkShift.Name = "checkShift";
            checkShift.Size = new System.Drawing.Size(50, 19);
            checkShift.TabIndex = 2;
            checkShift.Text = "&Shift";
            checkShift.UseVisualStyleBackColor = true;
            checkShift.CheckStateChanged += CheckStateChanged;
            // 
            // checkAlt
            // 
            checkAlt.AutoSize = true;
            checkAlt.Location = new System.Drawing.Point(145, 25);
            checkAlt.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            checkAlt.Name = "checkAlt";
            checkAlt.Size = new System.Drawing.Size(41, 19);
            checkAlt.TabIndex = 3;
            checkAlt.Text = "&Alt";
            checkAlt.UseVisualStyleBackColor = true;
            checkAlt.CheckStateChanged += CheckStateChanged;
            // 
            // keyLabel
            // 
            keyLabel.AutoSize = true;
            keyLabel.Location = new System.Drawing.Point(9, 61);
            keyLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            keyLabel.Name = "keyLabel";
            keyLabel.Size = new System.Drawing.Size(51, 15);
            keyLabel.TabIndex = 13;
            keyLabel.Text = "キー(&K)：";
            // 
            // systemKeyLabel
            // 
            systemKeyLabel.AutoSize = true;
            systemKeyLabel.Location = new System.Drawing.Point(9, 6);
            systemKeyLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            systemKeyLabel.Name = "systemKeyLabel";
            systemKeyLabel.Size = new System.Drawing.Size(55, 15);
            systemKeyLabel.TabIndex = 14;
            systemKeyLabel.Text = "修飾子：";
            // 
            // keysComboBox
            // 
            keysComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            keysComboBox.FormattingEnabled = true;
            keysComboBox.Location = new System.Drawing.Point(19, 80);
            keysComboBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            keysComboBox.Name = "keysComboBox";
            keysComboBox.Size = new System.Drawing.Size(165, 23);
            keysComboBox.TabIndex = 4;
            keysComboBox.SelectedIndexChanged += keysComboBox_SelectedIndexChanged;
            // 
            // resetButton
            // 
            resetButton.Location = new System.Drawing.Point(191, 80);
            resetButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            resetButton.Name = "resetButton";
            resetButton.Size = new System.Drawing.Size(88, 23);
            resetButton.TabIndex = 5;
            resetButton.Text = "リセット(&R)";
            resetButton.UseVisualStyleBackColor = true;
            resetButton.Click += resetButton_Click;
            // 
            // ShortcutKeysControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(resetButton);
            Controls.Add(keysComboBox);
            Controls.Add(systemKeyLabel);
            Controls.Add(keyLabel);
            Controls.Add(checkAlt);
            Controls.Add(checkShift);
            Controls.Add(checkCtrl);
            Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            Name = "ShortcutKeysControl";
            Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            Size = new System.Drawing.Size(288, 119);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.CheckBox checkCtrl;
        private System.Windows.Forms.CheckBox checkShift;
        private System.Windows.Forms.CheckBox checkAlt;
        private System.Windows.Forms.Label keyLabel;
        private System.Windows.Forms.Label systemKeyLabel;
        private System.Windows.Forms.ComboBox keysComboBox;
        private System.Windows.Forms.Button resetButton;
    }
}
