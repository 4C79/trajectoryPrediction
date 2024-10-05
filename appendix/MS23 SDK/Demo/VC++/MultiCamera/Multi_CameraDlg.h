// Multi_CameraDlg.h : header file
//
//BIG5 TRANS ALLOWED
#if !defined(AFX_MULTI_CAMERADLG_H__3FFD4567_7679_4CA0_9E7B_B5D8C532E3E0__INCLUDED_)
#define AFX_MULTI_CAMERADLG_H__3FFD4567_7679_4CA0_9E7B_B5D8C532E3E0__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

//引用MindVision相机SDK所需要的文件
#include "CameraApi.h"
#ifdef _WIN64
#pragma comment(lib, "..\\MVCAMSDK_X64.lib")
#else
#pragma comment(lib, "..\\MVCAMSDK.lib")
#endif
/////////////////////////////////////////////////////////////////////////////
// CMulti_CameraDlg dialog

class CMulti_CameraDlg : public CDialog
{
// Construction
public:
	CMulti_CameraDlg(CWnd* pParent = NULL);	// standard constructor

// Dialog Data
	//{{AFX_DATA(CMulti_CameraDlg)
	enum { IDD = IDD_MULTI_CAMERA_DIALOG };
	CButton	m_CameraName3;
	CButton	m_CameraName4;
	CButton	m_CameraName2;
	CButton	m_CameraName1;
	CStatic	m_DisplayArea4;
	CStatic	m_DisplayArea3;
	CStatic	m_DisplayArea2;
	CStatic	m_DisplayArea1;
	BOOL	m_bCamera1Pause;
	BOOL	m_bCamera2Pause;
	BOOL	m_bCamera3Pause;
	BOOL	m_bCamera4Pause;
	//}}AFX_DATA

public://相机所需变量定义
	int		m_hCamera[4];
	BYTE*	m_pFrameBuffer[4];
	UINT    m_threadID[4];//图像抓取线程的ID
	HANDLE  m_hDispThread[4];//图像抓取线程的句柄
	BOOL    m_bExit[4];//用来通知图像抓取线程结束
	tSdkFrameHead   m_sFrInfo[4];//用于保存当前图像帧的帧头信息

	BOOL InitCamera();
	void Snapshot(int hCamera);
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CMulti_CameraDlg)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV support
	//}}AFX_VIRTUAL
	


// Implementation
protected:
	HICON m_hIcon;

	// Generated message map functions
	//{{AFX_MSG(CMulti_CameraDlg)
	virtual BOOL OnInitDialog();
	afx_msg void OnSysCommand(UINT nID, LPARAM lParam);
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	afx_msg void OnCheckPause1();
	afx_msg void OnCheckPause2();
	afx_msg void OnCheckPause3();
	afx_msg void OnCheckPause4();
	afx_msg void OnButtonCmSet1();
	afx_msg void OnButtonSnapshot1();
	afx_msg void OnButtonCmSet2();
	afx_msg void OnButtonSnapshot2();
	afx_msg void OnButtonCmSet3();
	afx_msg void OnButtonSnapshot3();
	afx_msg void OnButtonCmSet4();
	afx_msg void OnButtonSnapshot4();
	afx_msg void OnClose();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()


};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_MULTI_CAMERADLG_H__3FFD4567_7679_4CA0_9E7B_B5D8C532E3E0__INCLUDED_)
