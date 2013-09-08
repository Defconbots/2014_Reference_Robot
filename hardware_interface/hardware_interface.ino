#include <Servo.h> 

int fricken_laser = 7;
Servo s1;
Servo s2;

// for printf
int my_putc(char c, FILE *t)
{
    Serial.write(c);
}

void setup()
{
    fdevopen(&my_putc, 0);  
    Serial.begin(57600);
    Serial.setTimeout(1000);
    s2.attach(3);
    s1.attach(9);
    pinMode(fricken_laser, OUTPUT);
}

void loop()
{
    char buf[10];
    int num_read = 0;
    memset(buf,0,sizeof(buf));
    num_read = Serial.readBytesUntil('\n',buf,10);
    if (num_read == 10)
    {
        int pos1 = 0;
        int pos2 = 0;
        int laser_on = 0;
        sscanf(buf,"%d,%d,%d\n",&pos1,&pos2,&laser_on);
        s1.write(pos1);
        s2.write(pos2);
        digitalWrite(fricken_laser,laser_on ? LOW : HIGH);
        printf("p1: %d p2: %d laser: %d\n",pos1,pos2,laser_on);
    }
}

// samples datas
// 100,100,1

// 150,100,0

