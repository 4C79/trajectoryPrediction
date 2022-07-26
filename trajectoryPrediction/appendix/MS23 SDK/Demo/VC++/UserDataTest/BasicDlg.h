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

	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
		
public:
	
	CameraHandle    m_hCamera;	//相机的设备句柄|the handle of the camera we use
	
	INT				m_iMaxBlockDatalen;
	BOOL InitCamera();
	afx_msg void OnBnClickedButtonNameRead();
	afx_msg void OnBnClickedButtonNameWrite();
	afx_msg void OnBnClickedButtonSnRead2();
	afx_msg void OnBnClickedButtonSnWrite2();
	afx_msg void OnBnClickedButtonSnRead3();
	afx_msg void OnBnClickedButtonSnWrite3();
	afx_msg void OnBnClickedButtonDataWriteEdit();
	afx_msg void OnBnClickedButtonDataWriteZero();
	afx_msg void OnBnClickedButtonDataWrite0xff();
	afx_msg void OnBnClickedButtonDataWriteRandom();
	afx_msg void OnBnClickedButtonDataWriteInc();
	afx_msg void OnBnClickedButtonDataRead();
	CString m_sBlockDataReadOut;
	CString m_sBlockDataWrite;
	CString m_sSerialNumber1;
	CString m_sSerialNumber2;
	CString m_sSerialNumber3;
	CString m_sCameraName;
	CString m_sCameraNameDesc;
	CString m_sSerialNumberDesc;
	CString m_sBlockDataStaticBox;
	afx_msg void OnBnClickedButtonSnRead1();
	afx_msg void OnBnClickedButtonSnWrite1();
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_BASICDLG_H__DE07E1D0_D0B7_4FA5_A4F3_45499366E00E__INCLUDED_)
