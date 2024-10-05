//BIG5 TRANS ALLOWED
namespace Basic
{
    partial class Advanced
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.BtnPlay = new System.Windows.Forms.Button();
            this.BtnSettings = new System.Windows.Forms.Button();
            this.BtnSnapshot = new System.Windows.Forms.Button();
            this.PreviewBox = new System.Windows.Forms.PictureBox();
            this.StateLabel = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.SaveImage = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.PreviewBox)).BeginInit();
            this.SuspendLayout();
            // 
            // BtnPlay
            // 
            this.BtnPlay.Location = new System.Drawing.Point(12, 12);
            this.BtnPlay.Name = "BtnPlay";
            this.BtnPlay.Size = new System.Drawing.Size(75, 23);
            this.BtnPlay.TabIndex = 0;
            this.BtnPlay.Text = "Play";
            this.BtnPlay.UseVisualStyleBackColor = true;
            this.BtnPlay.Click += new System.EventHandler(this.BtnPlay_Click);
            // 
            // BtnSettings
            // 
            this.BtnSettings.Location = new System.Drawing.Point(104, 12);
            this.BtnSettings.Name = "BtnSettings";
            this.BtnSettings.Size = new System.Drawing.Size(75, 23);
            this.BtnSettings.TabIndex = 0;
            this.BtnSettings.Text = "Settings";
            this.BtnSettings.UseVisualStyleBackColor = true;
            this.BtnSettings.Click += new System.EventHandler(this.BtnSettings_Click);
            // 
            // BtnSnapshot
            // 
            this.BtnSnapshot.Location = new System.Drawing.Point(195, 12);
            this.BtnSnapshot.Name = "BtnSnapshot";
            this.BtnSnapshot.Size = new System.Drawing.Size(75, 23);
            this.BtnSnapshot.TabIndex = 0;
            this.BtnSnapshot.Text = "Snapshot";
            this.BtnSnapshot.UseVisualStyleBackColor = true;
            this.BtnSnapshot.Click += new System.EventHandler(this.BtnSnapshot_Click);
            // 
            // PreviewBox
            // 
            this.PreviewBox.Location = new System.Drawing.Point(12, 51);
            this.PreviewBox.Name = "PreviewBox";
            this.PreviewBox.Size = new System.Drawing.Size(673, 462);
            this.PreviewBox.TabIndex = 1;
            this.PreviewBox.TabStop = false;
            // 
            // StateLabel
            // 
            this.StateLabel.AutoSize = true;
            this.StateLabel.Location = new System.Drawing.Point(14, 523);
            this.StateLabel.Name = "StateLabel";
            this.StateLabel.Size = new System.Drawing.Size(41, 12);
            this.StateLabel.TabIndex = 2;
            this.StateLabel.Text = "状态栏";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timer2
            // 
            this.timer2.Enabled = true;
            this.timer2.Interval = 50;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // SaveImage
            // 
            this.SaveImage.Location = new System.Drawing.Point(288, 12);
            this.SaveImage.Name = "SaveImage";
            this.SaveImage.Size = new System.Drawing.Size(75, 23);
            this.SaveImage.TabIndex = 3;
            this.SaveImage.Text = "SaveImage";
            this.SaveImage.UseVisualStyleBackColor = true;
            this.SaveImage.Click += new System.EventHandler(this.SaveImage_Click);
            // 
            // Advanced
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(697, 546);
            this.Controls.Add(this.SaveImage);
            this.Controls.Add(this.StateLabel);
            this.Controls.Add(this.PreviewBox);
            this.Controls.Add(this.BtnSnapshot);
            this.Controls.Add(this.BtnSettings);
            this.Controls.Add(this.BtnPlay);
            this.Name = "Advanced";
            this.Text = "Camera C# demo[Advanced]";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BasicForm_FormClosing);
            this.Load += new System.EventHandler(this.BasicForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PreviewBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnPlay;
        private System.Windows.Forms.Button BtnSettings;
        private System.Windows.Forms.Button BtnSnapshot;
        private System.Windows.Forms.PictureBox PreviewBox;
        private System.Windows.Forms.Label StateLabel;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Button SaveImage;
    }
}

