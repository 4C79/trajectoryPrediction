Imports MVSDK
Imports MVSDK.MvApi
Imports HalconDotNet
Imports System.Diagnostics

Public Class Form1

    Private m_Grabber(3) As IntPtr
    Private m_hCamera(3) As Integer
    Private m_DevInfo(3) As tSdkCameraDevInfo
    Private m_StatLabel(3) As Label
    Private m_FrameCallback As pfnCameraGrabberFrameCallback = AddressOf FrameCallback

    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        InitUI()
        OpenCameras()
    End Sub

    Private Sub Form1_Shown(sender As System.Object, e As System.EventArgs) Handles MyBase.Shown
        For Each Grabber In m_Grabber
            If Grabber <> IntPtr.Zero Then
                CameraGrabber_StartLive(Grabber)
            End If
        Next
    End Sub

    Private Sub Form1_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        For i As Integer = 0 To m_Grabber.Length - 1
            CameraGrabber_Destroy(m_Grabber(i))
            m_Grabber(i) = IntPtr.Zero
            m_hCamera(i) = 0
        Next
    End Sub

    Private Sub InitUI()
        m_StatLabel(0) = Cam1Stat
        m_StatLabel(1) = Cam2Stat
        m_StatLabel(2) = Cam3Stat
        m_StatLabel(3) = Cam4Stat

        For Each stat As Label In m_StatLabel
            stat.Text = "未连接"
        Next
    End Sub

    Private Sub OpenCameras()
        Dim status As CameraSdkStatus
        Dim CameraList() As tSdkCameraDevInfo = Nothing

        CameraEnumerateDevice(CameraList)
        If CameraList Is Nothing OrElse CameraList.Length < 1 Then
            MsgBox("未扫描到相机")
            Return
        End If

        For i As Integer = 0 To Math.Min(CameraList.Length, 4) - 1
            status = CameraGrabber_Create(m_Grabber(i), CameraList(i))
            If status = CameraSdkStatus.CAMERA_STATUS_SUCCESS Then
                InitGrabber(i)
                m_StatLabel(i).Text = "已打开"
            Else
                m_StatLabel(i).Text = String.Format("打开失败:{0}", status)
            End If
        Next
    End Sub

    Private Sub InitGrabber(ByVal index As Integer)
        Dim Grabber As IntPtr = m_Grabber(index)
        Dim hCamera As Integer
        Dim DevInfo As tSdkCameraDevInfo = Nothing

        CameraGrabber_GetCameraHandle(Grabber, hCamera)
        m_hCamera(index) = hCamera

        CameraGrabber_GetCameraDevInfo(Grabber, DevInfo)
        m_DevInfo(index) = DevInfo

        CameraCreateSettingPage(hCamera, Me.Handle, CStrToString(DevInfo.acFriendlyName), Nothing, IntPtr.Zero, 0)
        CameraGrabber_SetRGBCallback(Grabber, m_FrameCallback, 0)

        Dim cap As tSdkCameraCapbility
        CameraGetCapability(hCamera, cap)
        If cap.sIspCapacity.bMonoSensor <> 0 Then
            ' 黑白相机设置ISP输出灰度图像
            CameraSetIspOutFormat(hCamera, emImageFormat.CAMERA_MEDIA_TYPE_MONO8)
        Else
            ' 彩色相机输出BGR24图像
            CameraSetIspOutFormat(hCamera, emImageFormat.CAMERA_MEDIA_TYPE_BGR8)
        End If
    End Sub

    Private Sub FrameCallback(
        ByVal Grabber As IntPtr,
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
        Dim Image As HObject = Nothing

        Try
            If pFrameHead.uiMediaType = emImageFormat.CAMERA_MEDIA_TYPE_MONO8 Then
                HOperatorSet.GenImage1(Image, "byte", w, h, pFrameBuffer)
            ElseIf pFrameHead.uiMediaType = emImageFormat.CAMERA_MEDIA_TYPE_BGR8 Then
                HOperatorSet.GenImageInterleaved(Image,
                    pFrameBuffer,
                    "bgr",
                    w, h,
                    -1, "byte",
                    w, h,
                    0, 0, -1, 0)
            Else
                Throw New HalconException("Image format is not supported!!")
            End If

            Dim ImageRaw As HObject = Image
            HOperatorSet.MirrorImage(ImageRaw, Image, "row")
            ImageRaw.Dispose()

            For i As Integer = 0 To m_Grabber.Length - 1
                If m_Grabber(i) = Grabber Then
                    ProcessImage(i, Image)
                    Exit For
                End If
            Next

        Catch ex As HalconException
            MessageBox.Show(ex.GetErrorMessage())
        Finally
            If Image IsNot Nothing Then
                Image.Dispose()
            End If
        End Try
    End Sub

    Private Sub ProcessImage(index As Integer, Image As HObject)
        If index = 0 Then
            ProcessCam1Image(Image)
        ElseIf index = 1 Then
            ProcessCam2Image(Image)
        ElseIf index = 2 Then
            ProcessCam3Image(Image)
        ElseIf index = 3 Then
            ProcessCam4Image(Image)
        Else
            Throw New HalconException("Grabber index invalid!!")
        End If
    End Sub

    Private Sub ProcessCam1Image(Image As HObject)
        DispImage(HWnd1.HalconWindow, Image)
    End Sub

    Private Sub ProcessCam2Image(Image As HObject)
        DispImage(HWnd2.HalconWindow, Image)
    End Sub

    Private Sub ProcessCam3Image(Image As HObject)
        DispImage(HWnd3.HalconWindow, Image)
    End Sub

    Private Sub ProcessCam4Image(Image As HObject)
        DispImage(HWnd4.HalconWindow, Image)
    End Sub

    Private Sub DispImage(HWnd As HWindow, Image As HObject)
        Dim w As HTuple = New HTuple()
        Dim h As HTuple = New HTuple()
        HOperatorSet.GetImageSize(Image, w, h)
        HWnd.SetPart(0, 0, h - 1, w - 1)
        HWnd.DispObj(Image)
    End Sub

    Private Sub CamSettings_Click(sender As System.Object, e As System.EventArgs) Handles Cam1Settings.Click, Cam4Settings.Click, Cam3Settings.Click, Cam2Settings.Click
        Dim index As Integer = -1
        If sender.Equals(Cam1Settings) Then
            index = 0
        ElseIf sender.Equals(Cam2Settings) Then
            index = 1
        ElseIf sender.Equals(Cam3Settings) Then
            index = 2
        ElseIf sender.Equals(Cam4Settings) Then
            index = 3
        End If

        Dim hCamera As Integer = m_hCamera(index)
        If hCamera > 0 Then
            CameraShowSettingPage(hCamera, 1)
        End If
    End Sub

    Private Sub Timer1_Tick(sender As System.Object, e As System.EventArgs) Handles Timer1.Tick
        For i As Integer = 0 To m_Grabber.Length - 1
            Dim Grabber As IntPtr = m_Grabber(i)
            If Grabber = IntPtr.Zero Then
                Continue For
            End If

            Dim Stat As tSdkGrabberStat
            CameraGrabber_GetStat(Grabber, Stat)

            m_StatLabel(i).Text = String.Format("{0}*{1}    显示:{2:F1}    采集:{3:F1}    帧数:{4}",
                                                Stat.Width, Stat.Height, Stat.DispFps, Stat.CapFps, Stat.Capture)
        Next
    End Sub
End Class
