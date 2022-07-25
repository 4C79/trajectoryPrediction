import sys
from PyQt5.QtWidgets import QApplication, QMainWindow
import os
import main

if __name__ == '__main__':
    # os.system('D:\software\Whale.exe')
    app = QApplication(sys.argv)
    MainWindow = QMainWindow()
    ui = main.Ui_MainWindow()
    ui.setupUi(MainWindow)
    MainWindow.show()
    sys.exit(app.exec_())