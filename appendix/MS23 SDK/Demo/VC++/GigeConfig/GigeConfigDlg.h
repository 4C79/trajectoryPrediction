
// GigeConfigDlg.h : 头文件
//

#pragma once


#include "CameraApi.h"
#include "afxwin.h"
#ifdef _WIN64
#pragma comment(lib, ".\\MVCAMSDK_X64.lib")
#else
#pragma comment(lib, ".\\MVCAMSDK.lib")
#endif


// CGigeConfigDlg 对话框
class CGigeConfigDlg : public CDialogEx
{
// 构造
public:
	CGigeConfigDlg(CWnd* pParent = NULL);	// 标准构造函数

// 对话框数据
	enum { IDD = IDD_GIGECONFIG_DIALOG };

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV 支持

protected:

	CArray<tSdkCameraDevInfo> m_CamerasInfo;


// 实现
protected:
	HICON m_hIcon;

	// 生成的消息映射函数
	virtual BOOL OnInitDialog();
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	DECLARE_MESSAGE_MAP()
public:
	CListBox m_ListBoxCamera;
	CString m_strCamIp;
	CString m_strCamMask;
	CString m_strCamGateway;
	CString m_strEtIp;
	CString m_strEtMask;
	CString m_strEtGateway;
	afx_msg void OnBnClickedButtonUpdate();
	afx_msg void OnLbnSelchangeListCamera();
	afx_msg void OnBnClickedButtonApply();
	CString m_strCamMac;
	CString m_strEtMac;
};
