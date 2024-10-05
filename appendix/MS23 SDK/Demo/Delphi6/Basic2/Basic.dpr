program Basic;

uses
  Forms,
  Main in 'Main.pas' {Form1},
  CameraDefine in '..\Units\CameraDefine.pas',
  CameraStatus in '..\Units\CameraStatus.pas',
  CameraApi in '..\Units\CameraApi.pas',
  TSnapshot in 'TSnapshot.pas' {SnapshotForm};

{$R *.res}

begin
  Application.Initialize;
  Application.CreateForm(TForm1, Form1);
  Application.CreateForm(TSnapshotForm, SnapshotForm);
  Application.Run;
end.
