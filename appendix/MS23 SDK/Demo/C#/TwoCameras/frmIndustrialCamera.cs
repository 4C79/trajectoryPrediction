using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using CameraHandle = System.Int32;
using CIdentCommon.Classes;
using System.Threading;
using System.Runtime.InteropServices;
using MvApi = MVSDK.MvApi;
using MVSDK;


namespace CIdent.Forms
{
    public partial class frmIndustrialCamera : Form
    {
        #region variable
        protected CameraHandle m_hCamera = 0;             // 句柄
        protected IntPtr m_ImageBuffer;             // 预览通道RGB图像缓存
        protected IntPtr m_ImageBufferSnapshot;     // 抓拍通道RGB图像缓存
        protected tSdkCameraCapbility tCameraCapability;  // 相机特性描述
        protected int m_iDisplayedFrames = 0;    //已经显示的总帧数
        protected IntPtr m_iCaptureCallbackCtx;     //图像回调函数的上下文参数
        protected Thread m_tCaptureThread;          //图像抓取线程
        protected bool m_bExitCaptureThread = false;//采用线程采集时，让线程退出的标志
        protected IntPtr m_iSettingPageMsgCallbackCtx; //相机配置界面消息回调函数的上下文参数   
        protected tSdkFrameHead m_tFrameHead;
        //protected SnapshotDlg m_DlgSnapshot = new SnapshotDlg();               //显示抓拍图像的窗口
        protected bool m_bEraseBk = false;
        #endregion

        #region 对外接口属性
        /// <summary>
        /// 正面照片
        /// </summary>
        public byte[] PositiveImageStream
        { get; set; }

        /// <summary>
        /// 左侧照片
        /// </summary>
        public byte[] LeftImageStream
        { get; set; }

        /// <summary>
        /// 右侧照片
        /// </summary>
        public byte[] RightImageStream
        { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string PersonName
        {
            get;
            set;
        }
        #endregion

        public frmIndustrialCamera()
        {
            InitializeComponent();

            this.FormClosed += new FormClosedEventHandler(frmIndustrialCamera_FormClosed);

            //初始化SDK
            MvApi.CameraSdkInit(1);//1:SDK中使用中文进行提示和创建相机配置窗口  0：英文
            // 检测是否有连接相机，如果已经连接，则直接初始化相机并开始预览
            if (InitCamera() == true)
            {
                MvApi.CameraPlay(m_hCamera);
                //BtnPlay.Text = "暂停";
            }
        }


#if USE_CALL_BACK
        public void ImageCaptureCallback(CameraHandle hCamera, uint pFrameBuffer, ref tSdkFrameHead pFrameHead, uint pContext)
        {
            //图像处理，将原始输出转换为RGB格式的位图数据，同时叠加白平衡、饱和度、LUT等ISP处理。
            MvApi.CameraImageProcess(hCamera, pFrameBuffer, (IntPtr)((int)m_ImageBuffer&(~0xf)), ref pFrameHead);
            //叠加十字线、自动曝光窗口、白平衡窗口信息(仅叠加设置为可见状态的)。   
            MvApi.CameraImageOverlay(hCamera, (IntPtr)((int)m_ImageBuffer & (~0xf)), ref pFrameHead);
            //调用SDK封装好的接口，显示预览图像
            MvApi.CameraDisplayRGB24(hCamera, (IntPtr)((int)m_ImageBuffer & (~0xf)), ref pFrameHead);
            m_tFrameHead = pFrameHead;
            m_iDisplayedFrames++;

            if (pFrameHead.iWidth != m_tFrameHead.iWidth || pFrameHead.iHeight != m_tFrameHead.iHeight)
            {
                timer2.Enabled = true;
                timer2.Start();
                m_tFrameHead = pFrameHead;
            }
            
        }
#else
        public void CaptureThreadProc()
        {
            CameraSdkStatus eStatus;
            tSdkFrameHead FrameHead;
            uint uRawBuffer;//rawbuffer由SDK内部申请。应用层不要调用delete之类的释放函数

            while (m_bExitCaptureThread == false)
            {
                //500毫秒超时,图像没捕获到前，线程会被挂起,释放CPU，所以该线程中无需调用sleep
                eStatus = MvApi.CameraGetImageBuffer(m_hCamera, out FrameHead, out uRawBuffer, 500);

                if (eStatus == CameraSdkStatus.CAMERA_STATUS_SUCCESS)//如果是触发模式，则有可能超时
                {
                    //图像处理，将原始输出转换为RGB格式的位图数据，同时叠加白平衡、饱和度、LUT等ISP处理。
                    MvApi.CameraImageProcess(m_hCamera, uRawBuffer, m_ImageBuffer, ref FrameHead);
                    //叠加十字线、自动曝光窗口、白平衡窗口信息(仅叠加设置为可见状态的)。    
                    MvApi.CameraImageOverlay(m_hCamera, m_ImageBuffer, ref FrameHead);
                    //调用SDK封装好的接口，显示预览图像
                    MvApi.CameraDisplayRGB24(m_hCamera, m_ImageBuffer, ref FrameHead);
                    //成功调用CameraGetImageBuffer后必须释放，下次才能继续调用CameraGetImageBuffer捕获图像。
                    MvApi.CameraReleaseImageBuffer(m_hCamera, uRawBuffer);

                    if (FrameHead.iWidth != m_tFrameHead.iWidth || FrameHead.iHeight != m_tFrameHead.iHeight)
                    {
                        m_bEraseBk = true;
                        m_tFrameHead = FrameHead;
                    }
                    m_iDisplayedFrames++;
                }

            }

        }
#endif


        private bool InitCamera()
        {
            tSdkCameraDevInfo[] tCameraDevInfoList = new tSdkCameraDevInfo[12];
            IntPtr ptr;
            int i;
#if USE_CALL_BACK
            CAMERA_SNAP_PROC pCaptureCallOld = null;
#endif
            ptr = Marshal.AllocHGlobal(Marshal.SizeOf(new tSdkCameraDevInfo()) * 12);
            int iCameraCounts = 12;//如果有多个相机时，表示最大只获取最多12个相机的信息列表。该变量必须初始化，并且大于1
            if (m_hCamera > 0)
            {
                //已经初始化过，直接返回 true

                return true;
            }
            if (MvApi.CameraEnumerateDevice(ptr, ref iCameraCounts) == CameraSdkStatus.CAMERA_STATUS_SUCCESS)
            {
                for (i = 0; i < 12; i++)
                {
                    tCameraDevInfoList[i] = (tSdkCameraDevInfo)Marshal.PtrToStructure((IntPtr)((int)ptr + i * Marshal.SizeOf(new tSdkCameraDevInfo())), typeof(tSdkCameraDevInfo));
                }
                Marshal.FreeHGlobal(ptr);

                if (iCameraCounts >= 1)//此时iCameraCounts返回了实际连接的相机个数。如果大于1，则初始化第一个相机
                {
                    if (MvApi.CameraInit(ref tCameraDevInfoList[0], -1, -1, ref m_hCamera) == CameraSdkStatus.CAMERA_STATUS_SUCCESS)
                    {
                        //获得相机特性描述
                        ptr = Marshal.AllocHGlobal(Marshal.SizeOf(new tSdkCameraCapbility()));
                        MvApi.CameraGetCapability(m_hCamera, ptr);
                        tCameraCapability = (tSdkCameraCapbility)Marshal.PtrToStructure(ptr, typeof(tSdkCameraCapbility));
                        Marshal.FreeHGlobal(ptr);
                        Marshal.FreeHGlobal(m_ImageBuffer);
                        m_ImageBuffer = Marshal.AllocHGlobal(tCameraCapability.sResolutionRange.iWidthMax * tCameraCapability.sResolutionRange.iHeightMax * 3 + 1024);
                        m_ImageBufferSnapshot = Marshal.AllocHGlobal(tCameraCapability.sResolutionRange.iWidthMax * tCameraCapability.sResolutionRange.iHeightMax * 3 + 1024);

                        //初始化显示模块，使用SDK内部封装好的显示接口
                        MvApi.CameraDisplayInit(m_hCamera, pic_Image.Handle);
                        MvApi.CameraSetDisplaySize(m_hCamera, pic_Image.Width, pic_Image.Height);

                        //设置抓拍通道的分辨率。
                        tSdkImageResolution tResolution;
                        tResolution.fZoomScale = 1.0F;
                        tResolution.iVOffset = 0;
                        tResolution.iHOffset = 0;
                        tResolution.uBinMode = 0;
                        tResolution.uSkipMode = 0;
                        tResolution.iWidth = tCameraCapability.sResolutionRange.iWidthMax;
                        tResolution.iHeight = tCameraCapability.sResolutionRange.iHeightMax;
                        //tResolution.iIndex = 0xff;表示自定义分辨率,如果tResolution.iWidth和tResolution.iHeight
                        //定义为0，则表示跟随预览通道的分辨率进行抓拍。抓拍通道的分辨率可以动态更改。
                        //本例中将抓拍分辨率固定为最大分辨率。
                        tResolution.iIndex = 0xff;
                        tResolution.acDescription = new byte[32];//描述信息可以不设置
                        MvApi.CameraSetResolutionForSnap(m_hCamera, ref tResolution);

                        //让SDK来根据相机的型号动态创建该相机的配置窗口。
                        MvApi.CameraCreateSettingPage(m_hCamera, this.Handle, tCameraDevInfoList[0].acFriendlyName,/*SettingPageMsgCalBack*/null,/*m_iSettingPageMsgCallbackCtx*/(IntPtr)null, 0);

                        //两种方式来获得预览图像，设置回调函数或者使用定时器或者独立线程的方式，
                        //主动调用CameraGetImageBuffer接口来抓图。
                        //本例中仅演示了两种的方式,注意，两种方式也可以同时使用，但是在回调函数中，
                        //不要使用CameraGetImageBuffer，否则会造成死锁现象。
#if USE_CALL_BACK
                        MvApi.CameraSetCallbackFunction(m_hCamera, ImageCaptureCallback, m_iCaptureCallbackCtx, ref pCaptureCallOld);
#else //如果需要采用多线程，使用下面的方式
                        m_bExitCaptureThread = false;
                        m_tCaptureThread = new Thread(new ThreadStart(CaptureThreadProc));
                        m_tCaptureThread.Start();

#endif
                        return true;

                    }
                    else
                    {
                        m_hCamera = 0;
                        //StateLabel.Text = "相机初始化失败";
                        return false;
                    }


                }
            }

            return false;

        }


        void frmIndustrialCamera_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (m_hCamera > 0)
            {
            #if !USE_CALL_BACK //使用回调函数的方式则不需要停止线程
                m_bExitCaptureThread = true;
                while (m_tCaptureThread.IsAlive)
                {
                    Thread.Sleep(10);
                }
            #endif
                MvApi.CameraUnInit(m_hCamera);
                Marshal.FreeHGlobal(m_ImageBuffer);
                Marshal.FreeHGlobal(m_ImageBufferSnapshot);
                m_hCamera = 0;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            tSdkFrameStatistic tFrameStatistic;
            if (m_hCamera > 0)
            {
                //获得SDK中图像帧统计信息，捕获帧、错误帧等。
                MvApi.CameraGetFrameStatistic(m_hCamera, out tFrameStatistic);
                ////显示帧率有应用程序自己记录。
                //string sFrameInfomation = String.Format("| 图像分辨率:{0}*{1} | 显示帧数{2} | 捕获帧数{3} |", m_tFrameHead.iWidth, m_tFrameHead.iHeight, m_iDisplayedFrames, tFrameStatistic.iCapture);
                //StateLabel.Text = sFrameInfomation;

            }
            else
            {
                //StateLabel.Text = "";
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            //切换分辨率后，擦除一次背景
            if (m_bEraseBk == true)
            {
                m_bEraseBk = false;
                pic_Image.Refresh();
            }
        }
    }
}
