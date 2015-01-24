// This #include statement was automatically added by the Spark IDE.
#include "sd-card-library/sd-card-library.h"
#include "application.h"


char hard_state_read[6];
long int hard_state=0;
File state;

// SOFTWARE SPI pin configuration - modify as required
// The default pins are the same as HARDWARE SPI
const uint8_t chipSelect = D0;    // Also used for HARDWARE SPI setup
const uint8_t mosiPin = D3;
const uint8_t misoPin = D2;
const uint8_t clockPin = D1;


void setup()
{
  Serial.begin(9600);
  
  //HARDWARE SPI
  //SD.begin(chipSelect);
  
  //SOFTWARE SPI
  SD.begin(mosiPin, misoPin, clockPin, chipSelect);
}


void loop()
{
    
    while(analogRead(A7)==0){
        SD_read();
        /* Buraya ilgili motorları döndürme kısmı gelecek */
        
        SD_write(hard_state+1);
        
    }
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
