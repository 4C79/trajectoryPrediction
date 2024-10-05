<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form3
    Inherits System.Windows.Forms.Form
		'BIG5 TRANS ALLOWED
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
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.TextBox3 = New System.Windows.Forms.TextBox()
        Me.TextBox4 = New System.Windows.Forms.TextBox()
        Me.TextBox5 = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.ButtonReadSn2 = New System.Windows.Forms.Button()
        Me.ButtonWriteSn2 = New System.Windows.Forms.Button()
        Me.ButtonReadSn3 = New System.Windows.Forms.Button()
        Me.ButtonWriteSn3 = New System.Windows.Forms.Button()
        Me.ButtonReadSn1 = New System.Windows.Forms.Button()
        Me.TextBox6 = New System.Windows.Forms.TextBox()
        Me.ButtonWriteBlockData = New System.Windows.Forms.Button()
        Me.ButtonReadBlockData = New System.Windows.Forms.Button()
        Me.TextBox7 = New System.Windows.Forms.TextBox()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.ButtonWriteSn3)
        Me.GroupBox1.Controls.Add(Me.ButtonReadSn3)
        Me.GroupBox1.Controls.Add(Me.ButtonWriteSn2)
        Me.GroupBox1.Controls.Add(Me.ButtonReadSn1)
        Me.GroupBox1.Controls.Add(Me.ButtonReadSn2)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.TextBox5)
        Me.GroupBox1.Controls.Add(Me.TextBox4)
        Me.GroupBox1.Controls.Add(Me.TextBox3)
        Me.GroupBox1.Controls.Add(Me.TextBox2)
        Me.GroupBox1.Controls.Add(Me.TextBox1)
        Me.GroupBox1.Location = New System.Drawing.Point(9, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(407, 146)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "序列号读写"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.ButtonReadBlockData)
        Me.GroupBox2.Controls.Add(Me.TextBox7)
        Me.GroupBox2.Controls.Add(Me.ButtonWriteBlockData)
        Me.GroupBox2.Controls.Add(Me.TextBox6)
        Me.GroupBox2.Location = New System.Drawing.Point(9, 177)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(407, 106)
        Me.GroupBox2.TabIndex = 1
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "自定义数据块读写"
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(72, 27)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(251, 21)
        Me.TextBox1.TabIndex = 0
        '
        'TextBox2
        '
        Me.TextBox2.Location = New System.Drawing.Point(71, 67)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(123, 21)
        Me.TextBox2.TabIndex = 1
        '
        'TextBox3
        '
        Me.TextBox3.Location = New System.Drawing.Point(240, 67)
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.Size = New System.Drawing.Size(123, 21)
        Me.TextBox3.TabIndex = 2
        '
        'TextBox4
        '
        Me.TextBox4.Location = New System.Drawing.Point(71, 105)
        Me.TextBox4.Name = "TextBox4"
        Me.TextBox4.Size = New System.Drawing.Size(123, 21)
        Me.TextBox4.TabIndex = 3
        '
        'TextBox5
        '
        Me.TextBox5.Location = New System.Drawing.Point(240, 105)
        Me.TextBox5.Name = "TextBox5"
        Me.TextBox5.Size = New System.Drawing.Size(123, 21)
        Me.TextBox5.TabIndex = 4
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 28)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(65, 12)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "一级序列号"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(5, 73)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(65, 12)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "二级序列号"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(5, 108)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(65, 12)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "三级序列号"
        '
        'ButtonReadSn2
        '
        Me.ButtonReadSn2.Location = New System.Drawing.Point(198, 67)
        Me.ButtonReadSn2.Name = "ButtonReadSn2"
        Me.ButtonReadSn2.Size = New System.Drawing.Size(36, 23)
        Me.ButtonReadSn2.TabIndex = 7
        Me.ButtonReadSn2.Text = "读"
        Me.ButtonReadSn2.UseVisualStyleBackColor = True
        '
        'ButtonWriteSn2
        '
        Me.ButtonWriteSn2.Location = New System.Drawing.Point(369, 67)
        Me.ButtonWriteSn2.Name = "ButtonWriteSn2"
        Me.ButtonWriteSn2.Size = New System.Drawing.Size(36, 23)
        Me.ButtonWriteSn2.TabIndex = 7
        Me.ButtonWriteSn2.Text = "写"
        Me.ButtonWriteSn2.UseVisualStyleBackColor = True
        '
        'ButtonReadSn3
        '
        Me.ButtonReadSn3.Location = New System.Drawing.Point(198, 104)
        Me.ButtonReadSn3.Name = "ButtonReadSn3"
        Me.ButtonReadSn3.Size = New System.Drawing.Size(36, 23)
        Me.ButtonReadSn3.TabIndex = 7
        Me.ButtonReadSn3.Text = "读"
        Me.ButtonReadSn3.UseVisualStyleBackColor = True
        '
        'ButtonWriteSn3
        '
        Me.ButtonWriteSn3.Location = New System.Drawing.Point(370, 104)
        Me.ButtonWriteSn3.Name = "ButtonWriteSn3"
        Me.ButtonWriteSn3.Size = New System.Drawing.Size(36, 23)
        Me.ButtonWriteSn3.TabIndex = 7
        Me.ButtonWriteSn3.Text = "写"
        Me.ButtonWriteSn3.UseVisualStyleBackColor = True
        '
        'ButtonReadSn1
        '
        Me.ButtonReadSn1.Location = New System.Drawing.Point(329, 25)
        Me.ButtonReadSn1.Name = "ButtonReadSn1"
        Me.ButtonReadSn1.Size = New System.Drawing.Size(36, 23)
        Me.ButtonReadSn1.TabIndex = 7
        Me.ButtonReadSn1.Text = "读"
        Me.ButtonReadSn1.UseVisualStyleBackColor = True
        '
        'TextBox6
        '
        Me.TextBox6.Location = New System.Drawing.Point(14, 26)
        Me.TextBox6.Name = "TextBox6"
        Me.TextBox6.Size = New System.Drawing.Size(335, 21)
        Me.TextBox6.TabIndex = 0
        '
        'ButtonWriteBlockData
        '
        Me.ButtonWriteBlockData.Location = New System.Drawing.Point(365, 24)
        Me.ButtonWriteBlockData.Name = "ButtonWriteBlockData"
        Me.ButtonWriteBlockData.Size = New System.Drawing.Size(36, 23)
        Me.ButtonWriteBlockData.TabIndex = 8
        Me.ButtonWriteBlockData.Text = "写"
        Me.ButtonWriteBlockData.UseVisualStyleBackColor = True
        '
        'ButtonReadBlockData
        '
        Me.ButtonReadBlockData.Location = New System.Drawing.Point(365, 61)
        Me.ButtonReadBlockData.Name = "ButtonReadBlockData"
        Me.ButtonReadBlockData.Size = New System.Drawing.Size(36, 23)
        Me.ButtonReadBlockData.TabIndex = 10
        Me.ButtonReadBlockData.Text = "读"
        Me.ButtonReadBlockData.UseVisualStyleBackColor = True
        '
        'TextBox7
        '
        Me.TextBox7.Location = New System.Drawing.Point(14, 63)
        Me.TextBox7.Name = "TextBox7"
        Me.TextBox7.Size = New System.Drawing.Size(335, 21)
        Me.TextBox7.TabIndex = 9
        '
        'Form3
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(428, 292)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "Form3"
        Me.Text = "相机数据读写测试"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents ButtonWriteSn3 As System.Windows.Forms.Button
    Friend WithEvents ButtonReadSn3 As System.Windows.Forms.Button
    Friend WithEvents ButtonWriteSn2 As System.Windows.Forms.Button
    Friend WithEvents ButtonReadSn1 As System.Windows.Forms.Button
    Friend WithEvents ButtonReadSn2 As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TextBox5 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox4 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents ButtonReadBlockData As System.Windows.Forms.Button
    Friend WithEvents TextBox7 As System.Windows.Forms.TextBox
    Friend WithEvents ButtonWriteBlockData As System.Windows.Forms.Button
    Friend WithEvents TextBox6 As System.Windows.Forms.TextBox
End Class
