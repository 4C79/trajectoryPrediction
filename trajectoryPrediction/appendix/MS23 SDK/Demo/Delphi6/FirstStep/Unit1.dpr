unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, ExtCtrls, StdCtrls, CameraApi, CameraDefine, CameraStatus;

type
  TForm1 = class(TForm)
    ButtonSettings: TButton;
    ButtonPlay: TButton;
    ButtonStop: TButton;
    ButtonSnap: TButton;
    DispWnd: TPanel;
    Label1: TLabel;
    Timer1: TTimer;
    procedure FormCreate(Sender: TObject);
    procedure FormClose(Sender: TObject; var Action: TCloseAction);
    procedure ButtonSettingsClick(Sender: TObject);
    procedure ButtonPlayClick(Sender: TObject);
    procedure ButtonStopClick(Sender: TObject);
    procedure ButtonSnapClick(Sender: TObject);
    procedure Timer1Timer(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  Form1: TForm1;
  m_Grabber:Pointer;
  m_hCamera:CameraHandle;
  m_DevInfo:tSdkCameraDevInfo;

implementation

{$R *.dfm}

procedure TForm1.FormCreate(Sender: TObject);
var
  Status:Integer;
begin
  m_Grabber := NIL;
  Status := CameraGrabber_CreateFromDevicePage(@m_Grabber);
  if Status = CAMERA_STATUS_SUCCESS then
  begin
    CameraGrabber_GetCameraHandle(m_Grabber, @m_hCamera);
    CameraGrabber_GetCameraDevInfo(m_Grabber, @m_DevInfo);

    CameraCreateSettingPage(m_hCamera, Form1.Handle, @m_DevInfo.acFriendlyName, CAMERA_PAGE_MSG_PROC(0), NIL, 0);

    CameraGrabber_SetHWnd(m_Grabber, DispWnd.Handle);
    CameraGrabber_StartLive(m_Grabber);
  end;
end;

procedure TForm1.FormClose(Sender: TObject; var Action: TCloseAction);
begin
  CameraGrabber_Destroy(m_Grabber);
end;

procedure TForm1.ButtonSettingsClick(Sender: TObject);
begin
  if m_Grabber <> Pointer(0) then
    CameraShowSettingPage(m_hCamera, TRUE);
end;

procedure TForm1.ButtonPlayClick(Sender: TObject);
begin
  if m_Grabber <> Pointer(0) then
    CameraGrabber_StartLive(m_Grabber);
end;

procedure TForm1.ButtonStopClick(Sender: TObject);
begin
  if m_Grabber <> Pointer(0) then
    CameraGrabber_StopLive(m_Grabber);
end;

procedure TForm1.ButtonSnapClick(Sender: TObject);
var
Image:Pointer;
Status:Integer;
sPath:array[0..255] of char;
sFileName:string;
sCurPath:string;
begin
  if m_Grabber <> Pointer(0) then
  begin
    Status := CameraGrabber_SaveImage(m_Grabber, @Image, 2000);
    if Status = CAMERA_STATUS_SUCCESS then
    begin
      sCurPath := ExtractFilePath(Application.ExeName);
      sFileName := Format('%s%d.BMP',[sCurPath,GetTickCount]);

      strPCopy(sPath,sFileName);
      Status := CameraImage_SaveAsBmp(Image, sPath);
      if Status = CAMERA_STATUS_SUCCESS then
        ShowMessage(sFileName)
      else
        ShowMessage('Save File Failed');

      CameraImage_Destroy(Image)
    end
    else
    begin
      ShowMessage('Snap failed!')
    end;
  end;
end;

procedure TForm1.Timer1Timer(Sender: TObject);
var
stat:tSdkGrabberStat;
begin
  if m_Grabber <> Pointer(0) then
  begin
    CameraGrabber_GetStat(m_Grabber, @stat);
    Label1.Caption := Format(
       '| Resolution:%d X %d | Display FPS:%f | Cap FPS:%f |',
       [stat.Width,
       stat.Height,
       stat.DispFPS,
       stat.CapFPS]
       );
  end
end;

end.
