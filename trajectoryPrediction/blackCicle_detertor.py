# 查找圆（精确度高）
import math
import cv2
import numpy as np
import imutils
import random
from PIL import Image

pi = 3.14159265358979324

def is_circle(c):
    # 近似轮廓
    peri = cv2.arcLength(c, True)  # 计算轮廓的周长
    approx = cv2.approxPolyDP(c, 0.01 * peri, True)  # 曲线拟合
    # 判断当前的轮廓是不是矩形
    return not len(approx) == 4

def find_circle(frame):

    gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)

    edged = cv2.Canny(gray, 170, 255)

    # 寻找图中的轮廓并设置mask
    cnts = cv2.findContours(edged.copy(), cv2.RETR_LIST, cv2.CHAIN_APPROX_SIMPLE)
    cnts = imutils.grab_contours(cnts)
    cnts = sorted(cnts, key=lambda x: cv2.contourArea(x), reverse=True)

    box = (0, 0, 0, 0)
    for cnt in cnts:
        if is_circle(cnt):
            (x, y, w, h) = cv2.boundingRect(cnt)
            box = (x, y, x + w, y + h)
            break
    return box

if __name__ == '__main__':
    image = Image.open("C:\\Users\\soda\\Desktop\\2\\frame7.jpg")
    cnt = np.array(image)

    bbox = find_circle(cnt)
    x, y, x2, y2 = bbox
    cx = int((x + x2) / 2)
    cy = int((y + y2) / 2)

    cv2.circle(cnt, (cx, cy), 20, (0, 0, 255), 4)

    cv2.namedWindow('Frame', 0)

    cv2.resizeWindow('Frame', 900, 900)  # 自己设定窗口图片的大小

    cv2.imshow("Frame", cnt)
    cv2.waitKey(0)

