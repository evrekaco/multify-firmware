// This #include statement was automatically added by the Spark IDE.
#include "sd-card-library/sd-card-library.h"
#include "application.h"

TCPClient client;

byte server[] = { 160, 153, 72, 200 };
int count=0;
int i=0;

char hard_state_read[6];
long int hard_state=0;
File state;

const uint8_t chipSelect = D0;    // Also used for HARDWARE SPI setup
const uint8_t mosiPin = D3;
const uint8_t misoPin = D2;
const uint8_t clockPin = D1;

void setup()
{
  // Make sure your Serial Terminal app is closed before powering your Core
    Serial.begin(9600);
    SD.begin(mosiPin, misoPin, clockPin, chipSelect);
    pinMode(A7,INPUT);
}

void loop()
{
    if(i==0)
    {   while(analogRead(A7)!=0) SPARK_WLAN_Loop(); }
    
    SD_read(); 
    Serial.println("Starting...");
    
    if(client.connect(server, 80))
    {
        Serial.println("connected");
        client.println("GET /adminMultify/webservice.php?method=foursquare&device_id=1&format=html HTTP/1.0");
        client.println("Host: www.multify.co");
        client.println("Content-Length: 0");
        client.println();
    }
    else
    {
        Serial.println("connection failed");
    }
  
    while(client.connected())
    {
        if(client.available()){  
            char c = client.read();
            Serial.print(c);
        }
    }
    SD_write(hard_state+1);
    if(!client.connected())
    {
        Serial.println();
        Serial.println("disconnected.");
        client.flush();
    }   
   i++;
   delay(500);
}


void SD_read(){
        state = SD.open("state.txt");
        for(int i=0;i<6;i++)
        {
            hard_state_read[i] = state.read();
        }
        state.close();
        
        hard_state=atoi(hard_state_read);
        Serial.print("sd card değeri: ");
        Serial.println(hard_state);
}

void SD_write(int cnt){
        SD.remove("state.txt");
        state = SD.open("state.txt", FILE_WRITE);
      
      // if the file opened okay, write to it:
      if (state) {
        Serial.print("Writing to state.txt...");
        state.print(cnt);
      // close the file:
        state.close();
        Serial.println("done.");
      } else {
        // if the file didn't open, print an error:
        Serial.println("error opening state.txt");
      }
}
