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
        Me.ButtonSettings = New System.Windows.Forms.Button()
        Me.ButtonPlay = New System.Windows.Forms.Button()
        Me.ButtonStop = New System.Windows.Forms.Button()
        Me.ButtonSnap = New System.Windows.Forms.Button()
        Me.DispWnd = New System.Windows.Forms.PictureBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.SoftTrigger = New System.Windows.Forms.Button()
        CType(Me.DispWnd, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ButtonSettings
        '
        Me.ButtonSettings.Location = New System.Drawing.Point(12, 12)
        Me.ButtonSettings.Name = "ButtonSettings"
        Me.ButtonSettings.Size = New System.Drawing.Size(75, 23)
        Me.ButtonSettings.TabIndex = 0
        Me.ButtonSettings.Text = "相机设置"
        Me.ButtonSettings.UseVisualStyleBackColor = True
        '
        'ButtonPlay
        '
        Me.ButtonPlay.Location = New System.Drawing.Point(119, 12)
        Me.ButtonPlay.Name = "ButtonPlay"
        Me.ButtonPlay.Size = New System.Drawing.Size(75, 23)
        Me.ButtonPlay.TabIndex = 1
        Me.ButtonPlay.Text = "播放"
        Me.ButtonPlay.UseVisualStyleBackColor = True
        '
        'ButtonStop
        '
        Me.ButtonStop.Location = New System.Drawing.Point(226, 12)
        Me.ButtonStop.Name = "ButtonStop"
        Me.ButtonStop.Size = New System.Drawing.Size(75, 23)
        Me.ButtonStop.TabIndex = 2
        Me.ButtonStop.Text = "停止"
        Me.ButtonStop.UseVisualStyleBackColor = True
        '
        'ButtonSnap
        '
        Me.ButtonSnap.Location = New System.Drawing.Point(333, 12)
        Me.ButtonSnap.Name = "ButtonSnap"
        Me.ButtonSnap.Size = New System.Drawing.Size(75, 23)
        Me.ButtonSnap.TabIndex = 3
        Me.ButtonSnap.Text = "抓图"
        Me.ButtonSnap.UseVisualStyleBackColor = True
        '
        'DispWnd
        '
        Me.DispWnd.Location = New System.Drawing.Point(12, 54)
        Me.DispWnd.Name = "DispWnd"
        Me.DispWnd.Size = New System.Drawing.Size(689, 508)
        Me.DispWnd.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.DispWnd.TabIndex = 4
        Me.DispWnd.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 574)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(41, 12)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Label1"
        '
        'Timer1
        '
        Me.Timer1.Enabled = True
        Me.Timer1.Interval = 1000
        '
        'SoftTrigger
        '
        Me.SoftTrigger.Location = New System.Drawing.Point(448, 12)
        Me.SoftTrigger.Name = "SoftTrigger"
        Me.SoftTrigger.Size = New System.Drawing.Size(75, 23)
        Me.SoftTrigger.TabIndex = 6
        Me.SoftTrigger.Text = "软触发"
        Me.SoftTrigger.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(713, 598)
        Me.Controls.Add(Me.SoftTrigger)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.DispWnd)
        Me.Controls.Add(Me.ButtonSnap)
        Me.Controls.Add(Me.ButtonStop)
        Me.Controls.Add(Me.ButtonPlay)
        Me.Controls.Add(Me.ButtonSettings)
        Me.Name = "Form1"
        Me.Text = "Form1"
        CType(Me.DispWnd, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ButtonSettings As System.Windows.Forms.Button
    Friend WithEvents ButtonPlay As System.Windows.Forms.Button
    Friend WithEvents ButtonStop As System.Windows.Forms.Button
    Friend WithEvents ButtonSnap As System.Windows.Forms.Button
    Friend WithEvents DispWnd As System.Windows.Forms.PictureBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents SoftTrigger As System.Windows.Forms.Button

End Class
