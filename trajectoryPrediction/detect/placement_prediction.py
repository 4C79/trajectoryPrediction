import cv2
from detect.kalmanfilter import KalmanFilter
from detect import blackCircle_Finder
from detect.EPNP import calculate
from detect import orange_prediction
from detect import LSM
import os

def saveProcess_l(frame,path,box):
    cv2.circle(frame,box, 20, (255, 0, 0), 4)
    path = "..\\\\result\\\\l\\\\" + path
    cv2.imwrite(path,frame)

def saveProcess_r(frame,path,box):
    cv2.circle(frame,box, 20, (255, 0, 0), 4)
    path = "..\\\\result\\\\r\\\\" + path
    cv2.imwrite(path,frame)

def getAns(path_dir_l, path_dir_r):
    bf = blackCircle_Finder
    op = orange_prediction
    lm = LSM

    # 初始化结果坐标信息
    res = []

    # 得到文件列表
    path_l_list = os.listdir(path_dir_l)
    path_r_list = os.listdir(path_dir_r)

    # #将文件列表按数字从小到大排序
    # path_l_list.sort(key=lambda x: int(x.split('.')[0]))
    # path_r_list.sort(key=lambda x: int(x.split('.')[0]))

    # 获取左右目文件数量，返回出错信息
    if (len(path_l_list) != len(path_r_list)):
        print("左右目图片数不一致，请检查！")
        quit()

    # 循环同时处理左右目图片
    for i in range(0, len(path_l_list)):
        # 计算圆心三维坐标
        path_l = path_dir_l + '\\\\' + path_l_list[i]
        path_r = path_dir_r + '\\\\' + path_r_list[i]
        l = cv2.imread(path_l)
        r = cv2.imread(path_r)
        circle_l = bf.circle_detectImage(l)
        circle_r = bf.circle_detectImage(r)
        print(circle_l,circle_r)
        saveProcess_l(l,path_l_list[i],circle_l)
        saveProcess_r(r,path_r_list[i],circle_r)
        tmp = calculate(circle_l, circle_r)
        print(tmp)
        # 坚持圆失败
        if (tmp == []):
            print("检测失败" + str(i))
            continue

        # save result
        res.append(tmp)

    return lm.lsm(res)
