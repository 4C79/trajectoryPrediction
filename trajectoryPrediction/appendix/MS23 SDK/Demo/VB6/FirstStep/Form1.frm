VERSION 5.00
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
   Begin VB.Timer InfoUpdateTimer 
      Enabled         =   0   'False
      Interval        =   1000
      Left            =   6600
      Top             =   240
   End
   Begin VB.CommandButton ButtonPlay 
      Caption         =   "Play"
      Height          =   375
      Index           =   1
      Left            =   1560
      TabIndex        =   5
      Top             =   240
      Width           =   1215
   End
   Begin VB.PictureBox ImageDisplayer 
      Height          =   7215
      Left            =   240
      ScaleHeight     =   477
      ScaleMode       =   3  'Pixel
      ScaleWidth      =   773
      TabIndex        =   3
      Top             =   840
      Width           =   11655
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
   Begin VB.Label StatusLabel 
      Caption         =   "Video Information"
      Height          =   375
      Left            =   240
      TabIndex        =   4
      Top             =   8280
      Width           =   11655
   End
End
Attribute VB_Name = "Form1"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Public g_Grabber As Long
Public g_hCamera As Long

Private Sub ButtonPause_Click(index As Integer)
CameraGrabber_StopLive (g_Grabber)
End Sub

Private Sub ButtonPlay_Click(index As Integer)
CameraGrabber_StartLive (g_Grabber)
End Sub

Private Sub CommandSetCamera_Click(index As Integer) '显示相机配置窗口

Dim iRet As Long
iRet = CameraShowSettingPage(g_hCamera, 1)

End Sub

Private Sub CommandSnapShot_Click(index As Integer)

Dim iRet As Long
Dim Image As Long
Dim sFileName As String
Dim sTmp() As Byte

iRet = CameraGrabber_SaveImage(g_Grabber, Image, 2000)
If iRet = 0 Then
    sFileName = App.Path + "\" + Format$(Now, "yyyy-mm-dd-h-mm-ss") + ".BMP" '//初始化保存文件的文件名
    sTmp = StrConv(sFileName, vbFromUnicode)
    ReDim Preserve sTmp(UBound(sTmp) + 1)
    sTmp(UBound(sTmp)) = 0
    
    iRet = CameraImage_SaveAsBmp(Image, sTmp(0))
    If iRet = CAMERA_STATUS_SUCCESS Then
        MsgBox ("Save image OK! The file is:[" + sFileName + "]OK!")
    Else
        MsgBox ("Save image failed")
    End If
    
    CameraImage_Destroy (Image)
Else
    MsgBox ("Save image timeout")
End If

End Sub

Private Sub Form_Load()

Dim iRet As Integer
Dim DevInfo As tSdkCameraDevInfo
Dim sCameraInfo As tSdkCameraCapbility

iRet = CameraGrabber_CreateFromDevicePage(g_Grabber)
If iRet <> 0 Then
    Unload Me
    Exit Sub
End If

iRet = CameraGrabber_GetCameraDevInfo(g_Grabber, DevInfo)
iRet = CameraGrabber_GetCameraHandle(g_Grabber, g_hCamera)
iRet = CameraCreateSettingPage(g_hCamera, Form1.hWnd, DevInfo.acFriendlyName(0), 0, 0, 0)

iRet = CameraGetCapability(g_hCamera, sCameraInfo)
If sCameraInfo.sIspCapacity.bMonoSensor <> 0 Then
    iRet = CameraSetIspOutFormat(g_hCamera, CAMERA_MEDIA_TYPE_MONO8)
End If

iRet = CameraGrabber_SetHWnd(g_Grabber, ImageDisplayer.hWnd)
iRet = CameraGrabber_StartLive(g_Grabber)

InfoUpdateTimer.Enabled = True

End Sub

Private Sub Form_Unload(Cancel As Integer)

InfoUpdateTimer.Enabled = False
CameraGrabber_Destroy (g_Grabber)

End Sub

'一秒刷新一次图像信息
Private Sub InfoUpdateTimer_Timer()

Dim stat As tSdkGrabberStat
Dim sFrameInfo As String
Dim iRet As Long

iRet = CameraGrabber_GetStat(g_Grabber, stat)

sFrameInfo = "| Resolution " + Format$(stat.Width, "General Number") + " X " + Format$(stat.Height, "General Number") + " | Display FPS " + Format$(stat.DispFps, "General Number") + " | Cap FPS " + Format$(stat.CapFps, "General Number") + "|"
StatusLabel.Caption = sFrameInfo
    
End Sub

'///BIG5 TRANS ALLOWED

