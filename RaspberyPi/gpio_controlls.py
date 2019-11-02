import RPi.GPIO as GPIO
import pigpio
import time
from multiprocessing.pool import ThreadPool

class gpio_controlls:

    def __init__(self, int1, int2, en, servo):

        self.int1 = int1  
        self.int2 = int2  
        self.en = en  
        self.servo = servo
        self.natocenie_old = 0
        self.natocenie_new = 0
        self.posledne_natocenie = 6    
        
        GPIO.setwarnings(False)
        GPIO.setmode(GPIO.BCM)
        GPIO.setup(self.int1,GPIO.OUT)
        GPIO.setup(self.int2,GPIO.OUT)
        GPIO.setup(self.en,GPIO.OUT)
        GPIO.output(self.int1,GPIO.LOW)
        GPIO.output(self.int2,GPIO.LOW)
        
        self.motor=GPIO.PWM(self.en,1000)
        self.pi = pigpio.pi()
        self.pi.set_mode(self.servo, pigpio.OUTPUT) 
    
    def setValues(self, forwardbackward, rightleft, state):
        self.forwardbackward = forwardbackward
        self.rightleft = rightleft
        self.natocenie_old = self.natocenie_new
        self.natocenie_new = self.rightleft
        self.state = state
        self.posun = round(self.rightleft*1.176, 0)+1400
  
   
        
        #forward
        if self.forwardbackward > 0:
            self.motor.start(self.forwardbackward/2.55)
            GPIO.output(self.int1,GPIO.LOW)
            GPIO.output(self.int2,GPIO.HIGH)

        #backward
        if self.forwardbackward < 0:
            self.motor.start((self.forwardbackward*-1)/2.55)
            GPIO.output(self.int1,GPIO.HIGH)
            GPIO.output(self.int2,GPIO.LOW)

        #servo controll
        self.pi.set_servo_pulsewidth(4, self.posun)

        #posledne natocenie zprava
        if self.natocenie_new == 0 and self.natocenie_old > 0:
            self.posledne_natocenie = 8

        #posledne natocenie zlava
        if self.natocenie_new == 0 and self.natocenie_old < 0:
            self.posledne_natocenie = 9

        #cenotrvanie serva

        #zprava
        if self.posledne_natocenie == 8 and self.state == 1:
            self.pi.set_servo_pulsewidth(4, 1378)
            time.sleep(0.5)
            self.pi.set_servo_pulsewidth(4, 1400)

        #zlava
        if self.posledne_natocenie == 9 and self.state == 1:
            self.pi.set_servo_pulsewidth(4, 1431)
            time.sleep(0.5)
            self.pi.set_servo_pulsewidth(4, 1400)
        #disable GPIO pins when car is not moving
        if self.forwardbackward == 0:
            GPIO.output(self.int1,GPIO.LOW)
            GPIO.output(self.int2,GPIO.LOW)
          
        print(self.forwardbackward, self.rightleft, self.state, self.natocenie_old, self.natocenie_new, self.posledne_natocenie, self.state)