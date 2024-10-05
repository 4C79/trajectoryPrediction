// BasicDlg.h : header file
//
//BIG5 TRANS ALLOWED
#if !defined(AFX_BASICDLG_H__DE07E1D0_D0B7_4FA5_A4F3_45499366E00E__INCLUDED_)
#define AFX_BASICDLG_H__DE07E1D0_D0B7_4FA5_A4F3_45499366E00E__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "SdkCallTrace.h"



/*输出例程中调用相机的SDK接口日志信息*/
#define SDK_TRACE(_FUNC_,TXT) \
{\
	CameraSdkStatus status;\
	CString msg;\
	CString FuncName;\
	FuncName = #_FUNC_;\
	FuncName = FuncName.Left(FuncName.FindOneOf("("));\
\
	status = _FUNC_;\
	if (status != CAMERA_STATUS_SUCCESS)\
	{\
	msg.Format(gLanguage?"函数:[%s] 调用失败!":"Function:[%s] return error",FuncName);\
	m_DlgLog.AppendLog(msg);\
	msg.Format(gLanguage?"错误码:%d. 请参考CameraStatus.h中错误码的详细定义":"Error code:%d.refer to CameraStatus.h for more information",status);\
	m_DlgLog.AppendLog(msg);\
	}\
	else\
	{\
	msg.Format(gLanguage?"函数:[%s] 调用成功!":"Function:[%s] success",FuncName);\
	m_DlgLog.AppendLog(msg);\
	msg.Format(gLanguage?"功能:%s.":"Action:%s",TXT);\
	m_DlgLog.AppendLog(msg);\
	}\
	msg = "";\
	m_DlgLog.AppendLog(msg);\
}


/////////////////////////////////////////////////////////////////////////////
// CBasicDlg dialog

class CBasicDlg : public CDialog
{
// Construction
public:
	CBasicDlg(CWnd* pParent = NULL);	// standard constructor
	CSdkCallTrace m_DlgLog;
// Dialog Data
	//{{AFX_DATA(CBasicDlg)
	enum { IDD = IDD_BASIC_DIALOG_CN };
		// NOTE: the ClassWizard will add data members here
	//}}AFX_DATA

	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CBasicDlg)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation

protected:
	HICON m_hIcon;
	
	// Generated message map functions
	//{{AFX_MSG(CBaseDlg)
	virtual BOOL OnInitDialog();
	afx_msg void OnSysCommand(UINT nID, LPARAM lParam);
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	afx_msg void OnButtonPreview();
	afx_msg void OnButtonCameraSettings();

	afx_msg void OnButtonAbout();
	afx_msg void OnClose();
	afx_msg void OnTimer(UINT_PTR nIDEvent);
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
		
public:
	CStatic	        m_cPreview;//用于显示相机图像的窗口控件|the control used to display the images.
	CameraHandle    m_hCamera;	//相机的设备句柄|the handle of the camera we use
	tSdkFrameHead   m_sFrInfo;//用于保存当前图像帧的帧头信息

	int	            m_iDispFrameNum;//用于记录当前已经显示的图像帧的数量
	float           m_fDispFps;//显示帧率
	float           m_fCapFps;//捕获帧率
    tSdkFrameStatistic  m_sFrameCount;
    tSdkFrameStatistic  m_sFrameLast;
    int		        m_iTimeLast;

	BYTE*           m_pFrameBuffer;		//用于将原始图像数据转换为RGB的缓冲区
	BYTE*           m_pDisplayBuffer;	//2副图像拼接的缓冲区
	BOOL	        m_bPause;//是否暂停图像

    UINT            m_threadID;//图像抓取线程的ID
	HANDLE          m_hDispThread;//图像抓取线程的句柄
	BOOL            m_bExit;//用来通知图像抓取线程结束
	BOOL InitCamera();
	double m_dExp1;
	double m_dExp2;
	BOOL m_bMergeImage;
	afx_msg void OnBnClickedCheckMerge();
	afx_msg void OnEnChangeEditLongExp();
	afx_msg void OnEnChangeEditShortExp();
	UINT m_uGain1;
	UINT m_uGain2;
	CString m_sGainRange;
	afx_msg void OnEnChangeEditLongGain1();
	afx_msg void OnEnChangeEditShortGain2();
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_BASICDLG_H__DE07E1D0_D0B7_4FA5_A4F3_45499366E00E__INCLUDED_)
