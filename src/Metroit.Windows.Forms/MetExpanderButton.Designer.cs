namespace Metroit.Windows.Forms
{
    partial class MetExpanderButton
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

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.titleLabel = new System.Windows.Forms.Label();
            this.expanderIcon = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.expanderIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // titleLabel
            // 
            this.titleLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.titleLabel.BackColor = System.Drawing.Color.Transparent;
            this.titleLabel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.titleLabel.Location = new System.Drawing.Point(25, 0);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(86, 10);
            this.titleLabel.TabIndex = 4;
            this.titleLabel.FontChanged += new System.EventHandler(this.titleLabel_FontChanged);
            this.titleLabel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.HeaderMouseClick);
            this.titleLabel.MouseEnter += new System.EventHandler(this.HeaderMouseEnter);
            this.titleLabel.MouseLeave += new System.EventHandler(this.HeaderMouseLeave);
            // 
            // expanderIcon
            // 
            this.expanderIcon.Cursor = System.Windows.Forms.Cursors.Hand;
            this.expanderIcon.Location = new System.Drawing.Point(0, 0);
            this.expanderIcon.Margin = new System.Windows.Forms.Padding(3, 3, 2, 3);
            this.expanderIcon.Name = "expanderIcon";
            this.expanderIcon.Size = new System.Drawing.Size(23, 23);
            this.expanderIcon.TabIndex = 3;
            this.expanderIcon.TabStop = false;
            this.expanderIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.HeaderMouseClick);
            this.expanderIcon.MouseEnter += new System.EventHandler(this.HeaderMouseEnter);
            this.expanderIcon.MouseLeave += new System.EventHandler(this.HeaderMouseLeave);
            // 
            // ExpanderButton
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.expanderIcon);
            this.Name = "ExpanderButton";
            this.Size = new System.Drawing.Size(111, 23);
            this.SizeChanged += new System.EventHandler(this.ExpanderButton_SizeChanged);
            this.ParentChanged += new System.EventHandler(this.ExpanderButton_ParentChanged);
            ((System.ComponentModel.ISupportInitialize)(this.expanderIcon)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.PictureBox expanderIcon;
    }
}
