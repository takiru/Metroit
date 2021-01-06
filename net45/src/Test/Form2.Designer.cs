namespace Test
{
    partial class Form2
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
            this.metDateTimePicker1 = new Metroit.Windows.Forms.MetDateTimePicker();
            ((System.ComponentModel.ISupportInitialize)(this.metDateTimePicker1)).BeginInit();
            this.SuspendLayout();
            // 
            // metDateTimePicker1
            // 
            this.metDateTimePicker1.Location = new System.Drawing.Point(52, 45);
            this.metDateTimePicker1.Name = "metDateTimePicker1";
            this.metDateTimePicker1.ShowToday = false;
            this.metDateTimePicker1.ShowTodayCircle = false;
            this.metDateTimePicker1.ShowTorailingDates = false;
            this.metDateTimePicker1.Size = new System.Drawing.Size(200, 19);
            this.metDateTimePicker1.TabIndex = 0;
            this.metDateTimePicker1.Value = new System.DateTime(2021, 1, 1, 14, 46, 47, 956);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(324, 276);
            this.Controls.Add(this.metDateTimePicker1);
            this.Name = "Form2";
            this.Text = "Form2";
            this.Load += new System.EventHandler(this.Form2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.metDateTimePicker1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Metroit.Windows.Forms.MetDateTimePicker metDateTimePicker1;
    }
}