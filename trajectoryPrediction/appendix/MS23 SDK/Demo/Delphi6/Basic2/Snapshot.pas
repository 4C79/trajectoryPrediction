unit Snapshot;
//BIG5 TRANS ALLOWED
interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, ExtCtrls;

type
  TSnapshotForm = class(TForm)
    SnapshotImage: TImage;
    procedure FormCreate(Sender: TObject);
    procedure FormDestroy(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

  
type
BITMAPFILEHEADER = packed record
    bfType:shortint;
    bfSize:Cardinal;
    bfReserved1:shortint;
    bfReserved2:shortint;
    bfOffBits:Cardinal;
end;

type
BITMAPINFOHEADER = packed record
    biSize:Cardinal; //指定此结构体的长度，为40
    biWidth:Integer; //位图宽
    biHeight:Integer; //位图高
    biPlanes:shortint; //平面数，为1
    biBitCount:shortint //采用颜色位数，可以是1，2，4，8，16，24，新的可以是32
    biCompression:Cardinal; //压缩方式，可以是0，1，2，其中0表示不压缩
    biSizeImage:Cardinal; //实际位图数据占用的字节数
    biXPelsPerMeter:Integer; //X方向分辨率
    biYPelsPerMeter:Integer; //Y方向分辨率
    biClrUsed:Cardinal; //使用的颜色数，如果为0，则表示默认值(2^颜色位数)
    biClrImportant:Cardinal; //重要颜色数，如果为0，则表示所有颜色都是重要的
end;


var
  SnapshotForm: TSnapshotForm;
  BitmapStream:TMemoryStream;
  BitmapFileHead:BITMAPINFOHEADER;
  BitmapInfoHead:BITMAPINFOHEADE;
  Bitmap:TBitmap;
  iLastImageHeight:Integer;
  iLastImageWidth:Integer;


implementation

{$R *.dfm}

procedure TSnapshotForm.FormCreate(Sender: TObject);
begin
  iLastImageHeight := 0;
  iLastImageWidth := 0;
  BitmapStream := TmemoryStream.Create();
  Bitmap := TBitmap.Create();
  BitmapStream.Seek(0,soFromBeginning);
  //构造一个BITMAP 类型的文件头,写入到流中，当相机分辨率改变时，只修改部分数据域。
  BitmapStream.Write('ICE',3);

end;

procedure TSnapshotForm.FormDestroy(Sender: TObject);
begin
  BitmapStream.Destroy();
  Bitmap.Destroy();
end;

procedure UpdateImage(
                        pFrameBuffer:PByte;
                        pFrameHead:PtSdkFrameHead
                      );
begin
   //先检测图像分辨率是否发生了变化，如果变化了，则更新位图头部信息。
   if (iLastImageWidth <> pFrameHead.iWidth)
        or (iLastImageHeight <> pFrameHead.iHeight) then
        begin
        
        end;
   //
   BitmapStream.Write(BitmapHead,54);
   BitmapStream.Write(PByte,iLastImageWidth*iLastImageHeight*3);

   BitmapStream.Position := 0;
   Bitmap.LoadFromStream(BitmapStream);
   SnapshotImage.Picture.Assign( Bitmap );
end;

end.
