namespace CIdent.Forms
{
    partial class frmIndustrialCamera
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmIndustrialCamera));
            this.pic_Image = new CIdent.Controls.CropPicture();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btn_right = new System.Windows.Forms.Button();
            this.btn_left = new System.Windows.Forms.Button();
            this.btn_positive = new System.Windows.Forms.Button();
            this.pic_right = new CIdentCommon.Controls.CPictureBox();
            this.pic_left = new CIdentCommon.Controls.CPictureBox();
            this.pic_positive = new CIdentCommon.Controls.CPictureBox();
            this.btnGetPhoto = new System.Windows.Forms.Button();
            this.btnInitDevice = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pic_Image)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pic_right)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_left)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_positive)).BeginInit();
            this.SuspendLayout();
            // 
            // pic_Image
            // 
            this.pic_Image.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pic_Image.CropImage = null;
            this.pic_Image.CropImageFile = "";
            this.pic_Image.CropRect = ((System.Drawing.RectangleF)(resources.GetObject("pic_Image.CropRect")));
            this.pic_Image.Location = new System.Drawing.Point(8, 3);
            this.pic_Image.Name = "pic_Image";
            this.pic_Image.ShowCrop = true;
            this.pic_Image.Size = new System.Drawing.Size(800, 600);
            this.pic_Image.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pic_Image.TabIndex = 16;
            this.pic_Image.TabStop = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btn_right);
            this.groupBox3.Controls.Add(this.btn_left);
            this.groupBox3.Controls.Add(this.btn_positive);
            this.groupBox3.Controls.Add(this.pic_right);
            this.groupBox3.Controls.Add(this.pic_left);
            this.groupBox3.Controls.Add(this.pic_positive);
            this.groupBox3.Controls.Add(this.btnGetPhoto);
            this.groupBox3.Controls.Add(this.btnInitDevice);
            this.groupBox3.Location = new System.Drawing.Point(814, -6);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(131, 607);
            this.groupBox3.TabIndex = 35;
            this.groupBox3.TabStop = false;
            // 
            // btn_right
            // 
            this.btn_right.Location = new System.Drawing.Point(25, 575);
            this.btn_right.Name = "btn_right";
            this.btn_right.Size = new System.Drawing.Size(80, 26);
            this.btn_right.TabIndex = 29;
            this.btn_right.Text = "右侧面照";
            this.btn_right.UseVisualStyleBackColor = true;
            // 
            // btn_left
            // 
            this.btn_left.Location = new System.Drawing.Point(26, 374);
            this.btn_left.Name = "btn_left";
            this.btn_left.Size = new System.Drawing.Size(80, 26);
            this.btn_left.TabIndex = 28;
            this.btn_left.Text = "左侧面照";
            this.btn_left.UseVisualStyleBackColor = true;
            // 
            // btn_positive
            // 
            this.btn_positive.Location = new System.Drawing.Point(26, 177);
            this.btn_positive.Name = "btn_positive";
            this.btn_positive.Size = new System.Drawing.Size(80, 26);
            this.btn_positive.TabIndex = 27;
            this.btn_positive.Text = "正面照";
            this.btn_positive.UseVisualStyleBackColor = true;
            // 
            // pic_right
            // 
            this.pic_right.BorderStyle = System.Windows.Forms.Border3DStyle.Flat;
            this.pic_right.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pic_right.DisplayText = "点击拍照";
            this.pic_right.FileName = "*";
            this.pic_right.ImageFromBase64 = null;
            this.pic_right.ImageStream = null;
            this.pic_right.Location = new System.Drawing.Point(5, 410);
            this.pic_right.MouseInColor = System.Drawing.Color.Peru;
            this.pic_right.Name = "pic_right";
            this.pic_right.Size = new System.Drawing.Size(120, 160);
            this.pic_right.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pic_right.TabIndex = 26;
            this.pic_right.TabStop = false;
            this.pic_right.Tag = "003";
            // 
            // pic_left
            // 
            this.pic_left.BorderStyle = System.Windows.Forms.Border3DStyle.Flat;
            this.pic_left.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pic_left.DisplayText = "点击拍照";
            this.pic_left.FileName = "D:\\Work\\Ident\\Code\\CIdent\\CIdent\\bin\\Debug\\right.bmp";
            this.pic_left.ImageFromBase64 = null;
            this.pic_left.ImageStream = null;
            this.pic_left.Location = new System.Drawing.Point(5, 209);
            this.pic_left.MouseInColor = System.Drawing.Color.Peru;
            this.pic_left.Name = "pic_left";
            this.pic_left.Size = new System.Drawing.Size(120, 160);
            this.pic_left.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pic_left.TabIndex = 25;
            this.pic_left.TabStop = false;
            this.pic_left.Tag = "002";
            // 
            // pic_positive
            // 
            this.pic_positive.BorderStyle = System.Windows.Forms.Border3DStyle.Flat;
            this.pic_positive.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pic_positive.DisplayText = "点击拍照";
            this.pic_positive.FileName = "*";
            this.pic_positive.ImageFromBase64 = null;
            this.pic_positive.ImageStream = null;
            this.pic_positive.Location = new System.Drawing.Point(5, 12);
            this.pic_positive.MouseInColor = System.Drawing.Color.Peru;
            this.pic_positive.Name = "pic_positive";
            this.pic_positive.Size = new System.Drawing.Size(120, 160);
            this.pic_positive.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pic_positive.TabIndex = 24;
            this.pic_positive.TabStop = false;
            this.pic_positive.Tag = "001";
            // 
            // btnGetPhoto
            // 
            this.btnGetPhoto.Location = new System.Drawing.Point(23, 456);
            this.btnGetPhoto.Name = "btnGetPhoto";
            this.btnGetPhoto.Size = new System.Drawing.Size(79, 30);
            this.btnGetPhoto.TabIndex = 23;
            this.btnGetPhoto.Text = "采集照片";
            this.btnGetPhoto.UseVisualStyleBackColor = true;
            this.btnGetPhoto.Visible = false;
            // 
            // btnInitDevice
            // 
            this.btnInitDevice.Location = new System.Drawing.Point(14, 456);
            this.btnInitDevice.Name = "btnInitDevice";
            this.btnInitDevice.Size = new System.Drawing.Size(79, 30);
            this.btnInitDevice.TabIndex = 22;
            this.btnInitDevice.Text = "初始化设备";
            this.btnInitDevice.UseVisualStyleBackColor = true;
            this.btnInitDevice.Visible = false;
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
            // frmIndustrialCamera
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(953, 650);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.pic_Image);
            this.Name = "frmIndustrialCamera";
            this.Text = "frmIndustrialCamera";
            ((System.ComponentModel.ISupportInitialize)(this.pic_Image)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pic_right)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_left)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_positive)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private CIdent.Controls.CropPicture pic_Image;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btn_right;
        private System.Windows.Forms.Button btn_left;
        private System.Windows.Forms.Button btn_positive;
        private CIdentCommon.Controls.CPictureBox pic_right;
        private CIdentCommon.Controls.CPictureBox pic_left;
        private CIdentCommon.Controls.CPictureBox pic_positive;
        private System.Windows.Forms.Button btnGetPhoto;
        private System.Windows.Forms.Button btnInitDevice;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timer2;
    }
}