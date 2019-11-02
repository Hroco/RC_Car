import socket
import time
import threading
from gpio_controlls import gpio_controlls
from multiprocessing.pool import ThreadPool

count = [0]
reset = [0]
controll = gpio_controlls(24,23,25,4)

def counter():
    while (True):
        time.sleep(0.1)
        count[0] += 1
        if (count[0] == 20):
            reset[0] = 1

def udp_server(host='192.168.1.33', port=50000):
    s = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    s.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
    s.bind((host, port))
    threading.Thread(target=counter).start()
    while True:
        (data, addr) = s.recvfrom(128*1024)
        yield data

hodnota = 0

for data in udp_server():
    data = data.decode()
    x = data.split(",")
    forwardbackward = x[0]
    rightleft = x[1]
    int_forwardbackward = int(forwardbackward)
    int_rightleft = int(rightleft)

    hodnota = int_rightleft

    if (reset[0] == 1):
        state = 1
        reset[0] = 0
    else:
        state = 0
    if  int_rightleft != 0:
        count[0] = 0

    controll.setValues(int_forwardbackward,int_rightleft,state)
