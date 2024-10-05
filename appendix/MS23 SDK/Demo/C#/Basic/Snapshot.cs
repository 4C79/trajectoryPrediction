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

        public void UpdateImage(ref tSdkFrameHead tFrameHead,IntPtr pRgbBuffer)
        {
            SnapshotBox.Width = tFrameHead.iWidth;
            SnapshotBox.Height = tFrameHead.iHeight;
            SnapshotBox.Image = MvApi.CSharpImageFromFrame(pRgbBuffer, ref tFrameHead);
            panel1.AutoScroll = true;
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