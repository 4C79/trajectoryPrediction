// OCXDlg.h : header file
//
//{{AFX_INCLUDES()
#include "mvocx.h"
//}}AFX_INCLUDES

#if !defined(AFX_OCXDLG_H__928F95AE_3305_473B_A10D_771A85CA5461__INCLUDED_)
#define AFX_OCXDLG_H__928F95AE_3305_473B_A10D_771A85CA5461__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

/////////////////////////////////////////////////////////////////////////////
// COCXDlg dialog

class COCXDlg : public CDialog
{
// Construction
public:
	COCXDlg(CWnd* pParent = NULL);	// standard constructor

// Dialog Data
	//{{AFX_DATA(COCXDlg)
	enum { IDD = IDD_OCX_DIALOG };
	CString	m_sFileName;
	int		m_iFileType;
	BOOL    m_bPaused;
	CMVOCX	m_MvOcx;
	BOOL    m_bCameraFound;

	CString	m_sFileName1;
	int		m_iFileType1;
	BOOL    m_bPaused1;
	CMVOCX  m_MvOcx1;
	BOOL    m_bCameraFound1;

	BOOL	m_bThreadQuit[2];
	HANDLE	m_hThread[2];

public:

	static unsigned __stdcall DisplayThreadEntry1(void* Arg) { return ((COCXDlg*)Arg)->DisplayThread1(); }
	static unsigned __stdcall DisplayThreadEntry2(void* Arg) { return ((COCXDlg*)Arg)->DisplayThread2(); }
	unsigned DisplayThread1();
	unsigned DisplayThread2();

	//}}AFX_DATA

	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(COCXDlg)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV support
	//}}AFX_VIRTUAL
	BOOL CameraInit();
// Implementation
protected:
	HICON m_hIcon;

	// Generated message map functions
	//{{AFX_MSG(COCXDlg)
	virtual BOOL OnInitDialog();
	afx_msg void OnSysCommand(UINT nID, LPARAM lParam);
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	afx_msg void OnButtonPause();
	afx_msg void OnButtonCameraSet();
	afx_msg void OnButtonSnapshot();
	afx_msg void OnButtonSnapToBuffer();
	afx_msg void OnButtonPause1();
	afx_msg void OnButtonCameraSet1();
	afx_msg void OnButtonSnapshot1();
	afx_msg void OnButtonSnapToBuffer1();
	afx_msg void OnClose();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnTimer(UINT_PTR nIDEvent);
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_OCXDLG_H__928F95AE_3305_473B_A10D_771A85CA5461__INCLUDED_)
