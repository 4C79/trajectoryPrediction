
#ifdef DLL_EXPORT
#define MVSDK_API extern "C" __declspec(dllexport)
#else
#define MVSDK_API extern "C" __declspec(dllimport)
#endif


MVSDK_API void* __stdcall LoadCameraConfigFile(char *ConfigFile);
MVSDK_API int __stdcall FreeCameraConfigFile(PVOID pConfig);
MVSDK_API int __stdcall RawToBmp(char* RawFile,char* BmpFile,PVOID pConfig);
MVSDK_API int __stdcall RawToJpg(char* RawFile,char* JpgFile,PVOID pConfig);
MVSDK_API int __stdcall RawDecodeInit(INT iWidthMax,INT iHeightMax);
MVSDK_API int __stdcall RawDecodeUnInit();
MVSDK_API int __stdcall UnLoadCameraConfigFile(PVOID pConfig);