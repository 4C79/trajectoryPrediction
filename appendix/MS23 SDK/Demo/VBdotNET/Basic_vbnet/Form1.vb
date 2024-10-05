Imports System.IO

'BIG5 TRANS ALLOWED

Public Class Form1

    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Dim iCameraNum As Long
        Dim CameraList() As tSdkCameraDevInfo
        Dim iRet As Integer

        '//枚举设备，获得设备列表
        iRet = CameraEnumerateDevice(CameraList)
        iCameraNum = CameraList.GetLength(0)

        If iCameraNum < 1 Then
            m_hCamera = 0
            MsgBox("没有扫描到相机，请插上相机后再次运行本程序")
            Me.Close()
            Exit Sub
        Else

        End If

        iRet = InitCamera(CameraList(0), PictureBox1.Handle, PictureBox1.Width, PictureBox1.Height)

        If iRet <> 0 Then
            m_hCamera = 0
            MsgBox("Camera init error")
            Me.Close()
            Exit Sub
        Else
            Timer1.Enabled = True
            Timer1.Start()

            '下面两行启动定时器主动抓图
            Timer2.Enabled = True
            Timer2.Start()

            '下面两行设置抓图回调，使用回调的方式抓图
            'm_CapCallback = AddressOf CaptureImageCallbackFunc
            'CameraSetCallbackFunction(m_hCamera, m_CapCallback, 0, 0)

        End If
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        '显示配置窗口
        Dim iRet As Long
        iRet = CameraShowSettingPage(m_hCamera, 1)
    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        '播放
        CameraPlay(m_hCamera)
    End Sub

    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles Button3.Click
        '暂停
        CameraPause(m_hCamera)
    End Sub

    Private Sub Button4_Click(sender As System.Object, e As System.EventArgs) Handles Button4.Click
        '拍照，保存图片到指定路径
        Dim iRet As Long
        iRet = Snapshot()
    End Sub

    Private Sub Timer1_Tick(sender As System.Object, e As System.EventArgs) Handles Timer1.Tick
        Dim sFrameInfo As String
        sFrameInfo = "| Resolution " + Format$(m_iLastImageWidth, "General Number") + " X " + Format$(m_iLastImageHeight, "General Number") + " | Display frames " + Format$(m_iDisplayCounts, "General Number") + "|"
        Label1.Text = sFrameInfo
    End Sub

    Private Sub Timer2_Tick(sender As System.Object, e As System.EventArgs) Handles Timer2.Tick
        Dim iRet As Long
        iRet = CaptureImage()
    End Sub

    Private Sub Form1_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        If m_hCamera <> 0 Then
            Timer1.Enabled = False
            Timer2.Enabled = False
            CameraUnInit(m_hCamera)         '反初始化相机
            CameraAlignFree(m_pFrameBuffer)   '释放申请的内存块

        End If
    End Sub

    Private Sub SnapshotToBitmap_Click(sender As System.Object, e As System.EventArgs) Handles SnapshotToBitmap.Click
        Dim FrameInfo As tSdkFrameHead
        Dim pRawBuffer As Integer
        Dim pRgbBuffer As Integer
        Dim pBufferHandle As Integer
        Dim sFileName As String
        Dim sImageSize As tSdkImageResolution
        Dim iRet As Integer
        Dim sTmp() As Byte


        sImageSize.iWidth = 0
        sImageSize.iHeight = 0
        sImageSize.iIndex = &HFF

        '//CameraSetResolutionForSnap设置抓拍时的分辨率，可以和预览时相同，也可以不同。
        '//sImageSize.iIndex = 0xff; sImageSize.iWidth 和 sImageSize.iHeight 置0，表示抓拍时分辨率和预览时相同。
        '//如果您希望抓拍时为单独的分辨率，请参考我们的Snapshot例程。或者参阅SDK手册中有关CameraSetResolutionForSnap接口的详细说明
        iRet = CameraSetResolutionForSnap(m_hCamera, sImageSize)

        ' //  CameraSnapToBuffer抓拍一帧图像数据到缓冲区中，该缓冲区由SDK内部申请,成功调用后，需要
		'// !!!!!!注意：CameraSnapToBuffer 会切换分辨率拍照，速度较慢。做实时处理，请用CameraGetImageBuffer函数取图或者回调函数。
        iRet = CameraSnapToBuffer(m_hCamera, FrameInfo, pRawBuffer, 1000)

        If iRet <> CAMERA_STATUS_SUCCESS Then
            MsgBox("Snapshot failed,is camera in pause mode?")
        Else
            '//成功抓拍后，保存到文件


            '//申请一个buffer，用来将获得的原始数据转换为RGB数据，并同时获得图像处理效果
            pRgbBuffer = CameraAlignMalloc(FrameInfo.iWidth * FrameInfo.iHeight * 5, 16)


            '//CameraImageProcess得到RGB格式的数据,如果需要保存RAW数据，则直接调用CameraSaveImage，文件类型选择RAW格式即可
            iRet = CameraImageProcess(m_hCamera, pRawBuffer, pRgbBuffer, FrameInfo)

            iRet = CameraReleaseImageBuffer(m_hCamera, pRawBuffer)

            Form2.UpdateImage(pRgbBuffer, FrameInfo)

            CameraAlignFree(pRgbBuffer)   '释放申请的内存块

        End If


    End Sub

    Private Sub RWtest_Click(sender As System.Object, e As System.EventArgs) Handles RWtest.Click
        Form3.Show()

    End Sub
End Class
