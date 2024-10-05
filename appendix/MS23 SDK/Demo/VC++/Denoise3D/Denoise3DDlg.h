
// Denoise3DDlg.h : 头文件
//

#pragma once

#include "CameraApi.h"
#include "CameraGrabber.h"
#include "CameraImage.h"
#pragma comment(lib, "MVCAMSDK.lib")


// CDenoise3DDlg 对话框
class CDenoise3DDlg : public CDialogEx
{
// 构造
public:
	CDenoise3DDlg(CWnd* pParent = NULL);	// 标准构造函数

// 对话框数据
	enum { IDD = IDD_DENOISE3D_DIALOG };

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV 支持

	void* m_Grabber;
	CameraHandle m_hCamera;
	tSdkCameraDevInfo m_DevInfo;

	struct MvImage
	{
		tSdkFrameHead RawHead;
		BYTE* RawData;
		tSdkFrameHead RgbHead;
		BYTE* RgbData;

		MvImage()
		{
			ZeroMemory(&RawHead, sizeof(RawHead));
			RawData = NULL;
			ZeroMemory(&RgbHead, sizeof(RgbHead));
			RgbData = NULL;
		}

		~MvImage()
		{
			CameraAlignFree(RawData);
			CameraAlignFree(RgbData);
		}
	};
	MvImage m_Imgs[4];

private:

	void DoImgCap(int index);
	BOOL DoCompose();
	void DispImg(int index);
	void SaveImg(int index);

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
	afx_msg void OnBnClickedButtonImgCap1();
	afx_msg void OnBnClickedButtonImgCap2();
	afx_msg void OnBnClickedButtonImgCap3();
	afx_msg void OnBnClickedButtonImgSave1();
	afx_msg void OnBnClickedButtonImgSave2();
	afx_msg void OnBnClickedButtonImgSave3();
	afx_msg void OnBnClickedButtonImgSave4();
	afx_msg void OnBnClickedButtonSettings();
};
