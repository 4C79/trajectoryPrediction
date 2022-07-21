import cv2


def video2frame(videos_path, frames_save_path, time_interval):
    '''
    :param videos_path: 视频的存放路径
    :param frames_save_path: 视频切分成帧之后图片的保存路径
    :param time_interval: 保存间隔
    :return:
    '''
    vidcap = cv2.VideoCapture(videos_path)
    success, image = vidcap.read()
    count = 0
    while success:
        success, image = vidcap.read()
        count += 1
        path = frames_save_path + str(count) + ".jpg"
        print(path)
        cv2.imencode('.jpg', image)[1].tofile(path)
        # if count == 20:
        #   break


if __name__ == '__main__':
    videos_path = 'F:\\姜\\相机\\高速相机MS28-H\\1\\1.MP4'
    frames_save_path = 'E:\\1\\'
    time_interval = 1  # 隔一帧保存一次
    video2frame(videos_path, frames_save_path, time_interval)
