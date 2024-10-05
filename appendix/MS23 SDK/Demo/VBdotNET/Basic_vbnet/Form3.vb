Imports Microsoft.VisualBasic.VbStrConv

'BIG5 TRANS ALLOWED


Public Class Form3

    Private Sub ButtonReadSn1_Click(sender As System.Object, e As System.EventArgs) Handles ButtonReadSn1.Click
        Dim sn() As Byte
        ReDim sn(32 - 1)
        Dim sn1str As String
        Dim zeroChars() As Char = {ChrW(0)}

        CameraReadSN(m_hCamera, sn(0), 0)

        sn1str = System.Text.Encoding.ASCII.GetString(sn, 0, 31).TrimEnd(zeroChars)


        TextBox1.ResetText()
        TextBox1.AppendText(sn1str)

    End Sub

    Private Sub ButtonReadSn2_Click(sender As System.Object, e As System.EventArgs) Handles ButtonReadSn2.Click
        Dim sn() As Byte
        ReDim sn(32 - 1)
        Dim sn1str As String
        Dim zeroChars() As Char = {ChrW(0)}

        CameraReadSN(m_hCamera, sn(0), 1)

        sn1str = System.Text.Encoding.ASCII.GetString(sn, 0, 31).TrimEnd(zeroChars)


        TextBox2.ResetText()
        TextBox2.AppendText(sn1str)
    End Sub

    Private Sub ButtonWriteSn2_Click(sender As System.Object, e As System.EventArgs) Handles ButtonWriteSn2.Click
        Dim str = TextBox3.Text()
        Dim sn() As Byte
        ReDim sn(32 - 1)
        sn = System.Text.Encoding.ASCII.GetBytes(str)

        CameraWriteSN(m_hCamera, sn(0), 1)
    End Sub

    Private Sub ButtonReadSn3_Click(sender As System.Object, e As System.EventArgs) Handles ButtonReadSn3.Click
        Dim sn() As Byte
        ReDim sn(32 - 1)
        Dim sn1str As String
        Dim zeroChars() As Char = {ChrW(0)}

        CameraReadSN(m_hCamera, sn(0), 2)

        sn1str = System.Text.Encoding.ASCII.GetString(sn, 0, 31).TrimEnd(zeroChars)


        TextBox4.ResetText()
        TextBox4.AppendText(sn1str)
    End Sub

    Private Sub ButtonWriteSn3_Click(sender As System.Object, e As System.EventArgs) Handles ButtonWriteSn3.Click
        Dim str = TextBox5.Text()
        Dim sn() As Byte
        ReDim sn(32 - 1)
        sn = System.Text.Encoding.ASCII.GetBytes(str)

        CameraWriteSN(m_hCamera, sn(0), 2)
    End Sub

    Private Sub ButtonWriteBlockData_Click(sender As System.Object, e As System.EventArgs) Handles ButtonWriteBlockData.Click
        Dim str = TextBox6.Text()
        Dim block_data() As Byte
        ReDim block_data(256 - 1)
        block_data = System.Text.Encoding.ASCII.GetBytes(str)

        CameraSaveUserData(m_hCamera, 0, block_data(0), block_data.Length) 'block_data.Length不能超过相机最大的块空间，不同型号的相机，机身块数据空间大小不一样
    End Sub

    Private Sub ButtonReadBlockData_Click(sender As System.Object, e As System.EventArgs) Handles ButtonReadBlockData.Click
        Dim block_data() As Byte
        ReDim block_data(256 - 1)
        Dim StrData As String
        Dim zeroChars() As Char = {ChrW(0)}

        CameraLoadUserData(m_hCamera, 0, block_data(0), block_data.Length)

        StrData = System.Text.Encoding.ASCII.GetString(block_data, 0, 255).TrimEnd(zeroChars)

        TextBox7.ResetText()
        TextBox7.AppendText(StrData)
    End Sub
End Class