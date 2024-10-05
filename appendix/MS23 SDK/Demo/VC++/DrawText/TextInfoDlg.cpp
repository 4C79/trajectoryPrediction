// TextInfoDlg.cpp : 实现文件
//

#include "stdafx.h"
#include "DrawText.h"
#include "TextInfoDlg.h"
#include "afxdialogex.h"


// CTextInfoDlg 对话框

IMPLEMENT_DYNAMIC(CTextInfoDlg, CDialogEx)

CTextInfoDlg::CTextInfoDlg(CWnd* pParent /*=NULL*/)
	: CDialogEx(CTextInfoDlg::IDD, pParent)
	, m_strFontName(_T("c:\\windows\\fonts\\simsun.ttc"))
	, m_uFontWidth(48)
	, m_uFontHeight(48)
	, m_OutRectLeft(0)
	, m_OutRectTop(0)
	, m_OutRectWidth(300)
	, m_OutRectHeight(300)
	, m_bAlphaBlend(TRUE)
	, m_bAntiAliasing(FALSE)
	, m_bAutoBreakLine(TRUE)
	, m_strText(_T("在此输入要显示的文字"))
	, m_TextColor(RGB(255, 0, 0))
{

}

CTextInfoDlg::~CTextInfoDlg()
{
}

void CTextInfoDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
	DDX_Text(pDX, IDC_EDIT_FONT_FILENAME, m_strFontName);
	DDX_Text(pDX, IDC_EDIT_FONT_WIDTH, m_uFontWidth);
	DDX_Text(pDX, IDC_EDIT_FONT_HEIGHT, m_uFontHeight);
	DDX_Text(pDX, IDC_EDIT_LEFT, m_OutRectLeft);
	DDX_Text(pDX, IDC_EDIT_TOP, m_OutRectTop);
	DDX_Text(pDX, IDC_EDIT_WIDTH, m_OutRectWidth);
	DDX_Text(pDX, IDC_EDIT_HEIGHT, m_OutRectHeight);
	DDX_Control(pDX, IDC_COMBO_HORI_ALIGN, m_ComboHoriAlign);
	DDX_Control(pDX, IDC_COMBO_VERT_ALIGN, m_ComboVertAlign);
	DDX_Check(pDX, IDC_CHECK_ALPHA_BLEND, m_bAlphaBlend);
	DDX_Check(pDX, IDC_CHECK_AUTO_BREAKLINE, m_bAutoBreakLine);
	DDX_Text(pDX, IDC_EDIT_TEXT, m_strText);
	DDX_Check(pDX, IDC_CHECK_ANTI_ALIASING, m_bAntiAliasing);
}


BEGIN_MESSAGE_MAP(CTextInfoDlg, CDialogEx)
	ON_WM_DESTROY()
	ON_BN_CLICKED(IDC_BUTTON_SETTINGS, &CTextInfoDlg::OnBnClickedButtonSettings)
	ON_WM_CTLCOLOR()
	ON_BN_CLICKED(IDC_BUTTON_SEL_COLOR, &CTextInfoDlg::OnBnClickedButtonSelColor)
	ON_BN_CLICKED(IDC_BUTTON_CLEAR_TEXT, &CTextInfoDlg::OnBnClickedButtonClearText)
	ON_BN_CLICKED(IDC_BUTTON_ADDTEXT, &CTextInfoDlg::OnBnClickedButtonAddtext)
END_MESSAGE_MAP()


// CTextInfoDlg 消息处理程序


BOOL CTextInfoDlg::OnInitDialog()
{
	CDialogEx::OnInitDialog();

	// TODO:  在此添加额外的初始化
	m_pDrawTextDlg = (CDrawTextDlg*)AfxGetMainWnd();

	tSdkCameraCapbility sCap;
	CameraGetCapability(m_pDrawTextDlg->m_hCamera, &sCap);
	m_OutRectWidth = sCap.sResolutionRange.iWidthMax;
	m_OutRectHeight = sCap.sResolutionRange.iHeightMax;

	m_ComboHoriAlign.SetCurSel(0);
	m_ComboVertAlign.SetCurSel(0);

	UpdateData(FALSE);
	return TRUE;  // return TRUE unless you set the focus to a control
	// 异常: OCX 属性页应返回 FALSE
}

void CTextInfoDlg::OnDestroy()
{
	CDialogEx::OnDestroy();

	// TODO: 在此处添加消息处理程序代码
}

void CTextInfoDlg::OnBnClickedButtonSettings()
{
	CameraShowSettingPage(m_pDrawTextDlg->m_hCamera, TRUE);
}

HBRUSH CTextInfoDlg::OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor)
{
	HBRUSH hbr = CDialogEx::OnCtlColor(pDC, pWnd, nCtlColor);

	// TODO:  在此更改 DC 的任何特性
	if (pWnd->GetSafeHwnd() == GetDlgItem(IDC_STATIC_TEXT_COLOR)->GetSafeHwnd())
	{
		pDC->SetTextColor(m_TextColor);
	}

	// TODO:  如果默认的不是所需画笔，则返回另一个画笔
	return hbr;
}

void CTextInfoDlg::OnBnClickedButtonSelColor()
{
	CColorDialog Dlg;
	Dlg.m_cc.Flags |= CC_FULLOPEN | CC_RGBINIT;
	Dlg.m_cc.rgbResult = m_TextColor;
	if (Dlg.DoModal() == IDOK)
	{
		m_TextColor = Dlg.m_cc.rgbResult;
		GetDlgItem(IDC_STATIC_TEXT_COLOR)->Invalidate();
	}
}

void CTextInfoDlg::OnBnClickedButtonClearText()
{
	m_pDrawTextDlg->m_csTextItemArray.Lock();
	m_pDrawTextDlg->m_TextItemArray.RemoveAll();
	m_pDrawTextDlg->m_csTextItemArray.Unlock();
}

void CTextInfoDlg::OnBnClickedButtonAddtext()
{
	UpdateData(TRUE);
	m_strText.Remove('\r');

	TextItem Item;
	Item.strFontFileName = m_strFontName;
	Item.uFontW = m_uFontWidth;
	Item.uFontH = m_uFontHeight;
	Item.strText = m_strText;
	Item.rcOut.SetRect(m_OutRectLeft, m_OutRectTop, 
		m_OutRectLeft + m_OutRectWidth, m_OutRectTop + m_OutRectHeight);
	Item.uColor = m_TextColor;
	Item.uFlags = 0;
	if (m_bAlphaBlend)
		Item.uFlags |= CAMERA_DT_ALPHA_BLEND;
	if (m_bAntiAliasing)
		Item.uFlags |= CAMERA_DT_ANTI_ALIASING;
	if (!m_bAutoBreakLine)
		Item.uFlags |= CAMERA_DT_SINGLELINE;
	switch (m_ComboHoriAlign.GetCurSel())
	{
	case 1:
		Item.uFlags |= CAMERA_DT_HCENTER;
		break;
	case 2:
		Item.uFlags |= CAMERA_DT_RIGHT;
		break;
	}
	switch (m_ComboVertAlign.GetCurSel())
	{
	case 1:
		Item.uFlags |= CAMERA_DT_VCENTER;
		break;
	case 2:
		Item.uFlags |= CAMERA_DT_BOTTOM;
		break;
	}

	m_pDrawTextDlg->m_csTextItemArray.Lock();
	m_pDrawTextDlg->m_TextItemArray.Add(Item);
	m_pDrawTextDlg->m_csTextItemArray.Unlock();
}
