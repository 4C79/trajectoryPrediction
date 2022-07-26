VERSION 5.00
Object = "{5B6AA1D1-A98B-4D0C-8051-18FAADF25516}#1.0#0"; "IndustrialCamera.ocx"
Begin VB.Form Form1 
   Caption         =   "Camera VB6 Demo[OCX]"
   ClientHeight    =   8790
   ClientLeft      =   120
   ClientTop       =   450
   ClientWidth     =   15375
   Icon            =   "Form1.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   586
   ScaleMode       =   3  'Pixel
   ScaleWidth      =   1025
   StartUpPosition =   3  '敦諳�捩�
   Begin VB.Timer Timer2 
      Interval        =   50
      Left            =   6600
      Top             =   240
   End
   Begin VB.CommandButton SW_Triiger_Btn1 
      Caption         =   "Sw Trigger once "
      Height          =   375
      Left            =   12720
      TabIndex        =   40
      Top             =   8040
      Width           =   1935
   End
   Begin VB.Frame Frame10 
      Caption         =   "Trigger Mode"
      Height          =   615
      Left            =   7920
      TabIndex        =   39
      Top             =   7800
      Width           =   4695
      Begin VB.OptionButton Option18 
         Caption         =   "EXT Trigger"
         Height          =   255
         Index           =   2
         Left            =   2880
         TabIndex        =   43
         Top             =   240
         Width           =   1335
      End
      Begin VB.OptionButton Option18 
         Caption         =   "Sw Trigger"
         Height          =   255
         Index           =   1
         Left            =   1560
         TabIndex        =   42
         Top             =   240
         Width           =   1335
      End
      Begin VB.OptionButton Option18 
         Caption         =   "Continuous"
         Height          =   255
         Index           =   0
         Left            =   120
         TabIndex        =   41
         Top             =   240
         Width           =   1335
      End
   End
   Begin VB.CommandButton SW_Triiger_Btn 
      Caption         =   "Sw Trigger once"
      Height          =   375
      Left            =   5040
      TabIndex        =   38
      Top             =   7920
      Width           =   1935
   End
   Begin VB.Timer Timer1 
      Left            =   7200
      Top             =   8280
   End
   Begin VB.Frame Frame8 
      Caption         =   "Input IO0 state"
      Height          =   1095
      Left            =   7920
      TabIndex        =   31
      Top             =   6600
      Width           =   1575
      Begin VB.OptionButton Option16 
         Caption         =   "Low"
         Height          =   375
         Left            =   120
         TabIndex        =   33
         Top             =   240
         Width           =   1095
      End
      Begin VB.OptionButton Option15 
         Caption         =   "High"
         Height          =   255
         Left            =   120
         TabIndex        =   32
         Top             =   600
         Width           =   1095
      End
   End
   Begin VB.Frame Frame7 
      Caption         =   "Input IO1 state"
      Height          =   1095
      Left            =   9600
      TabIndex        =   28
      Top             =   6600
      Width           =   1575
      Begin VB.OptionButton Option14 
         Caption         =   "High"
         Height          =   255
         Left            =   120
         TabIndex        =   30
         Top             =   600
         Width           =   1095
      End
      Begin VB.OptionButton Option13 
         Caption         =   "Low"
         Height          =   375
         Left            =   120
         TabIndex        =   29
         Top             =   240
         Width           =   1095
      End
   End
   Begin VB.Frame Frame6 
      Caption         =   "Output IO0 state"
      Height          =   1095
      Left            =   11280
      TabIndex        =   25
      Top             =   6600
      Width           =   1815
      Begin VB.OptionButton Option12 
         Caption         =   "Low"
         Height          =   375
         Left            =   120
         TabIndex        =   27
         Top             =   240
         Width           =   1095
      End
      Begin VB.OptionButton Option11 
         Caption         =   "High"
         Height          =   255
         Left            =   120
         TabIndex        =   26
         Top             =   600
         Width           =   1095
      End
   End
   Begin VB.Frame Frame5 
      Caption         =   "Output IO1 state"
      Height          =   1095
      Left            =   13200
      TabIndex        =   22
      Top             =   6600
      Width           =   1815
      Begin VB.OptionButton Option10 
         Caption         =   "High"
         Height          =   255
         Left            =   120
         TabIndex        =   24
         Top             =   600
         Width           =   1095
      End
      Begin VB.OptionButton Option9 
         Caption         =   "Low"
         Height          =   375
         Left            =   120
         TabIndex        =   23
         Top             =   240
         Width           =   1095
      End
   End
   Begin VB.Frame Frame4 
      Caption         =   "Output IO1 state"
      Height          =   1095
      Left            =   5520
      TabIndex        =   19
      Top             =   6600
      Width           =   1815
      Begin VB.OptionButton Option8 
         Caption         =   "Low"
         Height          =   375
         Left            =   120
         TabIndex        =   21
         Top             =   240
         Width           =   1095
      End
      Begin VB.OptionButton Option7 
         Caption         =   "High"
         Height          =   255
         Left            =   120
         TabIndex        =   20
         Top             =   600
         Width           =   1095
      End
   End
   Begin VB.Frame Frame3 
      Caption         =   "Output IO0 state"
      Height          =   1095
      Left            =   3600
      TabIndex        =   16
      Top             =   6600
      Width           =   1815
      Begin VB.OptionButton Option6 
         Caption         =   "High"
         Height          =   255
         Left            =   120
         TabIndex        =   18
         Top             =   600
         Width           =   1095
      End
      Begin VB.OptionButton Option5 
         Caption         =   "Low"
         Height          =   375
         Left            =   120
         TabIndex        =   17
         Top             =   240
         Width           =   1095
      End
   End
   Begin VB.Frame Frame2 
      Caption         =   "Input IO1 state"
      Height          =   1095
      Left            =   1920
      TabIndex        =   13
      Top             =   6600
      Width           =   1575
      Begin VB.OptionButton Option4 
         Caption         =   "Low"
         Height          =   375
         Left            =   120
         TabIndex        =   15
         Top             =   240
         Width           =   1095
      End
      Begin VB.OptionButton Option3 
         Caption         =   "High"
         Height          =   255
         Left            =   120
         TabIndex        =   14
         Top             =   600
         Width           =   1095
      End
   End
   Begin VB.Frame Frame1 
      Caption         =   "Input IO0 state"
      Height          =   1095
      Left            =   240
      TabIndex        =   10
      Top             =   6600
      Width           =   1575
      Begin VB.OptionButton Option2 
         Caption         =   "High"
         Height          =   255
         Left            =   120
         TabIndex        =   12
         Top             =   600
         Width           =   1095
      End
      Begin VB.OptionButton Option1 
         Caption         =   "Low"
         Height          =   375
         Left            =   120
         TabIndex        =   11
         Top             =   240
         Width           =   1095
      End
   End
   Begin VB.CommandButton ButtonPause1 
      Caption         =   "Pause"
      Height          =   375
      Index           =   1
      Left            =   10560
      TabIndex        =   9
      Top             =   240
      Width           =   1215
   End
   Begin VB.CommandButton CommandSetCamera1 
      Caption         =   "Settings"
      Height          =   375
      Index           =   0
      Left            =   7920
      TabIndex        =   8
      Top             =   240
      Width           =   1215
   End
   Begin VB.CommandButton CommandSnapShot1 
      Caption         =   "Snapshot"
      Height          =   375
      Index           =   0
      Left            =   11880
      TabIndex        =   7
      Top             =   240
      Width           =   1215
   End
   Begin VB.CommandButton ButtonPlay1 
      Caption         =   "Play"
      Height          =   375
      Index           =   0
      Left            =   9240
      TabIndex        =   6
      Top             =   240
      Width           =   1215
   End
   Begin MVOCXLib.MVOCX MVOCX 
      Height          =   5535
      Left            =   240
      TabIndex        =   4
      Top             =   840
      Width           =   6975
      _Version        =   65536
      _ExtentX        =   12303
      _ExtentY        =   9763
      _StockProps     =   0
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
   Begin VB.CommandButton CommandSnapShot 
      Caption         =   "Snapshot"
      Height          =   375
      Index           =   2
      Left            =   4200
      TabIndex        =   2
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
   Begin MVOCXLib.MVOCX MVOCX1 
      Height          =   5535
      Left            =   7920
      TabIndex        =   5
      Top             =   840
      Width           =   6975
      _Version        =   65536
      _ExtentX        =   12303
      _ExtentY        =   9763
      _StockProps     =   0
   End
   Begin VB.Frame Frame9 
      Caption         =   "Trigger Mode"
      Height          =   615
      Left            =   240
      TabIndex        =   34
      Top             =   7800
      Width           =   4695
      Begin VB.OptionButton Option17 
         Caption         =   "EXT Trigger"
         Height          =   255
         Index           =   2
         Left            =   2880
         TabIndex        =   37
         Top             =   240
         Width           =   1695
      End
      Begin VB.OptionButton Option17 
         Caption         =   "Sw Trigger"
         Height          =   255
         Index           =   1
         Left            =   1440
         TabIndex        =   36
         Top             =   240
         Width           =   1335
      End
      Begin VB.OptionButton Option17 
         Caption         =   "Continuous"
         Height          =   255
         Index           =   0
         Left            =   120
         TabIndex        =   35
         Top             =   240
         Width           =   1335
      End
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
    MVOCX.mvCameraPause
End Sub

Private Sub ButtonPause1_Click(index As Integer)
    MVOCX1.mvCameraPause
End Sub

Private Sub ButtonPlay_Click(index As Integer)
    MVOCX.mvCameraPlay
End Sub

Private Sub ButtonPlay1_Click(index As Integer)
    MVOCX1.mvCameraPlay
End Sub

Private Sub CommandSetCamera_Click(index As Integer) '顯示相機配置窗口
    MVOCX.mvCameraShowConfigPage
End Sub

Private Sub CommandSetCamera1_Click(index As Integer)
    MVOCX1.mvCameraShowConfigPage
End Sub

Private Sub CommandSnapShot_Click(index As Integer)
Dim iBufferAddr As Long
Dim iWidth As Long
Dim iHeight As Long
Dim iBpp As Long 'iBpp 表示要獲取的圖像位深度，8表示獲取8位灰度圖像，24表示獲取BGR24格式。黑白相機建議寫8，彩色相機寫24。
    iBpp = 8
    iBufferAddr = MVOCX.mvCameraGetImage(iWidth, iHeight, iBpp)  '獲得圖像緩沖區的地址和寬高信息。
    
End Sub


Private Sub Form_Load()
Dim iCameraNum As Long
Dim CameraList(10) As tSdkCameraDevInfo
Dim iret As Integer
Dim lImageWidth As Long
Dim lImageHeight As Long



'//枚舉設備，獲得設備列表
iCameraNum = 10 '調用CameraEnumerateDevice前，先設置iCameraNums = 10，表示最多只讀取10個設備，如果需要枚舉更多的設備，請更改sCameraList數組的大小和iCameraNums的值
iret = CameraEnumerateDevice(CameraList(0), iCameraNum)

If iCameraNum < 1 Then
            MsgBox ("沒有掃描到相機，請插上相機后再次運行本程序")
            Unload Me
            Exit Sub
Else
    
End If

    
   
   iret = MVOCX1.mvCameraInit(1)
   iret = MVOCX.mvCameraInit(0)
   
   'use mvCameraInitEx to init the camera by the friendly name.for example ,set the name of the two cameras as camera1 and camera2
   'iret = MVOCX1.mvCameraInitEx("camera2")
   'iret = MVOCX.mvCameraInitEx("camera1")
   
If iret = 0 Then
            MsgBox ("相機初始化失敗")
            Unload Me
            Exit Sub
Else
  

  ' MVOCX.mvSetImageFormat (0)  means 8 bit gray image format
  ' MVOCX.mvSetImageFormat (1)  means 24 bit RGB image format
  ' MVOCX.mvSetImageFormat (2)  means 32 bit RGBA image format
  ' MVOCX.mvSetImageFormat (3)  means 24 bit BGR image format
  ' MVOCX.mvSetImageFormat (4)  means 32 bit BGRA image format
      
   
   MVOCX.mvCameraPlay
   MVOCX1.mvCameraPlay
   
   'Get the image size for current configuration
   
   iret = MVOCX.mvCameraGetImageSize(lImageWidth, lImageHeight)
   iret = MVOCX.mvCameraGetTriggerMode
   
   If iret = 0 Then
   Option17.Item(0).Value = True
   End If
   
   If iret = 1 Then
   Option17.Item(1).Value = True
   End If
   
   If iret = 2 Then
   Option17.Item(2).Value = True
   End If
   
   iret = MVOCX1.mvCameraGetTriggerMode
   
   If iret = 0 Then
   Option18.Item(0).Value = True
   End If
   
   If iret = 1 Then
   Option18.Item(1).Value = True
   End If
   
   If iret = 2 Then
   Option18.Item(2).Value = True
   End If
   
   'MVOCX.mvCameraEnableCallBack 1
   'MVOCX1.mvCameraEnableCallBack 1
   
  'If preivew is not needed in ocx,use MVOCX.mvCameraEnablePreview (0) to stop priview
  Timer1.Interval = 300
  Timer1.Enabled = True
End If

End Sub

Private Sub Form_Unload(Cancel As Integer)
       MVOCX.mvCameraUnInit
       MVOCX1.mvCameraUnInit
End Sub


'///BIG5 TRANS ALLOWED
Private Sub MVOCX_OnFrameCaptured(ByVal FramebufferAddr As Long, ByVal ImageWidth As Long, ByVal ImageHeight As Long, ByVal Format As Long)

'Format is decided by MVOCX.mvSetImageFormat ,the default value is 3,means BGR 24bit format
'Format = 0 ,the sizeof the FrameBuffer is ImageWidth X ImageHeight
'Format = 1 ,the sizeof the FrameBuffer is ImageWidth X ImageHeight X 3
'Format = 2 ,the sizeof the FrameBuffer is ImageWidth X ImageHeight X 4
'Format = 3 ,the sizeof the FrameBuffer is ImageWidth X ImageHeight X 3
'Format = 4 ,the sizeof the FrameBuffer is ImageWidth X ImageHeight X 4


End Sub

Private Sub MVOCX1_OnFrameCaptured(ByVal FramebufferAddr As Long, ByVal ImageWidth As Long, ByVal ImageHeight As Long, ByVal Format As Long)
'Format is decided by MVOCX.mvSetImageFormat ,the default value is 3,means BGR 24bit format
'Format = 0 ,the sizeof the FrameBuffer is ImageWidth X ImageHeight
'Format = 1 ,the sizeof the FrameBuffer is ImageWidth X ImageHeight X 3
'Format = 2 ,the sizeof the FrameBuffer is ImageWidth X ImageHeight X 4
'Format = 3 ,the sizeof the FrameBuffer is ImageWidth X ImageHeight X 3
'Format = 4 ,the sizeof the FrameBuffer is ImageWidth X ImageHeight X 4
End Sub

Private Sub Option10_Click()
'Set the state for output IO 1 ,Set to high level state
MVOCX1.mvCameraSetIoState 1, 1
End Sub

Private Sub Option11_Click()
'Set the state for output IO 0 ,Set to high level state
MVOCX1.mvCameraSetIoState 0, 1
End Sub

Private Sub Option12_Click()
'Set the state for output IO 0 ,Set to low level state
MVOCX1.mvCameraSetIoState 0, 0
End Sub


Private Sub Option17_Click(index As Integer)
MVOCX.mvCameraSetTriggerMode index
End Sub

Private Sub Option18_Click(index As Integer)
MVOCX1.mvCameraSetTriggerMode index
End Sub

Private Sub Option5_Click()
'Set the state for output IO 0 ,Set to low level state
MVOCX.mvCameraSetIoState 0, 0

End Sub

Private Sub Option6_Click()
'Set the state for output IO 0 ,Set to high level state
MVOCX.mvCameraSetIoState 0, 1
End Sub

Private Sub Option7_Click()
'Set the state for output IO 1 ,Set to high level state
MVOCX.mvCameraSetIoState 1, 1
End Sub

Private Sub Option8_Click()
'Set the state for output IO 1 ,Set to low level state
MVOCX.mvCameraSetIoState 1, 0
End Sub

Private Sub Option9_Click()
'Set the state for output IO 1 ,Set to low level state
MVOCX1.mvCameraSetIoState 1, 0
End Sub



Private Sub SW_Triiger_Btn_Click()
MVOCX.mvCameraSoftTrigger
End Sub

Private Sub SW_Triiger_Btn1_Click()
MVOCX1.mvCameraSoftTrigger
End Sub

Private Sub Timer1_Timer()
Dim iState As Long

' for camera 1

iState = MVOCX.mvCameraGetIoState(0)

If iState = 0 Then
Option1.Value = True
Option2.Value = False
End If

If iState = 1 Then
Option1.Value = False
Option2.Value = True
End If

iState = MVOCX.mvCameraGetIoState(1)

If iState = 0 Then
Option1.Value = True
Option2.Value = False
End If

If iState = 1 Then
Option4.Value = False
Option3.Value = True
End If


'for camera 2
iState = MVOCX1.mvCameraGetIoState(0)

If iState = 0 Then
Option16.Value = True
Option15.Value = False
End If

If iState = 1 Then
Option16.Value = False
Option15.Value = True
End If

iState = MVOCX1.mvCameraGetIoState(1)

If iState = 0 Then
Option13.Value = True
Option14.Value = False
End If

If iState = 1 Then
Option13.Value = False
Option14.Value = True
End If
End Sub

Private Sub Timer2_Timer()
Dim Width As Long
Dim Height As Long
Dim bpp As Long
Dim BufferAddr As Long
bpp = 24

BufferAddr = MVOCX.mvCameraGetImage(Width, Height, bpp)

If BufferAddr <> 0 Then 'if you do not need preview  ,don't call mvCameraDisplayImage
MVOCX.mvCameraDisplayImage BufferAddr, Width, Height, bpp
End If

BufferAddr = MVOCX1.mvCameraGetImage(Width, Height, bpp)

If BufferAddr <> 0 Then 'if you do not need preview   ,don't call  mvCameraDisplayImage
MVOCX1.mvCameraDisplayImage BufferAddr, Width, Height, bpp
End If

End Sub
