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


namespace TriggerAndStrobe
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
        //protected SnapshotDlg m_DlgSnapshot = new SnapshotDlg();               //显示抓拍图像的窗口
        protected bool m_bEraseBk = false;
        protected bool m_bSaveImage = false;
        public const Int32 EXT_TRIG_MASK_LEVEL_MODE = 2;
        #endregion

        // GPIO模式
        public enum emCameraGPIOMode
        {
            IOMODE_TRIG_INPUT = 0,		//触发输入
            IOMODE_STROBE_OUTPUT,		//闪光灯输出
            IOMODE_GP_INPUT,			//通用型输入
            IOMODE_GP_OUTPUT,			//通用型输出
            IOMODE_PWM_OUTPUT,			//PWM型输出
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

        public Form1()
        {
            InitializeComponent();

            if (InitCamera() == true)
            {
                MvApi.CameraPlay(m_hCamera);
                MvApi.CameraSetTriggerMode(m_hCamera, (int)emSdkSnapMode.EXTERNAL_TRIGGER);
                textBox1.Text = "0";

                for (int i = 0; i < panel1.Controls.Count; i++ )
                {
                    if (panel1.Controls[i].Name == "radioButton1")
                    {
                        RadioButton r = (RadioButton)panel1.Controls[i];
                        r.Checked = true;
                    }
                    
                }

                button1.Text = "暂停";
                //BtnPlay.Text = "Pause";
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private bool InitCamera()
        {
            CameraSdkStatus status;
            tSdkCameraDevInfo[] tCameraDevInfoList;
            //IntPtr ptr;
            uint uMask = 0;
            //int i;
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
                        MvApi.CameraGetExtTrigCapability(m_hCamera,  ref uMask);

                        if (tCameraCapability.iTriggerDesc > 2)
                        {
                            comboBox1.Items.Add("上升沿触发");
                            comboBox1.Items.Add("下降沿触发");
                        }
                        else
                        {
                            MessageBox.Show("This camera do not support external trigger mode !Program will exit");
                            return false;
                        }

                        if ((uMask & EXT_TRIG_MASK_LEVEL_MODE) != 0)
                        {
                            comboBox1.Items.Add("高电平脉冲触发");
                            comboBox1.Items.Add("低电平脉冲触发");
                        }

                        comboBox2.Items.Add("自动模式");
                        comboBox2.Items.Add("手动模式");
                        comboBox2.Items.Add("GPIO输出模式");

                        comboBox1.SelectedIndex = 0;
                        comboBox2.SelectedIndex = 0;

                        m_ImageBuffer = Marshal.AllocHGlobal(tCameraCapability.sResolutionRange.iWidthMax * tCameraCapability.sResolutionRange.iHeightMax * 3 + 1024);
                        //m_ImageBufferSnapshot = Marshal.AllocHGlobal(tCameraCapability.sResolutionRange.iWidthMax * tCameraCapability.sResolutionRange.iHeightMax * 3 + 1024);

                        if (tCameraCapability.sIspCapacity.bMonoSensor != 0)
                        {
                            // 黑白相机输出8位灰度数据
                            MvApi.CameraSetIspOutFormat(m_hCamera, (uint)MVSDK.emImageFormat.CAMERA_MEDIA_TYPE_MONO8);
                        }

                        //初始化显示模块，使用SDK内部封装好的显示接口
                        MvApi.CameraDisplayInit(m_hCamera, pictureBox1.Handle);
                        MvApi.CameraSetDisplaySize(m_hCamera, pictureBox1.Width, pictureBox1.Height);

                        //MvApi.CameraSetResolutionForSnap(m_hCamera, ref tResolution);

                        int tabindex;
                        tabindex = (int)emSdkPropSheetMask.PROP_SHEET_INDEX_TRIGGER_SET;
                        //让SDK来根据相机的型号动态创建该相机的配置窗口。
                        MvApi.CameraCreateSettingPage(m_hCamera, this.Handle, tCameraDevInfoList[0].acFriendlyName, null, (IntPtr)null, (uint)(-1 & ~(1 << tabindex)));

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
                m_hCamera = 0;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MvApi.CameraShowSettingPage(m_hCamera, 1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(button1.Text == "播放")
            {
                button1.Text = "暂停";
                MvApi.CameraPlay(m_hCamera);
            }
            else
            {
                button1.Text = "播放";
                MvApi.CameraPause(m_hCamera);
            }
        }

        //切换输入触发信号种类
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            MvApi.CameraSetExtTrigSignalType(m_hCamera, comboBox1.SelectedIndex);
            /*
	          CameraSetExtTrigSignalType(m_hCamera,0); //设置为上升沿信号触发。
	          CameraSetExtTrigSignalType(m_hCamera,1); //设置为下降沿信号触发。
	          CameraSetExtTrigSignalType(m_hCamera,2); //设置为高电平信号触发，如果一直为高电平，则会一直触发，相当于连续预览。
	          CameraSetExtTrigSignalType(m_hCamera,3); //设置为低电平信号触发，如果一直为低电平，则会一直触发，相当于连续预览。。
	        */
//             switch (comboBox1.SelectedIndex)
//             {
//                 case 0: 
//                     break;
//                 case 1: 
//                     break;
//                 case 2: 
//                     break;
//                 case 3: 
//                     break;
//             }
        }

        //切换闪光灯信号输出模式
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex < 2)
            {
                MvApi.CameraSetStrobeMode(m_hCamera, comboBox2.SelectedIndex);
                MvApi.CameraSetOutPutIOMode(m_hCamera, 0, (int)emCameraGPIOMode.IOMODE_STROBE_OUTPUT);
            }
            else if (comboBox2.SelectedIndex == 2)
            {
                MvApi.CameraSetOutPutIOMode(m_hCamera, 0, (int)emCameraGPIOMode.IOMODE_GP_OUTPUT);
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
           
        }

        /*设置电平触发方式下的去抖时间
          只有在电平触发模式下,去抖时间才有效,在边沿触发下,忽略去抖时间。
          因此边沿触发方式，请一定要使用电子开关。电平触发方式加上去抖时间，可以去掉
          机械开关的抖动。
        */
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            int m_JitterTime;

            m_JitterTime = Convert.ToInt32(textBox1.Text);

            MvApi.CameraSetExtTrigJitterTime(m_hCamera, (uint)m_JitterTime);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                if (comboBox2.SelectedIndex < 2)
                {
                    MvApi.CameraSetStrobePolarity(m_hCamera, 0);
                }
                else if (comboBox2.SelectedIndex == 2)
                {
                    MvApi.CameraSetIOState(m_hCamera, 0, 0);
                }
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                if (comboBox2.SelectedIndex < 2)
                {
                    MvApi.CameraSetStrobePolarity(m_hCamera, 1);
                }
                else if (comboBox2.SelectedIndex == 2)
                {
                    MvApi.CameraSetIOState(m_hCamera, 0, 1);
                }
            }
        }

    }
}
