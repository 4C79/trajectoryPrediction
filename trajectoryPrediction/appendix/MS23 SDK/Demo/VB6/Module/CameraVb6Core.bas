Attribute VB_Name = "CameraVbCore"
'///BIG5 TRANS ALLOWED


Public m_hCamera As Long        '相机句柄
Public m_hCamera2 As Long        '相机句柄
Public m_pFrameBuffer As Long   '显示缓冲区首地址
Public m_pFrameBuffer2 As Long   '显示缓冲区首地址
Public m_iLastImageWidth As Long
Public m_iLastImageHeight As Long
Public m_iDisplayCounts As Long '记录当前显示的总帧数
'实现类似malloc的操作，用于动态申请内存
Public Declare Function GlobalAlloc Lib "kernel32" (ByVal wFlags As Long, ByVal dwBytes As Long) As Long
Public Declare Function GlobalLock Lib "kernel32" (ByVal hMem As Long) As Long
Public Declare Function GlobalUnlock Lib "kernel32" (ByVal hMem As Long) As Long
Public Declare Function GlobalFree Lib "kernel32" (ByVal hMem As Long) As Long


'捕获一帧图像并显示
Public Function CaptureImage() As Long
 
Dim sFrameInfo As tSdkFrameHead
Dim pRawBuffer As Long          '原始数据的缓冲区地址，从SDK中获得
Dim iRet As Long
Dim Image As Long
Dim Pic As IPictureDisp
            
     '捕获并显示相机1图像
    iRet = CameraGetImageBuffer(m_hCamera, sFrameInfo, pRawBuffer, 200) '抓取一帧图像，超时时间为200毫秒
    
    
    If iRet = CAMERA_STATUS_SUCCESS Then
       
        
        '将原始数据转换为RGB格式，同时叠加图像处理的效果
        iRet = CameraImageProcess(m_hCamera, pRawBuffer, m_pFrameBuffer, sFrameInfo)
        
         If (m_iLastImageWidth <> sFrameInfo.iWidth) Or (m_iLastImageHeight <> sFrameInfo.iHeight) Then
            m_iLastImageWidth = sFrameInfo.iWidth
            m_iLastImageHeight = sFrameInfo.iHeight
            Form1.ImageDisplayer.Refresh
        End If
        
        If iRet = CAMERA_STATUS_SUCCESS Then '处理成功，则显示该图像，这里使用了SDK封装的接口进行图像显示，您也可以自己实现图像的显示，或者直接在这里调用保存图像的接口进行保存
            iRet = CameraDisplayRGB24(m_hCamera, m_pFrameBuffer, sFrameInfo)
        End If
        
        iRet = CameraReleaseImageBuffer(m_hCamera, pRawBuffer) '成功调用CameraGetImageBuffer后必须调用CameraReleaseImageBuffer释放buffer
        
        
    End If
    
    
     '捕获并显示相机2图像
    iRet = CameraGetImageBuffer(m_hCamera2, sFrameInfo, pRawBuffer, 200) '抓取一帧图像，超时时间为200毫秒
    
    
    If iRet = CAMERA_STATUS_SUCCESS Then
       
        
        '将原始数据转换为RGB格式，同时叠加图像处理的效果
        iRet = CameraImageProcess(m_hCamera2, pRawBuffer, m_pFrameBuffer2, sFrameInfo)
        
        
        If iRet = CAMERA_STATUS_SUCCESS Then '处理成功，则显示该图像，这里使用了SDK封装的接口进行图像显示，您也可以自己实现图像的显示，或者直接在这里调用保存图像的接口进行保存
            iRet = CameraDisplayRGB24(m_hCamera2, m_pFrameBuffer2, sFrameInfo)
        End If
        
        iRet = CameraReleaseImageBuffer(m_hCamera2, pRawBuffer) '成功调用CameraGetImageBuffer后必须调用CameraReleaseImageBuffer释放buffer
        
        
    End If
    
    m_iDisplayCounts = m_iDisplayCounts + 1
    CaptureImage = CAMERA_STATUS_SUCCESS
End Function
'用抓拍模式捕获一帧图像并保存到文件中
Public Function Snapshot() As Long
    Dim FrameInfo As tSdkFrameHead
    Dim pRawBuffer As Long
    Dim pRgbBuffer As Long
    Dim pBufferHandle As Long
    Dim sFileName As String
    Dim sImageSize As tSdkImageResolution
    Dim iRet As Long
    Dim msg As String
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
        MsgBox ("Snapshot failed,is camera in pause mode?")
    Else
        '//成功抓拍后，保存到文件
        
        sFileName = App.Path + "\" + Format$(Now, "yyyy-mm-dd-h-mm-ss") + ".BMP" '//初始化保存文件的文件名
 
        '//申请一个buffer，用来将获得的原始数据转换为RGB数据，并同时获得图像处理效果
        pRgbBuffer = CameraAlignMalloc(FrameInfo.iWidth * FrameInfo.iHeight * 4, 16)
       
        
        '//CameraImageProcess得到RGB格式的数据,如果需要保存RAW数据，则直接调用CameraSaveImage，文件类型选择RAW格式即可
        iRet = CameraImageProcess(m_hCamera, pRawBuffer, pRgbBuffer, FrameInfo)
        
        
        '将VB的string类型转换为C的字符串，并且在字符串末尾添加结束符
        sTmp = StrConv(sFileName, vbFromUnicode)
        ReDim Preserve sTmp(UBound(sTmp) + 1)
        sTmp(UBound(sTmp)) = 0
        
        iRet = CameraSaveImage(m_hCamera, sTmp(0), pRgbBuffer, FrameInfo, 2, 100) '1:JPG 2:BMP 4:RAW 8:PNG
        
        If iRet = CAMERA_STATUS_SUCCESS Then
            MsgBox ("Save image OK! The file is:[" + sFileName + "]OK!")
        Else
            MsgBox ("Save image failed")
        End If
        
        iRet = CameraReleaseImageBuffer(m_hCamera, pRawBuffer)
        
        CameraAlignFree (pRgbBuffer)
        
        GlobalUnlock (pBufferHandle)   '释放申请的内存块
        GlobalFree (pBufferHandle)
        
    End If
End Function



'相机配置窗口的消息回调函数，当通过配置窗口切换参数组时，会触发相应的消息，并通过此回调此函数来传递消息，您可以在这个回调函数中，与界面进行同步。
'hCamera: 当前相机的句柄
'MSG:消息类型，
   ' SHEET_MSG_LOAD_PARAM_DEFAULT    = 0,//加载默认参数的按钮被点击，加载默认参数完成后触发该消息,
   ' SHEET_MSG_LOAD_PARAM_GROUP      = 1,//切换参数组完成后触发该消息,
   ' SHEET_MSG_LOAD_PARAM_FROMFILE   = 2,//加载参数按钮被点击，已从文件中加载相机参数后触发该消息
   ' SHEET_MSG_SAVE_PARAM_GROUP      = 3//保存参数按钮被点击，参数保存后触发该消息
   ' 具体参见CameraDefine.bas中emSdkPropSheetMsg类型

'uParam:消息附带的参数，不同的消息，参数意义不同。
   ' 当 MSG 为 SHEET_MSG_LOAD_PARAM_DEFAULT时，uParam表示被加载成默认参数组的索引号，从0开始，分别对应A,B,C,D四组
   ' 当 MSG 为 SHEET_MSG_LOAD_PARAM_GROUP时，uParam表示切换后的参数组的索引号，从0开始，分别对应A,B,C,D四组
   ' 当 MSG 为 SHEET_MSG_LOAD_PARAM_FROMFILE时，uParam表示被文件中参数覆盖的参数组的索引号，从0开始，分别对应A,B,C,D四组
   ' 当 MSG 为 SHEET_MSG_SAVE_PARAM_GROUP时，uParam表示当前保存的参数组的索引号，从0开始，分别对应A,B,C,D四组
Public Sub CameraConfigWinMsgCallBack(ByVal hCamera As Long, ByVal msg As Long, ByVal uParam As Long, ByVal pContext As Long)
    
   
End Sub

'初始化相机
Public Function InitCamera(ByRef pCameraInfo As tSdkCameraDevInfo, ByVal hDisplayWnd As Long, ByVal iDisWidth As Long, ByVal iDisHeight As Long) As Integer
Dim iRet As Long
Dim sCameraInfo As tSdkCameraCapbility

m_iDisplayCounts = 0

iRet = CameraInit(pCameraInfo, -1, -1, m_hCamera)

If iRet <> CAMERA_STATUS_SUCCESS Then
    InitCamera = iRet
    Exit Function
End If

'获得该相机的特性描述,申请一块内存用来进行图像处理、显示
   
iRet = CameraGetCapability(m_hCamera, sCameraInfo)

m_pFrameBuffer = CameraAlignMalloc(sCameraInfo.sResolutionRange.iWidthMax * sCameraInfo.sResolutionRange.iWidthMax * 4, 16)

If sCameraInfo.sIspCapacity.bMonoSensor <> 0 Then
    iRet = CameraSetIspOutFormat(m_hCamera, CAMERA_MEDIA_TYPE_MONO8)
End If

'初始化显示模块
iRet = CameraDisplayInit(m_hCamera, hDisplayWnd)

If iRet <> CAMERA_STATUS_SUCCESS Then
    InitCamera = iRet
    Exit Function
End If

iRet = CameraSetDisplaySize(m_hCamera, iDisWidth, iDisHeight)

If iRet <> CAMERA_STATUS_SUCCESS Then
    InitCamera = iRet
    Exit Function
End If


'建立设备配置窗口
iRet = CameraCreateSettingPage(m_hCamera, Form1.hWnd, pCameraInfo.acFriendlyName(0), AddressOf CameraConfigWinMsgCallBack, &H20121111, 0) '&H20121111在CameraConfigWinMsgCallBack回调时被传回，演示如何传递自定义参数

If iRet <> CAMERA_STATUS_SUCCESS Then
    InitCamera = iRet
    Exit Function
End If


'开始捕获
iRet = CameraPlay(m_hCamera)
If iRet <> CAMERA_STATUS_SUCCESS Then
    InitCamera = iRet
    Exit Function
End If




InitCamera = iRet
End Function


'初始化相机
Public Function InitCameraForEvision(ByRef pCameraInfo As tSdkCameraDevInfo) As Integer
Dim iRet As Long
Dim sCameraInfo As tSdkCameraCapbility

m_iDisplayCounts = 0

iRet = CameraInit(pCameraInfo, -1, -1, m_hCamera)

If iRet <> CAMERA_STATUS_SUCCESS Then
    InitCameraForEvision = iRet
    Exit Function
End If

'获得该相机的特性描述,申请一块内存用来进行图像处理、显示
   
iRet = CameraGetCapability(m_hCamera, sCameraInfo)

m_pFrameBuffer = CameraAlignMalloc(sCameraInfo.sResolutionRange.iWidthMax * sCameraInfo.sResolutionRange.iWidthMax * 4, 16)




'建立设备配置窗口
iRet = CameraCreateSettingPage(m_hCamera, Form1.hWnd, pCameraInfo.acFriendlyName(0), AddressOf CameraConfigWinMsgCallBack, &H20121111, 0) '&H20121111在CameraConfigWinMsgCallBack回调时被传回，演示如何传递自定义参数

If iRet <> CAMERA_STATUS_SUCCESS Then
    InitCameraForEvision = iRet
    Exit Function
End If

'设置垂直翻转，evision的显示控件上下是反的
iRet = CameraSetMirror(m_hCamera, 1, 1)

'开始捕获
iRet = CameraPlay(m_hCamera)
If iRet <> CAMERA_STATUS_SUCCESS Then
    InitCameraForEvision = iRet
    Exit Function
End If




InitCameraForEvision = iRet
End Function


'根据相机名字，初始化相机1
Public Function InitCamera1ByNames(ByRef name As Byte, ByVal hDisplayWnd As Long, ByVal iDisWidth As Long, ByVal iDisHeight As Long) As Integer
Dim iRet As Long
Dim sCameraInfo As tSdkCameraCapbility

m_iDisplayCounts = 0

iRet = CameraInitEx2(name, m_hCamera)

If iRet <> CAMERA_STATUS_SUCCESS Then
    InitCamera1ByNames = iRet
    Exit Function
End If

'获得该相机的特性描述,申请一块内存用来进行图像处理、显示
   
iRet = CameraGetCapability(m_hCamera, sCameraInfo)

m_pFrameBuffer = CameraAlignMalloc(sCameraInfo.sResolutionRange.iWidthMax * sCameraInfo.sResolutionRange.iWidthMax * 4, 16)

If sCameraInfo.sIspCapacity.bMonoSensor <> 0 Then
    iRet = CameraSetIspOutFormat(m_hCamera, CAMERA_MEDIA_TYPE_MONO8)
End If

'初始化显示模块
iRet = CameraDisplayInit(m_hCamera, hDisplayWnd)

If iRet <> CAMERA_STATUS_SUCCESS Then
    InitCamera1ByNames = iRet
    Exit Function
End If

iRet = CameraSetDisplaySize(m_hCamera, iDisWidth, iDisHeight)

If iRet <> CAMERA_STATUS_SUCCESS Then
    InitCamera1ByNames = iRet
    Exit Function
End If


'建立设备配置窗口
iRet = CameraCreateSettingPage(m_hCamera, Form1.hWnd, name, AddressOf CameraConfigWinMsgCallBack, &H20121111, 0) '&H20121111在CameraConfigWinMsgCallBack回调时被传回，演示如何传递自定义参数

If iRet <> CAMERA_STATUS_SUCCESS Then
    InitCamera1ByNames = iRet
    Exit Function
End If


'开始捕获
iRet = CameraPlay(m_hCamera)
If iRet <> CAMERA_STATUS_SUCCESS Then
    InitCamera1ByNames = iRet
    Exit Function
End If




InitCamera1ByNames = iRet
End Function

'根据相机名字，初始化相机2
Public Function InitCamera2ByNames(ByRef name As Byte, ByVal hDisplayWnd As Long, ByVal iDisWidth As Long, ByVal iDisHeight As Long) As Integer
Dim iRet As Long
Dim sCameraInfo As tSdkCameraCapbility

m_iDisplayCounts = 0

iRet = CameraInitEx2(name, m_hCamera2)

If iRet <> CAMERA_STATUS_SUCCESS Then
    InitCamera2ByNames = iRet
    Exit Function
End If

'获得该相机的特性描述,申请一块内存用来进行图像处理、显示
   
iRet = CameraGetCapability(m_hCamera2, sCameraInfo)

m_pFrameBuffer2 = CameraAlignMalloc(sCameraInfo.sResolutionRange.iWidthMax * sCameraInfo.sResolutionRange.iWidthMax * 4, 16)

If sCameraInfo.sIspCapacity.bMonoSensor <> 0 Then
    iRet = CameraSetIspOutFormat(m_hCamera2, CAMERA_MEDIA_TYPE_MONO8)
End If

'初始化显示模块
iRet = CameraDisplayInit(m_hCamera2, hDisplayWnd)

If iRet <> CAMERA_STATUS_SUCCESS Then
    InitCamera2ByNames = iRet
    Exit Function
End If

iRet = CameraSetDisplaySize(m_hCamera2, iDisWidth, iDisHeight)

If iRet <> CAMERA_STATUS_SUCCESS Then
    InitCamera2ByNames = iRet
    Exit Function
End If


'建立设备配置窗口
iRet = CameraCreateSettingPage(m_hCamera2, Form1.hWnd, name, AddressOf CameraConfigWinMsgCallBack, &H20121111, 0) '&H20121111在CameraConfigWinMsgCallBack回调时被传回，演示如何传递自定义参数

If iRet <> CAMERA_STATUS_SUCCESS Then
    InitCamera2ByNames = iRet
    Exit Function
End If


'开始捕获
iRet = CameraPlay(m_hCamera2)
If iRet <> CAMERA_STATUS_SUCCESS Then
    InitCamera2ByNames = iRet
    Exit Function
End If




InitCamera2ByNames = iRet
End Function
