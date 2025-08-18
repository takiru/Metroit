namespace Metroit.Windows.Forms
{
    partial class MetNotificationPanel
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
            TitleLabel = new System.Windows.Forms.Label();
            FramePanel = new System.Windows.Forms.Panel();
            ScrollablePanel = new MetPanel();
            NotificationLabelStyle = new System.Windows.Forms.Label();
            FramePanel.SuspendLayout();
            SuspendLayout();
            // 
            // TitleLabel
            // 
            TitleLabel.AutoSize = true;
            TitleLabel.Dock = System.Windows.Forms.DockStyle.Top;
            TitleLabel.Location = new System.Drawing.Point(0, 0);
            TitleLabel.Name = "TitleLabel";
            TitleLabel.Size = new System.Drawing.Size(63, 15);
            TitleLabel.TabIndex = 1;
            TitleLabel.Text = "📢 お知らせ";
            // 
            // FramePanel
            // 
            FramePanel.Controls.Add(ScrollablePanel);
            FramePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            FramePanel.Location = new System.Drawing.Point(0, 15);
            FramePanel.Name = "FramePanel";
            FramePanel.Size = new System.Drawing.Size(150, 135);
            FramePanel.TabIndex = 2;
            FramePanel.Paint += FramePanel_Paint;
            // 
            // ScrollablePanel
            // 
            ScrollablePanel.AutoScroll = true;
            ScrollablePanel.BackColor = System.Drawing.Color.White;
            ScrollablePanel.Location = new System.Drawing.Point(1, 1);
            ScrollablePanel.Name = "ScrollablePanel";
            ScrollablePanel.Size = new System.Drawing.Size(148, 133);
            ScrollablePanel.TabIndex = 0;
            // 
            // NotificationLabelStyle
            // 
            NotificationLabelStyle.Location = new System.Drawing.Point(69, 0);
            NotificationLabelStyle.Name = "NotificationLabelStyle";
            NotificationLabelStyle.Size = new System.Drawing.Size(67, 13);
            NotificationLabelStyle.TabIndex = 0;
            NotificationLabelStyle.Text = "Notification Default Style";
            NotificationLabelStyle.Visible = false;
            // 
            // NotificationPanel
            // 
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            Controls.Add(NotificationLabelStyle);
            Controls.Add(FramePanel);
            Controls.Add(TitleLabel);
            Name = "NotificationPanel";
            FramePanel.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Label TitleLabel;
        private System.Windows.Forms.Panel FramePanel;
        private MetPanel ScrollablePanel;
        private System.Windows.Forms.Label NotificationLabelStyle;
    }
}
