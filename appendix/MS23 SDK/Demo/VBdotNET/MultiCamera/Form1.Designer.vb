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
        Me.label4 = New System.Windows.Forms.Label()
        Me.label3 = New System.Windows.Forms.Label()
        Me.label2 = New System.Windows.Forms.Label()
        Me.label1 = New System.Windows.Forms.Label()
        Me.buttonStop4 = New System.Windows.Forms.Button()
        Me.buttonPlay4 = New System.Windows.Forms.Button()
        Me.buttonSettings4 = New System.Windows.Forms.Button()
        Me.DispWnd4 = New System.Windows.Forms.PictureBox()
        Me.buttonStop3 = New System.Windows.Forms.Button()
        Me.buttonPlay3 = New System.Windows.Forms.Button()
        Me.buttonSettings3 = New System.Windows.Forms.Button()
        Me.DispWnd3 = New System.Windows.Forms.PictureBox()
        Me.buttonStop2 = New System.Windows.Forms.Button()
        Me.buttonPlay2 = New System.Windows.Forms.Button()
        Me.buttonSettings2 = New System.Windows.Forms.Button()
        Me.DispWnd2 = New System.Windows.Forms.PictureBox()
        Me.buttonStop1 = New System.Windows.Forms.Button()
        Me.buttonPlay1 = New System.Windows.Forms.Button()
        Me.buttonSettings1 = New System.Windows.Forms.Button()
        Me.DispWnd1 = New System.Windows.Forms.PictureBox()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        CType(Me.DispWnd4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DispWnd3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DispWnd2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DispWnd1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'label4
        '
        Me.label4.AutoSize = True
        Me.label4.Location = New System.Drawing.Point(409, 727)
        Me.label4.Name = "label4"
        Me.label4.Size = New System.Drawing.Size(41, 12)
        Me.label4.TabIndex = 52
        Me.label4.Text = "label4"
        '
        'label3
        '
        Me.label3.AutoSize = True
        Me.label3.Location = New System.Drawing.Point(10, 729)
        Me.label3.Name = "label3"
        Me.label3.Size = New System.Drawing.Size(41, 12)
        Me.label3.TabIndex = 51
        Me.label3.Text = "label3"
        '
        'label2
        '
        Me.label2.AutoSize = True
        Me.label2.Location = New System.Drawing.Point(409, 356)
        Me.label2.Name = "label2"
        Me.label2.Size = New System.Drawing.Size(41, 12)
        Me.label2.TabIndex = 50
        Me.label2.Text = "label2"
        '
        'label1
        '
        Me.label1.AutoSize = True
        Me.label1.Location = New System.Drawing.Point(12, 356)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(41, 12)
        Me.label1.TabIndex = 49
        Me.label1.Text = "label1"
        '
        'buttonStop4
        '
        Me.buttonStop4.Location = New System.Drawing.Point(597, 381)
        Me.buttonStop4.Name = "buttonStop4"
        Me.buttonStop4.Size = New System.Drawing.Size(75, 23)
        Me.buttonStop4.TabIndex = 47
        Me.buttonStop4.Text = "停止"
        Me.buttonStop4.UseVisualStyleBackColor = True
        '
        'buttonPlay4
        '
        Me.buttonPlay4.Location = New System.Drawing.Point(504, 381)
        Me.buttonPlay4.Name = "buttonPlay4"
        Me.buttonPlay4.Size = New System.Drawing.Size(75, 23)
        Me.buttonPlay4.TabIndex = 46
        Me.buttonPlay4.Text = "播放"
        Me.buttonPlay4.UseVisualStyleBackColor = True
        '
        'buttonSettings4
        '
        Me.buttonSettings4.Location = New System.Drawing.Point(411, 381)
        Me.buttonSettings4.Name = "buttonSettings4"
        Me.buttonSettings4.Size = New System.Drawing.Size(75, 23)
        Me.buttonSettings4.TabIndex = 45
        Me.buttonSettings4.Text = "相机设置"
        Me.buttonSettings4.UseVisualStyleBackColor = True
        '
        'DispWnd4
        '
        Me.DispWnd4.Location = New System.Drawing.Point(411, 410)
        Me.DispWnd4.Name = "DispWnd4"
        Me.DispWnd4.Size = New System.Drawing.Size(355, 314)
        Me.DispWnd4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.DispWnd4.TabIndex = 44
        Me.DispWnd4.TabStop = False
        '
        'buttonStop3
        '
        Me.buttonStop3.Location = New System.Drawing.Point(198, 381)
        Me.buttonStop3.Name = "buttonStop3"
        Me.buttonStop3.Size = New System.Drawing.Size(75, 23)
        Me.buttonStop3.TabIndex = 42
        Me.buttonStop3.Text = "停止"
        Me.buttonStop3.UseVisualStyleBackColor = True
        '
        'buttonPlay3
        '
        Me.buttonPlay3.Location = New System.Drawing.Point(105, 381)
        Me.buttonPlay3.Name = "buttonPlay3"
        Me.buttonPlay3.Size = New System.Drawing.Size(75, 23)
        Me.buttonPlay3.TabIndex = 41
        Me.buttonPlay3.Text = "播放"
        Me.buttonPlay3.UseVisualStyleBackColor = True
        '
        'buttonSettings3
        '
        Me.buttonSettings3.Location = New System.Drawing.Point(12, 381)
        Me.buttonSettings3.Name = "buttonSettings3"
        Me.buttonSettings3.Size = New System.Drawing.Size(75, 23)
        Me.buttonSettings3.TabIndex = 40
        Me.buttonSettings3.Text = "相机设置"
        Me.buttonSettings3.UseVisualStyleBackColor = True
        '
        'DispWnd3
        '
        Me.DispWnd3.Location = New System.Drawing.Point(12, 410)
        Me.DispWnd3.Name = "DispWnd3"
        Me.DispWnd3.Size = New System.Drawing.Size(355, 314)
        Me.DispWnd3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.DispWnd3.TabIndex = 39
        Me.DispWnd3.TabStop = False
        '
        'buttonStop2
        '
        Me.buttonStop2.Location = New System.Drawing.Point(597, 10)
        Me.buttonStop2.Name = "buttonStop2"
        Me.buttonStop2.Size = New System.Drawing.Size(75, 23)
        Me.buttonStop2.TabIndex = 37
        Me.buttonStop2.Text = "停止"
        Me.buttonStop2.UseVisualStyleBackColor = True
        '
        'buttonPlay2
        '
        Me.buttonPlay2.Location = New System.Drawing.Point(504, 10)
        Me.buttonPlay2.Name = "buttonPlay2"
        Me.buttonPlay2.Size = New System.Drawing.Size(75, 23)
        Me.buttonPlay2.TabIndex = 36
        Me.buttonPlay2.Text = "播放"
        Me.buttonPlay2.UseVisualStyleBackColor = True
        '
        'buttonSettings2
        '
        Me.buttonSettings2.Location = New System.Drawing.Point(411, 10)
        Me.buttonSettings2.Name = "buttonSettings2"
        Me.buttonSettings2.Size = New System.Drawing.Size(75, 23)
        Me.buttonSettings2.TabIndex = 35
        Me.buttonSettings2.Text = "相机设置"
        Me.buttonSettings2.UseVisualStyleBackColor = True
        '
        'DispWnd2
        '
        Me.DispWnd2.Location = New System.Drawing.Point(411, 39)
        Me.DispWnd2.Name = "DispWnd2"
        Me.DispWnd2.Size = New System.Drawing.Size(355, 314)
        Me.DispWnd2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.DispWnd2.TabIndex = 34
        Me.DispWnd2.TabStop = False
        '
        'buttonStop1
        '
        Me.buttonStop1.Location = New System.Drawing.Point(198, 10)
        Me.buttonStop1.Name = "buttonStop1"
        Me.buttonStop1.Size = New System.Drawing.Size(75, 23)
        Me.buttonStop1.TabIndex = 32
        Me.buttonStop1.Text = "停止"
        Me.buttonStop1.UseVisualStyleBackColor = True
        '
        'buttonPlay1
        '
        Me.buttonPlay1.Location = New System.Drawing.Point(105, 10)
        Me.buttonPlay1.Name = "buttonPlay1"
        Me.buttonPlay1.Size = New System.Drawing.Size(75, 23)
        Me.buttonPlay1.TabIndex = 31
        Me.buttonPlay1.Text = "播放"
        Me.buttonPlay1.UseVisualStyleBackColor = True
        '
        'buttonSettings1
        '
        Me.buttonSettings1.Location = New System.Drawing.Point(12, 10)
        Me.buttonSettings1.Name = "buttonSettings1"
        Me.buttonSettings1.Size = New System.Drawing.Size(75, 23)
        Me.buttonSettings1.TabIndex = 30
        Me.buttonSettings1.Text = "相机设置"
        Me.buttonSettings1.UseVisualStyleBackColor = True
        '
        'DispWnd1
        '
        Me.DispWnd1.Location = New System.Drawing.Point(12, 39)
        Me.DispWnd1.Name = "DispWnd1"
        Me.DispWnd1.Size = New System.Drawing.Size(355, 314)
        Me.DispWnd1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.DispWnd1.TabIndex = 29
        Me.DispWnd1.TabStop = False
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
        Me.ClientSize = New System.Drawing.Size(781, 749)
        Me.Controls.Add(Me.label4)
        Me.Controls.Add(Me.label3)
        Me.Controls.Add(Me.label2)
        Me.Controls.Add(Me.label1)
        Me.Controls.Add(Me.buttonStop4)
        Me.Controls.Add(Me.buttonPlay4)
        Me.Controls.Add(Me.buttonSettings4)
        Me.Controls.Add(Me.DispWnd4)
        Me.Controls.Add(Me.buttonStop3)
        Me.Controls.Add(Me.buttonPlay3)
        Me.Controls.Add(Me.buttonSettings3)
        Me.Controls.Add(Me.DispWnd3)
        Me.Controls.Add(Me.buttonStop2)
        Me.Controls.Add(Me.buttonPlay2)
        Me.Controls.Add(Me.buttonSettings2)
        Me.Controls.Add(Me.DispWnd2)
        Me.Controls.Add(Me.buttonStop1)
        Me.Controls.Add(Me.buttonPlay1)
        Me.Controls.Add(Me.buttonSettings1)
        Me.Controls.Add(Me.DispWnd1)
        Me.Name = "Form1"
        Me.Text = "Form1"
        CType(Me.DispWnd4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DispWnd3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DispWnd2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DispWnd1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents label4 As System.Windows.Forms.Label
    Private WithEvents label3 As System.Windows.Forms.Label
    Private WithEvents label2 As System.Windows.Forms.Label
    Private WithEvents label1 As System.Windows.Forms.Label
    Private WithEvents buttonStop4 As System.Windows.Forms.Button
    Private WithEvents buttonPlay4 As System.Windows.Forms.Button
    Private WithEvents buttonSettings4 As System.Windows.Forms.Button
    Private WithEvents DispWnd4 As System.Windows.Forms.PictureBox
    Private WithEvents buttonStop3 As System.Windows.Forms.Button
    Private WithEvents buttonPlay3 As System.Windows.Forms.Button
    Private WithEvents buttonSettings3 As System.Windows.Forms.Button
    Private WithEvents DispWnd3 As System.Windows.Forms.PictureBox
    Private WithEvents buttonStop2 As System.Windows.Forms.Button
    Private WithEvents buttonPlay2 As System.Windows.Forms.Button
    Private WithEvents buttonSettings2 As System.Windows.Forms.Button
    Private WithEvents DispWnd2 As System.Windows.Forms.PictureBox
    Private WithEvents buttonStop1 As System.Windows.Forms.Button
    Private WithEvents buttonPlay1 As System.Windows.Forms.Button
    Private WithEvents buttonSettings1 As System.Windows.Forms.Button
    Private WithEvents DispWnd1 As System.Windows.Forms.PictureBox
    Friend WithEvents Timer1 As System.Windows.Forms.Timer

End Class
