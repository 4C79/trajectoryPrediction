Imports System.IO
Imports System.Runtime.InteropServices



Public Class Form2
    Public Function UpdateImage(ByVal pRgbBuffer As Integer, ByRef tFrameHead As tSdkFrameHead) As Integer
        Dim bmi As BITMAPINFOHEADER
        Dim bmfi As BITMAPFILEHEADER
        '        Dim MyImage As Bitmap
        Dim stream As New MemoryStream



        bmfi.bfType = 19778 '((int)'M' << 8) | ((int)'B')
        bmfi.bfOffBits = 54
        bmfi.bfSize = (54 + tFrameHead.iWidth * tFrameHead.iHeight * 3)
        bmfi.bfReserved1 = 0
        bmfi.bfReserved2 = 0

        bmi.biBitCount = 24
        bmi.biClrImportant = 0
        bmi.biClrUsed = 0
        bmi.biCompression = 0
        bmi.biPlanes = 1
        bmi.biSize = 40
        bmi.biHeight = tFrameHead.iHeight
        bmi.biWidth = tFrameHead.iWidth
        bmi.biXPelsPerMeter = 0
        bmi.biYPelsPerMeter = 0
        bmi.biSizeImage = 0
        Dim bw As New System.IO.BinaryWriter(stream)
        Dim data() As Byte
        ReDim data(14 - 1)
        Dim ptr As IntPtr

        ptr = Marshal.AllocHGlobal(54)
        Marshal.StructureToPtr(bmfi, ptr, False)
        Marshal.Copy(ptr, data, 0, data.Length)
        bw.Write(data)
        ReDim data(40 - 1)
        Marshal.StructureToPtr(bmi, ptr, False)
        Marshal.Copy(ptr, data, 0, data.Length)
        bw.Write(data)
        ReDim data(tFrameHead.iWidth * tFrameHead.iHeight * 3 - 1)
        Marshal.Copy(pRgbBuffer, data, 0, data.Length)
        bw.Write(data)
        Marshal.FreeHGlobal(ptr)


        PictureBox1.Width = tFrameHead.iWidth
        PictureBox1.Height = tFrameHead.iHeight
        PictureBox1.Image = Image.FromStream(stream)
        'panel1.AutoScroll = True
        Me.Show()


        Return 0

    End Function

End Class