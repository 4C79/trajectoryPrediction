object Form1: TForm1
  Left = 192
  Top = 130
  Width = 712
  Height = 636
  Caption = 'Form1'
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'MS Sans Serif'
  Font.Style = []
  OldCreateOrder = False
  OnClose = FormClose
  OnCreate = FormCreate
  PixelsPerInch = 96
  TextHeight = 13
  object Label1: TLabel
    Left = 32
    Top = 568
    Width = 633
    Height = 17
    Caption = 'Label1'
  end
  object ButtonSettings: TButton
    Left = 32
    Top = 32
    Width = 75
    Height = 25
    Caption = 'Settings'
    TabOrder = 0
    OnClick = ButtonSettingsClick
  end
  object ButtonPlay: TButton
    Left = 128
    Top = 32
    Width = 75
    Height = 25
    Caption = 'Play'
    TabOrder = 1
    OnClick = ButtonPlayClick
  end
  object ButtonStop: TButton
    Left = 224
    Top = 32
    Width = 75
    Height = 25
    Caption = 'Stop'
    TabOrder = 2
    OnClick = ButtonStopClick
  end
  object ButtonSnap: TButton
    Left = 320
    Top = 32
    Width = 75
    Height = 25
    Caption = 'Snap'
    TabOrder = 3
    OnClick = ButtonSnapClick
  end
  object DispWnd: TPanel
    Left = 32
    Top = 80
    Width = 633
    Height = 473
    TabOrder = 4
  end
  object Timer1: TTimer
    OnTimer = Timer1Timer
    Left = 432
    Top = 32
  end
end
