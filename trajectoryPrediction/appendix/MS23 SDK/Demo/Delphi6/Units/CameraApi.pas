unit CameraApi;
//BIG5 TRANS ALLOWED
interface
uses CameraDefine;
///****************************************************/
// 函数名 	: CameraSdkInit
// 功能描述	: 相机SDK初始化，在调用任何SDK其他接口前，必须
//			  先调用该接口进行初始化。该函数在整个进程运行
//			  期间只需要调用一次。	 
// 参数	    : iLanguageSel 用于选择SDK内部提示信息和界面的语种,
//						   0:表示英文,1:表示中文。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraSdkInit(
    iLanguageSel:Integer
):Integer; stdcall;external 'MVCAMSDK.DLL' name 'CameraSdkInit'


///****************************************************/
// 函数名 	: CameraEnumerateDevice
// 功能描述	: 枚举设备，并建立设备列表。在调用CameraInit
//			  之前，必须调用该函数来获得设备的信息。		
// 参数	    : pCameraList 设备列表数组指针。
//             piNums        设备的个数指针，调用时传入pCameraList
//                            数组的元素个数，函数返回时，保存实际找到的设备个数。
//              注意，piNums指向的值必须初始化，且不超过pCameraList数组元素个数，
//              否则有可能造成内存溢出。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraEnumerateDevice(
    pCameraList:PtSdkCameraDevInfo;
    piNums:PInteger
):Integer; stdcall;external 'MVCAMSDK.DLL'

///*****************************************************/
// 函数名 	: CameraInit
// 功能描述	: 相机初始化。初始化成功后，才能调用任何其他
//			  相机相关的操作接口。		
// 参数	    : pCameraInfo    该相机的设备描述信息，由CameraEnumerateDevice
//							 函数获得。	
//            iParamLoadMode  相机初始化时使用的参数加载方式。-1表示使用上次退出时的参数加载方式。
//            emTeam         初始化时使用的参数组。-1表示加载上次退出时的参数组。
//            pCameraHandle  相机的句柄指针，初始化成功后，该指针
//							 返回该相机的有效句柄，在调用其他相机
//							 相关的操作接口时，都需要传入该句柄，主要
//							 用于多相机之间的区分。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///*****************************************************/
function CameraInit(
    pCameraInfo:PtSdkCameraDevInfo;
    iParamLoadMode:Integer;
    emTeam:Integer;
    pCameraHandle:PCameraHandle
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraSetCallbackFunction
// 功能描述	: 设置图像捕获的回调函数。当捕获到新的图像数据帧时，
//			  pCallBack所指向的回调函数就会被调用。	
// 参数	    : hCamera	相机的句柄，由CameraInit函数获得。
//            pCallBack 回调函数指针。
//            pContext  回调函数的附加参数，在回调函数被调用时
//						该附加参数会被传入，可以为NULL。多用于
//						多个相机时携带附加信息。
//            pCallbackOld  用于保存当前的回调函数。可以为NULL。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraSetCallbackFunction(
    hCamera:CameraHandle;
    pCallBack:CAMERA_SNAP_PROC;
    pContext:Pointer;
    pCallbackOld:CAMERA_SNAP_PROC
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraUnInit
// 功能描述	: 相机反初始化。释放资源。
// 参数	    : hCamera 相机的句柄，由CameraInit函数获得。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraUnInit(
    hCamera:CameraHandle
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraGetInformation
// 功能描述	: 获得相机的描述信息
// 参数	    : hCamera 相机的句柄，由CameraInit函数获得。
//            pbuffer 指向相机描述信息指针的指针。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraGetInformation(
    hCamera:CameraHandle;
    pbuffer:Ppchar
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraImageProcess
// 功能描述	: 将获得的相机原始输出图像数据进行处理，叠加饱和度、
//			  颜色增益和校正、降噪等处理效果，最后得到RGB888
//			  格式的图像数据。	
// 参数	    : hCamera  相机的句柄，由CameraInit函数获得。
//            pbyIn	   输入图像数据的缓冲区地址，不能为NULL。	
//            pbyOut   处理后图像输出的缓冲区地址，不能为NULL。
//            pFrInfo  输入图像的帧头信息，处理完成后，帧头信息
//					   中的图像格式uiMediaType会随之改变。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraImageProcess(
    hCamera:CameraHandle; 
    pbyIn:PByte;
    pbyOut:PByte;
    pFrInfo:PtSdkFrameHead
):Integer; stdcall;external 'MVCAMSDK.DLL'


///*****************************************************/
// 函数名   : CameraImageProcessEx
// 功能描述 : 将获得的相机原始输出图像数据进行处理，叠加饱和度、
//            颜色增益和校正、降噪等处理效果，最后得到RGB888
//            格式的图像数据。  
// 参数     : hCamera      相机的句柄，由CameraInit函数获得。
//            pbyIn      输入图像数据的缓冲区地址，不能为NULL。 
//            pbyOut        处理后图像输出的缓冲区地址，不能为NULL。
//            pFrInfo       输入图像的帧头信息，处理完成后，帧头信息
//            uOutFormat    处理完后图像的输出格式可以是CAMERA_MEDIA_TYPE_MONO8 CAMERA_MEDIA_TYPE_RGB CAMERA_MEDIA_TYPE_RGBA8的其中一种。
//                          pbyIn对应的缓冲区大小，必须和uOutFormat指定的格式相匹配。
//            uReserved     预留参数，必须设置为0     
//                     中的图像格式uiMediaType会随之改变。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///*****************************************************/
function  CameraImageProcessEx(
    hCamera:CameraHandle; 
    pbyIn:PByte; 
    pbyOut:PByte;
    pFrInfo:PtSdkFrameHead;
    uOutFormat:Integer;
    uReserved:Integer
):Integer; stdcall;external 'MVCAMSDK.DLL'


///****************************************************/
// 函数名 	: CameraDisplayInit
// 功能描述	: 初始化SDK内部的显示模块。在调用CameraDisplayRGB24
//			  前必须先调用该函数初始化。如果您在二次开发中，
//			  使用自己的方式进行图像显示(不调用CameraDisplayRGB24)，
//			  则不需要调用本函数。	
// 参数	    : hCamera		相机的句柄，由CameraInit函数获得。
//            hWndDisplay	显示窗口的句柄，一般为窗口的m_hWnd成员。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraDisplayInit(
    hCamera:CameraHandle;
    hWndDisplay:HWND
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraDisplayRGB24
// 功能描述	: 显示图像。必须调用过CameraDisplayInit进行
//			  初始化才能调用本函数。	
// 参数	    : hCamera  相机的句柄，由CameraInit函数获得。
//            pbyRGB24 图像的数据缓冲区，RGB888格式。
//            pFrInfo  图像的帧头信息。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraDisplayRGB24(
    hCamera:CameraHandle;
    pbyRGB24:PByte;
    pFrInfo:PtSdkFrameHead
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraSetDisplayMode
// 功能描述	: 设置显示的模式。必须调用过CameraDisplayInit
//			  进行初始化才能调用本函数。
// 参数	    : hCamera  相机的句柄，由CameraInit函数获得。
//            iMode    显示模式，DISPLAYMODE_SCALE或者
//					   DISPLAYMODE_REAL,具体参见CameraDefine.pas单元
//					   中emSdkDisplayMode的定义。		 
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraSetDisplayMode(
    hCamera:CameraHandle;
    iMode:Integer
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraSetDisplayOffset
// 功能描述	: 设置显示的起始偏移值。仅当显示模式为DISPLAYMODE_REAL
//			  时有效。例如显示控件的大小为320X240，而图像的
//			  的尺寸为640X480，那么当iOffsetX = 160,iOffsetY = 120时
//			  显示的区域就是图像的居中320X240的位置。必须调用过
//			  CameraDisplayInit进行初始化才能调用本函数。
// 参数	    : hCamera   相机的句柄，由CameraInit函数获得。
//            iOffsetX  偏移的X坐标。
//            iOffsetY  偏移的Y坐标。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraSetDisplayOffset(
    hCamera:CameraHandle;
    iOffsetX:Integer;
    iOffsetY:Integer
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraSetDisplaySize
// 功能描述	: 设置显示控件的尺寸。必须调用过
//			  CameraDisplayInit进行初始化才能调用本函数。
// 参数	    : hCamera   相机的句柄，由CameraInit函数获得。
//            iWidth    宽度
//            iHeight   高度
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraSetDisplaySize(
    hCamera:CameraHandle; 
    iWidth:Integer;
    iHeight:Integer
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraGetImageBuffer
// 功能描述	: 获得一帧图像数据。为了提高效率，SDK在图像抓取时采用了零拷贝机制，
//			  CameraGetImageBuffer实际获得是内核中的一个缓冲区地址，
//			  该函数成功调用后，必须调用CameraReleaseImageBuffer释放由
//			  CameraGetImageBuffer得到的缓冲区,以便让内核继续使用
//			  该缓冲区。	
// 参数	    : hCamera	  相机的句柄，由CameraInit函数获得。
//            pFrameInfo  图像的帧头信息指针。
//            pbyBuffer   指向图像的数据的缓冲区指针。由于
//						  采用了零拷贝机制来提高效率，因此
//						  这里使用了一个指向指针的指针。
//            UINT wTimes 抓取图像的超时时间。单位毫秒。在
//						  wTimes时间内还未获得图像，则该函数
//						  会返回超时信息。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraGetImageBuffer(
    hCamera:CameraHandle;
    pFrameInfo:PtSdkFrameHead;
    pbyBuffer:Ppbyte;
    wTimes:Integer
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraSnapToBuffer
// 功能描述	: 抓拍一张图像到缓冲区中。相机会进入抓拍模式，并且
//			  自动切换到抓拍模式的分辨率进行图像捕获。然后将
//			  捕获到的数据保存到缓冲区中。
//			  该函数成功调用后，必须调用CameraReleaseImageBuffer
//			  释放由CameraSnapToBuffer得到的缓冲区。具体请参考
//			  CameraGetImageBuffer函数的功能描述部分。	
// 参数	    : hCamera	  相机的句柄，由CameraInit函数获得。
//            pFrameInfo  指针，返回图像的帧头信息。
//            pbyBuffer   指向指针的指针，用来返回图像缓冲区的地址。
//            uWaitTimeMs 超时时间，单位毫秒。在该时间内，如果仍然没有
//						  成功捕获的数据，则返回超时信息。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraSnapToBuffer(
    hCamera:CameraHandle;
    pFrameInfo:PtSdkFrameHead;
    pbyBuffer:Ppbyte;
    uWaitTimeMs:Cardinal
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraReleaseImageBuffer
// 功能描述	: 释放由CameraGetImageBuffer获得的缓冲区。
// 参数	    : hCamera	  相机的句柄，由CameraInit函数获得。
//            pbyBuffer	  由CameraGetImageBuffer获得的缓冲区地址。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraReleaseImageBuffer(
    hCamera:CameraHandle; 
    pbyBuffer:PByte
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraPlay
// 功能描述	: 让SDK进入工作模式，开始接收来自相机发送的图像
//			  数据。如果当前相机是触发模式，则需要接收到
//			  触发帧以后才会更新图像。
// 参数	    : hCamera	  相机的句柄，由CameraInit函数获得。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraPlay(
    hCamera:CameraHandle
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraPause
// 功能描述	: 让SDK进入暂停模式，不接收来自相机的图像数据，
//			  同时也会发送命令让相机暂停输出，释放传输带宽。
//			  暂停模式下，可以对相机的参数进行配置，并立即生效。	
// 参数	    : hCamera	  相机的句柄，由CameraInit函数获得。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraPause(
    hCamera:CameraHandle
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraStop
// 功能描述	: 让SDK进入停止状态，一般是反初始化时调用该函数，
//			  该函数被调用，不能再对相机的参数进行配置。
// 参数	    : hCamera	  相机的句柄，由CameraInit函数获得。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraStop(
    hCamera:CameraHandle
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraInitRecord
// 功能描述	: 初始化一次录像。
// 参数	    : hCamera	  相机的句柄，由CameraInit函数获得。
//            iFormat	  录像的格式，当前只支持不压缩和MSCV两种方式。	
//						  0:不压缩；1:MSCV方式压缩。
//            pcSavePath  录像文件保存的路径。
//            b2GLimit	  如果为TRUE,则文件大于2G时自动分割。
//            dwQuality   录像的质量因子，越大，则质量越好。范围1到100.
//            iFrameRate  录像的帧率。建议设定的比实际采集帧率大，
//						  这样就不会漏帧。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraInitRecord(
    hCamera:CameraHandle;
    iFormat:Integer;
    pcSavePath:Pchar;
    b2GLimit:LongBool;
    dwQuality:Cardinal;
    iFrameRate:Integer
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraStopRecord
// 功能描述	: 结束本次录像。当CameraInitRecord后，可以通过该函数
//			  来结束一次录像，并完成文件保存操作。
// 参数	    : hCamera	  相机的句柄，由CameraInit函数获得。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraStopRecord(
    hCamera:CameraHandle
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraPushFrame
// 功能描述	: 将一帧数据存入录像流中。必须调用CameraInitRecord
//			  才能调用该函数。CameraStopRecord调用后，不能再调用
//			  该函数。由于我们的帧头信息中携带了图像采集的时间戳
//			  信息，因此录像可以精准的时间同步，而不受帧率不稳定
//			  的影响。
// 参数	    : hCamera			相机的句柄，由CameraInit函数获得。
//            pbyImageBuffer    图像的数据缓冲区，必须是RGB格式。
//            pFrInfo           图像的帧头信息。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraPushFrame(
    hCamera:CameraHandle;
    pbyImageBuffer:PByte;
    pFrInfo:PtSdkFrameHead
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraSaveImage
// 功能描述	: 将图像缓冲区的数据保存成图片文件。
// 参数	    : hCamera		 相机的句柄，由CameraInit函数获得。
//            lpszFileName   图片保存文件完整路径。
//            pbyImageBuffer 图像的数据缓冲区。
//            pFrInfo        图像的帧头信息。
//            byFileType     图像保存的格式。取值范围参见CameraDefine.pas单元
//							 中emSdkFileType的类型定义。目前支持  
//							 BMP、JPG、PNG、RAW四种格式。其中RAW表示
//							 相机输出的原始数据，保存RAW格式文件要求
//							 pbyImageBuffer和pFrInfo是由CameraGetImageBuffer
//							 获得的数据，而且未经CameraImageProcess转换
//							 成BMP格式；反之，如果要保存成BMP、JPG或者
//							 PNG格式，则pbyImageBuffer和pFrInfo是由
//							 CameraImageProcess处理后的RGB格式数据。
//						     具体用法可以参考Advanced的例程。		
//            byQuality      图像保存的质量因子，仅当保存为JPG格式
//						     时该参数有效，范围1到100。其余格式
//                           可以写成0。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraSaveImage(
    hCamera:CameraHandle;
    lpszFileName:Pchar;
    pbyImageBuffer:PByte;
    pFrInfo:PtSdkFrameHead;
    byFileType:Byte;
    byQuality:Byte
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraGetImageResolution
// 功能描述	: 获得当前预览的分辨率。
// 参数	    : hCamera		 相机的句柄，由CameraInit函数获得。
//            psCurVideoSize 结构体指针，用于返回当前的分辨率。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraGetImageResolution(
    hCamera:CameraHandle;
    psCurVideoSize:PtSdkImageResolution
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraSetImageResolution
// 功能描述	: 设置预览的分辨率。
// 参数	    : hCamera		   相机的句柄，由CameraInit函数获得。
//            pImageResolution 结构体指针，用于返回当前的分辨率。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraSetImageResolution(
    hCamera:CameraHandle;
    pImageResolution:PtSdkImageResolution
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraGetMediaType
// 功能描述	: 获得相机当前输出原始数据的格式索引号。
// 参数	    : hCamera		相机的句柄，由CameraInit函数获得。
//            piMediaType   指针，用于返回当前格式类型的索引号。
//							由CameraGetCapability获得相机的属性，
//							在tSdkCameraCapbility结构体中的pMediaTypeDesc
//							成员中，以数组的形式保存了相机支持的格式，
//							piMediaType所指向的索引号，就是该数组的索引号。
//							pMediaTypeDesc[*piMediaType].iMediaType则表示当前格式的 
//							编码。该编码请参见CameraDefine.pas单元中[图像格式定义]部分。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraGetMediaType(
    hCamera:CameraHandle;
    piMediaType:PInteger
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraSetMediaType
// 功能描述	: 设置相机的输出原始数据格式。
// 参数	    : hCamera	  相机的句柄，由CameraInit函数获得。
//            iMediaType  由CameraGetCapability获得相机的属性，
//						  在tSdkCameraCapbility结构体中的pMediaTypeDesc
//						  成员中，以数组的形式保存了相机支持的格式，
//						  iMediaType就是该数组的索引号。
//						  pMediaTypeDesc[iMediaType].iMediaType则表示当前格式的 
//						  编码。该编码请参见CameraDefine.pas单元中[图像格式定义]部分。   
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraSetMediaType(
    hCamera:CameraHandle;
    iMediaType:Integer
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraSetAeState
// 功能描述	: 设置相机曝光的模式。自动或者手动。
// 参数	    : hCamera	  相机的句柄，由CameraInit函数获得。
//            bAeState    TRUE，使能自动曝光；FALSE，停止自动曝光。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraSetAeState(
    hCamera:CameraHandle;
    bAeState:LongBool
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraGetAeState
// 功能描述	: 获得相机当前的曝光模式。
// 参数	    : hCamera	 相机的句柄，由CameraInit函数获得。
//            pAeState   指针，用于返回自动曝光的使能状态。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraGetAeState(
    hCamera:CameraHandle;
    pAeState:PLongBool
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraSetSharpness
// 功能描述	: 设置图像的处理的锐化参数。
// 参数	    : hCamera	 相机的句柄，由CameraInit函数获得。
//            iSharpness 锐化参数。范围由CameraGetCapability
//					     获得，一般是[0,100]，0表示关闭锐化处理。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraSetSharpness(
    hCamera:CameraHandle;
    iSharpness:Integer
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraGetSharpness
// 功能描述	: 获取当前锐化设定值。
// 参数	    : hCamera	  相机的句柄，由CameraInit函数获得。
//            piSharpness 指针，返回当前设定的锐化的设定值。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraGetSharpness(
    hCamera:CameraHandle;
    piSharpness:PInteger
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraSetLutMode
// 功能描述	: 设置相机的查表变换模式LUT模式。
// 参数	    : hCamera	 相机的句柄，由CameraInit函数获得。
//            emLutMode  LUTMODE_PARAM_GEN 表示由伽马和对比度参数动态生成LUT表。
//						 LUTMODE_PRESET    表示使用预设的LUT表。
//						 LUTMODE_USER_DEF  表示使用用户自定的LUT表。
//						 LUTMODE_PARAM_GEN的定义参考CameraDefine.pas单元中emSdkLutMode类型。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraSetLutMode(
    hCamera:CameraHandle;
    emLutMode:Integer
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraGetLutMode
// 功能描述	: 获得相机的查表变换模式LUT模式。
// 参数	    : hCamera	 相机的句柄，由CameraInit函数获得。
//            pemLutMode 指针，返回当前LUT模式。意义与CameraSetLutMode
//						 中emLutMode参数相同。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraGetLutMode(
    hCamera:CameraHandle;
    pemLutMode:PInteger
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraSelectLutPreset
// 功能描述	: 选择预设LUT模式下的LUT表。必须先使用CameraSetLutMode
//			  将LUT模式设置为预设模式。
// 参数	    : hCamera	 相机的句柄，由CameraInit函数获得。
//            iSel		 表的索引号。表的个数由CameraGetCapability
//						 获得。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraSelectLutPreset(
    hCamera:CameraHandle;
    iSel:Integer
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraGetLutPresetSel
// 功能描述	: 获得预设LUT模式下的LUT表索引号。
// 参数	    : hCamera	 相机的句柄，由CameraInit函数获得。
//            piSel      指针，返回表的索引号。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraGetLutPresetSel(
    hCamera:CameraHandle;
    piSel:PInteger
):Integer; stdcall;external 'MVCAMSDK.DLL'

///*****************************************************/
// 函数名   : CameraSetCustomLut
// 功能描述 : 设置自定义的LUT表。必须先使用CameraSetLutMode
//            将LUT模式设置为自定义模式。
// 参数     : hCamera    相机的句柄，由CameraInit函数获得。
//             iChannel 指定要设定的LUT颜色通道，当为LUT_CHANNEL_ALL时，
//                      三个通道的LUT将被同时替换。
//                      参考CameraDefine.h中emSdkLutChannel定义。
//            pLut       指针，指向LUT表的地址。LUT表为无符号短整形数组，数组大小为
//                   4096，分别代码颜色通道从0到4096(12bit颜色精度)对应的映射值。   
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas
//            中错误码的定义。
///*****************************************************/
function CameraSetCustomLut(
    hCamera:CameraHandle;
    iChannel:Integer;
    pLut:PWord
):Integer; stdcall;external 'MVCAMSDK.DLL'

///*****************************************************/
// 函数名   : CameraGetCustomLut
// 功能描述 : 获得当前使用的自定义LUT表。
// 参数     : hCamera    相机的句柄，由CameraInit函数获得。
//             iChannel 指定要获得的LUT颜色通道。当为LUT_CHANNEL_ALL时，
//                      返回红色通道的LUT表。
//                      参考CameraDefine.h中emSdkLutChannel定义。
//            pLut       指针，指向LUT表的地址。LUT表为无符号短整形数组，数组大小为
//                   4096，分别代码颜色通道从0到4096(12bit颜色精度)对应的映射值。   
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas
//            中错误码的定义。
///*****************************************************/
function CameraGetCustomLut(
    hCamera:CameraHandle;
    iChannel:Integer;
    pLut:PWord
):Integer; stdcall;external 'MVCAMSDK.DLL'

///*****************************************************/
// 函数名   : CameraGetCurrentLut
// 功能描述 : 获得相机当前的LUT表，在任何LUT模式下都可以调用,
//            用来直观的观察LUT曲线的变化。
// 参数     : hCamera    相机的句柄，由CameraInit函数获得。
//             iChannel 指定要获得的LUT颜色通道。当为LUT_CHANNEL_ALL时，
//                      返回红色通道的LUT表。
//                      参考CameraDefine.h中emSdkLutChannel定义。
//            pLut       指针，指向LUT表的地址。LUT表为无符号短整形数组，数组大小为
//                   4096，分别代码颜色通道从0到4096(12bit颜色精度)对应的映射值。   
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///*****************************************************/
function CameraGetCurrentLut(
    hCamera:CameraHandle;
    iChannel:Integer;
    pLut:PWord
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraSetWbMode
// 功能描述	: 设置相机白平衡模式。分为手动和自动两种方式。
// 参数	    : hCamera	 相机的句柄，由CameraInit函数获得。
//            bAuto	     TRUE，则表示使能自动模式。
//						 FALSE，则表示使用手动模式，通过调用
//							   CameraSetOnceWB来进行一次白平衡。				
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraSetWbMode(
    hCamera:CameraHandle;
    bAuto:LongBool
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraGetWbMode
// 功能描述	: 获得当前的白平衡模式。
// 参数	    : hCamera	 相机的句柄，由CameraInit函数获得。
//            pbAuto	 指针，返回TRUE表示自动模式，FALSE
//						 为手动模式。	
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraGetWbMode(
    hCamera:CameraHandle;
    pbAuto:PLongBool
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraSetPresetClrTemp
// 功能描述	: 设置色温模式
// 参数	    : hCamera	 相机的句柄，由CameraInit函数获得。
//             iSel     索引号。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraSetPresetClrTemp(
    hCamera:CameraHandle;
    iSel:Integer
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraGetPresetClrTemp
// 功能描述	: 
// 参数	    : hCamera	 相机的句柄，由CameraInit函数获得。
//            int* piSel
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraGetPresetClrTemp(
    hCamera:CameraHandle;
    piSel:PInteger
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraSetOnceWB
// 功能描述	: 在手动白平衡模式下，调用该函数会进行一次白平衡。
//			  生效的时间为接收到下一帧图像数据时。
// 参数	    : hCamera	 相机的句柄，由CameraInit函数获得。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraSetOnceWB(
    hCamera:CameraHandle
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraSetOnceBB
// 功能描述	: 执行一次黑平衡操作。
// 参数	    : hCamera	 相机的句柄，由CameraInit函数获得。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码;请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraSetOnceBB(
    hCamera:CameraHandle
):Integer; stdcall;external 'MVCAMSDK.DLL'


///****************************************************/
// 函数名 	: CameraSetAeTarget
// 功能描述	: 设定自动曝光的亮度目标值。设定范围由CameraGetCapability
//			  函数获得。
// 参数	    : hCamera	 相机的句柄，由CameraInit函数获得。
//            iAeTarget  亮度目标值。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraSetAeTarget(
    hCamera:CameraHandle; 
    iAeTarget:Integer
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraGetAeTarget
// 功能描述	: 获得自动曝光的亮度目标值。
// 参数	    : hCamera	  相机的句柄，由CameraInit函数获得。
//            *piAeTarget 指针，返回目标值。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraGetAeTarget(
    hCamera:CameraHandle; 
    piAeTarget:PInteger
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraSetExposureTime
// 功能描述	: 设置曝光时间。单位为微秒。对于CMOS传感器，其曝光
//			  的单位是按照行来计算的，因此，曝光时间并不能在微秒
//			  级别连续可调。而是会按照整行来取舍。在调用
//			  本函数设定曝光时间后，建议再调用CameraGetExposureTime
//			  来获得实际设定的值。
// 参数	    : hCamera	     相机的句柄，由CameraInit函数获得。
//            fExposureTime  曝光时间，单位微秒。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraSetExposureTime(
    hCamera:CameraHandle; 
    fExposureTime:Double
):Integer; stdcall;external 'MVCAMSDK.DLL'

///*****************************************************/
// 函数名 	: CameraGetExposureLineTime
// 功能描述	: 获得一行的曝光时间。对于CMOS传感器，其曝光
//			  的单位是按照行来计算的，因此，曝光时间并不能在微秒
//			  级别连续可调。而是会按照整行来取舍。这个函数的
//		      作用就是返回CMOS相机曝光一行对应的时间。
// 参数	    : hCamera	 相机的句柄，由CameraInit函数获得。
//            float *pfLineTime 指针，返回一行的曝光时间，单位为微秒。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///*****************************************************/
function CameraGetExposureLineTime(
    hCamera:CameraHandle; 
    pfLineTime:PDouble
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraGetExposureTime
// 功能描述	: 获得相机的曝光时间。请参见CameraSetExposureTime
//			  的功能描述。
// 参数	    : hCamera	       相机的句柄，由CameraInit函数获得。
//            pfExposureTime   指针，返回当前的曝光时间，单位微秒。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraGetExposureTime(
    hCamera:CameraHandle;
    pfExposureTime:PDouble
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraSetAnalogGain
// 功能描述	: 设置相机的图像模拟增益值。该值乘以CameraGetCapability获得
//			  的相机属性结构体中sExposeDesc.fAnalogGainStep，就
//			  得到实际的图像信号放大倍数。
// 参数	    : hCamera	  相机的句柄，由CameraInit函数获得。
//            iAnalogGain 设定的模拟增益值。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraSetAnalogGain(
    hCamera:CameraHandle;
    iAnalogGain:Integer
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraGetAnalogGain
// 功能描述	: 获得图像信号的模拟增益值。参见CameraSetAnalogGain
//			  详细说明。
// 参数	    : hCamera	   相机的句柄，由CameraInit函数获得。
//            piAnalogGain 指针，返回当前的模拟增益值。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraGetAnalogGain(
    hCamera:CameraHandle; 
    piAnalogGain:PInteger
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraSetGain
// 功能描述	: 设置图像的数字增益。设定范围由CameraGetCapability
//			  获得的相机属性结构体中sRgbGainRange成员表述。
//			  实际的放大倍数是设定值/100。
// 参数	    : hCamera  相机的句柄，由CameraInit函数获得。
//            iRGain   红色通道的增益值。	
//            iGGain   绿色通道的增益值。
//            iBGain   蓝色通道的增益值。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraSetGain(
    hCamera:CameraHandle; 
    RGain:Integer; 
    iGGain:Integer; 
    iBGain:Integer
):Integer; stdcall;external 'MVCAMSDK.DLL'


///****************************************************/
// 函数名 	: CameraGetGain
// 功能描述	: 获得图像处理的数字增益。具体请参见CameraSetGain
//			  的功能描述部分。
// 参数	    : hCamera	 相机的句柄，由CameraInit函数获得。
//            piRGain	 指针，返回红色通道的数字增益值。
//            piGGain    指针，返回绿色通道的数字增益值。
//            piBGain    指针，返回蓝色通道的数字增益值。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraGetGain(
    hCamera:CameraHandle; 
    piRGain:PInteger; 
    piGGain:PInteger; 
    piBGain:PInteger
):Integer; stdcall;external 'MVCAMSDK.DLL'


///****************************************************/
// 函数名 	: CameraSetGamma
// 功能描述	: 设定LUT动态生成模式下的Gamma值。设定的值会
//			  马上保存在SDK内部，但是只有当相机处于动态
//			  参数生成的LUT模式时，才会生效。请参考CameraSetLutMode
//			  的函数说明部分。
// 参数	    : hCamera	 相机的句柄，由CameraInit函数获得。
//            iGamma     要设定的Gamma值。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraSetGamma(
    hCamera:CameraHandle; 
    iGamma:Integer
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraGetGamma
// 功能描述	: 获得LUT动态生成模式下的Gamma值。请参考CameraSetGamma
//			  函数的功能描述。
// 参数	    : hCamera	 相机的句柄，由CameraInit函数获得。
//            piGamma    指针，返回当前的Gamma值。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraGetGamma(
    hCamera:CameraHandle; 
    piGamma:PInteger
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraSetContrast
// 功能描述	: 设定LUT动态生成模式下的对比度值。设定的值会
//			  马上保存在SDK内部，但是只有当相机处于动态
//			  参数生成的LUT模式时，才会生效。请参考CameraSetLutMode
//			  的函数说明部分。
// 参数	    : hCamera	 相机的句柄，由CameraInit函数获得。
//            iContrast  设定的对比度值。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraSetContrast(
    hCamera:CameraHandle; 
    iContrast:Integer
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraGetContrast
// 功能描述	: 获得LUT动态生成模式下的对比度值。请参考
//			  CameraSetContrast函数的功能描述。
// 参数	    : hCamera	 相机的句柄，由CameraInit函数获得。
//            piContrast 指针，返回当前的对比度值。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraGetContrast(
    hCamera:CameraHandle; 
    piContrast:PInteger
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraSetSaturation
// 功能描述	: 设定图像处理的饱和度。对黑白相机无效。
//			  设定范围由CameraGetCapability获得。100表示
//			  表示原始色度，不增强。
// 参数	    : hCamera	   相机的句柄，由CameraInit函数获得。
//            iSaturation  设定的饱和度值。 
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraSetSaturation(
    hCamera:CameraHandle; 
    iSaturation:Integer
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraGetSaturation
// 功能描述	: 获得图像处理的饱和度。
// 参数	    : hCamera	   相机的句柄，由CameraInit函数获得。
//            piSaturation 指针，返回当前图像处理的饱和度值。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraGetSaturation(
    hCamera:CameraHandle; 
    piSaturation:PInteger
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraSetMonochrome
// 功能描述	: 设置彩色转为黑白功能的使能。
// 参数	    : hCamera	相机的句柄，由CameraInit函数获得。
//            bEnable   TRUE，表示将彩色图像转为黑白。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraSetMonochrome(
    hCamera:CameraHandle; 
    bEnable:LongBool
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraGetMonochrome
// 功能描述	: 获得彩色转换黑白功能的使能状况。
// 参数	    : hCamera	 相机的句柄，由CameraInit函数获得。
//            pbEnable   指针。返回TRUE表示开启了彩色图像
//						 转换为黑白图像的功能。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraGetMonochrome(
    hCamera:CameraHandle; 
    pbEnable:PLongBool
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraSetInverse
// 功能描述	: 设置彩图像颜色翻转功能的使能。
// 参数	    : hCamera	 相机的句柄，由CameraInit函数获得。
//            bEnable    TRUE，表示开启图像颜色翻转功能，
//						 可以获得类似胶卷底片的效果。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraSetInverse(
    hCamera:CameraHandle; 
    bEnable:LongBool
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraGetInverse
// 功能描述	: 获得图像颜色反转功能的使能状态。
// 参数	    : hCamera	 相机的句柄，由CameraInit函数获得。
//            pbEnable   指针，返回该功能使能状态。 
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraGetInverse(
    hCamera:CameraHandle; 
    pbEnable:PLongBool
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraSetAntiFlick
// 功能描述	: 设置自动曝光时抗频闪功能的使能状态。对于手动
//			  曝光模式下无效。
// 参数	    : hCamera	 相机的句柄，由CameraInit函数获得。
//            bEnable    TRUE，开启抗频闪功能;FALSE，关闭该功能。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraSetAntiFlick(
    hCamera:CameraHandle;
    bEnable:LongBool
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraGetAntiFlick
// 功能描述	: 获得自动曝光时抗频闪功能的使能状态。
// 参数	    : hCamera	 相机的句柄，由CameraInit函数获得。
//            pbEnable   指针，返回该功能的使能状态。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraGetAntiFlick(
    hCamera:CameraHandle; 
    pbEnable:PLongBool
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraGetLightFrequency
// 功能描述	: 获得自动曝光时，消频闪的频率选择。
// 参数	    : hCamera	     相机的句柄，由CameraInit函数获得。
//            piFrequencySel 指针，返回选择的索引号。0:50HZ 1:60HZ
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraGetLightFrequency(
    hCamera:CameraHandle; 
    piFrequencySel:PInteger
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraSetLightFrequency
// 功能描述	: 设置自动曝光时消频闪的频率。
// 参数	    : hCamera	    相机的句柄，由CameraInit函数获得。
//            iFrequencySel 0:50HZ , 1:60HZ 
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraSetLightFrequency(
    hCamera:CameraHandle;
    iFrequencySel:Integer
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraSetFrameSpeed
// 功能描述	: 设定相机输出图像的帧率。相机可供选择的帧率模式由
//			  CameraGetCapability获得的信息结构体中iFrameSpeedDesc
//			  表示最大帧率选择模式个数。
// 参数	    : hCamera	  相机的句柄，由CameraInit函数获得。
//            iFrameSpeed 选择的帧率模式索引号，范围从0到
//						  CameraGetCapability获得的信息结构体中iFrameSpeedDesc - 1	 
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraSetFrameSpeed(
    hCamera:CameraHandle; 
    iFrameSpeed:Integer
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraGetFrameSpeed
// 功能描述	: 获得相机输出图像的帧率选择索引号。具体用法参考
//			  CameraSetFrameSpeed函数的功能描述部分。
// 参数	    : hCamera	   相机的句柄，由CameraInit函数获得。
//            piFrameSpeed 指针，返回选择的帧率模式索引号。 
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraGetFrameSpeed(
    hCamera:CameraHandle; 
    piFrameSpeed:PInteger
):Integer; stdcall;external 'MVCAMSDK.DLL'


///*****************************************************/
// 函数名 	: CameraSetParameterMode
// 功能描述	: 设定参数存取的目标对象。
// 参数	    : hCamera	 相机的句柄，由CameraInit函数获得。
//            iMode	 参数存取的对象。参考CameraDefine.pas
//					中emSdkParameterMode的类型定义。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas
//            中错误码的定义。
///*****************************************************/
function CameraSetParameterMode(
    hCamera:CameraHandle; 
    iTarget:Integer
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraGetParameterMode
// 功能描述	: 
// 参数	    : hCamera	 相机的句柄，由CameraInit函数获得。
//            int* piMode
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraGetParameterMode(
    hCamera:CameraHandle; 
    piMode:PInteger
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraSetParameterMask
// 功能描述	: 设置参数存取的掩码。参数加载和保存时会根据该
//			  掩码来决定各个模块参数的是否加载或者保存。
// 参数	    : hCamera	相机的句柄，由CameraInit函数获得。
//            uMask     掩码。参考CameraDefine.pas单元中PROP_SHEET_INDEX
//						类型定义。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraSetParameterMask(
    hCamera:CameraHandle; 
    uMask:Cardinal
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraSaveParameter
// 功能描述	: 保存当前相机参数到指定的参数组中。相机提供了A,B,C,D
//			  A,B,C,D四组空间来进行参数的保存。	
// 参数	    : hCamera	 相机的句柄，由CameraInit函数获得。
//            iTeam      PARAMETER_TEAM_A 保存到A组中,
//						 PARAMETER_TEAM_B 保存到B组中,
//						 PARAMETER_TEAM_C 保存到C组中,
//						 PARAMETER_TEAM_D 保存到D组中
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraSaveParameter(
    hCamera:CameraHandle; 
    iTeam:Integer
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraReadParameterFromFile
// 功能描述	: 从PC上指定的参数文件中加载参数。我公司相机参数
//			  保存在PC上为.config后缀的文件，位于安装下的
//			  Camera\Configs文件夹中。
// 参数	    : hCamera	 相机的句柄，由CameraInit函数获得。
//            *sFileName 参数文件的完整路径。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraReadParameterFromFile(
    hCamera:CameraHandle;
    sFileName:PChar
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraLoadParameter
// 功能描述	: 加载指定组的参数到相机中。
// 参数	    : hCamera	 相机的句柄，由CameraInit函数获得。
//            iTeam		 PARAMETER_TEAM_A 加载A组参数,
//						 PARAMETER_TEAM_B 加载B组参数,
//						 PARAMETER_TEAM_C 加载C组参数,
//						 PARAMETER_TEAM_D 加载D组参数,
//						 PARAMETER_TEAM_DEFAULT 加载默认参数。 		
//						 类型定义参考CameraDefine.pas单元中emSdkParameterTeam类型
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraLoadParameter(
    hCamera:CameraHandle; 
    iTeam:Integer
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraGetCurrentParameterGroup
// 功能描述	: 获得当前选择的参数组。
// 参数	    : hCamera	 相机的句柄，由CameraInit函数获得。
//            piTeam     指针，返回当前选择的参数组。返回值
//						 参考CameraLoadParameter中iTeam参数。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraGetCurrentParameterGroup(
    hCamera:CameraHandle; 
    piTeam:PInteger
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraSetTransPackLen
// 功能描述	: 设置相机传输图像数据的分包大小。
//			  目前的SDK版本中，该接口仅对GIGE接口相机有效，
//			  用来控制网络传输的分包大小。对于支持巨帧的网卡，
//			  我们建议选择8K的分包大小，可以有效的降低传输
//			  所占用的CPU处理时间。
// 参数	    : hCamera	 相机的句柄，由CameraInit函数获得。
//            iPackSel	 分包长度选择的索引号。分包长度可由
//						 获得相机属性结构体中pPackLenDesc成员表述，
//						 iPackLenDesc成员则表示最大可选的分包模式个数。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraSetTransPackLen(
    hCamera:CameraHandle; 
    iPackSel:Integer
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraGetTransPackLen
// 功能描述	: 获得相机当前传输分包大小的选择索引号。
// 参数	    : hCamera	 相机的句柄，由CameraInit函数获得。
//            piPackSel  指针，返回当前选择的分包大小索引号。
//						 参见CameraSetTransPackLen中iPackSel的
//						 说明。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraGetTransPackLen(
    hCamera:CameraHandle; 
    piPackSel:PInteger
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraIsAeWinVisible
// 功能描述	: 获得自动曝光参考窗口的显示状态。
// 参数	    : hCamera	   相机的句柄，由CameraInit函数获得。
//            pbIsVisible  指针，返回TRUE，则表示当前窗口会
//						   被叠加在图像内容上。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraIsAeWinVisible(
    hCamera:CameraHandle;
    pbIsVisible:PLongBool
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraSetAeWinVisible
// 功能描述	: 设置自动曝光参考窗口的显示状态。当设置窗口状态
//			  为显示，调用CameraImageOverlay后，能够将窗口位置
//			  以矩形的方式叠加在图像上。
// 参数	    : hCamera	  相机的句柄，由CameraInit函数获得。
//            bIsVisible  TRUE，设置为显示；FALSE，不显示。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraSetAeWinVisible(
    hCamera:CameraHandle;
    bIsVisible:LongBool
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraGetAeWindow
// 功能描述	: 获得自动曝光参考窗口的位置。
// 参数	    : hCamera	 相机的句柄，由CameraInit函数获得。
//            piHOff     指针，返回窗口位置左上角横坐标值。
//            piVOff     指针，返回窗口位置左上角纵坐标值。
//            piWidth    指针，返回窗口的宽度。
//            piHeight   指针，返回窗口的高度。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraGetAeWindow(
    hCamera:CameraHandle; 
    piHOff:PInteger; 
    piVOff:PInteger; 
    piWidth:PInteger; 
    piHeight:PInteger
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraSetAeWindow
// 功能描述	: 设置自动曝光的参考窗口。
// 参数	    : hCamera	 相机的句柄，由CameraInit函数获得。
//            iHOff		 窗口左上角的横坐标
//            iVOff      窗口左上角的纵坐标
//            iWidth     窗口的宽度 
//            iHeight    窗口的高度
//			  如果iHOff、iVOff、iWidth、iHeight全部为0，则
//			  窗口设置为每个分辨率下的居中1/2大小。可以随着
//			  分辨率的变化而跟随变化；如果iHOff、iVOff、iWidth、iHeight
//			  所决定的窗口位置范围超出了当前分辨率范围内， 
//		      则自动使用居中1/2大小窗口。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraSetAeWindow(
    hCamera:CameraHandle; 
    iHOff:Integer; 
    iVOff:Integer; 
    iWidth:Integer; 
    iHeight:Integer
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraSetMirror
// 功能描述	: 设置图像镜像操作。镜像操作分为水平和垂直两个方向。
// 参数	    : hCamera	 相机的句柄，由CameraInit函数获得。
//            iDir		 表示镜像的方向。0，表示水平方向；1，表示垂直方向。
//            bEnable	 TRUE，使能镜像;FALSE，禁止镜像
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraSetMirror(
    hCamera:CameraHandle; 
    iDir:Integer; 
    bEnable:LongBool
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraGetMirror
// 功能描述	: 获得图像的镜像状态。
// 参数	    : hCamera	 相机的句柄，由CameraInit函数获得。
//            iDir		 表示要获得的镜像方向。
//						 0，表示水平方向；1，表示垂直方向。
//            pbEnable   指针，返回TRUE，则表示iDir所指的方向
//						 镜像被使能。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraGetMirror(
    hCamera:CameraHandle; 
    iDir:Integer; 
    pbEnable:PLongBool
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraGetWbWindow
// 功能描述	: 获得白平衡参考窗口的位置。
// 参数	    : hCamera	 相机的句柄，由CameraInit函数获得。
//            PiHOff	 指针，返回参考窗口的左上角横坐标	。
//            PiVOff     指针，返回参考窗口的左上角纵坐标	。
//            PiWidth    指针，返回参考窗口的宽度。
//            PiHeight   指针，返回参考窗口的高度。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraGetWbWindow(
    hCamera:CameraHandle; 
    PiHOff:PInteger; 
    PiVOff:PInteger; 
    PiWidth:PInteger; 
    PiHeight:PInteger
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraSetWbWindow
// 功能描述	: 设置白平衡参考窗口的位置。
// 参数	    : hCamera	相机的句柄，由CameraInit函数获得。
//            iHOff		参考窗口的左上角横坐标。
//            iVOff     参考窗口的左上角纵坐标。
//            iWidth    参考窗口的宽度。
//            iHeight   参考窗口的高度。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraSetWbWindow(
    hCamera:CameraHandle; 
    iHOff:Integer; 
    iVOff:Integer; 
    iWidth:Integer; 
    iHeigh:Integer
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraIsWbWinVisible
// 功能描述	: 获得白平衡窗口的显示状态。
// 参数	    : hCamera	 相机的句柄，由CameraInit函数获得。
//            pbShow	 指针，返回TRUE，则表示窗口是可见的。	
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraIsWbWinVisible(
    hCamera:CameraHandle;
    pbShow:PLongBool
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraSetWbWinVisible
// 功能描述	: 设置白平衡窗口的显示状态。
// 参数	    : hCamera	 相机的句柄，由CameraInit函数获得。
//            bShow      TRUE，则表示设置为可见。在调用
//						 CameraImageOverlay后，图像内容上将以矩形
//						 的方式叠加白平衡参考窗口的位置。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraSetWbWinVisible(
    hCamera:CameraHandle; 
    bShow:LongBool
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraImageOverlay
// 功能描述	: 将输入的图像数据上叠加十字线、白平衡参考窗口、
//			  自动曝光参考窗口等图形。只有设置为可见状态的
//			  十字线和参考窗口才能被叠加上。
//			  注意，该函数的输入图像必须是RGB格式。
// 参数	    : hCamera	 相机的句柄，由CameraInit函数获得。
//            pRgbBuffer 图像数据缓冲区。
//            pFrInfo    图像的帧头信息。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraImageOverlay(
    hCamera:CameraHandle;
    pRgbBuffer:PByte;
    pFrInfo:PtSdkFrameHead
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraSetCrossLine
// 功能描述	: 设置指定十字线的参数。
// 参数	    : hCamera	 相机的句柄，由CameraInit函数获得。
//            iLine		 表示要设置第几条十字线的状态。范围为[0,8]，共9条。		 
//            x          十字线中心位置的横坐标值。
//            y			 十字线中心位置的纵坐标值。
//            uColor     十字线的颜色，格式为(R|(G<<8)|(B<<16))
//            bVisible   十字线的显示状态。TRUE，表示显示。
//						 只有设置为显示状态的十字线，在调用
//						 CameraImageOverlay后才会被叠加到图像上。			
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraSetCrossLine(
    hCamera:CameraHandle; 
    iLine:Integer; 
    x:Integer;
    y:Integer;
    uColor:Cardinal;
    bVisible:LongBool
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraGetCrossLine
// 功能描述	: 获得指定十字线的状态。
// 参数	    : hCamera	 相机的句柄，由CameraInit函数获得。
//            iLine		 表示要获取的第几条十字线的状态。范围为[0,8]，共9条。	 
//            px		 指针，返回该十字线中心位置的横坐标。
//            py		 指针，返回该十字线中心位置的横坐标。
//            pcolor     指针，返回该十字线的颜色，格式为(R|(G<<8)|(B<<16))。
//            pbVisible  指针，返回TRUE，则表示该十字线可见。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraGetCrossLine(
    hCamera:CameraHandle; 
    iLine:Integer;
    px:PInteger;
    py:PInteger;
    pcolor:PCardinal;
    pbVisible:PLongBool
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraGetCapability
// 功能描述	: 获得相机的特性描述结构体。该结构体中包含了相机
//			  可设置的各种参数的范围信息。决定了相关函数的参数
//			  返回，也可用于动态创建相机的配置界面。
// 参数	    : hCamera	  相机的句柄，由CameraInit函数获得。
//            pCameraInfo 指针，返回该相机特性描述的结构体。
//	                      tSdkCameraCapbility在CameraDefine.pas单元中定义。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraGetCapability(
    hCamera:CameraHandle; 
    pCameraInfo:PtSdkCameraCapbility
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraWriteSN
// 功能描述	: 设置相机的序列号。我公司相机序列号分为3级。
//			  0级的是我公司自定义的相机序列号，出厂时已经
//			  设定好，1级和2级留给二次开发使用。每级序列
//			  号长度都是32个字节。
// 参数	    : hCamera  相机的句柄，由CameraInit函数获得。
//            pbySN	   序列号的缓冲区。	
//            iLevel   要设定的序列号级别，只能是1或者2。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraWriteSN(
    hCamera:CameraHandle; 
    pbySN:PByte; 
    iLevel:Integer
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraReadSN
// 功能描述	: 读取相机指定级别的序列号。序列号的定义请参考
//		      CameraWriteSN函数的功能描述部分。
// 参数	    : hCamera	 相机的句柄，由CameraInit函数获得。
//            pbySN		 序列号的缓冲区。
//            iLevel     要读取的序列号级别。只能是1和2。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraReadSN(
    hCamera:CameraHandle; 
    pbySN:PByte; 
    iLevel:Integer
):Integer; stdcall;external 'MVCAMSDK.DLL'
///****************************************************/
// 函数名 	: CameraSetTriggerDelayTime
// 功能描述	: 设置硬件触发模式下的触发延时时间，单位微秒。
//			  当硬触发信号来临后，经过指定的延时，再开始采集
//			  图像。仅部分型号的相机支持该功能。具体请查看
//			  产品说明书。
// 参数	    : hCamera	   相机的句柄，由CameraInit函数获得。
//            uDelayTimeUs 硬触发延时。单位微秒。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraSetTriggerDelayTime(
    hCamera:CameraHandle; 
    uDelayTimeUs:Cardinal
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraGetTriggerDelayTime
// 功能描述	: 获得当前设定的硬触发延时时间。
// 参数	    : hCamera	    相机的句柄，由CameraInit函数获得。
//            puDelayTimeUs 指针，返回延时时间，单位微秒。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraGetTriggerDelayTime(
    hCamera:CameraHandle; 
    puDelayTimeUs:PCardinal
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraSetTriggerCount
// 功能描述	: 设置触发模式下的触发帧数。对软件触发和硬件触发
//			  模式都有效。默认为1帧，即一次触发信号采集一帧图像。
// 参数	    : hCamera	相机的句柄，由CameraInit函数获得。
//            iCount    一次触发采集的帧数。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraSetTriggerCount(
    hCamera:CameraHandle; 
    iCount:Integer
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraGetTriggerCount
// 功能描述	: 获得一次触发的帧数。
// 参数	    : hCamera	 相机的句柄，由CameraInit函数获得。
//            INT* piCount
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraGetTriggerCount(
    hCamera:CameraHandle; 
    piCount:PInteger
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraSoftTrigger
// 功能描述	: 执行一次软触发。执行后，会触发由CameraSetTriggerCount
//		      指定的帧数。
// 参数	    : hCamera	 相机的句柄，由CameraInit函数获得。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraSoftTrigger(
    hCamera:CameraHandle
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraSetTriggerMode
// 功能描述	: 设置相机的触发模式。
// 参数	    : hCamera	 相机的句柄，由CameraInit函数获得。
//            iModeSel   模式选择索引号。可设定的模式由
//						 CameraGetCapability函数获取。请参考
//					     CameraDefine.pas单元中tSdkCameraCapbility的定义。
//						 一般情况，0表示连续采集模式；1表示
//						 软件触发模式；2表示硬件触发模式。	
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraSetTriggerMode(
    hCamera:CameraHandle; 
    iModeSel:Integer
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraGetTriggerMode
// 功能描述	: 获得相机的触发模式。
// 参数	    : hCamera	 相机的句柄，由CameraInit函数获得。
//            piModeSel  指针，返回当前选择的相机触发模式的索引号。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraGetTriggerMode(
    hCamera:CameraHandle;
    piModeSel:PInteger
):Integer; stdcall;external 'MVCAMSDK.DLL'

    
///*****************************************************/
// 函数名   : CameraSetStrobeMode
// 功能描述 : 设置IO引脚端子上的STROBE信号。该信号可以做闪光灯控制，也可以做外部机械快门控制。
// 参数     : hCamera 相机的句柄，由CameraInit函数获得。
//             iMode   当为STROBE_SYNC_WITH_TRIG_AUTO      和触发信号同步，触发后，相机进行曝光时，自动生成STROBE信号。
//                                                         此时，有效极性可设置(CameraSetStrobePolarity)。
//                     当为STROBE_SYNC_WITH_TRIG_MANUAL时，和触发信号同步，触发后，STROBE延时指定的时间后(CameraSetStrobeDelayTime)，
//                                                         再持续指定时间的脉冲(CameraSetStrobePulseWidth)，
//                                                         有效极性可设置(CameraSetStrobePolarity)。
//                     当为STROBE_ALWAYS_HIGH时，STROBE信号恒为高,忽略其他设置
//                     当为STROBE_ALWAYS_LOW时，STROBE信号恒为低,忽略其他设置
//
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///*****************************************************/
function  CameraSetStrobeMode(
    hCamera:CameraHandle;
    iMode:Integer
):Integer; stdcall;external 'MVCAMSDK.DLL'

///*****************************************************/
// 函数名   : CameraGetStrobeMode
// 功能描述 : 或者当前STROBE信号设置的模式。
// 参数     : hCamera 相机的句柄，由CameraInit函数获得。
//             piMode  指针，返回STROBE_SYNC_WITH_TRIG_AUTO,STROBE_SYNC_WITH_TRIG_MANUAL、STROBE_ALWAYS_HIGH或者STROBE_ALWAYS_LOW。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///*****************************************************/
function  CameraGetStrobeMode(
    hCamera:CameraHandle;
    piMode:PInteger
):Integer; stdcall;external 'MVCAMSDK.DLL'


///*****************************************************/
// 函数名   : CameraSetStrobeDelayTime
// 功能描述 : 当STROBE信号处于STROBE_SYNC_WITH_TRIG时，通过该函数设置其相对触发信号延时时间。
// 参数     : hCamera       相机的句柄，由CameraInit函数获得。
//             uDelayTimeUs  相对触发信号的延时时间，单位为us。可以为0，但不能为负数。 
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///*****************************************************/
function  CameraSetStrobeDelayTime(
    hCamera:CameraHandle;
    uDelayTimeUs:Integer
):Integer; stdcall;external 'MVCAMSDK.DLL'


///*****************************************************/
// 函数名   : CameraGetStrobeDelayTime
// 功能描述 : 当STROBE信号处于STROBE_SYNC_WITH_TRIG时，通过该函数获得其相对触发信号延时时间。
// 参数     : hCamera           相机的句柄，由CameraInit函数获得。
//             upDelayTimeUs     指针，返回延时时间，单位us。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///*****************************************************/
function  CameraGetStrobeDelayTime(
    hCamera:CameraHandle;
    upDelayTimeUs:PInteger
):Integer; stdcall;external 'MVCAMSDK.DLL'


///*****************************************************/
// 函数名   : CameraSetStrobePulseWidth
// 功能描述 : 当STROBE信号处于STROBE_SYNC_WITH_TRIG时，通过该函数设置其脉冲宽度。
// 参数     : hCamera       相机的句柄，由CameraInit函数获得。
//             uTimeUs       脉冲的宽度，单位为时间us。  
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///*****************************************************/
function  CameraSetStrobePulseWidth(
    hCamera:CameraHandle;
    uTimeUs:Integer
):Integer; stdcall;external 'MVCAMSDK.DLL'


///*****************************************************/
// 函数名   : CameraGetStrobePulseWidth
// 功能描述 : 当STROBE信号处于STROBE_SYNC_WITH_TRIG时，通过该函数获得其脉冲宽度。
// 参数     : hCamera   相机的句柄，由CameraInit函数获得。
//             upTimeUs  指针，返回脉冲宽度。单位为时间us。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///*****************************************************/
function  CameraGetStrobePulseWidth(
    hCamera:CameraHandle;
    upTimeUs:PInteger
):Integer; stdcall;external 'MVCAMSDK.DLL'


///*****************************************************/
// 函数名   : CameraSetStrobePolarity
// 功能描述 : 当STROBE信号处于STROBE_SYNC_WITH_TRIG时，通过该函数设置其有效电平的极性。默认为高有效，当触发信号到来时，STROBE信号被拉高。
// 参数     : hCamera   相机的句柄，由CameraInit函数获得。
//             iPolarity STROBE信号的极性，0为低电平有效，1为高电平有效。默认为高电平有效。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///*****************************************************/
function  CameraSetStrobePolarity(
    hCamera:CameraHandle;
    uPolarity:Integer
):Integer; stdcall;external 'MVCAMSDK.DLL'


///*****************************************************/
// 函数名   : CameraGetStrobePolarity
// 功能描述 : 获得相机当前STROBE信号的有效极性。默认为高电平有效。
// 参数     : hCamera       相机的句柄，由CameraInit函数获得。
//             ipPolarity    指针，返回STROBE信号当前的有效极性。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///*****************************************************/
function  CameraGetStrobePolarity(
    hCamera:CameraHandle;
    upPolarity:PInteger
):Integer; stdcall;external 'MVCAMSDK.DLL'


///*****************************************************/
// 函数名   : CameraSetExtTrigSignalType
// 功能描述 : 设置相机外触发信号的种类。上边沿、下边沿、或者高、低电平方式。
// 参数     : hCamera   相机的句柄，由CameraInit函数获得。
//             iType     外触发信号种类，返回值参考CameraDefine.h中
//                       emExtTrigSignal类型定义。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///*****************************************************/
function  CameraSetExtTrigSignalType(
    hCamera:CameraHandle;
    iType:Integer
):Integer; stdcall;external 'MVCAMSDK.DLL'


///*****************************************************/
// 函数名   : CameraGetExtTrigSignalType
// 功能描述 : 获得相机当前外触发信号的种类。
// 参数     : hCamera   相机的句柄，由CameraInit函数获得。
//             ipType    指针，返回外触发信号种类，返回值参考CameraDefine.h中
//                       emExtTrigSignal类型定义。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///*****************************************************/
function  CameraGetExtTrigSignalType(
    hCamera:CameraHandle;
    ipType:PInteger
):Integer; stdcall;external 'MVCAMSDK.DLL'


///*****************************************************/
// 函数名   : CameraSetExtTrigShutterType
// 功能描述 : 设置外触发模式下，相机快门的方式，默认为标准快门方式。
//              部分滚动快门的CMOS相机支持GRR方式。
// 参数     : hCamera   相机的句柄，由CameraInit函数获得。
//             iType     外触发快门方式。参考CameraDefine.h中emExtTrigShutterMode类型。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///*****************************************************/
function  CameraSetExtTrigShutterType(
    hCamera:CameraHandle;
    iType:Integer
):Integer; stdcall;external 'MVCAMSDK.DLL'


///*****************************************************/
// 函数名   : CameraSetExtTrigShutterType
// 功能描述 : 获得外触发模式下，相机快门的方式，默认为标准快门方式。
//              部分滚动快门的CMOS相机支持GRR方式。
// 参数     : hCamera   相机的句柄，由CameraInit函数获得。
//             ipType    指针，返回当前设定的外触发快门方式。返回值参考
//                       CameraDefine.h中emExtTrigShutterMode类型。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///*****************************************************/
function  CameraGetExtTrigShutterType(
    hCamera:CameraHandle;
    ipType:PInteger
):Integer; stdcall;external 'MVCAMSDK.DLL'


///*****************************************************/
// 函数名   : CameraSetExtTrigDelayTime
// 功能描述 : 设置外触发信号延时时间，默认为0，单位为微秒。 
//              当设置的值uDelayTimeUs不为0时，相机接收到外触发信号后，将延时uDelayTimeUs个微秒后再进行图像捕获。
// 参数     : hCamera       相机的句柄，由CameraInit函数获得。
//             uDelayTimeUs  延时时间，单位为微秒，默认为0.
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///*****************************************************/
function  CameraSetExtTrigDelayTime(
    hCamera:CameraHandle;
    uDelayTimeUs:Integer
):Integer; stdcall;external 'MVCAMSDK.DLL'


///*****************************************************/
// 函数名   : CameraGetExtTrigDelayTime
// 功能描述 : 获得设置的外触发信号延时时间，默认为0，单位为微秒。 
// 参数     : hCamera   相机的句柄，由CameraInit函数获得。
//            UINT* upDelayTimeUs
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///*****************************************************/
function  CameraGetExtTrigDelayTime(
    hCamera:CameraHandle;
    upDelayTimeUs:PInteger
):Integer; stdcall;external 'MVCAMSDK.DLL'


///*****************************************************/
// 函数名   : CameraSetExtTrigJitterTime
// 功能描述 : 设置相机外触发信号的消抖时间。默认为0，单位为微秒。
// 参数     : hCamera   相机的句柄，由CameraInit函数获得。
//            UINT uTimeUs
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///*****************************************************/
function  CameraSetExtTrigJitterTime(
    hCamera:CameraHandle;
    uTimeUs:Integer
):Integer; stdcall;external 'MVCAMSDK.DLL'

///*****************************************************/
// 函数名   : CameraGetExtTrigJitterTime
// 功能描述 : 获得设置的相机外触发消抖时间，默认为0.单位为微妙
// 参数     : hCamera   相机的句柄，由CameraInit函数获得。
//            UINT* upTimeUs
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///*****************************************************/
function  CameraGetExtTrigJitterTime(
    hCamera:CameraHandle;
    upTimeUs:PInteger
):Integer; stdcall;external 'MVCAMSDK.DLL'


///*****************************************************/
// 函数名   : CameraGetExtTrigCapability
// 功能描述 : 获得相机外触发的属性掩码
// 参数     : hCamera           相机的句柄，由CameraInit函数获得。
//             puCapabilityMask  指针，返回该相机外触发特性掩码，掩码参考CameraDefine.h中
//                               EXT_TRIG_MASK_ 开头的宏定义。   
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///*****************************************************/
function  CameraGetExtTrigCapability(
    hCamera:CameraHandle;
    puCapabilityMask:PInteger
):Integer; stdcall;external 'MVCAMSDK.DLL'



///****************************************************/
// 函数名 	: CameraGetResolutionForSnap
// 功能描述	: 获得抓拍模式下的分辨率选择索引号。
// 参数	    : hCamera	       相机的句柄，由CameraInit函数获得。
//            pImageResolution 指针，返回抓拍模式的分辨率。 
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraGetResolutionForSnap(
    hCamera:CameraHandle;
    pImageResolution:PtSdkImageResolution
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraSetResolutionForSnap
// 功能描述	: 设置抓拍模式下相机输出图像的分辨率。
// 参数	    : hCamera	      相机的句柄，由CameraInit函数获得。
//            pImageResolution 如果pImageResolution->iWidth 
//							   和 pImageResolution->iHeight都为0，
//			                   则表示设定为跟随当前预览分辨率。抓
//			                   怕到的图像的分辨率会和当前设定的	
//							   预览分辨率一样。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraSetResolutionForSnap(
    hCamera:CameraHandle;
    pImageResolution:PtSdkImageResolution
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraCustomizeResolution
// 功能描述	: 打开分辨率自定义面板，并通过可视化的方式
//			  来配置一个自定义分辨率。
// 参数	    : hCamera	   相机的句柄，由CameraInit函数获得。
//            pImageCustom 指针，返回自定义的分辨率。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraCustomizeResolution(
    hCamera:CameraHandle;
    pImageCustom:PtSdkImageResolution
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraCustomizeReferWin
// 功能描述	: 打开参考窗口自定义面板。并通过可视化的方式来
//			  获得一个自定义窗口的位置。一般是用自定义白平衡
//			  和自动曝光的参考窗口。
// 参数	    : hCamera	 相机的句柄，由CameraInit函数获得。
//            iWinType   要生成的参考窗口的用途。0,自动曝光参考窗口；
//						 1,白平衡参考窗口。
//            hParent    调用该函数的窗口的句柄。可以为NULL。
//            piHOff     指针，返回自定义窗口的左上角横坐标。
//            piVOff     指针，返回自定义窗口的左上角纵坐标。
//            piWidth    指针，返回自定义窗口的宽度。 
//            piHeight   指针，返回自定义窗口的高度。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraCustomizeReferWin(
    hCamera:CameraHandle;
    iWinType:Integer;
    hParent:HWND; 
    piHOff:PInteger;
    piVOff:PInteger;
    piWidth:PInteger;
    piHeight:PInteger
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraShowSettingPage
// 功能描述	: 设置相机属性配置窗口显示状态。必须先调用CameraCreateSettingPage
//			  成功创建相机属性配置窗口后，才能调用本函数进行
//			  显示。
// 参数	    : hCamera	 相机的句柄，由CameraInit函数获得。
//            bShow		 TRUE，显示;FALSE，隐藏。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraShowSettingPage(
    hCamera:CameraHandle;
    bShow:LongBool
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraCreateSettingPage
// 功能描述	: 创建该相机的属性配置窗口。调用该函数，SDK内部会
//			  帮您创建好相机的配置窗口，省去了您重新开发相机
//			  配置界面的时间。强烈建议使用您使用该函数让
//			  SDK为您创建好配置窗口。
// 参数	    : hCamera	    相机的句柄，由CameraInit函数获得。
//            hParent       应用程序主窗口的句柄。可以为NULL。
//            pWinText      字符串指针，窗口显示的标题栏。
//            pCallbackFunc 窗口消息的回调函数，当相应的事件发生时，
//							pCallbackFunc所指向的函数会被调用，
//							例如切换了参数之类的操作时，pCallbackFunc
//							被回调时，在入口参数处指明了消息类型。
//							这样可以方便您自己开发的界面和我们生成的UI
//							之间进行同步。该参数可以为NULL。	  
//            pCallbackCtx  回调函数的附加参数。可以为NULL。pCallbackCtx
//							会在pCallbackFunc被回调时，做为参数之一传入。
//							您可以使用该参数来做一些灵活的判断。
//            uReserved     预留。必须设置为0。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraCreateSettingPage(
    hCamera:CameraHandle;
    hParent:HWND;
    pWinText:PChar;
    pCallbackFunc:CAMERA_PAGE_MSG_PROC;
    pCallbackCtx:Pointer;
    uReserved:Cardinal
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraSetActiveSettingSubPage
// 功能描述	: 设置相机配置窗口的激活页面。相机配置窗口有多个
//			  子页面构成，该函数可以设定当前哪一个子页面
//			  为激活状态，显示在最前端。
// 参数	    : hCamera	 相机的句柄，由CameraInit函数获得。
//            index      子页面的索引号。参考CameraDefine.pas单元中
//						 PROP_SHEET_INDEX的定义。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraSetActiveSettingSubPage(
    hCamera:CameraHandle;
    index:Integer
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraSpecialControl
// 功能描述	: 相机一些特殊配置所调用的接口，二次开发时一般不需要
//			  调用。
// 参数	    : hCamera	 相机的句柄，由CameraInit函数获得。
//            dwCtrlCode 控制码。
//            dwParam    控制子码，不同的dwCtrlCode时，意义不同。
//            lpData     附加参数。不同的dwCtrlCode时，意义不同。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraSpecialControl(
    hCamera:CameraHandle; 
    dwCtrlCode:Cardinal;
    dwParam:Cardinal;
    lpData:Pointer
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraGetFrameStatistic
// 功能描述	: 获得相机接收帧率的统计信息，包括错误帧和丢帧的情况。
// 参数	    : hCamera	       相机的句柄，由CameraInit函数获得。
//            psFrameStatistic 指针，返回统计信息。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraGetFrameStatistic(
    hCamera:CameraHandle; 
    psFrameStatistic:PtSdkFrameStatistic
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraSetNoiseFilter
// 功能描述	: 设置图像降噪模块的使能状态。
// 参数	    : hCamera	相机的句柄，由CameraInit函数获得。
//            bEnable   TRUE，使能；FALSE，禁止。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraSetNoiseFilter(
    hCamera:CameraHandle;
    bEnable:LongBool
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraGetNoiseFilterState
// 功能描述	: 获得图像降噪模块的使能状态。
// 参数	    : hCamera	 相机的句柄，由CameraInit函数获得。
//            *pEnable   指针，返回状态。TRUE，为使能。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraGetNoiseFilterState(
    hCamera:CameraHandle;
    pEnable:PLongBool
):Integer; stdcall;external 'MVCAMSDK.DLL'

///****************************************************/
// 函数名 	: CameraRstTimeStamp
// 功能描述	: 复位图像采集的时间戳，从0开始。
// 参数	    : hCamera:CameraHandle
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///****************************************************/
function CameraRstTimeStamp(
    hCamera:CameraHandle
):Integer; stdcall;external 'MVCAMSDK.DLL'
 
    
///*****************************************************/
// 函数名   : CameraSaveUserData
// 功能描述 : 将用户自定义的数据保存到相机的非易性存储器中。
//              每个型号的相机可能支持的用户数据区最大长度不一样。
//              可以从设备的特性描述中获取该长度信息。
// 参数     : hCamera    相机的句柄，由CameraInit函数获得。
//            uStartAddr  起始地址，从0开始。
//            pbData      数据缓冲区指针
//            ilen        写入数据的长度，ilen + uStartAddr必须
//                        小于用户区最大长度
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///*****************************************************/
function CameraSaveUserData(
    hCamera:CameraHandle;
    uStartAddr:Cardinal;
    pbData:PByte;
    ilen:Integer
):Integer; stdcall;external 'MVCAMSDK.DLL'

///*****************************************************/
// 函数名   : CameraLoadUserData
// 功能描述 : 从相机的非易性存储器中读取用户自定义的数据。
//              每个型号的相机可能支持的用户数据区最大长度不一样。
//              可以从设备的特性描述中获取该长度信息。
// 参数     : hCamera    相机的句柄，由CameraInit函数获得。
//            uStartAddr  起始地址，从0开始。
//            pbData      数据缓冲区指针，返回读到的数据。
//            ilen        读取数据的长度，ilen + uStartAddr必须
//                        小于用户区最大长度
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///*****************************************************/
function CameraLoadUserData(
    hCamera:CameraHandle;
    uStartAddr:Cardinal;
    pbData:PByte;
    ilen:Integer
):Integer; stdcall;external 'MVCAMSDK.DLL'

///*****************************************************/
// 函数名 	: CameraGetFriendlyName
// 功能描述	: 读取用户自定义的设备昵称。
// 参数	    : hCamera    相机的句柄，由CameraInit函数获得。
//            pName      指针，返回指向0结尾的字符串，
//						 设备昵称不超过32个字节，因此该指针
//						 指向的缓冲区必须大于等于32个字节空间。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///*****************************************************/
function CameraGetFriendlyName(
	hCamera:CameraHandle;
	pName:PByte
):Integer; stdcall;external 'MVCAMSDK.DLL'

///*****************************************************/
// 函数名 	: CameraSetFriendlyName
// 功能描述	: 设置用户自定义的设备昵称。
// 参数	    : hCamera    相机的句柄，由CameraInit函数获得。
//            pName      指针，指向0结尾的字符串，
//						 设备昵称不超过32个字节，因此该指针
//						 指向字符串必须小于等于32个字节空间。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///*****************************************************/
function  CameraSetFriendlyName(
	hCamera:CameraHandle;
	pName:PByte
):Integer; stdcall;external 'MVCAMSDK.DLL'

///*****************************************************/
// 函数名 	: CameraSdkGetVersionString
// 功能描述	: 
// 参数	    : pVersionString 指针，返回SDK版本字符串。
//                            该指针指向的缓冲区大小必须大于
//                            32个字节
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///*****************************************************/
function  CameraSdkGetVersionString(
	pVersionString:PByte
):Integer; stdcall;external 'MVCAMSDK.DLL'

///*****************************************************/
// 函数名 	: CameraCheckFwUpdate
// 功能描述	: 检测固件版本，是否需要升级。
// 参数	    : hCamera 相机的句柄，由CameraInit函数获得。
//            pNeedUpdate 指针，返回固件检测状态，TRUE表示需要更新
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///*****************************************************/
function  CameraCheckFwUpdate(
	hCamera:CameraHandle;
	pNeedUpdate:PLongBool
):Integer; stdcall;external 'MVCAMSDK.DLL'

///*****************************************************/
// 函数名 	: CameraGetFirmwareVision
// 功能描述	: 获得固件版本的字符串
// 参数	    : hCamera 相机的句柄，由CameraInit函数获得。
//            pVersion 必须指向一个大于32字节的缓冲区，
//                      返回固件的版本字符串。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///*****************************************************/
function  CameraGetFirmwareVision(
	hCamera:CameraHandle;
	pVersion:PByte
):Integer; stdcall;external 'MVCAMSDK.DLL'

///*****************************************************/
// 函数名 	: CameraGetEnumInfo
// 功能描述	: 获得指定设备的枚举信息
// 参数	    : hCamera 相机的句柄，由CameraInit函数获得。
//            pCameraInfo 指针，返回设备的枚举信息。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///*****************************************************/
function  CameraGetEnumInfo(
	hCamera:CameraHandle;
	pCameraInfo:PtSdkCameraDevInfo
):Integer; stdcall;external 'MVCAMSDK.DLL'

///*****************************************************/
// 函数名 	: CameraGetInerfaceVersion
// 功能描述	: 获得指定设备接口的版本
// 参数	    : hCamera 相机的句柄，由CameraInit函数获得。
//            pVersion 指向一个大于32字节的缓冲区，返回接口版本字符串。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///*****************************************************/
function  CameraGetInerfaceVersion(
	hCamera:CameraHandle;
	pVersion:PByte
):Integer; stdcall;external 'MVCAMSDK.DLL'

///*****************************************************/
// 函数名 	: CameraSetIOState
// 功能描述	: 设置指定IO的电平状态，IO为输出型IO，相机
//              预留可编程输出IO的个数由tSdkCameraCapbility中
//              iOutputIoCounts决定。
// 参数	    : hCamera 相机的句柄，由CameraInit函数获得。
//            iOutputIOIndex IO的索引号，从0开始。
//            uState 要设定的状态，1为高，0为低
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///*****************************************************/
function  CameraSetIOState(
	hCamera:CameraHandle;
	iOutputIOIndex:Integer;
	uState:Cardinal
):Integer; stdcall;external 'MVCAMSDK.DLL'

///*****************************************************/
// 函数名 	: CameraGetIOState
// 功能描述	: 设置指定IO的电平状态，IO为输入型IO，相机
//              预留可编程输出IO的个数由tSdkCameraCapbility中
//              iInputIoCounts决定。
// 参数	    : hCamera 相机的句柄，由CameraInit函数获得。          
//            iInputIOIndex IO的索引号，从0开始。
//            puState 指针，返回IO状态,1为高，0为低
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///*****************************************************/
function  CameraGetIOState(
	hCamera:CameraHandle;
	iInputIOIndex:Integer;
	puState:PCardinal
):Integer; stdcall;external 'MVCAMSDK.DLL'

///*****************************************************/
// 函数名 	: CameraSetAeAlgorithm
// 功能描述	: 设置自动曝光时选择的算法，不同的算法适用于
//			  不同的场景。
// 参数	    : hCamera 			相机的句柄，由CameraInit函数获得。 
//            iIspProcessor 	选择执行该算法的对象，参考CameraDefine.h
//								emSdkIspProcessor的定义
//            iAeAlgorithmSel   要选择的算法编号。从0开始，最大值由tSdkCameraCapbility
//								中iAeAlmSwDesc和iAeAlmHdDesc决定。	
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///*****************************************************/
function  CameraSetAeAlgorithm(
    hCamera:CameraHandle;
    iIspProcessor:Integer;
    iAeAlgorithmSel:Integer
):Integer; stdcall;external 'MVCAMSDK.DLL'

///*****************************************************/
// 函数名 	: CameraGetAeAlgorithm
// 功能描述	: 获得当前自动曝光所选择的算法
// 参数	    : hCamera 			相机的句柄，由CameraInit函数获得。 
//            iIspProcessor 	选择执行该算法的对象，参考CameraDefine.h
//								emSdkIspProcessor的定义
//            piAeAlgorithmSel  返回当前选择的算法编号。从0开始，最大值由tSdkCameraCapbility
//								中iAeAlmSwDesc和iAeAlmHdDesc决定。	
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///*****************************************************/
function  CameraGetAeAlgorithm(
    hCamera:CameraHandle;
    iIspProcessor:Integer;
    piAlgorithmSel:PInteger
):Integer; stdcall;external 'MVCAMSDK.DLL'

///*****************************************************/
// 函数名 	: CameraSetBayerDecAlgorithm
// 功能描述	: 设置Bayer数据转彩色的算法。
// 参数	    : hCamera 			相机的句柄，由CameraInit函数获得。 
//            iIspProcessor 	选择执行该算法的对象，参考CameraDefine.h
//								emSdkIspProcessor的定义
//            iAlgorithmSel     要选择的算法编号。从0开始，最大值由tSdkCameraCapbility
//								中iBayerDecAlmSwDesc和iBayerDecAlmHdDesc决定。		
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///*****************************************************/
function  CameraSetBayerDecAlgorithm(
    hCamera:CameraHandle;
    iIspProcessor:Integer;
    iAlgorithmSel:Integer
):Integer; stdcall;external 'MVCAMSDK.DLL'

///*****************************************************/
// 函数名 	: CameraGetBayerDecAlgorithm
// 功能描述	: 获得Bayer数据转彩色所选择的算法。
// 参数	    : hCamera 			相机的句柄，由CameraInit函数获得。 
//            iIspProcessor 	选择执行该算法的对象，参考CameraDefine.h
//								emSdkIspProcessor的定义
//            piAlgorithmSel    返回当前选择的算法编号。从0开始，最大值由tSdkCameraCapbility
//								中iBayerDecAlmSwDesc和iBayerDecAlmHdDesc决定。	
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///*****************************************************/
function  CameraGetBayerDecAlgorithm(
    hCamera:CameraHandle;
    iIspProcessor:Integer;
    piAlgorithmSel:PInteger
):Integer; stdcall;external 'MVCAMSDK.DLL'

///*****************************************************/
// 函数名 	: CameraSetIspProcessor
// 功能描述	: 设置图像处理单元的算法执行对象，由PC端或者相机端
//			  来执行算法，当由相机端执行时，会降低PC端的CPU占用率。
// 参数	    : hCamera		相机的句柄，由CameraInit函数获得。 
//            iIspProcessor	参考CameraDefine.h中
//							emSdkIspProcessor的定义。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///*****************************************************/
function  CameraSetIspProcessor(
    hCamera:CameraHandle;
    iIspProcessor:Integer
):Integer; stdcall;external 'MVCAMSDK.DLL'

///*****************************************************/
// 函数名 	: CameraGetIspProcessor
// 功能描述	: 获得图像处理单元的算法执行对象。
// 参数	    : hCamera		 相机的句柄，由CameraInit函数获得。 
//            piIspProcessor 返回选择的对象，返回值参考CameraDefine.h中
//							 emSdkIspProcessor的定义。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///*****************************************************/
function  CameraGetIspProcessor(
    hCamera:CameraHandle;
    piIspProcessor:PInteger
):Integer; stdcall;external 'MVCAMSDK.DLL'

///*****************************************************/
// 函数名 	: CameraSetBlackLevel
// 功能描述	: 设置图像的黑电平基准，默认值为0
// 参数	    : hCamera	  相机的句柄，由CameraInit函数获得。 
//            iBlackLevel 要设定的电平值。范围为0到255。	
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///*****************************************************/
function  CameraSetBlackLevel(
	hCamera:CameraHandle;
	iBlackLevel:Integer
):Integer; stdcall;external 'MVCAMSDK.DLL'

///*****************************************************/
// 函数名 	: CameraGetBlackLevel
// 功能描述	: 获得图像的黑电平基准，默认值为0
// 参数	    : hCamera	   相机的句柄，由CameraInit函数获得。 
//            piBlackLevel 返回当前的黑电平值。范围为0到255。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///*****************************************************/
function  CameraGetBlackLevel(
	hCamera:CameraHandle;
	piBlackLevel:PInteger
):Integer; stdcall;external 'MVCAMSDK.DLL'

///*****************************************************/
// 函数名 	: CameraSetWhiteLevel
// 功能描述	: 设置图像的白电平基准，默认值为255
// 参数	    : hCamera		相机的句柄，由CameraInit函数获得。 
//            iWhiteLevel	要设定的电平值。范围为0到255。	
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///*****************************************************/
function  CameraSetWhiteLevel(
	hCamera:CameraHandle;
	iWhiteLevel:Integer
):Integer; stdcall;external 'MVCAMSDK.DLL'

///*****************************************************/
// 函数名 	: CameraGetWhiteLevel
// 功能描述	: 获得图像的白电平基准，默认值为255
// 参数	    : hCamera	   相机的句柄，由CameraInit函数获得。 
//            piWhiteLevel 返回当前的白电平值。范围为0到255。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///*****************************************************/
function  CameraGetWhiteLevel(
	hCamera:CameraHandle;
	piWhiteLevel:PInteger
):Integer; stdcall;external 'MVCAMSDK.DLL'


///*****************************************************/
// 函数名   : CameraEnumerateDeviceEx
// 功能描述 : 枚举设备，并建立设备列表。在调用CameraInitEx
//            之前，必须调用该函数枚举设备。
// 参数      : 
// 返回值    : 返回设备个数，0表示无。
///*****************************************************/
function CameraEnumerateDeviceEx(
):Integer; stdcall;external 'MVCAMSDK.DLL'

///*****************************************************/
// 函数名 	: CameraInitEx
// 功能描述	: 相机初始化。初始化成功后，才能调用任何其他
//			  相机相关的操作接口。		
// 参数	    : iDeviceIndex    相机的索引号，CameraEnumerateDeviceEx返回相机个数。	
//            iParamLoadMode  相机初始化时使用的参数加载方式。-1表示使用上次退出时的参数加载方式。
//            emTeam         初始化时使用的参数组。-1表示加载上次退出时的参数组。
//            pCameraHandle  相机的句柄指针，初始化成功后，该指针
//							 返回该相机的有效句柄，在调用其他相机
//							 相关的操作接口时，都需要传入该句柄，主要
//							 用于多相机之间的区分。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.pas单元
//            中错误码的定义。
///*****************************************************/
function CameraInitEx(
    iDeviceIndex:Integer;
    iParamLoadMode:Integer;
    emTeam:Integer;
    pCameraHandle:PCameraHandle
):Integer; stdcall;external 'MVCAMSDK.DLL'

    

///*****************************************************/
// 函数名 	: CameraGetImageBufferEx
// 功能描述	: 获得一帧图像数据。该接口获得的图像是经过处理后的RGB格式。该函数调用后，
//			  不需要调用 CameraReleaseImageBuffer 释放，也不要调用free之类的函数释放
//              来释放该函数返回的图像数据缓冲区。
// 参数	    : hCamera	  相机的句柄，由CameraInit函数获得。
//            piWidth    整形指针，返回图像的宽度
//            piHeight   整形指针，返回图像的高度
//            UINT wTimes 抓取图像的超时时间。单位毫秒。在
//						  wTimes时间内还未获得图像，则该函数
//						  会返回超时信息。
// 返回值   : 成功时，返回RGB数据缓冲区的首地址;
//            否则返回0。
///*****************************************************/
function CameraGetImageBufferEx(
    hCamera:CameraHandle;
    piWidth:PInteger;
    piHeight:PInteger;
    wTimes:Integer
):PByte; stdcall;external 'MVCAMSDK.DLL'



//*****************************************************/
// 函数名 	: CameraSetIspOutFormat
// 功能描述	: 设置CameraGetImageBuffer函数的图像处理的输出格式，支持
//              CAMERA_MEDIA_TYPE_MONO8和CAMERA_MEDIA_TYPE_RGB8和CAMERA_MEDIA_TYPE_RGBA8
//              以及CAMERA_MEDIA_TYPE_BGR8、CAMERA_MEDIA_TYPE_BGRA8
//              (在CameraDefine.h中定义)5种，分别对应8位灰度图像和24RGB、32位RGB、24位BGR、32位BGR彩色图像。
//              默认输出是CAMERA_MEDIA_TYPE_BGR8格式。
// 参数	    : hCamera		相机的句柄，由CameraInit函数获得。 
//             uFormat	要设定格式。CAMERA_MEDIA_TYPE_MONO8或者CAMERA_MEDIA_TYPE_RGB8、CAMERA_MEDIA_TYPE_RGBA8	、CAMERA_MEDIA_TYPE_BGR8、CAMERA_MEDIA_TYPE_BGRA8
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.h
//            中错误码的定义。
//*****************************************************/
function CameraSetIspOutFormat(
    hCamera:CameraHandle;
    uFormat:Integer          
):Integer; stdcall;external 'MVCAMSDK.DLL'

//*****************************************************/
// 函数名 	: CameraGetIspOutFormat
// 功能描述	: 获得CameraGetImageBuffer函数图像处理的输出格式，支持
//              CAMERA_MEDIA_TYPE_MONO8和CAMERA_MEDIA_TYPE_RGB8和CAMERA_MEDIA_TYPE_RGBA8
//              以及CAMERA_MEDIA_TYPE_BGR8、CAMERA_MEDIA_TYPE_BGRA8
//              (在CameraDefine.h中定义)5种，分别对应8位灰度图像和24RGB、32位RGB、24位BGR、32位BGR彩色图像。
// 参数	    : hCamera		相机的句柄，由CameraInit函数获得。 
//             puFormat	返回当前设定的格式。AMERA_MEDIA_TYPE_MONO8或者CAMERA_MEDIA_TYPE_RGB8、CAMERA_MEDIA_TYPE_RGBA8	、CAMERA_MEDIA_TYPE_BGR8、CAMERA_MEDIA_TYPE_BGRA8
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.h
//            中错误码的定义。
//*****************************************************/
function CameraGetIspOutFormat(
    hCamera:CameraHandle;
    puFormat:PInteger         
):Integer; stdcall;external 'MVCAMSDK.DLL'

//*****************************************************/
// 函数名 	: CameraGetErrorString
// 功能描述	: 获得错误码对应的描述字符串
// 参数	    : iStatusCode		错误码。(定义于CameraStatus.h中)
// 返回值   : 成功时，返回错误码对应的字符串首地址;
//            否则返回NULL。
//*****************************************************/
function CameraGetErrorString(
    hCamera:CameraHandle
):PByte; stdcall;external 'MVCAMSDK.DLL'


//*****************************************************/
// 函数名 	: CameraSetLedBrightness
// 功能描述	: 设置相机的LED亮度，不带LED的型号，此函数返回错误代码，表示不支持。
// 参数	    : hCamera	   相机的句柄，由CameraInit函数获得。 
//             index      LED灯的索引号，从0开始。如果只有一个可控制亮度的LED，则该参数为0 。
//             uBrightness LED亮度值，范围0到255. 0表示关闭，255最亮。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0)，表示相机连接状态正常;
//            否则返回 非0值，参考CameraStatus.h中错误码的定义。
//*****************************************************/
function CameraSetLedBrightness(
    hCamera:CameraHandle;
    index:Integer;
    uBrightness:Integer
):Integer; stdcall;external 'MVCAMSDK.DLL'

//*****************************************************/
// 函数名 	: CameraGetLedBrightness
// 功能描述	: 获得相机的LED亮度，不带LED的型号，此函数返回错误代码，表示不支持。
// 参数	    : hCamera	   相机的句柄，由CameraInit函数获得。 
//             index      LED灯的索引号，从0开始。如果只有一个可控制亮度的LED，则该参数为0 。
//             uBrightness 指针，返回LED亮度值，范围0到255. 0表示关闭，255最亮。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0)，表示相机连接状态正常;
//            否则返回 非0值，参考CameraStatus.h中错误码的定义。
//*****************************************************/
function CameraGetLedBrightness(
    hCamera:CameraHandle;
    index:Integer;
    uBrightness:PInteger          
):Integer; stdcall;external 'MVCAMSDK.DLL'

//*****************************************************/
// 函数名 	: CameraEnableTransferRoi
// 功能描述	: 使能或者禁止相机的多区域传输功能，不带该功能的型号，此函数返回错误代码，表示不支持。
//              该功能主要用于在相机端将采集的整幅画面切分，只传输指定的多个区域，以提高传输帧率。
//              多个区域传输到PC上后，会自动拼接成整幅画面，没有被传输的部分，会用黑色填充。
// 参数	    : hCamera	    相机的句柄，由CameraInit函数获得。 
//             uEnableMask 区域使能状态掩码，对应的比特位为1表示使能。0为禁止。目前SDK支持4个可编辑区域，index范围为0到3，即bit0 ，bit1，bit2，bit3控制4个区域的使能状态。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0)，表示相机连接状态正常;
//            对于不支持多区域ROI传输的型号，该函数会返回 CAMERA_STATUS_NOT_SUPPORTED(-4) 表示不支持   
//            其它非0值，参考CameraStatus.h中错误码的定义。
//*****************************************************/
function CameraEnableTransferRoi(
    hCamera:CameraHandle;
    uEnableMask:Integer
):Integer; stdcall;external 'MVCAMSDK.DLL'


//*****************************************************/
// 函数名 	: CameraSetTransferRoi
// 功能描述	: 设置相机传输的裁剪区域。在相机端，图像从传感器上被采集后，将会被裁剪成指定的区域来传送，此函数返回错误代码，表示不支持。
// 参数	    : hCamera	   相机的句柄，由CameraInit函数获得。 
//             index      ROI区域的索引号，从0开始。
//             X1,Y1      ROI区域的左上角坐标
//             X2,Y2      ROI区域的右上角坐标
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0)，表示相机连接状态正常;
//            对于不支持多区域ROI传输的型号，该函数会返回 CAMERA_STATUS_NOT_SUPPORTED(-4) 表示不支持   
//            其它非0值，参考CameraStatus.h中错误码的定义。
//*****************************************************/
function CameraSetTransferRoi(
    hCamera:CameraHandle;
		index:Integer;
		X1:Integer;
		Y1:Integer;
		X2:Integer;
		Y2:Integer
):Integer; stdcall;external 'MVCAMSDK.DLL'


//*****************************************************/
// 函数名 	: CameraGetTransferRoi
// 功能描述	: 设置相机传输的裁剪区域。在相机端，图像从传感器上被采集后，将会被裁剪成指定的区域来传送，此函数返回错误代码，表示不支持。
// 参数	    : hCamera	   相机的句柄，由CameraInit函数获得。 
//             index      ROI区域的索引号，从0开始。
//             pX1,pY1      ROI区域的左上角坐标
//             pX2,pY2      ROI区域的右上角坐标
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0)，表示相机连接状态正常;
//            对于不支持多区域ROI传输的型号，该函数会返回 CAMERA_STATUS_NOT_SUPPORTED(-4) 表示不支持   
//            其它非0值，参考CameraStatus.h中错误码的定义。
//*****************************************************/
function CameraGetTransferRoi(
		hCamera:CameraHandle;
		index:Integer;
		pX1:PInteger;
		pY1:PInteger;
		pX2:PInteger;
		pY2:PInteger
):Integer; stdcall;external 'MVCAMSDK.DLL'

//*****************************************************/
// 函数名 	: CameraAlignMalloc
// 功能描述	: 申请一段对齐的内存空间。功能和malloc类似，但
//						是返回的内存是以align指定的字节数对齐的。
// 参数	    : size	   空间的大小。 
//            align    地址对齐的字节数。
// 返回值   : 成功时，返回非0值，表示内存首地址。失败返回NULL。
//*****************************************************/
function CameraAlignMalloc(
        size:Integer;
        align:Integer
):PByte; stdcall;external 'MVCAMSDK.DLL'

//*****************************************************/
// 函数名 	: CameraAlignFree
// 功能描述	: 释放由CameraAlignMalloc函数申请的内存空间。
// 参数	    : membuffer	   由CameraAlignMalloc返回的内存首地址。 
// 返回值   : 无。
//*****************************************************/
procedure CameraAlignFree(
    membuffer:PByte
); stdcall;external 'MVCAMSDK.DLL'

//******************************************************/
// 函数名 	: CameraSetAutoConnect
// 功能描述	: 设置自动使能重连
// 参数	    : hCamera	   相机的句柄，由CameraInit函数获得。 
//			  bEnable	   使能相机重连，当位TRUE时，SDK内部自动检测相机是否掉线，掉线后自己重连。
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0)，表示相机连接状态正常;
//            对于不支持的型号，该函数会返回 CAMERA_STATUS_NOT_SUPPORTED(-4) 表示不支持   
//            其它非0值，参考CameraStatus.h中错误码的定义。
//******************************************************/
function CameraSetAutoConnect(
	hCamera:CameraHandle;
	bEnable:Integer
):Integer; stdcall;external 'MVCAMSDK.DLL'

//******************************************************/
// 函数名 	: CameraGetReConnectCounts
// 功能描述	: 获得相机自动重连的次数，前提是CameraSetAutoConnect 使能相机自动重连功能。默认是使能的。
// 参数	    : hCamera	   相机的句柄，由CameraInit函数获得。 
//			 puCounts	   返回掉线自动重连的次数
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0)，表示相机连接状态正常;
//            对于不支持的型号，该函数会返回 CAMERA_STATUS_NOT_SUPPORTED(-4) 表示不支持   
//            其它非0值，参考CameraStatus.h中错误码的定义。
//******************************************************/
function CameraGetReConnectCounts(
	hCamera:CameraHandle;
	puCounts:PCardinal
):Integer; stdcall;external 'MVCAMSDK.DLL'

//******************************************************/
// 函数名   : CameraEvaluateImageDefinition
// 功能描述 : 图片清晰度评估
// 参数     : hCamera  相机的句柄，由CameraInit函数获得。
//			  iAlgorithSel 使用的评估算法,详见emEvaluateDefinitionAlgorith中的定义
//            pbyIn    输入图像数据的缓冲区地址，不能为NULL。 
//            pFrInfo  输入图像的帧头信息
//			  DefinitionValue 返回的清晰度估值（越大越清晰）
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.h
//            中错误码的定义。
//******************************************************/
function CameraEvaluateImageDefinition(
	hCamera:CameraHandle;
	iAlgorithSel:Integer;
	pbyIn:PByte;
	pFrInfo:PtSdkFrameHead;
	DefinitionValue: PDouble
	):Integer; stdcall;external 'MVCAMSDK.DLL'

//******************************************************/
// 函数名   : CameraDrawText
// 功能描述 : 在输入的图像数据中绘制文字
// 参数     : pRgbBuffer 图像数据缓冲区
//			  pFrInfo 图像的帧头信息
//			  pFontFileName 字体文件名
//			  FontWidth 字体宽度
//			  FontHeight 字体高度
//			  pText 要输出的文字
//			  (Left, Top, Width, Height) 文字的输出矩形
//			  TextColor 文字颜色RGB
//			  uFlags 输出标志,详见emCameraDrawTextFlags中的定义
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.h
//            中错误码的定义。
//******************************************************/
function CameraDrawText(
	pRgbBuffer: PByte;
	pFrInfo: PtSdkFrameHead;
	pFontFileName:Pchar;
	FontWidth:Cardinal;
	FontHeight:Cardinal;
	pText:Pchar;
	Left:Integer;
	Top:Integer;
	Width:Cardinal;
	Height:Cardinal;
	TextColor:Cardinal;
	uFlags:Cardinal
	):Integer; stdcall;external 'MVCAMSDK.DLL'

//******************************************************/
// 函数名   : CameraGigeGetIp
// 功能描述 : 获取GIGE相机的IP地址
// 参数     : pCameraInfo 相机的设备描述信息，可由CameraEnumerateDevice函数获得。 
//			  CamIp 相机IP(注意：必须保证传入的缓冲区大于等于16字节)
//			  CamMask 相机子网掩码(注意：必须保证传入的缓冲区大于等于16字节)
//			  CamGateWay 相机网关(注意：必须保证传入的缓冲区大于等于16字节)
//			  EtIp 网卡IP(注意：必须保证传入的缓冲区大于等于16字节)
//			  EtMask 网卡子网掩码(注意：必须保证传入的缓冲区大于等于16字节)
//			  EtGateWay 网卡网关(注意：必须保证传入的缓冲区大于等于16字节)
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.h
//            中错误码的定义。
//******************************************************/
function CameraGigeGetIp(
		pCameraInfo:PtSdkCameraDevInfo;
		CamIp:Pchar;
		CamMask:Pchar;
		CamGateWay:Pchar;
		EtIp:Pchar;
		EtMask:Pchar;
		EtGateWay:Pchar
	):Integer; stdcall;external 'MVCAMSDK.DLL'

//******************************************************/
// 函数名   : CameraGigeSetIp
// 功能描述 : 设置GIGE相机的IP地址
// 参数     : pCameraInfo 相机的设备描述信息，可由CameraEnumerateDevice函数获得。 
//			  Ip 相机IP(如：192.168.1.100)
//			  SubMask 相机子网掩码(如：255.255.255.0)
//			  GateWay 相机网关(如：192.168.1.1)
//			  bPersistent TRUE: 设置相机为固定IP，FALSE：设置相机自动分配IP（忽略参数Ip, SubMask, GateWay）
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.h
//            中错误码的定义。
//******************************************************/
function CameraGigeSetIp(
	pCameraInfo:PtSdkCameraDevInfo;
	Ip:Pchar;
	SubMask:Pchar;
	GateWay:Pchar;
	bPersistent:Integer
	):Integer; stdcall;external 'MVCAMSDK.DLL'
	
//******************************************************/
// 函数名   : CameraGrabber_CreateFromDevicePage
// 功能描述 : 弹出相机列表让用户选择要打开的相机
// 参数     : 如果函数执行成功返回函数创建的Grabber
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.h
//            中错误码的定义。
//******************************************************/
function CameraGrabber_CreateFromDevicePage(
	Grabber:PPointer
	):Integer; stdcall;external 'MVCAMSDK.DLL'

//******************************************************/
// 函数名   : CameraGrabber_Create
// 功能描述 : 从设备描述信息创建Grabber
// 参数     : Grabber    如果函数执行成功返回函数创建的Grabber对象
//			  pDevInfo	该相机的设备描述信息，由CameraEnumerateDevice函数获得。 
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.h
//            中错误码的定义。
//******************************************************/
function CameraGrabber_Create(
	Grabber:PPointer;
	pDevInfo:PtSdkCameraDevInfo
	):Integer; stdcall;external 'MVCAMSDK.DLL'

//******************************************************/
// 函数名   : CameraGrabber_Destroy
// 功能描述 : 销毁Grabber
// 参数     : Grabber
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.h
//            中错误码的定义。
//******************************************************/
function CameraGrabber_Destroy(
	Grabber:Pointer
	):Integer; stdcall;external 'MVCAMSDK.DLL'

//******************************************************/
// 函数名	: CameraGrabber_SetHWnd
// 功能描述	: 设置预览视频的显示窗口
// 参数		: Grabber
//			  hWnd  窗口句柄
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.h
//            中错误码的定义。
//******************************************************/
function CameraGrabber_SetHWnd(
	Grabber:Pointer;
	hWnd:HWND
	):Integer; stdcall;external 'MVCAMSDK.DLL'

//******************************************************/
// 函数名	: CameraGrabber_StartLive
// 功能描述	: 启动预览
// 参数		: Grabber
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.h
//            中错误码的定义。
//******************************************************/
function CameraGrabber_StartLive(
	Grabber:Pointer
	):Integer; stdcall;external 'MVCAMSDK.DLL'

//******************************************************/
// 函数名	: CameraGrabber_StopLive
// 功能描述	: 停止预览
// 参数		: Grabber
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.h
//            中错误码的定义。
//******************************************************/
function CameraGrabber_StopLive(
	Grabber:Pointer
	):Integer; stdcall;external 'MVCAMSDK.DLL'

//******************************************************/
// 函数名	: CameraGrabber_SaveImage
// 功能描述	: 抓图
// 参数		: Grabber
//			  Image 返回抓取到的图像（需要调用CameraImage_Destroy释放）
//			  TimeOut 超时时间（毫秒）
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.h
//            中错误码的定义。
//******************************************************/
function CameraGrabber_SaveImage(
	Grabber:Pointer;
	Image:PPointer;
	TimeOut:Cardinal
	):Integer; stdcall;external 'MVCAMSDK.DLL'

//******************************************************/
// 函数名	: CameraGrabber_SaveImageAsync
// 功能描述	: 提交一个异步的抓图请求，提交成功后待抓图完成会回调用户设置的完成函数
// 参数		: Grabber
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.h
//            中错误码的定义。
//******************************************************/
function CameraGrabber_SaveImageAsync(
	Grabber:Pointer
	):Integer; stdcall;external 'MVCAMSDK.DLL'

//******************************************************/
// 函数名	: CameraGrabber_SetSaveImageCompleteCallback
// 功能描述	: 设置异步方式抓图的完成函数
// 参数		: Grabber
//			  Callback 当有抓图任务完成时被调用
//			  Context 当Callback被调用时，作为参数传入Callback
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.h
//            中错误码的定义。
//******************************************************/
function CameraGrabber_SetSaveImageCompleteCallback(
	Grabber:Pointer;
	Callback:pfnCameraGrabberSaveImageComplete;
	Context:Pointer
	):Integer; stdcall;external 'MVCAMSDK.DLL'

//******************************************************/
// 函数名	: CameraGrabber_SetFrameListener
// 功能描述	: 设置帧监听函数
// 参数		: Grabber
//			  Listener 监听函数，此函数返回0表示丢弃当前帧
//			  Context 当Listener被调用时，作为参数传入Listener
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.h
//            中错误码的定义。
//******************************************************/
function CameraGrabber_SetFrameListener(
	Grabber:Pointer;
	Listener:pfnCameraGrabberFrameListener;
	Context:Pointer
	):Integer; stdcall;external 'MVCAMSDK.DLL'

//******************************************************/
// 函数名	: CameraGrabber_GetCameraHandle
// 功能描述	: 获取相机句柄
// 参数		: Grabber
//			  hCamera 返回的相机句柄
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.h
//            中错误码的定义。
//******************************************************/
function CameraGrabber_GetCameraHandle(
	Grabber:Pointer;
	hCamera:PCameraHandle
	):Integer; stdcall;external 'MVCAMSDK.DLL'

//******************************************************/
// 函数名	: CameraGrabber_GetStat
// 功能描述	: 获取帧统计信息
// 参数		: Grabber
//			  stat 返回的统计信息
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.h
//            中错误码的定义。
//******************************************************/
function CameraGrabber_GetStat(
	Grabber:Pointer;
	stat:PtSdkGrabberStat
	):Integer; stdcall;external 'MVCAMSDK.DLL'

//******************************************************/
// 函数名	: CameraGrabber_GetCameraDevInfo
// 功能描述	: 获取相机DevInfo
// 参数		: Grabber
//			  DevInfo 返回的相机DevInfo
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.h
//            中错误码的定义。
//******************************************************/
function CameraGrabber_GetCameraDevInfo(
	Grabber:Pointer;
	DevInfo:PtSdkCameraDevInfo
	):Integer; stdcall;external 'MVCAMSDK.DLL'
	
//******************************************************/
// 函数名	: CameraImage_Create
// 功能描述	: 创建一个新的Image
// 参数		: Image
//			  pFrameBuffer 帧数据缓冲区
//			  pFrameHead 帧头
//			  bCopy TRUE: 复制出一份新的帧数据  FALSE: 不复制，直接使用pFrameBuffer指向的缓冲区
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.h
//            中错误码的定义。
//******************************************************/
function CameraImage_Create(
	Image:PPointer;
	pFrameBuffer:PByte;
	pFrameHead:PtSdkFrameHead;
	bCopy:Integer
	):Integer; stdcall;external 'MVCAMSDK.DLL'

//******************************************************/
// 函数名	: CameraImage_Destroy
// 功能描述	: 销毁Image
// 参数		: Image
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.h
//            中错误码的定义。
//******************************************************/
function CameraImage_Destroy(
	Image:Pointer
	):Integer; stdcall;external 'MVCAMSDK.DLL'

//******************************************************/
// 函数名	: CameraImage_GetData
// 功能描述	: 获取Image数据
// 参数		: Image
//			  DataBuffer 图像数据
//			  Head 图像信息
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.h
//            中错误码的定义。
//******************************************************/
function CameraImage_GetData(
	Image:Pointer;
	DataBuffer:Ppbyte;
	Head:PPtSdkFrameHead
	):Integer; stdcall;external 'MVCAMSDK.DLL'

//******************************************************/
// 函数名	: CameraImage_Draw
// 功能描述	: 绘制Image到指定窗口
// 参数		: Image
//			  hWnd 目的窗口
//			  Algorithm 缩放算法  0：快速但质量稍差  1：速度慢但质量好
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.h
//            中错误码的定义。
//******************************************************/
function CameraImage_Draw(
	Image:Pointer;
	hWnd:HWND;
	Algorithm:Integer
	):Integer; stdcall;external 'MVCAMSDK.DLL'

//******************************************************/
// 函数名	: CameraImage_BitBlt
// 功能描述	: 绘制Image到指定窗口（不缩放）
// 参数		: Image
//			  hWnd 目的窗口
//			  xDst,yDst: 目标矩形的左上角坐标
//			  cxDst,cyDst: 目标矩形的宽高
//			  xSrc,ySrc: 图像矩形的左上角坐标
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.h
//            中错误码的定义。
//******************************************************/
function CameraImage_BitBlt(
	Image:Pointer;
	hWnd:HWND;
	xDst:Integer;
	yDst:Integer;
	cxDst:Integer;
	cyDst:Integer;
	xSrc:Integer;
	ySrc:Integer
	):Integer; stdcall;external 'MVCAMSDK.DLL'

//******************************************************/
// 函数名	: CameraImage_SaveAsBmp
// 功能描述	: 以bmp格式保存Image
// 参数		: Image
//			  FileName 文件名
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.h
//            中错误码的定义。
//******************************************************/
function CameraImage_SaveAsBmp(
	Image:Pointer;
	FileName:Pchar
	):Integer; stdcall;external 'MVCAMSDK.DLL'

//******************************************************/
// 函数名	: CameraImage_SaveAsJpeg
// 功能描述	: 以jpg格式保存Image
// 参数		: Image
//			  FileName 文件名
//			  Quality 保存质量(1-100)，100为质量最佳但文件也最大
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.h
//            中错误码的定义。
//******************************************************/
function CameraImage_SaveAsJpeg(
	Image:Pointer;
	FileName:Pchar;
	Quality:Byte
	):Integer; stdcall;external 'MVCAMSDK.DLL'

//******************************************************/
// 函数名	: CameraImage_SaveAsPng
// 功能描述	: 以png格式保存Image
// 参数		: Image
//			  FileName 文件名
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.h
//            中错误码的定义。
//******************************************************/
function CameraImage_SaveAsPng(
	Image:Pointer;
	FileName:Pchar
	):Integer; stdcall;external 'MVCAMSDK.DLL'

//******************************************************/
// 函数名	: CameraImage_SaveAsRaw
// 功能描述	: 保存raw Image
// 参数		: Image
//			  FileName 文件名
//			  Format 0: 8Bit Raw     1: 16Bit Raw
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.h
//            中错误码的定义。
//******************************************************/
function CameraImage_SaveAsRaw(
	Image:Pointer;
	FileName:Pchar;
	Format:Integer
	):Integer; stdcall;external 'MVCAMSDK.DLL'

//******************************************************/
// 函数名	: CameraImage_IPicture
// 功能描述	: 从Image创建一个IPicture
// 参数		: Image
//			  Picture 新创建的IPicture
// 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
//            否则返回非0值的错误码,请参考CameraStatus.h
//            中错误码的定义。
//******************************************************/
function CameraImage_IPicture(
	Image:Pointer;
	NewPic:PPointer
	):Integer; stdcall;external 'MVCAMSDK.DLL'


implementation
//function CameraSdkInit;external 'MVCAMSDK.DLL' name 'CameraSdkInit';


end.
