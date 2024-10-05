// OCXDlg.cpp : implementation file
//
//BIG5 TRANS ALLOWED
#include "stdafx.h"
#include "OCX.h"
#include "OCXDlg.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CAboutDlg dialog used for App About

class CAboutDlg : public CDialog
{
public:
	CAboutDlg();

// Dialog Data
	//{{AFX_DATA(CAboutDlg)
	enum { IDD = IDD_ABOUTBOX };
	//}}AFX_DATA

	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CAboutDlg)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:
	//{{AFX_MSG(CAboutDlg)
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

CAboutDlg::CAboutDlg() : CDialog(CAboutDlg::IDD)
{
	//{{AFX_DATA_INIT(CAboutDlg)
	//}}AFX_DATA_INIT
}

void CAboutDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CAboutDlg)
	//}}AFX_DATA_MAP
}

BEGIN_MESSAGE_MAP(CAboutDlg, CDialog)
	//{{AFX_MSG_MAP(CAboutDlg)
		// No message handlers
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// COCXDlg dialog

COCXDlg::COCXDlg(CWnd* pParent /*=NULL*/)
	: CDialog(COCXDlg::IDD, pParent)
{
	//{{AFX_DATA_INIT(COCXDlg)
	
	m_iFileType = 0;
	m_iFileType1 = 0;
	m_bPaused = FALSE;
	m_bPaused1 = FALSE;
	m_sFileName = ".//test";
	m_sFileName1 = ".//test1";
	m_bCameraFound = FALSE;
	m_bCameraFound1 = FALSE;
	//}}AFX_DATA_INIT
	// Note that LoadIcon does not require a subsequent DestroyIcon in Win32
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
}

void COCXDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(COCXDlg)
	DDX_Text(pDX, IDC_EDIT1, m_sFileName);
	DDX_Radio(pDX, IDC_RADIO1, m_iFileType);
	DDX_Control(pDX, IDC_MINDVISIONCTRL1, m_MvOcx);
	DDX_Text(pDX, IDC_EDIT2, m_sFileName1);
	DDX_Radio(pDX, IDC_RADIO3, m_iFileType1);
	DDX_Control(pDX, IDC_MINDVISIONCTRL2, m_MvOcx1);
	//}}AFX_DATA_MAP
}

BEGIN_MESSAGE_MAP(COCXDlg, CDialog)
	//{{AFX_MSG_MAP(COCXDlg)
	ON_WM_SYSCOMMAND()
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_BN_CLICKED(IDC_BUTTON_PAUSE, OnButtonPause)
	ON_BN_CLICKED(IDC_BUTTON_CAMERA_SET, OnButtonCameraSet)
	ON_BN_CLICKED(IDC_BUTTON_SNAPSHOT, OnButtonSnapshot)
	ON_BN_CLICKED(IDC_BUTTON_SNAP_TO_BUFFER, OnButtonSnapToBuffer)
	ON_BN_CLICKED(IDC_BUTTON_PAUSE2, OnButtonPause1)
	ON_BN_CLICKED(IDC_BUTTON_CAMERA_SET2, OnButtonCameraSet1)
	ON_BN_CLICKED(IDC_BUTTON_SNAPSHOT2, OnButtonSnapshot1)
	ON_BN_CLICKED(IDC_BUTTON_SNAP_TO_BUFFER2, OnButtonSnapToBuffer1)
	ON_WM_CLOSE()
	//}}AFX_MSG_MAP
	ON_WM_TIMER()
END_MESSAGE_MAP()


BOOL COCXDlg::CameraInit()
{
	if (m_MvOcx.mvCameraInit(0) != FALSE)//初始化失败
	{
		m_bCameraFound = TRUE;
	}

	if (m_MvOcx1.mvCameraInit(1) != FALSE)//初始化失败
	{
		m_bCameraFound1 = TRUE;
	}

	m_bThreadQuit[0] = m_bThreadQuit[1] = FALSE;
	m_hThread[0] = m_hThread[1] = NULL;

	if (m_bCameraFound)
	{
		m_MvOcx.mvCameraPlay();
		m_hThread[0] = (HANDLE)_beginthreadex(NULL, 0, DisplayThreadEntry1, this, 0, NULL);
	}
	
	if (m_bCameraFound1)
	{
		m_MvOcx1.mvCameraPlay();
		m_hThread[1] = (HANDLE)_beginthreadex(NULL, 0, DisplayThreadEntry2, this, 0, NULL);
	}

	return TRUE;
}
/////////////////////////////////////////////////////////////////////////////
// COCXDlg message handlers

BOOL COCXDlg::OnInitDialog()
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
	
	// TODO: Add extra initialization here
	if (CameraInit() == FALSE)
	{
		MessageBox("Failed to access the camera!");
		return FALSE;
	}
	
	return TRUE;  // return TRUE  unless you set the focus to a control
}

void COCXDlg::OnSysCommand(UINT nID, LPARAM lParam)
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

void COCXDlg::OnPaint() 
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
HCURSOR COCXDlg::OnQueryDragIcon()
{
	return (HCURSOR) m_hIcon;
}

void COCXDlg::OnButtonPause() 
{
	// TODO: Add your control notification handler code here

	if (!m_bCameraFound)
	{
		MessageBox("No Camera!");
		return;
	}

	m_bPaused = !m_bPaused;

	if (m_bPaused)
	{
		m_MvOcx.mvCameraEnablePreview(0);
		GetDlgItem(IDC_BUTTON_PAUSE)->SetWindowText("Play");
	} 
	else
	{
		m_MvOcx.mvCameraEnablePreview(1);
		GetDlgItem(IDC_BUTTON_PAUSE)->SetWindowText("Pause");
	}
	
}

void COCXDlg::OnButtonCameraSet() 
{
	// TODO: Add your control notification handler code here
	m_MvOcx.mvCameraShowConfigPage();
}

void COCXDlg::OnButtonSnapshot() 
{
	// TODO: Add your control notification handler code here
	UpdateData(TRUE);
	
	if (!m_bCameraFound)
	{
		MessageBox("No Camera!");
		return;
	}

	if (m_iFileType == 0)
	{
		m_MvOcx.mvCameraSaveJpgEx(m_sFileName);
	}
	else
	{
		m_MvOcx.mvCameraSaveBmpEx(m_sFileName);
	}
}

void COCXDlg::OnButtonSnapToBuffer() 
{
	// TODO: Add your control notification handler code here
	if (!m_bCameraFound)
	{
		MessageBox("No Camera!");
		return;
	}

	long lWidth;
	long lHeight;
	BYTE* bFrameBuffer;
	bFrameBuffer = (BYTE*)m_MvOcx.mvCameraGetImage(&lWidth,&lHeight,24);//返回RGB24格式的数据
//	bFrameBuffer = (BYTE*)m_MvOcx.mvCameraGetImage(&lWidth,&lHeight,8);//返回8位灰度格式的数据
//	bFrameBuffer = (BYTE*)m_MvOcx.mvCameraGetImage(&lWidth,&lHeight,32);//返回RGBA 32位格式的数据

}


void COCXDlg::OnButtonPause1() 
{
	// TODO: Add your control notification handler code here
	if (!m_bCameraFound1)
	{
		MessageBox("No Camera!");
		return;
	}

	m_bPaused1 = !m_bPaused1;
	
	if (m_bPaused1)
	{
		m_MvOcx1.mvCameraEnablePreview(0);
		GetDlgItem(IDC_BUTTON_PAUSE)->SetWindowText("Play");
	} 
	else
	{
		m_MvOcx1.mvCameraEnablePreview(1);
		GetDlgItem(IDC_BUTTON_PAUSE)->SetWindowText("Pause");
	}
	
}

void COCXDlg::OnButtonCameraSet1() 
{
	// TODO: Add your control notification handler code here
	if (!m_bCameraFound1)
	{
		MessageBox("No Camera!");
		return;
	}

	m_MvOcx1.mvCameraShowConfigPage();
}

void COCXDlg::OnButtonSnapshot1() 
{
	// TODO: Add your control notification handler code here

	if (!m_bCameraFound1)
	{
		MessageBox("No Camera!");
		return;
	}

	UpdateData(TRUE);
	
	if (m_iFileType1 == 0)
	{
		m_MvOcx1.mvCameraSaveJpg(m_sFileName1);
	}
	else
	{
		m_MvOcx1.mvCameraSaveBmp(m_sFileName1);
	}
}

void COCXDlg::OnButtonSnapToBuffer1() 
{
	// TODO: Add your control notification handler code here
	if (!m_bCameraFound1)
	{
		MessageBox("No Camera!");
		return;
	}

	long lWidth;
	long lHeight;
	BYTE* bFrameBuffer;
	bFrameBuffer = (BYTE*)m_MvOcx1.mvCameraGetImage(&lWidth,&lHeight,32);
}

void COCXDlg::OnClose() 
{
	m_bThreadQuit[0] = m_bThreadQuit[1] = TRUE;
	if (m_hThread[0] != NULL)
	{
		WaitForSingleObject(m_hThread[0], INFINITE);
		CloseHandle(m_hThread[0]);
		m_hThread[0] = NULL;
	}
	if (m_hThread[1] != NULL)
	{
		WaitForSingleObject(m_hThread[1], INFINITE);
		CloseHandle(m_hThread[1]);
		m_hThread[1] = NULL;
	}

	// 相机反初始化
	if (m_bCameraFound)
	{
		m_MvOcx.mvCameraUnInit();
	}

	if (m_bCameraFound1)
	{
		m_MvOcx1.mvCameraUnInit();
	}

	CDialog::OnClose();
}

void COCXDlg::OnTimer(UINT_PTR nIDEvent)
{
	// TODO: 在此添加消息处理程序代码和/或调用默认值

	CDialog::OnTimer(nIDEvent);
}

unsigned COCXDlg::DisplayThread1()
{
	long w, h, buffer, bpp = 24;
	while (!m_bThreadQuit[0])
	{
		buffer = m_MvOcx.mvCameraGetImage(&w, &h, bpp);
		if (buffer != 0)
		{
			m_MvOcx.mvCameraDisplayImage(buffer, w, h, bpp);
		}
	}

	return 0;
}

unsigned COCXDlg::DisplayThread2()
{
	long w, h, buffer, bpp = 24;
	while (!m_bThreadQuit[1])
	{
		buffer = m_MvOcx1.mvCameraGetImage(&w, &h, bpp);
		if (buffer != 0)
		{
			m_MvOcx1.mvCameraDisplayImage(buffer, w, h, bpp);
		}
	}

	return 0;
}
