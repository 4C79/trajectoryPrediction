VERSION 5.00
Begin VB.Form Form1 
   Caption         =   "Camera VB Demo[Basic]"
   ClientHeight    =   8880
   ClientLeft      =   120
   ClientTop       =   450
   ClientWidth     =   11235
   Icon            =   "Form1.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   592
   ScaleMode       =   3  'Pixel
   ScaleWidth      =   749
   StartUpPosition =   3  '窗口缺省
   Begin VB.CommandButton Command1 
      Caption         =   "SW trigger"
      Height          =   255
      Left            =   4200
      TabIndex        =   3
      Top             =   600
      Width           =   1695
   End
   Begin VB.Timer InfoUpdateTimer 
      Enabled         =   0   'False
      Interval        =   1000
      Left            =   10560
      Top             =   600
   End
   Begin VB.Timer GrabTimer 
      Enabled         =   0   'False
      Interval        =   25
      Left            =   10080
      Top             =   600
   End
   Begin VB.PictureBox ImageDisplayer 
      Height          =   5655
      Left            =   240
      ScaleHeight     =   373
      ScaleMode       =   3  'Pixel
      ScaleWidth      =   717
      TabIndex        =   1
      Top             =   2400
      Width           =   10815
   End
   Begin VB.CommandButton CommandSetCamera 
      Caption         =   "Settings"
      Height          =   375
      Index           =   1
      Left            =   240
      TabIndex        =   0
      Top             =   120
      Width           =   1215
   End
   Begin VB.Frame Frame1 
      Caption         =   "Trigger Mode"
      Height          =   1215
      Left            =   1560
      TabIndex        =   4
      Top             =   0
      Width           =   4575
      Begin VB.OptionButton Option1 
         Caption         =   "Continuous mode"
         Height          =   180
         Index           =   0
         Left            =   120
         TabIndex        =   10
         Top             =   360
         Value           =   -1  'True
         Width           =   1695
      End
      Begin VB.OptionButton Option1 
         Caption         =   "Software trigger Mode"
         Height          =   180
         Index           =   1
         Left            =   120
         TabIndex        =   9
         Top             =   600
         Width           =   2415
      End
      Begin VB.OptionButton Option1 
         Caption         =   "External trigger Mode"
         Height          =   180
         Index           =   2
         Left            =   120
         TabIndex        =   8
         Top             =   840
         Width           =   2415
      End
   End
   Begin VB.Frame Frame2 
      Caption         =   "Strobe signal mode"
      Height          =   1215
      Left            =   6480
      TabIndex        =   5
      Top             =   0
      Width           =   4575
      Begin VB.OptionButton Option2 
         Caption         =   "Auto Strobe Mode"
         Height          =   255
         Index           =   0
         Left            =   240
         TabIndex        =   16
         Top             =   480
         Value           =   -1  'True
         Width           =   2175
      End
      Begin VB.OptionButton Option2 
         Caption         =   "Manual Mode"
         Height          =   255
         Index           =   1
         Left            =   240
         TabIndex        =   15
         Top             =   720
         Width           =   1335
      End
   End
   Begin VB.Frame Frame3 
      Caption         =   "External trigger signal type"
      Height          =   975
      Left            =   1560
      TabIndex        =   6
      Top             =   1320
      Width           =   4575
      Begin VB.OptionButton Option4 
         Caption         =   "Trailing eage"
         Height          =   255
         Index           =   1
         Left            =   1920
         TabIndex        =   14
         Top             =   240
         Width           =   1575
      End
      Begin VB.OptionButton Option4 
         Caption         =   "High level"
         Height          =   255
         Index           =   2
         Left            =   240
         TabIndex        =   13
         Top             =   480
         Width           =   1575
      End
      Begin VB.OptionButton Option4 
         Caption         =   "Low level"
         Height          =   255
         Index           =   3
         Left            =   1920
         TabIndex        =   12
         Top             =   480
         Width           =   1575
      End
      Begin VB.OptionButton Option4 
         Caption         =   "Leading eage"
         Height          =   255
         Index           =   0
         Left            =   240
         TabIndex        =   11
         Top             =   240
         Value           =   -1  'True
         Width           =   1575
      End
   End
   Begin VB.Frame Frame4 
      Caption         =   "Strobe manual mode palority"
      Height          =   975
      Left            =   6480
      TabIndex        =   7
      Top             =   1320
      Width           =   3015
      Begin VB.OptionButton Option3 
         Caption         =   "Active Low"
         Height          =   255
         Index           =   1
         Left            =   120
         TabIndex        =   18
         Top             =   480
         Width           =   1935
      End
      Begin VB.OptionButton Option3 
         Caption         =   "Active High"
         Height          =   255
         Index           =   0
         Left            =   120
         TabIndex        =   17
         Top             =   240
         Value           =   -1  'True
         Width           =   1935
      End
   End
   Begin VB.Label StatusLabel 
      Caption         =   "Video Information"
      Height          =   375
      Left            =   240
      TabIndex        =   2
      Top             =   8280
      Width           =   10215
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

Private Sub Command1_Click() '发送一个软触发消息，执行一次软触发
Dim iRet As Long
iRet = CameraSoftTrigger(m_hCamera)
End Sub

Private Sub CommandSetCamera_Click(index As Integer) '显示相机配置窗口
Dim iRet As Long
iRet = CameraShowSettingPage(m_hCamera, 1)

End Sub

Private Sub CommandSnapShot_Click(index As Integer)
Dim iRet As Long
    iRet = Snapshot()
End Sub

Private Sub Form_Load()
Dim iCameraNum As Long
Dim CameraList(10) As tSdkCameraDevInfo
Dim iRet As Integer
Dim iMode As Long


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

    ImageDisplayer.ScaleMode = 3 '获得像素宽高
    
    iRet = InitCamera(CameraList(0), ImageDisplayer.hWnd, ImageDisplayer.Width, ImageDisplayer.Height)
    
    
   
If iRet <> 0 Then
            m_hCamera = 0
            MsgBox ("相机初始化失败")
            Unload Me
            Exit Sub
Else
    GrabTimer.Enabled = True
    InfoUpdateTimer.Enabled = True
    '读取相机当前触发和闪光灯参数，更新界面单选按钮
    iRet = CameraGetTriggerMode(m_hCamera, iMode)
    Option1.Item(iMode).Value = True
     iRet = CameraGetExtTrigSignalType(m_hCamera, iMode)
    Option4.Item(iMode).Value = True
    iRet = CameraGetStrobeMode(m_hCamera, iMode)
    Option2.Item(iMode).Value = True
    iRet = CameraGetStrobePolarity(m_hCamera, iMode)
    Option3.Item(iMode).Value = True
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

'由于VB6不能很好的支持多线程编程，例程中使用了定时器的方式，来主动抓取图像。如果您使用的是VB.NET，则可以使用回调函数或者建立采集线程主动抓取的方式。
Private Sub GrabTimer_Timer()
Dim iRet As Long
    iRet = CaptureImage()
End Sub
'一秒刷新一次图像信息
Private Sub InfoUpdateTimer_Timer()
Dim sFrameInfo As String
    sFrameInfo = "| Resolution " + Format$(m_iLastImageWidth, "General Number") + " X " + Format$(m_iLastImageHeight, "General Number") + " | Display frames " + Format$(m_iDisplayCounts, "General Number") + "|"
    StatusLabel.Caption = sFrameInfo
    
End Sub

'///BIG5 TRANS ALLOWED

Private Sub Option1_Click(index As Integer) '设置相机工作模式，0为连续预览模式，1为软触发模式，2为硬件触发模式
Dim iRet As Long
iRet = CameraSetTriggerMode(m_hCamera, index)

End Sub

Private Sub Option2_Click(index As Integer) '设置相机的闪光灯输出模式，0为自动模式，1为手动模式。自动模式为相机自动产生闪光灯同步信号，手动模式则可以手动调节闪光灯信号的延时和脉冲宽度。
Dim iRet As Long
iRet = CameraSetStrobeMode(m_hCamera, index)
End Sub

Private Sub Option3_Click(index As Integer) '设置手动闪光灯模式下，闪光灯信号的有效极性。
Dim iRet As Long
iRet = CameraSetStrobePolarity(m_hCamera, index)
End Sub

Private Sub Option4_Click(index As Integer) '设置相机的外触发触发信号的种类，0为上升沿触发，1为下降沿触发，2为高电平触发，3为低电平触发。电平触发模式有消抖功能，通过相关CameraSetExtTrigJitterTime可以设置去抖时间
Dim iRet As Long
iRet = CameraSetExtTrigSignalType(m_hCamera, index)
End Sub
