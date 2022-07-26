#pragma once

#include "AsyncSaveImage.h"


// CCameraDlg 对话框

class CCameraDlg : public CDialogEx
{
	DECLARE_DYNAMIC(CCameraDlg)

public:
	CCameraDlg(char const* SavePrefix, CWnd* pParent = NULL);   // 标准构造函数
	virtual ~CCameraDlg();

	int Open(int index);
	void Close();

	int Start();
	void Stop();

	int SetTriggerMode(int mode);
	int SetTriggerCount(int count);
	int SoftTrigger();

	void SetSaveDir(char const* SaveDir);
	void SetSaveFormat(int Fmt);
	void SetAutoSave(BOOL bEnable);
	void SetOnlySaveTrigger(BOOL bEnable);
	void SetOverlayDate(BOOL bEnable);
	void SetSaveCount(int count);
	int GetSaveCount() const;

// 对话框数据
	enum { IDD = IDD_CAM_DIALOG };

private:

	static int __stdcall FrameListenerEntry(void *Grabber, int Phase, BYTE *pFrameBuffer, tSdkFrameHead *pFrameHead, void *Context)
	{
		CCameraDlg* pThis = (CCameraDlg*)Context;

		if (Phase == 0)
		{
			return pThis->FrameRAWCallback(Grabber, pFrameBuffer, pFrameHead);
		}
		else if (Phase == 2)
		{
			CameraDisplayRGB24(pThis->m_hCamera, pFrameBuffer, pFrameHead);
			return 0;
		}

		return 1;
	}
	int FrameRAWCallback(void* Grabber, BYTE *pFrameBuffer, tSdkFrameHead* pFrameHead);

private:

	void *m_Grabber;
	int m_hCamera;
	BOOL m_bMono;
	tSdkCameraDevInfo m_DevInfo;
	CString m_strSavePrefix;
	int m_SaveNum;
	int m_SaveCount;

	CCriticalSection m_DataLock;
	CString m_strSaveDir;
	int m_SaveFmt;
	BOOL m_bAutoSave;
	BOOL m_bOnlySaveTrigger;
	BOOL m_bOverlayDate;
	CAsyncSaveImage::DrawTextItem m_OverlayItem;

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV 支持

	DECLARE_MESSAGE_MAP()
public:
	virtual BOOL OnInitDialog();
	afx_msg void OnDestroy();
	afx_msg void OnTimer(UINT_PTR nIDEvent);
	afx_msg void OnBnClickedButtonSetting();
	afx_msg void OnSize(UINT nType, int cx, int cy);
};
