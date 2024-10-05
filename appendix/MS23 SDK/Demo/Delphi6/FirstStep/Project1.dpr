program Project1;

uses
  Forms,
  Unit1 in 'Unit1.dpr' {Form1},
  CameraApi in '..\Units\CameraApi.pas',
  CameraDefine in '..\Units\CameraDefine.pas',
  CameraStatus in '..\Units\CameraStatus.pas';

{$R *.res}

begin
  Application.Initialize;
  Application.CreateForm(TForm1, Form1);
  Application.Run;
end.
