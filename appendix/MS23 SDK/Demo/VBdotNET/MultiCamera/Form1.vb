Imports MVSDK
Imports MVSDK.MvApi

Public Class Form1

    Private m_CameraList(3) As Camera

    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        ' 打开相机
        Dim nCamera As Integer = Camera.GetCount()
        For i = 0 To nCamera - 1
            Dim cam As Camera = New Camera()
            Dim status As Integer = cam.Open(i)
            If status = 0 Then
                Dim index As Integer = i
                AddHandler cam.Capture, Sub(pFrameBuffer As IntPtr, FrameHead As tSdkFrameHead) ProcessImage(index, pFrameBuffer, FrameHead)
                cam.StartLive()
                m_CameraList(i) = cam
            Else
                MessageBox.Show(String.Format("Camera {0} open failed: {1}", i, status))
            End If
        Next
    End Sub

    Private Sub Form1_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        For i = 0 To m_CameraList.Length - 1
            If m_CameraList(i) IsNot Nothing Then
                m_CameraList(i).Close()
                m_CameraList(i) = Nothing
            End If
        Next
    End Sub

    Private Sub ProcessImage(ByVal index As Integer,
                            ByVal pFrameBuffer As IntPtr,
                            ByVal FrameHead As tSdkFrameHead)
        Dim width As Integer = FrameHead.iWidth
        Dim height As Integer = FrameHead.iHeight
        Dim mono As Boolean = FrameHead.uiMediaType = CUInt(emImageFormat.CAMERA_MEDIA_TYPE_MONO8)

        ' 释放内存
        GC.Collect()

        ' 创建Bitmap
        Dim Image As Bitmap
        If mono Then
            Image = New Bitmap(Width, Height, Width, Imaging.PixelFormat.Format8bppIndexed, pFrameBuffer)
            Dim GrayPal As Drawing.Imaging.ColorPalette = Image.Palette
            For Y = 0 To 255
                GrayPal.Entries(Y) = Color.FromArgb(255, Y, Y, Y)
            Next
            Image.Palette = GrayPal
        Else
            Image = New Bitmap(Width, Height, Width * 3, Imaging.PixelFormat.Format24bppRgb, pFrameBuffer)
        End If

        ' 由于SDK输出的数据默认是从底到顶的，转换为Bitmap需要做一下垂直镜像
        Image.RotateFlip(RotateFlipType.RotateNoneFlipY)

        ' 显示图像
        ' 由于当前处于相机的回调线程中，委托到主线程中操作PictureBox
        Dim PicBox() As PictureBox = New PictureBox() {DispWnd1, DispWnd2, DispWnd3, DispWnd4}
        PicBox(index).Invoke(Sub()
                                 PicBox(index).Image = Image
                                 PicBox(index).Refresh()
                             End Sub)
    End Sub

    Private Sub buttonSettings1_Click(sender As System.Object, e As System.EventArgs) Handles buttonSettings1.Click
        m_CameraList(0).ShowSettingPage()
    End Sub

    Private Sub buttonPlay1_Click(sender As System.Object, e As System.EventArgs) Handles buttonPlay1.Click
        m_CameraList(0).StartLive()
    End Sub

    Private Sub buttonStop1_Click(sender As System.Object, e As System.EventArgs) Handles buttonStop1.Click
        m_CameraList(0).StopLive()
    End Sub

    Private Sub buttonSettings2_Click(sender As System.Object, e As System.EventArgs) Handles buttonSettings2.Click
        m_CameraList(1).ShowSettingPage()
    End Sub

    Private Sub buttonPlay2_Click(sender As System.Object, e As System.EventArgs) Handles buttonPlay2.Click
        m_CameraList(1).StartLive()
    End Sub

    Private Sub buttonStop2_Click(sender As System.Object, e As System.EventArgs) Handles buttonStop2.Click
        m_CameraList(1).StopLive()
    End Sub

    Private Sub buttonSettings3_Click(sender As System.Object, e As System.EventArgs) Handles buttonSettings3.Click
        m_CameraList(2).ShowSettingPage()
    End Sub

    Private Sub buttonPlay3_Click(sender As System.Object, e As System.EventArgs) Handles buttonPlay3.Click
        m_CameraList(2).StartLive()
    End Sub

    Private Sub buttonStop3_Click(sender As System.Object, e As System.EventArgs) Handles buttonStop3.Click
        m_CameraList(2).StopLive()
    End Sub

    Private Sub buttonSettings4_Click(sender As System.Object, e As System.EventArgs) Handles buttonSettings4.Click
        m_CameraList(3).ShowSettingPage()
    End Sub

    Private Sub buttonPlay4_Click(sender As System.Object, e As System.EventArgs) Handles buttonPlay4.Click
        m_CameraList(3).StartLive()
    End Sub

    Private Sub buttonStop4_Click(sender As System.Object, e As System.EventArgs) Handles buttonStop4.Click
        m_CameraList(3).StopLive()
    End Sub

    Private Sub Timer1_Tick(sender As System.Object, e As System.EventArgs) Handles Timer1.Tick
        Dim Labels() As Label = New Label() {label1, label2, label3, label4}
        For i = 0 To 3
            Dim cam As Camera = m_CameraList(i)
            If cam IsNot Nothing Then
                Dim stat As tSdkGrabberStat = cam.GetStat()
                Labels(i).Text = String.Format("{0}*{1}    显示:{2:F1}FPS    采集:{3:F1}FPS    帧数:{4}",
                                                stat.Width, stat.Height, stat.DispFps, stat.CapFps, stat.Capture)
            End If
        Next
    End Sub
End Class
