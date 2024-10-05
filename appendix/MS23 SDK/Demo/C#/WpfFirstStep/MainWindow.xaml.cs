using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MVSDK;//使用SDK接口
using CameraHandle = System.Int32;
using MvApi = MVSDK.MvApi;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Interop;
using System.Runtime.InteropServices;

namespace WpfFirstStep
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        protected IntPtr m_Grabber = IntPtr.Zero;
        protected CameraHandle m_hCamera = 0;
        protected tSdkCameraDevInfo m_DevInfo;
        protected ColorPalette m_GrayPal;
        protected pfnCameraGrabberFrameCallback m_FrameCallback;
        protected System.Windows.Threading.DispatcherTimer m_StatTimer;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            m_FrameCallback = new pfnCameraGrabberFrameCallback(CameraGrabberFrameCallback);
            InitCamera();

            // 启动帧率统计定时器
            m_StatTimer = new System.Windows.Threading.DispatcherTimer();
            m_StatTimer.Interval = TimeSpan.FromSeconds(1);
            m_StatTimer.Tick += timer1_Tick;
            m_StatTimer.Start();
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

                var handle = (new WindowInteropHelper(this)).Handle;
                MvApi.CameraCreateSettingPage(m_hCamera, handle, m_DevInfo.acFriendlyName, null, (IntPtr)0, 0);

                MvApi.CameraGrabber_SetRGBCallback(m_Grabber, m_FrameCallback, IntPtr.Zero);

                // 黑白相机设置ISP输出灰度图像
                // 彩色相机ISP默认会输出BGR24图像
                tSdkCameraCapbility cap;
                MvApi.CameraGetCapability(m_hCamera, out cap);
                if (cap.sIspCapacity.bMonoSensor != 0)
                {
                    MvApi.CameraSetIspOutFormat(m_hCamera, (uint)MVSDK.emImageFormat.CAMERA_MEDIA_TYPE_MONO8);

                    // 创建灰度调色板
                    Bitmap Image = new Bitmap(1, 1, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
                    m_GrayPal = Image.Palette;
                    for (int Y = 0; Y < m_GrayPal.Entries.Length; Y++)
                        m_GrayPal.Entries[Y] = System.Drawing.Color.FromArgb(255, Y, Y, Y);
                }

                // 为了演示如何在回调中使用相机数据创建Bitmap并显示到PictureBox中，这里不使用SDK内置的绘制操作
                //MvApi.CameraGrabber_SetHWnd(m_Grabber, this.DispWnd.Handle);

                MvApi.CameraGrabber_StartLive(m_Grabber);
            }
            else
            {
                MessageBox.Show(String.Format("打开相机失败，原因：{0}", status));
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MvApi.CameraGrabber_Destroy(m_Grabber);
        }

        private void BtnSettings_Click(object sender, RoutedEventArgs e)
        {
            if (m_Grabber != IntPtr.Zero)
                MvApi.CameraShowSettingPage(m_hCamera, 1);
        }

        private void BtnPlay_Click(object sender, RoutedEventArgs e)
        {
            if (m_Grabber != IntPtr.Zero)
                MvApi.CameraGrabber_StartLive(m_Grabber);
        }

        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            if (m_Grabber != IntPtr.Zero)
                MvApi.CameraGrabber_StopLive(m_Grabber);
        }

        private void BtnSnap_Click(object sender, RoutedEventArgs e)
        {
            if (m_Grabber != IntPtr.Zero)
            {
                IntPtr Image;
                if (MvApi.CameraGrabber_SaveImage(m_Grabber, out Image, 2000) == CameraSdkStatus.CAMERA_STATUS_SUCCESS)
                {
                    string filename = System.IO.Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory.ToString(),
                        string.Format("{0}.bmp", System.Environment.TickCount));

                    MvApi.CameraImage_SaveAsBmp(Image, filename);

                    MvApi.CameraImage_Destroy(Image);

                    MessageBox.Show(filename);
                }
                else
                {
                    MessageBox.Show("Snap failed");
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (m_Grabber != IntPtr.Zero)
            {
                tSdkGrabberStat stat;
                MvApi.CameraGrabber_GetStat(m_Grabber, out stat);
                string info = String.Format("| Resolution:{0}*{1} | DispFPS:{2:0.0} | CapFPS:{3:0.0} |",
                    stat.Width, stat.Height, stat.DispFps, stat.CapFps);
                LabelStat.Content = info;
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

            // 执行一次GC，释放出内存
            GC.Collect();

            // 由于SDK输出的数据默认是从底到顶的，转换为Bitmap需要做一下垂直镜像
            MvApi.CameraFlipFrameBuffer(pFrameBuffer, ref pFrameHead, 1);

            int w = pFrameHead.iWidth;
            int h = pFrameHead.iHeight;
            Boolean gray = (pFrameHead.uiMediaType == (uint)MVSDK.emImageFormat.CAMERA_MEDIA_TYPE_MONO8);
            Bitmap Image = new Bitmap(w, h,
                gray ? w : w * 3,
                gray ? System.Drawing.Imaging.PixelFormat.Format8bppIndexed : System.Drawing.Imaging.PixelFormat.Format24bppRgb,
                pFrameBuffer);

            // 如果是灰度图要设置调色板
            if (gray)
            {
                Image.Palette = m_GrayPal;
            }

            IntPtr hBitmap = Image.GetHbitmap();
            BitmapSource bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                hBitmap, IntPtr.Zero, Int32Rect.Empty,
                System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
            bitmapSource.Freeze();
            DeleteObject(hBitmap);

            this.Dispatcher.Invoke(new Action(() =>
            {
                ImageDisp.Source = bitmapSource;
            }));
        }

        [DllImport("gdi32")]
        static extern int DeleteObject(IntPtr o);
    }
}
