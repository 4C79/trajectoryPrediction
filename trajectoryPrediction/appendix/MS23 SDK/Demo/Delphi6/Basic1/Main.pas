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
  Dialogs, StdCtrls, ExtCtrls, CameraApi, CameraDefine, CameraStatus;

type
  TForm1 = class(TForm)
    BtnPlay: TButton;
    BtnSettings: TButton;
    BtnSnapshot: TButton;
    StatusBar: TStaticText;
    InfoUpdateTimer: TTimer;
    Displayer: TPanel;
    procedure FormCreate(Sender: TObject);
    procedure BtnSettingsClick(Sender: TObject);
    procedure BtnPlayClick(Sender: TObject);
    procedure InfoUpdateTimerTimer(Sender: TObject);
    procedure BtnSnapshotClick(Sender: TObject);

  private
    { Private declarations }
  public
    { Public declarations }

  end;

var
  Form1: TForm1;
  m_hCamera:Integer;  //相机句柄
  iStatus:Integer;    //函数返回值
  CameraList:array[1..8] of tSdkCameraDevInfo; //相机列表，这里数组大小为8，表示最多只扫描8个设备
                                               //如果需要扫描更多的设备，请将该数组大小改为您想要的数。
  iCameraNumber:Integer;        //相机的个数
  m_iDisplayCount:Integer;      //已经显示的帧数
  m_pRgbBufferAlligned:PByte;   //经过16字节对齐修正后的地址
  sCameraCapability:tSdkCameraCapbility; //相机特性描述结构体
  m_bPause:LongBool;                     //用来指示是否暂停采集
  m_iLastFrameWidth:Integer;             //保存上一次捕获到的图像的宽度
  m_iLastFrameHeight:Integer;            //保存上一次捕获到的图像的高度
  m_iSnapshotTimes:Integer;              //记录抓怕的次数
implementation

{$R *.dfm}

//图像捕获的回调函数，例程中使用该方式来获取图像，您也可用采用多线程或者定时器，然后
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
                        m_hCamera,
                        pFrameBuffer,
                        m_pRgbBufferAlligned,
                        pFrameHead
                        );
    CameraImageOverlay(m_hCamera, m_pRgbBufferAlligned, pFrameHead);
    CameraDisplayRGB24(m_hCamera, m_pRgbBufferAlligned, pFrameHead);

    if (m_iLastFrameWidth <> pFrameHead.iWidth)
        or (m_iLastFrameHeight <> pFrameHead.iHeight) then
            Form1.Displayer.Invalidate();

    m_iLastFrameWidth := pFrameHead.iWidth;
    m_iLastFrameHeight := pFrameHead.iHeight;


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
sImageSize:tSdkImageResolution;
begin
       //初始化相机
   iStatus := CameraInit(@CameraList[1],-1,-1,@m_hCamera);
   if iStatus <>  CAMERA_STATUS_SUCCESS then
        begin
        ShowMessage('相机初始化失败！程序即将退出。');
        halt;
        end;
   //获得相机特性，根据最大分辨率来申请一块内存，用做图像处理的中间缓冲区,多分配1024个，用来进行地址对齐
   iStatus := CameraGetCapability(m_hCamera,@sCameraCapability); //check iStatus if need
   if iStatus <>  CAMERA_STATUS_SUCCESS then
        halt;

   m_pRgbBufferAlligned := CameraAlignMalloc(sCameraCapability.sResolutionRange.iWidthMax * sCameraCapability.sResolutionRange.iHeightMax * 4,16);
   //因为Delphi6 中 GetMemory返回的地址不是16字节对齐的，因此用SDK提供的CameraAlignMalloc申请内存

   //创建设备参数配置窗口
   iStatus := CameraCreateSettingPage(
                                m_hCamera,
                                Form1.Handle,
				@CameraList[1].acFriendlyName,
                                @CameraSettingPageCallback,
                                Pointer(0),
                                0);
                                
   if iStatus <>  CAMERA_STATUS_SUCCESS then
        halt;

   //设置图像捕获的回调函数
   iStatus := CameraSetCallbackFunction(
                                m_hCamera,
                                @CameraGrabCallBack,
                                Pointer(@m_iDisplayCount),
                                Pointer(0)
                                );

   if iStatus <>  CAMERA_STATUS_SUCCESS then
        begin
        ShowMessage('设置图像抓取回调函数失败！程序即将退出。');
        CameraUnInit(m_hCamera);
        m_hCamera := 0;
        halt;
        end;
        
   //初始化显示模块，这里使用了SDK中封装的显示接口进行图像显示，您也可用自己用其他方式
   //实现图像的显示。但是如果要使用SDK中的显示相关函数，则必须进行以下的初始化。
   iStatus := CameraDisplayInit(m_hCamera,Form1.Displayer.Handle);
   if iStatus <>  CAMERA_STATUS_SUCCESS then
        begin
        halt;
        end;
   //如果窗口可以动态改变大小，再次CameraSetDisplaySize设置为新的宽高值即可。
   iStatus := CameraSetDisplaySize(
                                   m_hCamera,
                                   Form1.Displayer.Width,
                                   Form1.Displayer.Height
                                   );

   if iStatus <>  CAMERA_STATUS_SUCCESS then
        begin
        halt;
        end;

     //设置抓拍的分辨率和预览相同
     // iIndex 为0XFF，其余设置为0 ，表示和预览相同的分辨率进行抓拍。
     // 如果需要独立设置抓拍的分辨率，将其余设置为想要的分辨率
     sImageSize.iIndex := $ff;
     sImageSize.iWidth := 0;
     sImageSize.iHeight:= 0;
     sImageSize.iHOffsetFOV := 0;
     sImageSize.iVOffsetFOV := 0;
     sImageSize.iWidthFOV := 0;
     sImageSize.iHeightFOV := 0;
     
     CameraSetResolutionForSnap(m_hCamera,@sImageSize);

   //让相机开始工作
   CameraPlay(m_hCamera);
   m_bPause := FALSE;

end;

procedure TForm1.BtnSettingsClick(Sender: TObject);
begin
     CameraShowSettingPage(m_hCamera,TRUE);
end;

procedure TForm1.BtnPlayClick(Sender: TObject);
begin
     if m_bPause = TRUE then
        begin
        m_bPause := FALSE;
        BtnPlay.Caption := 'Pause';
        CameraPlay(m_hCamera);
        end
     else
        begin
        m_bPause := TRUE;
        BtnPlay.Caption := 'Play';
        CameraPause(m_hCamera);//暂停后，相机停止输出图像，预览和抓拍都无效。
        end;
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

procedure TForm1.BtnSnapshotClick(Sender: TObject);
var
pRawBuffer:PByte;
pRgbBufferAlligned:PByte;
iStatus:Integer;
FrameInfo:tSdkFrameHead;
sPath:array[0..255] of char;
sFileName:string;
sCurPath:string;
begin
      //生成文件名

      sCurPath := ExtractFilePath(Application.ExeName);
      sFileName := Format('%sSnapshot%d.BMP',[sCurPath,m_iSnapshotTimes]);
      
      strPCopy(sPath,sFileName);

      //CameraSnapToBuffer抓拍一帧图像到内存中
	  // !!!!!!注意：CameraSnapToBuffer 会切换分辨率拍照，速度较慢。做实时处理，请用CameraGetImageBuffer函数取图或者回调函数。
      iStatus := CameraSnapToBuffer(m_hCamera,@FrameInfo,@pRawBuffer,1000);

      if  iStatus = CAMERA_STATUS_SUCCESS then
      begin
      pRgbBufferAlligned := CameraAlignMalloc(FrameInfo.iWidth*FrameInfo.iHeight*4,16);
      CameraImageProcess(m_hCamera,pRawBuffer,pRgbBufferAlligned,@FrameInfo);
      CameraReleaseImageBuffer(m_hCamera,pRawBuffer);

      //调用相机SDK接口CameraSaveImage进行图片保存，您也开可以将pRgbBufferAlligned进行显示或者使用
      //另外的接口进行保存图片。Basic2例程中演示了如何将pRgbBufferAlligned中的
      //图像显示到Timage控件上。
      iStatus := CameraSaveImage(
                      m_hCamera,
                      @sPath,
                      pRgbBufferAlligned,//如果要保存RAW数据，则不需要CameraImageProcess，这里缓冲填 pRawBuffer
                      @FrameInfo,
                      2,//FILE_JPG = 1 ;FILE_BMP = 2; FILE_RAW = 4;FILE_PNG = 8
                      100 //图像质量因子，仅当FILE_JPG类型时有效，100为最大，1最小
                      );
      CameraAlignFree(pRgbBufferAlligned);
      m_iSnapshotTimes := m_iSnapshotTimes + 1;
      if  iStatus = CAMERA_STATUS_SUCCESS then
        ShowMessage('Snapshot OK!File save in:'+sFileName)
      else
        ShowMessage('Failed to save image')
      end
      else
        ShowMessage('Snapshot failed!Do you set the camera in pause mode ?Resume it and try angin');
end;

initialization
  //单元初始化时判断有无相机，并进行初始化
   m_hCamera     := 0;
   m_pRgbBufferAlligned := nil;
   iCameraNumber := high(CameraList);     //iCameraNumber必须先填充好最大扫描的相机个数。不能为0；

   //扫描相机
   iStatus := CameraEnumerateDevice(@CameraList,@iCameraNumber);
   if iStatus <> CAMERA_STATUS_SUCCESS then
        begin
        ShowMessage('没有检测到相机！程序即将退出，请插上相机后再运行本例程。');
        halt;
        end;

finalization
   //单元退出时关闭相机并释放资源
         
   if m_hCamera <> 0 then
   CameraUnInit(m_hCamera);

   if m_pRgbBufferAlligned <> nil then
   CameraAlignFree(m_pRgbBufferAlligned);


end.
