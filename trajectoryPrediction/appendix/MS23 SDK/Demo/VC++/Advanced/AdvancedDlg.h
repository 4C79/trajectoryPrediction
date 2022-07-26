// AdvancedDlg.h : header file
//
//BIG5 TRANS ALLOWED

#if !defined(AFX_ADVANCEDDLG_H__4B7A0F2F_62FA_4C67_AB8B_8DE471779F2C__INCLUDED_)
#define AFX_ADVANCEDDLG_H__4B7A0F2F_62FA_4C67_AB8B_8DE471779F2C__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000


#include "SdkCallTrace.h"//用于显示例程中SDK调用函数的日志信息
/////////////////////////////////////////////////////////////////////////////
// CAdvancedDlg dialog

//引用相机的SDK包，lib文件的路径根据您的开发环境修改。
#include "CameraApi.h"	 
#ifdef _WIN64
#pragma comment(lib, "..\\MVCAMSDK_X64.lib")
#else
#pragma comment(lib, "..\\MVCAMSDK.lib")
#endif
/*输出例程中调用相机的SDK接口日志信息*/
#define SDK_TRACE(_FUNC_,TXT) \
{\
	CameraSdkStatus status;\
	CString msg;\
	CString FuncName;\
	FuncName = #_FUNC_;\
	FuncName = FuncName.Left(FuncName.FindOneOf("("));\
\
	status = _FUNC_;\
	if (status != CAMERA_STATUS_SUCCESS)\
	{\
	msg.Format(gLanguage?"函数:[%s] 调用失败!":"Function:[%s] return error",FuncName);\
	m_DlgLog.AppendLog(msg);\
	msg.Format(gLanguage?"错误码:%d. 请参考CameraStatus.h中错误码的详细定义":"Error code:%d.refer to CameraStatus.h for more information",status);\
	m_DlgLog.AppendLog(msg);\
	}\
	else\
	{\
	msg.Format(gLanguage?"函数:[%s] 调用成功!":"Function:[%s] success",FuncName);\
	m_DlgLog.AppendLog(msg);\
	msg.Format(gLanguage?"功能:%s.":"Action:%s",TXT);\
	m_DlgLog.AppendLog(msg);\
	}\
	msg = "";\
	m_DlgLog.AppendLog(msg);\
}
class CAdvancedDlg : public CDialog
{
// Construction
public:
	CAdvancedDlg(CWnd* pParent = NULL);	// standard constructor

// Dialog Data
	//{{AFX_DATA(CAdvancedDlg)
	enum { IDD = IDD_ADVANCED_DIALOG_CN };
	CComboBox	m_FrameSpeedList;	//帧率选择列表
	CComboBox	m_cClrTmpList;      //色温列表控件
	CSliderCtrl	m_cSldAeTarget;		//自动曝光亮度目标滚动条
	CSliderCtrl	m_cSldSaturation;	//饱和度调节滚动条
	CSliderCtrl	m_cSldRedGain;		//红色数字增益调节滚动条
	CSliderCtrl	m_cSldGreenGain;	//绿色数字增益调节滚动条
	CSliderCtrl	m_cSldGamma;		//gamma调节滚动条
	CSliderCtrl	m_cSldExposureLines;//帧曝光行数调节滚动条
	CSliderCtrl	m_cSldSharppen;		//锐化调节滚动条
	CSliderCtrl	m_cSldContrast;		//对比度调节滚动条
	CSliderCtrl	m_cSldBlueGain;		//蓝色数字增益调节滚动条
	CSliderCtrl	m_cSldAnalogGain;	//模拟增益调节滚动条	
	CComboBox	m_cPresetLutList;	//预设LUT列表
	CComboBox	m_cResolutionList;	//分辨率列表
	CStatic	m_cPreview;				//显示图像的控件
	int		m_iTriggerModeSel;		//触发选择模式
	int		m_iSnapfileTypeSel;		//抓拍文件保存格式
	int		m_iSnapshotResSel;		//抓怕分辨率模式(可以和预览时相同，也可以单独设置)
	int		m_iWbModeSel;			//白平衡模式(分为手动和自动两种)
	int		m_iLutModeSel;			//LUT设置模式
	int		m_iExposureModeSel;		//曝光模式(分为自动和手动两种)
	int		m_iAntiflickFreqSel;	//自动曝光时抗频闪的频率选择(分为50和60赫兹两种)
	int		m_iCrossPositionX;		//十字线中心坐标的X值
	int		m_iCrossPositionY;		//十字线中心坐标的Y值
	int		m_iHardwareTriggerDelayUs;	//硬触发时的延时，单位us
	int		m_iTriggerCount;		//触摸模式下，一次触发信号所触发的帧数
	int     m_iClrTempSel;          //当前选择的色温在列表中的索引号
	CString	m_sSnapFilePath;		//抓拍图像保存的路径
	BOOL	m_bVflip;				//图像垂直镜像标志
	BOOL	m_bHflip;				//图像水平镜像标志
	BOOL	m_bAntiFlick;			//抗频闪使能标志
	BOOL	m_bNoiseReduce;			//降噪使能标志
	BOOL	m_bDisplayWbWindow;		//白平衡窗口显示标志
	BOOL	m_bDisplayAeWindow;		//自动曝光窗口显示标志
	BOOL	m_bDisplayCrosshair;	//十字线显示使能标志
	int		m_iAutoSnapCycleMs;		//自动抓拍周期，单位毫秒
	int		m_iParamGroupSel;		//当前的参数组索引号
	BOOL	m_bSaveParamOnExit;		//退出程序时保存参数的使能标志
	BOOL	m_bSaveParamOnSwitch;	//切换参数组时保存参数的使能标志
	UINT	m_uCrosshairColor;		//十字线的颜色
	CSdkCallTrace m_DlgLog;			//信息显示窗口
	BOOL	m_bNotOverlayOnSnap;	//抓拍时是否叠加十字线等信息
	tSdkCameraCapbility m_sCameraInfo; //相机特性描述
	//}}AFX_DATA

	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CAdvancedDlg)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:
	HICON m_hIcon;

	void MySetDlgItemText(UINT uItemID, LPCSTR pFmt, ...);

	// Generated message map functions
	//{{AFX_MSG(CAdvancedDlg)
	virtual BOOL OnInitDialog();
	afx_msg void OnSysCommand(UINT nID, LPARAM lParam);
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	afx_msg void OnButtonRoiReset();
	afx_msg void OnSelchangeComboResolution();
	afx_msg void OnButtonSoftTrigOnce();
	afx_msg void OnButtonSnapshot();
	afx_msg void OnButtonAeWinSet();
	afx_msg void OnBtnWbOnce();
	afx_msg void OnBtnWbWinSet();
	afx_msg void OnButtonCustomResSnapshot();
	afx_msg void OnButtonBrowseSnapshot();
	afx_msg void OnRadioTriggerMode();
	afx_msg void OnChangeEdtTriggerCount();
	afx_msg void OnChangeEdtTriggerDelay();
	afx_msg void OnSetCrosshair();
	afx_msg void OnRadioExposureMode();
	afx_msg void OnChkAntiFlick();
	afx_msg void OnAntiFilckhz();
	afx_msg void OnCheckDisplayAeWin();
	afx_msg void OnHScroll(UINT nSBCode, UINT nPos, CScrollBar* pScrollBar);
	afx_msg void OnCheckNoiseReduce();
	afx_msg void OnButtonLutLoadCustom();
	afx_msg void OnButtonEditLut();
	afx_msg void OnSelchangeComboPresetLut();
	afx_msg void OnRadioLutMode();
	afx_msg void OnRadioWbMode();

#ifdef _WIN64
	afx_msg void OnTimer(UINT_PTR nIDEvent); 
#else
	afx_msg void OnTimer(UINT nIDEvent);
#endif
	
	afx_msg void OnClose();
	afx_msg void OnButtonSaveParam();
	afx_msg void OnButtonLoadDefault();
	afx_msg void OnButtonLoadParamFromFile();
	afx_msg void OnRadioChangeParamGroup();
	afx_msg void OnCheckDisWbWin();
	afx_msg void OnButtonCustomCt();
	afx_msg void OnSelchangeComboCt();
	afx_msg void OnSelchangeComboFrameSpeed();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()


public:
	CameraHandle    m_hCamera;	//相机的设备句柄|the handle of the camera we use
	tSdkFrameHead   m_sFrInfo;//用于保存当前图像帧的帧头信息

	int	            m_iDispFrameNum;//用于记录当前已经显示的图像帧的数量
	float           m_fDispFps;//显示帧率
	float           m_fCapFps;//捕获帧率
    tSdkFrameStatistic  m_sFrameCount;
    tSdkFrameStatistic  m_sFrameLast;
    int		        m_iTimeLast;
	 
	BYTE*           m_pFrameBuffer;//用于将原始图像数据转换为RGB的缓冲区（采集线程中）
	BOOL	        m_bPause;//是否暂停图像

    float           m_fAnalogGainStep;//相机亮度增益调节的步进值,不同型号的相机，该值不一样
	UINT            m_uiMaxExpTime;//相机曝光的时间的最长值。单位为 行,不同型号的相机，该值不一样

	UINT            m_threadID;//图像抓取线程的ID
	HANDLE          m_hDispThread;//图像抓取线程的句柄
	BOOL            m_bExit;//用来通知图像抓取线程结束

	double			m_fExpTime;//当前的帧曝光时间，单位为us
	double			m_fExpLineTime;//当前的行曝光时间，单位为us
	INT 			m_iAnalogGain;//当前的信号模拟增益值，步进值为m_fAnalogGainStep

	BYTE*			m_pbImgBuffer;//用来保存原始格式数换成RGB格式后的图像数据（抓拍模式下）
	
	tSdkImageResolution	m_tRoiResolution;//自定义的分辨率（预览模式下）
	tSdkImageResolution m_tRoiResolutionSnapshot;//自定义的分辨率（预览模式下）

    BOOL            m_bMonoSensor;//为TRUE时表示该相机为黑白相机
	//相机初始化
	BOOL InitCamera();

	//初始化控件
	BOOL InitControls(tSdkCameraCapbility * pCameraInfo);

	//更新控件的显示
	VOID UpdateControls();

	//更新曝光控件的显示
	VOID UpdateExposure();

	//检查文件路径是否有效
	BOOL CheckPath();
	
	//递归方式创建一个完整路径
	BOOL mkdirEx(const char* lpPath);
	afx_msg void OnBnClickedAutoSnap();
	BOOL m_bAutoSnapshot;
	afx_msg void OnEnChangeEditExptext();
	afx_msg void OnEnChangeEditGaintext();
	afx_msg void OnBnClickedChkHfip();
	afx_msg void OnBnClickedChkVflip();
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_ADVANCEDDLG_H__4B7A0F2F_62FA_4C67_AB8B_8DE471779F2C__INCLUDED_)
