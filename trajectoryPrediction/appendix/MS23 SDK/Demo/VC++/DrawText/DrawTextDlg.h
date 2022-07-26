
// DrawTextDlg.h : 头文件
//

#pragma once

#include "CameraApi.h"
#ifdef _WIN64
#pragma comment(lib, ".\\MVCAMSDK_X64.lib")
#else
#pragma comment(lib, ".\\MVCAMSDK.lib")
#endif

struct TextItem
{
	CString strFontFileName;
	UINT uFontW, uFontH;
	CString strText;
	CRect rcOut;
	UINT uColor;
	UINT uFlags;
};


// CDrawTextDlg 对话框
class CDrawTextDlg : public CDialogEx
{
// 构造
public:
	CDrawTextDlg(CWnd* pParent = NULL);	// 标准构造函数

// 对话框数据
	enum { IDD = IDD_DRAWTEXT_DIALOG };

public:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV 支持

	CWnd*			m_pDisp;
	CameraHandle    m_hCamera;	//相机的设备句柄|the handle of the camera we use
	tSdkFrameHead   m_sFrInfo;//用于保存当前图像帧的帧头信息

	int	            m_iDispFrameNum;//用于记录当前已经显示的图像帧的数量
	float           m_fDispFps;//显示帧率
	float           m_fCapFps;//捕获帧率
	tSdkFrameStatistic  m_sFrameCount;
	tSdkFrameStatistic  m_sFrameLast;
	int		        m_iTimeLast;

	BYTE*           m_pFrameBuffer;//用于将原始图像数据转换为RGB的缓冲区

	UINT            m_threadID;//图像抓取线程的ID
	HANDLE          m_hDispThread;//图像抓取线程的句柄
	BOOL            m_bExit;//用来通知图像抓取线程结束

	CDialog*		m_pTextInfoDlg;

	CCriticalSection m_csTextItemArray;
	CArray<TextItem> m_TextItemArray;

	BOOL InitCamera();
	static UINT WINAPI uiDisplayThread(LPVOID lpParam);

// 实现
protected:
	HICON m_hIcon;

	// 生成的消息映射函数
	virtual BOOL OnInitDialog();
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnTimer(UINT_PTR nIDEvent);
	afx_msg void OnDestroy();
};
