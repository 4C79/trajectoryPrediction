
// stdafx.h : è÷ĞçÆf¸¾äô`Á§Ç»¸¾äô`Á§¨Û
// ÷áŒëÃİ¶¼ŠpÍ[ËñÏé¶¼ÔØòÛÇ»
// –ƒÓçôÀ³Xõ·Ç»¸¾äô`Á§

#pragma once

#ifndef _SECURE_ATL
#define _SECURE_ATL 1
#endif

#ifndef VC_EXTRALEAN
#define VC_EXTRALEAN            // Ö² Windows ÆY¸œö¹ºø·ßŒæŠpÍ[Ç»ÔIÌ£
#endif

#include "targetver.h"

#define _ATL_CSTRING_EXPLICIT_CONSTRUCTORS      // ÒéÍD CString µÊŠõÉø–fÎµŒë«}ŒdÇ»

// ÊÙë· MFC ÇÚÒéÍD¶¼â±ËñÃİ¶¼ñİÎÂêzç²½÷Ç»¼Á»¿¶m›ŸÇ»¸‘ç¥
#define _AFX_ALL_WARNINGS

#include <afxwin.h>         // MFC ÃéêzàVÁ§î£è÷ĞàVÁ§
#include <afxext.h>         // MFC Èæ—H


#include <afxdisp.h>        // MFC Ú`ĞÛÕÔF



#ifndef _AFX_NO_OLE_SUPPORT
#include <afxdtctl.h>           // MFC ÇÚ Internet Explorer 4 ÊóÆÍ·íÁ§Ç»±EØÊ
#endif
#ifndef _AFX_NO_AFXCMN_SUPPORT
#include <afxcmn.h>             // MFC ÇÚ Windows ÊóÆÍ·íÁ§Ç»±EØÊ
#endif // _AFX_NO_AFXCMN_SUPPORT

#include <afxcontrolbars.h>     // ÷ÕÙç?î£·íÁ§ãğÇ» MFC ±EØÊ









#ifdef _UNICODE
#if defined _M_IX86
#pragma comment(linker,"/manifestdependency:\"type='win32' name='Microsoft.Windows.Common-Controls' version='6.0.0.0' processorArchitecture='x86' publicKeyToken='6595b64144ccf1df' language='*'\"")
#elif defined _M_X64
#pragma comment(linker,"/manifestdependency:\"type='win32' name='Microsoft.Windows.Common-Controls' version='6.0.0.0' processorArchitecture='amd64' publicKeyToken='6595b64144ccf1df' language='*'\"")
#else
#pragma comment(linker,"/manifestdependency:\"type='win32' name='Microsoft.Windows.Common-Controls' version='6.0.0.0' processorArchitecture='*' publicKeyToken='6595b64144ccf1df' language='*'\"")
#endif
#endif


