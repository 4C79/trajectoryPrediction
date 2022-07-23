import cv2
from detect.kalmanfilter import KalmanFilter
from detect import blackCircle_Finder
from detect.EPNP import calculate
from detect import orange_prediction
from detect import LSM
import os


def getAns(path_l, path_r):
    bf = blackCircle_Finder
    op = orange_prediction
    lm = LSM

    # 初始化结果坐标信息
    res = []

    # 得到文件列表
    path_l_list = os.listdir(path_l)
    path_r_list = os.listdir(path_r)

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
        l = cv2.imread(path_l + '\\\\' + path_l_list[i])
        r = cv2.imread(path_r + '\\\\' + path_r_list[i])
        tmp = calculate(bf.circle_detectImage(l), bf.circle_detectImage(r))

        # 坚持圆失败
        if (tmp == []):
            print("检测失败" + str(i))
            continue

        # save result
        res.append(tmp)

    return lm.lsm(res)
