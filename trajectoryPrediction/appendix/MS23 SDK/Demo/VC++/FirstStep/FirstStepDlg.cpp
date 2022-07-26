
// FirstStepDlg.cpp : 实现文件
//

#include "stdafx.h"
#include "FirstStep.h"
#include "FirstStepDlg.h"
#include "afxdialogex.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


// CFirstStepDlg 对话框




CFirstStepDlg::CFirstStepDlg(CWnd* pParent /*=NULL*/)
	: CDialogEx(CFirstStepDlg::IDD, pParent)
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
}

void CFirstStepDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CFirstStepDlg, CDialogEx)
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_WM_DESTROY()
	ON_BN_CLICKED(IDC_BUTTON_SETTINGS, &CFirstStepDlg::OnBnClickedButtonSettings)
	ON_BN_CLICKED(IDC_BUTTON_PLAY, &CFirstStepDlg::OnBnClickedButtonPlay)
	ON_BN_CLICKED(IDC_BUTTON_STOP, &CFirstStepDlg::OnBnClickedButtonStop)
	ON_BN_CLICKED(IDC_BUTTON_SNAP, &CFirstStepDlg::OnBnClickedButtonSnap)
	ON_WM_TIMER()
END_MESSAGE_MAP()


// CFirstStepDlg 消息处理程序

BOOL CFirstStepDlg::OnInitDialog()
{
	CDialogEx::OnInitDialog();

	// 设置此对话框的图标。当应用程序主窗口不是对话框时，框架将自动
	//  执行此操作
	SetIcon(m_hIcon, TRUE);			// 设置大图标
	SetIcon(m_hIcon, FALSE);		// 设置小图标

	CameraSdkInit(1);

	CameraSdkStatus status;
	m_Grabber = NULL;

	tSdkCameraDevInfo DevList[16];
	int nDev = ARRAYSIZE(DevList);
	status = CameraEnumerateDevice(DevList, &nDev);
	if (nDev == 1)
	{
		status = CameraGrabber_Create(&m_Grabber, &DevList[0]);
	}
	else if (nDev > 1)
	{
		status = CameraGrabber_CreateFromDevicePage(&m_Grabber);
	}
	
	if (status == CAMERA_STATUS_SUCCESS)
	{
		SetTimer(1, 1000, NULL);

		CameraGrabber_GetCameraHandle(m_Grabber, &m_hCamera);
		CameraGrabber_GetCameraDevInfo(m_Grabber, &m_DevInfo);

		CameraCreateSettingPage(m_hCamera, m_hWnd, m_DevInfo.acFriendlyName, NULL, NULL, 0);

		CameraGrabber_SetRGBCallback(m_Grabber, FrameRGBCallbackEntry, this);

		// 黑白相机设置ISP输出灰度图像
		// 彩色相机ISP默认会输出BGR24图像
		tSdkCameraCapbility cap;
		CameraGetCapability(m_hCamera, &cap);
		if (cap.sIspCapacity.bMonoSensor)
			CameraSetIspOutFormat(m_hCamera, CAMERA_MEDIA_TYPE_MONO8);

		CameraGrabber_SetHWnd(m_Grabber, GetDlgItem(IDC_DISP)->m_hWnd);
		CameraGrabber_StartLive(m_Grabber);
	}
	else
	{
		CString str;
		str.Format("打开相机失败！status=%d", status);
		MessageBox(str);
		EndDialog(0);
	}

	return TRUE;  // 除非将焦点设置到控件，否则返回 TRUE
}

// 如果向对话框添加最小化按钮，则需要下面的代码
//  来绘制该图标。对于使用文档/视图模型的 MFC 应用程序，
//  这将由框架自动完成。

void CFirstStepDlg::OnPaint()
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
HCURSOR CFirstStepDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}

void CFirstStepDlg::OnDestroy()
{
	CDialogEx::OnDestroy();

	if (m_Grabber != NULL)
	{
		CameraGrabber_Destroy(m_Grabber);
		m_Grabber = NULL;
	}
}

void CFirstStepDlg::OnBnClickedButtonSettings()
{
	CameraShowSettingPage(m_hCamera, TRUE);
}

void CFirstStepDlg::OnBnClickedButtonPlay()
{
	CameraGrabber_StartLive(m_Grabber);
}

void CFirstStepDlg::OnBnClickedButtonStop()
{
	CameraGrabber_StopLive(m_Grabber);
}

void CFirstStepDlg::OnBnClickedButtonSnap()
{
	void* Image = NULL;
	if (CameraGrabber_SaveImage(m_Grabber, &Image, 2000) == CAMERA_STATUS_SUCCESS)
	{
		// 初始化保存文件的文件名
		CString FileName;
		CString strTime = CTime::GetCurrentTime().Format(_T("%Y%m%d%H%M%S"));
		GetCurrentDirectory(MAX_PATH, FileName.GetBufferSetLength(MAX_PATH));
		FileName.ReleaseBuffer();
		FileName.AppendFormat("\\Snapshot%s.bmp", strTime);

		if (CameraImage_SaveAsBmp(Image, FileName) == CAMERA_STATUS_SUCCESS)
			MessageBox(FileName);
		else
			MessageBox("保存失败");
		CameraImage_Destroy(Image);
	}
	else
	{
		MessageBox("抓图超时");
	}
}

void CFirstStepDlg::OnTimer(UINT_PTR nIDEvent)
{
	tSdkGrabberStat Stat;
	CameraGrabber_GetStat(m_Grabber, &Stat);

	CString strText;
	strText.Format("宽:%u 高:%u 显示帧率:%.02f 捕获帧率:%.02f", Stat.Width, Stat.Height, Stat.DispFps, Stat.CapFps);
	SetDlgItemText(IDC_STATIC_STAT, strText);

	CDialogEx::OnTimer(nIDEvent);
}

void CFirstStepDlg::FrameRGBCallback(void* Grabber, BYTE *pFrameBuffer, tSdkFrameHead* pFrameHead)
{
	// 数据处理回调

	// 由于黑白相机在相机打开后设置了ISP输出灰度图像
	// 因此此处pFrameBuffer=8位灰度数据
	// 否则会和彩色相机一样输出BGR24数据

	// 彩色相机ISP默认会输出BGR24图像
	// pFrameBuffer=BGR24数据
}
