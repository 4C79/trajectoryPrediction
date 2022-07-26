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
using HalconDotNet;

namespace Halcon
{
    public partial class Form1 : Form
    {
        #region variable
        protected IntPtr m_Grabber = IntPtr.Zero;
        protected CameraHandle m_hCamera = 0;
        protected tSdkCameraDevInfo m_DevInfo;
        protected pfnCameraGrabberFrameCallback m_FrameCallback;
        protected pfnCameraGrabberSaveImageComplete m_SaveImageComplete;
        protected HDevelopExport m_HDevelopExport;
        #endregion

        public Form1()
        {
            InitializeComponent();

            m_HDevelopExport = new HDevelopExport();
            m_HDevelopExport.InitHalcon(hWindowControl1.HalconWindow);

            m_FrameCallback = new pfnCameraGrabberFrameCallback(CameraGrabberFrameCallback);
            m_SaveImageComplete = new pfnCameraGrabberSaveImageComplete(CameraGrabberSaveImageComplete);

            if (MvApi.CameraGrabber_CreateFromDevicePage(out m_Grabber) == CameraSdkStatus.CAMERA_STATUS_SUCCESS)
            {
                MvApi.CameraGrabber_GetCameraDevInfo(m_Grabber, out m_DevInfo);
                MvApi.CameraGrabber_GetCameraHandle(m_Grabber, out m_hCamera);
                MvApi.CameraCreateSettingPage(m_hCamera, this.Handle, m_DevInfo.acFriendlyName, null, (IntPtr)0, 0);

                MvApi.CameraGrabber_SetRGBCallback(m_Grabber, m_FrameCallback, IntPtr.Zero);
                MvApi.CameraGrabber_SetSaveImageCompleteCallback(m_Grabber, m_SaveImageComplete, IntPtr.Zero);

                // 黑白相机设置ISP输出灰度图像
                // 彩色相机ISP默认会输出BGR24图像
                tSdkCameraCapbility cap;
                MvApi.CameraGetCapability(m_hCamera, out cap);
                if (cap.sIspCapacity.bMonoSensor != 0)
                {
                    MvApi.CameraSetIspOutFormat(m_hCamera, (uint)MVSDK.emImageFormat.CAMERA_MEDIA_TYPE_MONO8);
                }

                MvApi.CameraGrabber_SetHWnd(m_Grabber, this.DispWnd.Handle);
                MvApi.CameraGrabber_StartLive(m_Grabber);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
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

        private void buttonSnap_Click(object sender, EventArgs e)
        {
            if (m_Grabber != IntPtr.Zero)
                MvApi.CameraGrabber_SaveImageAsync(m_Grabber);
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
            int w = pFrameHead.iWidth;
            int h = pFrameHead.iHeight;

            HObject Image = null;
            try
            {
                if (pFrameHead.uiMediaType == (uint)MVSDK.emImageFormat.CAMERA_MEDIA_TYPE_MONO8)
                {
                    HOperatorSet.GenImage1(out Image, "byte", w, h, pFrameBuffer);
                }
                else if (pFrameHead.uiMediaType == (uint)MVSDK.emImageFormat.CAMERA_MEDIA_TYPE_BGR8)
                {
                    HOperatorSet.GenImageInterleaved(out Image,
                        pFrameBuffer,
                        "bgr",
                        w, h,
                        -1, "byte",
                        w, h,
                        0, 0, -1, 0);
                }
                else
                {
                    throw new HalconException("Image format is not supported!!");
                }

                HObject ImageRaw = Image;
                HOperatorSet.MirrorImage(ImageRaw, out Image, "row");
                ImageRaw.Dispose();

                m_HDevelopExport.action(Image);
            }
            catch (HalconException Exc)
            {
            }
            finally
            {
                if (Image != null)
                    Image.Dispose();
            }
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
    }


    // halcon 导出类
    public partial class HDevelopExport
    {
        public HTuple hv_ExpDefaultWinHandle;

        // Main procedure 
        public void action(HObject ho_Image)
        {
            HObject ho_Region, ho_ConnectedRegions;
            HObject ho_Contours;

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_Contours);

            HTuple Pointer, type, Width, Height;
            HOperatorSet.GetImagePointer1(ho_Image, out Pointer, out type, out Width, out Height); 

            ho_Region.Dispose();
            HOperatorSet.Threshold(ho_Image, out ho_Region, 128, 255);

            ho_ConnectedRegions.Dispose();
            HOperatorSet.Connection(ho_Region, out ho_ConnectedRegions); 

            ho_Contours.Dispose();
            HOperatorSet.GenContourRegionXld(ho_ConnectedRegions, out ho_Contours, "border");

            HOperatorSet.ClearWindow(hv_ExpDefaultWinHandle);
            HOperatorSet.SetPart(hv_ExpDefaultWinHandle, 0, 0, Height, Width);
            HOperatorSet.DispObj(ho_Contours, hv_ExpDefaultWinHandle);

            ho_Region.Dispose();
            ho_ConnectedRegions.Dispose();
            ho_Contours.Dispose(); 
        }

        public void InitHalcon(HTuple Window)
        {
            // Default settings used in HDevelop 
            HOperatorSet.SetSystem("do_low_error", "false");
            hv_ExpDefaultWinHandle = Window;
        }
    }
}
