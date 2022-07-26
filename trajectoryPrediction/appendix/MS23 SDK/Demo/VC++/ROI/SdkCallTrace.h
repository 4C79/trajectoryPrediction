#if !defined(AFX_SDKCALLTRACE_H__04F027B6_BD7A_4483_9F1A_ECD584BF3716__INCLUDED_)
#define AFX_SDKCALLTRACE_H__04F027B6_BD7A_4483_9F1A_ECD584BF3716__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// SdkCallTrace.h : header file
//

class CSdkCallTrace : public CDialog
{
	// Construction
public:
	CSdkCallTrace(CWnd* pParent = NULL);   // standard constructor
	VOID AppendLog(CString log);
	// Dialog Data
	//{{AFX_DATA(CSdkCallTrace)
	enum { IDD = IDD_DIALOG_LOG };
	CListBox	m_cLogList;
	BOOL		m_bPause;
	//}}AFX_DATA
	
	
	// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CSdkCallTrace)
protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL
	
	// Implementation
protected:
	
	// Generated message map functions
	//{{AFX_MSG(CSdkCallTrace)
	// NOTE: the ClassWizard will add member functions here
	//}}AFX_MSG
	afx_msg void OnSize(UINT nType, int cx, int cy);
	afx_msg void OnButtonPause();
	afx_msg void OnButtonClean();
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_SDKCALLTRACE_H__04F027B6_BD7A_4483_9F1A_ECD584BF3716__INCLUDED_)
