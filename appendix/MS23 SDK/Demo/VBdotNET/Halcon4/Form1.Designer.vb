<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form 重写 Dispose，以清理组件列表。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows 窗体设计器所必需的
    Private components As System.ComponentModel.IContainer

    '注意: 以下过程是 Windows 窗体设计器所必需的
    '可以使用 Windows 窗体设计器修改它。
    '不要使用代码编辑器修改它。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.HWnd1 = New HalconDotNet.HWindowControl()
        Me.HWnd2 = New HalconDotNet.HWindowControl()
        Me.HWnd3 = New HalconDotNet.HWindowControl()
        Me.HWnd4 = New HalconDotNet.HWindowControl()
        Me.Cam1Settings = New System.Windows.Forms.Button()
        Me.Cam2Settings = New System.Windows.Forms.Button()
        Me.Cam4Settings = New System.Windows.Forms.Button()
        Me.Cam3Settings = New System.Windows.Forms.Button()
        Me.Cam1Stat = New System.Windows.Forms.Label()
        Me.Cam2Stat = New System.Windows.Forms.Label()
        Me.Cam3Stat = New System.Windows.Forms.Label()
        Me.Cam4Stat = New System.Windows.Forms.Label()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.SuspendLayout()
        '
        'HWnd1
        '
        Me.HWnd1.BackColor = System.Drawing.Color.Black
        Me.HWnd1.BorderColor = System.Drawing.Color.Black
        Me.HWnd1.ImagePart = New System.Drawing.Rectangle(0, 0, 640, 480)
        Me.HWnd1.Location = New System.Drawing.Point(12, 51)
        Me.HWnd1.Name = "HWnd1"
        Me.HWnd1.Size = New System.Drawing.Size(482, 400)
        Me.HWnd1.TabIndex = 0
        Me.HWnd1.WindowSize = New System.Drawing.Size(482, 400)
        '
        'HWnd2
        '
        Me.HWnd2.BackColor = System.Drawing.Color.Black
        Me.HWnd2.BorderColor = System.Drawing.Color.Black
        Me.HWnd2.ImagePart = New System.Drawing.Rectangle(0, 0, 640, 480)
        Me.HWnd2.Location = New System.Drawing.Point(500, 51)
        Me.HWnd2.Name = "HWnd2"
        Me.HWnd2.Size = New System.Drawing.Size(482, 400)
        Me.HWnd2.TabIndex = 1
        Me.HWnd2.WindowSize = New System.Drawing.Size(482, 400)
        '
        'HWnd3
        '
        Me.HWnd3.BackColor = System.Drawing.Color.Black
        Me.HWnd3.BorderColor = System.Drawing.Color.Black
        Me.HWnd3.ImagePart = New System.Drawing.Rectangle(0, 0, 640, 480)
        Me.HWnd3.Location = New System.Drawing.Point(12, 508)
        Me.HWnd3.Name = "HWnd3"
        Me.HWnd3.Size = New System.Drawing.Size(482, 400)
        Me.HWnd3.TabIndex = 2
        Me.HWnd3.WindowSize = New System.Drawing.Size(482, 400)
        '
        'HWnd4
        '
        Me.HWnd4.BackColor = System.Drawing.Color.Black
        Me.HWnd4.BorderColor = System.Drawing.Color.Black
        Me.HWnd4.ImagePart = New System.Drawing.Rectangle(0, 0, 640, 480)
        Me.HWnd4.Location = New System.Drawing.Point(500, 508)
        Me.HWnd4.Name = "HWnd4"
        Me.HWnd4.Size = New System.Drawing.Size(482, 400)
        Me.HWnd4.TabIndex = 3
        Me.HWnd4.WindowSize = New System.Drawing.Size(482, 400)
        '
        'Cam1Settings
        '
        Me.Cam1Settings.Location = New System.Drawing.Point(12, 22)
        Me.Cam1Settings.Name = "Cam1Settings"
        Me.Cam1Settings.Size = New System.Drawing.Size(75, 23)
        Me.Cam1Settings.TabIndex = 4
        Me.Cam1Settings.Text = "相机1"
        Me.Cam1Settings.UseVisualStyleBackColor = True
        '
        'Cam2Settings
        '
        Me.Cam2Settings.Location = New System.Drawing.Point(500, 22)
        Me.Cam2Settings.Name = "Cam2Settings"
        Me.Cam2Settings.Size = New System.Drawing.Size(75, 23)
        Me.Cam2Settings.TabIndex = 5
        Me.Cam2Settings.Text = "相机2"
        Me.Cam2Settings.UseVisualStyleBackColor = True
        '
        'Cam4Settings
        '
        Me.Cam4Settings.Location = New System.Drawing.Point(500, 479)
        Me.Cam4Settings.Name = "Cam4Settings"
        Me.Cam4Settings.Size = New System.Drawing.Size(75, 23)
        Me.Cam4Settings.TabIndex = 7
        Me.Cam4Settings.Text = "相机4"
        Me.Cam4Settings.UseVisualStyleBackColor = True
        '
        'Cam3Settings
        '
        Me.Cam3Settings.Location = New System.Drawing.Point(12, 479)
        Me.Cam3Settings.Name = "Cam3Settings"
        Me.Cam3Settings.Size = New System.Drawing.Size(75, 23)
        Me.Cam3Settings.TabIndex = 6
        Me.Cam3Settings.Text = "相机3"
        Me.Cam3Settings.UseVisualStyleBackColor = True
        '
        'Cam1Stat
        '
        Me.Cam1Stat.AutoSize = True
        Me.Cam1Stat.Location = New System.Drawing.Point(93, 27)
        Me.Cam1Stat.Name = "Cam1Stat"
        Me.Cam1Stat.Size = New System.Drawing.Size(41, 12)
        Me.Cam1Stat.TabIndex = 8
        Me.Cam1Stat.Text = "Label1"
        '
        'Cam2Stat
        '
        Me.Cam2Stat.AutoSize = True
        Me.Cam2Stat.Location = New System.Drawing.Point(581, 27)
        Me.Cam2Stat.Name = "Cam2Stat"
        Me.Cam2Stat.Size = New System.Drawing.Size(41, 12)
        Me.Cam2Stat.TabIndex = 9
        Me.Cam2Stat.Text = "Label1"
        '
        'Cam3Stat
        '
        Me.Cam3Stat.AutoSize = True
        Me.Cam3Stat.Location = New System.Drawing.Point(93, 484)
        Me.Cam3Stat.Name = "Cam3Stat"
        Me.Cam3Stat.Size = New System.Drawing.Size(41, 12)
        Me.Cam3Stat.TabIndex = 10
        Me.Cam3Stat.Text = "Label1"
        '
        'Cam4Stat
        '
        Me.Cam4Stat.AutoSize = True
        Me.Cam4Stat.Location = New System.Drawing.Point(581, 484)
        Me.Cam4Stat.Name = "Cam4Stat"
        Me.Cam4Stat.Size = New System.Drawing.Size(41, 12)
        Me.Cam4Stat.TabIndex = 11
        Me.Cam4Stat.Text = "Label1"
        '
        'Timer1
        '
        Me.Timer1.Enabled = True
        Me.Timer1.Interval = 1000
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(995, 920)
        Me.Controls.Add(Me.Cam4Stat)
        Me.Controls.Add(Me.Cam3Stat)
        Me.Controls.Add(Me.Cam2Stat)
        Me.Controls.Add(Me.Cam1Stat)
        Me.Controls.Add(Me.Cam4Settings)
        Me.Controls.Add(Me.Cam3Settings)
        Me.Controls.Add(Me.Cam2Settings)
        Me.Controls.Add(Me.Cam1Settings)
        Me.Controls.Add(Me.HWnd4)
        Me.Controls.Add(Me.HWnd3)
        Me.Controls.Add(Me.HWnd2)
        Me.Controls.Add(Me.HWnd1)
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Form1"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents HWnd1 As HalconDotNet.HWindowControl
    Friend WithEvents HWnd2 As HalconDotNet.HWindowControl
    Friend WithEvents HWnd3 As HalconDotNet.HWindowControl
    Friend WithEvents HWnd4 As HalconDotNet.HWindowControl
    Friend WithEvents Cam1Settings As System.Windows.Forms.Button
    Friend WithEvents Cam2Settings As System.Windows.Forms.Button
    Friend WithEvents Cam4Settings As System.Windows.Forms.Button
    Friend WithEvents Cam3Settings As System.Windows.Forms.Button
    Friend WithEvents Cam1Stat As System.Windows.Forms.Label
    Friend WithEvents Cam2Stat As System.Windows.Forms.Label
    Friend WithEvents Cam3Stat As System.Windows.Forms.Label
    Friend WithEvents Cam4Stat As System.Windows.Forms.Label
    Friend WithEvents Timer1 As System.Windows.Forms.Timer

End Class
