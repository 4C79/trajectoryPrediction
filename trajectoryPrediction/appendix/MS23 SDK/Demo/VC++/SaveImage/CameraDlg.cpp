// CameraDlg.cpp : 实现文件
//

#include "stdafx.h"
#include "SaveImage.h"
#include "CameraDlg.h"
#include "afxdialogex.h"
#include "AsyncSaveImage.h"


// CCameraDlg 对话框

IMPLEMENT_DYNAMIC(CCameraDlg, CDialogEx)

CCameraDlg::CCameraDlg(char const* SavePrefix, CWnd* pParent /*=NULL*/)
	: CDialogEx(CCameraDlg::IDD, pParent)
{
	m_Grabber = NULL;
	m_hCamera = 0;
	m_bMono = FALSE;
	m_strSavePrefix = SavePrefix;
	m_SaveNum = 0;
	m_SaveFmt = 0;
	m_bAutoSave = FALSE;
	m_bOnlySaveTrigger = FALSE;
	m_SaveCount = -1;
	m_bOverlayDate = FALSE;
	m_OverlayItem.strFont = "C:\\Windows\\Fonts\\simsun.ttc";
	m_OverlayItem.FontWidth = 96;
	m_OverlayItem.FontHeight = 96;
	m_OverlayItem.TextColor = RGB(255, 255, 255);
	m_OverlayItem.x = m_OverlayItem.y = 0;
}

CCameraDlg::~CCameraDlg()
{
}

void CCameraDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
}


BEGIN_MESSAGE_MAP(CCameraDlg, CDialogEx)
	ON_WM_DESTROY()
	ON_WM_TIMER()
	ON_BN_CLICKED(IDC_BUTTON_SETTING, &CCameraDlg::OnBnClickedButtonSetting)
	ON_WM_SIZE()
END_MESSAGE_MAP()


// CCameraDlg 消息处理程序

BOOL CCameraDlg::OnInitDialog()
{
	CDialogEx::OnInitDialog();

	

	return TRUE;  // return TRUE unless you set the focus to a control
	// 异常: OCX 属性页应返回 FALSE
}

void CCameraDlg::OnDestroy()
{
	CDialogEx::OnDestroy();

	// TODO: 在此处添加消息处理程序代码
}

int CCameraDlg::Open(int index)
{
	int status = CameraGrabber_CreateByIndex(&m_Grabber, index);
	if (status != CAMERA_STATUS_SUCCESS)
	{
		return status;
	}

	CameraGrabber_GetCameraDevInfo(m_Grabber, &m_DevInfo);
	CameraGrabber_GetCameraHandle(m_Grabber, &m_hCamera);

	CameraCreateSettingPage(m_hCamera, m_hWnd, m_DevInfo.acFriendlyName, NULL, NULL, 0);
	CameraGrabber_SetFrameListener(m_Grabber, FrameListenerEntry, this);

	// 初始化显示
	CWnd* pDisp = GetDlgItem(IDC_DISP);
	CRect rc;
	pDisp->GetClientRect(rc);
	CameraDisplayInit(m_hCamera, pDisp->m_hWnd);
	CameraSetDisplaySize(m_hCamera, rc.Width(), rc.Height());

	// 黑白相机设置ISP输出灰度图像
	// 彩色相机ISP默认会输出BGR24图像
	tSdkCameraCapbility cap;
	CameraGetCapability(m_hCamera, &cap);
	m_bMono = cap.sIspCapacity.bMonoSensor;
	if (m_bMono)
		CameraSetIspOutFormat(m_hCamera, CAMERA_MEDIA_TYPE_MONO8);

	SetTimer(1, 1000, NULL);
	return CAMERA_STATUS_SUCCESS;
}

void CCameraDlg::Close()
{
	KillTimer(1);
	CameraGrabber_Destroy(m_Grabber);
	m_Grabber = NULL;
	m_hCamera = 0;
}

int CCameraDlg::Start()
{
	return CameraGrabber_StartLive(m_Grabber);
}

void CCameraDlg::Stop()
{
	CameraGrabber_StopLive(m_Grabber);
}

int CCameraDlg::SetTriggerMode(int mode)
{
	int status = CameraSetTriggerMode(m_hCamera, mode);
	return status;
}

int CCameraDlg::SetTriggerCount(int count)
{
	int status = CameraSetTriggerCount(m_hCamera, count);
	return status;
}

int CCameraDlg::SoftTrigger()
{
	int status = CameraSoftTrigger(m_hCamera);
	return status;
}

int CCameraDlg::FrameRAWCallback(void* Grabber, BYTE *pFrameBuffer, tSdkFrameHead* pFrameHead)
{
	if (!m_bAutoSave)
		return 1;
	if (m_bOnlySaveTrigger && !pFrameHead->bIsTrigger)
		return 1;
	if (m_SaveCount == 0)
		return 1;

	CString strSaveDir;
	int SaveFmt;
	{
		CSingleLock Lock(&m_DataLock, TRUE);
		strSaveDir = m_strSaveDir;
		SaveFmt = m_SaveFmt;
	}
	if (strSaveDir.IsEmpty())
		return 1;
	
	CString strFileName;
	strFileName.Format("%s\\%s%s_%06d",
		strSaveDir,
		m_strSavePrefix,
		CTime::GetCurrentTime().Format(_T("%Y%m%d%H%M%S")),
		++m_SaveNum
		);

	switch (SaveFmt)
	{
	case 1:
		SaveFmt = FILE_JPG;
		break;
	case 2:
		SaveFmt = FILE_PNG;
		break;
	default:
		SaveFmt = (m_bMono ? FILE_BMP_8BIT : FILE_BMP);
		break;
	}

	CAsyncSaveImage::DrawTextItemArray DrawTexts;
	if (m_bOverlayDate)
	{
		m_OverlayItem.strText = CTime::GetCurrentTime().Format(_T("%Y-%m-%d %H:%M:%S"));
		DrawTexts.push_back(m_OverlayItem);
	}

	if (CAsyncSaveImage::GetInstance()->SaveImage(m_hCamera, strFileName, 
		pFrameBuffer, pFrameHead, 
		SaveFmt, 80,
		DrawTexts) )
	{
		CSingleLock Lock(&m_DataLock, TRUE);
		if (m_SaveCount > 0)
			--m_SaveCount;
	}
	return 0;
}

void CCameraDlg::OnTimer(UINT_PTR nIDEvent)
{
	if (m_Grabber != NULL)
	{
		tSdkGrabberStat Stat;
		CameraGrabber_GetStat(m_Grabber, &Stat);

		CString strSaveCount;
		int SaveCount = m_SaveCount;
		if (SaveCount > 0)
			strSaveCount.Format("%d帧", SaveCount);
		else if (SaveCount == 0)
			strSaveCount = "已停止";
		else
			strSaveCount = "无限制";

		CString strText;
		strText.Format("宽:%u 高:%u 帧数:%d 显示:%.02fFPS 捕获:%.02fFPS 保存:%s",
			Stat.Width, Stat.Height, Stat.Capture,
			Stat.DispFps, Stat.CapFps, strSaveCount);
		SetDlgItemText(IDC_STAT, strText);
	}

	CDialogEx::OnTimer(nIDEvent);
}

void CCameraDlg::OnBnClickedButtonSetting()
{
	if (m_hCamera > 0)
	{
		CameraShowSettingPage(m_hCamera, TRUE);
	}
}

void CCameraDlg::OnSize(UINT nType, int cx, int cy)
{
	CDialogEx::OnSize(nType, cx, cy);

	CRect rc(0, 0, cx, cy);
	rc.top = 50;
	CWnd* pDisp = GetDlgItem(IDC_DISP);
	if (pDisp != NULL)
	{
		pDisp->MoveWindow(rc);
	}
}

void CCameraDlg::SetSaveDir(char const* SaveDir)
{
	CSingleLock Lock(&m_DataLock, TRUE);
	m_strSaveDir = SaveDir;
}

void CCameraDlg::SetSaveFormat(int Fmt)
{
	m_SaveFmt = Fmt;
}

void CCameraDlg::SetAutoSave(BOOL bEnable)
{
	m_bAutoSave = bEnable;
}

void CCameraDlg::SetOnlySaveTrigger(BOOL bEnable)
{
	m_bOnlySaveTrigger = bEnable;
}

void CCameraDlg::SetSaveCount(int count)
{
	CSingleLock Lock(&m_DataLock, TRUE);
	m_SaveCount = count;
}

int CCameraDlg::GetSaveCount() const
{
	return m_SaveCount;
}

void CCameraDlg::SetOverlayDate(BOOL bEnable)
{
	m_bOverlayDate = bEnable;
}
