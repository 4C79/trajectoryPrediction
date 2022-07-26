#if !defined(AFX_UDEFCLRTMP_H__6B386279_32EF_4380_95F9_C2A0E28D8D08__INCLUDED_)
#define AFX_UDEFCLRTMP_H__6B386279_32EF_4380_95F9_C2A0E28D8D08__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// UdefClrTmp.h : header file
//

/////////////////////////////////////////////////////////////////////////////
// CUdefClrTmp dialog

class CUdefClrTmp : public CDialog
{
// Construction
public:
	CUdefClrTmp(CWnd* pParent = NULL);   // standard constructor

// Dialog Data
	//{{AFX_DATA(CUdefClrTmp)
	enum { IDD = IDD_DIALOG_CUSTOM_CT };
	UINT	m_uRed;
	UINT	m_uGreen;
	UINT	m_uBlue;
	float	m_fClrMatrix1;
	float	m_fClrMatrix2;
	float	m_fClrMatrix3;
	float	m_fClrMatrix4;
	float	m_fClrMatrix5;
	float	m_fClrMatrix6;
	float	m_fClrMatrix7;
	float	m_fClrMatrix8;
	float	m_fClrMatrix9;
	//}}AFX_DATA


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CUdefClrTmp)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:

	// Generated message map functions
	//{{AFX_MSG(CUdefClrTmp)
	virtual void OnOK();
	afx_msg void OnButtonSetClrTmp();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_UDEFCLRTMP_H__6B386279_32EF_4380_95F9_C2A0E28D8D08__INCLUDED_)
