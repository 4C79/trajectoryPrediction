
// SaveImageDlg.cpp : 实现文件
//

#include "stdafx.h"
#include "SaveImage.h"
#include "SaveImageDlg.h"
#include "afxdialogex.h"
#include "AsyncSaveImage.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

// 相机个数
#define CAM_NUM				2

// 参数保存
#define SECTION_NAME			"config"

#define CONFIG_READ_STR(name, def)	theApp.GetProfileString(SECTION_NAME, #name, def)
#define CONFIG_WRITE_STR(name, val)	theApp.WriteProfileString(SECTION_NAME, #name, val)

#define CONFIG_READ_INT(name, def)	theApp.GetProfileInt(SECTION_NAME, #name, def)
#define CONFIG_WRITE_INT(name, val)	theApp.WriteProfileInt(SECTION_NAME, #name, val)


// CSaveImageDlg 对话框




CSaveImageDlg::CSaveImageDlg(CWnd* pParent /*=NULL*/)
	: CDialogEx(CSaveImageDlg::IDD, pParent)
{
	m_hPeriodTrigTimer = NULL;
	m_PeriodTrigFrameCount = 0;
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
}

void CSaveImageDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_GROUP_CAM, m_CamGroup);
	DDX_Control(pDX, IDC_COMBO_TRIGGER_MODE, m_ComboTriggerMode);
	DDX_Control(pDX, IDC_EDIT_TRIGGER_COUNT, m_EditTriggerCount);
	DDX_Control(pDX, IDC_EDIT_SAVE_DIR, m_EditSaveDir);
	DDX_Control(pDX, IDC_COMBO_SAVE_FMT, m_ComboSaveFmt);
	DDX_Control(pDX, IDC_EDIT_SAVE_COUNT, m_EditSaveCount);
	DDX_Control(pDX, IDC_EDIT_TRIG_TIMER_PERIOD, m_EditTrigTimerPeriod);
	DDX_Control(pDX, IDC_EDIT_TRIG_TIMER_ACTIVE, m_EditTrigTimerActiveTime);
	DDX_Control(pDX, IDC_EDIT_TRIG_TIMER_INTERVAL, m_EditTrigTimerInterval);
}

BEGIN_MESSAGE_MAP(CSaveImageDlg, CDialogEx)
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_WM_CLOSE()
	ON_CBN_SELCHANGE(IDC_COMBO_TRIGGER_MODE, &CSaveImageDlg::OnCbnSelchangeComboTriggerMode)
	ON_EN_KILLFOCUS(IDC_EDIT_TRIGGER_COUNT, &CSaveImageDlg::OnEnKillfocusEditTriggerCount)
	ON_BN_CLICKED(IDC_BUTTON_SOFT_TRIGGER, &CSaveImageDlg::OnBnClickedButtonSoftTrigger)
	ON_EN_KILLFOCUS(IDC_EDIT_SAVE_DIR, &CSaveImageDlg::OnEnKillfocusEditSaveDir)
	ON_CBN_SELCHANGE(IDC_COMBO_SAVE_FMT, &CSaveImageDlg::OnCbnSelchangeComboSaveFmt)
	ON_BN_CLICKED(IDC_CHECK_AUTO_SAVE, &CSaveImageDlg::OnBnClickedCheckAutoSave)
	ON_BN_CLICKED(IDC_CHECK_SAVE_ONLY_TRIGGER, &CSaveImageDlg::OnBnClickedCheckSaveOnlyTrigger)
	ON_BN_CLICKED(IDC_BUTTON_SET_SAVE_COUNT, &CSaveImageDlg::OnBnClickedButtonSetSaveCount)
	ON_BN_CLICKED(IDC_BUTTON_TRIG_TIMER_START, &CSaveImageDlg::OnBnClickedButtonTrigTimerStart)
	ON_WM_TIMER()
	ON_EN_KILLFOCUS(IDC_EDIT_TRIG_TIMER_PERIOD, &CSaveImageDlg::OnEnKillfocusEditTrigTimerPeriod)
	ON_EN_KILLFOCUS(IDC_EDIT_TRIG_TIMER_ACTIVE, &CSaveImageDlg::OnEnKillfocusEditTrigTimerActive)
	ON_BN_CLICKED(IDC_CHECK_OVERLAY_DATE, &CSaveImageDlg::OnBnClickedCheckOverlayDate)
	ON_EN_KILLFOCUS(IDC_EDIT_TRIG_TIMER_INTERVAL, &CSaveImageDlg::OnEnKillfocusEditTrigTimerInterval)
END_MESSAGE_MAP()


// CSaveImageDlg 消息处理程序

BOOL CSaveImageDlg::OnInitDialog()
{
	CDialogEx::OnInitDialog();

	// 设置此对话框的图标。当应用程序主窗口不是对话框时，框架将自动
	//  执行此操作
	SetIcon(m_hIcon, TRUE);			// 设置大图标
	SetIcon(m_hIcon, FALSE);		// 设置小图标

	// 初始化异步保存对象
	SYSTEM_INFO info;
	GetSystemInfo(&info);
	CAsyncSaveImage::GetInstance()->Init(info.dwNumberOfProcessors, 1000);
	CAsyncSaveImage::GetInstance()->Start();

	// 打开相机
	OpenCameras();

	// 配置默认抓图参数
	m_ComboTriggerMode.SetCurSel(CONFIG_READ_INT(m_ComboTriggerMode, 1));
	OnCbnSelchangeComboTriggerMode();

	SetDlgItemInt(IDC_EDIT_TRIGGER_COUNT, CONFIG_READ_INT(m_EditTriggerCount, 1));
	OnEnKillfocusEditTriggerCount();

	m_EditSaveDir.SetWindowText(CONFIG_READ_STR(m_EditSaveDir, ""));
	OnEnKillfocusEditSaveDir();

	m_ComboSaveFmt.SetCurSel(CONFIG_READ_INT(m_ComboSaveFmt, 1));
	OnCbnSelchangeComboSaveFmt();

	CheckDlgButton(IDC_CHECK_AUTO_SAVE, CONFIG_READ_INT(CheckAutoSave, 0) ? BST_CHECKED : BST_UNCHECKED);
	OnBnClickedCheckAutoSave();

	CheckDlgButton(IDC_CHECK_SAVE_ONLY_TRIGGER, CONFIG_READ_INT(CheckSaveOnlyTrigger, 1) ? BST_CHECKED : BST_UNCHECKED);
	OnBnClickedCheckSaveOnlyTrigger();

	SetDlgItemInt(IDC_EDIT_TRIG_TIMER_PERIOD, CONFIG_READ_INT(m_EditTrigTimerPeriod, 2));
	OnEnKillfocusEditTrigTimerPeriod();

	SetDlgItemInt(IDC_EDIT_TRIG_TIMER_ACTIVE, CONFIG_READ_INT(m_EditTrigTimerActiveTime, 0));
	OnEnKillfocusEditTrigTimerActive();

	SetDlgItemInt(IDC_EDIT_TRIG_TIMER_INTERVAL, CONFIG_READ_INT(m_EditTrigTimerInterval, 1000));
	OnEnKillfocusEditTrigTimerInterval();

	CheckDlgButton(IDC_CHECK_OVERLAY_DATE, CONFIG_READ_INT(CheckOverlayDate, 0) ? BST_CHECKED : BST_UNCHECKED);
	OnBnClickedCheckOverlayDate();

	m_EditSaveCount.SetWindowText("0");

	// 让所有相机开始工作
	for (int i = 0; i < m_pCamList.GetSize(); ++i)
	{
		m_pCamList[i]->Start();
	}

	SetTimer(1, 200, NULL);
	return TRUE;  // 除非将焦点设置到控件，否则返回 TRUE
}

void CSaveImageDlg::OpenCameras()
{
	CRect rcGroup;
	m_CamGroup.GetClientRect(rcGroup);
	rcGroup.DeflateRect(5, 5);
	rcGroup.top = 20;

	int CamWidth = rcGroup.Width() / CAM_NUM;
	int CamHeight = rcGroup.Height();

	for (int i = 0; i < CAM_NUM; ++i)
	{
		CString strPrefix;
		strPrefix.Format("cam%d_", i);

		CCameraDlg* pCamDlg = new CCameraDlg(strPrefix, &m_CamGroup);
		pCamDlg->Create(CCameraDlg::IDD, &m_CamGroup);
		pCamDlg->MoveWindow(rcGroup.left + CamWidth * i, rcGroup.top, CamWidth, CamHeight);
		pCamDlg->ShowWindow(SW_SHOW);

		int status = pCamDlg->Open(i);
		if (status != CAMERA_STATUS_SUCCESS)
		{
			m_pInvalidCamList.Add(pCamDlg);
			pCamDlg->EnableWindow(FALSE);
		}
		else
		{
			m_pCamList.Add(pCamDlg);
		}
	}
}

void CSaveImageDlg::CloseCameras()
{
	for (int i = 0; i < m_pCamList.GetSize(); ++i)
	{
		m_pCamList[i]->Close();
		delete m_pCamList[i];
	}
	m_pCamList.RemoveAll();

	for (int i = 0; i < m_pInvalidCamList.GetSize(); ++i)
	{
		delete m_pInvalidCamList[i];
	}
	m_pInvalidCamList.RemoveAll();
}

// 如果向对话框添加最小化按钮，则需要下面的代码
//  来绘制该图标。对于使用文档/视图模型的 MFC 应用程序，
//  这将由框架自动完成。

void CSaveImageDlg::OnPaint()
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
HCURSOR CSaveImageDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}

void CSaveImageDlg::OnClose()
{
	KillTimer(1);

	if (m_hPeriodTrigTimer != NULL)
	{
		timeKillEvent(m_hPeriodTrigTimer);
		m_hPeriodTrigTimer = NULL;
	}

	CAsyncSaveImage::GetInstance()->Stop();
	CloseCameras();
	CAsyncSaveImage::PurgeInstance();

	CDialogEx::OnClose();
}

void CSaveImageDlg::OnCbnSelchangeComboTriggerMode()
{
	int mode = m_ComboTriggerMode.GetCurSel();
	for (int i = 0; i < m_pCamList.GetSize(); ++i)
	{
		m_pCamList[i]->SetTriggerMode(mode);
	}
	CONFIG_WRITE_INT(m_ComboTriggerMode, mode);
}

void CSaveImageDlg::OnEnKillfocusEditTriggerCount()
{
	CString str;
	m_EditTriggerCount.GetWindowText(str);

	int count = atoi(str);
	count = max(count, 1);
	SetDlgItemInt(IDC_EDIT_TRIGGER_COUNT, count);

	for (int i = 0; i < m_pCamList.GetSize(); ++i)
	{
		m_pCamList[i]->SetTriggerCount(count);
	}

	CONFIG_WRITE_INT(m_EditTriggerCount, count);
}

void CSaveImageDlg::TriggerCameras()
{
	for (int i = 0; i < m_pCamList.GetSize(); ++i)
	{
		m_pCamList[i]->SoftTrigger();
	}
}

void CSaveImageDlg::OnBnClickedButtonSoftTrigger()
{
	TriggerCameras();
}

// 判断目录是否存在  
static bool FolderExists(CString s)
{  
	DWORD attr;   
	attr = GetFileAttributes(s);   
	return (attr != (DWORD)(-1) ) &&  
		( attr & FILE_ATTRIBUTE_DIRECTORY);   
}  

// 递归创建目录， 如果目录已经存在或者创建成功返回TRUE  
static bool SuperMkDir(CString P)  
{  
	int len=P.GetLength();  
	if ( len <2 ) return false;   

	if('\\'==P[len-1])  
	{  
		P=P.Left(len-1);  
		len=P.GetLength();  
	}  
	if ( len <=0 ) return false;  

	if (len <=3)   
	{  
		if (FolderExists(P))return true;  
		else return false;   
	}  

	if (FolderExists(P))return true;  



	CString Parent;  
	Parent=P.Left(P.ReverseFind('\\') );  

	if(Parent.GetLength()<=0)return false;   

	bool Ret=SuperMkDir(Parent);   

	if(Ret)   
	{  
		SECURITY_ATTRIBUTES sa;  
		sa.nLength=sizeof(SECURITY_ATTRIBUTES);  
		sa.lpSecurityDescriptor=NULL;  
		sa.bInheritHandle=0;  
		Ret=(CreateDirectory(P,&sa)==TRUE);  
		return Ret;  
	}  
	else  
		return false;  
}

void CSaveImageDlg::OnEnKillfocusEditSaveDir()
{
	CString str;
	m_EditSaveDir.GetWindowText(str);

	if (!str.IsEmpty() && !SuperMkDir(str))
	{
		MessageBox("无效的保存目录");
		str = "";
	}

	for (int i = 0; i < m_pCamList.GetSize(); ++i)
	{
		m_pCamList[i]->SetSaveDir(str);
	}

	CONFIG_WRITE_STR(m_EditSaveDir, str);
}

void CSaveImageDlg::OnCbnSelchangeComboSaveFmt()
{
	int fmt = m_ComboSaveFmt.GetCurSel();
	for (int i = 0; i < m_pCamList.GetSize(); ++i)
	{
		m_pCamList[i]->SetSaveFormat(fmt);
	}
	CONFIG_WRITE_INT(m_ComboSaveFmt, fmt);
}

void CSaveImageDlg::OnBnClickedCheckAutoSave()
{
	BOOL bCheckAutoSave = (IsDlgButtonChecked(IDC_CHECK_AUTO_SAVE) ? TRUE : FALSE);
	for (int i = 0; i < m_pCamList.GetSize(); ++i)
	{
		m_pCamList[i]->SetAutoSave(bCheckAutoSave);
	}
	CONFIG_WRITE_INT(CheckAutoSave, bCheckAutoSave);
}

void CSaveImageDlg::OnBnClickedCheckSaveOnlyTrigger()
{
	BOOL bCheckSaveOnlyTrigger = (IsDlgButtonChecked(IDC_CHECK_SAVE_ONLY_TRIGGER) ? TRUE : FALSE);
	for (int i = 0; i < m_pCamList.GetSize(); ++i)
	{
		m_pCamList[i]->SetOnlySaveTrigger(bCheckSaveOnlyTrigger);
	}
	CONFIG_WRITE_INT(CheckSaveOnlyTrigger, bCheckSaveOnlyTrigger);
}

void CSaveImageDlg::OnBnClickedButtonSetSaveCount()
{
	int count = GetDlgItemInt(IDC_EDIT_SAVE_COUNT);
	if (count <= 0)
	{
		count = -1;
	}
	for (int i = 0; i < m_pCamList.GetSize(); ++i)
	{
		m_pCamList[i]->SetSaveCount(count);
	}
}

void CSaveImageDlg::OnBnClickedButtonTrigTimerStart()
{
	if (m_hPeriodTrigTimer == NULL)
	{
		int Period = GetDlgItemInt(IDC_EDIT_TRIG_TIMER_PERIOD);
		int ActiveTime = GetDlgItemInt(IDC_EDIT_TRIG_TIMER_ACTIVE);
		int Interval = GetDlgItemInt(IDC_EDIT_TRIG_TIMER_INTERVAL);

		if (ActiveTime > 0)
		{
			m_PeriodTrigFrameCount = Period * ActiveTime * 1000 / Interval;
		}
		else
		{
			m_PeriodTrigFrameCount = INFINITE;
		}

		UINT uDelay = max(Interval / Period, 1);
		m_hPeriodTrigTimer = timeSetEvent(uDelay, 0, PeriodTrigProc, (DWORD_PTR)this, 
			TIME_PERIODIC | TIME_CALLBACK_FUNCTION | TIME_KILL_SYNCHRONOUS);
	}
	else
	{
		timeKillEvent(m_hPeriodTrigTimer);
		m_hPeriodTrigTimer = NULL;
	}
}

void CSaveImageDlg::OnPeriodTrig()
{
	if (m_PeriodTrigFrameCount > 0)
	{
		if (m_PeriodTrigFrameCount != INFINITE)
		{
			--m_PeriodTrigFrameCount;
		}

		TriggerCameras();
	}
}

void CSaveImageDlg::OnTimer(UINT_PTR nIDEvent)
{
	if (m_hPeriodTrigTimer != NULL && m_PeriodTrigFrameCount == 0)
	{
		timeKillEvent(m_hPeriodTrigTimer);
		m_hPeriodTrigTimer = NULL;
	}

	SetDlgItemText(IDC_BUTTON_TRIG_TIMER_START, (m_hPeriodTrigTimer != NULL ? "停止" : "启动") );

	CDialogEx::OnTimer(nIDEvent);
}

void CSaveImageDlg::OnEnKillfocusEditTrigTimerPeriod()
{
	int Period = GetDlgItemInt(IDC_EDIT_TRIG_TIMER_PERIOD);
	Period = max(Period, 1);
	SetDlgItemInt(IDC_EDIT_TRIG_TIMER_PERIOD, Period);

	CONFIG_WRITE_INT(m_EditTrigTimerPeriod, Period);
}

void CSaveImageDlg::OnEnKillfocusEditTrigTimerActive()
{
	int ActiveTime = GetDlgItemInt(IDC_EDIT_TRIG_TIMER_ACTIVE);
	ActiveTime = max(ActiveTime, 0);
	SetDlgItemInt(IDC_EDIT_TRIG_TIMER_ACTIVE, ActiveTime);

	CONFIG_WRITE_INT(m_EditTrigTimerActiveTime, ActiveTime);
}

void CSaveImageDlg::OnBnClickedCheckOverlayDate()
{
	BOOL bCheckOverlayDate = (IsDlgButtonChecked(IDC_CHECK_OVERLAY_DATE) ? TRUE : FALSE);
	for (int i = 0; i < m_pCamList.GetSize(); ++i)
	{
		m_pCamList[i]->SetOverlayDate(bCheckOverlayDate);
	}
	CONFIG_WRITE_INT(CheckOverlayDate, bCheckOverlayDate);
}

void CSaveImageDlg::OnEnKillfocusEditTrigTimerInterval()
{
	int Interval = GetDlgItemInt(IDC_EDIT_TRIG_TIMER_INTERVAL);
	Interval = max(Interval, 1);
	SetDlgItemInt(IDC_EDIT_TRIG_TIMER_INTERVAL, Interval);

	CONFIG_WRITE_INT(m_EditTrigTimerInterval, Interval);
}
