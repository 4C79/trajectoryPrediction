// Basic.cpp : Defines the class behaviors for the application.
//

#include "stdafx.h"
#include "Basic.h"
#include "BasicDlg.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CBasicApp

BEGIN_MESSAGE_MAP(CBasicApp, CWinApp)
	//{{AFX_MSG_MAP(CBasicApp)
		// NOTE - the ClassWizard will add and remove mapping macros here.
		//    DO NOT EDIT what you see in these blocks of generated code!
	//}}AFX_MSG
	ON_COMMAND(ID_HELP, CWinApp::OnHelp)
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CBasicApp construction

CBasicApp::CBasicApp()
{
	// TODO: add construction code here,
	// Place all significant initialization in InitInstance
}

/////////////////////////////////////////////////////////////////////////////
// The one and only CBasicApp object

CBasicApp theApp;
int gLanguage = 0;
/////////////////////////////////////////////////////////////////////////////
// CBasicApp initialization

int GetRegLanguage()
{
	LONG status;
	char strPath[MAX_PATH];
	char szCompany[64];
	HKEY hkey = NULL;
	DWORD dwType = REG_SZ;
	DWORD dwSize = MAX_PATH;
	BYTE abyValue[MAX_PATH];

	if (ERROR_SUCCESS != RegOpenKeyEx(HKEY_LOCAL_MACHINE, "Software\\Industry Camera", 0, KEY_READ, &hkey))
		return 0;
	dwSize = sizeof(szCompany);
	status = RegQueryValueEx(hkey, "Company", NULL, &dwType, (LPBYTE)szCompany, &dwSize);
	RegCloseKey(hkey);
	hkey = NULL;
	if (status != ERROR_SUCCESS)
		return 0;

	sprintf(strPath, "Software\\%s\\Settings", szCompany);
	RegOpenKeyEx(HKEY_LOCAL_MACHINE, strPath, 0, KEY_READ, &hkey);

	if (NULL != hkey)
	{
		memset(abyValue, 0x00, MAX_PATH);
		dwType = REG_SZ;
		dwSize = MAX_PATH;
		LONG status = RegQueryValueEx(hkey, "Language", NULL, &dwType, abyValue, &dwSize);
		RegCloseKey(hkey);

		if (ERROR_SUCCESS == status)
		{
			abyValue[MAX_PATH-1] = '\0';

			if (strcmp((const char *)abyValue,"Chinese") == 0)
			{
				return 1;
			}
			else if(strcmp((const char *)abyValue,"TradChinese") == 0)
			{
				return 2;
			}
			else
			{
				return 0;
			}

		}
	}

	return 0;
}


BOOL CBasicApp::InitInstance()
{
	AfxEnableControlContainer();

	// Standard initialization
	// If you are not using these features and wish to reduce the size
	//  of your final executable, you should remove from the following
	//  the specific initialization routines you do not need.

#ifdef _AFXDLL
	Enable3dControls();			// Call this when using MFC in a shared DLL
#else
	Enable3dControlsStatic();	// Call this when linking to MFC statically
#endif
	gLanguage = GetRegLanguage();

	CBasicDlg dlg;
	m_pMainWnd = &dlg;
	int nResponse = dlg.DoModal();
	if (nResponse == IDOK)
	{
		// TODO: Place code here to handle when the dialog is
		//  dismissed with OK
	}
	else if (nResponse == IDCANCEL)
	{
		// TODO: Place code here to handle when the dialog is
		//  dismissed with Cancel
	}

	// Since the dialog has been closed, return FALSE so that we exit the
	//  application, rather than start the application's message pump.
	return FALSE;
}
