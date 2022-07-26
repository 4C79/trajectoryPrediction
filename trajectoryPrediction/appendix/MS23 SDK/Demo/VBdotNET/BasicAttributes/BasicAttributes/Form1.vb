Imports System.Drawing.Imaging
Public Class Form1

    Public m_Grabber As IntPtr
    Public m_hCamera As Integer
    Public m_DevInfo As tSdkCameraDevInfo
    Public m_Cap As tSdkCameraCapbility
    Public m_GrayPal As ColorPalette
    Public m_RGBCallback As CB_CameraGrabberFrameCallback = AddressOf RGBCallback

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim iRet
        Dim CameraList(1) As tSdkCameraDevInfo
        Dim fvalue As Double
        Dim ivalue As Integer

        m_Grabber = 0
        Label4.Text = ""
        ComboBoxTriggerMode.SelectedIndex = 0

        CameraEnumerateDevice(CameraList)
        If CameraList.Length > 0 Then
            iRet = CameraGrabber_Create(m_Grabber, CameraList(0))
            If iRet = CAMERA_STATUS_SUCCESS Then
                CameraGrabber_GetCameraHandle(m_Grabber, m_hCamera)
                CameraGrabber_GetCameraDevInfo(m_Grabber, m_DevInfo)

                CameraCreateSettingPage(m_hCamera, Me.Handle, m_DevInfo.acFriendlyName(0), 0, 0, 0)
                CameraGrabber_SetRGBCallback(m_Grabber, m_RGBCallback, 0)

                Dim CurrentTriggerMode As Integer
                CameraGetTriggerMode(m_hCamera, CurrentTriggerMode)

                CameraGetCapability(m_hCamera, m_Cap)
                If CurrentTriggerMode <> 0 Then
                    CameraSetTriggerMode(m_hCamera, 0)
                End If
                CameraSetTriggerCount(m_hCamera, 1)

                ' 黑白相机设置ISP输出灰度图像
                ' 彩色相机ISP默认会输出BGR24图像
                If m_Cap.sIspCapacity.bMonoSensor <> 0 Then
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

                ' 设置手动曝光模式
                CameraSetAeState(m_hCamera, 0)

                CameraGetExposureTime(m_hCamera, fvalue)
                TextBoxExp.Text = Convert.ToString(fvalue / 1000.0)

                CameraGetAnalogGain(m_hCamera, ivalue)
                TextBoxGain.Text = Convert.ToString(ivalue * m_Cap.sExposeDesc.fAnalogGainStep)

                CameraGrabber_StartLive(m_Grabber)
            Else
                MsgBox(String.Format("相机打开失败，错误码：{0}", iRet))
            End If
        Else
            MsgBox("未扫描到相机")
        End If
    End Sub

    Public Function RGBCallback( _
        ByVal Grabber As Integer, _
        ByVal pFrameBuffer As IntPtr, _
        ByRef pFrameHead As tSdkFrameHead, _
        ByVal Context As IntPtr) As Integer
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

        PictureBoxDisp.Image = Image
    End Function

    Private Sub Form1_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        CameraGrabber_Destroy(m_Grabber)
    End Sub

    Private Sub ButtonAllSettings_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAllSettings.Click
        If m_Grabber <> 0 Then
            CameraShowSettingPage(m_hCamera, 1)
        End If
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Dim stat As tSdkGrabberStat
        Dim sFrameInfo As String
        If m_Grabber <> 0 Then
            CameraGrabber_GetStat(m_Grabber, stat)
            sFrameInfo = "| Resolution: " + Format$(stat.Width, "General Number") + " X " + Format$(stat.Height, "General Number") + " | Display FPS: " + Format$(stat.DispFps, "Fixed") + " | Cap FPS: " + Format$(stat.CapFps, "Fixed") + "|"
            Label4.Text = sFrameInfo
        End If
    End Sub

    Private Sub ComboBoxTriggerMode_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBoxTriggerMode.SelectedIndexChanged
        Dim Item As Integer
        If m_Grabber <> 0 Then
            Item = ComboBoxTriggerMode.SelectedIndex
            CameraSetTriggerMode(m_hCamera, Item)
        End If
        If Item <> 0 Then
            ButtonTrigger.Visible = True
        Else
            ButtonTrigger.Visible = False
        End If
    End Sub

    Private Sub ButtonTrigger_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonTrigger.Click
        If m_Grabber <> 0 Then
            CameraSoftTrigger(m_hCamera)
        End If
    End Sub

    Private Sub TextBoxExp_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBoxExp.TextChanged
        If m_Grabber <> 0 Then
            Try
                Dim time As Double
                time = Convert.ToDouble(TextBoxExp.Text) * 1000.0
                CameraSetExposureTime(m_hCamera, time)
            Catch ex As Exception
            End Try
        End If
    End Sub

    Private Sub TextBoxGain_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBoxGain.TextChanged
        If m_Grabber <> 0 Then
            Try
                Dim gain As Double
                gain = Convert.ToDouble(TextBoxGain.Text)
                CameraSetAnalogGain(m_hCamera, gain / m_Cap.sExposeDesc.fAnalogGainStep)
            Catch ex As Exception
            End Try
        End If
    End Sub
End Class
