
// RawTransTestDlg.h : 头文件
//

#pragma once


// CRawTransTestDlg 对话框
class CRawTransTestDlg : public CDialogEx
{
// 构造
public:
	CRawTransTestDlg(CWnd* pParent = NULL);	// 标准构造函数

// 对话框数据
	enum { IDD = IDD_RAWTRANSTEST_DIALOG };

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV 支持


// 实现
protected:
	HICON m_hIcon;

	// 生成的消息映射函数
	virtual BOOL OnInitDialog();
	afx_msg void OnSysCommand(UINT nID, LPARAM lParam);
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	DECLARE_MESSAGE_MAP()
public:
	CString m_sRawFile;
	CString m_sOutPutFile;
	afx_msg void OnBnClickedButtonTrans();
	afx_msg void OnBnClickedButtonInit();
	afx_msg void OnBnClickedButton3();
	CString m_sConfigFile;

	PVOID m_pCameraConfig;
	afx_msg void OnBnClickedButtonTrans2();
	CString m_sJpgOutFile;
	afx_msg void OnClose();
};
