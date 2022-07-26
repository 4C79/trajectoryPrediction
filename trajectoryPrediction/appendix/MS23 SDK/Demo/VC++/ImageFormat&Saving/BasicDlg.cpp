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
, m_iOutputFormat(1)
, m_iSaveFormat(0)
, m_sFileSavePath(_T("C:\\Camera Capture Image\\CaptureImage"))
{
	//{{AFX_DATA_INIT(CBasicDlg)
		// NOTE: the ClassWizard will add member initialization here
	//}}AFX_DATA_INIT
	// Note that LoadIcon does not require a subsequent DestroyIcon in Win32
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
	m_bPause = TRUE;
	m_hCamera = -1;
	m_iDispFrameNum = 0;
	m_bExit = FALSE;
	m_hDispThread = NULL;
	m_pFrameBuffer = NULL;

}

void CBasicDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CBasicDlg)
	//}}AFX_DATA_MAP
	DDX_Radio(pDX, IDC_RADIO6, m_iOutputFormat);
	DDX_Radio(pDX, IDC_RADIO1, m_iSaveFormat);
	DDX_Text(pDX, IDC_EDIT1, m_sFileSavePath);
}

BEGIN_MESSAGE_MAP(CBasicDlg, CDialog)
	//{{AFX_MSG_MAP(CBasicDlg)
	ON_WM_SYSCOMMAND()
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_BN_CLICKED(IDC_BUTTON_CAMERA_SETTINGS, OnButtonCameraSettings)
	ON_WM_CLOSE()
	//}}AFX_MSG_MAP
	ON_BN_CLICKED(IDC_BUTTON_SAVE, OnBnClickedButtonSave)
	ON_BN_CLICKED(IDC_RADIO6, OnBnClickedRadioOutPutFormat)
	ON_BN_CLICKED(IDC_RADIO7, OnBnClickedRadioOutPutFormat)
	ON_BN_CLICKED(IDC_RADIO8, OnBnClickedRadioOutPutFormat)
	ON_BN_CLICKED(IDC_RADIO1, OnBnClickedRadioSaveFormat)
	ON_BN_CLICKED(IDC_RADIO2, OnBnClickedRadioSaveFormat)
	ON_BN_CLICKED(IDC_RADIO4, OnBnClickedRadioSaveFormat)
	ON_BN_CLICKED(IDC_RADIO5, OnBnClickedRadioSaveFormat)
	ON_BN_CLICKED(IDC_RADIO3, OnBnClickedRadioSaveFormat)
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


void CBasicDlg::OnButtonCameraSettings() 
{
	
	//Show the settings window。
	SDK_TRACE(CameraShowSettingPage(m_hCamera,TRUE),gLanguage?"显示相机的属性配置窗口，该窗口由SDK内部生成":"show the camera config page");
}


BOOL CBasicDlg::InitCamera()
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
	SDK_TRACE(CameraGetCapability(m_hCamera,&sCameraInfo),gLanguage?"获得该相机的特性描述":"Get the capability of the camera");

	m_pFrameBuffer = (BYTE *)CameraAlignMalloc(sCameraInfo.sResolutionRange.iWidthMax*sCameraInfo.sResolutionRange.iHeightMax*4,16);	//如果ISP输出是RGBA格式，则一个像素占4个字节，所以申请*4大小的内存

	ASSERT(m_pFrameBuffer);
	
	if (sCameraInfo.sIspCapacity.bMonoSensor == TRUE)
	{
		//黑白相机，设置图像保存格式为8bit灰度bmp位图，SDK输出格式为8bit
		m_iSaveFormat = 2;
		m_iOutputFormat = 0;
		SDK_TRACE(CameraSetIspOutFormat(m_hCamera,CAMERA_MEDIA_TYPE_MONO8),gLanguage?"黑白相机设置为8位灰度格式输出":"Set mono8bit as isp format");
	}
	else
	{
		//彩色相机，设置图像保存格式为24位灰度bmp位图，SDK输出格式为24的RGB数据,如果是使用opencv、labview等也可以设置为32位的彩色格式，图像数据按4字节对齐，方便使用硬件加速。
		m_iSaveFormat = 1;
		m_iOutputFormat = 1;
		SDK_TRACE(CameraSetIspOutFormat(m_hCamera,CAMERA_MEDIA_TYPE_RGB8),gLanguage?"彩色相机设置为RGB24格式输出":"Set RGB24 as isp format");
	}

	//通知SDK内部建该相机的属性页面。窗口名称为该相机的名称。您也可以根据SDK的接口来自己实现相机的属性配置页面，
	//但是我们建议您使用SDK内部自动创建的方式，来省去您在界面开发上的大量时间。
	//Create the settings window for the camera
	SDK_TRACE(CameraCreateSettingPage(m_hCamera,GetSafeHwnd(),
								sCameraList[0].acFriendlyName,NULL,(PVOID)this,0)
								,gLanguage?"通知SDK内部建该相机的属性页面":"Create camera config page");
 
	
	//Tell the camera begin to sendding image
	SDK_TRACE(CameraPlay(m_hCamera),gLanguage?"开始预览":"Start preview");

	
	return TRUE;
}


void CBasicDlg::OnClose() 
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
	CDialog::OnClose();
}

//Capture and save image to a file
void CBasicDlg::OnBnClickedButtonSave()
{
	// TODO: 在此添加控件通知处理程序代码
	
	int status;
	tSdkFrameHead tImageHead;
	BYTE *pbyBuffer;
	CString sFileName;
	UpdateData(TRUE);

	//获得一帧原始图像数据
	if(CameraGetImageBuffer(m_hCamera,&tImageHead,&pbyBuffer,1000) == CAMERA_STATUS_SUCCESS)
	{	
		//要保存RAW数据，无需调用CameraImageProcess，RAW数据可以用photoshop打开。
		if (m_iSaveFormat == 0)
		{
			sFileName = m_sFileSavePath + ".raw";
			CameraSaveImage(m_hCamera,sFileName.GetBuffer(1),pbyBuffer,&tImageHead,FILE_RAW,100);
		}
		else
		{
			//调用CameraImageProcess进行图像处理，最后输出的图像数据格式，由CameraSetIspOutFormat决定。处理后的格式，会在tImageHead.uiMediaType中表明
			status = CameraImageProcess(m_hCamera, pbyBuffer, m_pFrameBuffer,&tImageHead);//连续模式

			ASSERT(status == CAMERA_STATUS_SUCCESS);

			//叠加十字线、AE、AWB窗口，不需要则可忽略这一行
			CameraImageOverlay(m_hCamera, m_pFrameBuffer, &tImageHead);


			//根据界面上选择的格式，保存图像
			switch(m_iSaveFormat)
			{
			case 1:
			default:
				sFileName = m_sFileSavePath + "(24bit).bmp";
				CameraSaveImage(m_hCamera,sFileName.GetBuffer(1),m_pFrameBuffer,&tImageHead,FILE_BMP,100);
				break;

			case 2:
				//如果需要保存成8位bmp的图像，请先调用好CameraSetIspOutFormat(m_hCamera,CAMERA_MEDIA_TYPE_MONO8);
				sFileName = m_sFileSavePath + "(8bit).bmp";
				CameraSaveImage(m_hCamera,sFileName.GetBuffer(1),m_pFrameBuffer,&tImageHead,FILE_BMP_8BIT,100);
				break;

			case 3:
				sFileName = m_sFileSavePath + ".png";
				CameraSaveImage(m_hCamera,sFileName.GetBuffer(1),m_pFrameBuffer,&tImageHead,FILE_PNG,100);
				break;

			case 4:
				sFileName = m_sFileSavePath + ".jpg";
				CameraSaveImage(m_hCamera,sFileName.GetBuffer(1),m_pFrameBuffer,&tImageHead,FILE_JPG,100);
				break;
			}
		}
		

		//在成功调用CameraGetImageBuffer后，必须调用CameraReleaseImageBuffer来释放获得的buffer。
		//否则再次调用CameraGetImageBuffer时，程序将被挂起阻塞，直到其它线程中调用CameraReleaseImageBuffer来释放了buffer
		CameraReleaseImageBuffer(m_hCamera,pbyBuffer);

	}
	else
	{
		MessageBox("抓图超时，是否设定在触发模式下？");
	}
}


//设置SDK的ISP处理后的图像输出格式
void CBasicDlg::OnBnClickedRadioOutPutFormat()
{
	// TODO: 在此添加控件通知处理程序代码
	UpdateData(TRUE);

	switch(m_iOutputFormat)
	{
		//设置为8bit灰度格式，黑白相机可设置为8bit的灰度图像输出，以节约内存存储空间，同时可提高图像算法处理的速度。例如halcon和labview以及opencv都支持8bit的灰度位图直接处理。
	case 0:
		CameraSetIspOutFormat(m_hCamera,CAMERA_MEDIA_TYPE_MONO8);
		break;

		//设置为24bit位图格式，相机默认输出该格式，适合彩色相机。
	case 1:
	default:
		CameraSetIspOutFormat(m_hCamera,CAMERA_MEDIA_TYPE_RGB8);
		break;

		//设置为32bit位图格式,opencv、labview、halcon等图像处理软件，有可能需要32位深度的彩色图像格式，一个像素按4个字节对齐，方便调用SSE算法进行加速
	case 2:
		CameraSetIspOutFormat(m_hCamera,CAMERA_MEDIA_TYPE_RGBA8);
		break;

	}

}



void CBasicDlg::OnBnClickedRadioSaveFormat()
{
	// TODO: 在此添加控件通知处理程序代码
	UpdateData(TRUE);
	//如果是需要保存BMP 8位深度的图像，则需要将SDK的ISP格式输出切换8bit，这样在保存图像时，就无需多余的转换操作。
	if (m_iSaveFormat == 2)
	{
		m_iOutputFormat = 0;
		CameraSetIspOutFormat(m_hCamera,CAMERA_MEDIA_TYPE_MONO8);
	}
	else if (m_iSaveFormat == 1 || m_iSaveFormat == 3 || m_iSaveFormat == 4)//如果是保存24位位深的图片，则切换SDK 的ISP输出格式为24bit
	{
		m_iOutputFormat = 1;
		CameraSetIspOutFormat(m_hCamera,CAMERA_MEDIA_TYPE_RGB8);
	}

	UpdateData(FALSE);
}

