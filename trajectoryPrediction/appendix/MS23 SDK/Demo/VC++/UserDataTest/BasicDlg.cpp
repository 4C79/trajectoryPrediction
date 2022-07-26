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
, m_sBlockDataReadOut(_T(""))
, m_sBlockDataWrite(_T(""))
, m_sSerialNumber1(_T(""))
, m_sSerialNumber2(_T(""))
, m_sSerialNumber3(_T(""))
, m_sCameraName(_T(""))
, m_sCameraNameDesc(_T(""))
, m_sSerialNumberDesc(_T(""))
, m_sBlockDataStaticBox(_T(""))
{
	//{{AFX_DATA_INIT(CBasicDlg)
		// NOTE: the ClassWizard will add member initialization here
	//}}AFX_DATA_INIT
	// Note that LoadIcon does not require a subsequent DestroyIcon in Win32
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
	m_hCamera = -1;

}

void CBasicDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CBasicDlg)
	//}}AFX_DATA_MAP
	DDX_Text(pDX, IDC_EDIT_BLOCK_DATA_READOUT, m_sBlockDataReadOut);
	DDX_Text(pDX, IDC_EDIT_BLOCK_DATA_WRITE, m_sBlockDataWrite);
	DDX_Text(pDX, IDC_EDIT_SN1, m_sSerialNumber1);
	DDX_Text(pDX, IDC_EDIT_SN2, m_sSerialNumber2);
	DDX_Text(pDX, IDC_EDIT_SN3, m_sSerialNumber3);
	DDX_Text(pDX, IDC_EDIT_CAMERA_NAME, m_sCameraName);
	DDX_Text(pDX, IDC_EDIT_NAME_DESC, m_sCameraNameDesc);
	DDX_Text(pDX, IDC_EDIT_SN_DESC, m_sSerialNumberDesc);
	DDX_Text(pDX, IDC_STATIC_BLOCK_DATA, m_sBlockDataStaticBox);
}

BEGIN_MESSAGE_MAP(CBasicDlg, CDialog)
	//{{AFX_MSG_MAP(CBasicDlg)
	ON_WM_SYSCOMMAND()
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_WM_CLOSE()

	//}}AFX_MSG_MAP
	ON_BN_CLICKED(IDC_BUTTON_NAME_READ, OnBnClickedButtonNameRead)
	ON_BN_CLICKED(IDC_BUTTON_NAME_WRITE, OnBnClickedButtonNameWrite)
	ON_BN_CLICKED(IDC_BUTTON_SN_READ2, OnBnClickedButtonSnRead2)
	ON_BN_CLICKED(IDC_BUTTON_SN_WRITE2, OnBnClickedButtonSnWrite2)
	ON_BN_CLICKED(IDC_BUTTON_SN_READ3, OnBnClickedButtonSnRead3)
	ON_BN_CLICKED(IDC_BUTTON_SN_WRITE3, OnBnClickedButtonSnWrite3)
	ON_BN_CLICKED(IDC_BUTTON_DATA_WRITE_EDIT, OnBnClickedButtonDataWriteEdit)
	ON_BN_CLICKED(IDC_BUTTON_DATA_WRITE_ZERO, OnBnClickedButtonDataWriteZero)
	ON_BN_CLICKED(IDC_BUTTON_DATA_WRITE_0XFF, OnBnClickedButtonDataWrite0xff)
	ON_BN_CLICKED(IDC_BUTTON_DATA_WRITE_RANDOM, OnBnClickedButtonDataWriteRandom)
	ON_BN_CLICKED(IDC_BUTTON_DATA_WRITE_INC, OnBnClickedButtonDataWriteInc)
	ON_BN_CLICKED(IDC_BUTTON_DATA_READ, OnBnClickedButtonDataRead)
	ON_BN_CLICKED(IDC_BUTTON_SN_READ1, OnBnClickedButtonSnRead1)
	ON_BN_CLICKED(IDC_BUTTON_SN_WRITE1, &CBasicDlg::OnBnClickedButtonSnWrite1)
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
	    
		UpdateData(FALSE);

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
	tSdkCameraDevInfo sCameraList[10];
	INT iCameraNums;
	CameraSdkStatus status;
	CRect rect;
	tSdkCameraCapbility tCapability;
	
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

	SDK_TRACE(CameraGetCapability(m_hCamera,&tCapability),gLanguage?"获得相机特性描述":"Get the capability of the camera");

	m_sBlockDataStaticBox.Format("自定义数据块读写测试,该相机用户数据区长度为[%d]字节",tCapability.iUserDataMaxLen);
	
	m_iMaxBlockDatalen = tCapability.iUserDataMaxLen;

	m_sCameraNameDesc = "说明：所有型号的工业相机均支持相机名称自定义功能,该功能可方便软件区分多相机，同时相机中也包含了唯一的序列号,\
也可以用于区分多相机。相机名称通过专用的API接口修改后，永久有效，和电脑以及使用的软件无关，修改相机名称后，无论使用哪台\
电脑、无论是何种软件接口访问相机相机名称都会是您修改过的名称。默认情况下，相机名称是根据型号自动生成的，例如如果接了2台MV-U130M，那么它们的\
的名称分别是MV-U130M#0 和 MV-U130M#1。名称修改后，需要将相机断电重启后才能生效。";

	m_sSerialNumberDesc = "说明：相机预留3级序列号可供二次开发访问。其中第1级序列号是出厂时已经写入，用户无法修改，为只读状态；2,3级序列号可以通过\
该例程演示的API接口进行修改，或者您也可以直接使用该DEMO进行序列号改写操作。序列号长度最大为32个字节。您也可以将2,3级序列号\
的存储空间用作其他的自定义数据。";

	//另外需要注意的是：CameraSaveUserData函数，用于保存用户自定义的数据，但是访问的起始地址必须是32字节对齐的。否则可能造成保存数据无效。
	return TRUE;
}


void CBasicDlg::OnClose() 
{
	//反初始化相机
	if (m_hCamera > 0)
	{
	
		//反初始化相机。
		CameraUnInit(m_hCamera);
		m_hCamera = 0;
	}

 
	CDialog::OnClose();
}


void CBasicDlg::OnBnClickedButtonNameRead()
{
	// TODO: 在此添加控件通知处理程序代码
	char frName[32];

	SDK_TRACE(CameraGetFriendlyName(m_hCamera,frName),"获得自定义设备名");

	m_sCameraName = frName;
	//如果读出来的数据不是有效的字符串，则表示该相机并未自定义过名称，显示出来可能就是乱码。此时其名称是通过型号名自动生成的。

	UpdateData(FALSE);
}


void CBasicDlg::OnBnClickedButtonNameWrite()
{
	// TODO: 在此添加控件通知处理程序代码
	UpdateData(TRUE);

	//写入英文 “auto” 或者中文 “自动” ，则表示使用型号自动生成相机名称，相机出厂时默认就是该方式。

	SDK_TRACE(CameraSetFriendlyName(m_hCamera,m_sCameraName.GetBuffer(32)),"设置自定义相机名称");

	m_sCameraName.ReleaseBuffer();

}


void CBasicDlg::OnBnClickedButtonSnRead2()
{
	// TODO: 在此添加控件通知处理程序代码
	char sn[32];
	memset(sn,0,32);
	//默认情况下，2,3级别序列号是没有经过初始化的，读出来的数据可能是乱码。
	SDK_TRACE(CameraReadSN(m_hCamera,(BYTE*)sn,1),"读二级序列号");
	m_sSerialNumber2 = sn;
	UpdateData(FALSE);
}


void CBasicDlg::OnBnClickedButtonSnWrite2()
{
	// TODO: 在此添加控件通知处理程序代码
	UpdateData(TRUE);
	SDK_TRACE(CameraWriteSN(m_hCamera,(BYTE*)m_sSerialNumber2.GetBuffer(32),1),"写入二级序列号");
	m_sSerialNumber2.ReleaseBuffer();

}


void CBasicDlg::OnBnClickedButtonSnRead3()
{
	// TODO: 在此添加控件通知处理程序代码
	char sn[32];
	memset(sn,0,32);
	//默认情况下，2,3级别序列号是没有经过初始化的，读出来的数据可能是乱码。
	SDK_TRACE(CameraReadSN(m_hCamera,(BYTE*)sn,2),"读取三级序列号");

	m_sSerialNumber3 = sn;
	UpdateData(FALSE);
}


void CBasicDlg::OnBnClickedButtonSnWrite3()
{
	// TODO: 在此添加控件通知处理程序代码
	UpdateData(TRUE);
	SDK_TRACE(CameraWriteSN(m_hCamera,(BYTE*)m_sSerialNumber3.GetBuffer(32),2),"写入三级序列号");
	m_sSerialNumber3.ReleaseBuffer();
}


void CBasicDlg::OnBnClickedButtonDataWriteEdit()
{
	// TODO: 在此添加控件通知处理程序代码
	UpdateData(TRUE);
	CString msg;
	BYTE *pReadBuffer;
	BYTE *pWriteBuffer;
	int i;
	int iWriteDataLen = m_sBlockDataWrite.GetLength();

	if (iWriteDataLen > m_iMaxBlockDatalen)
	{
		msg.Format("输入数据过长，该相机用户数据区长度为%d字节，已输入%d字节，超出的字节将不会被写入设备中。",m_iMaxBlockDatalen,iWriteDataLen);
		MessageBox(msg);
		iWriteDataLen = m_iMaxBlockDatalen;
	}
	
	pReadBuffer = (BYTE*)malloc(m_iMaxBlockDatalen);
	pWriteBuffer = (BYTE*)malloc(m_iMaxBlockDatalen);
	memcpy(pWriteBuffer,m_sBlockDataWrite.GetBuffer(iWriteDataLen),iWriteDataLen);
	m_sBlockDataWrite.ReleaseBuffer();

	do 
	{
		if (CameraSaveUserData(m_hCamera,0,pWriteBuffer,iWriteDataLen) != CAMERA_STATUS_SUCCESS)
		{
			MessageBox("写入数据出错");
			break;
		}



		if (CameraLoadUserData(m_hCamera,0,pReadBuffer,iWriteDataLen) != CAMERA_STATUS_SUCCESS)
		{
			MessageBox("读取数据出错");
			break;
		}

		for (i = 0;i < iWriteDataLen;i++)
		{
			if (pReadBuffer[i] != pWriteBuffer[i])
			{
				MessageBox("数据校验失败！");
				break;
			}
		}

		if (i == iWriteDataLen)
		{
			MessageBox("写入成功（已校验）");

			pReadBuffer[iWriteDataLen] = 0;//补一个结束符到最后，用于字符串测试显示。

			m_sBlockDataReadOut = pReadBuffer;

			UpdateData(FALSE);
		}
		
	} while (0);
	

	free(pReadBuffer);
	free(pWriteBuffer);
}

//用户数据区全部写入0然后再读取进行对比
void CBasicDlg::OnBnClickedButtonDataWriteZero()
{
	// TODO: 在此添加控件通知处理程序代码
	UpdateData(TRUE);
	CString msg;
	BYTE *pReadBuffer;
	BYTE *pWriteBuffer;
	int i;

	pReadBuffer = (BYTE*)malloc(m_iMaxBlockDatalen);
	pWriteBuffer = (BYTE*)malloc(m_iMaxBlockDatalen);

	memset(pWriteBuffer,0,m_iMaxBlockDatalen);

	do 
	{
		if (CameraSaveUserData(m_hCamera,0,pWriteBuffer,m_iMaxBlockDatalen) != CAMERA_STATUS_SUCCESS)
		{
			MessageBox("写入数据出错");
			break;
		}



		if (CameraLoadUserData(m_hCamera,0,pReadBuffer,m_iMaxBlockDatalen) != CAMERA_STATUS_SUCCESS)
		{
			MessageBox("读取数据出错");
			break;
		}

		for (i = 0;i < m_iMaxBlockDatalen;i++)
		{
			if (pReadBuffer[i] != pWriteBuffer[i])
			{
				MessageBox("数据校验失败！");
				break;
			}
		}

		if (i == m_iMaxBlockDatalen)
		{
			MessageBox("写入成功（已校验）");
		}

	} while (0);


	free(pReadBuffer);
	free(pWriteBuffer);
}

//全部写入0XFF并校验
void CBasicDlg::OnBnClickedButtonDataWrite0xff()
{
	// TODO: 在此添加控件通知处理程序代码
	UpdateData(TRUE);
	CString msg;
	BYTE *pReadBuffer;
	BYTE *pWriteBuffer;
	int i;

	pReadBuffer = (BYTE*)malloc(m_iMaxBlockDatalen);
	pWriteBuffer = (BYTE*)malloc(m_iMaxBlockDatalen);

	memset(pWriteBuffer,0xff,m_iMaxBlockDatalen);

	do 
	{
		if (CameraSaveUserData(m_hCamera,0,pWriteBuffer,m_iMaxBlockDatalen) != CAMERA_STATUS_SUCCESS)
		{
			MessageBox("写入数据出错");
			break;
		}



		if (CameraLoadUserData(m_hCamera,0,pReadBuffer,m_iMaxBlockDatalen) != CAMERA_STATUS_SUCCESS)
		{
			MessageBox("读取数据出错");
			break;
		}

		for (i = 0;i < m_iMaxBlockDatalen;i++)
		{
			if (pReadBuffer[i] != pWriteBuffer[i])
			{
				MessageBox("数据校验失败！");
				break;
			}
		}

		if (i == m_iMaxBlockDatalen)
		{
			MessageBox("写入成功（已校验）");
		}

	} while (0);


	free(pReadBuffer);
	free(pWriteBuffer);
}

//写入随机数后读取进行校验
void CBasicDlg::OnBnClickedButtonDataWriteRandom()
{
	// TODO: 在此添加控件通知处理程序代码
	UpdateData(TRUE);
	CString msg;
	BYTE *pReadBuffer;
	BYTE *pWriteBuffer;
	int i;

	pReadBuffer = (BYTE*)malloc(m_iMaxBlockDatalen);
	pWriteBuffer = (BYTE*)malloc(m_iMaxBlockDatalen);

	srand((unsigned) time(NULL)); 

	for (i = 0;i < m_iMaxBlockDatalen;i++)
	{
		pWriteBuffer[i] = rand()&0xff;
	}

	do 
	{
		if (CameraSaveUserData(m_hCamera,0,pWriteBuffer,m_iMaxBlockDatalen) != CAMERA_STATUS_SUCCESS)
		{
			MessageBox("写入数据出错");
			break;
		}



		if (CameraLoadUserData(m_hCamera,0,pReadBuffer,m_iMaxBlockDatalen) != CAMERA_STATUS_SUCCESS)
		{
			MessageBox("读取数据出错");
			break;
		}

		for (i = 0;i < m_iMaxBlockDatalen;i++)
		{
			if (pReadBuffer[i] != pWriteBuffer[i])
			{
				MessageBox("数据校验失败！");
				break;
			}
		}

		if (i == m_iMaxBlockDatalen)
		{
			MessageBox("写入成功（已校验）");
		}

	} while (0);


	free(pReadBuffer);
	free(pWriteBuffer);
}


void CBasicDlg::OnBnClickedButtonDataWriteInc()
{
	// TODO: 在此添加控件通知处理程序代码
	UpdateData(TRUE);
	CString msg;
	BYTE *pReadBuffer;
	BYTE *pWriteBuffer;
	int i;

	pReadBuffer = (BYTE*)malloc(m_iMaxBlockDatalen);
	pWriteBuffer = (BYTE*)malloc(m_iMaxBlockDatalen);

	for (i = 0;i < m_iMaxBlockDatalen;i++)
	{
		pWriteBuffer[i] = i&0xff;
	}


	do 
	{
		if (CameraSaveUserData(m_hCamera,0,pWriteBuffer,m_iMaxBlockDatalen) != CAMERA_STATUS_SUCCESS)
		{
			MessageBox("写入数据出错");
			break;
		}



		if (CameraLoadUserData(m_hCamera,0,pReadBuffer,m_iMaxBlockDatalen) != CAMERA_STATUS_SUCCESS)
		{
			MessageBox("读取数据出错");
			break;
		}

		for (i = 0;i < m_iMaxBlockDatalen;i++)
		{
			if (pReadBuffer[i] != pWriteBuffer[i])
			{
				MessageBox("数据校验失败！");
				break;
			}
		}

		if (i == m_iMaxBlockDatalen)
		{
			MessageBox("写入成功（已校验）");
		}

	} while (0);


	free(pReadBuffer);
	free(pWriteBuffer);
}


void CBasicDlg::OnBnClickedButtonDataRead()
{
	// TODO: 在此添加控件通知处理程序代码
	INT iStatus;
	iStatus = 	CameraLoadUserData(m_hCamera,0,(BYTE*)m_sBlockDataReadOut.GetBuffer(m_iMaxBlockDatalen),m_iMaxBlockDatalen);
	m_sBlockDataReadOut.ReleaseBuffer();

	if (iStatus != CAMERA_STATUS_SUCCESS)
	{
		MessageBox("读取失败");
		return;
	}

	UpdateData(FALSE);
}


void CBasicDlg::OnBnClickedButtonSnRead1()
{
	// TODO: 在此添加控件通知处理程序代码
	char sn[32];
	SDK_TRACE(CameraReadSN(m_hCamera,(BYTE*)sn,0),"读取一级序列号");
	m_sSerialNumber1 = sn;
	UpdateData(FALSE);
}


void CBasicDlg::OnBnClickedButtonSnWrite1()
{
	// TODO: 在此添加控件通知处理程序代码
	char sn[32];
	UpdateData(TRUE);
	memcpy(sn,m_sSerialNumber1.GetBuffer(1),31);
	m_sSerialNumber1.ReleaseBuffer();
	sn[31] = 0 ;

	

}
