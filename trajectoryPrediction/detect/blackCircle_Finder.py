import cv2
import numpy as np


def circle_detectFrame(frame):
    # 灰度化
    # gray = cv2.cvtColor(image, cv2.COLOR_BGR2GRAY)
    # 输出图像大小，方便根据图像大小调节minRadius和maxRadius
    # print(image.shape)
    maxx = frame.shape[0]
    maxy = frame.shape[1]
    # 进行中值滤波
    img = cv2.medianBlur(frame, 5)

    # 霍夫变换圆检测
    circles = cv2.HoughCircles(img, cv2.HOUGH_GRADIENT, 1, 50, param1=100, param2=30, minRadius=1, maxRadius=100)
    if circles is None:
        return (0, 0)
    for circle in circles[0]:
        # 圆的基本信息
        # print(circle[2])
        # 坐标行列－圆心坐标
        x = int(circle[0])
        y = int(circle[1])
        zh = img[y][x]
        # 半径
        r = int(circle[2])
        sum = 0
        for i in range(max(0, x + r), min(x - r, maxy - 1)):
            sum += img[y][i]
        for i in range(max(0, y - r), min(y + r, maxx - 1)):
            sum += img[i][x]
        avg = sum / (4 * r)
        if (avg > 40 or zh > 40):
            continue
        box = (x, y)
        return box


def circle_detectImage(image):

    # 灰度化
    gray = cv2.cvtColor(image, cv2.COLOR_BGR2GRAY)
    # 输出图像大小，方便根据图像大小调节minRadius和maxRadius
    # print(image.shape)
    maxx = gray.shape[0]
    maxy = gray.shape[1]
    # 进行中值滤波
    img = cv2.medianBlur(gray, 5)

    # 霍夫变换圆检测
    circles = cv2.HoughCircles(img, cv2.HOUGH_GRADIENT, 1, 50, param1=100, param2=30, minRadius=1, maxRadius=100)
    if circles is None:
        return 0
    for circle in circles[0]:
        # 圆的基本信息
        # print(circle[2])
        # 坐标行列－圆心坐标
        x = int(circle[0])
        y = int(circle[1])
        zh = img[y][x]
        # 半径
        r = int(circle[2])
        sum = 0
        for i in range(max(0, x + r), min(x - r, maxy - 1)):
            sum += img[y][i]
        for i in range(max(0, y - r), min(y + r, maxx - 1)):
            sum += img[i][x]
        avg = sum / (4 * r)
        if (avg > 40 or zh > 40):
            continue
        box = (x, y)
        return box


if __name__ == '__main__':
    img = cv2.imread("..\data\l\\0.jpg")
    tmp = circle_detectImage(img)
    print(tmp)
