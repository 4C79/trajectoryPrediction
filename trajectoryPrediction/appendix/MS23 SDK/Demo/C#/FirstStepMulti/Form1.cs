using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using MVSDK;//使用SDK接口
using CameraHandle = System.Int32;
using MvApi = MVSDK.MvApi;
using System.IO;

namespace FirstStepMulti
{
    public partial class Form1 : Form
    {
        #region variable
        protected IntPtr[] m_Grabber = new IntPtr[4];
        protected CameraHandle[] m_hCamera = new CameraHandle[4];
        protected tSdkCameraDevInfo[] m_DevInfo;
        protected pfnCameraGrabberFrameCallback m_FrameCallback;
        protected pfnCameraGrabberSaveImageComplete m_SaveImageComplete;
        #endregion

        public Form1()
        {
            InitializeComponent();

            m_FrameCallback = new pfnCameraGrabberFrameCallback(CameraGrabberFrameCallback);
            m_SaveImageComplete = new pfnCameraGrabberSaveImageComplete(CameraGrabberSaveImageComplete);
 
            MvApi.CameraEnumerateDevice(out m_DevInfo);
            int NumDev = (m_DevInfo != null ? Math.Min(m_DevInfo.Length, 4) : 0);

            IntPtr[] hDispWnds = { this.DispWnd1.Handle, this.DispWnd2.Handle, this.DispWnd3.Handle, this.DispWnd4.Handle };
            for (int i = 0; i < NumDev; ++i)
            {
                if (MvApi.CameraGrabber_Create(out m_Grabber[i], ref m_DevInfo[i]) == CameraSdkStatus.CAMERA_STATUS_SUCCESS)
                {
                    MvApi.CameraGrabber_GetCameraHandle(m_Grabber[i], out m_hCamera[i]);
                    MvApi.CameraCreateSettingPage(m_hCamera[i], this.Handle, m_DevInfo[i].acFriendlyName, null, (IntPtr)0, 0);

                    MvApi.CameraGrabber_SetRGBCallback(m_Grabber[i], m_FrameCallback, IntPtr.Zero);
                    MvApi.CameraGrabber_SetSaveImageCompleteCallback(m_Grabber[i], m_SaveImageComplete, IntPtr.Zero);

                    // 黑白相机设置ISP输出灰度图像
                    // 彩色相机ISP默认会输出BGR24图像
                    tSdkCameraCapbility cap;
                    MvApi.CameraGetCapability(m_hCamera[i], out cap);
                    if (cap.sIspCapacity.bMonoSensor != 0)
                        MvApi.CameraSetIspOutFormat(m_hCamera[i], (uint)MVSDK.emImageFormat.CAMERA_MEDIA_TYPE_MONO8);

                    MvApi.CameraGrabber_SetHWnd(m_Grabber[i], hDispWnds[i]);
                }
            }
            for (int i = 0; i < NumDev; ++i)
            {
                if (m_Grabber[i] != IntPtr.Zero)
                    MvApi.CameraGrabber_StartLive(m_Grabber[i]);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            for (int i = 0; i < 4; ++i)
            {
                if (m_Grabber[i] != IntPtr.Zero)
                    MvApi.CameraGrabber_Destroy(m_Grabber[i]);
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
        }

        private void CameraGrabberSaveImageComplete(
            IntPtr Grabber,
            IntPtr Image,	// 需要调用CameraImage_Destroy释放
            CameraSdkStatus Status,
            IntPtr Context)
        {
            if (Image != IntPtr.Zero)
            {
                string filename = System.IO.Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory.ToString(),
                        string.Format("{0}.bmp", System.Environment.TickCount));

                MvApi.CameraImage_SaveAsBmp(Image, filename);

                MessageBox.Show(filename);
            }

            MvApi.CameraImage_Destroy(Image);
        }

        private void buttonSettings1_Click(object sender, EventArgs e)
        {
            if (m_Grabber[0] != IntPtr.Zero)
                MvApi.CameraShowSettingPage(m_hCamera[0], 1);
        }

        private void buttonPlay1_Click(object sender, EventArgs e)
        {
            if (m_Grabber[0] != IntPtr.Zero)
                MvApi.CameraGrabber_StartLive(m_Grabber[0]);
        }

        private void buttonStop1_Click(object sender, EventArgs e)
        {
            if (m_Grabber[0] != IntPtr.Zero)
                MvApi.CameraGrabber_StopLive(m_Grabber[0]);
        }

        private void buttonSnap1_Click(object sender, EventArgs e)
        {
            if (m_Grabber[0] != IntPtr.Zero)
                MvApi.CameraGrabber_SaveImageAsync(m_Grabber[0]);
        }

        private void buttonSettings2_Click(object sender, EventArgs e)
        {
            if (m_Grabber[1] != IntPtr.Zero)
                MvApi.CameraShowSettingPage(m_hCamera[1], 1);
        }

        private void buttonPlay2_Click(object sender, EventArgs e)
        {
            if (m_Grabber[1] != IntPtr.Zero)
                MvApi.CameraGrabber_StartLive(m_Grabber[1]);
        }

        private void buttonStop2_Click(object sender, EventArgs e)
        {
            if (m_Grabber[1] != IntPtr.Zero)
                MvApi.CameraGrabber_StopLive(m_Grabber[1]);
        }

        private void buttonSnap2_Click(object sender, EventArgs e)
        {
            if (m_Grabber[1] != IntPtr.Zero)
                MvApi.CameraGrabber_SaveImageAsync(m_Grabber[1]);
        }

        private void buttonSettings3_Click(object sender, EventArgs e)
        {
            if (m_Grabber[2] != IntPtr.Zero)
                MvApi.CameraShowSettingPage(m_hCamera[2], 1);
        }

        private void buttonPlay3_Click(object sender, EventArgs e)
        {
            if (m_Grabber[2] != IntPtr.Zero)
                MvApi.CameraGrabber_StartLive(m_Grabber[2]);
        }

        private void buttonStop3_Click(object sender, EventArgs e)
        {
            if (m_Grabber[2] != IntPtr.Zero)
                MvApi.CameraGrabber_StopLive(m_Grabber[2]);
        }

        private void buttonSnap3_Click(object sender, EventArgs e)
        {
            if (m_Grabber[2] != IntPtr.Zero)
                MvApi.CameraGrabber_SaveImageAsync(m_Grabber[2]);
        }

        private void buttonSettings4_Click(object sender, EventArgs e)
        {
            if (m_Grabber[3] != IntPtr.Zero)
                MvApi.CameraShowSettingPage(m_hCamera[3], 1);
        }

        private void buttonPlay4_Click(object sender, EventArgs e)
        {
            if (m_Grabber[3] != IntPtr.Zero)
                MvApi.CameraGrabber_StartLive(m_Grabber[3]);
        }

        private void buttonStop4_Click(object sender, EventArgs e)
        {
            if (m_Grabber[3] != IntPtr.Zero)
                MvApi.CameraGrabber_StopLive(m_Grabber[3]);
        }

        private void buttonSnap4_Click(object sender, EventArgs e)
        {
            if (m_Grabber[3] != IntPtr.Zero)
                MvApi.CameraGrabber_SaveImageAsync(m_Grabber[3]);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Label[] Labels = { label1, label2, label3, label4 };
            for (int i = 0; i < 4; ++i)
            {
                if (m_Grabber[i] != IntPtr.Zero)
                {
                    tSdkGrabberStat stat;
                    MvApi.CameraGrabber_GetStat(m_Grabber[i], out stat);
                    string info = String.Format("| Size:{0}*{1} | DispFPS:{2} | CapFPS:{3} |",
                        stat.Width, stat.Height, stat.DispFps, stat.CapFps);
                    Labels[i].Text = info;
                }
            }
        }
    }
}
