unit TSnapshot;
//BIG5 TRANS ALLOWED
interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, ExtCtrls, CameraApi, CameraDefine, CameraStatus;

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
PBITMAPFILEHEADER = ^BITMAPFILEHEADER;
BITMAPFILEHEADER = packed record
    bfType:array[1..2] of char;
    bfSize:Cardinal;
    bfReserved1:WORD;
    bfReserved2:WORD;
    bfOffBits:Cardinal;
end;

type
PBITMAPINFOHEADER = ^BITMAPINFOHEADER;
BITMAPINFOHEADER = packed record
    biSize:Cardinal; //指定此结构体的长度，为40
    biWidth:Integer; //位图宽
    biHeight:Integer; //位图高
    biPlanes:WORD; //平面数，为1
    biBitCount:WORD; //采用颜色位数，可以是1，2，4，8，16，24，新的可以是32
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
  BitmapFileHead:BITMAPFILEHEADER;
  BitmapInfoHead:BITMAPINFOHEADER;
  Bitmap:TBitmap;
  iLastImageHeight:Integer;
  iLastImageWidth:Integer;
  BitmapHead:array[1..54] of Byte;


procedure UpdateImage(
                        pFrameBuffer:PByte;
                        pFrameHead:PtSdkFrameHead
                      );

implementation

{$R *.dfm}

procedure TSnapshotForm.FormCreate(Sender: TObject);
var
pBitmapFileHead:PBITMAPFILEHEADER;
pBitmapInfoHead:PBITMAPINFOHEADER;
begin
  iLastImageHeight := 0;
  iLastImageWidth := 0;
  BitmapStream := TmemoryStream.Create();
  BitmapStream.SetSize(1280*1024*3+54);
  Bitmap := TBitmap.Create();
  BitmapStream.Seek(0,soFromBeginning);
  //构造一个BITMAP 类型的文件头,写入到流中，当相机分辨率改变时，只修改部分数据域。
  pBitmapFileHead := PBITMAPFILEHEADER(@BitmapHead[1]);
  pBitmapInfoHead := PBITMAPINFOHEADER(@BitmapHead[15]);
  pBitmapFileHead.bfType[1]         := 'B';
  pBitmapFileHead.bfType[2]         := 'M';
  pBitmapFileHead.bfSize            := 54;
  pBitmapFileHead.bfReserved1       := 0;
  pBitmapFileHead.bfReserved2       := 0;
  pBitmapFileHead.bfOffBits         := 54;
  pBitmapInfoHead.biSize            := 40;
  pBitmapInfoHead.biWidth           := 0;//图像的宽高，需要动态修改
  pBitmapInfoHead.biHeight          := 0;
  pBitmapInfoHead.biPlanes          := 1;
  pBitmapInfoHead.biBitCount        := 24;
  pBitmapInfoHead.biCompression     := 0;
  pBitmapInfoHead.biSizeImage       := 0;
  pBitmapInfoHead.biXPelsPerMeter   := 0;
  pBitmapInfoHead.biYPelsPerMeter   := 0;
  pBitmapInfoHead.biClrUsed         := 0;
  pBitmapInfoHead.biClrImportant    := 0;
  

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
var
pBitmapFileHead:PBITMAPFILEHEADER;
pBitmapInfoHead:PBITMAPINFOHEADER;
begin
        //pFrameHead.iWidth := 64;
        //pFrameHead.iHeight := 64;
   //先检测图像分辨率是否发生了变化，如果变化了，则更新位图头部信息。
   if (iLastImageWidth <> pFrameHead.iWidth)
        or (iLastImageHeight <> pFrameHead.iHeight) then
        begin
        pBitmapFileHead := PBITMAPFILEHEADER(@BitmapHead[1]);
        pBitmapInfoHead := PBITMAPINFOHEADER(@BitmapHead[15]);
        pBitmapFileHead.bfSize := 54 +  (pFrameHead.iWidth*pFrameHead.iHeight*3);
        pBitmapInfoHead.biWidth := pFrameHead.iWidth;
        pBitmapInfoHead.biHeight := pFrameHead.iHeight;
        iLastImageWidth := pFrameHead.iWidth;
        iLastImageHeight :=  pFrameHead.iHeight;
        end;
   
   BitmapStream.Position := 0;
   BitmapStream.Write(BitmapHead,54);
   BitmapStream.Write(pFrameBuffer^,iLastImageWidth*iLastImageHeight*3);
   //BitmapStream.SaveToFile('c:\test.Bmp');  //如果需要保存成文件，就反注释这一行

   BitmapStream.Position := 0;
   Bitmap.LoadFromStream(BitmapStream);
   SnapshotForm.SnapshotImage.Picture.Assign( Bitmap );
   SnapshotForm.Show();
end;

end.
