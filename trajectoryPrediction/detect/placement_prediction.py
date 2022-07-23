import cv2
from detect.kalmanfilter import KalmanFilter
from detect import blackCircle_Finder
from detect.EPNP import calculate
from detect import orange_prediction
from detect import LSM
import os

def getAns(path_l,path_r):

    bf = blackCircle_Finder
    op = orange_prediction
    lm = LSM

    # 初始化结果坐标信息
    res = []

    #得到文件列表
    path_l_list = os.listdir(path_l)
    path_r_list = os.listdir(path_r)

    # #将文件列表按数字从小到大排序
    # path_l_list.sort(key=lambda x: int(x.split('.')[0]))
    # path_r_list.sort(key=lambda x: int(x.split('.')[0]))

    # 获取左右目文件数量，返回出错信息
    if(len(path_l_list)!=len(path_r_list)):
            print("左右目图片数不一致，请检查！")
            quit()

    # 循环同时处理左右目图片
    for l,r in path_l_list,path_r_list :

        # 计算圆心三维坐标
        tmp = calculate(bf.circle_detectImage(l),bf.circle_detectImage(r))

        # 坚持圆失败
        if(tmp==False):
            print("检测失败")
            continue

        # save result
        res.append(tmp)

    return lm.lsm(res)

if __name__ == '__main__':
    getAns('F:\\双目相机材料\\calibration\\l','F:\\双目相机材料\\calibration\\r')