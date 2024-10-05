// SdkCallTrace.cpp : implementation file
//

#include "stdafx.h"
#include "Basic.h"
#include "SdkCallTrace.h"
//BIG5 TRANS ALLOWED
#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CSdkCallTrace dialog


CSdkCallTrace::CSdkCallTrace(CWnd* pParent /*=NULL*/)
: CDialog(CSdkCallTrace::IDD, pParent)
{
	//{{AFX_DATA_INIT(CSdkCallTrace)
	// NOTE: the ClassWizard will add member initialization here
	//}}AFX_DATA_INIT
	m_bPause = FALSE;
}


void CSdkCallTrace::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CSdkCallTrace)
	DDX_Control(pDX, IDC_LIST_TRACE_LOG, m_cLogList);
	//}}AFX_DATA_MAP
}

VOID CSdkCallTrace::AppendLog(CString log)
{
	if (m_bPause)
	{
		return;
	}

	if (m_cLogList.GetCount() > 1000)//大于1000条后自动清零，防止溢出
	{
		m_cLogList.ResetContent();
	}

	m_cLogList.InsertString(-1,log);
	m_cLogList.SetCurSel(m_cLogList.GetCount() - 1 );
}

BEGIN_MESSAGE_MAP(CSdkCallTrace, CDialog)
//{{AFX_MSG_MAP(CSdkCallTrace)
ON_WM_SIZE()
// NOTE: the ClassWizard will add message map macros here
//}}AFX_MSG_MAP
ON_BN_CLICKED(IDC_BUTTON_PAUSE, OnButtonPause)
ON_BN_CLICKED(IDC_BUTTON_CLEAN, OnButtonClean)
END_MESSAGE_MAP()

VOID CSdkCallTrace::OnButtonPause()
{
	m_bPause = !m_bPause;
	GetDlgItem(IDC_BUTTON_PAUSE)->SetWindowText(m_bPause == FALSE ? "Pause":"Resume");
}

VOID CSdkCallTrace::OnButtonClean()
{
	m_cLogList.ResetContent();
}

void CSdkCallTrace::OnSize(UINT nType, int cx, int cy) 
{
	CDialog::OnSize(nType, cx, cy);
	
	// TODO: Add your message handler code here
	RECT rect;
	
	if (m_cLogList.m_hWnd == NULL)
	{
		return;
	}
	
	GetClientRect(&rect);
	rect.top += 50;
	m_cLogList.MoveWindow(&rect);
}


/////////////////////////////////////////////////////////////////////////////
// CSdkCallTrace message handlers
