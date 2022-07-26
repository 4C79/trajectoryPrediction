object Form1: TForm1
  Left = 1042
  Top = 311
  Width = 749
  Height = 656
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
    Top = 592
    Width = 50
    Height = 17
    Caption = 'StatusBar'
    TabOrder = 3
  end
  object Displayer: TPanel
    Left = 0
    Top = 48
    Width = 729
    Height = 537
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
    Left = 680
    Top = 16
  end
end
