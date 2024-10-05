Imports System.IO
'BIG5 TRANS ALLOWED
Public Module CameraVbCore

    Public m_hCamera As Integer        '相机句柄
    Public m_hBufferHandle As Integer  '显示缓冲区句柄
    Public m_pFrameBuffer As Integer   '显示缓冲区首地址
    Public m_iLastImageWidth As Integer
    Public m_iLastImageHeight As Integer
    Public m_iDisplayCounts As Integer '记录当前显示的总帧数
    Public m_CapCallback As CAMERA_SNAP_PROC
    '实现类似malloc的操作，用于动态申请内存
    Public Declare Function GlobalAlloc Lib "kernel32" (ByVal wFlags As Integer, ByVal dwBytes As Integer) As Integer
    Public Declare Function GlobalLock Lib "kernel32" (ByVal hMem As Integer) As Integer
    Public Declare Function GlobalUnlock Lib "kernel32" (ByVal hMem As Integer) As Integer
    Public Declare Function GlobalFree Lib "kernel32" (ByVal hMem As Integer) As Integer

    '捕获一帧图像并显示
    Public Function CaptureImage() As Integer

        Dim sFrameInfo As tSdkFrameHead
        Dim pRawBuffer As Integer          '原始数据的缓冲区地址，从SDK中获得
        Dim iRet As Integer

        iRet = CameraGetImageBuffer(m_hCamera, sFrameInfo, pRawBuffer, 200) '抓取一帧图像，超时时间为200毫秒


        If iRet = CAMERA_STATUS_SUCCESS Then
            If (m_iLastImageWidth <> sFrameInfo.iWidth) Or (m_iLastImageHeight <> sFrameInfo.iHeight) Then
                m_iLastImageWidth = sFrameInfo.iWidth
                m_iLastImageHeight = sFrameInfo.iHeight
                Form1.PictureBox1.Refresh()
            End If

            '将原始数据转换为RGB格式，同时叠加图像处理的效果
            iRet = CameraImageProcess(m_hCamera, pRawBuffer, m_pFrameBuffer, sFrameInfo)

            If iRet = CAMERA_STATUS_SUCCESS Then '处理成功，则显示该图像，这里使用了SDK封装的接口进行图像显示，您也可以自己实现图像的显示，或者直接在这里调用保存图像的接口进行保存
                iRet = CameraDisplayRGB24(m_hCamera, m_pFrameBuffer, sFrameInfo)
            End If

            iRet = CameraReleaseImageBuffer(m_hCamera, pRawBuffer) '成功调用CameraGetImageBuffer后必须调用CameraReleaseImageBuffer释放buffer

            m_iDisplayCounts = m_iDisplayCounts + 1
        End If

        CaptureImage = CAMERA_STATUS_SUCCESS
    End Function

    Public Sub CaptureImageCallbackFunc(ByVal hCamera As Integer, _
            ByVal pRawBuffer As Integer, _
            ByRef sFrameInfo As tSdkFrameHead, _
            ByVal Context As Integer)

        Dim iRet As Integer

        If (m_iLastImageWidth <> sFrameInfo.iWidth) Or (m_iLastImageHeight <> sFrameInfo.iHeight) Then
            m_iLastImageWidth = sFrameInfo.iWidth
            m_iLastImageHeight = sFrameInfo.iHeight
            Form1.PictureBox1.Refresh()
        End If

        '将原始数据转换为RGB格式，同时叠加图像处理的效果
        iRet = CameraImageProcess(m_hCamera, pRawBuffer, m_pFrameBuffer, sFrameInfo)

        If iRet = CAMERA_STATUS_SUCCESS Then '处理成功，则显示该图像，这里使用了SDK封装的接口进行图像显示，您也可以自己实现图像的显示，或者直接在这里调用保存图像的接口进行保存
            iRet = CameraDisplayRGB24(m_hCamera, m_pFrameBuffer, sFrameInfo)
        End If

        m_iDisplayCounts = m_iDisplayCounts + 1

    End Sub

    '用抓拍模式捕获一帧图像并保存到文件中,也可以直接将CameraGetImageBuffer得到图像进行处理或者保存。
    Public Function Snapshot() As Integer
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

            sFileName = Application.StartupPath() + "\" + Format$(Now, "yyyy-mm-dd-h-mm-ss") + ".BMP" '//初始化保存文件的文件名

            '//申请一个buffer，用来将获得的原始数据转换为RGB数据，并同时获得图像处理效果
            pRgbBuffer = CameraAlignMalloc(FrameInfo.iWidth * FrameInfo.iHeight * 4, 16)

            '//CameraImageProcess得到RGB格式的数据,如果需要保存RAW数据，则直接调用CameraSaveImage，文件类型选择RAW格式即可
            iRet = CameraImageProcess(m_hCamera, pRawBuffer, pRgbBuffer, FrameInfo)


            '将VB的string类型转换为C的字符串，并且在字符串末尾添加结束符
            sTmp = System.Text.Encoding.Default.GetBytes(sFileName)
            sTmp(UBound(sTmp)) = 0

            iRet = CameraSaveImage(m_hCamera, sTmp(0), pRgbBuffer, FrameInfo, 2, 100) '1:JPG 2:BMP 4:RAW 8:PNG

            If iRet = CAMERA_STATUS_SUCCESS Then
                MsgBox("Save image OK! The file is:[" + sFileName + "]OK!")
            Else
                MsgBox("Save image failed")
            End If

            iRet = CameraReleaseImageBuffer(m_hCamera, pRawBuffer)

            CameraAlignFree(pRgbBuffer)

        End If

        Return iRet
    End Function


    '初始化相机
    Public Function InitCamera(ByRef pCameraInfo As tSdkCameraDevInfo, ByVal hDisplayWnd As Integer, ByVal iDisWidth As Integer, ByVal iDisHeight As Integer) As Integer
        Dim iRet As Integer
        Dim sCameraInfo As tSdkCameraCapbility

        m_iDisplayCounts = 0

        iRet = CameraInit(pCameraInfo, -1, -1, m_hCamera)

        If iRet <> CAMERA_STATUS_SUCCESS Then
            Return iRet
        End If

        '获得该相机的特性描述,申请一块内存用来进行图像处理、显示

        iRet = CameraGetCapability(m_hCamera, sCameraInfo)

        m_pFrameBuffer = CameraAlignMalloc(sCameraInfo.sResolutionRange.iWidthMax * sCameraInfo.sResolutionRange.iHeightMax * 4, 16)

        If sCameraInfo.sIspCapacity.bMonoSensor <> 0 Then
            CameraSetIspOutFormat(m_hCamera, CAMERA_MEDIA_TYPE_MONO8)
        End If

        '初始化显示模块
        iRet = CameraDisplayInit(m_hCamera, hDisplayWnd)

        If iRet <> CAMERA_STATUS_SUCCESS Then
            Return iRet
        End If

        iRet = CameraSetDisplaySize(m_hCamera, iDisWidth, iDisHeight)

        If iRet <> CAMERA_STATUS_SUCCESS Then
            Return iRet
        End If


        '建立设备配置窗口
        iRet = CameraCreateSettingPage(m_hCamera, Form1.Handle, pCameraInfo.acFriendlyName(0), 0, &H20121111, 0) '&H20121111在CameraConfigWinMsgCallBack回调时被传回，演示如何传递自定义参数

        If iRet <> CAMERA_STATUS_SUCCESS Then
            Return iRet
        End If

        '开始捕获
        iRet = CameraPlay(m_hCamera)

        Return iRet

    End Function

End Module
