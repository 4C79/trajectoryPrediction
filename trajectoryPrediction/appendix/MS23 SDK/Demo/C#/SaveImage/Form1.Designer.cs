namespace SaveImage
{
    partial class Form1
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
            this.DispWnd = new System.Windows.Forms.PictureBox();
            this.buttonSettings = new System.Windows.Forms.Button();
            this.buttonPlay = new System.Windows.Forms.Button();
            this.buttonSnap = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.StateLabel = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxTriggerPeriod = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonSoftTrigger = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxCamMode = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.comboBoxSaveFormat = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBoxAutoSave = new System.Windows.Forms.CheckBox();
            this.checkBoxAutoSaveOnlyTrigger = new System.Windows.Forms.CheckBox();
            this.textBoxSaveDir = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.DispWnd)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // DispWnd
            // 
            this.DispWnd.Location = new System.Drawing.Point(12, 12);
            this.DispWnd.Name = "DispWnd";
            this.DispWnd.Size = new System.Drawing.Size(648, 581);
            this.DispWnd.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.DispWnd.TabIndex = 0;
            this.DispWnd.TabStop = false;
            // 
            // buttonSettings
            // 
            this.buttonSettings.Location = new System.Drawing.Point(17, 27);
            this.buttonSettings.Name = "buttonSettings";
            this.buttonSettings.Size = new System.Drawing.Size(75, 23);
            this.buttonSettings.TabIndex = 1;
            this.buttonSettings.Text = "相机设置";
            this.buttonSettings.UseVisualStyleBackColor = true;
            this.buttonSettings.Click += new System.EventHandler(this.buttonSettings_Click);
            // 
            // buttonPlay
            // 
            this.buttonPlay.Location = new System.Drawing.Point(17, 62);
            this.buttonPlay.Name = "buttonPlay";
            this.buttonPlay.Size = new System.Drawing.Size(75, 23);
            this.buttonPlay.TabIndex = 2;
            this.buttonPlay.Text = "播放";
            this.buttonPlay.UseVisualStyleBackColor = true;
            this.buttonPlay.Click += new System.EventHandler(this.buttonPlay_Click);
            // 
            // buttonSnap
            // 
            this.buttonSnap.Location = new System.Drawing.Point(110, 27);
            this.buttonSnap.Name = "buttonSnap";
            this.buttonSnap.Size = new System.Drawing.Size(75, 23);
            this.buttonSnap.TabIndex = 4;
            this.buttonSnap.Text = "抓图";
            this.buttonSnap.UseVisualStyleBackColor = true;
            this.buttonSnap.Click += new System.EventHandler(this.buttonSnap_Click);
            // 
            // buttonStop
            // 
            this.buttonStop.Location = new System.Drawing.Point(110, 62);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(75, 23);
            this.buttonStop.TabIndex = 3;
            this.buttonStop.Text = "停止";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // StateLabel
            // 
            this.StateLabel.AutoSize = true;
            this.StateLabel.Location = new System.Drawing.Point(12, 596);
            this.StateLabel.Name = "StateLabel";
            this.StateLabel.Size = new System.Drawing.Size(41, 12);
            this.StateLabel.TabIndex = 5;
            this.StateLabel.Text = "状态栏";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.textBoxTriggerPeriod);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.buttonSoftTrigger);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.comboBoxCamMode);
            this.groupBox1.Controls.Add(this.buttonSettings);
            this.groupBox1.Controls.Add(this.buttonPlay);
            this.groupBox1.Controls.Add(this.buttonSnap);
            this.groupBox1.Controls.Add(this.buttonStop);
            this.groupBox1.Location = new System.Drawing.Point(666, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(360, 147);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "相机控制";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(330, 104);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 12);
            this.label5.TabIndex = 10;
            this.label5.Text = "ms";
            // 
            // textBoxTriggerPeriod
            // 
            this.textBoxTriggerPeriod.Location = new System.Drawing.Point(254, 100);
            this.textBoxTriggerPeriod.Name = "textBoxTriggerPeriod";
            this.textBoxTriggerPeriod.Size = new System.Drawing.Size(70, 21);
            this.textBoxTriggerPeriod.TabIndex = 9;
            this.textBoxTriggerPeriod.TextChanged += new System.EventHandler(this.textBoxTriggerPeriod_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(173, 105);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = "周期软触发：";
            // 
            // buttonSoftTrigger
            // 
            this.buttonSoftTrigger.Location = new System.Drawing.Point(211, 62);
            this.buttonSoftTrigger.Name = "buttonSoftTrigger";
            this.buttonSoftTrigger.Size = new System.Drawing.Size(136, 23);
            this.buttonSoftTrigger.TabIndex = 7;
            this.buttonSoftTrigger.Text = "一次软触发";
            this.buttonSoftTrigger.UseVisualStyleBackColor = true;
            this.buttonSoftTrigger.Click += new System.EventHandler(this.buttonSoftTrigger_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(209, 33);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "模式：";
            // 
            // comboBoxCamMode
            // 
            this.comboBoxCamMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCamMode.FormattingEnabled = true;
            this.comboBoxCamMode.Items.AddRange(new object[] {
            "连续",
            "软触发",
            "外触发"});
            this.comboBoxCamMode.Location = new System.Drawing.Point(254, 28);
            this.comboBoxCamMode.Name = "comboBoxCamMode";
            this.comboBoxCamMode.Size = new System.Drawing.Size(93, 20);
            this.comboBoxCamMode.TabIndex = 5;
            this.comboBoxCamMode.SelectedIndexChanged += new System.EventHandler(this.comboBoxCamMode_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.comboBoxSaveFormat);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.checkBoxAutoSave);
            this.groupBox2.Controls.Add(this.checkBoxAutoSaveOnlyTrigger);
            this.groupBox2.Controls.Add(this.textBoxSaveDir);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(666, 165);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(360, 202);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "保存设置";
            // 
            // comboBoxSaveFormat
            // 
            this.comboBoxSaveFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSaveFormat.FormattingEnabled = true;
            this.comboBoxSaveFormat.Items.AddRange(new object[] {
            "BMP",
            "PNG",
            "JPG"});
            this.comboBoxSaveFormat.Location = new System.Drawing.Point(60, 120);
            this.comboBoxSaveFormat.Name = "comboBoxSaveFormat";
            this.comboBoxSaveFormat.Size = new System.Drawing.Size(121, 20);
            this.comboBoxSaveFormat.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 124);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "格式：";
            // 
            // checkBoxAutoSave
            // 
            this.checkBoxAutoSave.AutoSize = true;
            this.checkBoxAutoSave.Location = new System.Drawing.Point(17, 75);
            this.checkBoxAutoSave.Name = "checkBoxAutoSave";
            this.checkBoxAutoSave.Size = new System.Drawing.Size(72, 16);
            this.checkBoxAutoSave.TabIndex = 3;
            this.checkBoxAutoSave.Text = "自动保存";
            this.checkBoxAutoSave.UseVisualStyleBackColor = true;
            // 
            // checkBoxAutoSaveOnlyTrigger
            // 
            this.checkBoxAutoSaveOnlyTrigger.AutoSize = true;
            this.checkBoxAutoSaveOnlyTrigger.Location = new System.Drawing.Point(110, 75);
            this.checkBoxAutoSaveOnlyTrigger.Name = "checkBoxAutoSaveOnlyTrigger";
            this.checkBoxAutoSaveOnlyTrigger.Size = new System.Drawing.Size(108, 16);
            this.checkBoxAutoSaveOnlyTrigger.TabIndex = 2;
            this.checkBoxAutoSaveOnlyTrigger.Text = "仅当触发时保存";
            this.checkBoxAutoSaveOnlyTrigger.UseVisualStyleBackColor = true;
            // 
            // textBoxSaveDir
            // 
            this.textBoxSaveDir.Location = new System.Drawing.Point(62, 26);
            this.textBoxSaveDir.Name = "textBoxSaveDir";
            this.textBoxSaveDir.Size = new System.Drawing.Size(292, 21);
            this.textBoxSaveDir.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "目录：";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1038, 617);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.StateLabel);
            this.Controls.Add(this.DispWnd);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.DispWnd)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox DispWnd;
        private System.Windows.Forms.Button buttonSettings;
        private System.Windows.Forms.Button buttonPlay;
        private System.Windows.Forms.Button buttonSnap;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.Label StateLabel;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBoxSaveDir;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBoxAutoSave;
        private System.Windows.Forms.CheckBox checkBoxAutoSaveOnlyTrigger;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxSaveFormat;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxCamMode;
        private System.Windows.Forms.Button buttonSoftTrigger;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxTriggerPeriod;
        private System.Windows.Forms.Label label4;
    }
}

