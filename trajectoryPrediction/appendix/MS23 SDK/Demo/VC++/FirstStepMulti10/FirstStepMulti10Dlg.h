
// FirstStepMulti10Dlg.h : 头文件
//

#pragma once

#include "CameraApi.h"
#include "CameraGrabber.h"
#include "CameraImage.h"
#pragma comment(lib, "MVCAMSDK.lib")


// CFirstStepMulti10Dlg 对话框
class CFirstStepMulti10Dlg : public CDialogEx
{
// 构造
public:
	CFirstStepMulti10Dlg(CWnd* pParent = NULL);	// 标准构造函数

// 对话框数据
	enum { IDD = IDD_FIRSTSTEPMULTI10_DIALOG };

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV 支持

	void* m_Grabber[10];
	CameraHandle m_hCamera[10];
	tSdkCameraDevInfo m_DevInfo[10];

	int m_NumberOfCameras;
	int m_MaxActiveCount;

	UINT m_TrggerTimerID;

	BOOL m_Paused[10];
	BOOL m_bImmediatelyTriggerNext;

	CCriticalSection m_csLock;
	int m_CurrentActiveCamera;
	DWORD m_TrggerTime[10];

	static int __stdcall FrameListenerEntry(void* Grabber, int Phase, BYTE *pFrameBuffer, tSdkFrameHead* pFrameHead, void* Context)
	{
		CFirstStepMulti10Dlg* pThis = (CFirstStepMulti10Dlg*)Context;
		return pThis->FrameListener(Grabber, Phase, pFrameBuffer, pFrameHead);
	}
	int FrameListener(void* Grabber, int Phase, BYTE *pFrameBuffer, tSdkFrameHead* pFrameHead);

	static void __stdcall SaveImageCompleteEntry(void* Grabber, void* Image, CameraSdkStatus Status, void* Context)
	{
		CFirstStepMulti10Dlg* pThis = (CFirstStepMulti10Dlg*)Context;
		pThis->SaveImageComplete(Grabber, Image, Status);
	}
	void SaveImageComplete(void* Grabber, void* Image, CameraSdkStatus Status);

	static void CALLBACK TriggerTimerEntry(UINT uTimerID, UINT uMsg, DWORD_PTR dwUser, DWORD_PTR dw1, DWORD_PTR dw2)
	{
		CFirstStepMulti10Dlg* pThis = (CFirstStepMulti10Dlg*)dwUser;
		pThis->TriggerTimer();
	}
	void TriggerTimer();

	void TriggerCamera();
	int GetNumberOfActiveCamera();

// 实现
protected:
	HICON m_hIcon;

	// 生成的消息映射函数
	virtual BOOL OnInitDialog();
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnDestroy();
	afx_msg void OnTimer(UINT_PTR nIDEvent);
	afx_msg void OnBnClickedCheckPause1();
	afx_msg void OnBnClickedButtonCmSet1();
	afx_msg void OnBnClickedButtonSnapshot1();
	afx_msg void OnBnClickedCheckPause2();
	afx_msg void OnBnClickedButtonCmSet2();
	afx_msg void OnBnClickedButtonSnapshot2();
	afx_msg void OnBnClickedCheckPause3();
	afx_msg void OnBnClickedButtonCmSet3();
	afx_msg void OnBnClickedButtonSnapshot3();
	afx_msg void OnBnClickedCheckPause4();
	afx_msg void OnBnClickedButtonCmSet4();
	afx_msg void OnBnClickedButtonSnapshot4();	
	afx_msg void OnBnClickedCheckPause5();
	afx_msg void OnBnClickedButtonCmSet5();
	afx_msg void OnBnClickedButtonSnapshot5();
	afx_msg void OnBnClickedCheckPause6();
	afx_msg void OnBnClickedButtonCmSet6();
	afx_msg void OnBnClickedButtonSnapshot6();
	afx_msg void OnBnClickedCheckPause7();
	afx_msg void OnBnClickedButtonCmSet7();
	afx_msg void OnBnClickedButtonSnapshot7();
	afx_msg void OnBnClickedCheckPause8();
	afx_msg void OnBnClickedButtonCmSet8();
	afx_msg void OnBnClickedButtonSnapshot8();
	afx_msg void OnBnClickedCheckPause9();
	afx_msg void OnBnClickedButtonCmSet9();
	afx_msg void OnBnClickedButtonSnapshot9();
	afx_msg void OnBnClickedCheckPause10();
	afx_msg void OnBnClickedButtonCmSet10();
	afx_msg void OnBnClickedButtonSnapshot10();
	afx_msg void OnBnClickedCheckImmTriNext();
	afx_msg void OnEnChangeEditMaxActive();
	afx_msg LRESULT OnSetDlgItemInt(WPARAM wParam, LPARAM lParam);
	afx_msg LRESULT OnSetDlgItemText(WPARAM wParam, LPARAM lParam);
};
