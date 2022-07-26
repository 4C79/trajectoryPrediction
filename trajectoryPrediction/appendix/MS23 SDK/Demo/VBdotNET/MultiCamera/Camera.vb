Imports MVSDK
Imports MVSDK.MvApi

Public Class Camera
    Private m_Grabber As IntPtr
    Private m_hCamera As Integer
    Private m_DevInfo As tSdkCameraDevInfo
    Private m_Cap As tSdkCameraCapbility
    Private m_SdkFrameCallback As pfnCameraGrabberFrameCallback = AddressOf SdkFrameCallback

    ' 采集事件
    Public Event Capture(ByVal pFrameBuffer As IntPtr, ByVal FrameHead As tSdkFrameHead)

    ' 获取相机个数
    Public Shared Function GetCount() As Integer
        Return CameraEnumerateDeviceEx()
    End Function

    ' 以序号打开相机
    Public Function Open(index As Integer) As Integer
        Dim status As CameraSdkStatus

        status = CameraGrabber_CreateByIndex(m_Grabber, index)
        If status = CameraSdkStatus.CAMERA_STATUS_SUCCESS Then
            InitGrabber()
        End If

        Return status
    End Function

    ' 以名字打开相机
    Public Function Open(name As String) As Integer
        Dim status As CameraSdkStatus

        status = CameraGrabber_CreateByName(m_Grabber, name)
        If status = CameraSdkStatus.CAMERA_STATUS_SUCCESS Then
            InitGrabber()
        End If

        Return status
    End Function

    ' 关闭相机
    Public Sub Close()
        If m_Grabber <> IntPtr.Zero Then
            CameraGrabber_Destroy(m_Grabber)
        End If
        m_Grabber = IntPtr.Zero
        m_hCamera = 0
    End Sub

    ' 设置触发模式（mode = 0:连续模式   1：软触发     2：硬件触发）
    Public Function SetTriggerMode(mode As Integer) As Boolean
        Return CameraSetTriggerMode(m_hCamera, mode) = 0
    End Function

    ' 发送一个触发指令给相机
    Public Function Trigger() As Boolean
        Return CameraSoftTrigger(m_hCamera) = 0
    End Function

    ' 启动相机采集
    Public Function StartLive() As Boolean
        Return CameraGrabber_StartLive(m_Grabber) = 0
    End Function

    ' 停止相机采集
    Public Function StopLive() As Boolean
        Return CameraGrabber_StopLive(m_Grabber) = 0
    End Function

    ' 显示相机配置页面
    Public Function ShowSettingPage() As Boolean
        Return CameraCreateSettingPage(m_hCamera, IntPtr.Zero, CStrToString(m_DevInfo.acFriendlyName), Nothing, IntPtr.Zero, 0) = 0 _
            And CameraShowSettingPage(m_hCamera, 1) = 0
    End Function

    ' 设置相机曝光时间
    Public Function SetExptime(ByVal time As Double) As Boolean
        Return CameraSetExposureTime(m_hCamera, time) = 0
    End Function

    ' 获取相机曝光时间
    Public Function GetExptime() As Double
        Dim time As Double
        CameraGetExposureTime(m_hCamera, time)
        Return time
    End Function

    ' 设置相机模拟增益
    Public Function SetAnalogGain(ByVal gain As Double) As Boolean
        Return CameraSetAnalogGain(m_hCamera, gain / m_Cap.sExposeDesc.fAnalogGainStep) = 0
    End Function

    ' 获取相机模拟增益
    Public Function GetAnalogGain() As Double
        Dim iGain As Integer
        CameraGetAnalogGain(m_hCamera, iGain)
        Return iGain * m_Cap.sExposeDesc.fAnalogGainStep
    End Function

    ' 获取帧率等统计信息
    Public Function GetStat() As tSdkGrabberStat
        Dim stat As tSdkGrabberStat
        CameraGrabber_GetStat(m_Grabber, stat)
        Return stat
    End Function

    ' 初始配置
    Private Sub InitGrabber()
        CameraGrabber_GetCameraHandle(m_Grabber, m_hCamera)
        CameraGrabber_GetCameraDevInfo(m_Grabber, m_DevInfo)
        CameraGetCapability(m_hCamera, m_Cap)

        CameraGrabber_SetRGBCallback(m_Grabber, m_SdkFrameCallback, 0)

        ' 设置相机为手动曝光模式
        CameraSetAeState(m_hCamera, 0)

        If m_Cap.sIspCapacity.bMonoSensor <> 0 Then
            ' 黑白相机设置ISP输出灰度图像
            CameraSetIspOutFormat(m_hCamera, emImageFormat.CAMERA_MEDIA_TYPE_MONO8)
        Else
            ' 彩色相机输出BGR24图像
            CameraSetIspOutFormat(m_hCamera, emImageFormat.CAMERA_MEDIA_TYPE_BGR8)
        End If
    End Sub

    ' SDK回调
    Private Sub SdkFrameCallback(
        ByVal Grabber As IntPtr,
        ByVal pFrameBuffer As IntPtr,
        ByRef pFrameHead As tSdkFrameHead,
        ByVal Context As IntPtr)
        RaiseEvent Capture(pFrameBuffer, pFrameHead)
    End Sub

End Class
