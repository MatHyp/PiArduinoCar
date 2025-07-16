import serial
import time

ser = serial.Serial('/dev/ttyACM0', 9600, timeout=1)

time.sleep(2)  


ser.write(b'A')

response = ser.readline().decode('utf-8').rstrip()
print("Odpowiedź z Arduino:", response)

ser.close()
