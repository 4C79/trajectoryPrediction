from PyQt5 import QtCore, QtGui, QtWidgets
import sys, os

from PyQt5.QtGui import QPixmap
from PyQt5.QtWidgets import QApplication, QMainWindow ,QMessageBox
import tkinter as tk
from tkinter import filedialog
from detect import placement_prediction
from detect import udp


class Ui_MainWindow(object):

    origin_path = "data"
    pp = placement_prediction
    up = udp
    data = []

    def setupUi(self, MainWindow):
        MainWindow.setObjectName("MainWindow")
        MainWindow.resize(550, 500)
        self.centralwidget = QtWidgets.QWidget(MainWindow)
        self.centralwidget.setObjectName("centralwidget")
        self.pushButton_1 = QtWidgets.QPushButton(self.centralwidget)
        self.pushButton_1.setGeometry(QtCore.QRect(110, 370, 91, 21))
        self.pushButton_1.setObjectName("pushButton")
        self.label = QtWidgets.QLabel(self.centralwidget)
        self.label.setGeometry(QtCore.QRect(60, 20, 421, 331))
        self.label.setFrameShape(QtWidgets.QFrame.Box)
        self.label.setObjectName("label")
        self.pushButton_2 = QtWidgets.QPushButton(self.centralwidget)
        self.pushButton_2.setGeometry(QtCore.QRect(180, 410, 91, 21))
        self.pushButton_2.setObjectName("pushButton_2")
        self.pushButton_3 = QtWidgets.QPushButton(self.centralwidget)
        self.pushButton_3.setGeometry(QtCore.QRect(290, 410, 91, 21))
        self.pushButton_3.setObjectName("pushButton_3")
        self.pushButton_4 = QtWidgets.QPushButton(self.centralwidget)
        self.pushButton_4.setGeometry(QtCore.QRect(60, 410, 91, 21))
        self.pushButton_4.setObjectName("pushButton_4")
        self.pushButton_5 = QtWidgets.QPushButton(self.centralwidget)
        self.pushButton_5.setGeometry(QtCore.QRect(410, 410, 91, 21))
        self.pushButton_5.setObjectName("pushButton_5")
        self.pushButton_6 = QtWidgets.QPushButton(self.centralwidget)
        self.pushButton_6.setGeometry(QtCore.QRect(340, 370, 91, 21))
        self.pushButton_6.setObjectName("pushButton_6")
        MainWindow.setCentralWidget(self.centralwidget)
        self.statusbar = QtWidgets.QStatusBar(MainWindow)
        self.statusbar.setObjectName("statusbar")
        MainWindow.setStatusBar(self.statusbar)

        self.pushButton_1.clicked.connect(self.task_1)
        self.pushButton_2.clicked.connect(self.task_2)
        self.pushButton_3.clicked.connect(self.task_3)
        self.pushButton_4.clicked.connect(self.task_4)
        self.pushButton_5.clicked.connect(self.task_5)
        self.pushButton_6.clicked.connect(self.task_6)

        self.retranslateUi(MainWindow)
        QtCore.QMetaObject.connectSlotsByName(MainWindow)

    def retranslateUi(self, MainWindow):
        _translate = QtCore.QCoreApplication.translate
        MainWindow.setWindowTitle(_translate("MainWindow", "MainWindow"))
        self.pushButton_1.setText(_translate("MainWindow", "选定文件"))
        self.label.setText(_translate("MainWindow", "Display"))
        self.pushButton_2.setText(_translate("MainWindow", "kanlman演示"))
        self.pushButton_3.setText(_translate("MainWindow", "三维轨迹"))
        self.pushButton_4.setText(_translate("MainWindow", "lsm轨迹拟合"))
        self.pushButton_5.setText(_translate("MainWindow", "3D演示"))
        self.pushButton_6.setText(_translate("MainWindow", "开始处理"))

    # 选择左右目拍摄的图片，不选默认就是../data
    def task_1(self):
        root = tk.Tk()
        root.withdraw()
        # 获取文件夹路径
        self.origin_path = filedialog.askdirectory()  # 获得选择好的文件夹

    # 运行程序，检测轨迹
    def task_2(self):
        self.pp.kalmanFilter(self.origin_path+"//l")
        resource = "img_kal.jpg"
        self.label.setScaledContents(True)
        self.label.setPixmap(QPixmap(resource))

    def task_3(self):
        resource = "trajectory.jpg"
        self.label.setScaledContents(True)
        self.label.setPixmap(QPixmap(resource))

    def task_4(self):
        resource = "trajectory_prediction.jpg"
        self.label.setScaledContents(True)
        self.label.setPixmap(QPixmap(resource))

    def task_5(self):
        self.up.transport(self.data)

    def task_6(self):
        self.data = self.pp.getAns(self.origin_path + '\\\\l', self.origin_path + '\\\\r')
        msg_box = QMessageBox(QMessageBox.Information, '提示', '处理完毕，落点预测坐标为：' + str(self.data[0]))
        msg_box.exec_()

    def get_file(path):  # 获取所有文件
        all_file = []
        for f in os.listdir(path):  #  listdir返回文件中所有目录
            f_name = os.path.join(path, f)
            all_file.append(f_name)
        return all_file

if __name__ == '__main__':
    app = QApplication(sys.argv)
    MainWindow = QMainWindow()
    ui = Ui_MainWindow()
    ui.setupUi(MainWindow)
    MainWindow.show()
    sys.exit(app.exec_())