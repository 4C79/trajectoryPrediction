// UdefClrTmp.cpp : implementation file
//
//BIG5 TRANS ALLOWED

#include "stdafx.h"
#include "Advanced.h"
#include "UdefClrTmp.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CUdefClrTmp dialog


CUdefClrTmp::CUdefClrTmp(CWnd* pParent /*=NULL*/)
	: CDialog(CUdefClrTmp::IDD, pParent)
{
	//{{AFX_DATA_INIT(CUdefClrTmp)
	m_uRed = 0;
	m_uGreen = 0;
	m_uBlue = 0;
	m_fClrMatrix1 = 0.0f;
	m_fClrMatrix2 = 0.0f;
	m_fClrMatrix3 = 0.0f;
	m_fClrMatrix4 = 0.0f;
	m_fClrMatrix5 = 0.0f;
	m_fClrMatrix6 = 0.0f;
	m_fClrMatrix7 = 0.0f;
	m_fClrMatrix8 = 0.0f;
	m_fClrMatrix9 = 0.0f;
	//}}AFX_DATA_INIT
}


void CUdefClrTmp::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CUdefClrTmp)
	DDX_Text(pDX, IDC_EDIT10, m_uRed);
	DDX_Text(pDX, IDC_EDIT11, m_uGreen);
	DDX_Text(pDX, IDC_EDIT12, m_uBlue);
	DDX_Text(pDX, IDC_EDIT1, m_fClrMatrix1);
	DDX_Text(pDX, IDC_EDIT2, m_fClrMatrix2);
	DDX_Text(pDX, IDC_EDIT3, m_fClrMatrix3);
	DDX_Text(pDX, IDC_EDIT4, m_fClrMatrix4);
	DDX_Text(pDX, IDC_EDIT5, m_fClrMatrix5);
	DDX_Text(pDX, IDC_EDIT6, m_fClrMatrix6);
	DDX_Text(pDX, IDC_EDIT7, m_fClrMatrix7);
	DDX_Text(pDX, IDC_EDIT8, m_fClrMatrix8);
	DDX_Text(pDX, IDC_EDIT9, m_fClrMatrix9);
	//}}AFX_DATA_MAP
}


BEGIN_MESSAGE_MAP(CUdefClrTmp, CDialog)
	//{{AFX_MSG_MAP(CUdefClrTmp)
	ON_BN_CLICKED(IDC_BUTTON1, OnButtonSetClrTmp)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CUdefClrTmp message handlers

void CUdefClrTmp::OnOK() 
{
	// TODO: Add extra validation here
	UpdateData(TRUE);
	CDialog::OnOK();
}

void CUdefClrTmp::OnButtonSetClrTmp() 
{
	// TODO: Add your control notification handler code here
	CColorDialog dlg; 
	COLORREF color; 
	
	if (dlg.DoModal()==IDOK) 
	{ 
		color = dlg.m_cc.rgbResult; 
		m_uRed= (color>>16)&0xff;
		m_uGreen = (color>>8)&0xff;
		m_uBlue = color&0xff;
		UpdateData(FALSE);
	} 
}
