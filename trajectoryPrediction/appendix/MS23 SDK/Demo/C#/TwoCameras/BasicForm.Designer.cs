//BIG5 TRANS ALLOWED
namespace Basic
{
    partial class BasicForm
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
            this.PreviewBox = new System.Windows.Forms.PictureBox();
            this.StateLabel = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.PreviewBox2 = new System.Windows.Forms.PictureBox();
            this.BtnSettings2 = new System.Windows.Forms.Button();
            this.BtnPlay2 = new System.Windows.Forms.Button();
            this.StateLabel2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.PreviewBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PreviewBox2)).BeginInit();
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
            // PreviewBox
            // 
            this.PreviewBox.Location = new System.Drawing.Point(12, 51);
            this.PreviewBox.Name = "PreviewBox";
            this.PreviewBox.Size = new System.Drawing.Size(545, 426);
            this.PreviewBox.TabIndex = 1;
            this.PreviewBox.TabStop = false;
            // 
            // StateLabel
            // 
            this.StateLabel.AutoSize = true;
            this.StateLabel.Location = new System.Drawing.Point(14, 488);
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
            // PreviewBox2
            // 
            this.PreviewBox2.Location = new System.Drawing.Point(585, 51);
            this.PreviewBox2.Name = "PreviewBox2";
            this.PreviewBox2.Size = new System.Drawing.Size(545, 426);
            this.PreviewBox2.TabIndex = 6;
            this.PreviewBox2.TabStop = false;
            // 
            // BtnSettings2
            // 
            this.BtnSettings2.Location = new System.Drawing.Point(677, 12);
            this.BtnSettings2.Name = "BtnSettings2";
            this.BtnSettings2.Size = new System.Drawing.Size(75, 23);
            this.BtnSettings2.TabIndex = 3;
            this.BtnSettings2.Text = "Settings";
            this.BtnSettings2.UseVisualStyleBackColor = true;
            this.BtnSettings2.Click += new System.EventHandler(this.BtnSettings2_Click);
            // 
            // BtnPlay2
            // 
            this.BtnPlay2.Location = new System.Drawing.Point(585, 12);
            this.BtnPlay2.Name = "BtnPlay2";
            this.BtnPlay2.Size = new System.Drawing.Size(75, 23);
            this.BtnPlay2.TabIndex = 4;
            this.BtnPlay2.Text = "Play";
            this.BtnPlay2.UseVisualStyleBackColor = true;
            this.BtnPlay2.Click += new System.EventHandler(this.BtnPlay2_Click);
            // 
            // StateLabel2
            // 
            this.StateLabel2.AutoSize = true;
            this.StateLabel2.Location = new System.Drawing.Point(587, 487);
            this.StateLabel2.Name = "StateLabel2";
            this.StateLabel2.Size = new System.Drawing.Size(41, 12);
            this.StateLabel2.TabIndex = 7;
            this.StateLabel2.Text = "状态栏";
            // 
            // BasicForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1143, 511);
            this.Controls.Add(this.StateLabel2);
            this.Controls.Add(this.PreviewBox2);
            this.Controls.Add(this.BtnSettings2);
            this.Controls.Add(this.BtnPlay2);
            this.Controls.Add(this.StateLabel);
            this.Controls.Add(this.PreviewBox);
            this.Controls.Add(this.BtnSettings);
            this.Controls.Add(this.BtnPlay);
            this.Name = "BasicForm";
            this.Text = "Camera C# demo[Basic]";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BasicForm_FormClosing);
            this.Load += new System.EventHandler(this.BasicForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PreviewBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PreviewBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnPlay;
        private System.Windows.Forms.Button BtnSettings;
        private System.Windows.Forms.PictureBox PreviewBox;
        private System.Windows.Forms.Label StateLabel;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.PictureBox PreviewBox2;
        private System.Windows.Forms.Button BtnSettings2;
        private System.Windows.Forms.Button BtnPlay2;
        private System.Windows.Forms.Label StateLabel2;
    }
}

