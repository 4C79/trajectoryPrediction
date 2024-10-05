// BasicDlg.h : header file
//
//BIG5 TRANS ALLOWED

#if !defined(AFX_BASICDLG_H__DE07E1D0_D0B7_4FA5_A4F3_45499366E00E__INCLUDED_)
#define AFX_BASICDLG_H__DE07E1D0_D0B7_4FA5_A4F3_45499366E00E__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "SdkCallTrace.h"

#include "CameraApi.h"	 
#include "CameraGrabber.h"
#ifdef _WIN64
#pragma comment(lib, "..\\MVCAMSDK_X64.lib")
#else
#pragma comment(lib, "..\\MVCAMSDK.lib")
#endif
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
	afx_msg void OnClose();
	afx_msg void OnTimer(UINT_PTR nIDEvent);
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
		
public:

	void*			m_Grabber;
	CameraHandle    m_hCamera;	//相机的设备句柄|the handle of the camera we use
	int				m_iInputIoCounts;
	BOOL InitCamera();
	BOOL m_bHighIN1;
	BOOL m_bHighIN2;
	BOOL m_bHighIN3;
	BOOL m_bHighIN4;
	BOOL m_bHighIN5;
	BOOL m_bHighIN6;
	BOOL m_bHighIN7;
	BOOL m_bHighIN8;
	BOOL m_bHighIN9;
	BOOL m_bHighIN10;
	BOOL m_bHighOUT1;
	BOOL m_bHighOUT2;
	BOOL m_bHighOUT3;
	BOOL m_bHighOUT4;
	BOOL m_bHighOUT5;
	BOOL m_bHighOUT6;
	BOOL m_bHighOUT7;
	BOOL m_bHighOUT8;
	BOOL m_bHighOUT9;
	BOOL m_bHighOUT10;


	afx_msg void OnBnClickedOUT1();
	afx_msg void OnBnClickedOUT2();
	afx_msg void OnBnClickedOUT3();
	afx_msg void OnBnClickedOUT4();
	afx_msg void OnBnClickedOUT5();
	afx_msg void OnBnClickedOUT6();
	afx_msg void OnBnClickedOUT7();
	afx_msg void OnBnClickedOUT8();
	afx_msg void OnBnClickedOUT9();
	afx_msg void OnBnClickedOUT10();

};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_BASICDLG_H__DE07E1D0_D0B7_4FA5_A4F3_45499366E00E__INCLUDED_)
