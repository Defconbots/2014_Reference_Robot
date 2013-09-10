#Defconbots 2014 Reference Robot Design

This is a reference design for the 2014 defconbots competition. It is meant as a nothing more than a starting point for aspiring hardware ethusiasts. The solution is intentionally simple and should not be used for the final competition. You will not win the competition with the reference design.

## Hardware

1. TTL controllable laser ($12): https://www.sparkfun.com/products/8654
2. Laser mount ($5): https://www.sparkfun.com/products/8674
3. Servo ($8 x 2): https://www.sparkfun.com/products/9065
4. Pan-tilt bracket ($6): https://www.sparkfun.com/products/10335
5. Arduino Pro Micro 5V/16MHz($20): https://www.sparkfun.com/products/11098
6. Webcam of your choice ($14): http://www.amazon.com/Logitech-QuickCam-for-Notebooks-Silver/dp/B000NB2GF8/

Total Price: ~$75

note: The logitech quickcam used is not recommended. The framerate and picture are terrible.

## Assembly

(insert schematic here)

## Firmware

Clone this repo and load the hardware_interface.ino onto your arduino. This firmware interprets serial commands in the form of "n,n,n\n" into the servo position and laser on/off.

## Software

Clone this repo and load TowerDefender.sln. Run/Edit to your liking.


