'BIG5 TRANS ALLOWED


Module CameraVbCore
    '相机1参数
    Public m_hCamera1 As Integer        '相机句柄
    Public m_hBufferHandle1 As Integer  '显示缓冲区句柄
    Public m_pFrameBuffer1 As Integer   '显示缓冲区首地址
    Public m_iLastImageWidth1 As Integer
    Public m_iLastImageHeight1 As Integer
    Public m_iDisplayCounts1 As Integer '记录当前显示的总帧数

    '相机2参数
    Public m_hCamera2 As Integer        '相机句柄
    Public m_hBufferHandle2 As Integer  '显示缓冲区句柄
    Public m_pFrameBuffer2 As Integer   '显示缓冲区首地址
    Public m_iLastImageWidth2 As Integer
    Public m_iLastImageHeight2 As Integer
    Public m_iDisplayCounts2 As Integer '记录当前显示的总帧数

    '实现类似malloc的操作，用于动态申请内存
    Public Declare Function GlobalAlloc Lib "kernel32" (ByVal wFlags As Integer, ByVal dwBytes As Integer) As Integer
    Public Declare Function GlobalLock Lib "kernel32" (ByVal hMem As Integer) As Integer
    Public Declare Function GlobalUnlock Lib "kernel32" (ByVal hMem As Integer) As Integer
    Public Declare Function GlobalFree Lib "kernel32" (ByVal hMem As Integer) As Integer

    '相机1捕获一帧图像并显示
    Public Function CaptureImage1() As Integer

        Dim sFrameInfo As tSdkFrameHead
        Dim pRawBuffer As Integer          '原始数据的缓冲区地址，从SDK中获得
        Dim iRet As Integer

        iRet = CameraGetImageBuffer(m_hCamera1, sFrameInfo, pRawBuffer, 200) '抓取一帧图像，超时时间为200毫秒


        If iRet = CAMERA_STATUS_SUCCESS Then
            If (m_iLastImageWidth1 <> sFrameInfo.iWidth) Or (m_iLastImageHeight1 <> sFrameInfo.iHeight) Then
                m_iLastImageWidth1 = sFrameInfo.iWidth
                m_iLastImageHeight1 = sFrameInfo.iHeight
                Form1.PictureBox1.Refresh()
            End If

            '将原始数据转换为RGB格式，同时叠加图像处理的效果
            iRet = CameraImageProcess(m_hCamera1, pRawBuffer, m_pFrameBuffer1, sFrameInfo)

            If iRet = CAMERA_STATUS_SUCCESS Then '处理成功，则显示该图像，这里使用了SDK封装的接口进行图像显示，您也可以自己实现图像的显示，或者直接在这里调用保存图像的接口进行保存
                iRet = CameraDisplayRGB24(m_hCamera1, m_pFrameBuffer1, sFrameInfo)
            End If

            iRet = CameraReleaseImageBuffer(m_hCamera1, pRawBuffer) '成功调用CameraGetImageBuffer后必须调用CameraReleaseImageBuffer释放buffer

            m_iDisplayCounts1 = m_iDisplayCounts1 + 1
        End If

        CaptureImage1 = CAMERA_STATUS_SUCCESS
    End Function

    '相机1捕获一帧图像并显示
    Public Function CaptureImage2() As Integer

        Dim sFrameInfo As tSdkFrameHead
        Dim pRawBuffer As Integer          '原始数据的缓冲区地址，从SDK中获得
        Dim iRet As Integer

        iRet = CameraGetImageBuffer(m_hCamera2, sFrameInfo, pRawBuffer, 200) '抓取一帧图像，超时时间为200毫秒


        If iRet = CAMERA_STATUS_SUCCESS Then
            If (m_iLastImageWidth2 <> sFrameInfo.iWidth) Or (m_iLastImageHeight2 <> sFrameInfo.iHeight) Then
                m_iLastImageWidth2 = sFrameInfo.iWidth
                m_iLastImageHeight2 = sFrameInfo.iHeight
                Form1.PictureBox1.Refresh()
            End If

            '将原始数据转换为RGB格式，同时叠加图像处理的效果
            iRet = CameraImageProcess(m_hCamera2, pRawBuffer, m_pFrameBuffer2, sFrameInfo)

            If iRet = CAMERA_STATUS_SUCCESS Then '处理成功，则显示该图像，这里使用了SDK封装的接口进行图像显示，您也可以自己实现图像的显示，或者直接在这里调用保存图像的接口进行保存
                iRet = CameraDisplayRGB24(m_hCamera2, m_pFrameBuffer2, sFrameInfo)
            End If

            iRet = CameraReleaseImageBuffer(m_hCamera2, pRawBuffer) '成功调用CameraGetImageBuffer后必须调用CameraReleaseImageBuffer释放buffer

            m_iDisplayCounts2 = m_iDisplayCounts2 + 1
        End If

        CaptureImage2 = CAMERA_STATUS_SUCCESS
    End Function

        '初始化相机1
    Public Function InitCamera1(ByVal hDisplayWnd As Integer, ByVal iDisWidth As Integer, ByVal iDisHeight As Integer) As Integer
        Dim iRet As Integer
        Dim sCameraInfo As tSdkCameraCapbility

        m_iDisplayCounts1 = 0

        'iRet = CameraInitEx2("camera1", m_hCamera1) '按名字打开相机
        iRet = CameraInitEx(0, -1, -1, m_hCamera1) '按序号打开

        If iRet <> CAMERA_STATUS_SUCCESS Then
            Return iRet
        End If

        '获得该相机的特性描述,申请一块内存用来进行图像处理、显示

        iRet = CameraGetCapability(m_hCamera1, sCameraInfo)

        m_hBufferHandle1 = GlobalAlloc(2, sCameraInfo.sResolutionRange.iWidthMax * sCameraInfo.sResolutionRange.iHeightMax * 3)
        m_pFrameBuffer1 = GlobalLock(m_hBufferHandle1)

        If sCameraInfo.sIspCapacity.bMonoSensor <> 0 Then
            CameraSetIspOutFormat(m_hCamera1, CAMERA_MEDIA_TYPE_MONO8)
        End If

        '初始化显示模块
        iRet = CameraDisplayInit(m_hCamera1, hDisplayWnd)

        If iRet <> CAMERA_STATUS_SUCCESS Then
            Return iRet
        End If

        iRet = CameraSetDisplaySize(m_hCamera1, iDisWidth, iDisHeight)

        If iRet <> CAMERA_STATUS_SUCCESS Then
            Return iRet
        End If


        '建立设备配置窗口
        iRet = CameraCreateSettingPage(m_hCamera1, Form1.Handle, StringToBytes("Camera1")(0), 0, &H20121111, 0) '&H20121111在CameraConfigWinMsgCallBack回调时被传回，演示如何传递自定义参数

        If iRet <> CAMERA_STATUS_SUCCESS Then
            Return iRet
        End If

        '开始捕获
        iRet = CameraPlay(m_hCamera1)

        Return iRet

    End Function
    '初始化相机2
    Public Function InitCamera2(ByVal hDisplayWnd As Integer, ByVal iDisWidth As Integer, ByVal iDisHeight As Integer) As Integer
        Dim iRet As Integer
        Dim sCameraInfo As tSdkCameraCapbility

        m_iDisplayCounts2 = 0


        'iRet = CameraInitEx2("camera2", m_hCamera2) '按名字打开相机
        iRet = CameraInitEx(1, -1, -1, m_hCamera2) '按序号打开

        If iRet <> CAMERA_STATUS_SUCCESS Then
            Return iRet
        End If

        '获得该相机的特性描述,申请一块内存用来进行图像处理、显示

        iRet = CameraGetCapability(m_hCamera2, sCameraInfo)

        m_hBufferHandle2 = GlobalAlloc(2, sCameraInfo.sResolutionRange.iWidthMax * sCameraInfo.sResolutionRange.iHeightMax * 3)
        m_pFrameBuffer2 = GlobalLock(m_hBufferHandle2)

        If sCameraInfo.sIspCapacity.bMonoSensor <> 0 Then
            CameraSetIspOutFormat(m_hCamera2, CAMERA_MEDIA_TYPE_MONO8)
        End If

        '初始化显示模块
        iRet = CameraDisplayInit(m_hCamera2, hDisplayWnd)

        If iRet <> CAMERA_STATUS_SUCCESS Then
            Return iRet
        End If

        iRet = CameraSetDisplaySize(m_hCamera2, iDisWidth, iDisHeight)

        If iRet <> CAMERA_STATUS_SUCCESS Then
            Return iRet
        End If


        '建立设备配置窗口
        iRet = CameraCreateSettingPage(m_hCamera2, Form1.Handle, StringToBytes("Camera2")(0), 0, &H20121111, 0) '&H20121111在CameraConfigWinMsgCallBack回调时被传回，演示如何传递自定义参数

        If iRet <> CAMERA_STATUS_SUCCESS Then
            Return iRet
        End If

        '开始捕获
        iRet = CameraPlay(m_hCamera2)

        Return iRet

    End Function

End Module
