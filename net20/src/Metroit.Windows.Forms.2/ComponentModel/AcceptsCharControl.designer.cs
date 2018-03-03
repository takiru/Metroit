namespace Metroit.Windows.Forms.ComponentModel
{
    partial class AcceptsCharControl
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
            this.checkAll = new System.Windows.Forms.CheckBox();
            this.checkNumeric = new System.Windows.Forms.CheckBox();
            this.checkAlpha = new System.Windows.Forms.CheckBox();
            this.checkSign = new System.Windows.Forms.CheckBox();
            this.checkCustom = new System.Windows.Forms.CheckBox();
            this.checkHalfNumeric = new System.Windows.Forms.CheckBox();
            this.checkFullNumeric = new System.Windows.Forms.CheckBox();
            this.checkFullAlpha = new System.Windows.Forms.CheckBox();
            this.checkHalfAlpha = new System.Windows.Forms.CheckBox();
            this.checkFullSign = new System.Windows.Forms.CheckBox();
            this.checkHalfSign = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // checkAll
            // 
            this.checkAll.AutoSize = true;
            this.checkAll.Location = new System.Drawing.Point(3, 3);
            this.checkAll.Name = "checkAll";
            this.checkAll.Size = new System.Drawing.Size(69, 16);
            this.checkAll.TabIndex = 7;
            this.checkAll.Text = "全て許可";
            this.checkAll.UseVisualStyleBackColor = true;
            this.checkAll.CheckedChanged += new System.EventHandler(this.checkAll_CheckedChanged);
            // 
            // checkNumeric
            // 
            this.checkNumeric.AutoSize = true;
            this.checkNumeric.Location = new System.Drawing.Point(3, 25);
            this.checkNumeric.Name = "checkNumeric";
            this.checkNumeric.Size = new System.Drawing.Size(48, 16);
            this.checkNumeric.TabIndex = 8;
            this.checkNumeric.Text = "数字";
            this.checkNumeric.ThreeState = true;
            this.checkNumeric.UseVisualStyleBackColor = true;
            this.checkNumeric.CheckStateChanged += new System.EventHandler(this.checkNumeric_CheckStateChanged);
            // 
            // checkAlpha
            // 
            this.checkAlpha.AutoSize = true;
            this.checkAlpha.Location = new System.Drawing.Point(3, 69);
            this.checkAlpha.Name = "checkAlpha";
            this.checkAlpha.Size = new System.Drawing.Size(48, 16);
            this.checkAlpha.TabIndex = 9;
            this.checkAlpha.Text = "英字";
            this.checkAlpha.ThreeState = true;
            this.checkAlpha.UseVisualStyleBackColor = true;
            this.checkAlpha.CheckStateChanged += new System.EventHandler(this.checkAlpha_CheckStateChanged);
            // 
            // checkSign
            // 
            this.checkSign.AutoSize = true;
            this.checkSign.Location = new System.Drawing.Point(3, 113);
            this.checkSign.Name = "checkSign";
            this.checkSign.Size = new System.Drawing.Size(48, 16);
            this.checkSign.TabIndex = 10;
            this.checkSign.Text = "記号";
            this.checkSign.ThreeState = true;
            this.checkSign.UseVisualStyleBackColor = true;
            this.checkSign.CheckStateChanged += new System.EventHandler(this.checkSign_CheckStateChanged);
            // 
            // checkCustom
            // 
            this.checkCustom.AutoSize = true;
            this.checkCustom.Location = new System.Drawing.Point(3, 157);
            this.checkCustom.Name = "checkCustom";
            this.checkCustom.Size = new System.Drawing.Size(60, 16);
            this.checkCustom.TabIndex = 11;
            this.checkCustom.Text = "カスタム";
            this.checkCustom.UseVisualStyleBackColor = true;
            this.checkCustom.CheckedChanged += new System.EventHandler(this.checkCustom_CheckedChanged);
            // 
            // checkHalfNumeric
            // 
            this.checkHalfNumeric.AutoSize = true;
            this.checkHalfNumeric.Location = new System.Drawing.Point(24, 47);
            this.checkHalfNumeric.Name = "checkHalfNumeric";
            this.checkHalfNumeric.Size = new System.Drawing.Size(48, 16);
            this.checkHalfNumeric.TabIndex = 12;
            this.checkHalfNumeric.Text = "半角";
            this.checkHalfNumeric.UseVisualStyleBackColor = true;
            // 
            // checkFullNumeric
            // 
            this.checkFullNumeric.AutoSize = true;
            this.checkFullNumeric.Location = new System.Drawing.Point(78, 47);
            this.checkFullNumeric.Name = "checkFullNumeric";
            this.checkFullNumeric.Size = new System.Drawing.Size(48, 16);
            this.checkFullNumeric.TabIndex = 13;
            this.checkFullNumeric.Text = "全角";
            this.checkFullNumeric.UseVisualStyleBackColor = true;
            // 
            // checkFullAlpha
            // 
            this.checkFullAlpha.AutoSize = true;
            this.checkFullAlpha.Location = new System.Drawing.Point(78, 91);
            this.checkFullAlpha.Name = "checkFullAlpha";
            this.checkFullAlpha.Size = new System.Drawing.Size(48, 16);
            this.checkFullAlpha.TabIndex = 15;
            this.checkFullAlpha.Text = "全角";
            this.checkFullAlpha.UseVisualStyleBackColor = true;
            // 
            // checkHalfAlpha
            // 
            this.checkHalfAlpha.AutoSize = true;
            this.checkHalfAlpha.Location = new System.Drawing.Point(24, 91);
            this.checkHalfAlpha.Name = "checkHalfAlpha";
            this.checkHalfAlpha.Size = new System.Drawing.Size(48, 16);
            this.checkHalfAlpha.TabIndex = 14;
            this.checkHalfAlpha.Text = "半角";
            this.checkHalfAlpha.UseVisualStyleBackColor = true;
            // 
            // checkFullSign
            // 
            this.checkFullSign.AutoSize = true;
            this.checkFullSign.Location = new System.Drawing.Point(78, 135);
            this.checkFullSign.Name = "checkFullSign";
            this.checkFullSign.Size = new System.Drawing.Size(48, 16);
            this.checkFullSign.TabIndex = 17;
            this.checkFullSign.Text = "全角";
            this.checkFullSign.UseVisualStyleBackColor = true;
            // 
            // checkHalfSign
            // 
            this.checkHalfSign.AutoSize = true;
            this.checkHalfSign.Location = new System.Drawing.Point(24, 135);
            this.checkHalfSign.Name = "checkHalfSign";
            this.checkHalfSign.Size = new System.Drawing.Size(48, 16);
            this.checkHalfSign.TabIndex = 16;
            this.checkHalfSign.Text = "半角";
            this.checkHalfSign.UseVisualStyleBackColor = true;
            // 
            // ApprovalCharControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkFullSign);
            this.Controls.Add(this.checkHalfSign);
            this.Controls.Add(this.checkFullAlpha);
            this.Controls.Add(this.checkHalfAlpha);
            this.Controls.Add(this.checkFullNumeric);
            this.Controls.Add(this.checkHalfNumeric);
            this.Controls.Add(this.checkCustom);
            this.Controls.Add(this.checkSign);
            this.Controls.Add(this.checkAlpha);
            this.Controls.Add(this.checkNumeric);
            this.Controls.Add(this.checkAll);
            this.Name = "ApprovalCharControl";
            this.Size = new System.Drawing.Size(150, 178);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkAll;
        private System.Windows.Forms.CheckBox checkNumeric;
        private System.Windows.Forms.CheckBox checkAlpha;
        private System.Windows.Forms.CheckBox checkSign;
        private System.Windows.Forms.CheckBox checkCustom;
        private System.Windows.Forms.CheckBox checkHalfNumeric;
        private System.Windows.Forms.CheckBox checkFullNumeric;
        private System.Windows.Forms.CheckBox checkFullAlpha;
        private System.Windows.Forms.CheckBox checkHalfAlpha;
        private System.Windows.Forms.CheckBox checkFullSign;
        private System.Windows.Forms.CheckBox checkHalfSign;

    }
}
