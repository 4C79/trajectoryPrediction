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
        Me.components = New System.ComponentModel.Container
        Me.PictureBoxDisp = New System.Windows.Forms.PictureBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.TextBoxExp = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.TextBoxGain = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.ComboBoxTriggerMode = New System.Windows.Forms.ComboBox
        Me.ButtonAllSettings = New System.Windows.Forms.Button
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.Label4 = New System.Windows.Forms.Label
        Me.ButtonTrigger = New System.Windows.Forms.Button
        CType(Me.PictureBoxDisp, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PictureBoxDisp
        '
        Me.PictureBoxDisp.Location = New System.Drawing.Point(12, 27)
        Me.PictureBoxDisp.Name = "PictureBoxDisp"
        Me.PictureBoxDisp.Size = New System.Drawing.Size(545, 478)
        Me.PictureBoxDisp.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBoxDisp.TabIndex = 0
        Me.PictureBoxDisp.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(575, 230)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(77, 12)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "曝光时间(ms)"
        '
        'TextBoxExp
        '
        Me.TextBoxExp.Location = New System.Drawing.Point(660, 227)
        Me.TextBoxExp.Name = "TextBoxExp"
        Me.TextBoxExp.Size = New System.Drawing.Size(121, 21)
        Me.TextBoxExp.TabIndex = 2
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(599, 281)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(53, 12)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "模拟增益"
        '
        'TextBoxGain
        '
        Me.TextBoxGain.Location = New System.Drawing.Point(660, 278)
        Me.TextBoxGain.Name = "TextBoxGain"
        Me.TextBoxGain.Size = New System.Drawing.Size(121, 21)
        Me.TextBoxGain.TabIndex = 4
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(599, 329)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(53, 12)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "触发模式"
        '
        'ComboBoxTriggerMode
        '
        Me.ComboBoxTriggerMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxTriggerMode.FormattingEnabled = True
        Me.ComboBoxTriggerMode.Items.AddRange(New Object() {"连续模式", "软触发", "硬件触发"})
        Me.ComboBoxTriggerMode.Location = New System.Drawing.Point(660, 326)
        Me.ComboBoxTriggerMode.Name = "ComboBoxTriggerMode"
        Me.ComboBoxTriggerMode.Size = New System.Drawing.Size(121, 20)
        Me.ComboBoxTriggerMode.TabIndex = 6
        '
        'ButtonAllSettings
        '
        Me.ButtonAllSettings.Location = New System.Drawing.Point(634, 161)
        Me.ButtonAllSettings.Name = "ButtonAllSettings"
        Me.ButtonAllSettings.Size = New System.Drawing.Size(118, 34)
        Me.ButtonAllSettings.TabIndex = 7
        Me.ButtonAllSettings.Text = "高级设置"
        Me.ButtonAllSettings.UseVisualStyleBackColor = True
        '
        'Timer1
        '
        Me.Timer1.Enabled = True
        Me.Timer1.Interval = 200
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(10, 12)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(41, 12)
        Me.Label4.TabIndex = 8
        Me.Label4.Text = "Label4"
        '
        'ButtonTrigger
        '
        Me.ButtonTrigger.Location = New System.Drawing.Point(677, 361)
        Me.ButtonTrigger.Name = "ButtonTrigger"
        Me.ButtonTrigger.Size = New System.Drawing.Size(75, 23)
        Me.ButtonTrigger.TabIndex = 9
        Me.ButtonTrigger.Text = "触发一次"
        Me.ButtonTrigger.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(808, 517)
        Me.Controls.Add(Me.ButtonTrigger)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.ButtonAllSettings)
        Me.Controls.Add(Me.ComboBoxTriggerMode)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.TextBoxGain)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.TextBoxExp)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.PictureBoxDisp)
        Me.Name = "Form1"
        Me.Text = "基本属性"
        CType(Me.PictureBoxDisp, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents PictureBoxDisp As System.Windows.Forms.PictureBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TextBoxExp As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TextBoxGain As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents ComboBoxTriggerMode As System.Windows.Forms.ComboBox
    Friend WithEvents ButtonAllSettings As System.Windows.Forms.Button
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents ButtonTrigger As System.Windows.Forms.Button

End Class
