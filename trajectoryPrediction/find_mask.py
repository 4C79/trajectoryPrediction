# 查找圆（精确度高）
import math
import cv2
import imutils
import numpy as np
from detect import EPNP
import random

pi = 3.14159265358979324


def is_circle(c):
    # 近似轮廓
    peri = cv2.arcLength(c, True)  # 计算轮廓的周长
    approx = cv2.approxPolyDP(c, 0.01 * peri, True)  # 曲线拟合

    # 判断当前的轮廓是不是矩形
    return not len(approx) == 4


def is_rectangle(c):
    # 近似轮廓
    peri = cv2.arcLength(c, True)  # 计算轮廓的周长
    approx = cv2.approxPolyDP(c, 0.02 * peri, True)  # 曲线拟合

    # 判断当前的轮廓是不是矩形
    return len(approx) == 4


# def draw_minRec(c):
#     # rect = cv2.minAreaRect(Max)
#     # # print(rect)
#     # box = np.int0(cv2.boxPoints(rect))
#     # print(box)
#     # cv2.drawContours(mask, [box], 0, (0, 255, 0), 3)


def myApprox(self, con):  # con为预先得到的最大轮廓
    num = 0.001
    # 初始化时不需要太小，因为四边形所需的值并不很小
    ep = num * cv2.arcLength(con, True)
    con = cv2.approxPolyDP(con, ep, True)
    while (1):
        if len(con) <= 4:  # 防止程序崩溃设置的<=4
            break
        else:
            num = num * 1.5
            ep = num * cv2.arcLength(con, True)
            con = cv2.approxPolyDP(con, ep, True)
            continue
    return con


def find_cloaction(Max):
    array = [[], [], [], []]
    array[0] = Max[0][0]
    array[1] = Max[0][0]
    array[2] = Max[0][0]
    array[3] = Max[0][0]
    for item in Max:
        # print(item) #[[1486 1733]]
        if item[0][1] > array[0][1]:
            array[0] = item[0]
        if item[0][0] > array[1][0]:
            array[1] = item[0]
        if item[0][1] < array[2][1]:
            array[2] = item[0]
        if item[0][0] < array[3][0]:
            array[3] = item[0]
    # print(array)
    return array


def find_circle(imageSource, save_dir):
    # 首先读取图片；然后进行颜色转换；最后进行边缘检测
    image = cv2.imread(imageSource)  # 读取的图像格式是BGR，数据范围0-255
    # lower = np.array([0, 0, 0])
    # upper = np.array([100, 100, 100])
    # shapeMask = cv2.inRange(image, 50, 100)
    gray = cv2.cvtColor(image, cv2.COLOR_BGR2GRAY)  # cv2.cvtColor(p1,p2) 是颜色空间转换函数，p1是需要转换的图片，p2是转换成何种格式。
    # print(gray)
    # cv2.COLOR_BGR2RGB 将BGR格式转换成RGB格式;cv2.COLOR_BGR2GRAY 将BGR格式转换成灰度图片
    edged = cv2.Canny(gray, 170, 255)
    # cv2.imshow(edged)
    # Canny检测算子,src表示输入的图片， thresh1表示最小阈值，thresh2表示最大阈值，用于进一步删选边缘信息
    # cv2.imshow("Original", image)

    # 寻找图中的轮廓并设置mask
    cnts = cv2.findContours(edged.copy(), cv2.RETR_LIST, cv2.CHAIN_APPROX_SIMPLE)
    cnts = imutils.grab_contours(cnts)
    mask = np.ones(image.shape[:2], dtype="uint8") * 255
    # print(cnts)
    # 循环遍历所有的轮廓
    flag = 0
    for c in cnts:
        if is_circle(c):
            # 检测该轮廓的类型，在新的mask中绘制结果
            if flag == 0:
                Max = c
                flag = 1
            else:
                if cv2.contourArea(Max) < cv2.contourArea(c):  # contourArea 计算轮廓的面积
                    Max = c
    # print(Max)
    # 返回圆轮廓的四个坐标 亦可拟合出圆心来做第五个坐标
    array1 = find_cloaction(Max)

    # 查找标志物结果
    peri = cv2.arcLength(Max, True)  # 计算轮廓的周长
    approx = cv2.approxPolyDP(Max, 0.01 * peri, True)  # 曲线拟合
    print(len(approx))
    # print(cv2.contourArea(Max) / 3.1415)
    cv2.drawContours(mask, [Max], -1, 0, -1)
    print(math.sqrt(cv2.contourArea(Max) / pi))
    # 移除不满足条件的轮廓并显示结果
    image = cv2.bitwise_and(image, image, mask=mask)
    print(save_dir)
    t = str(random.randint(1, 10))
    print(t)
    path = str(save_dir) + "\\" + t + ".png"
    print("test :" + path)
    cv2.imwrite(path, mask)
    dst = cv2.resize(mask, None, fx=0.2, fy=0.2, interpolation=cv2.INTER_LINEAR)
    cv2.imshow("Mask", dst)
    dst1 = cv2.resize(image, None, fx=0.2, fy=0.2, interpolation=cv2.INTER_LINEAR)
    cv2.imshow("After", dst1)
    cv2.waitKey(0)

    return array1


# 两副图解决3Dto2D坐标问题
def calculate_Epnp(circle1, circle2, target, save_dir):
    p = np.zeros((1, 4, 3), np.float32)
    p1 = np.zeros((1, 8, 2), np.float32)

    # 上、右、下、左  注意！ 左上坐标原点
    p[0][0] = np.array([50, 0, 0])
    p[0][1] = np.array([100, 50, 0])
    p[0][2] = np.array([50, 100, 0])
    p[0][3] = np.array([0, 50, 0])
    # p[0][0] = np.array([0, 0, 0])
    # p[0][1] = np.array([0, 200, 0])
    # p[0][2] = np.array([170, 0, 0])
    # p[0][3] = np.array([170, 200, 0])

    p1[0][0] = circle1[0]
    p1[0][1] = circle1[1]
    p1[0][2] = circle1[2]
    p1[0][3] = circle1[3]

    p1[0][4] = circle2[0]
    p1[0][5] = circle2[1]
    p1[0][6] = circle2[2]
    p1[0][7] = circle2[3]

    # p1[0][0] = np.array([1196, 1601])
    # p1[0][1] = np.array([1214, 2386])
    # p1[0][2] = np.array([1860, 1574])
    # p1[0][3] = np.array([1907, 2354])
    #
    # p1[0][4] = np.array([1068, 237])
    # p1[0][5] = np.array([1026, 1968])
    # p1[0][6] = np.array([2555, 235])
    # p1[0][7] = np.array([2566, 1980])

    # print(p1)
    meth = EPNP
    res = meth.Epnp(p, p1, target, save_dir)
    return res


