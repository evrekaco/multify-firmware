// This #include statement was automatically added by the Spark IDE.
#include "HttpClient/HttpClient.h"
#include "sd-card-library/sd-card-library.h"
#include "application.h"
#include "math.h"


long int hard_state=0;
int server_state=0;
int one_turn=10;
char hard_state_read[6];
int digit_turn[6]={0,0,0,0,0,0};

File ssid;
File state;

//SD card pin configurations
const uint8_t chipSelect = A2;    // Also used for HARDWARE SPI setup
const uint8_t mosiPin = A5;
const uint8_t misoPin = A4;
const uint8_t clockPin = A3;


//multify.co ip address
byte server[] = { 160, 153, 72, 200 };


/**
* Declaring the HTTP Request variables.
*/
unsigned int nextTime = 0;    // Next time to contact the server
HttpClient http;
// Headers currently need to be set at init, useful for API keys etc.
http_header_t headers[] = {
    { "Accept" , "*/*"},
    { NULL, NULL } // NOTE: Always terminate headers will NULL
};
http_request_t request;
http_response_t response;


/*
    MOTOR PIN CONFIGURATIONS
*/
int stp=0, e1=1, e2=2, e3=3, e4=4, e5=5, e6=6;

//Changer function definiton
int Change_state(String command);



//--------------------------------------------    SETUP BEGINS    --------------------------------------------
void setup() {
    //Changer function definition
    Spark.function("Changer", Change_state);
    
    Serial.begin(9600);
    
    //SD card initialization
    SD.begin(chipSelect);
    
    //Extra button
    pinMode(A7,INPUT);
    
    //Motor pin assignment
    pinMode(stp,OUTPUT);
    pinMode(e1,OUTPUT);
    pinMode(e2,OUTPUT);
    pinMode(e3,OUTPUT);
    pinMode(e4,OUTPUT);
    pinMode(e5,OUTPUT);
    pinMode(e6,OUTPUT);
    
    //led pin assignments
    pinMode(A6,OUTPUT);
    pinMode(D7,OUTPUT);
    digitalWrite(A6,HIGH);
    
    Stop_motors();
}
//--------------------------------------------    SETUP ENDS    --------------------------------------------


//--------------------------------------------    LOOP BEGINS    --------------------------------------------
void loop() {
    SD_read();
    while(hard_state==server_state)
    {
        digitalWrite(A6,LOW);
        Server_Check();
        digitalWrite(A6,HIGH);
        delay(500);
    }
    
    Serial.println("----------TURN MOTORS!!");
    Equalizer(hard_state,server_state);
    SD_write(hard_state);
}
//--------------------------------------------    LOOP ENDS    --------------------------------------------




//--------------------------------------------    SERVER_CHECK BEGINS    --------------------------------------------
void Server_Check(){
    if (nextTime > millis()) {
        return;
    }
    
//    Serial.println();
    Serial.println("Application>\tStart of HttpRequest.");
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
//    Serial.println(response.status);

//    Serial.print("Application>\tHTTP Response Body: ");
//    Serial.println(response.body);
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
//--------------------------------------------    SERVER_CHECK ENDS    --------------------------------------------


//--------------------------------------------    SD_READ BEGINS    --------------------------------------------
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
//--------------------------------------------    SD_READ ENDS    --------------------------------------------


//--------------------------------------------    SD_WRITE BEGINS    --------------------------------------------
void SD_write(int cnt){
        SD.remove("state.txt");
        state = SD.open("state.txt", FILE_WRITE);
      
      // if the file opened okay, write to it:
      if (state) {
        Serial.print("Writing to state.txt:  ");
        Serial.println(cnt);
        state.print(cnt);
      // close the file:
        state.close();
        Serial.println("done.");
      } else {
        // if the file didn't open, print an error:
        Serial.println("error opening state.txt");
      }
}
//--------------------------------------------    SD_WRITE ENDS    --------------------------------------------


//--------------------------------------------    CHANGE_STATE BEGINS    --------------------------------------------
int Change_state(String command)
{
    Serial.println("-----------NEW STATE RECEIVED!!");
    hard_state=atoi(command.c_str());
    SD_write(hard_state);
    Serial.println(hard_state);
    return 1;
}
//--------------------------------------------    CHANGE_STATE ENDS    --------------------------------------------


//--------------------------------------------    MOVE_MOTOR BEGINS    --------------------------------------------
void Move_motor(int motor, int stepnumber){
    switch(motor){
        case 1 :
            digitalWrite(e1,LOW);
            for(int j=0; j<stepnumber; j++){
                digitalWrite(stp,HIGH);
                delayMicroseconds(1000);
                digitalWrite(stp,LOW);
                delayMicroseconds(1000);
            }
        break;
        case 2 :    
            digitalWrite(e2,LOW);
            for(int j=0; j<stepnumber; j++){
                digitalWrite(stp,HIGH);
                delayMicroseconds(1000);
                digitalWrite(stp,LOW);
                delayMicroseconds(1000);
            }
        break;
        case 3 :    
            digitalWrite(e3,LOW);
            for(int j=0; j<stepnumber; j++){
                digitalWrite(stp,HIGH);
                delayMicroseconds(1000);
                digitalWrite(stp,LOW);
                delayMicroseconds(1000);
            }
        break;
        case 4 :    
            digitalWrite(e4,LOW);
            for(int j=0; j<stepnumber; j++){
                digitalWrite(stp,HIGH);
                delayMicroseconds(1000);
                digitalWrite(stp,LOW);
                delayMicroseconds(1000);
            }
        break;
        case 5 :    
            digitalWrite(e5,LOW);
            for(int j=0; j<stepnumber; j++){
                digitalWrite(stp,HIGH);
                delayMicroseconds(1000);
                digitalWrite(stp,LOW);
                delayMicroseconds(1000);
            }
        break;
        case 6 :    
            digitalWrite(e6,LOW);
            for(int j=0; j<stepnumber; j++){
                digitalWrite(stp,HIGH);
                delayMicroseconds(1000);
                digitalWrite(stp,LOW);
                delayMicroseconds(1000);
            }
        break;
    }
}
//--------------------------------------------    MOVE_MOTOR ENDS    --------------------------------------------



//--------------------------------------------    STOP_MOTOR BEGINS    --------------------------------------------
void Stop_motor(int motor){
    digitalWrite(stp,LOW);
    switch(motor){
        case 1 :
            digitalWrite(e1,HIGH);
        break;
        case 2 :
            digitalWrite(e2,HIGH);
        break;
        case 3 :
            digitalWrite(e3,HIGH);
        break;
        case 4 :
            digitalWrite(e4,HIGH);
        break;
        case 5 :
            digitalWrite(e5,HIGH);
        break;
        case 6 :
            digitalWrite(e6,HIGH);
        break;
    }
}
//--------------------------------------------    STOP_MOTOR ENDS    --------------------------------------------



//--------------------------------------------    STOP_MOTORS BEGINS    --------------------------------------------
void Stop_motors(){
    digitalWrite(stp,LOW);
    digitalWrite(e1,HIGH);
    digitalWrite(e2,HIGH);
    digitalWrite(e3,HIGH);
    digitalWrite(e4,HIGH);
    digitalWrite(e5,HIGH);
    digitalWrite(e6,HIGH);
}
//--------------------------------------------    STOP_MOTORS ENDS    --------------------------------------------


void Equalizer(int hrd_st, int srv_st){
    //Find the difference
    int difference = srv_st - hrd_st;
    //Parse the difference into digit_turn array
    for(int i=0;i<6;i++){
        digit_turn[i]=difference%10;
        difference=difference/10;
    }
    
    //turn each motor for the amount of corresponding digit_turn value
    for(int j=0; j<6; j++){
        int turn=digit_turn[j];
        for(turn; turn>0; turn--){
            //One turn for motor j^th
            Move_motor(j,one_turn);
            Stop_motor(j);
            //Increase hard_state according to the motor index
            //For ex: for the 3^th motor(j=2) increase hard_state by 100=(10^2) for each turn
            hard_state=hard_state+pow(10,j);
        }
    }
    //Set the digit_turn array to 0 after all motors has turned
    for(int k=0;k<6;k++){
        digit_turn[k]=0;
    }
}
