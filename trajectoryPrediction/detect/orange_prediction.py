import cv2
from detect.kalmanfilter import KalmanFilter
from detect import blackCircle_Finder
from detect.EPNP import calculate


def prediction(frame):
    bd = blackCircle_Finder
    bbox = bd.circle_detect(frame)
    kf = KalmanFilter()
    if bbox:
        cx, cy = bbox
    else:
        cx = 0
        cy = 0

    predicted = kf.predict(cx, cy)
    # cv2.rectangle(frame, (x, y), (x2, y2), (255, 0, 0), 4)
    cv2.circle(frame, (cx, cy), 20, (0, 0, 255), 4)
    cv2.circle(frame, (predicted[0], predicted[1]), 20, (255, 0, 0), 4)
    cv2.namedWindow('Frame', 0)
    cv2.resizeWindow('Frame', 900, 900)  # 自己设定窗口图片的大小
    cv2.imshow("Frame", frame)
    cv2.waitKey(150)


def binocular_videoPrediction():
    # load video
    capl = cv2.VideoCapture("C:\\Users\\soda\\Desktop\\无人\\1l.avi")
    capr = cv2.VideoCapture("C:\\Users\\soda\\Desktop\\无人\\1r.avi")

    # Load detector
    bd = blackCircle_Finder

    # Load KalmanFilter to predict the trajectory
    kf = KalmanFilter()

    # keep ans
    res = []

    # find target every frame
    while True:
        retl, framel = capl.read()
        retr, framer = capr.read()
        if retl or retr is False:
            break

        # bbox = od.detect(frame)
        bboxl = bd.circle_detect(framel)
        bboxr = bd.circle_detect(framer)

        # calculate the world coordinate
        wrold_now = calculate(bboxl, bboxr)
        print(wrold_now)

        # kalmanfilter pridict next world coordinate
        predicted = kf.predict(wrold_now)

        res.append(predicted)

    return res

def binocular_pictureTransformation(picture1,picture2):

    # Load detector
    bd = blackCircle_Finder

     # bbox = od.detect(frame)
    bboxl = bd.circle_detect(picture1)
    bboxr = bd.circle_detect(picture2)

    # calculate the world coordinate
    wrold_now = calculate(bboxl, bboxr)

    return wrold_now
