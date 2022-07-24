# coding=utf-8
import cv2
import numpy as np
# from camera import mvsdk
import platform
from detect import orange_prediction
from detect import blackCircle_Finder
from detect import placement_prediction
import os


def main_loop(path):
    op = orange_prediction
    bd = blackCircle_Finder

    # 枚举相机
    DevList = mvsdk.CameraEnumerateDevice()
    nDev = len(DevList)
    if nDev < 1:
        print("No camera was found!")
        return

    for i, DevInfo in enumerate(DevList):
        print("{}: {} {}".format(i, DevInfo.GetFriendlyName(), DevInfo.GetPortType()))
    # i = 0 if nDev == 1 else int(input("Select camera: "))
    # DevInfo = DevList[i]
    # print(DevInfo)

    # 打开两个相机
    DevInfo0 = DevList[0]
    DevInfo1 = DevList[1]

    try:
        hCamera0 = mvsdk.CameraInit(DevInfo0, -1, -1)
        hCamera1 = mvsdk.CameraInit(DevInfo1, -1, -1)
    except mvsdk.CameraException as e:
        print("CameraInit Failed({}): {}".format(e.error_code, e.message))
        return

    # 获取相机特性描述
    cap0 = mvsdk.CameraGetCapability(hCamera0)
    cap1 = mvsdk.CameraGetCapability(hCamera1)

    # 判断是黑白相机还是彩色相机
    monoCamera0 = (cap0.sIspCapacity.bMonoSensor != 0)
    monoCamera1 = (cap1.sIspCapacity.bMonoSensor != 0)

    # 黑白相机让ISP直接输出MONO数据，而不是扩展成R=G=B的24位灰度
    if monoCamera0:
        mvsdk.CameraSetIspOutFormat(hCamera0, mvsdk.CAMERA_MEDIA_TYPE_MONO8)
    else:
        mvsdk.CameraSetIspOutFormat(hCamera0, mvsdk.CAMERA_MEDIA_TYPE_BGR8)
    if monoCamera1:
        mvsdk.CameraSetIspOutFormat(hCamera1, mvsdk.CAMERA_MEDIA_TYPE_MONO8)
    else:
        mvsdk.CameraSetIspOutFormat(hCamera1, mvsdk.CAMERA_MEDIA_TYPE_BGR8)

    # 相机模式切换成连续采集
    mvsdk.CameraSetTriggerMode(hCamera0, 0)
    mvsdk.CameraSetTriggerMode(hCamera1, 0)

    # # 设置相机帧率
    # mvsdk.CameraSetFrameSpeed(hCamera0, 300)
    # mvsdk.CameraSetFrameSpeed(hCamera1, 300)

    # 手动曝光，曝光时间20ms
    mvsdk.CameraSetAeState(hCamera0, 0)
    mvsdk.CameraSetExposureTime(hCamera0, 3 * 1000)
    mvsdk.CameraSetAeState(hCamera1, 0)
    mvsdk.CameraSetExposureTime(hCamera1, 3 * 1000)

    # 让SDK内部取图线程开始工作
    mvsdk.CameraPlay(hCamera0)
    mvsdk.CameraPlay(hCamera1)

    # 计算RGB buffer所需的大小，这里直接按照相机的最大分辨率来分配
    FrameBufferSize0 = cap0.sResolutionRange.iWidthMax * cap0.sResolutionRange.iHeightMax * (1 if monoCamera0 else 3)
    FrameBufferSize1 = cap1.sResolutionRange.iWidthMax * cap1.sResolutionRange.iHeightMax * (1 if monoCamera1 else 3)

    # 分配RGB buffer，用来存放ISP输出的图像
    # 备注：从相机传输到PC端的是RAW数据，在PC端通过软件ISP转为RGB数据（如果是黑白相机就不需要转换格式，但是ISP还有其它处理，所以也需要分配这个buffer）
    pFrameBuffer0 = mvsdk.CameraAlignMalloc(FrameBufferSize0, 16)
    pFrameBuffer1 = mvsdk.CameraAlignMalloc(FrameBufferSize1, 16)

    time0 = 0  # 图片保存的文件名初始化

    while (1):  # 1ms触发一次，但是好像相机不够支持？
        # 从相机取一帧图片
        try:
            pRawData0, FrameHead0 = mvsdk.CameraGetImageBuffer(hCamera0, 1)
            mvsdk.CameraImageProcess(hCamera0, pRawData0, pFrameBuffer0, FrameHead0)
            mvsdk.CameraReleaseImageBuffer(hCamera0, pRawData0)

            pRawData1, FrameHead1 = mvsdk.CameraGetImageBuffer(hCamera1, 1)
            mvsdk.CameraImageProcess(hCamera1, pRawData1, pFrameBuffer1, FrameHead1)
            mvsdk.CameraReleaseImageBuffer(hCamera1, pRawData1)

            # windows下取到的图像数据是上下颠倒的，以BMP格式存放。转换成opencv则需要上下翻转成正的
            # linux下直接输出正的，不需要上下翻转
            if platform.system() == "Windows":
                mvsdk.CameraFlipFrameBuffer(pFrameBuffer0, FrameHead0, 1)
                mvsdk.CameraFlipFrameBuffer(pFrameBuffer1, FrameHead1, 1)

            # 此时图片已经存储在pFrameBuffer中，对于彩色相机pFrameBuffer=RGB数据，黑白相机pFrameBuffer=8位灰度数据
            # 把pFrameBuffer转换成opencv的图像格式以进行后续算法处理
            frame_data0 = (mvsdk.c_ubyte * FrameHead0.uBytes).from_address(pFrameBuffer0)
            frame0 = np.frombuffer(frame_data0, dtype=np.uint8)
            frame0 = frame0.reshape((FrameHead0.iHeight, FrameHead0.iWidth,
                                     1 if FrameHead0.uiMediaType == mvsdk.CAMERA_MEDIA_TYPE_MONO8 else 3))

            frame_data1 = (mvsdk.c_ubyte * FrameHead1.uBytes).from_address(pFrameBuffer1)
            frame1 = np.frombuffer(frame_data1, dtype=np.uint8)
            frame1 = frame1.reshape((FrameHead1.iHeight, FrameHead1.iWidth,
                                     1 if FrameHead1.uiMediaType == mvsdk.CAMERA_MEDIA_TYPE_MONO8 else 3))

            frame0 = cv2.resize(frame0, (640, 480), interpolation=cv2.INTER_LINEAR)
            frame1 = cv2.resize(frame1, (640, 480), interpolation=cv2.INTER_LINEAR)
            # cv2.imshow("Press q to end", frame0)
            # cv2.imshow("Press q to end", frame1)

            path0 = path + "/l/" + str(time0) + ".jpg"
            path1 = path + "/r/" + str(time0) + ".jpg"

            cv2.imwrite(path0, frame0)
            cv2.imwrite(path1, frame1)

            time0 += 1

        except mvsdk.CameraException as e:
            if e.error_code != mvsdk.CAMERA_STATUS_TIME_OUT:
                print("CameraGetImageBuffer failed({}): {}".format(e.error_code, e.message))

    # 关闭相机
    mvsdk.CameraUnInit(hCamera0)
    mvsdk.CameraUnInit(hCamera1)

    # 释放帧缓存
    mvsdk.CameraAlignFree(pFrameBuffer0)
    mvsdk.CameraAlignFree(pFrameBuffer1)

def fromVideoPlay():
    path = "..\\\\data"
    pp = placement_prediction
    print("落点的坐标为：")
    print(pp.getAns(path + '\\\\l', path + '\\\\r'))


# 需要保证帧率稳定且电脑处理速度快
def fromCameraPlay():
    try:
        path = "..\\\\data"
        pp = placement_prediction
        main_loop(path)
    finally:
        cv2.destroyAllWindows()

def kalmanFilter():
    kf_l = kalmanFilter()
    kf_r = kalmanFilter()


if __name__ == '__main__':
    fromVideoPlay()