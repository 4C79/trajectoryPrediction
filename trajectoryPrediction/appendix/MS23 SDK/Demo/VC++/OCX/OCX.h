// OCX.h : main header file for the OCX application
//

#if !defined(AFX_OCX_H__3F65B310_3A4A_4CAC_9020_0A72BA56EE5E__INCLUDED_)
#define AFX_OCX_H__3F65B310_3A4A_4CAC_9020_0A72BA56EE5E__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#ifndef __AFXWIN_H__
	#error include 'stdafx.h' before including this file for PCH
#endif

#include "resource.h"		// main symbols

/////////////////////////////////////////////////////////////////////////////
// COCXApp:
// See OCX.cpp for the implementation of this class
//

class COCXApp : public CWinApp
{
public:
	COCXApp();

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(COCXApp)
	public:
	virtual BOOL InitInstance();
	//}}AFX_VIRTUAL

// Implementation

	//{{AFX_MSG(COCXApp)
		// NOTE - the ClassWizard will add and remove member functions here.
		//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};


/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_OCX_H__3F65B310_3A4A_4CAC_9020_0A72BA56EE5E__INCLUDED_)
