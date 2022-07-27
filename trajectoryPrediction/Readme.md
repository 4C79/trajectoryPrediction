**背景**

```
通过两个相机拍摄目标飞行轨迹来预测其三维落点
```

**功能**

```
1. 相机标定 （./calibration/calibration.py ）
2. 视频分帧 （./detect/cutVideo.py）
3. 拍摄数据，以（640，480）分辨率保持图片 保证在40帧左右 （./camera/cv_grab.py/getData）
4. 检测黑色圆球，通过 cv2.HoughCircles 函数调整参数保证检测到目标 (./detect/blackCircle_Finder.py)
5. 通过文件读取图片数据，检测到目标时开始，检测失败时结束 (./detect/placement_prediction.py)
6. 使用kalman Filter进行轨迹预测，达到演示效果 (./detect/kalmanfilter.py)
7. 使用Epnp对固定场景下的4组控制点进行计算，从而求出目标三维坐标 (./detect/EPNP.py/calculate)
8. 通过LSM拟合三维坐标，预估目标落点 (./detect/LSM.py/lsm)
9. 软件界面演示 （display.py）
```

**不足**

```
1. 标定板不够精确，不能保证平整 ， 相机感光元件尺寸没有计算出
2. 不能保证左右目 高帧且同步 ，如果是保持问题，是否跟cpu性能、带宽有关 ，如何通过硬件同步（同步器）
3. 圆球检测时，时常失败，需要调参（如何优化此过程 1.先处理，找出最优阈值 2.标注第一帧，从其附近查找 3.处理完毕后，将检测失败的圆去除，通过kalman补帧）、使用yolo等检测算法寻找目标
4. 自动检测目标时，如何处理 误检，漏检 问题
5. 如何减少Epnp计算所产生的误差
6. LSM拟合效果如何？考虑ELSM？
7. 优化软件界面
```

**误差**

```

```

**硬件参数**

```

```

**技术原理**

```

```

**环境**

```
certifi           2022.6.15
click             7.1.2
cycler            0.11.0
fonttools         4.34.4
imutils           0.5.4
kiwisolver        1.4.3
matplotlib        3.5.2
numpy             1.21.6
opencv-python     3.4.11.39
packaging         21.3
Pillow            9.2.0
pip               22.1.2
pyparsing         3.0.9
PyQt5             5.15.4
pyqt5-plugins     5.15.4.2.2
PyQt5-Qt5         5.15.2
PyQt5-sip         12.11.0
pyqt5-tools       5.15.4.3.2
python-dateutil   2.8.2
python-dotenv     0.20.0
qt5-applications  5.15.2.2.2
qt5-tools         5.15.2.1.2
scipy             1.7.3
setuptools        63.1.0
six               1.16.0
typing_extensions 4.3.0
wheel             0.37.1
wincertstore      0.2
```

