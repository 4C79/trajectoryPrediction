VERSION 5.00
Begin VB.Form Form1 
   Caption         =   "Camera VB Demo[Basic]"
   ClientHeight    =   10065
   ClientLeft      =   120
   ClientTop       =   450
   ClientWidth     =   11520
   Icon            =   "Form1.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   671
   ScaleMode       =   3  'Pixel
   ScaleWidth      =   768
   StartUpPosition =   3  '窗口缺省
   Begin VB.PictureBox ImageDisplayer 
      Height          =   3855
      Index           =   3
      Left            =   6000
      ScaleHeight     =   253
      ScaleMode       =   3  'Pixel
      ScaleWidth      =   341
      TabIndex        =   19
      Top             =   5880
      Width           =   5175
   End
   Begin VB.PictureBox ImageDisplayer 
      Height          =   3855
      Index           =   2
      Left            =   240
      ScaleHeight     =   253
      ScaleMode       =   3  'Pixel
      ScaleWidth      =   341
      TabIndex        =   18
      Top             =   5880
      Width           =   5175
   End
   Begin VB.CommandButton ButtonPlay 
      Caption         =   "Play"
      Height          =   375
      Index           =   0
      Left            =   1560
      TabIndex        =   5
      Top             =   240
      Width           =   1215
   End
   Begin VB.CommandButton ButtonPlay 
      Caption         =   "Play"
      Height          =   375
      Index           =   1
      Left            =   7320
      TabIndex        =   4
      Top             =   240
      Width           =   1215
   End
   Begin VB.CommandButton ButtonPause 
      Caption         =   "Pause"
      Height          =   375
      Index           =   3
      Left            =   8640
      TabIndex        =   17
      Top             =   5280
      Width           =   1215
   End
   Begin VB.CommandButton CommandSetCamera 
      Caption         =   "Settings"
      Height          =   375
      Index           =   3
      Left            =   6000
      TabIndex        =   16
      Top             =   5280
      Width           =   1215
   End
   Begin VB.CommandButton CommandSnapShot 
      Caption         =   "Snapshot"
      Height          =   375
      Index           =   3
      Left            =   9960
      TabIndex        =   15
      Top             =   5280
      Width           =   1215
   End
   Begin VB.CommandButton ButtonPlay 
      Caption         =   "Play"
      Height          =   375
      Index           =   3
      Left            =   7320
      TabIndex        =   14
      Top             =   5280
      Width           =   1215
   End
   Begin VB.CommandButton ButtonPause 
      Caption         =   "Pause"
      Height          =   375
      Index           =   2
      Left            =   2880
      TabIndex        =   13
      Top             =   5280
      Width           =   1215
   End
   Begin VB.CommandButton CommandSetCamera 
      Caption         =   "Settings"
      Height          =   375
      Index           =   2
      Left            =   240
      TabIndex        =   12
      Top             =   5280
      Width           =   1215
   End
   Begin VB.CommandButton CommandSnapShot 
      Caption         =   "Snapshot"
      Height          =   375
      Index           =   1
      Left            =   9960
      TabIndex        =   11
      Top             =   240
      Width           =   1215
   End
   Begin VB.PictureBox ImageDisplayer 
      Height          =   3855
      Index           =   1
      Left            =   6000
      ScaleHeight     =   253
      ScaleMode       =   3  'Pixel
      ScaleWidth      =   341
      TabIndex        =   10
      Top             =   840
      Width           =   5175
   End
   Begin VB.CommandButton ButtonPlay 
      Caption         =   "Play"
      Height          =   375
      Index           =   2
      Left            =   1560
      TabIndex        =   9
      Top             =   5280
      Width           =   1215
   End
   Begin VB.CommandButton ButtonPause 
      Caption         =   "Pause"
      Height          =   375
      Index           =   1
      Left            =   8640
      TabIndex        =   8
      Top             =   240
      Width           =   1215
   End
   Begin VB.CommandButton CommandSetCamera 
      Caption         =   "Settings"
      Height          =   375
      Index           =   0
      Left            =   240
      TabIndex        =   7
      Top             =   240
      Width           =   1215
   End
   Begin VB.CommandButton CommandSnapShot 
      Caption         =   "Snapshot"
      Height          =   375
      Index           =   0
      Left            =   4200
      TabIndex        =   6
      Top             =   240
      Width           =   1215
   End
   Begin VB.PictureBox ImageDisplayer 
      Height          =   3855
      Index           =   0
      Left            =   240
      ScaleHeight     =   253
      ScaleMode       =   3  'Pixel
      ScaleWidth      =   341
      TabIndex        =   3
      Top             =   840
      Width           =   5175
   End
   Begin VB.CommandButton CommandSnapShot 
      Caption         =   "Snapshot"
      Height          =   375
      Index           =   2
      Left            =   4200
      TabIndex        =   2
      Top             =   5280
      Width           =   1215
   End
   Begin VB.CommandButton CommandSetCamera 
      Caption         =   "Settings"
      Height          =   375
      Index           =   1
      Left            =   6000
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
End
Attribute VB_Name = "Form1"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Dim g_Grabber(4) As Long
Dim g_hCamera(4) As Long

Private Sub ButtonPause_Click(index As Integer)

If g_Grabber(index) <> 0 Then
    CameraGrabber_StopLive (g_Grabber(index))
End If

End Sub

Private Sub ButtonPlay_Click(index As Integer)

If g_Grabber(index) <> 0 Then
    CameraGrabber_StartLive (g_Grabber(index))
End If

End Sub

Private Sub CommandSetCamera_Click(index As Integer) '显示相机配置窗口

Dim iRet As Long
If g_Grabber(index) <> 0 Then
    iRet = CameraShowSettingPage(g_hCamera(index), 1)
End If

End Sub

Private Sub CommandSnapShot_Click(index As Integer)

Dim iRet As Long
Dim Image As Long
Dim sFileName As String
Dim sTmp() As Byte

If g_Grabber(index) <> 0 Then
    iRet = CameraGrabber_SaveImage(g_Grabber(index), Image, 2000)
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
End If

End Sub

Private Sub Form_Load()

Dim iRet As Integer
Dim DevInfo As tSdkCameraDevInfo
Dim iCameraNum As Long
Dim CameraList(4) As tSdkCameraDevInfo
Dim sCameraInfo As tSdkCameraCapbility

iCameraNum = 4
iRet = CameraEnumerateDevice(CameraList(0), iCameraNum)

For i = 0 To iCameraNum - 1

    iRet = CameraGrabber_Create(g_Grabber(i), CameraList(i))
    If iRet = 0 Then
        iRet = CameraGrabber_GetCameraDevInfo(g_Grabber(i), DevInfo)
        iRet = CameraGrabber_GetCameraHandle(g_Grabber(i), g_hCamera(i))
        iRet = CameraCreateSettingPage(g_hCamera(i), Form1.hWnd, DevInfo.acFriendlyName(0), 0, 0, 0)
        iRet = CameraGrabber_SetHWnd(g_Grabber(i), ImageDisplayer(i).hWnd)
        
        iRet = CameraGetCapability(g_hCamera(i), sCameraInfo)
        If sCameraInfo.sIspCapacity.bMonoSensor <> 0 Then
            iRet = CameraSetIspOutFormat(g_hCamera(i), CAMERA_MEDIA_TYPE_MONO8)
        End If
        
    End If
    
Next i

For i = 0 To 3
    If g_Grabber(i) <> 0 Then
        iRet = CameraGrabber_StartLive(g_Grabber(i))
    End If
Next i

End Sub

Private Sub Form_Unload(Cancel As Integer)

For i = 0 To 3
    If g_Grabber(i) <> 0 Then
        iRet = CameraGrabber_Destroy(g_Grabber(i))
    End If
Next i

End Sub


'///BIG5 TRANS ALLOWED

