import socket
import time
from gpio_controlls import gpio_controlls
from multiprocessing.pool import ThreadPool
import threading
#k
val = [0]
secVal = [0]
controll = gpio_controlls(0,0,24,23,25,4,0,0,6)

def counter():
    while (True):
        time.sleep(0.1)
        val[0] += 1
        if (val[0] == 20):
            secVal[0] = 1

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
    print(int_forwardbackward,int_rightleft)

    if (secVal[0] == 1):
        controll.setValues(int_forwardbackward,int_rightleft,1)
        #val[0] = 0
        print("a")
        secVal[0] = 0
    else:
        controll.setValues(int_forwardbackward,int_rightleft,0)
        print("b")

    if  int_rightleft != 0:
        val[0] = 0
        print("c")

