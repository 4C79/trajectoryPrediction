// Multi_Camera.h : main header file for the MULTI_CAMERA application
//

#if !defined(AFX_MULTI_CAMERA_H__07ACAC7D_B93C_453A_BE67_48AC22C2F584__INCLUDED_)
#define AFX_MULTI_CAMERA_H__07ACAC7D_B93C_453A_BE67_48AC22C2F584__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#ifndef __AFXWIN_H__
	#error include 'stdafx.h' before including this file for PCH
#endif

#include "resource.h"		// main symbols

/////////////////////////////////////////////////////////////////////////////
// CMulti_CameraApp:
// See Multi_Camera.cpp for the implementation of this class
//

class CMulti_CameraApp : public CWinApp
{
public:
	CMulti_CameraApp();

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CMulti_CameraApp)
	public:
	virtual BOOL InitInstance();
	//}}AFX_VIRTUAL

// Implementation

	//{{AFX_MSG(CMulti_CameraApp)
		// NOTE - the ClassWizard will add and remove member functions here.
		//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};


/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_MULTI_CAMERA_H__07ACAC7D_B93C_453A_BE67_48AC22C2F584__INCLUDED_)
