// Advanced.h : main header file for the ADVANCED application
//

#if !defined(AFX_ADVANCED_H__37912088_0E18_4CE2_A17E_B7D0F0D09611__INCLUDED_)
#define AFX_ADVANCED_H__37912088_0E18_4CE2_A17E_B7D0F0D09611__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#ifndef __AFXWIN_H__
	#error include 'stdafx.h' before including this file for PCH
#endif

#include "resource.h"		// main symbols

/////////////////////////////////////////////////////////////////////////////
// CAdvancedApp:
// See Advanced.cpp for the implementation of this class
//

class CAdvancedApp : public CWinApp
{
public:
	CAdvancedApp();

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CAdvancedApp)
	public:
	virtual BOOL InitInstance();
	//}}AFX_VIRTUAL

// Implementation

	//{{AFX_MSG(CAdvancedApp)
		// NOTE - the ClassWizard will add and remove member functions here.
		//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};


/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_ADVANCED_H__37912088_0E18_4CE2_A17E_B7D0F0D09611__INCLUDED_)
