
// ImageDefinitionDlg.cpp : 实现文件
//

#include "stdafx.h"
#include "ImageDefinition.h"
#include "ImageDefinitionDlg.h"
#include "afxdialogex.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


// CImageDefinitionDlg 对话框
enum
{
	WM_SET_DEF_VAL = WM_USER + 1000,
};




CImageDefinitionDlg::CImageDefinitionDlg(CWnd* pParent /*=NULL*/)
	: CDialogEx(CImageDefinitionDlg::IDD, pParent)
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
	m_hCamera = -1;
	m_iDispFrameNum = 0;
	m_bExit = FALSE;
	m_hDispThread = NULL;
	m_pFrameBuffer = NULL;
}

void CImageDefinitionDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CImageDefinitionDlg, CDialogEx)
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_BN_CLICKED(IDC_BUTTON_SETTINGS, &CImageDefinitionDlg::OnBnClickedButtonSettings)
	ON_WM_TIMER()
	ON_WM_DESTROY()
	ON_CBN_SELCHANGE(IDC_COMBO_ALG, &CImageDefinitionDlg::OnCbnSelchangeComboAlg)
	ON_MESSAGE(WM_SET_DEF_VAL, &CImageDefinitionDlg::OnSetDefValue)
END_MESSAGE_MAP()


// CImageDefinitionDlg 消息处理程序

BOOL CImageDefinitionDlg::OnInitDialog()
{
	CDialogEx::OnInitDialog();

	// 设置此对话框的图标。当应用程序主窗口不是对话框时，框架将自动
	//  执行此操作
	SetIcon(m_hIcon, TRUE);			// 设置大图标
	SetIcon(m_hIcon, FALSE);		// 设置小图标

	// TODO: 在此添加额外的初始化代码
	m_pDisp = GetDlgItem(IDC_STATIC_IMAGE);

	m_iAlg = 0;
	((CComboBox*)GetDlgItem(IDC_COMBO_ALG))->SetCurSel(m_iAlg);

	CameraSdkInit(1);
	if (!InitCamera())
	{
		EndDialog(0);
		return TRUE;
	}

	SetTimer(100, 1000, NULL);
	return TRUE;  // 除非将焦点设置到控件，否则返回 TRUE
}

// 如果向对话框添加最小化按钮，则需要下面的代码
//  来绘制该图标。对于使用文档/视图模型的 MFC 应用程序，
//  这将由框架自动完成。

void CImageDefinitionDlg::OnPaint()
{
	if (IsIconic())
	{
		CPaintDC dc(this); // 用于绘制的设备上下文

		SendMessage(WM_ICONERASEBKGND, reinterpret_cast<WPARAM>(dc.GetSafeHdc()), 0);

		// 使图标在工作区矩形中居中
		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;

		// 绘制图标
		dc.DrawIcon(x, y, m_hIcon);
	}
	else
	{
		CDialogEx::OnPaint();
	}
}

//当用户拖动最小化窗口时系统调用此函数取得光标
//显示。
HCURSOR CImageDefinitionDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}

BOOL CImageDefinitionDlg::InitCamera()
{
	tSdkCameraDevInfo sCameraList[10];
	INT iCameraNums;
	CameraSdkStatus status;
	CRect rect;
	tSdkCameraCapbility sCameraInfo;

	//枚举设备，获得设备列表
	//Enumerate camera
	iCameraNums = 10;//调用CameraEnumerateDevice前，先设置iCameraNums = 10，表示最多只读取10个设备，如果需要枚举更多的设备，请更改sCameraList数组的大小和iCameraNums的值

	if (CameraEnumerateDevice(sCameraList,&iCameraNums) != CAMERA_STATUS_SUCCESS || iCameraNums == 0)
	{
		MessageBox("No camera was found!");
		return FALSE;
	}

	//该示例中，我们只假设连接了一个相机。因此，只初始化第一个相机。(-1,-1)表示加载上次退出前保存的参数，如果是第一次使用该相机，则加载默认参数.
	//In this demo ,we just init the first camera.
	if ((status = CameraInit(&sCameraList[0],-1,-1,&m_hCamera)) != CAMERA_STATUS_SUCCESS)
	{
		CString msg;
		msg.Format("Failed to init the camera! Error code is %d",status);
		MessageBox(msg);
		return FALSE;
	}


	//Get properties description for this camera.
	CameraGetCapability(m_hCamera,&sCameraInfo);

	m_pFrameBuffer = (BYTE *)CameraAlignMalloc(sCameraInfo.sResolutionRange.iWidthMax*sCameraInfo.sResolutionRange.iHeightMax*4,16);	

	ASSERT(m_pFrameBuffer);

	//使用SDK封装好的显示接口
	//Use Mindvision SDK to display camera images.
	CameraDisplayInit(m_hCamera, m_pDisp->m_hWnd);
	m_pDisp->GetClientRect(&rect);
	//Set display window size
	CameraSetDisplaySize(m_hCamera,rect.right - rect.left,rect.bottom - rect.top);

	//通知SDK内部建该相机的属性页面。窗口名称为该相机的名称。您也可以根据SDK的接口来自己实现相机的属性配置页面，
	//但是我们建议您使用SDK内部自动创建的方式，来省去您在界面开发上的大量时间。
	//Create the settings window for the camera
	CameraCreateSettingPage(m_hCamera,GetSafeHwnd(), sCameraList[0].acFriendlyName,NULL,0,0);

#ifdef USE_CALLBACK_GRAB_IMAGE //如果要使用回调函数方式，定义USE_CALLBACK_GRAB_IMAGE这个宏
	//Set the callback for image capture
	SDK_TRACE(CameraSetCallbackFunction(m_hCamera,GrabImageCallback,(PVOID)this,NULL),gLanguage?"设置图像抓取的回调函数":"Set image grab call back function");
#else
	m_hDispThread = (HANDLE)_beginthreadex(NULL, 0, &uiDisplayThread, this, 0,  &m_threadID);
	ASSERT (m_hDispThread); 
	SetThreadPriority(m_hDispThread,THREAD_PRIORITY_HIGHEST);
#endif
	//Tell the camera begin to sendding image
	CameraPlay(m_hCamera);
	return TRUE;
}

void CImageDefinitionDlg::OnBnClickedButtonSettings()
{
	CameraShowSettingPage(m_hCamera, TRUE);
}

void CImageDefinitionDlg::OnTimer(UINT_PTR nIDEvent)
{
	CString strStatusText;
	int iTimeCurrnet = 0;
	static int iDispNum = 0;

	//从SDK内部获得相机的采集总帧数，丢帧数等等。
	CameraGetFrameStatistic(m_hCamera, &m_sFrameCount);
	iTimeCurrnet = GetTickCount();

	if (0 != iTimeCurrnet-m_iTimeLast)
	{
		m_fCapFps = (float)((m_sFrameCount.iCapture - m_sFrameLast.iCapture)*1000.0)/(float)(iTimeCurrnet-m_iTimeLast);
		m_fDispFps = (float)((m_iDispFrameNum - iDispNum)*1000.0)/(float)(iTimeCurrnet-m_iTimeLast);
	}
	else
	{
		return;
	}        

	m_iTimeLast = iTimeCurrnet;

	//Update frame information
	strStatusText.Format("| 图像分辨率:%d*%d | 显示帧率:%4.2f FPS | 捕获帧率:%4.2f FPS |",
			m_sFrInfo.iWidth, m_sFrInfo.iHeight,
			m_fDispFps, m_fCapFps);	
	GetDlgItem(IDC_STATIC_STATUS)->SetWindowText(strStatusText);

	m_sFrameLast.iCapture = m_sFrameCount.iCapture;
	m_sFrameLast.iLost = m_sFrameCount.iLost;
	m_sFrameLast.iTotal = m_sFrameCount.iTotal;
	iDispNum = m_iDispFrameNum;
}

void CImageDefinitionDlg::OnDestroy()
{
	//反初始化相机
	if (m_hCamera > 0)
	{
		if (NULL != m_hDispThread)
		{
			//等待采集线程结束
			m_bExit = TRUE;
			::WaitForSingleObject(m_hDispThread, INFINITE);
			CloseHandle(m_hDispThread);
			m_hDispThread = NULL;
		}

		//反初始化相机。
		CameraUnInit(m_hCamera);
		m_hCamera = 0;
	}

	if (m_pFrameBuffer)
	{
		CameraAlignFree(m_pFrameBuffer);
		m_pFrameBuffer = NULL;
	}

	CDialogEx::OnDestroy();
}

UINT WINAPI CImageDefinitionDlg::uiDisplayThread(LPVOID lpParam)
{
	tSdkFrameHead 	sFrameInfo;
	CImageDefinitionDlg* 		pThis = (CImageDefinitionDlg*)lpParam;
	BYTE*			pbyBuffer;
	CameraSdkStatus status;


	while (!pThis->m_bExit)
	{   

		if(CameraGetImageBuffer(pThis->m_hCamera,&sFrameInfo,&pbyBuffer,1000) == CAMERA_STATUS_SUCCESS)
		{	
			//将获得的原始数据转换成RGB格式的数据，同时经过ISP模块，对图像进行降噪，边沿提升，颜色校正等处理。
			//我公司大部分型号的相机，原始数据都是Bayer格式的
			status = CameraImageProcess(pThis->m_hCamera, pbyBuffer, pThis->m_pFrameBuffer,&sFrameInfo);//连续模式

			//分辨率改变了，则刷新背景
			if (pThis->m_sFrInfo.iWidth != sFrameInfo.iWidth || pThis->m_sFrInfo.iHeight != sFrameInfo.iHeight)
			{
				pThis->m_sFrInfo.iWidth = sFrameInfo.iWidth;
				pThis->m_sFrInfo.iHeight = sFrameInfo.iHeight;
				pThis->Invalidate();
			}

			if(status == CAMERA_STATUS_SUCCESS)
			{
				double def_val;
				if (CameraEvaluateImageDefinition(pThis->m_hCamera, pThis->m_iAlg, pThis->m_pFrameBuffer, &sFrameInfo, &def_val) == CAMERA_STATUS_SUCCESS)
				{
					pThis->PostMessage(WM_SET_DEF_VAL, *(DWORD*)&def_val, *((DWORD*)&def_val + 1));
				}

				//调用SDK封装好的显示接口来显示图像,您也可以将m_pFrameBuffer中的RGB数据通过其他方式显示，比如directX,OpengGL,等方式。
				CameraImageOverlay(pThis->m_hCamera, pThis->m_pFrameBuffer, &sFrameInfo);
				CameraDisplayRGB24(pThis->m_hCamera, pThis->m_pFrameBuffer, &sFrameInfo);
				pThis->m_iDispFrameNum++;
			}

			//在成功调用CameraGetImageBuffer后，必须调用CameraReleaseImageBuffer来释放获得的buffer。
			//否则再次调用CameraGetImageBuffer时，程序将被挂起，知道其他线程中调用CameraReleaseImageBuffer来释放了buffer
			CameraReleaseImageBuffer(pThis->m_hCamera,pbyBuffer);

			memcpy(&pThis->m_sFrInfo,&sFrameInfo,sizeof(tSdkFrameHead));
		}

	}

	return 0;
}

void CImageDefinitionDlg::OnCbnSelchangeComboAlg()
{
	m_iAlg = ((CComboBox*)GetDlgItem(IDC_COMBO_ALG))->GetCurSel();
}

LRESULT CImageDefinitionDlg::OnSetDefValue(WPARAM wParam, LPARAM lParam)
{
	CString strText;
	double val;
	*(DWORD*)&val = (DWORD)wParam;
	*((DWORD*)&val + 1) = (DWORD)lParam;
	strText.Format("%.2lf", val);
	SetDlgItemText(IDC_EDIT_DEFINITION_VALUE, strText);
	return 0;
}
