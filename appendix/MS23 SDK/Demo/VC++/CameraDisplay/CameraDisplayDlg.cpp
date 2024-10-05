
// CameraDisplayDlg.cpp : 实现文件
//

#include "stdafx.h"
#include "CameraDisplay.h"
#include "CameraDisplayDlg.h"
#include "afxdialogex.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


extern int gLanguage;

// CCameraDisplayDlg 对话框



CCameraDisplayDlg::CCameraDisplayDlg(CWnd* pParent /*=NULL*/)
	: CDialogEx(CCameraDisplayDlg::IDD, pParent)
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
	m_bPause = TRUE;
	m_hCamera = -1;
	m_iDispFrameNum = 0;
	m_bExit = FALSE;
	m_hDispThread = NULL;
	m_pFrameBuffer = NULL;
}

void CCameraDisplayDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CCameraDisplayDlg, CDialogEx)
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_WM_TIMER()
	ON_WM_DESTROY()
	ON_BN_CLICKED(IDC_BUTTON_SETTINGS, &CCameraDisplayDlg::OnBnClickedButtonSettings)
	ON_BN_CLICKED(IDC_RADIO_SCALE, &CCameraDisplayDlg::OnBnClickedRadioScale)
	ON_BN_CLICKED(IDC_RADIO_DIRECT, &CCameraDisplayDlg::OnBnClickedRadioDirect)
	ON_WM_HSCROLL()
	ON_WM_VSCROLL()
END_MESSAGE_MAP()


// CCameraDisplayDlg 消息处理程序

BOOL CCameraDisplayDlg::OnInitDialog()
{
	CDialogEx::OnInitDialog();

	// 设置此对话框的图标。当应用程序主窗口不是对话框时，框架将自动
	//  执行此操作
	SetIcon(m_hIcon, TRUE);			// 设置大图标
	SetIcon(m_hIcon, FALSE);		// 设置小图标

	m_pDisp = GetDlgItem(IDC_STATIC_DISP);
	((CButton*)GetDlgItem(IDC_RADIO_SCALE))->SetCheck(BST_CHECKED);
	OnBnClickedRadioScale();

	// TODO: 在此添加额外的初始化代码
	CameraSdkInit(gLanguage);
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

void CCameraDisplayDlg::OnPaint()
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
HCURSOR CCameraDisplayDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}

BOOL CCameraDisplayDlg::InitCamera()
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
		MessageBox(msg +"，原因是" + CameraGetErrorString(status));
		return FALSE;
	}


	//Get properties description for this camera.
	CameraGetCapability(m_hCamera,&sCameraInfo);

	m_pFrameBuffer = (BYTE *)CameraAlignMalloc(sCameraInfo.sResolutionRange.iWidthMax*sCameraInfo.sResolutionRange.iHeightMax*4,16);	

	ASSERT(m_pFrameBuffer);

	if (sCameraInfo.sIspCapacity.bMonoSensor)
	{
		// 黑白相机输出8位灰度
		// Black and white camera output 8-bit grayscale
		CameraSetIspOutFormat(m_hCamera, CAMERA_MEDIA_TYPE_MONO8);
	}

	//使用SDK封装好的显示接口
	//Use  SDK to display camera images.
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
	m_bPause = FALSE;
	return TRUE;
}

BOOL CCameraDisplayDlg::ResetScrollBar(int iFrameW, int iFrameH)
{
	CRect rc;
	m_pDisp->GetClientRect(rc);

	int MaxHPos = iFrameW - rc.Width();
	int MaxVPos = iFrameH - rc.Height();
	if (MaxHPos > 0)
	{
		SetScrollRange(SB_HORZ, 0, MaxHPos);
		EnableScrollBar(SB_HORZ, ESB_ENABLE_BOTH);
	}
	else
	{
		EnableScrollBar(SB_HORZ, ESB_DISABLE_BOTH);
	}
	if (MaxVPos > 0)
	{
		SetScrollRange(SB_VERT, 0, MaxVPos);
		EnableScrollBar(SB_VERT, ESB_ENABLE_BOTH);
	}
	else
	{
		EnableScrollBar(SB_VERT, ESB_DISABLE_BOTH);
	}

	SetScrollPos(SB_HORZ, 0);
	SetScrollPos(SB_VERT, 0);
	CameraSetDisplayOffset(m_hCamera, 0, 0);
	return TRUE;
}

void CCameraDisplayDlg::OnTimer(UINT_PTR nIDEvent)
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
	if (gLanguage != 0)//chinese
	{
		strStatusText.Format("| 图像分辨率:%d*%d | 显示帧率:%4.2f FPS | 捕获帧率:%4.2f FPS |",
			m_sFrInfo.iWidth, m_sFrInfo.iHeight,
			m_fDispFps, m_fCapFps);
	}
	else//english
	{
		strStatusText.Format("| Resolution:%d*%d | Display rate:%4.2f FPS | Capture rate:%4.2f FPS |",
			m_sFrInfo.iWidth, m_sFrInfo.iHeight,
			m_fDispFps, m_fCapFps);
	}
	GetDlgItem(IDC_STATIC_INFORMATION)->SetWindowText(strStatusText);

	m_sFrameLast.iCapture = m_sFrameCount.iCapture;
	m_sFrameLast.iLost = m_sFrameCount.iLost;
	m_sFrameLast.iTotal = m_sFrameCount.iTotal;
	iDispNum = m_iDispFrameNum;
}

void CCameraDisplayDlg::OnDestroy()
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

UINT WINAPI CCameraDisplayDlg::uiDisplayThread(LPVOID lpParam)
{
	tSdkFrameHead 	sFrameInfo;
	CCameraDisplayDlg* 		pThis = (CCameraDisplayDlg*)lpParam;
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

				if (!pThis->m_bScaleDisp)
				{
					pThis->ResetScrollBar(sFrameInfo.iWidth, sFrameInfo.iHeight);
				}
			}

			if(status == CAMERA_STATUS_SUCCESS)
			{
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

void CCameraDisplayDlg::OnBnClickedButtonSettings()
{
	CameraShowSettingPage(m_hCamera, TRUE);
}

void CCameraDisplayDlg::OnBnClickedRadioScale()
{
	ShowScrollBar(SB_BOTH, FALSE);
	CameraSetDisplayMode(m_hCamera, DISPLAYMODE_SCALE);
	m_pDisp->Invalidate();
	m_bScaleDisp = TRUE;
}

void CCameraDisplayDlg::OnBnClickedRadioDirect()
{
	ShowScrollBar(SB_BOTH, TRUE);
	ResetScrollBar(m_sFrInfo.iWidth, m_sFrInfo.iHeight);
	CameraSetDisplayMode(m_hCamera, DISPLAYMODE_REAL);
	m_pDisp->Invalidate();
	m_bScaleDisp = FALSE;
}

void CCameraDisplayDlg::OnHScroll(UINT nSBCode, UINT nPos, CScrollBar* pScrollBar)
{
	int minpos = 0;
	int maxpos = 0;
	GetScrollRange(SB_HORZ, &minpos, &maxpos);

	// Get the current position of scroll box.  
	int curpos = GetScrollPos(SB_HORZ);  

	// Determine the new position of scroll box.  
	switch (nSBCode)  
	{  
	case SB_LEFT:      // Scroll to far left.  
		curpos = minpos;  
		break;  

	case SB_RIGHT:      // Scroll to far right.  
		curpos = maxpos;  
		break;  

	case SB_ENDSCROLL:   // End scroll.  
		break;  

	case SB_LINELEFT:      // Scroll left.  
		curpos = max(minpos, curpos - 10);  
		break;  

	case SB_LINERIGHT:   // Scroll right.  
		curpos = min(maxpos, curpos + 10);
		break;

	case SB_PAGELEFT:    // Scroll one page left.  
		if (curpos > minpos)  
			curpos = max(minpos, curpos - 100);  
		break;  

	case SB_PAGERIGHT:      // Scroll one page right.  
		if (curpos < maxpos)  
			curpos = min(maxpos, curpos + 100);
		break;  

	case SB_THUMBPOSITION: // Scroll to absolute position. nPos is the position  
		curpos = nPos;      // of the scroll box at the end of the drag operation.  
		break;  

	case SB_THUMBTRACK:   // Drag scroll box to specified position. nPos is the  
		curpos = nPos;     // position that the scroll box has been dragged to.  
		break;  
	}

	SetScrollPos(SB_HORZ, curpos);
	CameraSetDisplayOffset(m_hCamera, curpos, -1);

	CDialogEx::OnHScroll(nSBCode, curpos, pScrollBar);
}

void CCameraDisplayDlg::OnVScroll(UINT nSBCode, UINT nPos, CScrollBar* pScrollBar)
{
	int minpos = 0;
	int maxpos = 0;
	GetScrollRange(SB_VERT, &minpos, &maxpos);

	// Get the current position of scroll box.  
	int curpos = GetScrollPos(SB_VERT);  

	// Determine the new position of scroll box.  
	switch (nSBCode)  
	{  
	case SB_LEFT:      // Scroll to far left.  
		curpos = minpos;  
		break;  

	case SB_RIGHT:      // Scroll to far right.  
		curpos = maxpos;  
		break;  

	case SB_ENDSCROLL:   // End scroll.  
		break;  

	case SB_LINELEFT:      // Scroll left.  
		curpos = max(minpos, curpos - 10);  
		break;  

	case SB_LINERIGHT:   // Scroll right.  
		curpos = min(maxpos, curpos + 10);
		break;

	case SB_PAGELEFT:    // Scroll one page left.  
		if (curpos > minpos)  
			curpos = max(minpos, curpos - 100);  
		break;  

	case SB_PAGERIGHT:      // Scroll one page right.  
		if (curpos < maxpos)  
			curpos = min(maxpos, curpos + 100);
		break;  

	case SB_THUMBPOSITION: // Scroll to absolute position. nPos is the position  
		curpos = nPos;      // of the scroll box at the end of the drag operation.  
		break;  

	case SB_THUMBTRACK:   // Drag scroll box to specified position. nPos is the  
		curpos = nPos;     // position that the scroll box has been dragged to.  
		break;  
	}

	SetScrollPos(SB_VERT, curpos);
	CameraSetDisplayOffset(m_hCamera, -1, curpos);

	CDialogEx::OnVScroll(nSBCode, curpos, pScrollBar);
}
