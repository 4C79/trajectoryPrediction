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
using System.Runtime.InteropServices;


namespace Basic
{
    public partial class Settings : Form
    {
        public CameraHandle m_hCamera;//相机句柄
        private int m_iResolutionIndex = 0;//选择的预设分辨率索引号
        private tSdkImageResolution m_tRoiResolution;//用户自定义的分辨率
        private bool m_bInited = false;

        public Settings()
        {
            InitializeComponent();
        }

        private void trackBar_RedGain_Scroll(object sender, EventArgs e)
        {
            if (this.ActiveControl != sender)
            {
                return;
            }

            int r = trackBar_RedGain.Value;
            int g = trackBar_GreenGain.Value;
            int b = trackBar_BlueGain.Value;

            //滚动后更新左边输入框的里值
            textBox_RedGain.Text = r.ToString();
            textBox_GreenGain.Text = g.ToString();
            textBox_BlueGain.Text = b.ToString();

            MvApi.CameraSetGain(m_hCamera, r, g, b);
        }

        private void textBox_RedGain_TextChanged(object sender, EventArgs e)
        {
            if (this.ActiveControl != sender)
            {
                return;
            }

            if (textBox_RedGain.Text == ""
                || textBox_GreenGain.Text == ""
                || textBox_BlueGain.Text == ""
                )
            {
                return;
            }

            string s1 = textBox_RedGain.Text;
            int r = Convert.ToInt32(s1);
            
            string s2 = textBox_GreenGain.Text;
            int g = Convert.ToInt32(s2);

            string s3 = textBox_BlueGain.Text;
            int b = Convert.ToInt32(s3);

            //输入框里的值改变后，更新滚动条。
            trackBar_RedGain.Value = r;
            trackBar_GreenGain.Value = g;
            trackBar_BlueGain.Value = b;

            MvApi.CameraSetGain(m_hCamera, r, g, b);
        }

        //button1_Click 用于彩色相机的一键白平衡。
        private void button1_Click(object sender, EventArgs e)
        {
            MvApi.CameraSetOnceWB(m_hCamera);

            //跟新RGB控件上的值
            UpdateRgbGainControls();
            
            this.Invalidate();
           
        }


        //饱和度滚动条事件
        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            if (this.ActiveControl != sender)
            {
                return;
            }
            int saturation = trackBar4.Value;
            textBox_Saturation.Text = saturation.ToString();
            MvApi.CameraSetSaturation(m_hCamera, saturation);
        }

        //饱和度输入框事件
        private void textBox_Saturation_TextChanged(object sender, EventArgs e)
        {
            
            if (this.ActiveControl != sender)
            {
                return;
            }

            if (textBox_Saturation.Text == "")
            {
                return;
            }

            int saturation = Convert.ToInt32(textBox_Saturation.Text);
            trackBar4.Value = saturation;
            MvApi.CameraSetSaturation(m_hCamera, saturation);
        }

        private void checkBox_MonoMode_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_MonoMode.Checked)
            {
                MvApi.CameraSetMonochrome(m_hCamera, 1);
            }
            else{
                MvApi.CameraSetMonochrome(m_hCamera, 0);
            }

        }

        private void checkBox_InverseImage_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_InverseImage.Checked)
            {
                MvApi.CameraSetInverse(m_hCamera, 1);
            }
            else
            {
                MvApi.CameraSetInverse(m_hCamera, 0);
            }
        }

        //通过滚动条对比度调节
        private void trackBar_Contrast_Scroll(object sender, EventArgs e)
        {
            if (this.ActiveControl != sender)
            {
                return;
            }

            int contrast = trackBar_Contrast.Value;
           
            //滚动后更新左边输入框的里值
            textBox_Contrast.Text = contrast.ToString();
            
            MvApi.CameraSetContrast(m_hCamera, contrast);
        }

        //通过文本输入框 对比度调节
        private void textBox_Contrast_TextChanged(object sender, EventArgs e)
        {
            if (this.ActiveControl != sender)
            {
                return;
            }

            if (textBox_Contrast.Text == "")
            {
                return;
            }

            int contrast = Convert.ToInt32(textBox_Contrast.Text);
            trackBar_Contrast.Value = contrast;
            MvApi.CameraSetContrast(m_hCamera, contrast);
        }

        //通过滚动条调节伽马
        private void trackBar_Gamma_Scroll(object sender, EventArgs e)
        {
            if (this.ActiveControl != sender)
            {
                return;
            }

            int gamma = trackBar_Gamma.Value;
            double fGamma = ((double)gamma) / 100.0;//SDK中伽马的取值范围是0到1000，对应界面上的0到10.0。 1.0的gamma为原始值
            //滚动后更新左边输入框的里值
            textBox_Gamma.Text = fGamma.ToString();

            MvApi.CameraSetGamma(m_hCamera, gamma);
        }

        //通过文本框调节伽马
        private void textBox_Gamma_TextChanged(object sender, EventArgs e)
        {
            if (this.ActiveControl != sender)
            {
                return;
            }

            if (textBox_Gamma.Text == "")
            {
                return;
            }

            double fGamma = Convert.ToDouble(textBox_Gamma.Text);
            int gamma = (int)(fGamma * 100.0);//SDK中伽马的取值范围是0到1000，对应界面上的0到10.0。 1.0的gamma为原始值
            trackBar_Contrast.Value = gamma;
            MvApi.CameraSetGamma(m_hCamera, gamma);
        }

        //通过滚动条调节锐度
        private void trackBar_Sharpness_Scroll(object sender, EventArgs e)
        {
            if (this.ActiveControl != sender)
            {
                return;
            }
            int sharpness = trackBar_Sharpness.Value;

            //滚动后更新左边输入框的里值
            textBox_Sharpness.Text = sharpness.ToString();

            MvApi.CameraSetSharpness(m_hCamera, sharpness);
        }
        //通过文本框调节锐度
        private void textBox_Sharpness_TextChanged(object sender, EventArgs e)
        {
            if (this.ActiveControl != sender)
            {
                return;
            }

            if (textBox_Sharpness.Text == "")
            {
                return;
            }

            int sharpness = Convert.ToInt32(textBox_Sharpness.Text);
            trackBar_Sharpness.Value = sharpness;
            MvApi.CameraSetSharpness(m_hCamera, sharpness);
        }

        //2D降噪
        private void checkBox_2DDenoise_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_2DDenoise.Checked == true)
            {
                MvApi.CameraSetNoiseFilter(m_hCamera, 1);
            }
            else
            {
                MvApi.CameraSetNoiseFilter(m_hCamera, 0);
            }
        }

        //3D降噪
        private void comboBox_3DDenoise_SelectedIndexChanged(object sender, EventArgs e)
        {
            int counts = comboBox_3DDenoise.SelectedIndex;

            if (counts == 0)//禁止
            {
                MvApi.CameraSetDenoise3DParams(m_hCamera, 0, 0,null);

            }
            else{
                MvApi.CameraSetDenoise3DParams(m_hCamera, 1, counts + 1, null);
            }
            
        }

        //图像水平方向翻转一次
        private void checkBox_HFlip_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_HFlip.Checked == true)
            {
                MvApi.CameraSetMirror(m_hCamera, 0, 1);
            }
            else{
                MvApi.CameraSetMirror(m_hCamera, 0, 0);
            }
        }

        //图像垂直方向翻转一次
        private void checkBox_VFlip_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_VFlip.Checked == true)
            {
                MvApi.CameraSetMirror(m_hCamera, 1, 1);
            }
            else
            {
                MvApi.CameraSetMirror(m_hCamera, 1, 0);
            }
        }

        //图像翻转90度
        private void radioButton_Rotate90_CheckedChanged(object sender, EventArgs e)
        {
            MvApi.CameraSetRotate(m_hCamera, 1);
        }
        //图像翻转180度
        private void radioButton_Rotate180_CheckedChanged(object sender, EventArgs e)
        {
            MvApi.CameraSetRotate(m_hCamera, 2);
        }
        //图像翻转270度
        private void radioButton__Rotate270_CheckedChanged(object sender, EventArgs e)
        {
            MvApi.CameraSetRotate(m_hCamera, 3);
        }
        //图像翻转禁止
        private void radioButton_Forbbiden_CheckedChanged(object sender, EventArgs e)
        {
            MvApi.CameraSetRotate(m_hCamera, 0);
        }

        //设置为预设分辨率
        private void radioButton_ResolutionPreset_CheckedChanged(object sender, EventArgs e)
        {

            
            tSdkImageResolution t;
            MvApi.CameraGetImageResolution(m_hCamera, out t);
            t.iIndex = m_iResolutionIndex;//切换预设分辨率， 只需要设定index值就行了。 其余的值可忽略，或者填0
            MvApi.CameraSetImageResolution(m_hCamera,ref t);
            UpdateResolution();
        }

        //设置为自定义分辨率
        private void radioButton_ResolutionROI_CheckedChanged(object sender, EventArgs e)
        {
            
            MvApi.CameraSetImageResolution(m_hCamera, ref m_tRoiResolution);
            UpdateResolution();
        }

        //选中一个预设分辨率进行设置
        private void comboBox_RresPreset_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_iResolutionIndex = comboBox_RresPreset.SelectedIndex;

            tSdkImageResolution t;
            MvApi.CameraGetImageResolution(m_hCamera, out t);
            t.iIndex = m_iResolutionIndex;//切换预设分辨率， 只需要设定index值就行了。 
            MvApi.CameraSetImageResolution(m_hCamera, ref t);

        }

        //可视化设置自定义分辨率。
        private void button_ROI_Click(object sender, EventArgs e)
        {
            tSdkImageResolution t;
            CameraSdkStatus status;
            MvApi.CameraGetImageResolution(m_hCamera, out t);
            status = MvApi.CameraCustomizeResolution(m_hCamera, ref t);
            if (status == CameraSdkStatus.CAMERA_STATUS_SUCCESS)
            {
                m_tRoiResolution = t;
                MvApi.CameraSetImageResolution(m_hCamera, ref m_tRoiResolution);
            }
        }
        
        //设置相机的采集模式，分为连续、软触发和硬触发3种模式。无论哪种模式，采集图像都是同一个接口。
        private void comboBox_TriggerMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iMode = comboBox_TriggerMode.SelectedIndex;
            MvApi.CameraSetTriggerMode(m_hCamera, iMode);//0表示连续模式，1是软触发，2是硬触发。
            button_SwTrigger.Enabled = (iMode == 1 ? true : false);
        }

        //设置外触发信号的触发方式，分为上边沿和下边沿两种。
        private void comboBox_ExtSignalMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iMode = comboBox_ExtSignalMode.SelectedIndex;
        }

        //设置外触发信号的去抖时间。
        private void button_SetJetterTime_Click(object sender, EventArgs e)
        {
            uint iJitterTime = System.Convert.ToUInt32(textBox_JetterTime.Text);
            MvApi.CameraSetExtTrigJitterTime(m_hCamera, iJitterTime);
        }

        //设置闪光灯的模式为自动。当相机在触发模式工作时，曝光的同时，会自动输出闪光灯同步控制信号。
        private void radioButton_StrobeModeAuto_CheckedChanged(object sender, EventArgs e)
        {
            MvApi.CameraSetStrobeMode(m_hCamera, (int)emStrobeControl.STROBE_SYNC_WITH_TRIG_AUTO);
        }

        //设置闪光灯模式为手动模式
        private void radioButton_StrobeModeManul_CheckedChanged(object sender, EventArgs e)
        {
            MvApi.CameraSetStrobeMode(m_hCamera, (int)emStrobeControl.STROBE_SYNC_WITH_TRIG_MANUAL);
        }

        //设置半自动模式下，闪光灯信号的有效极性。
        private void comboBox_StrobePriority_SelectedIndexChanged(object sender, EventArgs e)
        {
            MvApi.CameraSetStrobePolarity(m_hCamera, comboBox_StrobePriority.SelectedIndex);
        }

        //设置半自动模式下的闪光灯的延时时间。单位是微秒
        private void button_SetStrobeDelay_Click(object sender, EventArgs e)
        {
            int iDelay = System.Convert.ToInt32(textBox_StrobeDelayTime.Text);
            MvApi.CameraSetStrobeDelayTime(m_hCamera, (uint)iDelay);
        }

        //设置相机半自动模式下的闪光灯脉冲宽度。注意单位为微秒。
        private void button_SetStrobePulseWidth_Click(object sender, EventArgs e)
        {
            int iWidth;
            iWidth = System.Convert.ToInt32(textBox_StrobePulseWidth.Text);
            MvApi.CameraSetStrobePulseWidth(m_hCamera, (uint)iWidth);
        }

        //通过滚动条修改相机的模拟增益值
        private void textBox_AnalogGain_TextChanged(object sender, EventArgs e)
        {
            if (this.ActiveControl != sender)
            {
                return;
            }

            if (textBox_AnalogGain.Text == "")
            {
                return;
            }

            int iGain = System.Convert.ToInt32(textBox_AnalogGain.Text);
            trackBar_AnalogGain.Value = iGain;
           
            MvApi.CameraSetAnalogGain(m_hCamera, iGain);
        }

        //通过输入控件修改相机的模拟增益值
        private void trackBar_AnalogGain_Scroll(object sender, EventArgs e)
        {
            if (this.ActiveControl != sender)
            {
                return;
            }

            int iGain = trackBar_AnalogGain.Value;
            textBox_AnalogGain.Text = iGain.ToString();

            MvApi.CameraSetAnalogGain(m_hCamera, iGain);
        }

        //修改相机曝光参数
        private void textBox_ExposureTime_TextChanged(object sender, EventArgs e)
        {
            if (textBox_ExposureTime.Text == "")
            {
                return;
            }

            double dExpTime = System.Convert.ToDouble(textBox_ExposureTime.Text);
            MvApi.CameraSetExposureTime(m_hCamera, dExpTime);
        }

      

        private void UpdateResolution()
        {
            tSdkImageResolution tRes;
            MvApi.CameraGetImageResolution(m_hCamera, out tRes);
            

            if (tRes.iIndex == 0xff)//0xff表示自定义分辨率
            {
                
                comboBox_RresPreset.Enabled = false;
                button_ROI.Enabled = true;
            }
            else
            {
               
                comboBox_RresPreset.Enabled = true;
                button_ROI.Enabled = false;
                m_iResolutionIndex = tRes.iIndex;
            }


        }

        private void UpdateRgbGainControls()
        {
            int r, g, b;
            r = g = b = 0;
            MvApi.CameraGetGain(m_hCamera, ref r, ref g, ref b);

            trackBar_RedGain.Value = r;
            trackBar_GreenGain.Value = g;
            trackBar_BlueGain.Value = b;

            textBox_RedGain.Text = r.ToString();
            textBox_GreenGain.Text = g.ToString();
            textBox_BlueGain.Text = b.ToString();

        }

        //根据相机的当前参数，刷新界面上控件，做到同步。
        public void UpdateControls()
        {
            //根据相机描述信息，设定控件的参数调节范围.
            tSdkCameraCapbility tCameraDeviceInfo;
            MvApi.CameraGetCapability(m_hCamera, out tCameraDeviceInfo);

            //获取RGB三个同道的数字增益后更新界面
            trackBar_RedGain.SetRange(0, 399);
            trackBar_GreenGain.SetRange(0, 399);
            trackBar_BlueGain.SetRange(0, 399);
            UpdateRgbGainControls();

            //饱和度
            int saturation = 0;
            MvApi.CameraGetSaturation(m_hCamera, ref saturation);
            trackBar4.SetRange(0, 100);
            trackBar4.Value = saturation;
            textBox_Saturation.Text = saturation.ToString();

            //彩色转黑白模式
            uint bEnable = 0;
            MvApi.CameraGetMonochrome(m_hCamera,ref bEnable);
            checkBox_MonoMode.Checked = (bEnable == 1?true:false);

            //反色模式
            MvApi.CameraGetInverse(m_hCamera,ref bEnable);
            checkBox_InverseImage.Checked = (bEnable == 1?true:false);

            //对比度
            int contrast = 0;
            MvApi.CameraGetContrast(m_hCamera,ref contrast);
            trackBar_Contrast.SetRange(0, 200);
            trackBar_Contrast.Value = contrast;
            textBox_Contrast.Text = contrast.ToString();

            //分辨率
            comboBox_RresPreset.Items.Clear();

            // 填充分辨率列表
            tSdkImageResolution[] infos = new tSdkImageResolution[tCameraDeviceInfo.iImageSizeDesc]; 
            IntPtr ptr = tCameraDeviceInfo.pImageSizeDesc;
            for (int i = 0; i < infos.Length; i++)  
            {  
                infos[i] = (tSdkImageResolution)Marshal.PtrToStructure((IntPtr)((Int64)ptr + i * Marshal.SizeOf(new tSdkImageResolution())), typeof(tSdkImageResolution));
                string sDescription = System.Text.Encoding.Default.GetString ( infos[i].acDescription );
                comboBox_RresPreset.Items.Insert(comboBox_RresPreset.Items.Count, sDescription);
            }  
            //Marshal.FreeHGlobal (ptr);
            UpdateResolution();
            comboBox_RresPreset.SelectedIndex = m_iResolutionIndex;
            radioButton_ResolutionROI.Checked = !comboBox_RresPreset.Enabled;
            radioButton_ResolutionPreset.Checked = comboBox_RresPreset.Enabled;
            MvApi.CameraGetImageResolution(m_hCamera, out m_tRoiResolution);
            m_tRoiResolution.iIndex = 0xff;

            //伽马
            int gamma = 0;
            MvApi.CameraGetGamma(m_hCamera, ref gamma);
            trackBar_Gamma.SetRange(0, 1000);
            trackBar_Gamma.Value = gamma;
            double fGamma = (((double)gamma) / 100.0);//为了好理解，界面上1.0对应SDK的伽马值100，表示1倍的意思，伽马为1的时候，是默认值，相当于没有开启伽马校正。
            textBox_Gamma.Text = fGamma.ToString();

            //锐度
            int sharpness = 0;
            MvApi.CameraGetSharpness(m_hCamera, ref sharpness);
            trackBar_Sharpness.SetRange(0, 200);
            trackBar_Sharpness.Value = sharpness;
            textBox_Sharpness.Text = sharpness.ToString();

            //2D降噪
            MvApi.CameraGetNoiseFilterState(m_hCamera, ref bEnable);
            if(bEnable == 1)
            {
                checkBox_2DDenoise.Checked = true;
            }
            else{
                checkBox_2DDenoise.Checked = false;
            }

            //3D降噪
            int bUseWeight;
            int counts;
            int iEnable;
            MvApi.CameraGetDenoise3DParams(m_hCamera, out iEnable,out counts,out bUseWeight, null);
            //填充列表
            comboBox_3DDenoise.Items.Clear();
            comboBox_3DDenoise.Items.Insert(comboBox_3DDenoise.Items.Count,"禁用");
            int j ;
            for (j = 2;j <= 8;j++)
            {
                comboBox_3DDenoise.Items.Insert(comboBox_3DDenoise.Items.Count,j.ToString());
            }

            if (iEnable == 0)
            {
                comboBox_3DDenoise.SelectedIndex = 0;
            }
            else{
                comboBox_3DDenoise.SelectedIndex = counts - 1;
            }


            //水平镜像
            MvApi.CameraGetMirror(m_hCamera, 0, ref bEnable);
            if (bEnable == 1)
            {
                checkBox_HFlip.Checked = true;
            }
            else
            {
                checkBox_HFlip.Checked = false;
            }
            //垂直镜像
            MvApi.CameraGetMirror(m_hCamera, 1, ref bEnable);
            if (bEnable == 1)
            {
                checkBox_VFlip.Checked = true;
            }
            else
            {
                checkBox_VFlip.Checked = false;
            }

            //旋转图像
            int iRotate;
            MvApi.CameraGetRotate(m_hCamera, out iRotate);
            radioButton_Rotate90.Checked = (iRotate == 1 ? true : false);
            radioButton_Rotate180.Checked = (iRotate == 2 ? true : false);
            radioButton__Rotate270.Checked = (iRotate == 3 ? true : false);
            radioButton_Forbbiden.Checked = (iRotate == 0 ? true : false);

            //图像采集模式
            int iGrabMode = 0;
            MvApi.CameraGetTriggerMode(m_hCamera, ref iGrabMode);
            comboBox_TriggerMode.Items.Clear();
            comboBox_TriggerMode.Items.Insert(comboBox_TriggerMode.Items.Count, "连续采集模式");
            comboBox_TriggerMode.Items.Insert(comboBox_TriggerMode.Items.Count, "软触发模式");
            comboBox_TriggerMode.Items.Insert(comboBox_TriggerMode.Items.Count, "硬触发模式");
            comboBox_TriggerMode.SelectedIndex = iGrabMode;
            button_SwTrigger.Enabled = (iGrabMode == 1 ? true : false);

            //外触发信号模式
            int iSignalMode = 0;
            MvApi.CameraGetExtTrigSignalType(m_hCamera, ref iSignalMode);
            comboBox_ExtSignalMode.Items.Clear();
            comboBox_ExtSignalMode.Items.Insert(comboBox_ExtSignalMode.Items.Count, "上升沿触发");
            comboBox_ExtSignalMode.Items.Insert(comboBox_ExtSignalMode.Items.Count, "下降沿触发");
            comboBox_ExtSignalMode.SelectedIndex = iSignalMode;

            //外触发信号去抖时间
            uint uJitterTime = 0;
            MvApi.CameraGetExtTrigJitterTime(m_hCamera, ref uJitterTime);
            textBox_JetterTime.Text = uJitterTime.ToString();

            //闪光灯信号模式
            int iStrobMode = 0;
            MvApi.CameraGetStrobeMode(m_hCamera, ref iStrobMode);
            radioButton_StrobeModeAuto.Checked = (iStrobMode == 0 ? true : false);
            radioButton_StrobeModeManul.Checked = (iStrobMode == 1 ? true : false);

            //闪光灯半自动模式下的有效极性
            int uPriority = 0;
            MvApi.CameraGetStrobePolarity(m_hCamera, ref uPriority);
            comboBox_StrobePriority.Items.Clear();
            comboBox_StrobePriority.Items.Insert(comboBox_StrobePriority.Items.Count, "高电平有效");
            comboBox_StrobePriority.Items.Insert(comboBox_StrobePriority.Items.Count, "低电平有效");
            comboBox_StrobePriority.SelectedIndex = uPriority;

            //闪光灯半自动模式下的延时时间
            uint uDelayTime = 0;
            MvApi.CameraGetStrobeDelayTime(m_hCamera, ref uDelayTime);
            textBox_StrobeDelayTime.Text = uDelayTime.ToString();

            //闪光灯半自动模式下的脉冲宽度
            uint uPluseWidth = 0;
            MvApi.CameraGetStrobePulseWidth(m_hCamera, ref uPluseWidth);
            textBox_StrobePulseWidth.Text = uPluseWidth.ToString();


            //相机的模拟增益值
           
            trackBar_AnalogGain.SetRange((int)tCameraDeviceInfo.sExposeDesc.uiAnalogGainMin, (int)tCameraDeviceInfo.sExposeDesc.uiAnalogGainMax);

            //相机的曝光时间
            double dCameraExpTimeMin = 0;//最小时间,单位为
            double dCameraExpTimeMax = 0;//最大时间
            MvApi.CameraGetExposureLineTime(m_hCamera, ref dCameraExpTimeMin);//相机的曝光时间，最小值为一行时间像素。
            dCameraExpTimeMax = (dCameraExpTimeMin * (double)tCameraDeviceInfo.sExposeDesc.uiExposeTimeMax);
            label_ExpMin.Text = "曝光时间最小值：" + dCameraExpTimeMin.ToString() + "微秒";
            label_ExpMax.Text = "曝光时间最大值：" + dCameraExpTimeMax.ToString() + "微秒";

            uint uState = 0;
            MvApi.CameraGetAeState(m_hCamera, ref uState);

            radioButton_AutoExp.Checked = (uState == 1?true:false);
            radioButton_ManulExp.Checked = (uState == 0 ? true : false);

            UpdateExpsoureControls();
        }

        private void UpdateExpsoureControls()
        {
            uint uState = 0;
            MvApi.CameraGetAeState(m_hCamera, ref uState);

            if (uState == 1)
            {
                textBox_AnalogGain.Enabled = false;
                textBox_ExposureTime.Enabled = false;
                trackBar_AnalogGain.Enabled = false;

               
            }
            else
            {
                textBox_AnalogGain.Enabled = true;
                textBox_ExposureTime.Enabled = true;
                trackBar_AnalogGain.Enabled = true;

                int iAnalogGain = 0;
                MvApi.CameraGetAnalogGain(m_hCamera, ref iAnalogGain);

                trackBar_AnalogGain.Value = iAnalogGain;
                textBox_AnalogGain.Text = iAnalogGain.ToString();

                double dCameraExpTime = 0;
                MvApi.CameraGetExposureTime(m_hCamera, ref dCameraExpTime);
                textBox_ExposureTime.Text = dCameraExpTime.ToString();

             
            }
        }

        private void Settings_Shown(object sender, EventArgs e)
        {
            UpdateControls();
            m_bInited = true;
        }

        //恢复相机默认参数
        private void button_DefaultParam_Click(object sender, EventArgs e)
        {
            MvApi.CameraLoadParameter(m_hCamera, (int)emSdkParameterTeam.PARAMETER_TEAM_DEFAULT);
            UpdateControls();
            this.Refresh();
            MessageBox.Show("相机已经恢复默认参数");
        }

        //保存相机参数到指定的文件
        private void button_SaveParamToFile_Click(object sender, EventArgs e)
        {
            string FileName = "c:\\camera.config";//保存参数的路径和文件可以修改，但是后缀必须是config结尾。
            MvApi.CameraSaveParameterToFile(m_hCamera, FileName);
            MessageBox.Show("参数保存成功：" + FileName);

        }

        //从指定文件中加载相机参数
        private void button_LoadParamFromeFIle_Click(object sender, EventArgs e)
        {
            string FileName = "c:\\camera.config";
            MvApi.CameraReadParameterFromFile(m_hCamera, FileName);
            MessageBox.Show("参数加载成功：" + FileName);
            UpdateControls();
            this.Refresh();
        }

        private void radioButton_AutoExp_CheckedChanged(object sender, EventArgs e)
        {
            MvApi.CameraSetAeState(m_hCamera, 1);//设置为自动曝光模式
            UpdateExpsoureControls();
        }

        private void radioButton_ManulExp_CheckedChanged(object sender, EventArgs e)
        {
            MvApi.CameraSetAeState(m_hCamera, 0);//设置为手动曝光模式
            UpdateExpsoureControls();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (m_hCamera <= 0 || m_bInited == false)
            {
                return;
            }

            uint AeMode = 0;
            MvApi.CameraGetAeState(m_hCamera, ref AeMode);

            if (AeMode == 1)
            {
                int iGain = 0;
                double dExpTime = 0;
                MvApi.CameraGetAnalogGain(m_hCamera, ref iGain);
                MvApi.CameraGetExposureTime(m_hCamera, ref dExpTime);
                textBox_AnalogGain.Text = iGain.ToString();
                trackBar_AnalogGain.Value = iGain;
                textBox_ExposureTime.Text = dExpTime.ToString();
            }
        }

        private void button_SwTrigger_Click(object sender, EventArgs e)
        {
            MvApi.CameraSoftTriggerEx(m_hCamera, 1);//执行软触发时，会清空相机内部缓存，重新开始曝光取一张图像。
        }

        private void Settings_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        



    }
}
