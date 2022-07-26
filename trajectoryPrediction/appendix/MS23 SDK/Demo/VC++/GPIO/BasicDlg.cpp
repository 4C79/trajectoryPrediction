// BasicDlg.cpp : implementation file
//
//BIG5 TRANS ALLOWED
#include "stdafx.h"
#include "Basic.h"

#include "BasicDlg.h"
#include "malloc.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

extern int gLanguage;
/////////////////////////////////////////////////////////////////////////////
// CAboutDlg dialog used for App About

class CAboutDlg : public CDialog
{
public:
	CAboutDlg();

// Dialog Data
	//{{AFX_DATA(CAboutDlg)
	enum { IDD = IDD_ABOUTBOX };
	CString	m_sAboutInfo;
	//}}AFX_DATA

	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CAboutDlg)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

	DECLARE_MESSAGE_MAP()
};


CAboutDlg::CAboutDlg() : CDialog(CAboutDlg::IDD)
{
	//{{AFX_DATA_INIT(CAboutDlg)
	m_sAboutInfo = _T("This example shows how to integrate the camera into your system in a very fast and easy way!");
	//}}AFX_DATA_INIT
}

void CAboutDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CAboutDlg)
	DDX_Text(pDX, IDC_EDIT_ABOUT, m_sAboutInfo);
	//}}AFX_DATA_MAP
}

BEGIN_MESSAGE_MAP(CAboutDlg, CDialog)
	//{{AFX_MSG_MAP(CAboutDlg)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CBasicDlg dialog

CBasicDlg::CBasicDlg(CWnd* pParent /*=NULL*/)
: CDialog(gLanguage == 0?IDD_BASIC_DIALOG:IDD_BASIC_DIALOG_CN, pParent)
, m_bHighIN1(FALSE)
, m_bHighIN2(FALSE)
, m_bHighIN3(FALSE)
, m_bHighIN4(FALSE)
, m_bHighIN5(FALSE)
, m_bHighIN6(FALSE)
, m_bHighIN7(FALSE)
, m_bHighIN8(FALSE)
, m_bHighIN9(FALSE)
, m_bHighIN10(FALSE)
, m_bHighOUT1(FALSE)
, m_bHighOUT2(FALSE)
, m_bHighOUT3(FALSE)
, m_bHighOUT4(FALSE)
, m_bHighOUT5(FALSE)
, m_bHighOUT6(FALSE)
, m_bHighOUT7(FALSE)
, m_bHighOUT8(FALSE)
, m_bHighOUT9(FALSE)
, m_bHighOUT10(FALSE)
{
	//{{AFX_DATA_INIT(CBasicDlg)
		// NOTE: the ClassWizard will add member initialization here
	//}}AFX_DATA_INIT
	// Note that LoadIcon does not require a subsequent DestroyIcon in Win32
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
	
	m_Grabber = NULL;
	m_hCamera = -1;
}

void CBasicDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CBasicDlg)

	//}}AFX_DATA_MAP
	DDX_Radio(pDX, IDC_RADIO1, m_bHighIN1);
	DDX_Radio(pDX, IDC_RADIO3, m_bHighIN2);
	DDX_Radio(pDX, IDC_RADIO5, m_bHighIN3);
	DDX_Radio(pDX, IDC_RADIO7, m_bHighIN4);
	DDX_Radio(pDX, IDC_RADIO9, m_bHighIN5);
	DDX_Radio(pDX, IDC_RADIO11, m_bHighIN6);
	DDX_Radio(pDX, IDC_RADIO13, m_bHighIN7);
	DDX_Radio(pDX, IDC_RADIO15, m_bHighIN8);
	DDX_Radio(pDX, IDC_RADIO17, m_bHighIN9);
	DDX_Radio(pDX, IDC_RADIO19, m_bHighIN10);
	DDX_Radio(pDX, IDC_RADIO21, m_bHighOUT1);
	DDX_Radio(pDX, IDC_RADIO23, m_bHighOUT2);
	DDX_Radio(pDX, IDC_RADIO25, m_bHighOUT3);
	DDX_Radio(pDX, IDC_RADIO27, m_bHighOUT4);
	DDX_Radio(pDX, IDC_RADIO29, m_bHighOUT5);
	DDX_Radio(pDX, IDC_RADIO31, m_bHighOUT6);
	DDX_Radio(pDX, IDC_RADIO33, m_bHighOUT7);
	DDX_Radio(pDX, IDC_RADIO35, m_bHighOUT8);
	DDX_Radio(pDX, IDC_RADIO37, m_bHighOUT9);
	DDX_Radio(pDX, IDC_RADIO39, m_bHighOUT10);
}

BEGIN_MESSAGE_MAP(CBasicDlg, CDialog)
	//{{AFX_MSG_MAP(CBasicDlg)
	ON_WM_SYSCOMMAND()
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()

	ON_WM_CLOSE()
	ON_WM_TIMER()
	//}}AFX_MSG_MAP
	

	ON_BN_CLICKED(IDC_RADIO21, OnBnClickedOUT1)
	ON_BN_CLICKED(IDC_RADIO22, OnBnClickedOUT1)
	ON_BN_CLICKED(IDC_RADIO23, OnBnClickedOUT2)
	ON_BN_CLICKED(IDC_RADIO24, OnBnClickedOUT2)
	ON_BN_CLICKED(IDC_RADIO25, OnBnClickedOUT3)
	ON_BN_CLICKED(IDC_RADIO26, OnBnClickedOUT3)
	ON_BN_CLICKED(IDC_RADIO27, OnBnClickedOUT4)
	ON_BN_CLICKED(IDC_RADIO28, OnBnClickedOUT4)
	ON_BN_CLICKED(IDC_RADIO29, OnBnClickedOUT5)
	ON_BN_CLICKED(IDC_RADIO30, OnBnClickedOUT5)
	ON_BN_CLICKED(IDC_RADIO31, OnBnClickedOUT6)
	ON_BN_CLICKED(IDC_RADIO32, OnBnClickedOUT6)
	ON_BN_CLICKED(IDC_RADIO33, OnBnClickedOUT7)
	ON_BN_CLICKED(IDC_RADIO34, OnBnClickedOUT7)
	ON_BN_CLICKED(IDC_RADIO35, OnBnClickedOUT8)
	ON_BN_CLICKED(IDC_RADIO36, OnBnClickedOUT8)
	ON_BN_CLICKED(IDC_RADIO37, OnBnClickedOUT9)
	ON_BN_CLICKED(IDC_RADIO38, OnBnClickedOUT9)
	ON_BN_CLICKED(IDC_RADIO39, OnBnClickedOUT10)
	ON_BN_CLICKED(IDC_RADIO40, OnBnClickedOUT10)
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CBasicDlg message handlers

BOOL CBasicDlg::OnInitDialog()
{
	CDialog::OnInitDialog();

	// Add "About..." menu item to system menu.

	// IDM_ABOUTBOX must be in the system command range.
	ASSERT((IDM_ABOUTBOX & 0xFFF0) == IDM_ABOUTBOX);
	ASSERT(IDM_ABOUTBOX < 0xF000);

	CMenu* pSysMenu = GetSystemMenu(FALSE);
	if (pSysMenu != NULL)
	{
		CString strAboutMenu;
		strAboutMenu.LoadString(IDS_ABOUTBOX);
		if (!strAboutMenu.IsEmpty())
		{
			pSysMenu->AppendMenu(MF_SEPARATOR);
			pSysMenu->AppendMenu(MF_STRING, IDM_ABOUTBOX, strAboutMenu);
		}
	}

	// Set the icon for this dialog.  The framework does this automatically
	//  when the application's main window is not a dialog
	SetIcon(m_hIcon, TRUE);			// Set big icon
	SetIcon(m_hIcon, FALSE);		// Set small icon

	m_DlgLog.Create(IDD_DIALOG_LOG,this);//创建一个信息窗口来显示日志

	do 
	{
		m_DlgLog.ShowWindow(SW_SHOW);
		
// 		m_DlgLog.AppendLog("Basic Demo start");
// 		m_DlgLog.AppendLog("LoadSdkApi was called to load SDK api from MVCAMSDK.dll");
// 		m_DlgLog.AppendLog("LoadSdkApi is open source in CameraApiLoad.h ");
// 		m_DlgLog.AppendLog("It shows how to load the api from MVCAMSDK.dll,");
// 		m_DlgLog.AppendLog("you can also use your own way to load MVCAMSDK.dll");


		//Init the SDK，0:英文版 1:中文版 ，作用于相机的描述信息和SDK生成的设备配置界面上
		SDK_TRACE(CameraSdkInit(gLanguage),gLanguage?"初始化SDK":"Init SDK");

		if (!InitCamera())
		{
			break;
		}
		
		SetTimer(0,1000,NULL);//使用一个定时器来计算帧率

		return TRUE;

	} while(0);
	
	//没有找到相机或者初始化失败，退出程序
	EndDialog(0);
	return FALSE;  // return TRUE  unless you set the focus to a control
}

void CBasicDlg::OnSysCommand(UINT nID, LPARAM lParam)
{
	if ((nID & 0xFFF0) == IDM_ABOUTBOX)
	{
		CAboutDlg dlgAbout;
		dlgAbout.DoModal();
	}
	else
	{
		CDialog::OnSysCommand(nID, lParam);
	}
}

// If you add a minimize button to your dialog, you will need the code below
//  to draw the icon.  For MFC applications using the document/view model,
//  this is automatically done for you by the framework.

void CBasicDlg::OnPaint() 
{
	if (IsIconic())
	{
		CPaintDC dc(this); // device context for painting

		SendMessage(WM_ICONERASEBKGND, (WPARAM) dc.GetSafeHdc(), 0);

		// Center icon in client rectangle
		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;

		// Draw the icon
		dc.DrawIcon(x, y, m_hIcon);
	}
	else
	{
		CDialog::OnPaint();
	}
}

// The system calls this to obtain the cursor to display while the user drags
//  the minimized window.
HCURSOR CBasicDlg::OnQueryDragIcon()
{
	return (HCURSOR) m_hIcon;
}

BOOL CBasicDlg::InitCamera()
{
	tSdkCameraDevInfo DevInfo;
	CameraSdkStatus status;
	CRect rect;
	tSdkCameraCapbility sCameraInfo;

	status = CameraGrabber_CreateFromDevicePage(&m_Grabber);
	if (status != CAMERA_STATUS_SUCCESS)
	{
		if (status != CAMERA_STATUS_USER_CANCEL)
		{
			CString msg;
			msg.Format("Failed to init the camera! Error code is %d",status);
			MessageBox(msg);
		}
		return FALSE;
	}
	
	CameraGrabber_GetCameraHandle(m_Grabber, &m_hCamera);
	CameraGrabber_GetCameraDevInfo(m_Grabber, &DevInfo);
	SetWindowText(DevInfo.acFriendlyName);

	//Get properties description for this camera.
	SDK_TRACE(CameraGetCapability(m_hCamera,&sCameraInfo),gLanguage?"获得该相机的特性描述":"Get the capability of the camera");

	if (sCameraInfo.iInputIoCounts == 0 && sCameraInfo.iOutputIoCounts ==0)
	{
		MessageBox(gLanguage?"该型号相机没有GPIO，演示程序无法运行":"This camera has no GPIO,demo will be exit");
		return FALSE;
	}
	else
	{
		int i;

		m_iInputIoCounts = sCameraInfo.iInputIoCounts;

		for (i = 0;i < sCameraInfo.iInputIoCounts;i++)
		{
			GetDlgItem(IDC_STATIC_IN1 + i)->ShowWindow(SW_SHOW);
			GetDlgItem(IDC_RADIO1 + i*2)->ShowWindow(SW_SHOW);
			GetDlgItem(IDC_RADIO1 + i*2 + 1)->ShowWindow(SW_SHOW);
		}

		for (i = 0;i < sCameraInfo.iOutputIoCounts;i++)
		{
			GetDlgItem(IDC_STATIC_OUT1 + i)->ShowWindow(SW_SHOW);
			GetDlgItem(IDC_RADIO21 + i*2)->ShowWindow(SW_SHOW);
			GetDlgItem(IDC_RADIO21 + i*2 + 1)->ShowWindow(SW_SHOW);
		}
	}

    //IO初始化为低电平，与控件上显示的同步
    for (int i = 0;i < sCameraInfo.iOutputIoCounts;i++)
    {
        CameraSetIOState(m_hCamera,i,0);
    }

	return TRUE;
}


void CBasicDlg::OnClose() 
{
	//反初始化相机
	CameraGrabber_Destroy(m_Grabber);

	CDialog::OnClose();
}

void CBasicDlg::OnTimer(UINT_PTR nIDEvent)//一秒中刷新一次所有输入IO状态
{
	int i;

	BOOL *bInputStateGrop[10] =
	{

		&m_bHighIN1,
		&m_bHighIN2,
		&m_bHighIN3,
		&m_bHighIN4,
		&m_bHighIN5,
		&m_bHighIN6,
		&m_bHighIN7,
		&m_bHighIN8,
		&m_bHighIN9,
		&m_bHighIN10,

	};

	if (m_hCamera > 0 && m_iInputIoCounts > 0)
	{
		for (i = 0;i < m_iInputIoCounts;i++)
		{
			SDK_TRACE(CameraGetIOState(m_hCamera,i,(UINT*)bInputStateGrop[i]),gLanguage?"获得该相机输入IO状态":"Get the level of the input IO");
		}

		UpdateData(FALSE);
	}
	
    CDialog::OnTimer(nIDEvent);
}


void CBasicDlg::OnBnClickedOUT1()
{
	// TODO: 在此添加控件通知处理程序代码
	UpdateData(TRUE);
	SDK_TRACE(CameraSetIOState(m_hCamera,0,m_bHighOUT1),gLanguage?"设置输出IO1电平":"Set the level output IO1");
}


void CBasicDlg::OnBnClickedOUT2()
{
	// TODO: 在此添加控件通知处理程序代码
	UpdateData(TRUE);
	SDK_TRACE(CameraSetIOState(m_hCamera,1,m_bHighOUT2),gLanguage?"设置输出IO2电平":"Set the level output IO2");
}


void CBasicDlg::OnBnClickedOUT3()
{
	// TODO: 在此添加控件通知处理程序代码
	UpdateData(TRUE);
	SDK_TRACE(CameraSetIOState(m_hCamera,2,m_bHighOUT3),gLanguage?"设置输出IO3电平":"Set the level output IO3");
}


void CBasicDlg::OnBnClickedOUT4()
{
	// TODO: 在此添加控件通知处理程序代码
	UpdateData(TRUE);
	SDK_TRACE(CameraSetIOState(m_hCamera,3,m_bHighOUT4),gLanguage?"设置输出IO4电平":"Set the level output IO4");
}

void CBasicDlg::OnBnClickedOUT5()
{
	// TODO: 在此添加控件通知处理程序代码
	UpdateData(TRUE);
	SDK_TRACE(CameraSetIOState(m_hCamera,4,m_bHighOUT5),gLanguage?"设置输出IO5电平":"Set the level output IO5");
}

void CBasicDlg::OnBnClickedOUT6()
{
	// TODO: 在此添加控件通知处理程序代码
	UpdateData(TRUE);
	SDK_TRACE(CameraSetIOState(m_hCamera,5,m_bHighOUT6),gLanguage?"设置输出IO6电平":"Set the level output IO6");
}

void CBasicDlg::OnBnClickedOUT7()
{
	// TODO: 在此添加控件通知处理程序代码
	UpdateData(TRUE);
	SDK_TRACE(CameraSetIOState(m_hCamera,6,m_bHighOUT7),gLanguage?"设置输出IO7电平":"Set the level output IO7");
}

void CBasicDlg::OnBnClickedOUT8()
{
	// TODO: 在此添加控件通知处理程序代码
	UpdateData(TRUE);
	SDK_TRACE(CameraSetIOState(m_hCamera,7,m_bHighOUT8),gLanguage?"设置输出IO8电平":"Set the level output IO8");
}

void CBasicDlg::OnBnClickedOUT9()
{
	// TODO: 在此添加控件通知处理程序代码
	UpdateData(TRUE);
	SDK_TRACE(CameraSetIOState(m_hCamera,8,m_bHighOUT9),gLanguage?"设置输出IO9电平":"Set the level output IO9");
}

void CBasicDlg::OnBnClickedOUT10()
{
	// TODO: 在此添加控件通知处理程序代码
	UpdateData(TRUE);
	SDK_TRACE(CameraSetIOState(m_hCamera,9,m_bHighOUT10),gLanguage?"设置输出IO10电平":"Set the level output IO10");
}


