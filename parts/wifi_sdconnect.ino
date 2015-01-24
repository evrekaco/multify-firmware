/*
 * 
 */
#include "application.h"
#include "sd-card-library/sd-card-library.h"

const uint8_t chipSelect = A2;

File credential;

SYSTEM_MODE(SEMI_AUTOMATIC);

int i = 0;
int run = 1;
char ssid[64];
char pass[64];

void setup() 
{
    pinMode(A0,OUTPUT);
    
    Serial.begin(9600);
    
    // Initialize HARDWARE SPI with user defined chipSelect
    if (!SD.begin(chipSelect)) 
    {
        Serial.println("initialization failed!");
    }
    
    credential = SD.open("ssid.txt");
    
    i = 0;
    run = 1;
    while(run)
    {
        char ch = credential.read();
        if(ch == ',')
        {
            run = 0;
            ssid[i] = '\0';
        }
        else
        {
            ssid[i] = ch;
        }
        i++;
    }
    
    i = 0;
    run = 1;
    while(run)
    {
        char ch = credential.read();
        if(ch == ',')
        {
            run = 0;
            pass[i] = '\0';
        }
        else
        {
            pass[i] = ch;
        }
        i++;
    }
    
    credential.close();

    digitalWrite(A0,HIGH);
    
    if(Spark.connected()) 
    {
        Spark.disconnect();
    }
    
    WiFi.setCredentials(ssid,pass);
    Spark.connect();
    
    while(Spark.connected() == false) 
    {
        Spark.connect();
        Serial.print("Name: ");
        Serial.println(ssid);
        Serial.print("Pass: ");
        Serial.println(pass);
        Serial.println("Trying ...");
        delay(500);
    }
    digitalWrite(A0,LOW);
}

void loop() 
{ 
    digitalWrite(A0,HIGH);
    delay(100);
    digitalWrite(A0,LOW);
    delay(100);
    
    Serial.println("------------------------");
    Serial.println("Connected to:");
    Serial.print("Name: ");
    Serial.println(ssid);
    Serial.print("Pass: ");
    Serial.println(pass);
}
