object Form1: TForm1
  Left = 1004
  Top = 196
  Width = 707
  Height = 675
  Caption = 'Form1'
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'MS Sans Serif'
  Font.Style = []
  OldCreateOrder = False
  OnActivate = FormActivate
  OnClose = FormClose
  PixelsPerInch = 96
  TextHeight = 13
  object Label1: TLabel
    Left = 8
    Top = 600
    Width = 32
    Height = 13
    Caption = 'Label1'
  end
  object Panel1: TPanel
    Left = 8
    Top = 48
    Width = 665
    Height = 537
    Caption = 'Panel1'
    TabOrder = 0
  end
  object Button_pause: TButton
    Left = 8
    Top = 8
    Width = 105
    Height = 33
    Caption = 'Pause'
    TabOrder = 1
    OnClick = Button_pauseClick
  end
  object Button_Start: TButton
    Left = 120
    Top = 8
    Width = 105
    Height = 33
    Caption = 'Play'
    TabOrder = 2
    OnClick = Button_StartClick
  end
  object Button_snapshot: TButton
    Left = 232
    Top = 8
    Width = 105
    Height = 33
    Caption = 'Snapshot'
    TabOrder = 3
    OnClick = Button_snapshotClick
  end
  object Button_settings: TButton
    Left = 344
    Top = 8
    Width = 105
    Height = 33
    Caption = 'Settings'
    TabOrder = 4
    OnClick = Button_settingsClick
  end
  object Timer1: TTimer
    OnTimer = Timer1Timer
    Left = 464
    Top = 8
  end
end
