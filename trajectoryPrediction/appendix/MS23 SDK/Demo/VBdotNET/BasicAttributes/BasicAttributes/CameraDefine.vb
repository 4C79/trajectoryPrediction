Imports System.Runtime.InteropServices

Public Module CameraDefine
    'BIG5 TRANS ALLOWED
    '图像查表变换的方式 emSdkLutMode
    Public Const LUTMODE_PARAM_GEN = 0   '通过调节参数动态生成LUT表
    Public Const LUTMODE_PRESET = 1      '使用预设的LUT表
    Public Const LUTMODE_USER_DEF = 2    '使用用户自定义的LUT表

    '相机的视频流控制 emSdkRunMode
    Public Const RUNMODE_PLAY = 0         '正常预览，捕获到图像就显示。（如果相机处于触发模式，则会等待触发帧的到来）
    Public Const RUNMODE_PAUSE = 1        '暂停，会暂停相机的图像输出，同时也不会去捕获图像
    Public Const RUNMODE_STOP = 2         '停止相机工作。反初始化后，相机就处于停止模式

    'SDK内部显示接口的显示方式 emSdkDisplayMode
    Public Const DISPLAYMODE_SCALE = 0    '缩放显示模式，缩放到显示控件的尺寸
    Public Const DISPLAYMODE_REAL = 1     '1:1显示模式，当图像尺寸大于显示控件的尺寸时，只显示局部

    '录像状态 emSdkRecordMode
    Public Const RECORD_STOP = 0          '停止
    Public Const RECORD_START = 1         '录像中
    Public Const RECORD_PAUSE = 2         '暂停

    '图像的镜像操作 emSdkMirrorDirection
    Public Const MIRROR_DIRECTION_HORIZONTAL = 0    '水平镜像
    Public Const MIRROR_DIRECTION_VERTICAL = 1      '垂直镜像

    '相机视频的帧率 emSdkFrameSpeed
    Public Const FRAME_SPEED_LOW = 0        '低速模式
    Public Const FRAME_SPEED_NORMAL = 1     '普通模式
    Public Const FRAME_SPEED_HIGH = 2       '高速模式(需要较高的传输带宽,多设备共享传输带宽时会对帧率的稳定性有影响)
    Public Const FRAME_SPEED_SUPER = 3      '超高速模式(需要较高的传输带宽,多设备共享传输带宽时会对帧率的稳定性有影响)

    '保存文件的格式类型 emSdkFileType
    Public Const FILE_JPG = 1               'JPG
    Public Const FILE_BMP = 2               'BMP
    Public Const FILE_RAW = 4               '相机输出的bayer格式文件,对于不支持bayer格式输出相机，无法保存为该格式
    Public Const FILE_PNG = 8               'PNG

    '相机中的图像传感器的工作模式 emSdkSnapMode
    Public Const CONTINUATION = 0           '连续采集模式
    Public Const SOFT_TRIGGER = 1           '软件触发模式，由软件发送指令后，传感器开始采集指定帧数的图像，采集完成后，停止输出
    Public Const EXTERNAL_TRIGGER = 2       '硬件触发模式，当接收到外部信号，传感器开始采集指定帧数的图像，采集完成后，停止输出

    '自动曝光时抗频闪的频闪 emSdkLightFrequency
    Public Const LIGHT_FREQUENCY_50HZ = 0   '50HZ,一般的灯光都是50HZ
    Public Const LIGHT_FREQUENCY_60HZ = 1   '60HZ,主要是指显示器的

    '相机的配置参数，分为A,B,C,D 4组进行保存。emSdkParameterTeam
    Public Const PARAMETER_TEAM_DEFAULT = &HFF
    Public Const PARAMETER_TEAM_A = 0
    Public Const PARAMETER_TEAM_B = 1
    Public Const PARAMETER_TEAM_C = 2
    Public Const PARAMETER_TEAM_D = 3

    '/*emSdkParameterMode 相机参数加载模式，参数加载分为从文件和从设备加载两种方式
    'PARAM_MODE_BY_MODEL:所有同型号的相机共用ABCD四组参数文件。修改
    '                   一台相机的参数文件，会影响到整个同型号的
    '                   相机参数加载。
    'PARAM_MODE_BY_NAME:所有设备名相同的相机，共用ABCD四组参数文件。
    '                默认情况下，当电脑上只接了某型号一台相机时，
    '                 设备名都是一样的，而您希望某一台相机能够加载
    '                 不同的参数文件，则可以通过修改其设备名的方式
    '                 来让其加载指定的参数文件。
    'PARAM_MODE_BY_SN:相机按照自己的唯一序列号来加载ABCD四组参数文件，
    '               序列号在出厂时已经固化在相机内，每台相机的序列号
    '               都不相同，通过这种方式，每台相机的参数文件都是独立的。
    '您可以根据自己的使用环境，灵活使用以上几种方式加载参数。例如，以
    'MV-U300为例，您希望多台该型号的相机在您的 电脑上都共用4组参数，那么就
    '使用PARAM_MODE_BY_MODEL方式;如果您希望其中某一台或者某几台MV-U300能
    '使用自己参数文件而其余的MV-U300又要使用相同的参数文件，那么使用
    'PARAM_MODE_BY_NAME方式;如果您希望每台MV-U300都使用不同的参数文件，那么
    '使用PARAM_MODE_BY_SN方式。
    '参数文件存在安装目录的 \Camera\Configs 目录下，以config为后缀名的文件。
    '*/
    Public Const PARAM_MODE_BY_MODEL = 0     '根据相机型号名从文件中加载参数，例如MV-U300
    Public Const PARAM_MODE_BY_NAME = 1      '根据设备昵称(tSdkCameraDevInfo.acFriendlyName)从文件中加载参数，例如MV-U300,该昵称可自定义
    Public Const PARAM_MODE_BY_SN = 2        '根据设备的唯一序列号从文件中加载参数，序列号在出厂时已经写入设备，每台相机拥有不同的序列号。
    Public Const PARAM_MODE_IN_DEVICE = 3    '从设备的固态存储器中加载参数。不是所有的型号都支持从相机中读写参数组，由tSdkCameraCapbility.bParamInDevice决定

    'SDK生成的相机配置页面掩码值 emSdkPropSheetMask
    Public Const PROP_SHEET_INDEX_EXPOSURE = 0
    Public Const PROP_SHEET_INDEX_ISP_COLOR = 1
    Public Const PROP_SHEET_INDEX_ISP_LUT = 2
    Public Const PROP_SHEET_INDEX_ISP_SHAPE = 3
    Public Const PROP_SHEET_INDEX_VIDEO_FORMAT = 4
    Public Const PROP_SHEET_INDEX_RESOLUTION = 5
    Public Const PROP_SHEET_INDEX_IO_CTRL = 6
    Public Const PROP_SHEET_INDEX_TRIGGER_SET = 7
    Public Const PROP_SHEET_INDEX_OVERLAY = 8
    Public Const PROP_SHEET_INDEX_DEVICE_INFO = 9

    'SDK生成的相机配置页面的回调消息类型 emSdkPropSheetMsg
    Public Const SHEET_MSG_LOAD_PARAM_DEFAULT = 0    '参数被恢复成默认后，触发该消息
    Public Const SHEET_MSG_LOAD_PARAM_GROUP = 1      '加载指定参数组，触发该消息
    Public Const SHEET_MSG_LOAD_PARAM_FROMFILE = 2   '从指定文件加载参数后，触发该消息
    Public Const SHEET_MSG_SAVE_PARAM_GROUP = 3      '当前参数组被保存时，触发该消息

    '可视化选择参考窗口的类型 emSdkRefWinType
    Public Const REF_WIN_AUTO_EXPOSURE = 0
    Public Const REF_WIN_WHITE_BALANCE = 1

    '可视化选择参考窗口的类型 emSdkResolutionMode
    Public Const RES_MODE_PREVIEW = 0
    Public Const RES_MODE_SNAPSHOT = 1

    '白平衡时色温模式 emSdkClrTmpMode
    Public Const CT_MODE_AUTO = 0        '自动识别色温
    Public Const CT_MODE_PRESET = 1      '使用指定的预设色温
    Public Const CT_MODE_USER_DEF = 2    '自定义色温(增益和矩阵)

    'LUT的颜色通道 emSdkLutChannel
    Public Const LUT_CHANNEL_ALL = 0     'R,B,G三通道同时调节
    Public Const LUT_CHANNEL_RED = 1     '红色通道
    Public Const LUT_CHANNEL_GREEN = 2   '绿色通道
    Public Const LUT_CHANNEL_BLUE = 3    '蓝色通道

    'ISP处理单元 emSdkIspProcessor
    Public Const ISP_PROCESSSOR_PC = 0       '使用PC的软件ISP模块
    Public Const ISP_PROCESSSOR_DEVICE = 1   '使用相机自带的硬件ISP模块

    '闪光灯信号控制方式 emStrobeControl
    Public Const STROBE_SYNC_WITH_TRIG_AUTO = 0       '和触发信号同步，触发后，相机进行曝光时，自动生成STROBE信号。此时，有效极性可设置(CameraSetStrobePolarity)。
    Public Const STROBE_SYNC_WITH_TRIG_MANUAL = 1     '和触发信号同步，触发后，STROBE延时指定的时间后(CameraSetStrobeDelayTime)，再持续指定时间的脉冲(CameraSetStrobePulseWidth)，有效极性可设置(CameraSetStrobePolarity)。
    Public Const STROBE_ALWAYS_HIGH = 2               '始终为高，忽略STROBE信号的其他设置
    Public Const STROBE_ALWAYS_LOW = 3                '始终为低，忽略STROBE信号的其他设置


    '硬件外触发的信号种类 emExtTrigSignal
    Public Const EXT_TRIG_LEADING_EDGE = 0        '上升沿触发，默认为该方式
    Public Const EXT_TRIG_TRAILING_EDGE = 1       '下降沿触发
    Public Const EXT_TRIG_HIGH_LEVEL = 2          '高电平触发,电平宽度决定曝光时间，仅部分型号的相机支持电平触发方式。
    Public Const EXT_TRIG_LOW_LEVEL = 3           '低电平触发,


    '硬件外触发时的快门方式 emExtTrigShutterMode
    Public Const EXT_TRIG_EXP_STANDARD = 0        '标准方式，默认为该方式。
    Public Const EXT_TRIG_EXP_GRR = 1             '全局复位方式，部分滚动快门的CMOS型号的相机支持该方式，配合外部机械快门，可以达到全局快门的效果，适合拍高速运动的物体

    '相机的设备信息，只读信息，请勿修改
    Public Structure tSdkCameraDevInfo
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=32)> _
        Dim acProductSeries() As Byte '产品系列
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=32)> _
        Dim acProductName() As Byte   '产品名称
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=32)> _
        Dim acFriendlyName() As Byte  '昵称，后加#和索引号来区分
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=32)> _
        Dim acLinkName() As Byte      '驱动名称,内部使用
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=32)> _
        Dim acDriverVersion() As Byte '驱动版本
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=32)> _
        Dim acSensorType() As Byte    'sensor类型
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=32)> _
        Dim acPortType() As Byte      '接口类型
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=32)> _
        Dim acSn() As Byte            '产品唯一序列号
        Dim uInstance As Integer         '该型号相机在该电脑上的实例索引号，用于区分同型号多相机
    End Structure

    '相机的分辨率设定范围
    Public Structure tSdkResolutionRange
        Dim iHeightMax As Integer      '图像最大高度
        Dim iHeightMin As Integer      '图像最小高度
        Dim iWidthMax As Integer       '图像最大宽度
        Dim iWidthMin As Integer       '图像最小宽度
        Dim uSkipModeMask As Integer   'SKIP模式掩码，为0，表示不支持SKIP 。bit0为1,表示支持SKIP 2x2 ;bit1为1，表示支持SKIP 3x3....
        Dim uBinSumModeMask As Integer 'BIN(求和)模式掩码，为0，表示不支持BIN 。bit0为1,表示支持BIN 2x2 ;bit1为1，表示支持BIN 3x3....
        Dim uBinAverageModeMask As Integer  'BIN(求均值)模式掩码，为0，表示不支持BIN 。bit0为1,表示支持BIN 2x2 ;bit1为1，表示支持BIN 3x3....
        Dim uResampleMask As Integer   '硬件重采样的掩码
    End Structure


    '相机的分辨率描述
    Public Structure tSdkImageResolution
        Dim iIndex As Integer              ' 索引号，[0,N]表示预设的分辨率(N 为预设分辨率的最大个数，一般不超过20),OXFF 表示自定义分辨率(ROI)
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=32)> _
        Dim acDescription() As Byte     ' 该分辨率的描述信息。仅预设分辨率时该信息有效。自定义分辨率可忽略该信息
        Dim uBinSumMode As Integer         ' 是否BIN的尺寸,16bit为1表示sum，为0表示average；低16位表示BIN矩阵大小，为0表示禁止BIN模式,范围不能超过tSdkResolutionRange中uBinModeMask
        Dim uBinAverageMode As Integer     ' BIN(求均值)的模式,范围不能超过tSdkResolutionRange中uBinAverageModeMask
        Dim uSkipMode As Integer           ' 是否SKIP的尺寸，为0表示禁止SKIP模式。1表示SKIP2X2，2表示SKIP3X3，以此类推,范围不能超过tSdkResolutionRange中uSkipModeMask
        Dim uResampleMask As Integer       ' 硬件重采样的掩码
        Dim iHOffsetFOV As Integer         ' 采集视场相对于Sensor最大视场左上角的垂直偏移
        Dim iVOffsetFOV As Integer         ' 采集视场相对于Sensor最大视场左上角的水平偏移
        Dim iWidthFOV As Integer           ' 采集视场的宽度
        Dim iHeightFOV As Integer          ' 采集视场的高度
        Dim iWidth As Integer              ' 相机最终输出的图像的宽度
        Dim iHeight As Integer             ' 相机最终输出的图像的高度
        Dim iWidthZoomHd As Integer        ' 硬件缩放的宽度,不需要进行此操作的分辨率，此变量设置为0.
        Dim iHeightZoomHd As Integer       ' 硬件缩放的高度,不需要进行此操作的分辨率，此变量设置为0.
        Dim iWidthZoomSw As Integer        ' 软件缩放的宽度,不需要进行此操作的分辨率，此变量设置为0.
        Dim iHeightZoomSw As Integer       ' 软件缩放的高度,不需要进行此操作的分辨率，此变量设置为0.
    End Structure

    '相机白平衡模式描述信息
    Public Structure tSdkColorTemperatureDes
        Dim iIndex As Integer              '模式索引号
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=32)> _
        Dim acDescription() As Byte     '描述信息
    End Structure

    '相机帧率描述信息
    Public Structure tSdkFrameSpeed
        Dim iIndex As Integer    ' 帧率索引号，一般0对应于低速模式，1对应于普通模式，2对应于高速模式
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=32)> _
        Dim acDescription() As Byte    ' 描述信息
    End Structure

    '相机曝光功能范围定义
    Public Structure tSdkExpose
        Dim uiTargetMin As Integer         '自动曝光亮度目标最小值
        Dim uiTargetMax As Integer         '自动曝光亮度目标最大值
        Dim uiAnalogGainMin As Integer     '模拟增益的最小值，单位为fAnalogGainStep中定义
        Dim uiAnalogGainMax As Integer     '模拟增益的最大值，单位为fAnalogGainStep中定义
        Dim fAnalogGainStep As Single   '模拟增益每增加1，对应的增加的放大倍数。例如，uiAnalogGainMin一般为16，fAnalogGainStep一般为0.125，那么最小放大倍数就是16*0.125 = 2倍
        Dim uiExposeTimeMin As Integer     '手动模式下，曝光时间的最小值，单位:行。根据CameraGetExposureLineTime可以获得一行对应的时间(微秒),从而得到整帧的曝光时间
        Dim uiExposeTimeMax As Integer     '手动模式下，曝光时间的最大值，单位:行
    End Structure

    '触发模式描述
    Public Structure tSdkTrigger
        Dim iIndex As Integer                  '模式索引号
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=32)> _
        Dim acDescription() As Byte         '该模式的描述信息
    End Structure

    '传输分包大小描述(主要是针对网络相机有效)
    Public Structure tSdkPackLength
        Dim iIndex As Integer                  '分包大小索引号
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=32)> _
        Dim acDescription() As Byte       '对应的描述信息
    End Structure

    '预设的LUT表描述
    Public Structure tSdkPresetLut
        Dim iIndex As Integer                  '编号
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=32)> _
        Dim acDescription() As Byte         '描述信息
    End Structure

    'AE算法描述
    Public Structure tSdkAeAlgorithm
        Dim iIndex As Integer              '编号
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=32)> _
        Dim acDescription() As Byte     '描述信息
    End Structure

    'RAW转RGB算法描述
    Public Structure tSdkBayerDecodeAlgorithm
        Dim iIndex As Integer               '编号
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=32)> _
        Dim acDescription() As Byte      '描述信息
    End Structure

    '帧率统计信息
    Public Structure tSdkFrameStatistic
        Dim iTotal As Integer                '当前采集的总帧数（包括错误帧）
        Dim iCapture As Integer              '当前采集的有效帧的数量
        Dim iLost As Integer                 '当前丢帧的数量
    End Structure

    '相机输出的图像数据格式
    Public Structure tSdkMediaType
        Dim iIndex As Integer              '格式种类编号
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=32)> _
        Dim acDescription() As Byte     '描述信息
        Dim iMediaType As Integer          '对应的图像格式编码，如CAMERA_MEDIA_TYPE_BAYGR8，在本文件中有定义。
    End Structure

    '伽马的设定范围
    Public Structure tGammaRange
        Dim iMin As Integer       '最小值
        Dim iMax As Integer       '最大值
    End Structure

    '对比度的设定范围
    Public Structure tContrastRange
        Dim iMin As Integer       '最小值
        Dim iMax As Integer       '最大值
    End Structure

    'RGB三通道数字增益的设定范围
    Public Structure tRgbGainRange
        Dim iRGainMin As Integer               '红色增益的最小值
        Dim iRGainMax As Integer               '红色增益的最大值
        Dim iGGainMin As Integer               '绿色增益的最小值
        Dim iGGainMax As Integer               '绿色增益的最大值
        Dim iBGainMin As Integer               '蓝色增益的最小值
        Dim iBGainMax As Integer               '蓝色增益的最大值
    End Structure

    '饱和度设定的范围
    Public Structure tSaturationRange
        Dim iMin As Integer            '最小值
        Dim iMax As Integer            '最大值
    End Structure

    '锐化的设定范围
    Public Structure tSharpnessRange
        Dim iMin As Integer                '最小值
        Dim iMax As Integer                '最大值
    End Structure

    'ISP模块的使能信息
    Public Structure tSdkIspCapacity
        Dim bMonoSensor As Integer             '表示该型号相机是否为黑白相机,如果是黑白相机，则颜色相关的功能都无法调节
        Dim bWbOnce As Integer                 '表示该型号相机是否支持手动白平衡功能
        Dim bAutoWb As Integer                 '表示该型号相机是否支持自动白平衡功能
        Dim bAutoExposure As Integer           '表示该型号相机是否支持自动曝光功能
        Dim bManualExposure As Integer         '表示该型号相机是否支持手动曝光功能
        Dim bAntiFlick As Integer              '表示该型号相机是否支持抗频闪功能
        Dim bDeviceIsp As Integer              '表示该型号相机是否支持硬件ISP功能
        Dim bForceUseDeviceIsp As Integer      'bDeviceIsp和bForceUseDeviceIsp同时为TRUE时，表示强制只用硬件ISP，不可取消。
        Dim bZoomHD As Integer                 '相机硬件是否支持图像缩放输出(只能是缩小)。
    End Structure

    '/* 定义整合的设备描述信息，这些信息可以用于动态构建UI */
    Public Structure tSdkCameraCapbility
        Dim pTriggerDesc As IntPtr            ' 触发模式 指向tSdkTrigger数组的地址，用Long类型表示指针
        Dim iTriggerDesc As Integer           ' 触发模式的个数，即pTriggerDesc数组的大小
        Dim pImageSizeDesc As IntPtr          ' 预设分辨率选择 tSdkImageResolution
        Dim iImageSizeDesc As Integer         ' 预设分辨率的个数，即pImageSizeDesc数组的大小
        Dim pClrTempDesc As IntPtr            ' 预设色温模式，用于白平衡 tSdkColorTemperatureDes
        Dim iClrTempDesc As Integer
        Dim pMediaTypeDesc As IntPtr          ' 相机输出图像格式 tSdkMediaType
        Dim iMediaTypdeDesc As Integer        ' 相机输出图像格式的种类个数，即pMediaTypeDesc数组的大小。
        Dim pFrameSpeedDesc As IntPtr         ' 可调节帧速类型，对应界面上普通 高速 和超级三种速度设置 tSdkFrameSpeed
        Dim iFrameSpeedDesc As Integer        ' 可调节帧速类型的个数，即pFrameSpeedDesc数组的大小。
        Dim pPackLenDesc As IntPtr            ' 传输包长度，一般用于网络设备 tSdkPackLength
        Dim iPackLenDesc As Integer           ' 可供选择的传输分包长度的个数，即pPackLenDesc数组的大小。
        Dim iOutputIoCounts As Integer        ' 可编程输出IO的个数
        Dim iInputIoCounts As Integer         ' 可编程输入IO的个数
        Dim pPresetLutDesc As IntPtr          ' 相机预设的LUT表 tSdkPresetLut
        Dim iPresetLut As Integer             ' 相机预设的LUT表的个数，即pPresetLutDesc数组的大小
        Dim iUserDataMaxLen As Integer        ' 指示该相机中用于保存用户数据区的最大长度。为0表示无。
        Dim bParamInDevice As Integer         ' 指示该设备是否支持从设备中读写参数组。1为支持，0不支持。
        Dim pAeAlmSwDesc As IntPtr            ' 软件自动曝光算法描述, 指向tSdkTrigger数组的地址，用Long类型表示指针
        Dim iAeAlmSwDesc As Integer           ' 软件自动曝光算法个数
        Dim pAeAlmHdDesc As IntPtr            ' 硬件自动曝光算法描述, 指向tSdkTrigger数组的地址，用Long类型表示指针
        Dim iAeAlmHdDesc As Integer           ' 硬件自动曝光算法个数
        Dim pBayerDecAlmSwDesc As IntPtr      ' 软件Bayer转换为RGB数据的算法描述, tSdkBayerDecodeAlgorithm，用Long类型表示指针
        Dim iBayerDecAlmSwDesc As Integer     ' 软件Bayer转换为RGB数据的算法个数
        Dim pBayerDecAlmHdDesc As IntPtr      ' 硬件Bayer转换为RGB数据的算法描述, tSdkBayerDecodeAlgorithm，用Long类型表示指针
        Dim iBayerDecAlmHdDesc As Integer     ' 硬件Bayer转换为RGB数据的算法个数
        '/* 图像参数的调节范围定义,用于动态构建UI*/
        Dim sExposeDesc As tSdkExpose               ' 曝光的范围值
        Dim sResolutionRange As tSdkResolutionRange ' 分辨率范围描述
        Dim sRgbGainRange As tRgbGainRange          ' 图像数字增益范围描述
        Dim sSaturationRange As tSaturationRange    ' 饱和度范围描述
        Dim sGammaRange As tGammaRange              ' 伽马范围描述
        Dim sContrastRange As tContrastRange        ' 对比度范围描述
        Dim sSharpnessRange As tSharpnessRange      ' 锐化范围描述
        Dim sIspCapacity As tSdkIspCapacity         ' ISP能力描述
    End Structure


    '图像帧头信息
    Public Structure tSdkFrameHead
        Dim uiMediaType As Integer         ' 图像格式,Image Format
        Dim uBytes As Integer              ' 图像数据字节数,Total bytes
        Dim iWidth As Integer              ' 高度 Image width
        Dim iHeight As Integer             ' 宽度 Image height
        Dim iWidthZoomSw As Integer        ' 软件缩放的宽度,不需要进行软件裁剪的图像，此变量设置为0.
        Dim iHeightZoomSw As Integer       ' 软件缩放的高度,不需要进行软件裁剪的图像，此变量设置为0.
        Dim bIsTrigger As Integer          ' 指示是否为触发帧 is trigger
        Dim uiTimeStamp As Integer         ' 该帧的采集时间，单位0.1毫秒
        Dim uiExpTime As Integer           ' 当前图像的曝光值，单位为微秒us
        Dim fAnalogGain As Single       ' 当前图像的模拟增益倍数
        Dim iGamma As Integer              ' 该帧图像的伽马设定值，仅当LUT模式为动态参数生成时有效，其余模式下为-1
        Dim iContrast As Integer           ' 该帧图像的对比度设定值，仅当LUT模式为动态参数生成时有效，其余模式下为-1
        Dim iSaturation As Integer         ' 该帧图像的饱和度设定值，对于黑白相机无意义，为0
        Dim fRgain As Single            ' 该帧图像处理的红色数字增益倍数，对于黑白相机无意义，为1
        Dim fGgain As Single            ' 该帧图像处理的绿色数字增益倍数，对于黑白相机无意义，为1
        Dim fBgain As Single            ' 该帧图像处理的蓝色数字增益倍数，对于黑白相机无意义，为1
    End Structure


    '图像帧描述
    Public Structure tSdkFrame
        Dim head As tSdkFrameHead       '帧头
        Dim pBuffer As IntPtr             '数据区
    End Structure

    'BMP图像头定义
    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode, Pack:=1)> _
    Public Structure BITMAPFILEHEADER
        Dim bfType As UInt16
        Dim bfSize As UInt32
        Dim bfReserved1 As UInt16
        Dim bfReserved2 As UInt16
        Dim bfOffBits As UInt32
    End Structure
    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode, Pack:=1)> _
    Public Structure BITMAPINFOHEADER
        Dim biSize As UInt32
        Dim biWidth As Integer
        Dim biHeight As Integer
        Dim biPlanes As UInt16
        Dim biBitCount As UInt16
        Dim biCompression As UInt32
        Dim biSizeImage As UInt32
        Dim biXPelsPerMeter As Integer
        Dim biYPelsPerMeter As Integer
        Dim biClrUsed As Integer
        Dim biClrImportant As Integer
        Dim BI_RGB As Integer
    End Structure


    'Grabber统计信息
    Public Structure tSdkGrabberStat
        Dim Width As Integer            ' 帧图像宽度
        Dim Height As Integer           ' 帧图像高度
        Dim Disp As Integer             ' 显示帧数量
        Dim Capture As Integer          ' 采集的有效帧的数量
        Dim Lost As Integer             ' 丢帧的数量
        Dim Err As Integer              ' 错帧的数量
        Dim DispFps As Single           ' 显示帧率
        Dim CapFps As Single            ' 捕获帧率
    End Structure


    '----------------------------IMAGE FORMAT DEFINE------------------------------------
    '----------------------------图像格式定义-------------------------------------------
    Public Const CAMERA_MEDIA_TYPE_MONO = &H1000000
    Public Const CAMERA_MEDIA_TYPE_RGB = &H2000000
    Public Const CAMERA_MEDIA_TYPE_COLOR = &H2000000
    Public Const CAMERA_MEDIA_TYPE_CUSTOM = &H80000000
    Public Const CAMERA_MEDIA_TYPE_COLOR_MASK = &HFF000000
    Public Const CAMERA_MEDIA_TYPE_OCCUPY1BIT = &H10000
    Public Const CAMERA_MEDIA_TYPE_OCCUPY2BIT = &H20000
    Public Const CAMERA_MEDIA_TYPE_OCCUPY4BIT = &H40000
    Public Const CAMERA_MEDIA_TYPE_OCCUPY8BIT = &H80000
    Public Const CAMERA_MEDIA_TYPE_OCCUPY10BIT = &HA0000
    Public Const CAMERA_MEDIA_TYPE_OCCUPY12BIT = &HC0000
    Public Const CAMERA_MEDIA_TYPE_OCCUPY16BIT = &H100000
    Public Const CAMERA_MEDIA_TYPE_OCCUPY24BIT = &H180000
    Public Const CAMERA_MEDIA_TYPE_OCCUPY32BIT = &H200000
    Public Const CAMERA_MEDIA_TYPE_OCCUPY36BIT = &H240000
    Public Const CAMERA_MEDIA_TYPE_OCCUPY48BIT = &H300000
    Public Const CAMERA_MEDIA_TYPE_EFFECTIVE_PIXEL_SIZE_MASK = &HFF0000
    Public Const CAMERA_MEDIA_TYPE_EFFECTIVE_PIXEL_SIZE_SHIFT = 16


    Public Const CAMERA_MEDIA_TYPE_ID_MASK = &HFFFF
    Public Const CAMERA_MEDIA_TYPE_COUNT = &H46

    'mono*/
    Public Const CAMERA_MEDIA_TYPE_MONO1P = (CAMERA_MEDIA_TYPE_MONO Or CAMERA_MEDIA_TYPE_OCCUPY1BIT Or &H37)
    Public Const CAMERA_MEDIA_TYPE_MONO2P = (CAMERA_MEDIA_TYPE_MONO Or CAMERA_MEDIA_TYPE_OCCUPY2BIT Or &H38)
    Public Const CAMERA_MEDIA_TYPE_MONO4P = (CAMERA_MEDIA_TYPE_MONO Or CAMERA_MEDIA_TYPE_OCCUPY4BIT Or &H39)
    Public Const CAMERA_MEDIA_TYPE_MONO8 = (CAMERA_MEDIA_TYPE_MONO Or CAMERA_MEDIA_TYPE_OCCUPY8BIT Or &H1)
    Public Const CAMERA_MEDIA_TYPE_MONO8S = (CAMERA_MEDIA_TYPE_MONO Or CAMERA_MEDIA_TYPE_OCCUPY8BIT Or &H2)
    Public Const CAMERA_MEDIA_TYPE_MONO10 = (CAMERA_MEDIA_TYPE_MONO Or CAMERA_MEDIA_TYPE_OCCUPY16BIT Or &H3)
    Public Const CAMERA_MEDIA_TYPE_MONO10_PACKED = (CAMERA_MEDIA_TYPE_MONO Or CAMERA_MEDIA_TYPE_OCCUPY12BIT Or &H4)
    Public Const CAMERA_MEDIA_TYPE_MONO12 = (CAMERA_MEDIA_TYPE_MONO Or CAMERA_MEDIA_TYPE_OCCUPY16BIT Or &H5)
    Public Const CAMERA_MEDIA_TYPE_MONO12_PACKED = (CAMERA_MEDIA_TYPE_MONO Or CAMERA_MEDIA_TYPE_OCCUPY12BIT Or &H6)
    Public Const CAMERA_MEDIA_TYPE_MONO14 = (CAMERA_MEDIA_TYPE_MONO Or CAMERA_MEDIA_TYPE_OCCUPY16BIT Or &H25)
    Public Const CAMERA_MEDIA_TYPE_MONO16 = (CAMERA_MEDIA_TYPE_MONO Or CAMERA_MEDIA_TYPE_OCCUPY16BIT Or &H7)

    'Bayer */
    Public Const CAMERA_MEDIA_TYPE_BAYGR8 = (CAMERA_MEDIA_TYPE_MONO Or CAMERA_MEDIA_TYPE_OCCUPY8BIT Or &H8)
    Public Const CAMERA_MEDIA_TYPE_BAYRG8 = (CAMERA_MEDIA_TYPE_MONO Or CAMERA_MEDIA_TYPE_OCCUPY8BIT Or &H9)
    Public Const CAMERA_MEDIA_TYPE_BAYGB8 = (CAMERA_MEDIA_TYPE_MONO Or CAMERA_MEDIA_TYPE_OCCUPY8BIT Or &HA)
    Public Const CAMERA_MEDIA_TYPE_BAYBG8 = (CAMERA_MEDIA_TYPE_MONO Or CAMERA_MEDIA_TYPE_OCCUPY8BIT Or &HB)

    Public Const CAMERA_MEDIA_TYPE_BAYGR10_MIPI = (CAMERA_MEDIA_TYPE_MONO Or CAMERA_MEDIA_TYPE_OCCUPY10BIT Or &H26)
    Public Const CAMERA_MEDIA_TYPE_BAYRG10_MIPI = (CAMERA_MEDIA_TYPE_MONO Or CAMERA_MEDIA_TYPE_OCCUPY10BIT Or &H27)
    Public Const CAMERA_MEDIA_TYPE_BAYGB10_MIPI = (CAMERA_MEDIA_TYPE_MONO Or CAMERA_MEDIA_TYPE_OCCUPY10BIT Or &H28)
    Public Const CAMERA_MEDIA_TYPE_BAYBG10_MIPI = (CAMERA_MEDIA_TYPE_MONO Or CAMERA_MEDIA_TYPE_OCCUPY10BIT Or &H29)


    Public Const CAMERA_MEDIA_TYPE_BAYGR10 = (CAMERA_MEDIA_TYPE_MONO Or CAMERA_MEDIA_TYPE_OCCUPY16BIT Or &HC)
    Public Const CAMERA_MEDIA_TYPE_BAYRG10 = (CAMERA_MEDIA_TYPE_MONO Or CAMERA_MEDIA_TYPE_OCCUPY16BIT Or &HD)
    Public Const CAMERA_MEDIA_TYPE_BAYGB10 = (CAMERA_MEDIA_TYPE_MONO Or CAMERA_MEDIA_TYPE_OCCUPY16BIT Or &HE)
    Public Const CAMERA_MEDIA_TYPE_BAYBG10 = (CAMERA_MEDIA_TYPE_MONO Or CAMERA_MEDIA_TYPE_OCCUPY16BIT Or &HF)

    Public Const CAMERA_MEDIA_TYPE_BAYGR12 = (CAMERA_MEDIA_TYPE_MONO Or CAMERA_MEDIA_TYPE_OCCUPY16BIT Or &H10)
    Public Const CAMERA_MEDIA_TYPE_BAYRG12 = (CAMERA_MEDIA_TYPE_MONO Or CAMERA_MEDIA_TYPE_OCCUPY16BIT Or &H11)
    Public Const CAMERA_MEDIA_TYPE_BAYGB12 = (CAMERA_MEDIA_TYPE_MONO Or CAMERA_MEDIA_TYPE_OCCUPY16BIT Or &H12)
    Public Const CAMERA_MEDIA_TYPE_BAYBG12 = (CAMERA_MEDIA_TYPE_MONO Or CAMERA_MEDIA_TYPE_OCCUPY16BIT Or &H13)


    Public Const CAMERA_MEDIA_TYPE_BAYGR10_PACKED = (CAMERA_MEDIA_TYPE_MONO Or CAMERA_MEDIA_TYPE_OCCUPY12BIT Or &H26)
    Public Const CAMERA_MEDIA_TYPE_BAYRG10_PACKED = (CAMERA_MEDIA_TYPE_MONO Or CAMERA_MEDIA_TYPE_OCCUPY12BIT Or &H27)
    Public Const CAMERA_MEDIA_TYPE_BAYGB10_PACKED = (CAMERA_MEDIA_TYPE_MONO Or CAMERA_MEDIA_TYPE_OCCUPY12BIT Or &H28)
    Public Const CAMERA_MEDIA_TYPE_BAYBG10_PACKED = (CAMERA_MEDIA_TYPE_MONO Or CAMERA_MEDIA_TYPE_OCCUPY12BIT Or &H29)

    Public Const CAMERA_MEDIA_TYPE_BAYGR12_PACKED = (CAMERA_MEDIA_TYPE_MONO Or CAMERA_MEDIA_TYPE_OCCUPY12BIT Or &H2A)
    Public Const CAMERA_MEDIA_TYPE_BAYRG12_PACKED = (CAMERA_MEDIA_TYPE_MONO Or CAMERA_MEDIA_TYPE_OCCUPY12BIT Or &H2B)
    Public Const CAMERA_MEDIA_TYPE_BAYGB12_PACKED = (CAMERA_MEDIA_TYPE_MONO Or CAMERA_MEDIA_TYPE_OCCUPY12BIT Or &H2C)
    Public Const CAMERA_MEDIA_TYPE_BAYBG12_PACKED = (CAMERA_MEDIA_TYPE_MONO Or CAMERA_MEDIA_TYPE_OCCUPY12BIT Or &H2D)

    Public Const CAMERA_MEDIA_TYPE_BAYGR16 = (CAMERA_MEDIA_TYPE_MONO Or CAMERA_MEDIA_TYPE_OCCUPY16BIT Or &H2E)
    Public Const CAMERA_MEDIA_TYPE_BAYRG16 = (CAMERA_MEDIA_TYPE_MONO Or CAMERA_MEDIA_TYPE_OCCUPY16BIT Or &H2F)
    Public Const CAMERA_MEDIA_TYPE_BAYGB16 = (CAMERA_MEDIA_TYPE_MONO Or CAMERA_MEDIA_TYPE_OCCUPY16BIT Or &H30)
    Public Const CAMERA_MEDIA_TYPE_BAYBG16 = (CAMERA_MEDIA_TYPE_MONO Or CAMERA_MEDIA_TYPE_OCCUPY16BIT Or &H31)

    'RGB */
    Public Const CAMERA_MEDIA_TYPE_RGB8 = (CAMERA_MEDIA_TYPE_COLOR Or CAMERA_MEDIA_TYPE_OCCUPY24BIT Or &H14)
    Public Const CAMERA_MEDIA_TYPE_BGR8 = (CAMERA_MEDIA_TYPE_COLOR Or CAMERA_MEDIA_TYPE_OCCUPY24BIT Or &H15)
    Public Const CAMERA_MEDIA_TYPE_RGBA8 = (CAMERA_MEDIA_TYPE_COLOR Or CAMERA_MEDIA_TYPE_OCCUPY32BIT Or &H16)
    Public Const CAMERA_MEDIA_TYPE_BGRA8 = (CAMERA_MEDIA_TYPE_COLOR Or CAMERA_MEDIA_TYPE_OCCUPY32BIT Or &H17)
    Public Const CAMERA_MEDIA_TYPE_RGB10 = (CAMERA_MEDIA_TYPE_COLOR Or CAMERA_MEDIA_TYPE_OCCUPY48BIT Or &H18)
    Public Const CAMERA_MEDIA_TYPE_BGR10 = (CAMERA_MEDIA_TYPE_COLOR Or CAMERA_MEDIA_TYPE_OCCUPY48BIT Or &H19)
    Public Const CAMERA_MEDIA_TYPE_RGB12 = (CAMERA_MEDIA_TYPE_COLOR Or CAMERA_MEDIA_TYPE_OCCUPY48BIT Or &H1A)
    Public Const CAMERA_MEDIA_TYPE_BGR12 = (CAMERA_MEDIA_TYPE_COLOR Or CAMERA_MEDIA_TYPE_OCCUPY48BIT Or &H1B)
    Public Const CAMERA_MEDIA_TYPE_RGB16 = (CAMERA_MEDIA_TYPE_COLOR Or CAMERA_MEDIA_TYPE_OCCUPY48BIT Or &H33)
    Public Const CAMERA_MEDIA_TYPE_RGB10V1_PACKED = (CAMERA_MEDIA_TYPE_COLOR Or CAMERA_MEDIA_TYPE_OCCUPY32BIT Or &H1C)
    Public Const CAMERA_MEDIA_TYPE_RGB10P32 = (CAMERA_MEDIA_TYPE_COLOR Or CAMERA_MEDIA_TYPE_OCCUPY32BIT Or &H1D)
    Public Const CAMERA_MEDIA_TYPE_RGB12V1_PACKED = (CAMERA_MEDIA_TYPE_COLOR Or CAMERA_MEDIA_TYPE_OCCUPY36BIT Or &H34)
    Public Const CAMERA_MEDIA_TYPE_RGB565P = (CAMERA_MEDIA_TYPE_COLOR Or CAMERA_MEDIA_TYPE_OCCUPY16BIT Or &H35)
    Public Const CAMERA_MEDIA_TYPE_BGR565P = (CAMERA_MEDIA_TYPE_COLOR Or CAMERA_MEDIA_TYPE_OCCUPY16BIT Or &H36)

    'YUV and YCbCr*/
    Public Const CAMERA_MEDIA_TYPE_YUV411_8_UYYVYY = (CAMERA_MEDIA_TYPE_COLOR Or CAMERA_MEDIA_TYPE_OCCUPY12BIT Or &H1E)
    Public Const CAMERA_MEDIA_TYPE_YUV422_8_UYVY = (CAMERA_MEDIA_TYPE_COLOR Or CAMERA_MEDIA_TYPE_OCCUPY16BIT Or &H1F)
    Public Const CAMERA_MEDIA_TYPE_YUV422_8 = (CAMERA_MEDIA_TYPE_COLOR Or CAMERA_MEDIA_TYPE_OCCUPY16BIT Or &H32)
    Public Const CAMERA_MEDIA_TYPE_YUV8_UYV = (CAMERA_MEDIA_TYPE_COLOR Or CAMERA_MEDIA_TYPE_OCCUPY24BIT Or &H20)
    Public Const CAMERA_MEDIA_TYPE_YCBCR8_CBYCR = (CAMERA_MEDIA_TYPE_COLOR Or CAMERA_MEDIA_TYPE_OCCUPY24BIT Or &H3A)
    'CAMERA_MEDIA_TYPE_YCBCR422_8 : YYYYCbCrCbCr
    Public Const CAMERA_MEDIA_TYPE_YCBCR422_8 = (CAMERA_MEDIA_TYPE_COLOR Or CAMERA_MEDIA_TYPE_OCCUPY16BIT Or &H3B)
    Public Const CAMERA_MEDIA_TYPE_YCBCR422_8_CBYCRY = (CAMERA_MEDIA_TYPE_COLOR Or CAMERA_MEDIA_TYPE_OCCUPY16BIT Or &H43)
    Public Const CAMERA_MEDIA_TYPE_YCBCR411_8_CBYYCRYY = (CAMERA_MEDIA_TYPE_COLOR Or CAMERA_MEDIA_TYPE_OCCUPY12BIT Or &H3C)
    Public Const CAMERA_MEDIA_TYPE_YCBCR601_8_CBYCR = (CAMERA_MEDIA_TYPE_COLOR Or CAMERA_MEDIA_TYPE_OCCUPY24BIT Or &H3D)
    Public Const CAMERA_MEDIA_TYPE_YCBCR601_422_8 = (CAMERA_MEDIA_TYPE_COLOR Or CAMERA_MEDIA_TYPE_OCCUPY16BIT Or &H3E)
    Public Const CAMERA_MEDIA_TYPE_YCBCR601_422_8_CBYCRY = (CAMERA_MEDIA_TYPE_COLOR Or CAMERA_MEDIA_TYPE_OCCUPY16BIT Or &H44)
    Public Const CAMERA_MEDIA_TYPE_YCBCR601_411_8_CBYYCRYY = (CAMERA_MEDIA_TYPE_COLOR Or CAMERA_MEDIA_TYPE_OCCUPY12BIT Or &H3F)
    Public Const CAMERA_MEDIA_TYPE_YCBCR709_8_CBYCR = (CAMERA_MEDIA_TYPE_COLOR Or CAMERA_MEDIA_TYPE_OCCUPY24BIT Or &H40)
    Public Const CAMERA_MEDIA_TYPE_YCBCR709_422_8 = (CAMERA_MEDIA_TYPE_COLOR Or CAMERA_MEDIA_TYPE_OCCUPY16BIT Or &H41)
    Public Const CAMERA_MEDIA_TYPE_YCBCR709_422_8_CBYCRY = (CAMERA_MEDIA_TYPE_COLOR Or CAMERA_MEDIA_TYPE_OCCUPY16BIT Or &H45)
    Public Const CAMERA_MEDIA_TYPE_YCBCR709_411_8_CBYYCRYY = (CAMERA_MEDIA_TYPE_COLOR Or CAMERA_MEDIA_TYPE_OCCUPY12BIT Or &H42)

    'RGB Planar */
    Public Const CAMERA_MEDIA_TYPE_RGB8_PLANAR = (CAMERA_MEDIA_TYPE_COLOR Or CAMERA_MEDIA_TYPE_OCCUPY24BIT Or &H21)
    Public Const CAMERA_MEDIA_TYPE_RGB10_PLANAR = (CAMERA_MEDIA_TYPE_COLOR Or CAMERA_MEDIA_TYPE_OCCUPY48BIT Or &H22)
    Public Const CAMERA_MEDIA_TYPE_RGB12_PLANAR = (CAMERA_MEDIA_TYPE_COLOR Or CAMERA_MEDIA_TYPE_OCCUPY48BIT Or &H23)
    Public Const CAMERA_MEDIA_TYPE_RGB16_PLANAR = (CAMERA_MEDIA_TYPE_COLOR Or CAMERA_MEDIA_TYPE_OCCUPY48BIT Or &H24)

End Module
