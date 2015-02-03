# bmp180-cs-test
I have created this app while creating lib for AVR for BMP180 - as you maybe know those are 8bit microcontrollers and avr-gcc compiler is somewhat a bitch when it comes to in32/int64 types :/ . 
So I have commited this app for testing code/equations for calculating pressure (and temperature) from Bosh BMP180.
It is possible to load own calibration data readed from sensor - this is done by calib.txt file. 
# calib.txt
This is normal text file, each line contain one calibration value. Calibration values (signed integers) are readed in order:
```
AC1
AC2
AC3
AC4
AC5
AC6
B1
B2
MB
MC
MD
```
