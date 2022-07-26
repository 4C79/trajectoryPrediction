unit CameraDefine;
//BIG5 TRANS ALLOWED
interface

//----------------------自定义数据类型---------------------------------------------
type
CameraHandle = Integer;         //相机句柄
PCameraHandle = ^CameraHandle;  //相机句柄的指针
Ppbyte = ^PByte;
Ppchar = ^PChar;
PLongBool = ^LongBool;
HWND = LongWord;
PPointer = ^Pointer;

//----------------------转义C++中(CameraDefine.h)的枚举类型-----------------------
const

//图像查表变换的方式 emSdkLutMode
LUTMODE_PARAM_GEN = 0;   //通过调节参数动态生成LUT表
LUTMODE_PRESET    = 1;   //使用预设的LUT表
LUTMODE_USER_DEF  = 2;   //使用用户自定义的LUT表

//相机的视频流控制 emSdkRunMode
RUNMODE_PLAY  = 0;     //正常预览，捕获到图像就显示。（如果相机处于触发模式，则会等待触发帧的到来）
RUNMODE_PAUSE = 1;     //暂停，会暂停相机的图像输出，同时也不会去捕获图像
RUNMODE_STOP  = 2;     //停止相机工作。反初始化后，相机就处于停止模式

//SDK内部显示接口的显示方式 emSdkDisplayMode
DISPLAYMODE_SCALE = 0;   //缩放显示模式，缩放到显示控件的尺寸
DISPLAYMODE_REAL  = 1;   //1:1显示模式，当图像尺寸大于显示控件的尺寸时，只显示局部  

//录像状态 emSdkRecordMode
RECORD_STOP  = 0;      //停止
RECORD_START = 1;      //录像中
RECORD_PAUSE = 2;      //暂停

//图像的镜像操作 emSdkMirrorDirection
MIRROR_DIRECTION_HORIZONTAL = 0;   //水平镜像
MIRROR_DIRECTION_VERTICAL   = 1;   //垂直镜像

//相机视频的帧率 emSdkFrameSpeed
FRAME_SPEED_LOW    = 0;   //低速模式
FRAME_SPEED_NORMAL = 1;   //普通模式
FRAME_SPEED_HIGH   = 2;   //高速模式(需要较高的传输带宽,多设备共享传输带宽时会对帧率的稳定性有影响)
FRAME_SPEED_SUPER  = 3;   //超高速模式(需要较高的传输带宽,多设备共享传输带宽时会对帧率的稳定性有影响)

//保存文件的格式类型 emSdkFileType
FILE_JPG = 1;         //JPG
FILE_BMP = 2;         //BMP
FILE_RAW = 4;         //相机输出的bayer格式文件,对于不支持bayer格式输出相机，无法保存为该格式
FILE_PNG = 8;         //PNG

//相机中的图像传感器的工作模式 emSdkSnapMode
CONTINUATION     = 0;     //连续采集模式
SOFT_TRIGGER     = 1;     //软件触发模式，由软件发送指令后，传感器开始采集指定帧数的图像，采集完成后，停止输出
EXTERNAL_TRIGGER = 2;     //硬件触发模式，当接收到外部信号，传感器开始采集指定帧数的图像，采集完成后，停止输出

//自动曝光时抗频闪的频闪 emSdkLightFrequency
LIGHT_FREQUENCY_50HZ = 0; //50HZ,一般的灯光都是50HZ
LIGHT_FREQUENCY_60HZ = 1; //60HZ,主要是指显示器的

//相机的配置参数，分为A,B,C,D 4组进行保存。emSdkParameterTeam
PARAMETER_TEAM_DEFAULT = 0xff;
PARAMETER_TEAM_A       = 0;
PARAMETER_TEAM_B       = 1;
PARAMETER_TEAM_C       = 2;
PARAMETER_TEAM_D       = 3;

///*emSdkParameterMode 相机参数加载模式，参数加载分为从文件和从设备加载两种方式
//PARAM_MODE_BY_MODEL:所有同型号的相机共用ABCD四组参数文件。修改
//                   一台相机的参数文件，会影响到整个同型号的
//                   相机参数加载。                   
//PARAM_MODE_BY_NAME:所有设备名相同的相机，共用ABCD四组参数文件。
//                默认情况下，当电脑上只接了某型号一台相机时，
//                 设备名都是一样的，而您希望某一台相机能够加载
//                 不同的参数文件，则可以通过修改其设备名的方式
//                 来让其加载指定的参数文件。                 
//PARAM_MODE_BY_SN:相机按照自己的唯一序列号来加载ABCD四组参数文件，
//               序列号在出厂时已经固化在相机内，每台相机的序列号
//               都不相同，通过这种方式，每台相机的参数文件都是独立的。
//您可以根据自己的使用环境，灵活使用以上几种方式加载参数。例如，以
//MV-U300为例，您希望多台该型号的相机在您的 电脑上都共用4组参数，那么就
//使用PARAM_MODE_BY_MODEL方式;如果您希望其中某一台或者某几台MV-U300能
//使用自己参数文件而其余的MV-U300又要使用相同的参数文件，那么使用
//PARAM_MODE_BY_NAME方式;如果您希望每台MV-U300都使用不同的参数文件，那么
//使用PARAM_MODE_BY_SN方式。
//参数文件存在安装目录的 \Camera\Configs 目录下，以config为后缀名的文件。
//*/
PARAM_MODE_BY_MODEL  = 0;   //根据相机型号名从文件中加载参数，例如MV-U300
PARAM_MODE_BY_NAME   = 1;   //根据设备昵称(tSdkCameraDevInfo.acFriendlyName)从文件中加载参数，例如MV-U300,该昵称可自定义
PARAM_MODE_BY_SN     = 2;   //根据设备的唯一序列号从文件中加载参数，序列号在出厂时已经写入设备，每台相机拥有不同的序列号。
PARAM_MODE_IN_DEVICE = 3;   //从设备的固态存储器中加载参数。不是所有的型号都支持从相机中读写参数组，由tSdkCameraCapbility.bParamInDevice决定

//SDK生成的相机配置页面掩码值 emSdkPropSheetMask
PROP_SHEET_INDEX_EXPOSURE      = 0;
PROP_SHEET_INDEX_ISP_COLOR     = 1;
PROP_SHEET_INDEX_ISP_LUT       = 2;
PROP_SHEET_INDEX_ISP_SHAPE     = 3;   
PROP_SHEET_INDEX_VIDEO_FORMAT  = 4;
PROP_SHEET_INDEX_RESOLUTION    = 5;
PROP_SHEET_INDEX_IO_CTRL       = 6;
PROP_SHEET_INDEX_TRIGGER_SET   = 7;
PROP_SHEET_INDEX_OVERLAY       = 8;
PROP_SHEET_INDEX_DEVICE_INFO   = 9;

//SDK生成的相机配置页面的回调消息类型 emSdkPropSheetMsg
SHEET_MSG_LOAD_PARAM_DEFAULT  = 0;   //参数被恢复成默认后，触发该消息
SHEET_MSG_LOAD_PARAM_GROUP    = 1;   //加载指定参数组，触发该消息
SHEET_MSG_LOAD_PARAM_FROMFILE = 2;   //从指定文件加载参数后，触发该消息
SHEET_MSG_SAVE_PARAM_GROUP    = 3;   //当前参数组被保存时，触发该消息

//可视化选择参考窗口的类型 emSdkRefWinType
REF_WIN_AUTO_EXPOSURE = 0;
REF_WIN_WHITE_BALANCE = 1;

//可视化选择参考窗口的类型 emSdkResolutionMode
RES_MODE_PREVIEW  = 0;
RES_MODE_SNAPSHOT = 1;

//白平衡时色温模式 emSdkClrTmpMode
CT_MODE_AUTO       = 0;   //自动识别色温
CT_MODE_PRESET     = 1;   //使用指定的预设色温
CT_MODE_USER_DEF   = 2;   //自定义色温(增益和矩阵)

//LUT的颜色通道 emSdkLutChannel
LUT_CHANNEL_ALL    = 0;   //R,B,G三通道同时调节
LUT_CHANNEL_RED    = 1;   //红色通道
LUT_CHANNEL_GREEN  = 2;   //绿色通道
LUT_CHANNEL_BLUE   = 3;   //蓝色通道

//ISP处理单元 emSdkIspProcessor
ISP_PROCESSSOR_PC      = 0;   //使用PC的软件ISP模块
ISP_PROCESSSOR_DEVICE  = 1;   //使用相机自带的硬件ISP模块

//闪光灯信号控制方式 emStrobeControl
STROBE_SYNC_WITH_TRIG_AUTO      = 0;     //和触发信号同步，触发后，相机进行曝光时，自动生成STROBE信号。此时，有效极性可设置(CameraSetStrobePolarity)。
STROBE_SYNC_WITH_TRIG_MANUAL    = 1;     //和触发信号同步，触发后，STROBE延时指定的时间后(CameraSetStrobeDelayTime)，再持续指定时间的脉冲(CameraSetStrobePulseWidth)，有效极性可设置(CameraSetStrobePolarity)。
STROBE_ALWAYS_HIGH              = 2;     //始终为高，忽略STROBE信号的其他设置
STROBE_ALWAYS_LOW               = 3;     //始终为低，忽略STROBE信号的其他设置

//硬件外触发的信号种类 emExtTrigSignal
EXT_TRIG_LEADING_EDGE   = 0;       //上升沿触发，默认为该方式
EXT_TRIG_TRAILING_EDGE  = 1;       //下降沿触发
EXT_TRIG_HIGH_LEVEL     = 2;       //高电平触发,电平宽度决定曝光时间，仅部分型号的相机支持电平触发方式。
EXT_TRIG_LOW_LEVEL      = 3;       //低电平触发,


//硬件外触发时的快门方式 emExtTrigShutterMode
EXT_TRIG_EXP_STANDARD    = 0;        //标准方式，默认为该方式。
EXT_TRIG_EXP_GRR        = 1;         //全局复位方式，部分滚动快门的CMOS型号的相机支持该方式，配合外部机械快门，可以达到全局快门的效果，适合拍高速运动的物体

//----------------------转义C++中(CameraDefine.h)的结构体类型-----------------------

//相机基本信息(只读，勿修改)
type
PtSdkCameraDevInfo = ^ tSdkCameraDevInfo;
tSdkCameraDevInfo = record
acProductSeries:array[1..32] of char;    // 产品系列
acProductName:array[1..32] of char;      // 产品名称
acFriendlyName:array[1..32] of char;     // 昵称，保存于设备中，用户可以动态修改，用来区分多相机同时使用
acLinkName:array[1..32] of char;         // 内部使用的连接符号名
acDriverVersion:array[1..32] of char;    // 驱动版本
acSensorType:array[1..32] of char;       // sensor类型
acPortType:array[1..32] of char;         // 接口类型
acSn:array[1..32] of char;               // 产品唯一序列号
uInstance:Cardinal;                       // 该型号相机在该电脑上的实例索引号，用于区分同型号多相机
end;

//相机的分辨率设定范围
type
ptSdkResolutionRange = ^tSdkResolutionRange;
tSdkResolutionRange = record
iHeightMax:Integer;             //图像最大高度
iHeightMin:Integer;             //图像最小高度
iWidthMax:Integer;              //图像最大宽度
iWidthMin:Integer;              //图像最小宽度
uSkipModeMask:Cardinal;         //SKIP模式掩码，为0，表示不支持SKIP 。bit0为1,表示支持SKIP 2x2 ;bit1为1，表示支持SKIP 3x3....
uBinSumModeMask:Cardinal;       //BIN(求和)模式掩码，为0，表示不支持BIN 。bit0为1,表示支持BIN 2x2 ;bit1为1，表示支持BIN 3x3....
uBinAverageModeMask:Cardinal;   //BIN(求均值)模式掩码，为0，表示不支持BIN 。bit0为1,表示支持BIN 2x2 ;bit1为1，表示支持BIN 3x3....
uResampleMask:Cardinal;         //硬件重采样的掩码
end;

//相机的分辨率描述
type
PtSdkImageResolution = ^tSdkImageResolution;
tSdkImageResolution = record
iIndex:Integer;                    // 索引号，[0,N]表示预设的分辨率(N 为预设分辨率的最大个数，一般不超过20),OXFF 表示自定义分辨率(ROI)
acDescription:array[1..32] of char;// 该分辨率的描述信息。仅预设分辨率时该信息有效。自定义分辨率可忽略该信息
uBinSumMode:Cardinal;              // 是否BIN的尺寸,16bit为1表示sum，为0表示average；低16位表示BIN矩阵大小，为0表示禁止BIN模式,范围不能超过tSdkResolutionRange中uBinModeMask
uBinAverageMode:Cardinal;          // BIN(求均值)的模式,范围不能超过tSdkResolutionRange中uBinAverageModeMask
uSkipMode:Cardinal;                // 是否SKIP的尺寸，为0表示禁止SKIP模式。1表示SKIP2X2，2表示SKIP3X3，以此类推,范围不能超过tSdkResolutionRange中uSkipModeMask
uResampleMask:Cardinal;            // 硬件重采样的掩码
iHOffsetFOV:Integer;               // 采集视场相对于Sensor最大视场左上角的垂直偏移
iVOffsetFOV:Integer;               // 采集视场相对于Sensor最大视场左上角的水平偏移
iWidthFOV:Integer;                 // 采集视场的宽度
iHeightFOV:Integer;                // 采集视场的高度
iWidth:Integer;                    // 相机最终输出的图像的宽度
iHeight:Integer;                   // 相机最终输出的图像的高度
iWidthZoomHd:Integer;              // 硬件缩放的宽度,不需要进行此操作的分辨率，此变量设置为0.
iHeightZoomHd:Integer;             // 硬件缩放的高度,不需要进行此操作的分辨率，此变量设置为0.
iWidthZoomSw:Integer;              // 软件缩放的宽度,不需要进行此操作的分辨率，此变量设置为0.
iHeightZoomSw:Integer;             // 软件缩放的高度,不需要进行此操作的分辨率，此变量设置为0.
end;

//相机白平衡模式描述信息
type
PtSdkColorTemperatureDes = ^tSdkColorTemperatureDes;
tSdkColorTemperatureDes = record
iIndex:Integer;                          // 模式索引号
acDescription:array[1..32] of char;      // 描述信息
end;

//相机帧率描述信息
type
PtSdkFrameSpeed = ^tSdkFrameSpeed;
tSdkFrameSpeed = record
iIndex:Integer;                      // 帧率索引号，一般0对应于低速模式，1对应于普通模式，2对应于高速模式
acDescription:array[1..32] of char;  // 描述信息      
end;

//相机曝光功能范围定义
type
PtSdkExpose = ^tSdkExpose;
tSdkExpose = record           
uiTargetMin:Cardinal;      //自动曝光亮度目标最小值
uiTargetMax:Cardinal;      //自动曝光亮度目标最大值
uiAnalogGainMin:Cardinal;  //模拟增益的最小值，单位为fAnalogGainStep中定义      
uiAnalogGainMax:Cardinal;  //模拟增益的最大值，单位为fAnalogGainStep中定义        
fAnalogGainStep:Single;    //模拟增益每增加1，对应的增加的放大倍数。例如，uiAnalogGainMin一般为16，fAnalogGainStep一般为0.125，那么最小放大倍数就是16*0.125 = 2倍
uiExposeTimeMin:Cardinal;  //手动模式下，曝光时间的最小值，单位:行。根据CameraGetExposureLineTime可以获得一行对应的时间(微秒),从而得到整帧的曝光时间    
uiExposeTimeMax:Cardinal;  //手动模式下，曝光时间的最大值，单位:行        
end;

//触发模式描述
type
PtSdkTrigger = ^tSdkTrigger;
tSdkTrigger = record
iIndex:Integer;                         //模式索引号
acDescription:array[1..32] of char;     //该模式的描述信息
end;

//传输分包大小描述(主要是针对网络相机有效)
type
PtSdkPackLength = ^tSdkPackLength;
tSdkPackLength = record
iIndex:Integer;                         //分包大小索引号
acDescription:array[1..32] of char;     //对应的描述信息     
iPackSize:Cardinal;
end;

//预设的LUT表描述
type
PtSdkPresetLut = ^tSdkPresetLut;
tSdkPresetLut = record
iIndex:Integer;                         //编号
acDescription:array[1..32] of char;     //描述信息
end;

//AE算法描述
type
PtSdkAeAlgorithm = ^tSdkAeAlgorithm;
tSdkAeAlgorithm = record
iIndex:Integer;                         //编号
acDescription:array[1..32] of char;     //描述信息
end;


//RAW转RGB算法描述
type
PtSdkBayerDecodeAlgorithm = ^tSdkBayerDecodeAlgorithm;
tSdkBayerDecodeAlgorithm = record
iIndex:Integer;                         //编号     
acDescription:array[1..32] of char;     //描述信息
end;

//帧率统计信息
type
PtSdkFrameStatistic = ^tSdkFrameStatistic;
tSdkFrameStatistic = record
iTotal:Integer;       //当前采集的总帧数（包括错误帧）
iCapture:Integer;     //当前采集的有效帧的数量
iLost:Integer;        //当前丢帧的数量
end;

//相机输出的图像数据格式
type
PtSdkMediaType = ^tSdkMediaType;
tSdkMediaType = record
iIndex:Integer;                     //格式种类编号
acDescription:array[1..32] of char; //描述信息
iMediaType:Cardinal;                //对应的图像格式编码，如CAMERA_MEDIA_TYPE_BAYGR8，在本文件中有定义。
end;

//伽马的设定范围
type
PtGammaRange = ^tGammaRange; 
tGammaRange = record
iMin:Integer;                 //最小值
iMax:Integer;                 //最大值
end;

//对比度的设定范围
type
PtContrastRange = ^tContrastRange;
tContrastRange = record
iMin:Integer;     //最小值
iMax:Integer;     //最大值
end;

//RGB三通道数字增益的设定范围
type
PtRgbGainRange = ^tRgbGainRange;
tRgbGainRange = record
iRGainMin:Integer;    //红色增益的最小值
iRGainMax:Integer;    //红色增益的最大值
iGGainMin:Integer;    //绿色增益的最小值
iGGainMax:Integer;    //绿色增益的最大值
iBGainMin:Integer;    //蓝色增益的最小值
iBGainMax:Integer;    //蓝色增益的最大值
end;

//饱和度设定的范围
type
PtSaturationRange = ^tSaturationRange;
tSaturationRange = record  
iMin:Integer;   //最小值
iMax:Integer;   //最大值
end;

//锐化的设定范围
type
PtSharpnessRange= ^tSharpnessRange;
tSharpnessRange = record  
iMin:Integer;   //最小值
iMax:Integer;   //最大值
end;

//ISP模块的使能信息
type
PtSdkIspCapacity = ^tSdkIspCapacity;
tSdkIspCapacity = record
bMonoSensor:LongBool;         //表示该型号相机是否为黑白相机,如果是黑白相机，则颜色相关的功能都无法调节
bWbOnce:LongBool;             //表示该型号相机是否支持手动白平衡功能
bAutoWb:LongBool;             //表示该型号相机是否支持自动白平衡功能
bAutoExposure:LongBool;       //表示该型号相机是否支持自动曝光功能
bManualExposure:LongBool;     //表示该型号相机是否支持手动曝光功能
bAntiFlick:LongBool;          //表示该型号相机是否支持抗频闪功能
bDeviceIsp:LongBool;          //表示该型号相机是否支持硬件ISP功能
bForceUseDeviceIsp:LongBool;  //bDeviceIsp和bForceUseDeviceIsp同时为TRUE时，表示强制只用硬件ISP，不可取消。
bZoomHD:LongBool;             //相机硬件是否支持图像缩放输出(只能是缩小)。
end;

// 定义整合的设备描述信息，这些信息可以用于动态构建UI */
type
PtSdkCameraCapbility = ^tSdkCameraCapbility;
tSdkCameraCapbility = record
pTriggerDesc:^tSdkTrigger;                // 触发模式
iTriggerDesc:Integer;                     // 触发模式的个数，即pTriggerDesc数组的大小
pImageSizeDesc:^tSdkImageResolution;      // 预设分辨率选择
iImageSizeDesc:Integer;                   // 预设分辨率的个数，即pImageSizeDesc数组的大小
pClrTempDesc:^tSdkColorTemperatureDes;    // 预设色温模式，用于白平衡
iClrTempDesc:Integer;
pMediaTypeDesc:^tSdkMediaType;            // 相机输出图像格式
iMediaTypdeDesc:Integer;                  // 相机输出图像格式的种类个数，即pMediaTypeDesc数组的大小。
pFrameSpeedDesc:^tSdkFrameSpeed;          // 可调节帧速类型，对应界面上普通 高速 和超级三种速度设置
iFrameSpeedDesc:Integer;                  // 可调节帧速类型的个数，即pFrameSpeedDesc数组的大小。
pPackLenDesc:^tSdkPackLength;             // 传输包长度，一般用于网络设备
iPackLenDesc:Integer;                     // 可供选择的传输分包长度的个数，即pPackLenDesc数组的大小。
iOutputIoCounts:Integer;                  // 可编程输入IO的个数
iInputIoCounts:Integer;                   // 可编程输入IO的个数
pPresetLutDesc:^tSdkPresetLut;            // 相机预设的LUT表
iPresetLut:Integer;                       // 相机预设的LUT表的个数，即pPresetLutDesc数组的大小
iUserDataMaxLen:Integer;                  // 指示该相机中用于保存用户数据区的最大长度。为0表示无。
bParamInDevice:LongBool;                  // 指示该设备是否支持从设备中读写参数组。1为支持，0不支持。
pAeAlmSwDesc:^tSdkAeAlgorithm;            // 软件自动曝光算法描述, 指向tSdkTrigger数组的地址，用Long类型表示指针
iAeAlmSwDesc:Integer;                     // 软件自动曝光算法个数
pAeAlmHdDesc:^tSdkAeAlgorithm;            // 硬件自动曝光算法描述, 指向tSdkTrigger数组的地址，用Long类型表示指针
iAeAlmHdDesc:Integer;                     // 硬件自动曝光算法个数
pBayerDecAlmSwDesc:^tSdkBayerDecodeAlgorithm;   // 软件Bayer转换为RGB数据的算法描述, tSdkBayerDecodeAlgorithm，用Long类型表示指针
iBayerDecAlmSwDesc:Integer;                     // 软件Bayer转换为RGB数据的算法个数
pBayerDecAlmHdDesc:^tSdkBayerDecodeAlgorithm;   // 硬件Bayer转换为RGB数据的算法描述, tSdkBayerDecodeAlgorithm，用Long类型表示指针
iBayerDecAlmHdDesc:Integer;                     // 硬件Bayer转换为RGB数据的算法个数
// 图像参数的调节范围定义,用于动态构建UI
sExposeDesc:tSdkExpose;                 // 曝光的范围值
sResolutionRange:tSdkResolutionRange;   // 分辨率范围描述 
sRgbGainRange:tRgbGainRange;            // 图像数字增益范围描述
sSaturationRange:tSaturationRange;      // 饱和度范围描述
sGammaRange:tGammaRange;                // 伽马范围描述
sContrastRange:tContrastRange;          // 对比度范围描述
sSharpnessRange:tSharpnessRange;        // 锐化范围描述
sIspCapacity:tSdkIspCapacity;           // ISP能力描述
end;

//图像帧头信息
type
PtSdkFrameHead = ^tSdkFrameHead;
PPtSdkFrameHead = ^PtSdkFrameHead;
tSdkFrameHead = record
uiMediaType:Cardinal;   // 图像格式,Image Format
uBytes:Cardinal;        // 图像数据字节数,Total bytes
iWidth:Integer;         // 宽度 Image width
iHeight:Integer;        // 高度 Image height
iWidthZoomSw:Integer;   // 软件缩放的宽度,不需要进行软件裁剪的图像，此变量设置为0.
iHeightZoomSw:Integer;  // 软件缩放的高度,不需要进行软件裁剪的图像，此变量设置为0.
bIsTrigger:LongBool;    // 指示是否为触发帧 is trigger
uiTimeStamp:Cardinal;   // 该帧的采集时间，单位0.1毫秒
uiExpTime:Cardinal;     // 当前图像的曝光值，单位为微秒us
fAnalogGain:Single;     // 当前图像的模拟增益倍数
iGamma:Integer;         // 该帧图像的伽马设定值，仅当LUT模式为动态参数生成时有效，其余模式下为-1
iContrast:Integer;      // 该帧图像的对比度设定值，仅当LUT模式为动态参数生成时有效，其余模式下为-1
iSaturation:Integer;    // 该帧图像的饱和度设定值，对于黑白相机无意义，为0
fRgain:Single;          // 该帧图像处理的红色数字增益倍数，对于黑白相机无意义，为1
fGgain:Single;          // 该帧图像处理的绿色数字增益倍数，对于黑白相机无意义，为1
fBgain:Single;          // 该帧图像处理的蓝色数字增益倍数，对于黑白相机无意义，为1
end;

//图像帧描述
type
PtSdkFrame = ^tSdkFrame;
tSdkFrame = record
head:tSdkFrameHead;     //帧头
pBuffer:^Byte;          //数据区
end;

//图像抓取回调函数
type
CAMERA_SNAP_PROC = procedure (
hCamera:CameraHandle;
pFrameBuffer:PByte;
pFrameHead:PtSdkFrameHead;
pContext:Pointer
); stdcall;

//SDK生成的相机配置页面的消息回调函数定义
type
CAMERA_PAGE_MSG_PROC = procedure (
hCamera:CameraHandle;
MSG:Cardinal;
uParam:Cardinal;
pContext:Pointer
); stdcall;

//////////////////////////////////////////////////////////////////////////
// Grabber 相关

// Grabber统计信息
type
PtSdkGrabberStat = ^tSdkGrabberStat;
tSdkGrabberStat = record
	Width: Integer;
	Height: Integer;	// 帧图像大小
	Disp: Integer;		// 显示帧数量
	Capture: Integer;	// 采集的有效帧的数量
	Lost: Integer;		// 丢帧的数量
	Error: Integer;		// 错帧的数量
	DispFps: Single;	// 显示帧率
	CapFps: Single;		// 捕获帧率
end;

// 图像捕获的回调函数定义
type
pfnCameraGrabberFrameListener = function(
	Grabber:Pointer;
	Phase:Integer;
	pFrameBuffer:PByte;
	pFrameHead:PtSdkFrameHead;
	Context:Pointer
):Integer; stdcall;

// 异步抓图的回调函数定义
type
pfnCameraGrabberSaveImageComplete = procedure(
	Grabber:Pointer;
	Image:Pointer;	// 需要调用CameraImage_Destroy释放
	Status:Integer;
	Context:Pointer
); stdcall;

//----------------------------IMAGE FORMAT DEFINE------------------------------------
//----------------------------图像格式定义-------------------------------------------
const
CAMERA_MEDIA_TYPE_MONO                           =$01000000;
CAMERA_MEDIA_TYPE_RGB                            =$02000000;
CAMERA_MEDIA_TYPE_COLOR                          =$02000000;
CAMERA_MEDIA_TYPE_CUSTOM                         =$80000000;
CAMERA_MEDIA_TYPE_COLOR_MASK                     =$FF000000;
CAMERA_MEDIA_TYPE_OCCUPY1BIT                     =$00010000;
CAMERA_MEDIA_TYPE_OCCUPY2BIT                     =$00020000;
CAMERA_MEDIA_TYPE_OCCUPY4BIT                     =$00040000;
CAMERA_MEDIA_TYPE_OCCUPY8BIT                     =$00080000;
CAMERA_MEDIA_TYPE_OCCUPY10BIT                    =$000A0000;
CAMERA_MEDIA_TYPE_OCCUPY12BIT                    =$000C0000;
CAMERA_MEDIA_TYPE_OCCUPY16BIT                    =$00100000;
CAMERA_MEDIA_TYPE_OCCUPY24BIT                    =$00180000;
CAMERA_MEDIA_TYPE_OCCUPY32BIT                    =$00200000;
CAMERA_MEDIA_TYPE_OCCUPY36BIT                    =$00240000;
CAMERA_MEDIA_TYPE_OCCUPY48BIT                    =$00300000;
CAMERA_MEDIA_TYPE_EFFECTIVE_PIXEL_SIZE_MASK      =$00FF0000;
CAMERA_MEDIA_TYPE_EFFECTIVE_PIXEL_SIZE_SHIFT     =16;
CAMERA_MEDIA_TYPE_ID_MASK                        =$0000FFFF;
CAMERA_MEDIA_TYPE_COUNT                          =$46;

//*mono*/
CAMERA_MEDIA_TYPE_MONO1P             =(CAMERA_MEDIA_TYPE_MONO or  CAMERA_MEDIA_TYPE_OCCUPY1BIT or  $0037);
CAMERA_MEDIA_TYPE_MONO2P             =(CAMERA_MEDIA_TYPE_MONO or  CAMERA_MEDIA_TYPE_OCCUPY2BIT or  $0038);
CAMERA_MEDIA_TYPE_MONO4P             =(CAMERA_MEDIA_TYPE_MONO or  CAMERA_MEDIA_TYPE_OCCUPY4BIT or  $0039);
CAMERA_MEDIA_TYPE_MONO8              =(CAMERA_MEDIA_TYPE_MONO or  CAMERA_MEDIA_TYPE_OCCUPY8BIT or  $0001);
CAMERA_MEDIA_TYPE_MONO8S             =(CAMERA_MEDIA_TYPE_MONO or  CAMERA_MEDIA_TYPE_OCCUPY8BIT or  $0002);
CAMERA_MEDIA_TYPE_MONO10             =(CAMERA_MEDIA_TYPE_MONO or  CAMERA_MEDIA_TYPE_OCCUPY16BIT or  $0003);
CAMERA_MEDIA_TYPE_MONO10_PACKED      =(CAMERA_MEDIA_TYPE_MONO or  CAMERA_MEDIA_TYPE_OCCUPY12BIT or  $0004);
CAMERA_MEDIA_TYPE_MONO12             =(CAMERA_MEDIA_TYPE_MONO or  CAMERA_MEDIA_TYPE_OCCUPY16BIT or  $0005);
CAMERA_MEDIA_TYPE_MONO12_PACKED      =(CAMERA_MEDIA_TYPE_MONO or  CAMERA_MEDIA_TYPE_OCCUPY12BIT or  $0006);
CAMERA_MEDIA_TYPE_MONO14             =(CAMERA_MEDIA_TYPE_MONO or  CAMERA_MEDIA_TYPE_OCCUPY16BIT or  $0025);
CAMERA_MEDIA_TYPE_MONO16             =(CAMERA_MEDIA_TYPE_MONO or  CAMERA_MEDIA_TYPE_OCCUPY16BIT or  $0007);

//*Bayer */
CAMERA_MEDIA_TYPE_BAYGR8             =(CAMERA_MEDIA_TYPE_MONO or  CAMERA_MEDIA_TYPE_OCCUPY8BIT or  $0008);
CAMERA_MEDIA_TYPE_BAYRG8             =(CAMERA_MEDIA_TYPE_MONO or  CAMERA_MEDIA_TYPE_OCCUPY8BIT or  $0009);
CAMERA_MEDIA_TYPE_BAYGB8             =(CAMERA_MEDIA_TYPE_MONO or  CAMERA_MEDIA_TYPE_OCCUPY8BIT or  $000A);
CAMERA_MEDIA_TYPE_BAYBG8             =(CAMERA_MEDIA_TYPE_MONO or  CAMERA_MEDIA_TYPE_OCCUPY8BIT or  $000B);

CAMERA_MEDIA_TYPE_BAYGR10_MIPI       =(CAMERA_MEDIA_TYPE_MONO or  CAMERA_MEDIA_TYPE_OCCUPY10BIT or  $0026);
CAMERA_MEDIA_TYPE_BAYRG10_MIPI       =(CAMERA_MEDIA_TYPE_MONO or  CAMERA_MEDIA_TYPE_OCCUPY10BIT or  $0027);
CAMERA_MEDIA_TYPE_BAYGB10_MIPI       =(CAMERA_MEDIA_TYPE_MONO or  CAMERA_MEDIA_TYPE_OCCUPY10BIT or  $0028);
CAMERA_MEDIA_TYPE_BAYBG10_MIPI       =(CAMERA_MEDIA_TYPE_MONO or  CAMERA_MEDIA_TYPE_OCCUPY10BIT or  $0029);


CAMERA_MEDIA_TYPE_BAYGR10            =(CAMERA_MEDIA_TYPE_MONO or  CAMERA_MEDIA_TYPE_OCCUPY16BIT or  $000C);
CAMERA_MEDIA_TYPE_BAYRG10            =(CAMERA_MEDIA_TYPE_MONO or  CAMERA_MEDIA_TYPE_OCCUPY16BIT or  $000D);
CAMERA_MEDIA_TYPE_BAYGB10            =(CAMERA_MEDIA_TYPE_MONO or  CAMERA_MEDIA_TYPE_OCCUPY16BIT or  $000E);
CAMERA_MEDIA_TYPE_BAYBG10            =(CAMERA_MEDIA_TYPE_MONO or  CAMERA_MEDIA_TYPE_OCCUPY16BIT or  $000F);

CAMERA_MEDIA_TYPE_BAYGR12            =(CAMERA_MEDIA_TYPE_MONO or  CAMERA_MEDIA_TYPE_OCCUPY16BIT or  $0010);
CAMERA_MEDIA_TYPE_BAYRG12            =(CAMERA_MEDIA_TYPE_MONO or  CAMERA_MEDIA_TYPE_OCCUPY16BIT or  $0011);
CAMERA_MEDIA_TYPE_BAYGB12            =(CAMERA_MEDIA_TYPE_MONO or  CAMERA_MEDIA_TYPE_OCCUPY16BIT or  $0012);
CAMERA_MEDIA_TYPE_BAYBG12            =(CAMERA_MEDIA_TYPE_MONO or  CAMERA_MEDIA_TYPE_OCCUPY16BIT or  $0013);


CAMERA_MEDIA_TYPE_BAYGR10_PACKED     =(CAMERA_MEDIA_TYPE_MONO or  CAMERA_MEDIA_TYPE_OCCUPY12BIT or  $0026);
CAMERA_MEDIA_TYPE_BAYRG10_PACKED     =(CAMERA_MEDIA_TYPE_MONO or  CAMERA_MEDIA_TYPE_OCCUPY12BIT or  $0027);
CAMERA_MEDIA_TYPE_BAYGB10_PACKED     =(CAMERA_MEDIA_TYPE_MONO or  CAMERA_MEDIA_TYPE_OCCUPY12BIT or  $0028);
CAMERA_MEDIA_TYPE_BAYBG10_PACKED     =(CAMERA_MEDIA_TYPE_MONO or  CAMERA_MEDIA_TYPE_OCCUPY12BIT or  $0029);

CAMERA_MEDIA_TYPE_BAYGR12_PACKED     =(CAMERA_MEDIA_TYPE_MONO or  CAMERA_MEDIA_TYPE_OCCUPY12BIT or  $002A);
CAMERA_MEDIA_TYPE_BAYRG12_PACKED     =(CAMERA_MEDIA_TYPE_MONO or  CAMERA_MEDIA_TYPE_OCCUPY12BIT or  $002B);
CAMERA_MEDIA_TYPE_BAYGB12_PACKED     =(CAMERA_MEDIA_TYPE_MONO or  CAMERA_MEDIA_TYPE_OCCUPY12BIT or  $002C);
CAMERA_MEDIA_TYPE_BAYBG12_PACKED     =(CAMERA_MEDIA_TYPE_MONO or  CAMERA_MEDIA_TYPE_OCCUPY12BIT or  $002D);

CAMERA_MEDIA_TYPE_BAYGR16            =(CAMERA_MEDIA_TYPE_MONO or  CAMERA_MEDIA_TYPE_OCCUPY16BIT or  $002E);
CAMERA_MEDIA_TYPE_BAYRG16            =(CAMERA_MEDIA_TYPE_MONO or  CAMERA_MEDIA_TYPE_OCCUPY16BIT or  $002F);
CAMERA_MEDIA_TYPE_BAYGB16            =(CAMERA_MEDIA_TYPE_MONO or  CAMERA_MEDIA_TYPE_OCCUPY16BIT or  $0030);
CAMERA_MEDIA_TYPE_BAYBG16            =(CAMERA_MEDIA_TYPE_MONO or  CAMERA_MEDIA_TYPE_OCCUPY16BIT or  $0031);

//*RGB */
CAMERA_MEDIA_TYPE_RGB8               =(CAMERA_MEDIA_TYPE_COLOR or  CAMERA_MEDIA_TYPE_OCCUPY24BIT or  $0014);
CAMERA_MEDIA_TYPE_BGR8               =(CAMERA_MEDIA_TYPE_COLOR or  CAMERA_MEDIA_TYPE_OCCUPY24BIT or  $0015);
CAMERA_MEDIA_TYPE_RGBA8              =(CAMERA_MEDIA_TYPE_COLOR or  CAMERA_MEDIA_TYPE_OCCUPY32BIT or  $0016);
CAMERA_MEDIA_TYPE_BGRA8              =(CAMERA_MEDIA_TYPE_COLOR or  CAMERA_MEDIA_TYPE_OCCUPY32BIT or  $0017);
CAMERA_MEDIA_TYPE_RGB10              =(CAMERA_MEDIA_TYPE_COLOR or  CAMERA_MEDIA_TYPE_OCCUPY48BIT or  $0018);
CAMERA_MEDIA_TYPE_BGR10              =(CAMERA_MEDIA_TYPE_COLOR or  CAMERA_MEDIA_TYPE_OCCUPY48BIT or  $0019);
CAMERA_MEDIA_TYPE_RGB12              =(CAMERA_MEDIA_TYPE_COLOR or  CAMERA_MEDIA_TYPE_OCCUPY48BIT or  $001A);
CAMERA_MEDIA_TYPE_BGR12              =(CAMERA_MEDIA_TYPE_COLOR or  CAMERA_MEDIA_TYPE_OCCUPY48BIT or  $001B);
CAMERA_MEDIA_TYPE_RGB16              =(CAMERA_MEDIA_TYPE_COLOR or  CAMERA_MEDIA_TYPE_OCCUPY48BIT or  $0033);
CAMERA_MEDIA_TYPE_RGB10V1_PACKED     =(CAMERA_MEDIA_TYPE_COLOR or  CAMERA_MEDIA_TYPE_OCCUPY32BIT or  $001C);
CAMERA_MEDIA_TYPE_RGB10P32           =(CAMERA_MEDIA_TYPE_COLOR or  CAMERA_MEDIA_TYPE_OCCUPY32BIT or  $001D);
CAMERA_MEDIA_TYPE_RGB12V1_PACKED     =(CAMERA_MEDIA_TYPE_COLOR or  CAMERA_MEDIA_TYPE_OCCUPY36BIT or  $0034);
CAMERA_MEDIA_TYPE_RGB565P            =(CAMERA_MEDIA_TYPE_COLOR or  CAMERA_MEDIA_TYPE_OCCUPY16BIT or  $0035);
CAMERA_MEDIA_TYPE_BGR565P            =(CAMERA_MEDIA_TYPE_COLOR or  CAMERA_MEDIA_TYPE_OCCUPY16BIT or  $0036);

//*YUV and YCbCr*/
CAMERA_MEDIA_TYPE_YUV411_8_UYYVYY    =(CAMERA_MEDIA_TYPE_COLOR or  CAMERA_MEDIA_TYPE_OCCUPY12BIT or  $001E);
CAMERA_MEDIA_TYPE_YUV422_8_UYVY      =(CAMERA_MEDIA_TYPE_COLOR or  CAMERA_MEDIA_TYPE_OCCUPY16BIT or  $001F);
CAMERA_MEDIA_TYPE_YUV422_8           =(CAMERA_MEDIA_TYPE_COLOR or  CAMERA_MEDIA_TYPE_OCCUPY16BIT or  $0032);
CAMERA_MEDIA_TYPE_YUV8_UYV           =(CAMERA_MEDIA_TYPE_COLOR or  CAMERA_MEDIA_TYPE_OCCUPY24BIT or  $0020);
CAMERA_MEDIA_TYPE_YCBCR8_CBYCR       =(CAMERA_MEDIA_TYPE_COLOR or  CAMERA_MEDIA_TYPE_OCCUPY24BIT or  $003A);
//CAMERA_MEDIA_TYPE_YCBCR422_8 : YYYYCbCrCbCr
CAMERA_MEDIA_TYPE_YCBCR422_8         =(CAMERA_MEDIA_TYPE_COLOR or  CAMERA_MEDIA_TYPE_OCCUPY16BIT or  $003B);
CAMERA_MEDIA_TYPE_YCBCR422_8_CBYCRY      =(CAMERA_MEDIA_TYPE_COLOR or  CAMERA_MEDIA_TYPE_OCCUPY16BIT or  $0043);
CAMERA_MEDIA_TYPE_YCBCR411_8_CBYYCRYY    =(CAMERA_MEDIA_TYPE_COLOR or  CAMERA_MEDIA_TYPE_OCCUPY12BIT or  $003C);
CAMERA_MEDIA_TYPE_YCBCR601_8_CBYCR       =(CAMERA_MEDIA_TYPE_COLOR or  CAMERA_MEDIA_TYPE_OCCUPY24BIT or  $003D);
CAMERA_MEDIA_TYPE_YCBCR601_422_8         =(CAMERA_MEDIA_TYPE_COLOR or  CAMERA_MEDIA_TYPE_OCCUPY16BIT or  $003E);
CAMERA_MEDIA_TYPE_YCBCR601_422_8_CBYCRY  =(CAMERA_MEDIA_TYPE_COLOR or  CAMERA_MEDIA_TYPE_OCCUPY16BIT or  $0044);
CAMERA_MEDIA_TYPE_YCBCR601_411_8_CBYYCRYY    =(CAMERA_MEDIA_TYPE_COLOR or  CAMERA_MEDIA_TYPE_OCCUPY12BIT or  $003F);
CAMERA_MEDIA_TYPE_YCBCR709_8_CBYCR           =(CAMERA_MEDIA_TYPE_COLOR or  CAMERA_MEDIA_TYPE_OCCUPY24BIT or  $0040);
CAMERA_MEDIA_TYPE_YCBCR709_422_8             =(CAMERA_MEDIA_TYPE_COLOR or  CAMERA_MEDIA_TYPE_OCCUPY16BIT or  $0041);
CAMERA_MEDIA_TYPE_YCBCR709_422_8_CBYCRY      =(CAMERA_MEDIA_TYPE_COLOR or  CAMERA_MEDIA_TYPE_OCCUPY16BIT or  $0045);
CAMERA_MEDIA_TYPE_YCBCR709_411_8_CBYYCRYY    =(CAMERA_MEDIA_TYPE_COLOR or  CAMERA_MEDIA_TYPE_OCCUPY12BIT or  $0042);

//*RGB Planar */
CAMERA_MEDIA_TYPE_RGB8_PLANAR        =(CAMERA_MEDIA_TYPE_COLOR or  CAMERA_MEDIA_TYPE_OCCUPY24BIT or  $0021);
CAMERA_MEDIA_TYPE_RGB10_PLANAR       =(CAMERA_MEDIA_TYPE_COLOR or  CAMERA_MEDIA_TYPE_OCCUPY48BIT or  $0022);
CAMERA_MEDIA_TYPE_RGB12_PLANAR       =(CAMERA_MEDIA_TYPE_COLOR or  CAMERA_MEDIA_TYPE_OCCUPY48BIT or  $0023);
CAMERA_MEDIA_TYPE_RGB16_PLANAR       =(CAMERA_MEDIA_TYPE_COLOR or  CAMERA_MEDIA_TYPE_OCCUPY48BIT or  $0024);
implementation

end.
