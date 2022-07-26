
// GigeConfigDlg.cpp : 实现文件
//

#include "stdafx.h"
#include "GigeConfig.h"
#include "GigeConfigDlg.h"
#include "afxdialogex.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


// CGigeConfigDlg 对话框




CGigeConfigDlg::CGigeConfigDlg(CWnd* pParent /*=NULL*/)
	: CDialogEx(CGigeConfigDlg::IDD, pParent)
	, m_strCamIp(_T(""))
	, m_strCamMask(_T(""))
	, m_strCamGateway(_T(""))
	, m_strCamMac(_T(""))
	, m_strEtIp(_T(""))
	, m_strEtMask(_T(""))
	, m_strEtGateway(_T(""))
	, m_strEtMac(_T(""))
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
}

void CGigeConfigDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_LIST_CAMERA, m_ListBoxCamera);
	DDX_Text(pDX, IDC_EDIT_CAM_IP, m_strCamIp);
	DDX_Text(pDX, IDC_EDIT_CAM_MASK, m_strCamMask);
	DDX_Text(pDX, IDC_EDIT_CAM_GATEWAY, m_strCamGateway);
	DDX_Text(pDX, IDC_EDIT_CAM_MAC, m_strCamMac);
	DDX_Text(pDX, IDC_EDIT_ET_IP, m_strEtIp);
	DDX_Text(pDX, IDC_EDIT_ET_MASK, m_strEtMask);
	DDX_Text(pDX, IDC_EDIT_ET_GATEWAY, m_strEtGateway);
	DDX_Text(pDX, IDC_EDIT_ET_MAC, m_strEtMac);
}

BEGIN_MESSAGE_MAP(CGigeConfigDlg, CDialogEx)
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_BN_CLICKED(IDC_BUTTON_UPDATE, &CGigeConfigDlg::OnBnClickedButtonUpdate)
	ON_LBN_SELCHANGE(IDC_LIST_CAMERA, &CGigeConfigDlg::OnLbnSelchangeListCamera)
	ON_BN_CLICKED(IDC_BUTTON_APPLY, &CGigeConfigDlg::OnBnClickedButtonApply)
END_MESSAGE_MAP()


// CGigeConfigDlg 消息处理程序

BOOL CGigeConfigDlg::OnInitDialog()
{
	CDialogEx::OnInitDialog();

	// 设置此对话框的图标。当应用程序主窗口不是对话框时，框架将自动
	//  执行此操作
	SetIcon(m_hIcon, TRUE);			// 设置大图标
	SetIcon(m_hIcon, FALSE);		// 设置小图标

	// TODO: 在此添加额外的初始化代码
	OnBnClickedButtonUpdate();

	return TRUE;  // 除非将焦点设置到控件，否则返回 TRUE
}

// 如果向对话框添加最小化按钮，则需要下面的代码
//  来绘制该图标。对于使用文档/视图模型的 MFC 应用程序，
//  这将由框架自动完成。

void CGigeConfigDlg::OnPaint()
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
HCURSOR CGigeConfigDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}

void CGigeConfigDlg::OnBnClickedButtonUpdate()
{
	m_ListBoxCamera.ResetContent();
	m_CamerasInfo.RemoveAll();

	tSdkCameraDevInfo DevInfo[64];
	INT nCount = ARRAYSIZE(DevInfo);
	if (CameraEnumerateDevice(DevInfo, &nCount) == CAMERA_STATUS_SUCCESS)
	{
		for (int i = 0; i < nCount; ++i)
		{
			if (memcmp(DevInfo[i].acPortType, "NET", 3) != 0)
				continue;

			m_ListBoxCamera.AddString(DevInfo[i].acFriendlyName);
			m_CamerasInfo.Add(DevInfo[i]);
		}
	}
}

void CGigeConfigDlg::OnLbnSelchangeListCamera()
{
	m_strCamIp.Empty();
	m_strCamMask.Empty();
	m_strCamGateway.Empty();
	m_strEtIp.Empty();
	m_strEtMask.Empty();
	m_strEtGateway.Empty();

	int Index = m_ListBoxCamera.GetCurSel();
	if (Index >= 0)
	{
		CameraGigeGetIp(&m_CamerasInfo[Index], 
			m_strCamIp.GetBufferSetLength(16),
			m_strCamMask.GetBufferSetLength(16),
			m_strCamGateway.GetBufferSetLength(16),
			m_strEtIp.GetBufferSetLength(16),
			m_strEtMask.GetBufferSetLength(16),
			m_strEtGateway.GetBufferSetLength(16));
		m_strCamIp.ReleaseBuffer();
		m_strCamMask.ReleaseBuffer();
		m_strCamGateway.ReleaseBuffer();
		m_strEtIp.ReleaseBuffer();
		m_strEtMask.ReleaseBuffer();
		m_strEtGateway.ReleaseBuffer();

		CameraGigeGetMac(&m_CamerasInfo[Index], 
			m_strCamMac.GetBufferSetLength(18),
			m_strEtMac.GetBufferSetLength(18));
		m_strCamMac.ReleaseBuffer();
		m_strEtMac.ReleaseBuffer();
	}
	
	UpdateData(FALSE);
}

void CGigeConfigDlg::OnBnClickedButtonApply()
{
	UpdateData(TRUE);
	int Index = m_ListBoxCamera.GetCurSel();
	if (Index >= 0)
	{
		if (CameraGigeSetIp(&m_CamerasInfo[Index], m_strCamIp, m_strCamMask, m_strCamGateway, TRUE) == CAMERA_STATUS_SUCCESS)
			MessageBox("设置成功");
		else
			MessageBox("设置失败");
	}
}
