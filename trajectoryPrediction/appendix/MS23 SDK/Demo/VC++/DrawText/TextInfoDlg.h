#pragma once

#include "DrawTextDlg.h"
#include "afxwin.h"


// CTextInfoDlg 对话框

class CTextInfoDlg : public CDialogEx
{
	DECLARE_DYNAMIC(CTextInfoDlg)

public:
	CTextInfoDlg(CWnd* pParent = NULL);   // 标准构造函数
	virtual ~CTextInfoDlg();

// 对话框数据
	enum { IDD = IDD_DIALOG_TEXT_INFO };

	CDrawTextDlg* m_pDrawTextDlg;

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV 支持

	DECLARE_MESSAGE_MAP()
public:
	virtual BOOL OnInitDialog();
	afx_msg void OnDestroy();
	afx_msg void OnBnClickedButtonSettings();
	CString m_strFontName;
	UINT m_uFontWidth;
	UINT m_uFontHeight;
	int m_OutRectLeft;
	int m_OutRectTop;
	UINT m_OutRectWidth;
	UINT m_OutRectHeight;
	CComboBox m_ComboHoriAlign;
	CComboBox m_ComboVertAlign;
	BOOL m_bAlphaBlend;
	BOOL m_bAntiAliasing;
	BOOL m_bAutoBreakLine;
	CString m_strText;
	COLORREF m_TextColor;
	afx_msg HBRUSH OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor);
	afx_msg void OnBnClickedButtonSelColor();
	afx_msg void OnBnClickedButtonClearText();
	afx_msg void OnBnClickedButtonAddtext();
};
