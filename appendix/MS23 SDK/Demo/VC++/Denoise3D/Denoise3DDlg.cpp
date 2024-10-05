
// Denoise3DDlg.cpp : 实现文件
//

#include "stdafx.h"
#include "Denoise3D.h"
#include "Denoise3DDlg.h"
#include "afxdialogex.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


// CDenoise3DDlg 对话框




CDenoise3DDlg::CDenoise3DDlg(CWnd* pParent /*=NULL*/)
	: CDialogEx(CDenoise3DDlg::IDD, pParent)
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
}

void CDenoise3DDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CDenoise3DDlg, CDialogEx)
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_WM_DESTROY()
	ON_WM_TIMER()
	ON_BN_CLICKED(IDC_BUTTON_IMG_CAP1, &CDenoise3DDlg::OnBnClickedButtonImgCap1)
	ON_BN_CLICKED(IDC_BUTTON_IMG_CAP2, &CDenoise3DDlg::OnBnClickedButtonImgCap2)
	ON_BN_CLICKED(IDC_BUTTON_IMG_CAP3, &CDenoise3DDlg::OnBnClickedButtonImgCap3)
	ON_BN_CLICKED(IDC_BUTTON_IMG_SAVE1, &CDenoise3DDlg::OnBnClickedButtonImgSave1)
	ON_BN_CLICKED(IDC_BUTTON_IMG_SAVE2, &CDenoise3DDlg::OnBnClickedButtonImgSave2)
	ON_BN_CLICKED(IDC_BUTTON_IMG_SAVE3, &CDenoise3DDlg::OnBnClickedButtonImgSave3)
	ON_BN_CLICKED(IDC_BUTTON_IMG_SAVE4, &CDenoise3DDlg::OnBnClickedButtonImgSave4)
	ON_BN_CLICKED(IDC_BUTTON_SETTINGS, &CDenoise3DDlg::OnBnClickedButtonSettings)
END_MESSAGE_MAP()


// CDenoise3DDlg 消息处理程序

BOOL CDenoise3DDlg::OnInitDialog()
{
	CDialogEx::OnInitDialog();

	// 设置此对话框的图标。当应用程序主窗口不是对话框时，框架将自动
	//  执行此操作
	SetIcon(m_hIcon, TRUE);			// 设置大图标
	SetIcon(m_hIcon, FALSE);		// 设置小图标

	// TODO: 在此添加额外的初始化代码
	CheckDlgButton(IDC_RADIO_AVG, BST_CHECKED);

	CameraSdkInit(1);

	m_Grabber = NULL;
	if (CameraGrabber_CreateFromDevicePage(&m_Grabber) == CAMERA_STATUS_SUCCESS)
	{
		SetTimer(1, 1000, NULL);

		CameraGrabber_GetCameraHandle(m_Grabber, &m_hCamera);
		CameraGrabber_GetCameraDevInfo(m_Grabber, &m_DevInfo);

		CameraCreateSettingPage(m_hCamera, m_hWnd, m_DevInfo.acFriendlyName, NULL, NULL, 0);

		CameraGrabber_SetHWnd(m_Grabber, GetDlgItem(IDC_STATIC_DISP)->m_hWnd);
		CameraGrabber_StartLive(m_Grabber);
	}
	else
	{
		EndDialog(0);
	}

	return TRUE;  // 除非将焦点设置到控件，否则返回 TRUE
}

void CDenoise3DDlg::OnDestroy()
{
	CDialogEx::OnDestroy();

	// TODO: 在此处添加消息处理程序代码
	CameraGrabber_Destroy(m_Grabber);
	m_Grabber = NULL;
}

// 如果向对话框添加最小化按钮，则需要下面的代码
//  来绘制该图标。对于使用文档/视图模型的 MFC 应用程序，
//  这将由框架自动完成。

void CDenoise3DDlg::OnPaint()
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
HCURSOR CDenoise3DDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}

void CDenoise3DDlg::OnTimer(UINT_PTR nIDEvent)
{
	tSdkGrabberStat Stat;
	CameraGrabber_GetStat(m_Grabber, &Stat);

	CString strText;
	strText.Format("宽:%u 高:%u 显示帧率:%.02f 捕获帧率:%.02f", Stat.Width, Stat.Height, Stat.DispFps, Stat.CapFps);
	SetDlgItemText(IDC_STATIC_STAT, strText);

	CDialogEx::OnTimer(nIDEvent);
}

void CDenoise3DDlg::DispImg(int index)
{
	void *RGBImage = NULL;
	if (CameraImage_Create(&RGBImage, m_Imgs[index].RgbData, &m_Imgs[index].RgbHead, FALSE) == CAMERA_STATUS_SUCCESS)
	{
		CameraImage_Draw(RGBImage, GetDlgItem(IDC_STATIC_IMG1 + index)->GetSafeHwnd(), 1);
	}
	CameraImage_Destroy(RGBImage);
}

void CDenoise3DDlg::DoImgCap(int index)
{
	tSdkFrameHead FrameInfo;
	BYTE *pRawBuffer;
	CameraSdkStatus status;

	// 分辨率与预览模式相同
	tSdkImageResolution sImageSize;
	memset(&sImageSize,0,sizeof(tSdkImageResolution));
	sImageSize.iIndex = 0xff;
	CameraSetResolutionForSnap(m_hCamera, &sImageSize);

	//	CameraSnapToBuffer抓拍一帧图像数据到缓冲区中，该缓冲区由SDK内部申请,成功调用后，需要
	// !!!!!!注意：CameraSnapToBuffer 会切换分辨率拍照，速度较慢。做实时处理，请用CameraGetImageBuffer函数取图或者回调函数。
	if((status = CameraSnapToBuffer(m_hCamera, &FrameInfo, &pRawBuffer, 1000)) != CAMERA_STATUS_SUCCESS)
	{
		MessageBox("Snapshot failed,is camera in pause mode?");
		return;
	}
	else
	{
		m_Imgs[index].RawHead = FrameInfo;
		CameraAlignFree(m_Imgs[index].RawData);
		m_Imgs[index].RawData = CameraAlignMalloc(FrameInfo.uBytes, 16);
		memcpy(m_Imgs[index].RawData, pRawBuffer, FrameInfo.uBytes);

		CameraAlignFree(m_Imgs[index].RgbData);
		m_Imgs[index].RgbData = CameraAlignMalloc(FrameInfo.iWidth * FrameInfo.iHeight * 3, 16);
		CameraImageProcess(m_hCamera, pRawBuffer, m_Imgs[index].RgbData, &FrameInfo);
		m_Imgs[index].RgbHead = FrameInfo;

		DispImg(index);

		if (DoCompose() )
		{
			DispImg(3);
		}

		CameraReleaseImageBuffer(m_hCamera, pRawBuffer);
	}
}

BOOL CDenoise3DDlg::DoCompose()
{
	CameraSdkStatus status;
	CString msg;
	tSdkFrameHead ImgHeads[4];
	BYTE* ImgDatas[4];

	for (int i = 0; i < 3; ++i)
	{
		if (m_Imgs[i].RawData == NULL)
			return FALSE;
		ImgHeads[i] = m_Imgs[i].RawHead;
		ImgDatas[i] = m_Imgs[i].RawData;
	}

	m_Imgs[3].RawHead = m_Imgs[2].RawHead;
	CameraAlignFree(m_Imgs[3].RawData);
	m_Imgs[3].RawData = CameraAlignMalloc(m_Imgs[3].RawHead.uBytes, 16);

	float Weights[3];
	if (GetCheckedRadioButton(IDC_RADIO_AVG, IDC_RADIO_SUM) == IDC_RADIO_AVG)
	{
		Weights[0] = Weights[1] = Weights[2] = 1.0f / ARRAYSIZE(Weights);
	}
	else
	{
		Weights[0] = Weights[1] = Weights[2] = 1.0f;
	}

	status = CameraManualDenoise3D(ImgHeads, ImgDatas, 3, Weights, &m_Imgs[3].RawHead, m_Imgs[3].RawData);
	if (status == CAMERA_STATUS_SUCCESS)
	{
		m_Imgs[3].RgbHead = m_Imgs[3].RawHead;
		CameraAlignFree(m_Imgs[3].RgbData);
		m_Imgs[3].RgbData = CameraAlignMalloc(m_Imgs[3].RgbHead.iWidth * m_Imgs[3].RgbHead.iHeight * 3, 16);
		CameraImageProcess(m_hCamera, m_Imgs[3].RawData, m_Imgs[3].RgbData, &m_Imgs[3].RgbHead);
		return TRUE;
	}
	else
	{
		msg.Format("合成失败, err:%d", status);
		MessageBox(msg);
		return FALSE;
	}
}

void CDenoise3DDlg::OnBnClickedButtonImgCap1()
{
	DoImgCap(0);
}

void CDenoise3DDlg::OnBnClickedButtonImgCap2()
{
	DoImgCap(1);
}

void CDenoise3DDlg::OnBnClickedButtonImgCap3()
{
	DoImgCap(2);
}

void CDenoise3DDlg::SaveImg(int index)
{
	CString msg;
	CameraSdkStatus status;

	if (m_Imgs[index].RgbData == NULL)
	{
		msg.Format("错误：图片%d未捕获", index + 1);
		MessageBox(msg);
		return;
	}

	CFileDialog Dlg(FALSE, "bmp", NULL, OFN_HIDEREADONLY | OFN_OVERWRITEPROMPT, 
		"图片 (*.bmp)|*.bmp||");
	if (Dlg.DoModal() == IDOK)
	{
		tSdkFrameHead FrameInfo = m_Imgs[index].RgbHead;
		BYTE* pRgbBuffer = m_Imgs[index].RgbData;

		void *RGBImage = NULL;
		if (CameraImage_Create(&RGBImage, pRgbBuffer, &FrameInfo, FALSE) == CAMERA_STATUS_SUCCESS)
		{
			if ((status = CameraImage_SaveAsBmp(RGBImage, Dlg.GetPathName())) == CAMERA_STATUS_SUCCESS)
			{
				msg.Format("'%s'已保存", (LPCSTR)Dlg.GetPathName());
			}
			else
			{
				msg.Format("'%s'保存失败，err: %d", (LPCSTR)Dlg.GetPathName(), status);
			}
			MessageBox(msg);
		}
		CameraImage_Destroy(RGBImage);
	}
}

void CDenoise3DDlg::OnBnClickedButtonImgSave1()
{
	SaveImg(0);
}

void CDenoise3DDlg::OnBnClickedButtonImgSave2()
{
	SaveImg(1);
}

void CDenoise3DDlg::OnBnClickedButtonImgSave3()
{
	SaveImg(2);
}

void CDenoise3DDlg::OnBnClickedButtonImgSave4()
{
	SaveImg(3);
}

void CDenoise3DDlg::OnBnClickedButtonSettings()
{
	CameraShowSettingPage(m_hCamera, TRUE);
}
