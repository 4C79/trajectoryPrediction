import socket


class viewData():
    id = 1002  # 编号
    type = 1  # 模型类型例如导弹靶标等
    state = 1  # 模型状态例如飞行爆炸销毁等
    head = 0  # 模型偏航角
    pitch = 0  # 模型俯仰角
    roll = 100  # 模型滚转角

    def __init__(self, x, y, z):
        self.x = x
        self.y = y
        self.z = z


def transport(data):
    # 初始化socket
    client_socket = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

    # 初始化接收端ip，端口号
    inet_addr = "127.0.0.1"
    Port = 8001
    server_address = (inet_addr, Port)

    size = len(data[0])

    for i in range(0, size):
        vD = viewData(data[i][0], data[i][1], data[i][2])
        client_socket.sendto(vD, server_address)
