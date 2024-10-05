using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MVSDK;//使用SDK接口
using CameraHandle = System.Int32;
using MvApi = MVSDK.MvApi;
using System.IO;

namespace GPIO
{
    public partial class Form1 : Form
    {
        #region variable
        protected IntPtr m_Grabber = IntPtr.Zero;
        protected CameraHandle m_hCamera = 0;
        protected tSdkCameraDevInfo m_DevInfo;
        //protected pfnCameraGrabberFrameCallback m_FrameCallback;
        protected tSdkCameraCapbility tCameraCapability;  // 相机特性描述
        protected Boolean m_bRecording = false;
        protected int m_iInputIoCounts;
        //protected uint bchek;
        #endregion

        public Form1()
        {
            InitializeComponent();

            if (InitCamera() == true)
            {
                MvApi.CameraGrabber_GetCameraHandle(m_Grabber, out m_hCamera);
                MvApi.CameraGrabber_GetCameraDevInfo(m_Grabber, out m_DevInfo);
                MvApi.CameraGetCapability(m_hCamera, out tCameraCapability);

                if (tCameraCapability.iInputIoCounts == 0 && tCameraCapability.iOutputIoCounts == 0)
                {
                    MessageBox.Show("该型号相机没有GPIO，演示程序无法运行", "Information");
                    //Environment.Exit(0);
                    //return false;
                }

                groupBox1.Controls.Clear();
                groupBox1.Controls.Add(panel1);
                groupBox1.Controls.Add(panel2);
                groupBox1.Controls.Add(panel3);
                groupBox1.Controls.Add(panel4);
                groupBox1.Controls.Add(panel5);
                groupBox1.Controls.Add(panel6);
                groupBox1.Controls.Add(panel7);
                groupBox1.Controls.Add(panel8);
                groupBox1.Controls.Add(panel9);
                groupBox1.Controls.Add(panel10);

                m_iInputIoCounts = tCameraCapability.iInputIoCounts;

                for (int i = 0; i < m_iInputIoCounts; i++)
                {
                    if (groupBox1.Controls[i] is Panel)
                    {
                        Panel p = (Panel)groupBox1.Controls[i];
                        p.Enabled = true;
                    }
                }

                groupBox2.Controls.Clear();
                groupBox2.Controls.Add(panel11);
                groupBox2.Controls.Add(panel12);
                groupBox2.Controls.Add(panel13);
                groupBox2.Controls.Add(panel14);
                groupBox2.Controls.Add(panel15);
                groupBox2.Controls.Add(panel16);
                groupBox2.Controls.Add(panel17);
                groupBox2.Controls.Add(panel18);
                groupBox2.Controls.Add(panel19);
                groupBox2.Controls.Add(panel20);

                m_iInputIoCounts = tCameraCapability.iOutputIoCounts;

                for (int i = 0; i < m_iInputIoCounts; i++)
                {
                    if (groupBox2.Controls[i] is Panel)
                    {
                        Panel p = (Panel)groupBox2.Controls[i];
                        p.Enabled = true;
                    }
                }

                String str;
                //IO初始化为低电平，与控件上显示的同步
                for (int i = 0; i < tCameraCapability.iOutputIoCounts; i++)
                {
                    MvApi.CameraSetIOState(m_hCamera, i, 0);

                    str = String.Format("panel{0}", i + 1);

                    for (int m = 0; m < groupBox2.Controls.Count; m++)
                    {
                        if (groupBox1.Controls[m].Name == str)
                        {
                            Panel p = (Panel)groupBox2.Controls[m];

                            for (int j = 0; j < p.Controls.Count; j++)
                            {
                                if (p.Controls[j].TabIndex == 1)
                                {
                                    RadioButton r = (RadioButton)p.Controls[j];
                                    r.Checked = true;
                                }
                            }

                            break;
                        }
                    }
                }
            }
            else
            {
                Environment.Exit(0);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //bchek = 0;
        }

        private bool InitCamera()
        {
            tSdkCameraDevInfo[] DevList;
            MvApi.CameraEnumerateDevice(out DevList);
            int NumDev = (DevList != null ? DevList.Length : 0);

            CameraSdkStatus status;

            if (NumDev < 1)
            {
                MessageBox.Show("未扫描到相机");
                return false;
            }
            else if (NumDev == 1)
            {
                status = MvApi.CameraGrabber_Create(out m_Grabber, ref DevList[0]);
            }
            else
            {
                status = MvApi.CameraGrabber_CreateFromDevicePage(out m_Grabber);
            }

            if (status != CameraSdkStatus.CAMERA_STATUS_SUCCESS)
            {
                MessageBox.Show(String.Format("打开相机失败，原因：{0}", status));
                return false;
            }

            return true;
        }

        private void Camera_FormClosing(object sender, FormClosingEventArgs e)
        {
            //反初始化相机
            if (IntPtr.Zero != m_Grabber)
            {
                MvApi.CameraGrabber_Destroy(m_Grabber);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            String name;
            uint bchek = 0;

            m_iInputIoCounts = tCameraCapability.iInputIoCounts;

            for (int i = 0; i < m_iInputIoCounts; i++)                 //panel1开始循环遍历
            {
                MvApi.CameraGetIOState(m_hCamera, i, ref bchek);

                name = String.Format("panel{0}", i + 1);

                for (int j = 0; j < groupBox1.Controls.Count; j++)
                {
                    if (groupBox1.Controls[j].Name == name)
                    {
                        Panel p = (Panel)groupBox1.Controls[j];

                        for (int m = 0; m < p.Controls.Count; m++)
                        {
                            if (bchek != 0)
                            {
                                if (p.Controls[m].TabIndex == 2)
                                {
                                    RadioButton r = (RadioButton)p.Controls[m];

                                    r.Checked = true;
                                }

                            }
                            else
                            {
                                if (p.Controls[m].TabIndex == 1)
                                {
                                    RadioButton r = (RadioButton)p.Controls[m];

                                    r.Checked = true;
                                }
                            }

                        } //end for m

                        break;
                    } // end if groupBox1
                } //end for j
            }
        }

        private void radioButton22_CheckedChanged(object sender, EventArgs e)
        {
            bool b = false;
            uint uistate = 0;

            b = radioButton22.Checked;
            uistate = (uint)Convert.ToInt32(b);

            MvApi.CameraSetIOState(m_hCamera, 0, uistate);
        }

        private void radioButton24_CheckedChanged(object sender, EventArgs e)
        {
            bool b = false;
            uint uistate = 0;

            b = radioButton24.Checked;
            uistate = (uint)Convert.ToInt32(b);

            MvApi.CameraSetIOState(m_hCamera, 1, uistate);
        }

        private void radioButton26_CheckedChanged(object sender, EventArgs e)
        {
            bool b = false;
            uint uistate = 0;

            b = radioButton26.Checked;
            uistate = (uint)Convert.ToInt32(b);

            MvApi.CameraSetIOState(m_hCamera, 2, uistate);
        }

        private void radioButton28_CheckedChanged(object sender, EventArgs e)
        {
            bool b = false;
            uint uistate = 0;

            b = radioButton28.Checked;
            uistate = (uint)Convert.ToInt32(b);

            MvApi.CameraSetIOState(m_hCamera, 3, uistate);
        }

        private void radioButton30_CheckedChanged(object sender, EventArgs e)
        {
            bool b = false;
            uint uistate = 0;

            b = radioButton30.Checked;
            uistate = (uint)Convert.ToInt32(b);

            MvApi.CameraSetIOState(m_hCamera, 4, uistate);
        }

        private void radioButton32_CheckedChanged(object sender, EventArgs e)
        {
            bool b = false;
            uint uistate = 0;

            b = radioButton32.Checked;
            uistate = (uint)Convert.ToInt32(b);

            MvApi.CameraSetIOState(m_hCamera, 5, uistate);
        }

        private void radioButton38_CheckedChanged(object sender, EventArgs e)
        {
            bool b = false;
            uint uistate = 0;

            b = radioButton38.Checked;
            uistate = (uint)Convert.ToInt32(b);

            MvApi.CameraSetIOState(m_hCamera, 6, uistate);
        }

        private void radioButton34_CheckedChanged(object sender, EventArgs e)
        {
            bool b = false;
            uint uistate = 0;

            b = radioButton34.Checked;
            uistate = (uint)Convert.ToInt32(b);

            MvApi.CameraSetIOState(m_hCamera, 7, uistate);
        }

        private void radioButton40_CheckedChanged(object sender, EventArgs e)
        {
            bool b = false;
            uint uistate = 0;

            b = radioButton40.Checked;
            uistate = (uint)Convert.ToInt32(b);

            MvApi.CameraSetIOState(m_hCamera, 8, uistate);
        }

        private void radioButton36_CheckedChanged(object sender, EventArgs e)
        {
            bool b = false;
            uint uistate = 0;

            b = radioButton36.Checked;
            uistate = (uint)Convert.ToInt32(b);

            MvApi.CameraSetIOState(m_hCamera, 9, uistate);
        }

    }
}
