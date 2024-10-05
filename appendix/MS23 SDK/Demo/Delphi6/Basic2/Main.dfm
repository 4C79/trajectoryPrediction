object Form1: TForm1
  Left = 883
  Top = 424
  Width = 643
  Height = 598
  Caption = 'Camera delphi6 demo [Basic]'
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'MS Sans Serif'
  Font.Style = []
  OldCreateOrder = False
  OnCreate = FormCreate
  PixelsPerInch = 96
  TextHeight = 13
  object BtnPlay: TButton
    Left = 0
    Top = 8
    Width = 105
    Height = 33
    Caption = 'Pause'
    TabOrder = 0
    OnClick = BtnPlayClick
  end
  object BtnSettings: TButton
    Left = 120
    Top = 8
    Width = 105
    Height = 33
    Caption = 'Settings'
    TabOrder = 1
    OnClick = BtnSettingsClick
  end
  object BtnSnapshot: TButton
    Left = 240
    Top = 8
    Width = 105
    Height = 33
    Caption = 'Snapshot'
    TabOrder = 2
    OnClick = BtnSnapshotClick
  end
  object StatusBar: TStaticText
    Left = 0
    Top = 536
    Width = 50
    Height = 17
    Caption = 'StatusBar'
    TabOrder = 3
  end
  object Displayer: TPanel
    Left = 0
    Top = 48
    Width = 625
    Height = 481
    Caption = 'Displayer'
    Color = clBlack
    Ctl3D = False
    Enabled = False
    FullRepaint = False
    ParentCtl3D = False
    TabOrder = 4
  end
  object InfoUpdateTimer: TTimer
    OnTimer = InfoUpdateTimerTimer
    Left = 560
    Top = 8
  end
  object GrabTimer: TTimer
    Interval = 20
    Left = 528
    Top = 8
  end
end
