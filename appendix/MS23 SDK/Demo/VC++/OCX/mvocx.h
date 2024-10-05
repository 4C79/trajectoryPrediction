// MVOCX.h : 由 Microsoft Visual C++ 创建的 ActiveX 控件包装类的声明

#pragma once

/////////////////////////////////////////////////////////////////////////////
// CMVOCX

class CMVOCX : public CWnd
{
protected:
	DECLARE_DYNCREATE(CMVOCX)
public:
	CLSID const& GetClsid()
	{
		static CLSID const clsid
			= { 0x2D6AC19F, 0x854C, 0x49C2, { 0x83, 0x66, 0xAE, 0x2B, 0xD2, 0x93, 0x88, 0x1F } };
		return clsid;
	}
	virtual BOOL Create(LPCTSTR lpszClassName, LPCTSTR lpszWindowName, DWORD dwStyle,
						const RECT& rect, CWnd* pParentWnd, UINT nID, 
						CCreateContext* pContext = NULL)
	{ 
		return CreateControl(GetClsid(), lpszWindowName, dwStyle, rect, pParentWnd, nID); 
	}

    BOOL Create(LPCTSTR lpszWindowName, DWORD dwStyle, const RECT& rect, CWnd* pParentWnd, 
				UINT nID, CFile* pPersist = NULL, BOOL bStorage = FALSE,
				BSTR bstrLicKey = NULL)
	{ 
		return CreateControl(GetClsid(), lpszWindowName, dwStyle, rect, pParentWnd, nID,
		pPersist, bStorage, bstrLicKey); 
	}

// 特性
public:

// 操作
public:

	void mvCameraPlay()
	{
		InvokeHelper(0x1, DISPATCH_METHOD, VT_EMPTY, NULL, NULL);
	}
	BOOL mvCameraInit(long iDeviceIndex)
	{
		BOOL result;
		static BYTE parms[] = VTS_I4 ;
		InvokeHelper(0x2, DISPATCH_METHOD, VT_BOOL, (void*)&result, parms, iDeviceIndex);
		return result;
	}
	void mvCameraPause()
	{
		InvokeHelper(0x3, DISPATCH_METHOD, VT_EMPTY, NULL, NULL);
	}
	void mvCameraUnInit()
	{
		InvokeHelper(0x4, DISPATCH_METHOD, VT_EMPTY, NULL, NULL);
	}
	void mvCameraSaveBmp(LPCTSTR sFileName)
	{
		static BYTE parms[] = VTS_BSTR ;
		InvokeHelper(0x5, DISPATCH_METHOD, VT_EMPTY, NULL, parms, sFileName);
	}
	void mvCameraSaveJpg(LPCTSTR sFileName)
	{
		static BYTE parms[] = VTS_BSTR ;
		InvokeHelper(0x6, DISPATCH_METHOD, VT_EMPTY, NULL, parms, sFileName);
	}
	void mvCameraStop()
	{
		InvokeHelper(0x7, DISPATCH_METHOD, VT_EMPTY, NULL, NULL);
	}
	void mvCameraShowConfigPage()
	{
		InvokeHelper(0x8, DISPATCH_METHOD, VT_EMPTY, NULL, NULL);
	}
	void mvCameraHideConfigPage()
	{
		InvokeHelper(0x9, DISPATCH_METHOD, VT_EMPTY, NULL, NULL);
	}
	long mvCameraGetImage(long * pWidth, long * pHeight, long bpp)
	{
		long result;
		static BYTE parms[] = VTS_PI4 VTS_PI4 VTS_I4 ;
		InvokeHelper(0xa, DISPATCH_METHOD, VT_I4, (void*)&result, parms, pWidth, pHeight, bpp);
		return result;
	}
	void mvCameraEnablePreview(long bEnable)
	{
		static BYTE parms[] = VTS_I4 ;
		InvokeHelper(0xb, DISPATCH_METHOD, VT_EMPTY, NULL, parms, bEnable);
	}
	void mvCameraSaveBmpEx(LPCTSTR sFileName)
	{
		static BYTE parms[] = VTS_BSTR ;
		InvokeHelper(0xc, DISPATCH_METHOD, VT_EMPTY, NULL, parms, sFileName);
	}
	void mvCameraSaveJpgEx(LPCTSTR sFileName)
	{
		static BYTE parms[] = VTS_BSTR ;
		InvokeHelper(0xd, DISPATCH_METHOD, VT_EMPTY, NULL, parms, sFileName);
	}
	void AboutBox()
	{
		InvokeHelper(DISPID_ABOUTBOX, DISPATCH_METHOD, VT_EMPTY, NULL, NULL);
	}
	long mvCameraSetIoState(long iOutputIoIndex, long iState)
	{
		long result;
		static BYTE parms[] = VTS_I4 VTS_I4 ;
		InvokeHelper(0xe, DISPATCH_METHOD, VT_I4, (void*)&result, parms, iOutputIoIndex, iState);
		return result;
	}
	long mvCameraGetIoState(long iInputIoIndex)
	{
		long result;
		static BYTE parms[] = VTS_I4 ;
		InvokeHelper(0xf, DISPATCH_METHOD, VT_I4, (void*)&result, parms, iInputIoIndex);
		return result;
	}
	long mvSetImageFormat(long iFormat)
	{
		long result;
		static BYTE parms[] = VTS_I4 ;
		InvokeHelper(0x10, DISPATCH_METHOD, VT_I4, (void*)&result, parms, iFormat);
		return result;
	}
	BOOL mvCameraInitEx(LPCTSTR sCameraFriendlyName)
	{
		BOOL result;
		static BYTE parms[] = VTS_BSTR ;
		InvokeHelper(0x11, DISPATCH_METHOD, VT_BOOL, (void*)&result, parms, sCameraFriendlyName);
		return result;
	}
	long mvCameraGetImageSize(long * pWidth, long * pHeight)
	{
		long result;
		static BYTE parms[] = VTS_PI4 VTS_PI4 ;
		InvokeHelper(0x12, DISPATCH_METHOD, VT_I4, (void*)&result, parms, pWidth, pHeight);
		return result;
	}
	long mvCameraSetTriggerMode(long lMode)
	{
		long result;
		static BYTE parms[] = VTS_I4 ;
		InvokeHelper(0x13, DISPATCH_METHOD, VT_I4, (void*)&result, parms, lMode);
		return result;
	}
	long mvCameraGetTriggerMode()
	{
		long result;
		InvokeHelper(0x14, DISPATCH_METHOD, VT_I4, (void*)&result, NULL);
		return result;
	}
	long mvCameraSoftTrigger()
	{
		long result;
		InvokeHelper(0x15, DISPATCH_METHOD, VT_I4, (void*)&result, NULL);
		return result;
	}
	long mvCameraEnableCallBack(long lEnable)
	{
		long result;
		static BYTE parms[] = VTS_I4 ;
		InvokeHelper(0x16, DISPATCH_METHOD, VT_I4, (void*)&result, parms, lEnable);
		return result;
	}
	long mvCameraDisplayImage(long BufferAddr, long ImageWdith, long ImageHeight, long bpp)
	{
		long result;
		static BYTE parms[] = VTS_I4 VTS_I4 VTS_I4 VTS_I4 ;
		InvokeHelper(0x17, DISPATCH_METHOD, VT_I4, (void*)&result, parms, BufferAddr, ImageWdith, ImageHeight, bpp);
		return result;
	}


};
