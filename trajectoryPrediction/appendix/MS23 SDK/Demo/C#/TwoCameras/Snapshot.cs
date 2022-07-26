//BIG5 TRANS ALLOWED
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MVSDK;
using System.Runtime.InteropServices;
using System.IO;

namespace Snapshot
{
    public partial class SnapshotDlg : Form
    {
        public SnapshotDlg()
        {
            InitializeComponent();
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct BITMAPFILEHEADER
        {
            public ushort bfType;
            public uint bfSize;
            public ushort bfReserved1;
            public ushort bfReserved2;
            public uint bfOffBits;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct BITMAPINFOHEADER
        {
            public uint biSize;
            public int biWidth;
            public int biHeight;
            public ushort biPlanes;
            public ushort biBitCount;
            public uint biCompression;
            public uint biSizeImage;
            public int biXPelsPerMeter;
            public int biYPelsPerMeter;
            public uint biClrUsed;
            public uint biClrImportant;
            public const int BI_RGB = 0;
        } 

        public void UpdateImage(ref tSdkFrameHead tFrameHead,IntPtr pRgbBuffer)
        {
            BITMAPINFOHEADER bmi;
            BITMAPFILEHEADER bmfi;

            bmfi.bfType = ((int)'M' << 8) | ((int)'B');
            bmfi.bfOffBits = 54;
            bmfi.bfSize = (uint)(54 + tFrameHead.iWidth * tFrameHead.iHeight * 3);
            bmfi.bfReserved1 = 0;
            bmfi.bfReserved2 = 0;

            bmi.biBitCount = 24;
            bmi.biClrImportant = 0;
            bmi.biClrUsed = 0;
            bmi.biCompression = 0;
            bmi.biPlanes = 1;
            bmi.biSize = 40;
            bmi.biHeight = tFrameHead.iHeight;
            bmi.biWidth = tFrameHead.iWidth;
            bmi.biXPelsPerMeter = 0;
            bmi.biYPelsPerMeter = 0;
            bmi.biSizeImage = 0;

            MemoryStream stream = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(stream);
            byte[] data = new byte[14];
            IntPtr ptr = Marshal.AllocHGlobal(54);
            Marshal.StructureToPtr((object)bmfi, ptr, false);
            Marshal.Copy(ptr, data, 0, data.Length);
            bw.Write(data);
            data = new byte[40];
            Marshal.StructureToPtr((object)bmi, ptr, false);
            Marshal.Copy(ptr, data, 0, data.Length);
            bw.Write(data);
            data = new byte[tFrameHead.iWidth * tFrameHead.iHeight * 3];
            Marshal.Copy(pRgbBuffer, data, 0, data.Length);
            bw.Write(data);
            Marshal.FreeHGlobal(ptr);

            
            SnapshotBox.Width = tFrameHead.iWidth;
            SnapshotBox.Height = tFrameHead.iHeight;
            SnapshotBox.Image = Image.FromStream(stream);
            panel1.AutoScroll = true;
            //SnapshotBox.Layout = 
        }

        private void SnapshotDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        private void SnapshotBox_Click(object sender, EventArgs e)
        {

        }

    }
}