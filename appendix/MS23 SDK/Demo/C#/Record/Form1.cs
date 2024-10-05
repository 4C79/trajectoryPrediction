using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MVSDK;//使用SDK接口
using CameraHandle = System.Int32;
using MvApi = MVSDK.MvApi;
using System.IO;

namespace Record
{
    public partial class Form1 : Form
    {
        #region variable
        protected IntPtr m_Grabber = IntPtr.Zero;
        protected CameraHandle m_hCamera = 0;
        protected tSdkCameraDevInfo m_DevInfo;
        protected pfnCameraGrabberFrameCallback m_FrameCallback;
        protected Boolean m_bRecording = false;
        #endregion

        public Form1()
        {
            InitializeComponent();
            textBoxRecordPath.Text = "test.mp4";

            m_FrameCallback = new pfnCameraGrabberFrameCallback(CameraGrabberFrameCallback);

            tSdkCameraDevInfo[] DevList;
            MvApi.CameraEnumerateDevice(out DevList);
            int NumDev = (DevList != null ? DevList.Length : 0);

            if (NumDev < 1)
            {
                MessageBox.Show("未扫描到相机");
            }
            else if (NumDev == 1)
            {
                MvApi.CameraGrabber_Create(out m_Grabber, ref DevList[0]);
            }
            else
            {
                MvApi.CameraGrabber_CreateFromDevicePage(out m_Grabber);
            }

            if (m_Grabber != IntPtr.Zero)
            {
                MvApi.CameraGrabber_GetCameraDevInfo(m_Grabber, out m_DevInfo);
                MvApi.CameraGrabber_GetCameraHandle(m_Grabber, out m_hCamera);
                MvApi.CameraCreateSettingPage(m_hCamera, this.Handle, m_DevInfo.acFriendlyName, null, (IntPtr)0, 0);

                MvApi.CameraGrabber_SetRGBCallback(m_Grabber, m_FrameCallback, IntPtr.Zero);

                MvApi.CameraGrabber_SetHWnd(m_Grabber, this.DispWnd.Handle);
                MvApi.CameraGrabber_StartLive(m_Grabber);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_bRecording)
            {
                MvApi.CameraStopRecord(m_hCamera);
            }
            MvApi.CameraGrabber_Destroy(m_Grabber);
        }

        private void buttonSettings_Click(object sender, EventArgs e)
        {
            if (m_Grabber != IntPtr.Zero)
                MvApi.CameraShowSettingPage(m_hCamera, 1);
        }

        private void buttonPlay_Click(object sender, EventArgs e)
        {
            if (m_Grabber != IntPtr.Zero)
                MvApi.CameraGrabber_StartLive(m_Grabber);
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            if (m_Grabber != IntPtr.Zero)
                MvApi.CameraGrabber_StopLive(m_Grabber);
        }

        private void buttonBeginRecord_Click(object sender, EventArgs e)
        {
            if (m_Grabber == IntPtr.Zero)
                return;

            if (m_bRecording)
            {
                m_bRecording = false;

                if (MvApi.CameraStopRecord(m_hCamera) == 0)
                    MessageBox.Show("录像保存成功");
                else
                    MessageBox.Show("录像保存失败");

                this.buttonBeginRecord.Text = "开始录像";
                this.Text = "录像";
            }
            else
            {
                String SavePath = this.textBoxRecordPath.Text;
                if (SavePath.Length < 1)
                {
                    MessageBox.Show("请先选择录像文件的保存路径");
                    return;
                }

                // 4：H264    1: MSVC    0: 无压缩
                int iFormat = 0;
                if (radioCodecH264.Checked)
                    iFormat = 4;
                else if (radioCodecMSVC.Checked)
                    iFormat = 1;

                UInt32 Quality = Convert.ToUInt32(numericQuality.Value);
                CameraSdkStatus status = MvApi.CameraInitRecord(m_hCamera, iFormat, SavePath, 0, Quality, 30);
                if (status != 0)
                {
                    MessageBox.Show("启动录像失败");
                    return;
                }

                m_bRecording = true;
                this.buttonBeginRecord.Text = "停止录像";
                this.Text = "录像（录像中。。。）";
            }
        }

        private void buttonSelRecordPath_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = System.IO.Directory.GetCurrentDirectory();
            sfd.Filter = "mp4|*.mp4|avi|*.avi";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                textBoxRecordPath.Text = sfd.FileName;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (m_Grabber != IntPtr.Zero)
            {
                tSdkGrabberStat stat;
                MvApi.CameraGrabber_GetStat(m_Grabber, out stat);
                string info = String.Format("| Resolution:{0}*{1} | DispFPS:{2} | CapFPS:{3} |",
                    stat.Width, stat.Height, stat.DispFps, stat.CapFps);
                StateLabel.Text = info;
            }
        }

        private void CameraGrabberFrameCallback(
            IntPtr Grabber,
            IntPtr pFrameBuffer,
            ref tSdkFrameHead pFrameHead,
            IntPtr Context)
        {
            // 数据处理回调

            // 由于黑白相机在相机打开后设置了ISP输出灰度图像
            // 因此此处pFrameBuffer=8位灰度数据
            // 否则会和彩色相机一样输出BGR24数据

            // 彩色相机ISP默认会输出BGR24图像
            // pFrameBuffer=BGR24数据
            if (m_bRecording)
            {
                MvApi.CameraPushFrame(m_hCamera, pFrameBuffer, ref pFrameHead);
            }
        }
    }
}
