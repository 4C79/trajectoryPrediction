Imports System.Drawing.Imaging

Public Class Form1

    Public m_Grabber As IntPtr
    Public m_hCamera As Integer
    Public m_DevInfo As tSdkCameraDevInfo
    Public m_GrayPal As ColorPalette
    Public m_FrameCallback As CB_CameraGrabberFrameCallback = AddressOf FrameCallback

    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

        Dim iRet
        Dim CameraList(1) As tSdkCameraDevInfo
        Dim SoftTriggerMode As Boolean = False

        Label1.Text = ""
        m_Grabber = 0

        CameraEnumerateDevice(CameraList)
        If CameraList.Length < 1 Then
            MsgBox("未扫描到相机")
            Return
        ElseIf CameraList.Length = 1 Then
            iRet = CameraGrabber_Create(m_Grabber, CameraList(0))
        Else
            iRet = CameraGrabber_CreateFromDevicePage(m_Grabber)
        End If

        If iRet = CAMERA_STATUS_SUCCESS Then
            CameraGrabber_GetCameraHandle(m_Grabber, m_hCamera)
            CameraGrabber_GetCameraDevInfo(m_Grabber, m_DevInfo)

            CameraCreateSettingPage(m_hCamera, Me.Handle, m_DevInfo.acFriendlyName(0), 0, 0, 0)
            CameraGrabber_SetRGBCallback(m_Grabber, m_FrameCallback, 0)

            ' 黑白相机设置ISP输出灰度图像
            ' 彩色相机ISP默认会输出BGR24图像
            Dim cap As tSdkCameraCapbility
            CameraGetCapability(m_hCamera, cap)
            If cap.sIspCapacity.bMonoSensor <> 0 Then
                CameraSetIspOutFormat(m_hCamera, CAMERA_MEDIA_TYPE_MONO8)

                ' 创建灰度调色板
                Dim Image As New Bitmap(1, 1, Imaging.PixelFormat.Format8bppIndexed)
                m_GrayPal = Image.Palette
                For Y As Integer = 0 To m_GrayPal.Entries.Length - 1
                    m_GrayPal.Entries(Y) = Color.FromArgb(255, Y, Y, Y)
                Next
            End If

            ' 设置VFlip，由于SDK输出的数据默认是从底到顶的，打开VFlip后就可以直接转换为Bitmap
            CameraSetMirror(m_hCamera, 1, 1)

            Dim CurrentTriggerMode As Integer
            CameraGetTriggerMode(m_hCamera, CurrentTriggerMode)

            ' 演示软触发的使用
            ' 在上面把变量打开即可：SoftTriggerMode = True 
            If SoftTriggerMode Then
                If CurrentTriggerMode <> 1 Then
                    CameraSetTriggerMode(m_hCamera, 1)
                End If
                Threading.Thread.Sleep(100)
                CameraClearBuffer(m_hCamera)
            Else
                If CurrentTriggerMode <> 0 Then
                    CameraSetTriggerMode(m_hCamera, 0)
                End If
                SoftTrigger.Visible = False
            End If

            ' 为了演示如何在回调中使用相机数据创建Bitmap并显示到PictureBox中，这里不使用SDK内置的绘制操作
            'CameraGrabber_SetHWnd(m_Grabber, DispWnd.Handle)

            CameraGrabber_StartLive(m_Grabber)
        Else
            MsgBox(String.Format("相机打开失败，错误码：{0}", iRet))
        End If
    End Sub

    Public Sub FrameCallback(
        ByVal Grabber As Integer,
        ByVal pFrameBuffer As IntPtr,
        ByRef pFrameHead As tSdkFrameHead,
        ByVal Context As IntPtr)
        ' 数据处理回调

        ' 由于黑白相机在相机打开后设置了ISP输出灰度图像
        ' 因此此处pFrameBuffer=8位灰度数据
        ' 否则会和彩色相机一样输出BGR24数据

        ' 彩色相机ISP默认会输出BGR24图像
        ' pFrameBuffer=BGR24数据

        ' 执行一次GC，释放出内存
        GC.Collect()

        Dim w As Integer = pFrameHead.iWidth
        Dim h As Integer = pFrameHead.iHeight
        Dim gray As Boolean = (pFrameHead.uiMediaType = CAMERA_MEDIA_TYPE_MONO8)

        Dim Image As New Bitmap(w, h, IIf(gray, w, w * 3), IIf(gray, PixelFormat.Format8bppIndexed, PixelFormat.Format24bppRgb), pFrameBuffer)
        If gray Then
            Image.Palette = m_GrayPal
        End If

        Me.Invoke(Sub()
                      DispWnd.Image = Image
                      DispWnd.Refresh()
                  End Sub)
    End Sub
    Private Sub Form1_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        CameraGrabber_Destroy(m_Grabber)
    End Sub

    Private Sub ButtonSettings_Click(sender As System.Object, e As System.EventArgs) Handles ButtonSettings.Click
        If m_Grabber <> 0 Then
            CameraShowSettingPage(m_hCamera, 1)
        End If
    End Sub

    Private Sub ButtonPlay_Click(sender As System.Object, e As System.EventArgs) Handles ButtonPlay.Click
        If m_Grabber <> 0 Then
            CameraGrabber_StartLive(m_Grabber)
        End If
    End Sub

    Private Sub ButtonStop_Click(sender As System.Object, e As System.EventArgs) Handles ButtonStop.Click
        If m_Grabber <> 0 Then
            CameraGrabber_StopLive(m_Grabber)
        End If
    End Sub

    Private Sub ButtonSnap_Click(sender As System.Object, e As System.EventArgs) Handles ButtonSnap.Click
        Dim Ret As Integer
        Dim Image As Integer
        Dim FileName As String
        Dim Tmp() As Byte
        If m_Grabber <> 0 Then
            Ret = CameraGrabber_SaveImage(m_Grabber, Image, 2000)
            If Ret = CAMERA_STATUS_SUCCESS Then
                FileName = Application.StartupPath() + "\" + Format$(Now, "yyyy-mm-dd-h-mm-ss") + ".BMP0"

                '将VB的string类型转换为C的字符串，并且在字符串末尾添加结束符
                Tmp = System.Text.Encoding.Default.GetBytes(FileName)
                Tmp(UBound(Tmp)) = 0

                CameraImage_SaveAsBmp(Image, Tmp(0))
                MessageBox.Show(FileName)

                CameraImage_Destroy(Image)
            Else
                MessageBox.Show("Snap failed!!")
            End If
        End If
    End Sub

    Private Sub Timer1_Tick(sender As System.Object, e As System.EventArgs) Handles Timer1.Tick
        Dim stat As tSdkGrabberStat
        Dim sFrameInfo As String
        If m_Grabber <> 0 Then
            CameraGrabber_GetStat(m_Grabber, stat)
            sFrameInfo = "| Resolution: " + Format$(stat.Width, "General Number") + " X " + Format$(stat.Height, "General Number") + " | Display FPS: " + Format$(stat.DispFps, "Fixed") + " | Cap FPS: " + Format$(stat.CapFps, "Fixed") + "|"
            Label1.Text = sFrameInfo
        End If
    End Sub

    Private Sub SoftTrigger_Click(sender As System.Object, e As System.EventArgs) Handles SoftTrigger.Click
        CameraSoftTrigger(m_hCamera)
    End Sub
End Class
