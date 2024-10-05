//#define USE_CALLBACK  //图像抓取方式宏定义，打开则采用回调函数方式，关闭，则使用多线程主动抓取方式。

//BIG5 TRANS ALLOWED

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using MVSDK;//使用SDK接口
using Snapshot; 
using CameraHandle = System.Int32;
using MvApi = MVSDK.MvApi;
using System.IO;



namespace Basic
{
    

    public partial class BasicForm : Form
    {
        

        #region variable

        //相机1
        protected CameraHandle m_hCamera1 = 0;             // 句柄
        protected IntPtr       m_ImageBuffer1;             // 预览通道RGB图像缓存
        protected tSdkCameraCapbility tCameraCapability1;  // 相机特性描述
        protected int          m_iDisplayedFrames = 0;    //已经显示的总帧数
        protected IntPtr       m_iCaptureCallbackCtx1;     //图像回调函数的上下文参数
        protected Thread       m_tCaptureThread1;          //图像抓取线程
        protected bool         m_bExitCaptureThread1 = false;//采用线程采集时，让线程退出的标志
        protected tSdkFrameHead m_tFrameHead1;
        protected bool          m_bEraseBk = false;

        //相机2
        protected CameraHandle m_hCamera2 = 0;             // 句柄
        protected IntPtr m_ImageBuffer2;             // 预览通道RGB图像缓存
        protected tSdkCameraCapbility tCameraCapability2;  // 相机特性描述
        protected IntPtr m_iCaptureCallbackCtx2;     //图像回调函数的上下文参数
        protected Thread m_tCaptureThread2;          //图像抓取线程
        protected bool m_bExitCaptureThread2 = false;//采用线程采集时，让线程退出的标志
        protected tSdkFrameHead m_tFrameHead2;
      
        #endregion

        public BasicForm()
        {
            InitializeComponent();

            InitCamera();
        }

        public void CaptureThreadProc1()
        {
            CameraSdkStatus eStatus;
            tSdkFrameHead FrameHead;
            IntPtr uRawBuffer;//rawbuffer由SDK内部申请。应用层不要调用delete之类的释放函数
  
            while(m_bExitCaptureThread1 == false)
            {
                //500毫秒超时,图像没捕获到前，线程会被挂起,释放CPU，所以该线程中无需调用sleep
                eStatus = MvApi.CameraGetImageBuffer(m_hCamera1, out FrameHead, out uRawBuffer, 500);
                
                if (eStatus == CameraSdkStatus.CAMERA_STATUS_SUCCESS)//如果是触发模式，则有可能超时
                {
                    //图像处理，将原始输出转换为RGB格式的位图数据，同时叠加白平衡、饱和度、LUT等ISP处理。
                    MvApi.CameraImageProcess(m_hCamera1, uRawBuffer, m_ImageBuffer1, ref FrameHead);
                    //叠加十字线、自动曝光窗口、白平衡窗口信息(仅叠加设置为可见状态的)。    
                    MvApi.CameraImageOverlay(m_hCamera1, m_ImageBuffer1, ref FrameHead);
                    //调用SDK封装好的接口，显示预览图像
                    MvApi.CameraDisplayRGB24(m_hCamera1, m_ImageBuffer1, ref FrameHead);
                    //成功调用CameraGetImageBuffer后必须释放，下次才能继续调用CameraGetImageBuffer捕获图像。
                    MvApi.CameraReleaseImageBuffer(m_hCamera1,uRawBuffer);

                    if (FrameHead.iWidth != m_tFrameHead1.iWidth || FrameHead.iHeight != m_tFrameHead1.iHeight)
                    {
                        m_bEraseBk = true;
                        m_tFrameHead1 = FrameHead;  
                    }
                    m_iDisplayedFrames++;
                }
           
            }
           
        }

        public void CaptureThreadProc2()
        {
            CameraSdkStatus eStatus;
            tSdkFrameHead FrameHead;
            IntPtr uRawBuffer;//rawbuffer由SDK内部申请。应用层不要调用delete之类的释放函数

            while (m_bExitCaptureThread2 == false)
            {
                //500毫秒超时,图像没捕获到前，线程会被挂起,释放CPU，所以该线程中无需调用sleep
                eStatus = MvApi.CameraGetImageBuffer(m_hCamera2, out FrameHead, out uRawBuffer, 500);

                if (eStatus == CameraSdkStatus.CAMERA_STATUS_SUCCESS)//如果是触发模式，则有可能超时
                {
                    //图像处理，将原始输出转换为RGB格式的位图数据，同时叠加白平衡、饱和度、LUT等ISP处理。
                    MvApi.CameraImageProcess(m_hCamera2, uRawBuffer, m_ImageBuffer2, ref FrameHead);
                    //叠加十字线、自动曝光窗口、白平衡窗口信息(仅叠加设置为可见状态的)。    
                    MvApi.CameraImageOverlay(m_hCamera2, m_ImageBuffer2, ref FrameHead);
                    //调用SDK封装好的接口，显示预览图像
                    MvApi.CameraDisplayRGB24(m_hCamera2, m_ImageBuffer2, ref FrameHead);
                    //成功调用CameraGetImageBuffer后必须释放，下次才能继续调用CameraGetImageBuffer捕获图像。
                    MvApi.CameraReleaseImageBuffer(m_hCamera2, uRawBuffer);

                    if (FrameHead.iWidth != m_tFrameHead2.iWidth || FrameHead.iHeight != m_tFrameHead2.iHeight)
                    {
                        m_bEraseBk = true;
                        m_tFrameHead2 = FrameHead;
                    }
      
                }

            }

        }

        private bool InitCamera()
        {
            CameraSdkStatus status;
            tSdkCameraDevInfo[] tCameraDevInfoList;

            status = MvApi.CameraEnumerateDevice(out tCameraDevInfoList);
            int iCameraCounts = (tCameraDevInfoList != null ? tCameraDevInfoList.Length : 0);
            if (iCameraCounts >= 2)//此时iCameraCounts返回了实际连接的相机个数。如果大于2，则初始化前2个相机
            {
                if (MvApi.CameraInit(ref tCameraDevInfoList[0], -1, -1, ref m_hCamera1) == CameraSdkStatus.CAMERA_STATUS_SUCCESS)
                {
                    //获得相机特性描述
                    MvApi.CameraGetCapability(m_hCamera1, out tCameraCapability1);

                    m_ImageBuffer1 = Marshal.AllocHGlobal(tCameraCapability1.sResolutionRange.iWidthMax * tCameraCapability1.sResolutionRange.iHeightMax * 3 + 1024);

                    if (tCameraCapability1.sIspCapacity.bMonoSensor != 0)
                    {
                        // 黑白相机输出8位灰度数据
                        MvApi.CameraSetIspOutFormat(m_hCamera1, (uint)MVSDK.emImageFormat.CAMERA_MEDIA_TYPE_MONO8);
                    }

                    //初始化显示模块，使用SDK内部封装好的显示接口
                    MvApi.CameraDisplayInit(m_hCamera1, PreviewBox.Handle);
                    MvApi.CameraSetDisplaySize(m_hCamera1, PreviewBox.Width, PreviewBox.Height);

                    //让SDK来根据相机的型号动态创建该相机的配置窗口。
                    MvApi.CameraCreateSettingPage(m_hCamera1, this.Handle, tCameraDevInfoList[0].acFriendlyName,/*SettingPageMsgCalBack*/null,/*m_iSettingPageMsgCallbackCtx*/(IntPtr)null, 0);

                    MvApi.CameraPlay(m_hCamera1);
                    BtnPlay.Text = "Pause";

                    //两种方式来获得预览图像，设置回调函数或者使用定时器或者独立线程的方式，
                    //主动调用CameraGetImageBuffer接口来抓图。
                    //本例中仅演示了两种的方式,注意，两种方式也可以同时使用，但是在回调函数中，
                    //不要使用CameraGetImageBuffer，否则会造成死锁现象。

                    m_bExitCaptureThread1 = false;
                    m_tCaptureThread1 = new Thread(new ThreadStart(CaptureThreadProc1));
                    m_tCaptureThread1.Start();

                }
                else
                {
                    m_hCamera1 = 0;
                    StateLabel.Text = "Camera 1 init error";
                    return false;
                }

                if (MvApi.CameraInit(ref tCameraDevInfoList[1], -1, -1, ref m_hCamera2) == CameraSdkStatus.CAMERA_STATUS_SUCCESS)
                {
                    //获得相机特性描述
                    MvApi.CameraGetCapability(m_hCamera2, out tCameraCapability2);

                    m_ImageBuffer2 = Marshal.AllocHGlobal(tCameraCapability2.sResolutionRange.iWidthMax * tCameraCapability2.sResolutionRange.iHeightMax * 3 + 1024);

                    if (tCameraCapability2.sIspCapacity.bMonoSensor != 0)
                    {
                        // 黑白相机输出8位灰度数据
                        MvApi.CameraSetIspOutFormat(m_hCamera2, (uint)MVSDK.emImageFormat.CAMERA_MEDIA_TYPE_MONO8);
                    }

                    //初始化显示模块，使用SDK内部封装好的显示接口
                    MvApi.CameraDisplayInit(m_hCamera2, PreviewBox2.Handle);
                    MvApi.CameraSetDisplaySize(m_hCamera2, PreviewBox2.Width, PreviewBox2.Height);

                    //让SDK来根据相机的型号动态创建该相机的配置窗口。
                    MvApi.CameraCreateSettingPage(m_hCamera2, this.Handle, tCameraDevInfoList[1].acFriendlyName,/*SettingPageMsgCalBack*/null,/*m_iSettingPageMsgCallbackCtx*/(IntPtr)null, 0);

                    MvApi.CameraPlay(m_hCamera2);
                    BtnPlay2.Text = "Pause";

                    m_bExitCaptureThread2 = false;
                    m_tCaptureThread2 = new Thread(new ThreadStart(CaptureThreadProc2));
                    m_tCaptureThread2.Start();
                }
                else
                {
                    m_hCamera2 = 0;
                    StateLabel.Text = "Camera 2 init error";
                    return false;
                }

                return true;
            }
            return false;
        }

        private void BtnPlay_Click(object sender, EventArgs e)
        {
            if (m_hCamera1 != 0)
            {
                if (BtnPlay.Text == "Play")
                {
                    MvApi.CameraPlay(m_hCamera1);
                    BtnPlay.Text = "Pause";
                }
                else
                {
                    MvApi.CameraPause(m_hCamera1);
                    BtnPlay.Text = "Play";
                }
            }
        }

        private void BasicForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //相机1反初始化
            if (m_tCaptureThread1 != null)
            {
                m_bExitCaptureThread1 = true;
                m_tCaptureThread1.Join();
                m_tCaptureThread1 = null;
            }

            if (m_hCamera1 != 0)
            {
                MvApi.CameraUnInit(m_hCamera1);
                m_hCamera1 = 0;
            }

            if (m_ImageBuffer1 != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(m_ImageBuffer1);
                m_ImageBuffer1 = IntPtr.Zero;
            }

            //相机2反初始化
            if (m_tCaptureThread2 != null)
            {
                m_bExitCaptureThread2 = true;
                m_tCaptureThread2.Join();
                m_tCaptureThread2 = null;
            }

            if (m_hCamera2 != 0)
            {
                MvApi.CameraUnInit(m_hCamera2);
                m_hCamera2 = 0;
            }

            if (m_ImageBuffer2 != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(m_ImageBuffer2);
                m_ImageBuffer2 = IntPtr.Zero;
            } 
        }

        private void BtnSettings_Click(object sender, EventArgs e)
        {
            if (m_hCamera1 > 0)
            {
                MvApi.CameraShowSettingPage(m_hCamera1, 1);//1 show ; 0 hide
            }
        }

        //1秒更新一次视频信息
        private void timer1_Tick(object sender, EventArgs e)
        {
            tSdkFrameStatistic tFrameStatistic;
            if (m_hCamera1 > 0)
            {
                //获得SDK中图像帧统计信息，捕获帧、错误帧等。
                MvApi.CameraGetFrameStatistic(m_hCamera1, out tFrameStatistic);
                //显示帧率有应用程序自己记录。
                string sFrameInfomation = String.Format("| Resolution:{0}*{1} | Display frames{2} | Capture frames{3} |", m_tFrameHead1.iWidth, m_tFrameHead1.iHeight, m_iDisplayedFrames, tFrameStatistic.iCapture);
                StateLabel.Text = sFrameInfomation;

                sFrameInfomation = String.Format("| Resolution:{0}*{1} |", m_tFrameHead2.iWidth, m_tFrameHead2.iHeight);
                StateLabel2.Text = sFrameInfomation;
                
            }
            else
            {
                StateLabel.Text = "";
            }
        }

        //用于分辨率切换时，刷新背景绘图
        private void timer2_Tick(object sender, EventArgs e)
        {
            //切换分辨率后，擦除一次背景
            if (m_bEraseBk == true)
            {
                m_bEraseBk = false;
                PreviewBox.Refresh();
            }
        }

        private void BasicForm_Load(object sender, EventArgs e)
        {

        }

        private void BtnSettings2_Click(object sender, EventArgs e)
        {
            if (m_hCamera2 > 0)
            {
                MvApi.CameraShowSettingPage(m_hCamera2, 1);//1 show ; 0 hide
            }
        }

        private void BtnPlay2_Click(object sender, EventArgs e)
        {
            if (m_hCamera2 > 0)
            {
                if (BtnPlay2.Text == "Play")
                {
                    MvApi.CameraPlay(m_hCamera2);
                    BtnPlay2.Text = "Pause";
                }
                else
                {
                    MvApi.CameraPause(m_hCamera2);
                    BtnPlay2.Text = "Play";
                }
            }
        }

    }
}