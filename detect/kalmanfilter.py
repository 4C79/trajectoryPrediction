# -*- coding: UTF-8 -*-
import cv2
import numpy as np


class KalmanFilter:

    kf = cv2.KalmanFilter(4, 2)  # 4：状态数，包括（x，y，dx，dy）坐标及速度（每次移动的距离）；2：观测量，能看到的是坐标值
    kf.measurementMatrix = np.array([[1, 0, 0, 0], [0, 1, 0, 0]], np.float32)  # 系统测量矩阵
    kf.transitionMatrix = np.array([[1, 0, 1 / 15, 0], [0, 1, 0, 1 / 15], [0, 0, 1, 0], [0, 0, 0, 1]],
                                   np.float32)  # 状态转移矩阵

    def predict2D(self, coordX, coordY):
        ''' This function estimates the position of the object'''

        measured = np.array([[np.float32(coordX)], [np.float32(coordY)]])
        self.kf.correct(measured)
        predicted = self.kf.predict()
        x, y = int(predicted[0]), int(predicted[1])
        return x, y
