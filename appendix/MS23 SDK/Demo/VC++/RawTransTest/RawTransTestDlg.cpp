
// RawTransTestDlg.cpp : 实现文件
//
//BIG5 TRANS ALLOWED


#include "stdafx.h"
#include "RawTransTest.h"
#include "RawTransTestDlg.h"
#include "afxdialogex.h"
#include "RawDecodeApi.h"
#include "CameraDefine.h"
#ifdef _DEBUG
#define new DEBUG_NEW
#endif


// 用于应用程序“关于”菜单项的 CAboutDlg 对话框

class CAboutDlg : public CDialogEx
{
public:
	CAboutDlg();

// 对话框数据
	enum { IDD = IDD_ABOUTBOX };

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV 支持

// 实现
protected:
	DECLARE_MESSAGE_MAP()
};

CAboutDlg::CAboutDlg() : CDialogEx(CAboutDlg::IDD)
{
}

void CAboutDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CAboutDlg, CDialogEx)
END_MESSAGE_MAP()


// CRawTransTestDlg 对话框




CRawTransTestDlg::CRawTransTestDlg(CWnd* pParent /*=NULL*/)
	: CDialogEx(CRawTransTestDlg::IDD, pParent)
	, m_sRawFile(_T("C:\\IMAGE\\test.RAW"))
	, m_sOutPutFile(_T("C:\\IMAGE\\test.bmp"))
	, m_sConfigFile(_T("C:\\IMAGE\\MV-UB300-Group0.config")
	)
	, m_sJpgOutFile(_T("C:\\IMAGE\\test.jpg"))
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
	m_pCameraConfig = NULL;

}

void CRawTransTestDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
	DDX_Text(pDX, IDC_EDIT1, m_sRawFile);
	DDX_Text(pDX, IDC_EDIT2, m_sOutPutFile);
	DDX_Text(pDX, IDC_EDIT3, m_sConfigFile);
	DDX_Text(pDX, IDC_EDIT4, m_sJpgOutFile);
}

BEGIN_MESSAGE_MAP(CRawTransTestDlg, CDialogEx)
	ON_WM_SYSCOMMAND()
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_BN_CLICKED(IDC_BUTTON_TRANS, &CRawTransTestDlg::OnBnClickedButtonTrans)
	ON_BN_CLICKED(IDC_BUTTON_TRANS2, &CRawTransTestDlg::OnBnClickedButtonTrans2)
	ON_WM_CLOSE()
END_MESSAGE_MAP()


// CRawTransTestDlg 消息处理程序

BOOL CRawTransTestDlg::OnInitDialog()
{
	CDialogEx::OnInitDialog();

	// 将“关于...”菜单项添加到系统菜单中。

	// IDM_ABOUTBOX 必须在系统命令范围内。
	ASSERT((IDM_ABOUTBOX & 0xFFF0) == IDM_ABOUTBOX);
	ASSERT(IDM_ABOUTBOX < 0xF000);

	CMenu* pSysMenu = GetSystemMenu(FALSE);
	if (pSysMenu != NULL)
	{
		BOOL bNameValid;
		CString strAboutMenu;
		bNameValid = strAboutMenu.LoadString(IDS_ABOUTBOX);
		ASSERT(bNameValid);
		if (!strAboutMenu.IsEmpty())
		{
			pSysMenu->AppendMenu(MF_SEPARATOR);
			pSysMenu->AppendMenu(MF_STRING, IDM_ABOUTBOX, strAboutMenu);
		}
	}

	// 设置此对话框的图标。当应用程序主窗口不是对话框时，框架将自动
	//  执行此操作
	SetIcon(m_hIcon, TRUE);			// 设置大图标
	SetIcon(m_hIcon, FALSE);		// 设置小图标

	// TODO: 在此添加额外的初始化代码

	//按最大的图像分辨率进行初始化,为了兼容1400万像素，这里假设分辨率最大下是4384X3288。1400万像素以下的相机，这里可以不修改。
	RawDecodeInit(4384,3288);
	return TRUE;  // 除非将焦点设置到控件，否则返回 TRUE
}

void CRawTransTestDlg::OnSysCommand(UINT nID, LPARAM lParam)
{
	if ((nID & 0xFFF0) == IDM_ABOUTBOX)
	{
		CAboutDlg dlgAbout;
		dlgAbout.DoModal();
	}
	else
	{
		CDialogEx::OnSysCommand(nID, lParam);
	}
}

// 如果向对话框添加最小化按钮，则需要下面的代码
//  来绘制该图标。对于使用文档/视图模型的 MFC 应用程序，
//  这将由框架自动完成。

void CRawTransTestDlg::OnPaint()
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
HCURSOR CRawTransTestDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}


//转换成BMP文件
void CRawTransTestDlg::OnBnClickedButtonTrans()
{
	// TODO: 在此添加控件通知处理程序代码
	int ret;
	UpdateData(TRUE);
	if (m_pCameraConfig == NULL)
	{

		//加载相机配置文件
		//如果需要解码多个相机的RAW，则需要调用多次LoadCameraConfigFile来加载不同的相机配置文件
		if ((m_pCameraConfig = LoadCameraConfigFile(m_sConfigFile.GetBuffer(1))) == NULL)
		{
			MessageBox("Failed to load the config file of the camera");
			m_sConfigFile.ReleaseBuffer();
			return;
		}

		m_sConfigFile.ReleaseBuffer();

	}

	ret = RawToBmp(m_sRawFile.GetBuffer(1),m_sOutPutFile.GetBuffer(1),m_pCameraConfig);
	m_sRawFile.ReleaseBuffer();
	m_sOutPutFile.ReleaseBuffer();

	if (ret == CAMERA_STATUS_SUCCESS)
	{
		MessageBox("Success down!");
	}
	else
	{
		MessageBox("Transfer failed,please check the settings.");
	}
}


//转换成JPG文件
void CRawTransTestDlg::OnBnClickedButtonTrans2()
{
	// TODO: 在此添加控件通知处理程序代码
	int ret;
	UpdateData(TRUE);
	if (m_pCameraConfig == NULL)
	{

		//加载相机配置文件
		//如果需要解码多个相机的RAW，则需要调用多次LoadCameraConfigFile来加载不同的相机配置文件
		if ((m_pCameraConfig = LoadCameraConfigFile(m_sConfigFile.GetBuffer(1))) == NULL)
		{
			MessageBox("Failed to load the config file of the camera");
			m_sConfigFile.ReleaseBuffer();
			return;
		}

		m_sConfigFile.ReleaseBuffer();

	}

	ret = RawToBmp(m_sRawFile.GetBuffer(1),m_sJpgOutFile.GetBuffer(1),m_pCameraConfig);//不同相机拍出来的RAW文件，需要加载对应的相机参数配置文件，得到句柄后调用RawToBmp函数
	m_sRawFile.ReleaseBuffer();
	m_sJpgOutFile.ReleaseBuffer();
	
	if (ret == CAMERA_STATUS_SUCCESS)
	{
		MessageBox("Success down!");
	}
	else
	{
		MessageBox("Transfer failed,please check the settings.");
	}
}


void CRawTransTestDlg::OnClose()
{
	// TODO: 在此添加消息处理程序代码和/或调用默认值

	// 释放相机参数句柄
	if (m_pCameraConfig != NULL)
	{
		UnLoadCameraConfigFile(m_pCameraConfig);
		m_pCameraConfig = NULL;
	}

	// 反初始化RAW编码库,释放内存分配
	RawDecodeUnInit();


	CDialogEx::OnClose();
}
