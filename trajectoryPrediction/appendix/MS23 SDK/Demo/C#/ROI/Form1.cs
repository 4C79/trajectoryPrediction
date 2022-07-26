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
//using Snapshot;
using CameraHandle = System.Int32;
using MvApi = MVSDK.MvApi;
using System.IO;

namespace ROI
{
    public partial class Form1 : Form
    {
        #region variable
        protected CameraHandle m_hCamera = 0;             // 句柄
        protected IntPtr m_ImageBuffer;             // 预览通道RGB图像缓存
        //protected IntPtr m_ImageBufferSnapshot;     // 抓拍通道RGB图像缓存
        protected tSdkCameraCapbility tCameraCapability;  // 相机特性描述
        protected int m_iDisplayedFrames = 0;    //已经显示的总帧数
        protected CAMERA_SNAP_PROC m_CaptureCallback;
        protected IntPtr m_iCaptureCallbackCtx;     //图像回调函数的上下文参数
        protected Thread m_tCaptureThread;          //图像抓取线程
        protected bool m_bExitCaptureThread = false;//采用线程采集时，让线程退出的标志
        protected IntPtr m_iSettingPageMsgCallbackCtx; //相机配置界面消息回调函数的上下文参数   
        protected tSdkFrameHead m_tFrameHead;
        protected tSdkImageResolution tResolution;
        protected Form2 m_DlgResolution = new Form2();
        protected bool m_bEraseBk = false;
        protected bool m_bSaveImage = false;
        #endregion

        public Form1()
        {
            InitializeComponent();

            if (InitCamera() == true)
            {
                MvApi.CameraPlay(m_hCamera);
                //BtnPlay.Text = "Pause";
            }

        }

#if USE_CALL_BACK
        public void ImageCaptureCallback(CameraHandle hCamera, IntPtr pFrameBuffer, ref tSdkFrameHead pFrameHead, IntPtr pContext)
        {
            //图像处理，将原始输出转换为RGB格式的位图数据，同时叠加白平衡、饱和度、LUT等ISP处理。
            MvApi.CameraImageProcess(hCamera, pFrameBuffer, m_ImageBuffer, ref pFrameHead);
            //叠加十字线、自动曝光窗口、白平衡窗口信息(仅叠加设置为可见状态的)。   
            MvApi.CameraImageOverlay(hCamera, m_ImageBuffer, ref pFrameHead);
            //调用SDK封装好的接口，显示预览图像
            MvApi.CameraDisplayRGB24(hCamera, m_ImageBuffer, ref pFrameHead);
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
            IntPtr uRawBuffer;//rawbuffer由SDK内部申请。应用层不要调用delete之类的释放函数

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

                    if (m_bSaveImage)
                    {
                        MvApi.CameraSaveImage(m_hCamera, "c:\\test.bmp", m_ImageBuffer, ref FrameHead, emSdkFileType.FILE_BMP, 100);
                        m_bSaveImage = false;
                    }
                }

            }

        }
#endif

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox_AREA1_X1.Text = "0";
            textBox_AREA1_X2.Text = "499";
            textBox_AREA1_Y1.Text = "0";
            textBox_AREA1_Y2.Text = "499";

            textBox_AREA2_X1.Text = "600";
            textBox_AREA2_X2.Text = "999";
            textBox_AREA2_Y1.Text = "600";
            textBox_AREA2_Y2.Text = "999";

            textBox_AREA3_X1.Text = "1000";
            textBox_AREA3_X2.Text = "1999";
            textBox_AREA3_Y1.Text = "1000";
            textBox_AREA3_Y2.Text = "1943";

            textBox_AREA4_X1.Text = "800";
            textBox_AREA4_X2.Text = "2591";
            textBox_AREA4_Y1.Text = "0";
            textBox_AREA4_Y2.Text = "499";
        }

        private bool InitCamera()
        {
            CameraSdkStatus status;
            tSdkCameraDevInfo[] tCameraDevInfoList;
            IntPtr ptr;
            int i;
#if USE_CALL_BACK
            CAMERA_SNAP_PROC pCaptureCallOld = null;
#endif
            if (m_hCamera > 0)
            {
                //已经初始化过，直接返回 true

                return true;
            }

            status = MvApi.CameraEnumerateDevice(out tCameraDevInfoList);
            if (status == CameraSdkStatus.CAMERA_STATUS_SUCCESS)
            {
                if (tCameraDevInfoList != null)//此时iCameraCounts返回了实际连接的相机个数。如果大于1，则初始化第一个相机
                {
                    status = MvApi.CameraInit(ref tCameraDevInfoList[0], -1, -1, ref m_hCamera);
                    if (status == CameraSdkStatus.CAMERA_STATUS_SUCCESS)
                    {
                        //获得相机特性描述
                        MvApi.CameraGetCapability(m_hCamera, out tCameraCapability);

                        m_ImageBuffer = Marshal.AllocHGlobal(tCameraCapability.sResolutionRange.iWidthMax * tCameraCapability.sResolutionRange.iHeightMax * 3 + 1024);
                        //m_ImageBufferSnapshot = Marshal.AllocHGlobal(tCameraCapability.sResolutionRange.iWidthMax * tCameraCapability.sResolutionRange.iHeightMax * 3 + 1024);

                        //初始化显示模块，使用SDK内部封装好的显示接口
                        MvApi.CameraDisplayInit(m_hCamera, pictureBox1.Handle);
                        MvApi.CameraSetDisplaySize(m_hCamera, pictureBox1.Width, pictureBox1.Height);

                        //设置抓拍通道的分辨率。
                        tResolution.iIndex = 0xff;
                        tResolution.iHeight = tResolution.iHeightFOV = tCameraCapability.sResolutionRange.iHeightMax;
                        tResolution.iWidth = tResolution.iWidthFOV = tCameraCapability.sResolutionRange.iWidthMax;

                        //MvApi.CameraSetResolutionForSnap(m_hCamera, ref tResolution);

                        //让SDK来根据相机的型号动态创建该相机的配置窗口。
                        MvApi.CameraCreateSettingPage(m_hCamera, this.Handle, tCameraDevInfoList[0].acFriendlyName,/*SettingPageMsgCalBack*/null,/*m_iSettingPageMsgCallbackCtx*/(IntPtr)null, 0);

                        //两种方式来获得预览图像，设置回调函数或者使用定时器或者独立线程的方式，
                        //主动调用CameraGetImageBuffer接口来抓图。
                        //本例中仅演示了两种的方式,注意，两种方式也可以同时使用，但是在回调函数中，
                        //不要使用CameraGetImageBuffer，否则会造成死锁现象。
#if USE_CALL_BACK
                        m_CaptureCallback = new CAMERA_SNAP_PROC(ImageCaptureCallback);
                        MvApi.CameraSetCallbackFunction(m_hCamera, m_CaptureCallback, m_iCaptureCallbackCtx, ref pCaptureCallOld);
#else //如果需要采用多线程，使用下面的方式
                        m_bExitCaptureThread = false;
                        m_tCaptureThread = new Thread(new ThreadStart(CaptureThreadProc));
                        m_tCaptureThread.Start();

#endif
                        //MvApi.CameraReadSN 和 MvApi.CameraWriteSN 用于从相机中读写用户自定义的序列号或者其他数据，32个字节
                        //MvApi.CameraSaveUserData 和 MvApi.CameraLoadUserData用于从相机中读取自定义数据，512个字节
                        return true;

                    }
                    else
                    {
                        m_hCamera = 0;
                        //StateLabel.Text = "Camera init error";
                        String errstr = string.Format("相机初始化错误，错误码{0},错误原因是", status);
                        String errstring = MvApi.CameraGetErrorString(status);
                        // string str1 
                        MessageBox.Show(errstr + errstring, "ERROR");
                        Environment.Exit(0);
                        return false;
                    }

                }
            }
            else
            {
                MessageBox.Show("没有找到相机，如果已经接上相机，可能是权限不够，请尝试使用管理员权限运行程序。");
                Environment.Exit(0);
            }

            return false;

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
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
                //Marshal.FreeHGlobal(m_ImageBufferSnapshot);
                m_hCamera = 0;
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            MvApi.CameraShowSettingPage(m_hCamera, 1);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            String str;
            str = "[图像传感器ROI设置]是调节相机前端图像传感器内部的图像输出区域和阵列读取方式，";
            str += "只支持单一ROI区域设置，ROI区域越小，图像传感器输出的帧率会越高;";
            str += "\r\n";
            str += "[传输裁剪ROI设置]是设定相机中图像处理芯片的传输区域，图像处理芯片从图像传感器采集到图像后，";
            str += "\r\n";
            str += "仅传输ROI区域内的图像内容，可以节约传输带宽，从而提高帧率。这种情况下，用户从SDK中得到的图像尺寸仍然";
            str += "是当前图像传感器输出的尺寸一样大小的，没有被指定传输的区域，会被填充为黑色。";
            str += "\r\n";
            str += "[传输裁剪ROI设置]中，区域的像素宽度必须是偶数，即必须满足 X2 - X1 + 1 为偶数的关系";
            str += " 如果X2-X1为奇数，X1会被自动修正以满足像素宽度是偶数的需要";
            
            MessageBox.Show(str, "帮助说明");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CameraSdkStatus iStatus;
            int x1, x2, y1, y2;

            x1 = Convert.ToInt32(textBox_AREA1_X1.Text);
            x2 = Convert.ToInt32(textBox_AREA1_X2.Text);
            y1 = Convert.ToInt32(textBox_AREA1_Y1.Text);
            y2 = Convert.ToInt32(textBox_AREA1_Y2.Text);


            iStatus = MvApi.CameraSetTransferRoi(m_hCamera, 0, (uint)x1, (uint)y1, (uint)x2, (uint)y2);

            if (iStatus == CameraSdkStatus.CAMERA_STATUS_NOT_SUPPORTED)
            {
                MessageBox.Show("该型号的相机不支持该功能");
            }
            else if (iStatus == CameraSdkStatus.CAMERA_STATUS_PARAMETER_INVALID)
            {
                MessageBox.Show("输入参数错误，请确保ROI窗口在当前分辨率的图像尺寸范围内，并且确保X2 > X1，Y2 > Y1");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CameraSdkStatus iStatus;
            int x1, x2, y1, y2;

            x1 = Convert.ToInt32(textBox_AREA2_X1.Text);
            x2 = Convert.ToInt32(textBox_AREA2_X2.Text);
            y1 = Convert.ToInt32(textBox_AREA2_Y1.Text);
            y2 = Convert.ToInt32(textBox_AREA2_Y2.Text);


            iStatus = MvApi.CameraSetTransferRoi(m_hCamera, 0, (uint)x1, (uint)y1, (uint)x2, (uint)y2);
            
            if (iStatus == CameraSdkStatus.CAMERA_STATUS_NOT_SUPPORTED)
            {
                MessageBox.Show("该型号的相机不支持该功能");
            }
            else if (iStatus == CameraSdkStatus.CAMERA_STATUS_PARAMETER_INVALID)
            {
                MessageBox.Show("输入参数错误，请确保ROI窗口在当前分辨率的图像尺寸范围内，并且确保X2 > X1，Y2 > Y1");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            CameraSdkStatus iStatus;
            int x1, x2, y1, y2;

            x1 = Convert.ToInt32(textBox_AREA3_X1.Text);
            x2 = Convert.ToInt32(textBox_AREA3_X2.Text);
            y1 = Convert.ToInt32(textBox_AREA3_Y1.Text);
            y2 = Convert.ToInt32(textBox_AREA3_Y2.Text);


            iStatus = MvApi.CameraSetTransferRoi(m_hCamera, 0, (uint)x1, (uint)y1, (uint)x2, (uint)y2);

            if (iStatus == CameraSdkStatus.CAMERA_STATUS_NOT_SUPPORTED)
            {
                MessageBox.Show("该型号的相机不支持该功能");
            }
            else if (iStatus == CameraSdkStatus.CAMERA_STATUS_PARAMETER_INVALID)
            {
                MessageBox.Show("输入参数错误，请确保ROI窗口在当前分辨率的图像尺寸范围内，并且确保X2 > X1，Y2 > Y1");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            CameraSdkStatus iStatus;
            int x1, x2, y1, y2;

            x1 = Convert.ToInt32(textBox_AREA4_X1.Text);
            x2 = Convert.ToInt32(textBox_AREA4_X2.Text);
            y1 = Convert.ToInt32(textBox_AREA4_Y1.Text);
            y2 = Convert.ToInt32(textBox_AREA4_Y2.Text);


            iStatus = MvApi.CameraSetTransferRoi(m_hCamera, 0, (uint)x1, (uint)y1, (uint)x2, (uint)y2);

            if (iStatus == CameraSdkStatus.CAMERA_STATUS_NOT_SUPPORTED)
            {
                MessageBox.Show("该型号的相机不支持该功能");
            }
            else if (iStatus == CameraSdkStatus.CAMERA_STATUS_PARAMETER_INVALID)
            {
                MessageBox.Show("输入参数错误，请确保ROI窗口在当前分辨率的图像尺寸范围内，并且确保X2 > X1，Y2 > Y1");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (MvApi.CameraCustomizeResolution(m_hCamera, ref tResolution) == CameraSdkStatus.CAMERA_STATUS_SUCCESS)
            {
                MvApi.CameraSetImageResolution(m_hCamera, ref tResolution);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            String str;

            m_DlgResolution.clearDlgtext();
            m_DlgResolution.setDlgtext("tSdkImageResolution sRoiResolution;\r\n");
            m_DlgResolution.setDlgtext("memset(&sRoiResolution,0,sizeof(sRoiResolution));\r\n");
            m_DlgResolution.setDlgtext("sRoiResolution.iIndex = 0xff;\\\\设置成0xff表示自定义分辨率，设置成0到N表示选择预设分辨率\r\n");

            str = String.Format("sRoiResolution.iWidth = {0};\r\n", tResolution.iWidth);
            m_DlgResolution.setDlgtext(str);
            str = String.Format("sRoiResolution.iWidthFOV = {0};\r\n", tResolution.iWidthFOV);
            m_DlgResolution.setDlgtext(str);
            str = String.Format("sRoiResolution.iHeight = {0};\r\n", tResolution.iHeight);
            m_DlgResolution.setDlgtext(str);
            str = String.Format("sRoiResolution.iHeightFOV = {0};\r\n", tResolution.iHeightFOV);
            m_DlgResolution.setDlgtext(str);
            str = String.Format("sRoiResolution.iWidthZoomHd = {0};\r\n", tResolution.iWidthZoomHd);
            m_DlgResolution.setDlgtext(str);
            str = String.Format("sRoiResolution.iHeightZoomHd = {0};\r\n", tResolution.iHeightZoomHd);
            m_DlgResolution.setDlgtext(str);
            str = String.Format("sRoiResolution.iHOffsetFOV = {0};\r\n", tResolution.iHOffsetFOV);
            m_DlgResolution.setDlgtext(str);
            str = String.Format("sRoiResolution.iVOffsetFOV = {0};\r\n", tResolution.iVOffsetFOV);
            m_DlgResolution.setDlgtext(str);
            str = String.Format("sRoiResolution.iWidthZoomSw = {0};\r\n", tResolution.iWidthZoomSw);
            m_DlgResolution.setDlgtext(str);
            str = String.Format("sRoiResolution.iHeightZoomSw = {0};\r\n", tResolution.iHeightZoomSw);
            m_DlgResolution.setDlgtext(str);
            str = String.Format("sRoiResolution.uBinAverageMode = {0};\r\n", tResolution.uBinAverageMode);
            m_DlgResolution.setDlgtext(str);
            str = String.Format("sRoiResolution.uBinSumMode = {0};\r\n", tResolution.uBinSumMode);
            m_DlgResolution.setDlgtext(str);
            str = String.Format("sRoiResolution.uResampleMask = {0};\r\n", tResolution.uResampleMask);
            m_DlgResolution.setDlgtext(str);
            str = String.Format("sRoiResolution.uSkipMode = {0};\r\n", tResolution.uSkipMode);
            m_DlgResolution.setDlgtext(str);
            str = String.Format("CameraSetImageResolution(m_hCamera,&sRoiResolution);\r\n");
            m_DlgResolution.setDlgtext(str);

            m_DlgResolution.Show();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            uint uEnableMask = 0;

            uEnableMask = (uint)Convert.ToInt32(checkBox1.Checked);

            if (checkBox1.Checked)
            {
                button1_Click(sender, e);
            }

            MvApi.CameraEnableTransferRoi(m_hCamera, uEnableMask);

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            uint uEnableMask = 0;

            uEnableMask = (uint)Convert.ToInt32(checkBox2.Checked);

            if (checkBox2.Checked)
            {
                button2_Click(sender, e);
            }

            MvApi.CameraEnableTransferRoi(m_hCamera, uEnableMask);
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            uint uEnableMask = 0;

            uEnableMask = (uint)Convert.ToInt32(checkBox3.Checked);

            if (checkBox3.Checked)
            {
                button3_Click(sender, e);
            }

            MvApi.CameraEnableTransferRoi(m_hCamera, uEnableMask);
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            uint uEnableMask = 0;

            uEnableMask = (uint)Convert.ToInt32(checkBox4.Checked);

            if (checkBox4.Checked)
            {
                button4_Click(sender, e);
            }

            MvApi.CameraEnableTransferRoi(m_hCamera, uEnableMask);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            tSdkFrameStatistic tFrameStatistic;
            if (m_hCamera > 0)
            {
                //获得SDK中图像帧统计信息，捕获帧、错误帧等。
                MvApi.CameraGetFrameStatistic(m_hCamera, out tFrameStatistic);
                //显示帧率有应用程序自己记录。
                string sFrameInfomation = String.Format("| Resolution:{0}*{1} | Display frames{2} | Capture frames{3} |", m_tFrameHead.iWidth, m_tFrameHead.iHeight, m_iDisplayedFrames, tFrameStatistic.iCapture);
                StateLabel.Text = sFrameInfomation;

            }
            else
            {
                StateLabel.Text = "";
            }

        }

    }
}
