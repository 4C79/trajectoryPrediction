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
using System.Drawing.Imaging;
using SaveImage.Properties;
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;

namespace SaveImage
{
    public partial class Form1 : Form
    {
        #region variable
        private IntPtr m_Grabber = IntPtr.Zero;
        private CameraHandle m_hCamera = 0;
        private tSdkCameraDevInfo m_DevInfo;
        private pfnCameraGrabberFrameListener m_RawCallback;
        private int m_SaveIndex = 0;
        private AsyncSaveImage m_AsyncSave = new AsyncSaveImage();
        private MMTimer m_TriggerTimer = new MMTimer();
        #endregion

        public Form1()
        {
            InitializeComponent();
            UISettingsLoad();
            m_TriggerTimer.Timer += TriggerTimerTick;

            m_RawCallback = new pfnCameraGrabberFrameListener(CameraGrabberRawCallback);
            InitCamera();

            m_AsyncSave.Start();
        }

        private void InitCamera()
        {
            CameraSdkStatus status = 0;

            tSdkCameraDevInfo[] DevList;
            MvApi.CameraEnumerateDevice(out DevList);
            int NumDev = (DevList != null ? DevList.Length : 0);
            if (NumDev < 1)
            {
                MessageBox.Show("未扫描到相机");
                return;
            }
            else if (NumDev == 1)
            {
                status = MvApi.CameraGrabber_Create(out m_Grabber, ref DevList[0]);
            }
            else
            {
                status = MvApi.CameraGrabber_CreateFromDevicePage(out m_Grabber);
            }

            if (status == 0)
            {
                MvApi.CameraGrabber_GetCameraDevInfo(m_Grabber, out m_DevInfo);
                MvApi.CameraGrabber_GetCameraHandle(m_Grabber, out m_hCamera);
                MvApi.CameraCreateSettingPage(m_hCamera, this.Handle, m_DevInfo.acFriendlyName, null, (IntPtr)0, 0);

                MvApi.CameraGrabber_SetFrameListener(m_Grabber, m_RawCallback, IntPtr.Zero);

                // 黑白相机设置ISP输出灰度图像
                // 彩色相机ISP默认会输出BGR24图像
                tSdkCameraCapbility cap;
                MvApi.CameraGetCapability(m_hCamera, out cap);
                if (cap.sIspCapacity.bMonoSensor != 0)
                {
                    MvApi.CameraSetIspOutFormat(m_hCamera, (uint)MVSDK.emImageFormat.CAMERA_MEDIA_TYPE_MONO8);
                }
				
				// 设置触发模式
                MvApi.CameraSetTriggerMode(m_hCamera, comboBoxCamMode.SelectedIndex);
                MvApi.CameraSetTriggerCount(m_hCamera, 1);

                MvApi.CameraGrabber_SetHWnd(m_Grabber, this.DispWnd.Handle);
                MvApi.CameraGrabber_StartLive(m_Grabber);
            }
            else
            {
                MessageBox.Show(String.Format("打开相机失败，原因：{0}", status) );
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_TriggerTimer.Stop();
            if (m_Grabber != IntPtr.Zero)
                MvApi.CameraGrabber_StopLive(m_Grabber);
            m_AsyncSave.Stop();
            m_AsyncSave.Clear();
            MvApi.CameraGrabber_Destroy(m_Grabber);
            UISettingsSave();
        }

        private void UISettingsSave()
        {
            Settings.Default.AutoSaveDir = textBoxSaveDir.Text;
            Settings.Default.AutoSaveEnable = checkBoxAutoSave.Checked;
            Settings.Default.AutoSaveOnlyTrigger = checkBoxAutoSaveOnlyTrigger.Checked;
            Settings.Default.AutoSaveFormat = comboBoxSaveFormat.SelectedIndex;
            Settings.Default.CamMode = comboBoxCamMode.SelectedIndex;
            Settings.Default.TriggerPeriod = textBoxTriggerPeriod.Text;
            Settings.Default.Save();
        }

        private void UISettingsLoad()
        {
            textBoxSaveDir.Text = Settings.Default.AutoSaveDir;
            checkBoxAutoSave.Checked = Settings.Default.AutoSaveEnable;
            checkBoxAutoSaveOnlyTrigger.Checked = Settings.Default.AutoSaveOnlyTrigger;
            comboBoxSaveFormat.SelectedIndex = Settings.Default.AutoSaveFormat;
            comboBoxCamMode.SelectedIndex = Settings.Default.CamMode;
            textBoxTriggerPeriod.Text = Settings.Default.TriggerPeriod;
        }

        private int CameraGrabberRawCallback(
            IntPtr Grabber,
            int Phase,
            IntPtr pFrameBuffer,
            ref tSdkFrameHead pFrameHead,
            IntPtr Context)
        {
            if (Phase == 0)
            {
                // 获取保存参数
                bool bAutoSave = false, bSaveOnlyTrigger = false;
                string Dir = "";
                int Fmt = 0;
                this.Invoke((EventHandler)delegate
                {
                    bAutoSave = checkBoxAutoSave.Checked;
                    bSaveOnlyTrigger = checkBoxAutoSaveOnlyTrigger.Checked;
                    Dir = textBoxSaveDir.Text;
                    Fmt = comboBoxSaveFormat.SelectedIndex;
                });

                if (bAutoSave && !(bSaveOnlyTrigger && pFrameHead.bIsTrigger == 0))
                {
                    string filename = DateTime.Now.ToString("yyyyMMddHHmmss");
                    ++m_SaveIndex;
                    filename += "_" + m_SaveIndex.ToString("0000");
                    SaveImage(CombineImageSavePath(Dir, filename), Fmt, pFrameBuffer, pFrameHead);
                    return 0;
                }
            }

            return 1;
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
            {
                IntPtr Image;
                if (MvApi.CameraGrabber_SaveImage(m_Grabber, out Image, 2000) == CameraSdkStatus.CAMERA_STATUS_SUCCESS)
                {
                    string path = CombineImageSavePath(textBoxSaveDir.Text, 
                        string.Format("Snap{0}", System.Environment.TickCount));

                    IntPtr pFrameBuffer, pFrameHead;
                    MvApi.CameraImage_GetData(Image, out pFrameBuffer, out pFrameHead);
                    tSdkFrameHead FrameHead = (tSdkFrameHead)Marshal.PtrToStructure(pFrameHead, typeof(tSdkFrameHead));

                    SaveImage(path, comboBoxSaveFormat.SelectedIndex, pFrameBuffer, FrameHead);
                    MvApi.CameraImage_Destroy(Image);

                    MessageBox.Show(path);
                }
                else
                {
                    MessageBox.Show("Snap failed");
                }
            }
        }

        private string CombineImageSavePath(string dir, string filename)
        {
            if (dir.Length > 0)
            {
                try
                {
                    dir = Path.GetFullPath(dir);
                }
                catch (Exception e)
                {
                    dir = "";
                }
            }
            if (dir.Length == 0)
            {
                dir = AppDomain.CurrentDomain.BaseDirectory.ToString();
            }
            return System.IO.Path.Combine(dir, filename);
        }

        private bool SaveImage(string path, int FmtIndex, IntPtr pFrameBuffer, tSdkFrameHead FrameHead)
        {
            int Fmt = 2;
            switch (FmtIndex)
            {
                case 0:
                    Fmt = (FrameHead.uiMediaType == (uint)MVSDK.emImageFormat.CAMERA_MEDIA_TYPE_MONO8 ? 16 : 2);
                    break;
                case 1:
                    Fmt = 8;
                    break;
                case 2:
                    Fmt = 1;
                    break;
            }
            return m_AsyncSave.SaveImage(m_hCamera, path, pFrameBuffer, ref FrameHead, (emSdkFileType)Fmt, 80);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (m_Grabber != IntPtr.Zero)
            {
                tSdkGrabberStat stat;
                MvApi.CameraGrabber_GetStat(m_Grabber, out stat);
                string info = String.Format("| 分辨率:{0}*{1} | 帧数:{4} | 显示帧率:{2:0.0} | 采集帧率:{3:0.0} |", 
                    stat.Width, stat.Height, stat.DispFps, stat.CapFps, stat.Capture);
                StateLabel.Text = info;
            }
        }

        private void SoftTrigger()
        {
            if (m_Grabber != IntPtr.Zero)
            {
                int mode = 0;
                MvApi.CameraGetTriggerMode(m_hCamera, ref mode);
                if (mode != 0)
                {
                    // 只触发模式下调用软触发指令
                    MvApi.CameraSoftTrigger(m_hCamera);
                }
            }
        }

        private void buttonSoftTrigger_Click(object sender, EventArgs e)
        {
            SoftTrigger();
        }

        private void comboBoxCamMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_Grabber != IntPtr.Zero)
            {
                var box = sender as ComboBox;
                MvApi.CameraSetTriggerMode(m_hCamera, box.SelectedIndex);
            }
        }

        private void textBoxTriggerPeriod_TextChanged(object sender, EventArgs e)
        {
            var box = sender as TextBox;

            int Interval = 0;
            try
            {
                Interval = int.Parse(box.Text);
            }
            catch (Exception) { }

            m_TriggerTimer.Stop();
            if (Interval > 0)
            {
                m_TriggerTimer.Start((uint)Interval, true);
            }
        }

        private void TriggerTimerTick(object sender, EventArgs e)
        {
            SoftTrigger();
        }
    }

    public class AsyncSaveImage
    {
        private class Item
        {
            public CameraHandle hCamera;
            public IntPtr Image;
            public string FileName;
            public emSdkFileType FileType;
            public byte Quality;
        }

        private Queue<Item> mImageQ = new Queue<Item>();
        private int mMaxQSize = 1024;
        private EventWaitHandle mQEvent = new EventWaitHandle(false, EventResetMode.ManualReset);
        private EventWaitHandle mQuitEvent = new EventWaitHandle(false, EventResetMode.ManualReset);
        private List<Thread> mSaveThreads;
        private int mThreadsNum = System.Environment.ProcessorCount;

        public AsyncSaveImage(int MaxQSize = 0, int nThreadNum = 0)
        {
            if (MaxQSize > 0)
            {
                mMaxQSize = MaxQSize;
            }
            if (nThreadNum > 0)
            {
                mThreadsNum = nThreadNum;
            }
        }

        public void Start()
        {
            if (mSaveThreads != null)
                return;

            mSaveThreads = new List<Thread>();
            mQuitEvent.Reset();
            for (int i = 0; i < mThreadsNum; ++i)
            {
                var thread = new Thread(SaveProc);
                thread.Start();
                mSaveThreads.Add(thread);
            }
        }

        public void Stop()
        {
            if (mSaveThreads != null)
            {
                mQuitEvent.Set();
                foreach (var thread in mSaveThreads)
                {
                    thread.Join();
                }
                mSaveThreads = null;
            }
        }

        public void Clear()
        {
            lock (mImageQ)
            {
                foreach (var item in mImageQ)
                {
                    MvApi.CameraImage_Destroy(item.Image);
                }
                mImageQ.Clear();
            }
        }

        public bool SaveImage(CameraHandle hCamera,
            string lpszFileName,
            IntPtr pbyImageBuffer,
            ref tSdkFrameHead pFrInfo,
            emSdkFileType byFileType,
            Byte byQuality)
        {
            IntPtr Image;
            if (MvApi.CameraImage_Create(out Image, pbyImageBuffer, ref pFrInfo, 1) != CameraSdkStatus.CAMERA_STATUS_SUCCESS)
                return false;

            Item new_item = new Item();
            new_item.hCamera = hCamera;
            new_item.Image = Image;
            new_item.FileName = lpszFileName;
            new_item.FileType = byFileType;
            new_item.Quality = byQuality;

            lock (mImageQ)
            {
                if (mImageQ.Count < mMaxQSize)
                {
                    mImageQ.Enqueue(new_item);
                    mQEvent.Set();
                    return true;
                }
                else
                {
                    MvApi.CameraImage_Destroy(Image);
                    return false;
                }
            }
        }

        private void SaveProc()
        {
            while (true)
            {
                if (WaitHandle.WaitAny(new WaitHandle[] { mQuitEvent, mQEvent }) == 0)
                    break;

                Item item;
                lock (mImageQ)
                {
                    if (mImageQ.Count < 1)
                    {
                        mQEvent.Reset();
                        continue;
                    }
                    item = mImageQ.Dequeue();
                }

                SaveItem(item);
                MvApi.CameraImage_Destroy(item.Image);
            }
        }

        private void SaveItem(Item item)
        {
            IntPtr Image = item.Image;

            IntPtr pRaw, pHeadPtr;
            MvApi.CameraImage_GetData(Image, out pRaw, out pHeadPtr);
            tSdkFrameHead FrameHead = (tSdkFrameHead)Marshal.PtrToStructure(pHeadPtr, typeof(tSdkFrameHead));

            uint uIspOutFmt = 0;
            MvApi.CameraGetIspOutFormat(item.hCamera, ref uIspOutFmt);

            int w = FrameHead.iWidth, h = FrameHead.iHeight;
            if (FrameHead.iWidthZoomSw > 0 && FrameHead.iHeightZoomSw > 0)
            {
                w = FrameHead.iWidthZoomSw;
                h = FrameHead.iHeightZoomSw;
            }

            bool IspOutGray = (uIspOutFmt == (uint)MVSDK.emImageFormat.CAMERA_MEDIA_TYPE_MONO8);
            int RequireBufferSize = w * h * (IspOutGray ? 1 : 3);
            IntPtr pFrameBuffer = MvApi.CameraAlignMalloc(RequireBufferSize, 16);
            if (pFrameBuffer != IntPtr.Zero)
            {
                MvApi.CameraImageProcess(item.hCamera, pRaw, pFrameBuffer, ref FrameHead);
                MvApi.CameraDisplayRGB24(item.hCamera, pFrameBuffer, ref FrameHead);

                MvApi.CameraSaveImage(item.hCamera, item.FileName, pFrameBuffer, ref FrameHead, item.FileType, item.Quality);
                MvApi.CameraAlignFree(pFrameBuffer);
            }
        }
    }

    public class MMTimer : IDisposable
    {
        //Lib API declarations
        [DllImport("Winmm.dll", CharSet = CharSet.Auto)]
        static extern uint timeSetEvent(uint uDelay, uint uResolution, TimerCallback lpTimeProc, UIntPtr dwUser,
                                        uint fuEvent);

        [DllImport("Winmm.dll", CharSet = CharSet.Auto)]
        static extern uint timeKillEvent(uint uTimerID);

        [DllImport("Winmm.dll", CharSet = CharSet.Auto)]
        static extern uint timeGetTime();

        [DllImport("Winmm.dll", CharSet = CharSet.Auto)]
        static extern uint timeBeginPeriod(uint uPeriod);

        [DllImport("Winmm.dll", CharSet = CharSet.Auto)]
        static extern uint timeEndPeriod(uint uPeriod);

        //Timer type definitions
        [Flags]
        public enum fuEvent : uint
        {
            TIME_ONESHOT = 0, //Event occurs once, after uDelay milliseconds. 
            TIME_PERIODIC = 1,
            TIME_CALLBACK_FUNCTION = 0x0000, /* callback is function */
            //TIME_CALLBACK_EVENT_SET = 0x0010, /* callback is event - use SetEvent */
            //TIME_CALLBACK_EVENT_PULSE = 0x0020  /* callback is event - use PulseEvent */
            TIME_KILL_SYNCHRONOUS = 0x0100,
        }

        //Delegate definition for the API callback
        delegate void TimerCallback(uint uTimerID, uint uMsg, UIntPtr dwUser, UIntPtr dw1, UIntPtr dw2);

        //IDisposable code
        private bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    Stop();
                }
            }
            disposed = true;
        }

        ~MMTimer()
        {
            Dispose(false);
        }

        /// <summary>
        /// The current timer instance ID
        /// </summary>
        uint id = 0;

        /// <summary>
        /// The callback used by the the API
        /// </summary>
        TimerCallback thisCB;

        /// <summary>
        /// The timer elapsed event 
        /// </summary>
        public event EventHandler Timer;

        protected virtual void OnTimer(EventArgs e)
        {
            if (Timer != null)
            {
                Timer(this, e);
            }
        }

        public MMTimer()
        {
            //Initialize the API callback
            thisCB = CBFunc;
        }

        /// <summary>
        /// Stop the current timer instance (if any)
        /// </summary>
        public void Stop()
        {
            lock (this)
            {
                if (id != 0)
                {
                    timeKillEvent(id);
                    Debug.WriteLine("MMTimer " + id.ToString() + " stopped");
                    id = 0;
                }
            }
        }

        /// <summary>
        /// Start a timer instance
        /// </summary>
        /// <param name="ms">Timer interval in milliseconds</param>
        /// <param name="repeat">If true sets a repetitive event, otherwise sets a one-shot</param>
        public void Start(uint ms, bool repeat)
        {
            //Kill any existing timer
            Stop();

            //Set the timer type flags
            fuEvent f = fuEvent.TIME_CALLBACK_FUNCTION | fuEvent.TIME_KILL_SYNCHRONOUS
                | (repeat ? fuEvent.TIME_PERIODIC : fuEvent.TIME_ONESHOT);

            lock (this)
            {
                id = timeSetEvent(ms, 1, thisCB, UIntPtr.Zero, (uint)f);
                if (id == 0)
                {
                    throw new Exception("timeSetEvent error");
                }
                Debug.WriteLine("MMTimer " + id.ToString() + " started");
            }
        }

        void CBFunc(uint uTimerID, uint uMsg, UIntPtr dwUser, UIntPtr dw1, UIntPtr dw2)
        {
            //Callback from the MMTimer API that fires the Timer event. Note we are in a different thread here
            OnTimer(new EventArgs());
        }
    }
}
