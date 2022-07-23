import numpy as np
import matplotlib.pyplot as plt
from mpl_toolkits.mplot3d import Axes3D

point = [[187, 217, 205], [195, 215, 192], [204, 214, 179], [212, 212, 166], [220, 210, 153], [229, 208, 140],
         [237, 206, 137, ], [245, 204, 114], [253, 202, 101], [261, 200, 88], [269, 198, 75], [277, 196, 62]]
test = [[285, 194, 49], [293, 192, 36], [301, 190, 23], [309, 188, 10], [317, 186, -2], [325, 184, -15],
        [333, 182, -27]]

all = [[187, 217, 205], [195, 215, 192], [204, 214, 179], [212, 212, 166], [220, 210, 153], [229, 208, 140],
       [237, 206, 137, ], [245, 204, 114], [253, 202, 101], [261, 200, 88], [269, 198, 75], [277, 196, 62],
       [285, 194, 49], [293, 192, 36], [301, 190, 23], [309, 188, 10], [317, 186, -2], [325, 184, -15], [333, 182, -27]]


def get_xyz(data):
    l = len(data)
    x = np.zeros(l)
    y = np.zeros(l)
    z = np.zeros(l)
    for i in range(l):
        x[i] = data[i][0]
        y[i] = data[i][1]
        z[i] = data[i][2]
    return x, y, z


def get_parameter(x, y, z, n):
    f1 = np.polyfit(z, x, n)
    f2 = np.polyfit(z, y, n)
    return f1, f2


def function(para, input):
    output = para[2] + para[1] * input + para[0] * input * input
    return output


def lsm(point):
    x, y, z = get_xyz(point)
    f1, f2 = get_parameter(x, y, z, 2)
    return (function(f1,0), function(f2,0))


if __name__ == '__main__':

    x, y, z = get_xyz(point)
    x_a, y_a, z_a = get_xyz(all)

    ax = plt.subplot(111, projection='3d')
    ax.scatter(x_a, y_a, z_a, c='b')

    f1_a, f2_a = get_parameter(x_a, y_a, z_a, 2)
    x_a_0 = function(f1_a, 0)
    y_a_0 = function(f2_a, 0)
    ax.scatter(x_a_0, y_a_0, 0, c='y', label='(%s, %s, 0)' % (x_a_0, y_a_0))

    f1, f2 = get_parameter(x, y, z, 2)
    # test = [[285,194,49], [293,192,36], [301,190,23], [309,188,10], [317,186,-2], [325,184,-15], [333,182,-27]]
    x_pre = np.zeros(len(test))
    y_pre = np.zeros(len(test))
    z_pre = np.zeros(len(test))

    for i in range(len(test)):
        z_pre[i] = test[i][2]
        x_pre[i] = function(f1, z_pre[i])
        y_pre[i] = function(f2, z_pre[i])

    print(function(f1, 0), function(f2, 0))
    print(lsm(all))

    ax.scatter(x_pre, y_pre, z_pre, c="r")
    x_0 = function(f1, 0)
    y_0 = function(f2, 0)
    ax.scatter(x_0, y_0, 0, c='g', label='(%s, %s, 0)' % (x_0, y_0))
    plt.legend()
    plt.show()
