
// CameraDisplay.cpp : 定义应用程序的类行为。
//

#include "stdafx.h"
#include "CameraDisplay.h"
#include "CameraDisplayDlg.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


// CCameraDisplayApp

BEGIN_MESSAGE_MAP(CCameraDisplayApp, CWinApp)
	ON_COMMAND(ID_HELP, &CWinApp::OnHelp)
END_MESSAGE_MAP()


// CCameraDisplayApp 构造

CCameraDisplayApp::CCameraDisplayApp()
{
	// 支持重新启动管理器
	m_dwRestartManagerSupportFlags = AFX_RESTART_MANAGER_SUPPORT_RESTART;

	// TODO: 在此处添加构造代码，
	// 将所有重要的初始化放置在 InitInstance 中
}


// 唯一的一个 CCameraDisplayApp 对象

CCameraDisplayApp theApp;
int gLanguage = 0;


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

// CCameraDisplayApp 初始化

BOOL CCameraDisplayApp::InitInstance()
{
	// 如果一个运行在 Windows XP 上的应用程序清单指定要
	// 使用 ComCtl32.dll 版本 6 或更高版本来启用可视化方式，
	//则需要 InitCommonControlsEx()。否则，将无法创建窗口。
	INITCOMMONCONTROLSEX InitCtrls;
	InitCtrls.dwSize = sizeof(InitCtrls);
	// 将它设置为包括所有要在应用程序中使用的
	// 公共控件类。
	InitCtrls.dwICC = ICC_WIN95_CLASSES;
	InitCommonControlsEx(&InitCtrls);

	CWinApp::InitInstance();


	AfxEnableControlContainer();

	// 创建 shell 管理器，以防对话框包含
	// 任何 shell 树视图控件或 shell 列表视图控件。
	CShellManager *pShellManager = new CShellManager;	

	gLanguage = GetRegLanguage();

	CCameraDisplayDlg dlg;
	m_pMainWnd = &dlg;
	INT_PTR nResponse = dlg.DoModal();
	if (nResponse == IDOK)
	{
		// TODO: 在此放置处理何时用
		//  “确定”来关闭对话框的代码
	}
	else if (nResponse == IDCANCEL)
	{
		// TODO: 在此放置处理何时用
		//  “取消”来关闭对话框的代码
	}

	// 删除上面创建的 shell 管理器。
	if (pShellManager != NULL)
	{
		delete pShellManager;
	}

	// 由于对话框已关闭，所以将返回 FALSE 以便退出应用程序，
	//  而不是启动应用程序的消息泵。
	return FALSE;
}

