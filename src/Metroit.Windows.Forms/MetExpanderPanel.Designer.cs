namespace Metroit.Windows.Forms
{
    partial class MetExpanderPanel
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
            this.components = new System.ComponentModel.Container();
            this.wrapPanel = new System.Windows.Forms.Panel();
            this.animationTimer = new System.Windows.Forms.Timer(this.components);
            this.expanderButton = new Metroit.Windows.Forms.MetExpanderButton();
            this.wrapPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // wrapPanel
            // 
            this.wrapPanel.BackColor = System.Drawing.Color.Transparent;
            this.wrapPanel.Controls.Add(this.expanderButton);
            this.wrapPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.wrapPanel.Location = new System.Drawing.Point(0, 0);
            this.wrapPanel.Name = "wrapPanel";
            this.wrapPanel.Padding = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.wrapPanel.Size = new System.Drawing.Size(365, 100);
            this.wrapPanel.TabIndex = 0;
            // 
            // animationTimer
            // 
            this.animationTimer.Interval = 10;
            this.animationTimer.Tick += new System.EventHandler(this.animationTimer_Tick);
            // 
            // expanderButton
            // 
            this.expanderButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.expanderButton.LineThickness = 1F;
            this.expanderButton.Location = new System.Drawing.Point(0, 0);
            this.expanderButton.Name = "expanderButton";
            this.expanderButton.Size = new System.Drawing.Size(365, 13);
            this.expanderButton.Svg.Collapsed.Rotation = 0F;
            this.expanderButton.Svg.Collapsed.SvgStream = null;
            this.expanderButton.Svg.Expanded.Rotation = 180F;
            this.expanderButton.Svg.Expanded.SvgStream = null;
            this.expanderButton.TabIndex = 0;
            this.expanderButton.TabStop = false;
            this.expanderButton.ExpandStateChanged += new Metroit.Windows.Forms.ExpandStateEventHandler(this.expanderButton_ExpandStateChanged);
            this.expanderButton.SizeChanged += new System.EventHandler(this.expanderButton_SizeChanged);
            // 
            // ExpanderPanel
            // 
            this.Controls.Add(this.wrapPanel);
            this.Size = new System.Drawing.Size(365, 319);
            this.wrapPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel wrapPanel;
        private Metroit.Windows.Forms.MetExpanderButton expanderButton;
        private System.Windows.Forms.Timer animationTimer;
    }
}
