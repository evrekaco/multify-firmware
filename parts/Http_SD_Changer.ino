// This #include statement was automatically added by the Spark IDE.
#include "HttpClient/HttpClient.h"
#include "sd-card-library/sd-card-library.h"
#include "application.h"


char hard_state_read[6];
long int hard_state=0;
File state;

/*
const uint8_t chipSelect = D0;    // Also used for HARDWARE SPI setup
const uint8_t mosiPin = D3;
const uint8_t misoPin = D2;
const uint8_t clockPin = D1;
*/

const uint8_t chipSelect = A2;    // Also used for HARDWARE SPI setup
const uint8_t mosiPin = A5;
const uint8_t misoPin = A4;
const uint8_t clockPin = A3;



byte server[] = { 160, 153, 72, 200 };
int server_state;
/**
* Declaring the variables.
*/
unsigned int nextTime = 0;    // Next time to contact the server
HttpClient http;

// Headers currently need to be set at init, useful for API keys etc.
http_header_t headers[] = {
    //  { "Content-Type", "application/json" },
    //  { "Accept" , "application/json" },
    { "Accept" , "*/*"},
    { NULL, NULL } // NOTE: Always terminate headers will NULL
};

http_request_t request;
http_response_t response;


int Change_state(String command);

void setup() {
    Spark.function("Changer", Change_state);
    Serial.begin(9600);
    SD.begin(chipSelect);
//    SD.begin(mosiPin, misoPin, clockPin, chipSelect);
    pinMode(A7,INPUT);
    pinMode(D7,OUTPUT);
    pinMode(D6,OUTPUT);
    digitalWrite(D6,HIGH);
}

void loop() {
    SD_read();
    while(hard_state==server_state)
    {
        digitalWrite(D6,LOW);
        Server_Check();
        digitalWrite(D6,HIGH);
        delay(500);
    }
    
    Serial.println("----------TURN MOTORS!!");
    SD_write(server_state);
}


void Server_Check(){
    if (nextTime > millis()) {
        return;
    }
    
    Serial.println();
    Serial.println("Application>\tStart of Loop.");
    // Request path and body can be set at runtime or at setup.
    request.hostname = "multify.co";
    request.ip=server;
    request.port = 80;
    request.path = "/adminMultify/webservice.php?method=foursquare&device_id=1&format=html";

    // The library also supports sending a body with your request:
    //request.body = "{\"key\":\"value\"}";
    
    // Get request
    http.get(request, response);

//    Serial.print("Application>\tResponse status: ");
    Serial.println(response.status);

//    Serial.print("Application>\tHTTP Response Body: ");
    Serial.println(response.body);
// This #include statement was automatically added by the Spark IDE.


    //string str = response.body;
    server_state = atoi(response.body.c_str());
    
    Serial.print("State is: ");
    Serial.println(server_state);
    nextTime = millis() + 1000;  
    
    //Check the response
    if(server_state>0)
    {
        digitalWrite(D7,HIGH);
        delay(500);
        digitalWrite(D7,LOW);
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

int Change_state(String command)
{
    Serial.println("-----------NEW STATE RECEIVED!!");
    hard_state=atoi(command.c_str());
    SD_write(hard_state);
    Serial.println(hard_state);
    return 1;
}
