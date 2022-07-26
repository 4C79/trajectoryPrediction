//程序执行流程：
//1,initialization中，扫描相机，如果没有连接到相机，程序直接退出
//2,FormCreate中，初始化相机(申请内存、初始化显示、设置图像抓取回调函数CameraGrabCallBack)
//3,CameraGrabCallBack,图像抓取回调函数，SDK中每捕获到一帧，该函数会被调用，
//  该例程中，CameraGrabCallBack里实现了图像的显示。
//BIG5 TRANS ALLOWED
unit Main;

interface


{添加相机的SDK单元 (CameraApi,CameraDefine, CameraStatus) }
uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, ExtCtrls, TSnapshot, CameraApi, CameraDefine, CameraStatus;

type
  TForm1 = class(TForm)
    BtnPlay: TButton;
    BtnSettings: TButton;
    StatusBar: TStaticText;
    InfoUpdateTimer: TTimer;
    Displayer: TPanel;
    GrabTimer: TTimer;
    Displayer2: TPanel;
    Camera2Settings: TButton;
    BtnPlay2: TButton;
    procedure FormCreate(Sender: TObject);
    procedure BtnSettingsClick(Sender: TObject);
    procedure InfoUpdateTimerTimer(Sender: TObject);
    procedure Camera2SettingsClick(Sender: TObject);
    procedure BtnPlay2Click(Sender: TObject);
    procedure BtnPlayClick(Sender: TObject);


  private
    { Private declarations }
  public
    { Public declarations }

  end;

var
  Form1:TForm1;
  m_hCamera1:Integer;  //相机1句柄
  m_hCamera2:Integer;  //相机2句柄
  iStatus:Integer;    //函数返回值
  CameraList:array[1..8] of tSdkCameraDevInfo; //相机列表，这里数组大小为8，表示最多只扫描8个设备
                                               //如果需要扫描更多的设备，请将该数组大小改为您想要的数。
  iCameraNumber:Integer;        //相机的个数
  m_iDisplayCount:Integer;      //已经显示的帧数
  m_pRgbBufferAlligned1:PByte;   //相机1经过16字节对齐修正后的地址
  sCameraCapability1:tSdkCameraCapbility; //相机1特性描述结构体
  m_pRgbBuffer2:PByte;           //相机2 图像处理的缓冲区
  m_pRgbBufferAlligned2:PByte;   //相机2 m_pRgbBuffer经过16字节对齐修正后的地址
  sCameraCapability2:tSdkCameraCapbility; //相机2特性描述结构体

  m_bPause:LongBool;                     //用来指示相机1是否暂停采集
  m_bPause2:LongBool;                    //用来指示相机2是否暂停采集

  m_iLastFrameWidth:Integer;             //保存相机1上一次捕获到的图像的宽度
  m_iLastFrameHeight:Integer;            //保存相机1上一次捕获到的图像的高度

  m_iLastFrameWidth2:Integer;             //保存相机2上一次捕获到的图像的宽度
  m_iLastFrameHeight2:Integer;            //保存相机2上一次捕获到的图像的高度

implementation

{$R *.dfm}

//相机1图像捕获的回调函数，例程中使用该方式来获取图像，您也可用采用多线程或者定时器，然后
//不断调用CameraGetImageBuffer来获取图像。
procedure CameraGrabCallBack(
hCamera:CameraHandle;
pFrameBuffer:PByte;
pFrameHead:PtSdkFrameHead;
pContext:Pointer
); stdcall;
var
pDisplayFram:PInteger;

begin

pDisplayFram :=  PInteger(pContext);
pDisplayFram^ :=  pDisplayFram^+1;
    //CameraImageProcess第三个参数，必须是16字节对齐的缓冲区
    CameraImageProcess(
                        m_hCamera1,
                        pFrameBuffer,
                        m_pRgbBufferAlligned1,
                        pFrameHead
                        );
    CameraImageOverlay(m_hCamera1, m_pRgbBufferAlligned1, pFrameHead);
    CameraDisplayRGB24(m_hCamera1, m_pRgbBufferAlligned1, pFrameHead);

    if (m_iLastFrameWidth <> pFrameHead.iWidth)
        or (m_iLastFrameHeight <> pFrameHead.iHeight) then
            Form1.Displayer.Invalidate();

    m_iLastFrameWidth := pFrameHead.iWidth;
    m_iLastFrameHeight := pFrameHead.iHeight;


end;


//相机2图像捕获的回调函数，例程中使用该方式来获取图像，您也可用采用多线程或者定时器，然后
//不断调用CameraGetImageBuffer来获取图像。
procedure CameraGrabCallBack2(
hCamera:CameraHandle;
pFrameBuffer:PByte;
pFrameHead:PtSdkFrameHead;
pContext:Pointer
); stdcall;
var
pDisplayFram:PInteger;
begin

    //CameraImageProcess第三个参数，必须是16字节对齐的缓冲区
    CameraImageProcess(
                        m_hCamera2,
                        pFrameBuffer,
                        m_pRgbBufferAlligned2,
                        pFrameHead
                        );
    CameraImageOverlay(m_hCamera2, m_pRgbBufferAlligned2, pFrameHead);
    CameraDisplayRGB24(m_hCamera2, m_pRgbBufferAlligned2, pFrameHead);

    if (m_iLastFrameWidth2 <> pFrameHead.iWidth)
        or (m_iLastFrameHeight2 <> pFrameHead.iHeight) then
            Form1.Displayer2.Invalidate();

    m_iLastFrameWidth2 := pFrameHead.iWidth;
    m_iLastFrameHeight2 := pFrameHead.iHeight;


end;

procedure CameraSettingPageCallback(
hCamera:CameraHandle;
MSG:Cardinal;
uParam:Cardinal;
pContext:Pointer
); stdcall;
begin



end;


procedure TForm1.FormCreate(Sender: TObject);
var
iStatus:Integer;
begin
   //----------------------初始化相机 1 --------------------------------
   iStatus := CameraInit(@CameraList[1],-1,-1,@m_hCamera1);
   if iStatus <>  CAMERA_STATUS_SUCCESS then
        begin
        ShowMessage('相机初始化失败！程序即将退出。');
        halt;
        end;
   //获得相机特性，根据最大分辨率来申请一块内存，用做图像处理的中间缓冲区,多分配1024个，用来进行地址对齐
   iStatus := CameraGetCapability(m_hCamera1,@sCameraCapability1); //check iStatus if need
   if iStatus <>  CAMERA_STATUS_SUCCESS then
        halt;
   //因为Delphi6 中 GetMemory返回的地址不一定是16字节对齐的，用下面的方式，将其对齐
   m_pRgbBufferAlligned1 :=  CameraAlignMalloc(sCameraCapability1.sResolutionRange.iWidthMax*
                             sCameraCapability1.sResolutionRange.iHeightMax*4,16);

   //创建设备参数配置窗口
   iStatus := CameraCreateSettingPage(
                                m_hCamera1,
                                Form1.Handle,
				@CameraList[1].acFriendlyName,
                                @CameraSettingPageCallback,
                                Pointer(0),
                                0);
                                
   if iStatus <>  CAMERA_STATUS_SUCCESS then
        halt;

   //设置图像捕获的回调函数
   iStatus := CameraSetCallbackFunction(
                                m_hCamera1,
                                @CameraGrabCallBack,
                                Pointer(@m_iDisplayCount),
                                Pointer(0)
                                );

   if iStatus <>  CAMERA_STATUS_SUCCESS then
        begin
        ShowMessage('设置图像抓取回调函数失败！程序即将退出。');
        CameraUnInit(m_hCamera1);
        m_hCamera1 := 0;
        halt;
        end;
        
   //初始化显示模块，这里使用了SDK中封装的显示接口进行图像显示，您也可用自己用其他方式
   //实现图像的显示。但是如果要使用SDK中的显示相关函数，则必须进行以下的初始化。
   iStatus := CameraDisplayInit(m_hCamera1,Form1.Displayer.Handle);
   if iStatus <>  CAMERA_STATUS_SUCCESS then
        begin
        halt;
        end;
   //如果窗口可以动态改变大小，再次CameraSetDisplaySize设置为新的宽高值即可。
   iStatus := CameraSetDisplaySize(
                                   m_hCamera1,
                                   Form1.Displayer.Width,
                                   Form1.Displayer.Height
                                   );

   if iStatus <>  CAMERA_STATUS_SUCCESS then
        begin
        halt;
        end;


   //让相机1开始工作
   CameraPlay(m_hCamera1);
   m_bPause := FALSE;

    //---------------------初始化相机 2 -------------------------
   iStatus := CameraInit(@CameraList[2],-1,-1,@m_hCamera2);
   if iStatus <>  CAMERA_STATUS_SUCCESS then
        begin
        ShowMessage('相机2初始化失败！程序即将退出。');
        halt;
        end;
   //获得相机特性，根据最大分辨率来申请一块内存，用做图像处理的中间缓冲区,多分配1024个，用来进行地址对齐
   iStatus := CameraGetCapability(m_hCamera2,@sCameraCapability2); //check iStatus if need
   if iStatus <>  CAMERA_STATUS_SUCCESS then
        halt;

   //因为Delphi6 中 GetMemory返回的地址不一定是16字节对齐的，用下面的方式，将其对齐
   m_pRgbBufferAlligned2 :=  CameraAlignMalloc(sCameraCapability2.sResolutionRange.iWidthMax*
                             sCameraCapability2.sResolutionRange.iHeightMax*4,16);

   //创建设备参数配置窗口
   iStatus := CameraCreateSettingPage(
                                m_hCamera2,
                                Form1.Handle,
				@CameraList[2].acFriendlyName,
                                Pointer(0),
                                Pointer(0),
                                0);
                                
   if iStatus <>  CAMERA_STATUS_SUCCESS then
        halt;

   //设置图像捕获的回调函数
   iStatus := CameraSetCallbackFunction(
                                m_hCamera2,
                                @CameraGrabCallBack2,
                                Pointer(@m_iDisplayCount),
                                Pointer(0)
                                );

   if iStatus <>  CAMERA_STATUS_SUCCESS then
        begin
        ShowMessage('设置图像抓取回调函数失败！程序即将退出。');
        CameraUnInit(m_hCamera2);
        m_hCamera1 := 0;
        halt;
        end;
        
   //初始化显示模块，这里使用了SDK中封装的显示接口进行图像显示，您也可用自己用其他方式
   //实现图像的显示。但是如果要使用SDK中的显示相关函数，则必须进行以下的初始化。
   iStatus := CameraDisplayInit(m_hCamera2,Form1.Displayer2.Handle);
   if iStatus <>  CAMERA_STATUS_SUCCESS then
        begin
        halt;
        end;
   //如果窗口可以动态改变大小，再次CameraSetDisplaySize设置为新的宽高值即可。
   iStatus := CameraSetDisplaySize(
                                   m_hCamera2,
                                   Form1.Displayer2.Width,
                                   Form1.Displayer2.Height
                                   );

   if iStatus <>  CAMERA_STATUS_SUCCESS then
        begin
        halt;
        end;


   //让相机2开始工作
   CameraPlay(m_hCamera2);
   m_bPause2 := FALSE;
end;


procedure TForm1.BtnSettingsClick(Sender: TObject);
begin
     CameraShowSettingPage(m_hCamera1,TRUE);
end;

procedure TForm1.InfoUpdateTimerTimer(Sender: TObject);
begin

       StatusBar.Caption :=  Format(
       '| Resolution:%d X %d | Display frames:%d |',
       [m_iLastFrameWidth,
       m_iLastFrameHeight,
       m_iDisplayCount]
       );
end;

procedure TForm1.Camera2SettingsClick(Sender: TObject);
begin
      CameraShowSettingPage(m_hCamera2,TRUE);
end;

procedure TForm1.BtnPlay2Click(Sender: TObject);
begin
     if m_bPause2 = TRUE then
        begin
        m_bPause2 := FALSE;
        BtnPlay2.Caption := 'Pause';
        CameraPlay(m_hCamera2);
        end
     else
        begin
        m_bPause2 := TRUE;
        BtnPlay2.Caption := 'Play';
        CameraPause(m_hCamera2);//暂停后，相机停止输出图像，预览和抓拍都无效。
        end;
end;

procedure TForm1.BtnPlayClick(Sender: TObject);
begin
  if m_bPause = TRUE then
        begin
        m_bPause := FALSE;
        BtnPlay.Caption := 'Pause';
        CameraPlay(m_hCamera1);
        end
     else
        begin
        m_bPause := TRUE;
        BtnPlay.Caption := 'Play';
        CameraPause(m_hCamera1);//暂停后，相机停止输出图像，预览和抓拍都无效。
        end;
end;

initialization
  //单元初始化时判断有无相机，并进行初始化
   m_hCamera1     := 0;
   m_pRgbBufferAlligned1  := nil;

   m_hCamera2     := 0;
   m_pRgbBufferAlligned2  := nil;

   iCameraNumber := high(CameraList);      //iCameraNumber必须先填充好最大扫描的相机个数。不能为0；


   //扫描相机
   iStatus := CameraEnumerateDevice(@CameraList,@iCameraNumber);
   if (iStatus <> CAMERA_STATUS_SUCCESS) or (iCameraNumber < 2) then
        begin
        ShowMessage('This demo need two cameras!');
        halt;
        end;


                                
finalization
   //单元退出时关闭相机并释放资源

   //如果 m_hCamera1不为0，则表示已经初始化了相机，则进行反初始化。
   if m_hCamera1 <> 0 then
   CameraUnInit(m_hCamera1);

   if m_pRgbBufferAlligned1 <> nil then
   CameraAlignFree(m_pRgbBufferAlligned1);

    if m_hCamera2 <> 0 then
   CameraUnInit(m_hCamera2);

   if m_pRgbBufferAlligned2 <> nil then
   CameraAlignFree(m_pRgbBufferAlligned2);
   
end.
