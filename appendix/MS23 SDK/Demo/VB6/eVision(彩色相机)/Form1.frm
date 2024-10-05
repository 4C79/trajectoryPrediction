VERSION 5.00
Object = "{9AE86E03-97BC-11D1-A49B-000021633168}#6.7#0"; "eVision.ocx"
Begin VB.Form Form1 
   Caption         =   "Camera VB Demo[Basic]"
   ClientHeight    =   8880
   ClientLeft      =   120
   ClientTop       =   450
   ClientWidth     =   12210
   Icon            =   "Form1.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   592
   ScaleMode       =   3  'Pixel
   ScaleWidth      =   814
   StartUpPosition =   3  '窗口缺省
   Begin eVision.EC24Image EC24Image1 
      Height          =   6975
      Left            =   360
      TabIndex        =   4
      Top             =   840
      Width           =   11415
      _Version        =   393223
      _ExtentX        =   20135
      _ExtentY        =   12303
      _StockProps     =   0
      BeginProperty TextFont {0BE35203-8F91-11CE-9DE3-00AA004BB851} 
         Name            =   "System"
         Size            =   9.75
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
   End
   Begin VB.Timer InfoUpdateTimer 
      Enabled         =   0   'False
      Interval        =   1000
      Left            =   6600
      Top             =   240
   End
   Begin VB.Timer GrabTimer 
      Enabled         =   0   'False
      Interval        =   25
      Left            =   6000
      Top             =   240
   End
   Begin VB.CommandButton ButtonPlay 
      Caption         =   "Play"
      Height          =   375
      Index           =   1
      Left            =   1560
      TabIndex        =   3
      Top             =   240
      Width           =   1215
   End
   Begin VB.CommandButton CommandSetCamera 
      Caption         =   "Settings"
      Height          =   375
      Index           =   1
      Left            =   240
      TabIndex        =   1
      Top             =   240
      Width           =   1215
   End
   Begin VB.CommandButton ButtonPause 
      Caption         =   "Pause"
      Height          =   375
      Index           =   0
      Left            =   2880
      TabIndex        =   0
      Top             =   240
      Width           =   1215
   End
   Begin VB.Label StatusLabel 
      Caption         =   "Video Information"
      Height          =   375
      Left            =   240
      TabIndex        =   2
      Top             =   8280
      Width           =   11655
   End
End
Attribute VB_Name = "Form1"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Private Sub MindVisionCameraApiLoad()

End Sub

Private Sub ButtonPause_Click(index As Integer)
    CameraPause (m_hCamera)
End Sub

Private Sub ButtonPlay_Click(index As Integer)
    CameraPlay (m_hCamera)
End Sub

Private Sub CommandSetCamera_Click(index As Integer) '显示相机配置窗口
Dim iRet As Long
iRet = CameraShowSettingPage(m_hCamera, 1)

End Sub



Private Sub Form_Load()
Dim iCameraNum As Long
Dim CameraList(10) As tSdkCameraDevInfo
Dim iRet As Integer



'//枚举设备，获得设备列表
iCameraNum = 10 '调用CameraEnumerateDevice前，先设置iCameraNums = 10，表示最多只读取10个设备，如果需要枚举更多的设备，请更改sCameraList数组的大小和iCameraNums的值
iRet = CameraEnumerateDevice(CameraList(0), iCameraNum)

If iCameraNum < 1 Then
            m_hCamera = 0
            MsgBox ("没有扫描到相机，请插上相机后再次运行本程序")
            Unload Me
            Exit Sub
Else
    
End If
    iRet = InitCameraForEvision(CameraList(0)) '初始化第一个相机。如果第2个相机，用 CameraList(1)

If iRet <> 0 Then
    m_hCamera = 0
    MsgBox ("相机初始化失败")
    Unload Me
    Exit Sub
Else
    'iRet = CameraSetIspOutFormat(m_hCamera, CAMERA_MEDIA_TYPE_MONO8) '彩色相机不要执行这句，默认是输出24位的彩色格式的。黑白相机就把这里激活。
    GrabTimer.Enabled = True
    InfoUpdateTimer.Enabled = True
End If

End Sub

Private Sub Form_Unload(Cancel As Integer)

If m_hCamera <> 0 Then
           GrabTimer.Enabled = False
           InfoUpdateTimer.Enabled = False
           CameraUnInit (m_hCamera)         '反初始化相机
           CameraAlignFree (m_pFrameBuffer)
End If

End Sub


'捕获一帧图像并显示
Public Function CaptureImageForEvision() As Long
 
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
        
     
        If iRet = CAMERA_STATUS_SUCCESS Then '处理成功，则显示该图像，这里使用了SDK封装的接口进行图像显示，您也可以自己实现图像的显示，或者直接在这里调用保存图像的接口进行保存
           ' iRet = CameraDisplayRGB24(m_hCamera, m_pFrameBuffer, sFrameInfo)
           EC24Image1.SetImagePointer m_pFrameBuffer
           EC24Image1.SetSize CLng(sFrameInfo.iWidth), CLng(sFrameInfo.iHeight)
           EC24Image1.ZoomX = CSng(EC24Image1.Width / sFrameInfo.iWidth)
           EC24Image1.ZoomY = CSng(EC24Image1.Height / sFrameInfo.iHeight)
           
           
           
           EC24Image1.Refresh
           
           
        End If
        
        iRet = CameraReleaseImageBuffer(m_hCamera, pRawBuffer) '成功调用CameraGetImageBuffer后必须调用CameraReleaseImageBuffer释放buffer
        
        
    End If
    
   
    
    m_iDisplayCounts = m_iDisplayCounts + 1
    CaptureImageForEvision = CAMERA_STATUS_SUCCESS
End Function


'由于VB6不能很好的支持多线程编程，例程中使用了定时器的方式，来主动抓取图像。如果您使用的是VB.NET，则可以使用回调函数或者建立采集线程主动抓取的方式。
Private Sub GrabTimer_Timer()
Dim iRet As Long
    iRet = CaptureImageForEvision()
End Sub
'一秒刷新一次图像信息
Private Sub InfoUpdateTimer_Timer()
Dim sFrameInfo As String
    sFrameInfo = "| Resolution " + Format$(m_iLastImageWidth, "General Number") + " X " + Format$(m_iLastImageHeight, "General Number") + " | Display frames " + Format$(m_iDisplayCounts, "General Number") + "|"
    StatusLabel.Caption = sFrameInfo
    
End Sub

'///BIG5 TRANS ALLOWED

