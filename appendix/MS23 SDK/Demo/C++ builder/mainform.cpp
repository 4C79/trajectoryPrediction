//---------------------------------------------------------------------------
//BIG5 TRANS ALLOWED
#include <vcl.h>
#pragma hdrstop

#include "mainform.h"
#include "stdio.h"
//---------------------------------------------------------------------------
#pragma package(smart_init)
#pragma resource "*.dfm"

#include "..\VC++\Include\CameraApi.h"
#include <process.h>
                             
TForm1 *Form1;
//相机句柄  ，多相机同时访问，通过不同的句柄区分。
int m_hCamera = 0;
//相机的图像缓冲区
unsigned char* m_pRgbBuffer = NULL;
//相机当前显示的图像总帧数
int  m_iDispNum = 0;
//上次统计时，相机显示的总帧数
int  m_iLastDisNum  = 0;
//抓图线程的句柄
HANDLE  m_hDispThread = NULL;
//抓图线程退出标志
int m_bExit = 0;
//上一帧包头信息
tSdkFrameHead m_sFrInfo;
//抓图线程ID
UINT m_threadID;

UINT WINAPI uCaptureThread(LPVOID lpvParam)
{
	TForm1* Form1 = (TForm1*)lpvParam;
	BYTE      *pbyBuffer;
	CameraSdkStatus status;


	tSdkFrameHead sFrameInfo;

	while (!m_bExit)
        {
		if(CameraGetImageBuffer(m_hCamera,&sFrameInfo,&pbyBuffer,1000) == CAMERA_STATUS_SUCCESS)
		{

			status = CameraImageProcess(m_hCamera, pbyBuffer, m_pRgbBuffer,&sFrameInfo);

			if (m_sFrInfo.iWidth != sFrameInfo.iWidth || m_sFrInfo.iHeight != sFrameInfo.iHeight)//切换分辨率大小时，擦除多余部分。
			{
				m_sFrInfo.iWidth = sFrameInfo.iWidth;
				m_sFrInfo.iHeight = sFrameInfo.iHeight;
                                Form1->Panel1->Invalidate();
			}

                        CameraDisplayRGB24(m_hCamera, m_pRgbBuffer, &sFrameInfo);
                        m_iDispNum++;


                        CameraReleaseImageBuffer(m_hCamera,pbyBuffer);

			memcpy(&m_sFrInfo,&sFrameInfo,sizeof(tSdkFrameHead));
		}

		
    }

	_endthreadex(0);

    return 0;
}


//---------------------------------------------------------------------------
__fastcall TForm1::TForm1(TComponent* Owner)
        : TForm(Owner)
{

        if (m_hCamera > 0)
        {
             Label1->SetTextBuf("123");
        }
}
//----------按下后暂停-----------------------------------------------------------------//----------按下后继续预览。（默认打开DEMO后自动预览）-----------------------------------------------------------------
//----------按下后保存图片到文件中-----------------------------------------------------------------
//----------按下后弹出相机设置对话框-----------------------------------------------------------------


void __fastcall TForm1::FormActivate(TObject *Sender)
{
        int n;
        CameraSdkStatus status;
        tSdkCameraCapbility sCameraCap;

        tSdkCameraDevInfo tEnumInfos[4];
        n = 4;
        //枚举设备，如果没找到相机，则打印出错信息后，退出
        CameraEnumerateDevice(tEnumInfos,&n);
        if (n == 0)
        {
           ShowMessage("No camera!");
           return;
        }

        status =  CameraInit(&tEnumInfos[0], -1,-1 ,&m_hCamera);

        if (CAMERA_STATUS_SUCCESS != status)
        {

           char msg[64];
           sprintf(msg,"Camera init failed ,error code is %d",status);
           ShowMessage(msg);
           return;
        }

         
        CameraGetCapability(m_hCamera, &sCameraCap);

        //申请一个RGB24的buffer
        m_pRgbBuffer = (BYTE *)CameraAlignMalloc(sCameraCap.sResolutionRange.iWidthMax*
        sCameraCap.sResolutionRange.iHeightMax*3,16);
        //如果希望输出RGB32的格式，则需要申请sCameraCap.sResolutionRange.iWidthMax*
        //sCameraCap.sResolutionRange.iHeightMax*4大小的buffer

         CameraDisplayInit(m_hCamera,Panel1->Handle);

	 CameraSetDisplaySize(m_hCamera,Panel1->Width,Panel1->Height);

         CameraCreateSettingPage(m_hCamera,Form1->Handle,tEnumInfos[0].acFriendlyName,NULL,0,0);

         m_bExit = 0;
         m_hDispThread = (HANDLE)_beginthreadex(NULL, 0, &uCaptureThread, (VOID*)Form1, 0, (unsigned int*) &m_threadID);

         //让相机开始采集图像
         CameraPlay(m_hCamera);

}
//---------------------------------------------------------------------------
void __fastcall TForm1::Button_settingsClick(TObject *Sender)
{
        //弹出内部集成的相机配置框进行设置。如果需要调用函数设置某个参数，请参考API手册，直接调用即可。
        if (m_hCamera > 0)
        {
             CameraShowSettingPage(m_hCamera,1);
        }
}
//---------------------------------------------------------------------------
void __fastcall TForm1::FormClose(TObject *Sender, TCloseAction &Action)
{
        if(m_hCamera > 0)
        {
            m_bExit = 1;  //让采集线程退出
            WaitForSingleObject(m_hDispThread, INFINITE);
            
            CameraUnInit(m_hCamera);
            
            m_hCamera = 0;
        }

        if (m_pRgbBuffer)
        {
            CameraAlignFree(m_pRgbBuffer);
            m_pRgbBuffer = NULL;
        }
}
//---------------------------------------------------------------------------
void __fastcall TForm1::Button_pauseClick(TObject *Sender)
{
        if (m_hCamera > 0)
        {
            CameraPause(m_hCamera);
        }
}
//---------------------------------------------------------------------------
void __fastcall TForm1::Button_StartClick(TObject *Sender)
{
        if (m_hCamera > 0)
        {
            CameraPlay(m_hCamera);
        }
}
//---------------------------------------------------------------------------
void __fastcall TForm1::Timer1Timer(TObject *Sender)
{
        char msg[128];
        tSdkFrameStatistic sFrameCount;
        static int iLastCaptureFrames = 0;
        CameraGetFrameStatistic(m_hCamera, &sFrameCount);

        sprintf(msg,"图像尺寸：%d X %d|显示总帧数:%d|采集帧率:%d|显示帧率:%d",
        m_sFrInfo.iWidth,m_sFrInfo.iHeight,sFrameCount.iTotal ,sFrameCount.iTotal - iLastCaptureFrames,m_iDispNum - m_iLastDisNum);
        m_iLastDisNum = m_iDispNum;
        iLastCaptureFrames =  sFrameCount.iTotal;

        Label1->SetTextBuf(msg);
}
//---------------------------------------------------------------------------
void __fastcall TForm1::Button_snapshotClick(TObject *Sender)
{
        tSdkFrameHead FrameInfo;
	BYTE *pRawBuffer;
	BYTE *pRgbBuffer;
        char msg[64];

        tSdkImageResolution sImageSize;
        CameraSdkStatus status;

        memset(&sImageSize,0,sizeof(tSdkImageResolution));
	sImageSize.iIndex = 0xff;  //设置成0xff表示和预览一样的分辨率抓拍
        //设置抓拍的分辨率
        CameraSetResolutionForSnap(m_hCamera,&sImageSize);

		// !!!!!!注意：CameraSnapToBuffer 会切换分辨率拍照，速度较慢。做实时处理，请用CameraGetImageBuffer函数取图或者回调函数。
        if((status = CameraSnapToBuffer(m_hCamera,&FrameInfo,&pRawBuffer,1000)) != CAMERA_STATUS_SUCCESS)
	{

		ShowMessage("Snapshot failed,is camera in pause mode?");
		return;
	}
	else
	{

		//成功抓拍后，保存到文件


		//申请一个buffer，用来将获得的原始数据转换为RGB数据，并同时获得图像处理效果
		pRgbBuffer = (BYTE *)CameraAlignMalloc(FrameInfo.iWidth*FrameInfo.iHeight*3,16);
		//Process the raw data,and get the return image in RGB format
		CameraImageProcess(m_hCamera,pRawBuffer,pRgbBuffer,&FrameInfo);

		 //Release the buffer which get from CameraSnapToBuffer or CameraGetImageBuffer
		CameraReleaseImageBuffer(m_hCamera,pRawBuffer);

		//CameraSaveImage 保存图像，这里仅仅演示如何保存BMP图像。如果需要保存成其他格式的，里如果JPG,PNG,RAW等，
		//请参考我们的Snapshot例程。或者参阅SDK手册中有关CameraSaveImage接口的详细说明
		if((status = CameraSaveImage(m_hCamera,"C:\\test.bmp",pRgbBuffer,&FrameInfo,FILE_BMP,100)) != CAMERA_STATUS_SUCCESS)
		{
			ShowMessage("Failed to save image");
		}
		else
		{
			sprintf(msg,"Image save in:[%s]","C\\test.bmp");
			ShowMessage(msg);
		}

		CameraAlignFree(pRgbBuffer);
		
	}
}
//---------------------------------------------------------------------------
