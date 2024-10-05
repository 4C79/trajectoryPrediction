
// FirstStepMultiDlg.cpp : 实现文件
//

#include "stdafx.h"
#include "FirstStepMulti10.h"
#include "FirstStepMulti10Dlg.h"
#include "afxdialogex.h"
#include "Mmsystem.h"

#pragma comment(lib, "Winmm.lib")

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

enum
{
	WM_SET_DLGITEM_INT = (WM_USER + 100),
	WM_SET_DLGITEM_TEXT,
};


static UINT g_CMS_ID[] = {
	IDC_STATIC_CM1, IDC_STATIC_CM2, IDC_STATIC_CM3, IDC_STATIC_CM4, IDC_STATIC_CM5, 
	IDC_STATIC_CM6, IDC_STATIC_CM7, IDC_STATIC_CM8, IDC_STATIC_CM9, IDC_STATIC_CM10 
};

static UINT g_DISPS_ID[] = { 
	IDC_CM_VIEW_1, IDC_CM_VIEW_2, IDC_CM_VIEW_3, IDC_CM_VIEW_4, IDC_CM_VIEW_5, 
	IDC_CM_VIEW_6, IDC_CM_VIEW_7, IDC_CM_VIEW_8, IDC_CM_VIEW_9, IDC_CM_VIEW_10 
};

static UINT g_FpsStaticsID[] = { 
	IDC_STATIC_FPS1, IDC_STATIC_FPS2, IDC_STATIC_FPS3, IDC_STATIC_FPS4, IDC_STATIC_FPS5,
	IDC_STATIC_FPS6, IDC_STATIC_FPS7, IDC_STATIC_FPS8, IDC_STATIC_FPS9, IDC_STATIC_FPS10
};

static UINT g_TimeStaticsID[] = { 
	IDC_STATIC_TIME1, IDC_STATIC_TIME2, IDC_STATIC_TIME3, IDC_STATIC_TIME4, IDC_STATIC_TIME5,
	IDC_STATIC_TIME6, IDC_STATIC_TIME7, IDC_STATIC_TIME8, IDC_STATIC_TIME9, IDC_STATIC_TIME10
};

static UINT g_FrameStaticsID[] = { 
	IDC_STATIC_FRAME1, IDC_STATIC_FRAME2, IDC_STATIC_FRAME3, IDC_STATIC_FRAME4, IDC_STATIC_FRAME5,
	IDC_STATIC_FRAME6, IDC_STATIC_FRAME7, IDC_STATIC_FRAME8, IDC_STATIC_FRAME9, IDC_STATIC_FRAME10
};



// CFirstStepMulti10Dlg 对话框
CFirstStepMulti10Dlg::CFirstStepMulti10Dlg(CWnd* pParent /*=NULL*/)
	: CDialogEx(CFirstStepMulti10Dlg::IDD, pParent)
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
	ZeroMemory(m_Grabber, sizeof(m_Grabber));
	ZeroMemory(m_hCamera, sizeof(m_hCamera));
	ZeroMemory(m_DevInfo, sizeof(m_DevInfo));

	m_NumberOfCameras = 0;
	m_MaxActiveCount = 0;

	m_TrggerTimerID = 0;
	ZeroMemory(m_Paused, sizeof(m_Paused));
	m_bImmediatelyTriggerNext = FALSE;

	m_CurrentActiveCamera = 0;
	ZeroMemory(m_TrggerTime, sizeof(m_TrggerTime));
}

void CFirstStepMulti10Dlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CFirstStepMulti10Dlg, CDialogEx)
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_WM_DESTROY()
	ON_WM_TIMER()
	ON_BN_CLICKED(IDC_CHECK_PAUSE_1, &CFirstStepMulti10Dlg::OnBnClickedCheckPause1)
	ON_BN_CLICKED(IDC_BUTTON_CM_SET_1, &CFirstStepMulti10Dlg::OnBnClickedButtonCmSet1)
	ON_BN_CLICKED(IDC_BUTTON_SNAPSHOT_1, &CFirstStepMulti10Dlg::OnBnClickedButtonSnapshot1)
	ON_BN_CLICKED(IDC_CHECK_PAUSE_2, &CFirstStepMulti10Dlg::OnBnClickedCheckPause2)
	ON_BN_CLICKED(IDC_BUTTON_CM_SET_2, &CFirstStepMulti10Dlg::OnBnClickedButtonCmSet2)
	ON_BN_CLICKED(IDC_BUTTON_SNAPSHOT_2, &CFirstStepMulti10Dlg::OnBnClickedButtonSnapshot2)
	ON_BN_CLICKED(IDC_CHECK_PAUSE_3, &CFirstStepMulti10Dlg::OnBnClickedCheckPause3)
	ON_BN_CLICKED(IDC_BUTTON_CM_SET_3, &CFirstStepMulti10Dlg::OnBnClickedButtonCmSet3)
	ON_BN_CLICKED(IDC_BUTTON_SNAPSHOT_3, &CFirstStepMulti10Dlg::OnBnClickedButtonSnapshot3)
	ON_BN_CLICKED(IDC_CHECK_PAUSE_4, &CFirstStepMulti10Dlg::OnBnClickedCheckPause4)
	ON_BN_CLICKED(IDC_BUTTON_CM_SET_4, &CFirstStepMulti10Dlg::OnBnClickedButtonCmSet4)
	ON_BN_CLICKED(IDC_BUTTON_SNAPSHOT_4, &CFirstStepMulti10Dlg::OnBnClickedButtonSnapshot4)	
	ON_BN_CLICKED(IDC_CHECK_PAUSE_5, &CFirstStepMulti10Dlg::OnBnClickedCheckPause5)
	ON_BN_CLICKED(IDC_BUTTON_CM_SET_5, &CFirstStepMulti10Dlg::OnBnClickedButtonCmSet5)
	ON_BN_CLICKED(IDC_BUTTON_SNAPSHOT_5, &CFirstStepMulti10Dlg::OnBnClickedButtonSnapshot5)
	ON_BN_CLICKED(IDC_CHECK_PAUSE_6, &CFirstStepMulti10Dlg::OnBnClickedCheckPause6)
	ON_BN_CLICKED(IDC_BUTTON_CM_SET_6, &CFirstStepMulti10Dlg::OnBnClickedButtonCmSet6)
	ON_BN_CLICKED(IDC_BUTTON_SNAPSHOT_6, &CFirstStepMulti10Dlg::OnBnClickedButtonSnapshot6)
	ON_BN_CLICKED(IDC_CHECK_PAUSE_7, &CFirstStepMulti10Dlg::OnBnClickedCheckPause7)
	ON_BN_CLICKED(IDC_BUTTON_CM_SET_7, &CFirstStepMulti10Dlg::OnBnClickedButtonCmSet7)
	ON_BN_CLICKED(IDC_BUTTON_SNAPSHOT_7, &CFirstStepMulti10Dlg::OnBnClickedButtonSnapshot7)
	ON_BN_CLICKED(IDC_CHECK_PAUSE_8, &CFirstStepMulti10Dlg::OnBnClickedCheckPause8)
	ON_BN_CLICKED(IDC_BUTTON_CM_SET_8, &CFirstStepMulti10Dlg::OnBnClickedButtonCmSet8)
	ON_BN_CLICKED(IDC_BUTTON_SNAPSHOT_8, &CFirstStepMulti10Dlg::OnBnClickedButtonSnapshot8)
	ON_BN_CLICKED(IDC_CHECK_PAUSE_9, &CFirstStepMulti10Dlg::OnBnClickedCheckPause9)
	ON_BN_CLICKED(IDC_BUTTON_CM_SET_9, &CFirstStepMulti10Dlg::OnBnClickedButtonCmSet9)
	ON_BN_CLICKED(IDC_BUTTON_SNAPSHOT_9, &CFirstStepMulti10Dlg::OnBnClickedButtonSnapshot9)
	ON_BN_CLICKED(IDC_CHECK_PAUSE_10, &CFirstStepMulti10Dlg::OnBnClickedCheckPause10)
	ON_BN_CLICKED(IDC_BUTTON_CM_SET_10, &CFirstStepMulti10Dlg::OnBnClickedButtonCmSet10)
	ON_BN_CLICKED(IDC_BUTTON_SNAPSHOT_10, &CFirstStepMulti10Dlg::OnBnClickedButtonSnapshot10)
	ON_BN_CLICKED(IDC_CHECK_IMM_TRI_NEXT, &CFirstStepMulti10Dlg::OnBnClickedCheckImmTriNext)
	ON_EN_CHANGE(IDC_EDIT_MAX_ACTIVE, &CFirstStepMulti10Dlg::OnEnChangeEditMaxActive)
	ON_MESSAGE(WM_SET_DLGITEM_INT, &CFirstStepMulti10Dlg::OnSetDlgItemInt)
	ON_MESSAGE(WM_SET_DLGITEM_TEXT, &CFirstStepMulti10Dlg::OnSetDlgItemText)
END_MESSAGE_MAP()


// CFirstStepMulti10Dlg 消息处理程序

BOOL CFirstStepMulti10Dlg::OnInitDialog()
{
	CDialogEx::OnInitDialog();

	// 设置此对话框的图标。当应用程序主窗口不是对话框时，框架将自动
	//  执行此操作
	SetIcon(m_hIcon, TRUE);			// 设置大图标
	SetIcon(m_hIcon, FALSE);		// 设置小图标

	CameraSdkInit(1);

	int nDev = ARRAYSIZE(m_DevInfo);
	CameraEnumerateDevice(m_DevInfo, &nDev);
	for (int i = 0; i < nDev; ++i)
	{
		if (CameraGrabber_Create(&m_Grabber[i], &m_DevInfo[i]) != CAMERA_STATUS_SUCCESS)
			continue;

		SetDlgItemText(g_CMS_ID[i], m_DevInfo[i].acFriendlyName);

		CameraGrabber_GetCameraHandle(m_Grabber[i], &m_hCamera[i]);
		CameraCreateSettingPage(m_hCamera[i], m_hWnd, m_DevInfo[i].acFriendlyName, NULL, NULL, 0);

		CameraGrabber_SetFrameListener(m_Grabber[i], FrameListenerEntry, this);
		CameraGrabber_SetSaveImageCompleteCallback(m_Grabber[i], SaveImageCompleteEntry, this);

		// 设置为软触发
		CameraSetTriggerMode(m_hCamera[i], 1);

		// 打开单帧模式
		CameraSetSingleGrabMode(m_hCamera[i], TRUE);

		CameraGrabber_SetHWnd(m_Grabber[i], GetDlgItem(g_DISPS_ID[i])->m_hWnd);
		CameraGrabber_StartLive(m_Grabber[i]);

		++m_NumberOfCameras;
	}
	
	m_CurrentActiveCamera = -1;
	m_MaxActiveCount = m_NumberOfCameras;
	SetDlgItemInt(IDC_EDIT_MAX_ACTIVE, m_MaxActiveCount);

	m_TrggerTimerID = timeSetEvent(1000, 1, TriggerTimerEntry, (DWORD_PTR)this, 
		TIME_PERIODIC | TIME_CALLBACK_FUNCTION | TIME_KILL_SYNCHRONOUS);

	SetTimer(1, 1000, NULL);

	return TRUE;  // 除非将焦点设置到控件，否则返回 TRUE
}

// 如果向对话框添加最小化按钮，则需要下面的代码
//  来绘制该图标。对于使用文档/视图模型的 MFC 应用程序，
//  这将由框架自动完成。

void CFirstStepMulti10Dlg::OnPaint()
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
HCURSOR CFirstStepMulti10Dlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}

void CFirstStepMulti10Dlg::OnDestroy()
{
	CDialogEx::OnDestroy();

	timeKillEvent(m_TrggerTimerID);

	for (int i = 0; i < ARRAYSIZE(m_Grabber); ++i)
	{
		CameraGrabber_Destroy(m_Grabber[i]);
	}
}

#define CAM_BTNS_IMPL(n)		\
void CFirstStepMulti10Dlg::OnBnClickedCheckPause##n()\
{\
	if (m_Grabber[n - 1] != NULL)\
	{\
		if (((CButton*)GetDlgItem(IDC_CHECK_PAUSE_##n))->GetCheck() == BST_CHECKED)\
		{\
			m_Paused[n - 1] = TRUE;\
			CameraGrabber_StopLive(m_Grabber[n - 1]);\
		}\
		else\
		{\
			m_Paused[n - 1] = FALSE;\
			CameraGrabber_StartLive(m_Grabber[n - 1]);\
		}\
	}\
}\
\
void CFirstStepMulti10Dlg::OnBnClickedButtonCmSet##n()\
{\
	if (m_Grabber[n - 1] != NULL)\
	{\
		CameraShowSettingPage(m_hCamera[n - 1], TRUE);\
	}\
}\
\
void CFirstStepMulti10Dlg::OnBnClickedButtonSnapshot##n()\
{\
	if (m_Grabber[n - 1] != NULL)\
	{\
		CameraGrabber_SaveImageAsync(m_Grabber[n - 1]);\
	}\
}

CAM_BTNS_IMPL(1)
CAM_BTNS_IMPL(2)
CAM_BTNS_IMPL(3)
CAM_BTNS_IMPL(4)
CAM_BTNS_IMPL(5)
CAM_BTNS_IMPL(6)
CAM_BTNS_IMPL(7)
CAM_BTNS_IMPL(8)
CAM_BTNS_IMPL(9)
CAM_BTNS_IMPL(10)

// 每当Grabber捕获到一帧图像时，会分3个阶段来依次调用FrameListener
// 如果FrameListener返回0，Grabber将会丢弃此帧并结束针对此帧的所有后续处理阶段
// 阶段0: RAW数据处理，pFrameBuffer=Raw数据
// 阶段1: 截图前处理，pFrameBuffer=RGB数据
// 阶段2: 显示前处理，pFrameBuffer=RGB数据
int CFirstStepMulti10Dlg::FrameListener(void* Grabber, int Phase, BYTE *pFrameBuffer, tSdkFrameHead* pFrameHead)
{
	if (Phase == 0)
	{
		CSingleLock Lock(&m_csLock, TRUE);
		for (int i = 0; i < ARRAYSIZE(m_Grabber); ++i)
		{
			if (m_Grabber[i] == Grabber)
			{
				if (m_TrggerTime[i] != 0)
				{
					PostMessage(WM_SET_DLGITEM_INT, g_TimeStaticsID[i], GetTickCount() - m_TrggerTime[i]);

					m_TrggerTime[i] = 0;
					if (m_bImmediatelyTriggerNext)
					{
						TriggerCamera();
					}
				}
				break;
			}
		}
	}
	else if (Phase == 1)
	{

	}
	else if (Phase == 2)
	{
		
	}

	return 1;
}

void CFirstStepMulti10Dlg::SaveImageComplete(void* Grabber, void* Image, CameraSdkStatus Status)
{
	if (Image != NULL)
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
}

void CFirstStepMulti10Dlg::OnTimer(UINT_PTR nIDEvent)
{
	// 显示帧率
	for (int i = 0; i < ARRAYSIZE(m_Grabber); ++i)
	{
		if (m_Grabber[i] != NULL)
		{
			if (!m_Paused[i])
			{
				tSdkGrabberStat Stat = { 0 };
				CameraGrabber_GetStat(m_Grabber[i], &Stat);

				CString str;
				str.Format("%.02f", Stat.DispFps);
				SetDlgItemText(g_FpsStaticsID[i], str);

				SetDlgItemInt(g_FrameStaticsID[i], Stat.Disp);
			}
			else
			{
				SetDlgItemText(g_FpsStaticsID[i], "P");
			}
		}
	}

	CDialogEx::OnTimer(nIDEvent);
}

void CFirstStepMulti10Dlg::TriggerTimer()
{
	CSingleLock Lock(&m_csLock, TRUE);

	// 检查过久无响应的软触发
	for (int i = 0; i < ARRAYSIZE(m_Grabber); ++i)
	{
		if (m_Grabber[i] == NULL)
			continue;

		if (m_TrggerTime[i] != 0 && GetTickCount() - m_TrggerTime[i] > 5000)
		{
			PostMessage(WM_SET_DLGITEM_TEXT, g_TimeStaticsID[i], (LPARAM)_strdup("TO"));
			m_TrggerTime[i] = 0;
		}
	}

	// 尝试发起触发指令
	TriggerCamera();
}

void CFirstStepMulti10Dlg::TriggerCamera()
{
	for (int NumActiveCamera = GetNumberOfActiveCamera();
		NumActiveCamera < m_MaxActiveCount;
		++NumActiveCamera)
	{
		BOOL bFound = FALSE;
		for (int i = 0; i < ARRAYSIZE(m_Grabber); ++i)
		{
			m_CurrentActiveCamera = ++m_CurrentActiveCamera % ARRAYSIZE(m_Grabber);
			if (m_Grabber[m_CurrentActiveCamera] != NULL 
				&& m_TrggerTime[m_CurrentActiveCamera] == 0
				&& !m_Paused[m_CurrentActiveCamera])
			{
				bFound = TRUE;
				break;
			}
		}
		if (!bFound)
			break;

		m_TrggerTime[m_CurrentActiveCamera] = GetTickCount();
		CameraSoftTrigger(m_hCamera[m_CurrentActiveCamera]);
		CameraRestartGrab(m_hCamera[m_CurrentActiveCamera]);
	}
}

int CFirstStepMulti10Dlg::GetNumberOfActiveCamera()
{
	int n = 0;
	for (int i = 0; i < ARRAYSIZE(m_TrggerTime); ++i)
	{
		if (m_TrggerTime[i] != 0)
			++n;
	}
	return n;
}

void CFirstStepMulti10Dlg::OnBnClickedCheckImmTriNext()
{
	if (((CButton*)GetDlgItem(IDC_CHECK_IMM_TRI_NEXT))->GetCheck() == BST_CHECKED)
		m_bImmediatelyTriggerNext = TRUE;
	else
		m_bImmediatelyTriggerNext = FALSE;
}

void CFirstStepMulti10Dlg::OnEnChangeEditMaxActive()
{
	// TODO:  如果该控件是 RICHEDIT 控件，它将不
	// 发送此通知，除非重写 CDialogEx::OnInitDialog()
	// 函数并调用 CRichEditCtrl().SetEventMask()，
	// 同时将 ENM_CHANGE 标志“或”运算到掩码中。

	// TODO:  在此添加控件通知处理程序代码
	m_MaxActiveCount = GetDlgItemInt(IDC_EDIT_MAX_ACTIVE);
}

LRESULT CFirstStepMulti10Dlg::OnSetDlgItemInt(WPARAM wParam, LPARAM lParam)
{
	SetDlgItemInt((int)wParam, (UINT)lParam);
	return 0;
}

LRESULT CFirstStepMulti10Dlg::OnSetDlgItemText(WPARAM wParam, LPARAM lParam)
{
	SetDlgItemText((int)wParam, (LPCTSTR)lParam);
	free((void*)lParam);
	return 0;
}
