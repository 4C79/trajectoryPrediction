
'BIG5 TRANS ALLOWED

Public Class Form1


    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Dim iCameraNum As Long
        Dim iRet As Integer
        Dim iRet1 As Integer
        Dim iRet2 As Integer
        '//枚举设备，获得设备列表

        iCameraNum = CameraEnumerateDeviceEx()

        If iCameraNum < 2 Then
            m_hCamera1 = 0
            m_hCamera2 = 0
            MsgBox("没有扫描到2个相机，请插上2个相机后再次运行本程序")
            Me.Close()
            Exit Sub
        Else

        End If


        iRet1 = InitCamera1(PictureBox1.Handle, PictureBox1.Width, PictureBox1.Height)
        iRet2 = InitCamera2(PictureBox2.Handle, PictureBox2.Width, PictureBox2.Height)

        If iRet1 <> 0 Or iRet2 <> 0 Then
            m_hCamera1 = 0
            m_hCamera2 = 0
            MsgBox("Camera init error")
            Me.Close()
            Exit Sub
        Else
            Timer1.Enabled = True
            Timer1.Start()
            Timer2.Enabled = True
            Timer2.Start()
        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        '显示配置窗口
        Dim iRet As Long
        iRet = CameraShowSettingPage(m_hCamera1, 1)
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        '播放
        CameraPlay(m_hCamera1)
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        '暂停
        CameraPause(m_hCamera1)
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Dim sFrameInfo As String
        sFrameInfo = "| Resolution " + Format$(m_iLastImageWidth1, "General Number") + " X " + Format$(m_iLastImageHeight1, "General Number") + " | Display frames " + Format$(m_iDisplayCounts1, "General Number") + "|"
        Label1.Text = sFrameInfo
    End Sub

    Private Sub Timer2_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer2.Tick
        Dim iRet As Long
        iRet = CaptureImage1()
        iRet = CaptureImage2()
    End Sub

    Private Sub Form1_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        If m_hCamera1 <> 0 Then
            Timer1.Enabled = False
            Timer2.Enabled = False
            CameraUnInit(m_hCamera1)         '反初始化相机
            GlobalUnlock(m_hBufferHandle1)   '释放申请的内存块
            GlobalFree(m_hBufferHandle1)

            CameraUnInit(m_hCamera2)         '反初始化相机
            GlobalUnlock(m_hBufferHandle2)   '释放申请的内存块
            GlobalFree(m_hBufferHandle2)
        End If
    End Sub

    Private Sub Button4_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        '显示配置窗口
        Dim iRet As Long
        iRet = CameraShowSettingPage(m_hCamera2, 1)
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        '播放
        CameraPlay(m_hCamera2)
    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        '暂停
        CameraPause(m_hCamera2)
    End Sub
End Class
