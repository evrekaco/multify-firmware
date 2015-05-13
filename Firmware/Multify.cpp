//  Multifu Firmware Code
//  Created and Developed by EVREKA LTD. STI.
//  24.02.2015

// This #include statement was automatically added by the Spark IDE.
#include "sd-card-library/sd-card-library.h"
#include "HttpClient/HttpClient.h"
#include "application.h"
#include "math.h"

SYSTEM_MODE(SEMI_AUTOMATIC);
int initial=0;

int one_turn=20;
int speed=1000;

long int hard_state_temp, hard_state=0;
int hard_state_int[6]={0,0,0,0,0,0};
char hard_state_read[6]={0,0,0,0,0,0};
int server_state=0;

int digit_turn[6]={0,0,0,0,0,0};
int turn=0;
int digit=0;

File myFile;
File state;

int i = 0;
int run = 1;
char name[64];
char pass[64];

// SOFTWARE SPI pin configuration - modify as required
const uint8_t chipSelect = A2;    // Also used for HARDWARE SPI setup
const uint8_t mosiPin = A5;
const uint8_t misoPin = A4;
const uint8_t clockPin = A3;

byte server[] = { 46, 101, 55, 98 }; //multify server ip address

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
String myID = Spark.deviceID();
/*
    MOTOR PIN CONFIGURATIONS
*/
int stp=0, e1=1, e2=2, e3=3, e4=4, e5=5, e6=6;

//'Fix Me' Function
int Change_state(String command);

//--------------------------------------------    SETUP BEGINS    --------------------------------------------
void setup() {
    //Motor pin assignment
    pinMode(stp,OUTPUT);
    pinMode(e1,OUTPUT);
    pinMode(e2,OUTPUT);
    pinMode(e3,OUTPUT);
    pinMode(e4,OUTPUT);
    pinMode(e5,OUTPUT);
    pinMode(e6,OUTPUT); 
    //Initially stop all motors
    Stop_motors();
    
    //LEDS for DEBUG
    pinMode(D7,OUTPUT);
    pinMode(A6,OUTPUT);
    digitalWrite(A6,HIGH);
    delay(300);
    digitalWrite(A6,LOW);
    //'Fix Me' Initialization
    Spark.function("Changer", Change_state);
    Serial.begin(9600);
    //SD card Initialization
    SD.begin(chipSelect);
    delay(100);
    if(initial==0){
        WiFi_sd();
    }
    digitalWrite(A6,HIGH);    
}
//--------------------------------------------    SETUP ENDS    --------------------------------------------

//--------------------------------------------    LOOP BEGINS    --------------------------------------------
void loop() {
    if(initial==1){
        Spark.connect();
        initial=2;
    }
    //First check hard_state
    SD_read();
    //Second check check-in count (sever_state)
    Server_Check();
    delay(100);
    
    //Wait for a succesfull http response
    while(server_state==0){
        digitalWrite(A6,LOW);
        Server_Check();
        digitalWrite(A6,HIGH);
        delay(100);
    }
    
    
    //After a successive http response compare the server and hard states
    while(hard_state==server_state)
    {
        digitalWrite(A6,LOW);
        Server_Check();
        digitalWrite(A6,HIGH);
        delay(100);
    }
    
    //Turn the motors after a check-in received
    Serial.println("----------TURN MOTORS!!");
    Equalizer(hard_state,server_state);
    //Update the SD Card
    SD_write(hard_state);
}
//--------------------------------------------    LOOP ENDS    --------------------------------------------

//--------------------------------------------    WIFI_SD BEGINS    --------------------------------------------
void WiFi_sd(){
    myFile = SD.open("internet.txt");
    
    i = 0;
    run = 1;
    while(run)
    {
        char ch = myFile.read();
        if(ch == ',')
        {
            run = 0;
            name[i] = '\0';
        }
        else
        {
            name[i] = ch;
        }
        i++;
    }
    
    i = 0;
    run = 1;
    while(run)
    {
        char ch = myFile.read();
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
    
    myFile.close();

    //digitalWrite(A0,HIGH);
    
    WiFi.on();
    digitalWrite(D7,HIGH);
    WiFi.clearCredentials();
    WiFi.setCredentials(name,pass);
    digitalWrite(D7,LOW);
    Spark.connect();
    
    //digitalWrite(A0,LOW);    
}
//--------------------------------------------    WIFI_SD ENDS    --------------------------------------------

//--------------------------------------------    SERVER_CHECK BEGINS    --------------------------------------------
//Checks the server for a check-in
void Server_Check(){
    if (nextTime > millis()) {
        return;
    }
    
//    Serial.println();
    Serial.println("Application>\tStart of HttpRequest.");
    // Request path and body can be set at runtime or at setup.
    request.hostname = "multify.co";
//    request.ip=server;
    request.port = 80;
//    request.path = "/adminMultify/webservice.php?method=foursquare&device_id="+myID+"&format=html";
    request.path = "/get_device_data_raw/"+myID+"/";
    

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
    nextTime = millis() + 100;  
    
    /*
    //Check the response
    if(server_state>0)
    {
        digitalWrite(D7,HIGH);
        delay(500);
        digitalWrite(D7,LOW);
    }
    */
        
    
    Serial.print("Hard_state_int: ");
    for(int i=0; i<6; i++){
        Serial.print(hard_state_int[i]);
        Serial.print("  ");
    }
    Serial.println(" ");
}
//--------------------------------------------    SERVER_CHECK ENDS    --------------------------------------------


//--------------------------------------------    SD_READ BEGINS    --------------------------------------------
//Gets the counter state from sdcard
void SD_read(){
    digit=0;
    //open state file
    state = SD.open("state.txt");
    
    digit=state.size();
    
    //read the file and keep it in state_read string
    for(int i=0;i<6;i++)
    {
      hard_state_read[i] = state.read();
    }
    
    //close the file
    state.close();
    //set the hard_state
    hard_state=atoi(hard_state_read);
    Serial.print("sd card değeri: ");
    Serial.println(hard_state);
    
    for(int i=0; i<(6-digit); i++){
        hard_state_int[i] = 0;
    }
    
    for(int i=0; i<digit; i++){
        hard_state_int[6-digit+i] = (hard_state_read[i] - '0');
    }    
}
//--------------------------------------------    SD_READ ENDS    --------------------------------------------


//--------------------------------------------    SD_WRITE BEGINS    --------------------------------------------
//Writes the counter state to sdcard
void SD_write(int cnt){
    //to overwrite the state file first remove it
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
    } 
    else {
        // if the file didn't open, print an error:
        Serial.println("error opening state.txt");
    }
}
//--------------------------------------------    SD_WRITE ENDS    --------------------------------------------


//--------------------------------------------    CHANGE_STATE BEGINS    --------------------------------------------
//Changes the hard_state when data received from 'Fix My Multify' part
int Change_state(String command)
{
    Serial.println("-----------NEW STATE RECEIVED!!");
    hard_state=atoi(command.c_str());
    SD_write(hard_state);
    Serial.println(hard_state);
    if(server_state>hard_state){
        hard_state_temp=hard_state;
        for(int i=5;i>=0;i--){
            hard_state_int[i]=hard_state_temp%10;
            hard_state_temp=hard_state_temp/10;
        }         
    }
    return 1;
}
//--------------------------------------------    CHANGE_STATE ENDS    --------------------------------------------


//--------------------------------------------    MOVE_MOTOR BEGINS    --------------------------------------------
//Selects a motor and moves it
void Move_motor(int motor, int stepnumber){
    switch(motor){
        case 1 :
            digitalWrite(e1,LOW);
            for(int j=0; j<stepnumber; j++){
                digitalWrite(stp,HIGH); //According to the motor number sets the relative pins to HIGH or LOW
                delayMicroseconds(speed); // Motor speed can be set via these lines
                digitalWrite(stp,LOW);
                delayMicroseconds(speed); // Motor speed can be set via these lines
            }
        break;
        case 2 :    
            digitalWrite(e2,LOW);
            for(int j=0; j<stepnumber; j++){
                digitalWrite(stp,HIGH);
                delayMicroseconds(speed);
                digitalWrite(stp,LOW);
                delayMicroseconds(speed);
            }
        break;
        case 3 :    
            digitalWrite(e3,LOW);
            for(int j=0; j<stepnumber; j++){
                digitalWrite(stp,HIGH);
                delayMicroseconds(speed);
                digitalWrite(stp,LOW);
                delayMicroseconds(speed);
            }
        break;
        case 4 :    
            digitalWrite(e4,LOW);
            for(int j=0; j<stepnumber; j++){
                digitalWrite(stp,HIGH);
                delayMicroseconds(speed);
                digitalWrite(stp,LOW);
                delayMicroseconds(speed);
            }
        break;
        case 5 :    
            digitalWrite(e5,LOW);
            for(int j=0; j<stepnumber; j++){
                digitalWrite(stp,HIGH);
                delayMicroseconds(speed);
                digitalWrite(stp,LOW);
                delayMicroseconds(speed);
            }
        break;
        case 6 :    
            digitalWrite(e6,LOW);
            for(int j=0; j<stepnumber; j++){
                digitalWrite(stp,HIGH);
                delayMicroseconds(speed);
                digitalWrite(stp,LOW);
                delayMicroseconds(speed);
            }
        break;
    }
}
//--------------------------------------------    MOVE_MOTOR ENDS    --------------------------------------------



//--------------------------------------------    STOP_MOTOR BEGINS    --------------------------------------------
//Stops selected motor
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
    //DEBUG
    Serial.print(motor);
    Serial.println("th Motor has been stopped!");
}
//--------------------------------------------    STOP_MOTOR ENDS    --------------------------------------------


//--------------------------------------------    STOP_MOTORS BEGINS    --------------------------------------------
//Stops All Motors
void Stop_motors(){
    digitalWrite(stp,LOW);
    digitalWrite(e1,HIGH);
    digitalWrite(e2,HIGH);
    digitalWrite(e3,HIGH);
    digitalWrite(e4,HIGH);
    digitalWrite(e5,HIGH);
    digitalWrite(e6,HIGH);
    Serial.println("ALL MOTORS HAVE BEEN STOPPED!");
}
//--------------------------------------------    STOP_MOTORS ENDS    --------------------------------------------


//--------------------------------------------    EQUALIZER BEGINS    --------------------------------------------
//  Evaluates and parses the difference of hard and server states
//  Turns the motors to equalize both states
void Equalizer(int hrd_st, int srv_st){
    
    Move_all();
    //Find the difference
    int difference = srv_st - hrd_st;
    //Parse the difference into digit_turn array
    for(int i=5;i>=0;i--){
        digit_turn[i]=difference%10;
        difference=difference/10;
    }
    
    //DEBUG
    Serial.print("Digit Turns:");
    for(int a=0;a<6;a++){
        Serial.print(digit_turn[a]);
        Serial.print("   ");
    }
    Serial.println("");
    
    //turn each motor for the amount of corresponding digit_turn value
    for(int j=1; j<7; j++){
        turn=digit_turn[6-j];

        //POSITIVE TURN
        if(turn>0){
            //DEBUG
            Serial.print(j);
            Serial.print("th Motor will turn by ");
            Serial.println(turn);
            
            //Move_motor(j,10*one_turn);
            
            //CLASSICAL TURN
            for(turn;turn>0;turn--){
                Move_motor(j,one_turn);
                Stop_motor(j);
            }
            //Increase hard_state according to the motor index
            //For ex: for the 3^th motor(j=2) increase hard_state by 100=(10^2) for each turn
            hard_state=hard_state + digit_turn[6-j]*pow(10,j-1);
            
            //EXTRA TURN
            if(digit_turn[6-j] + hard_state_int[6-j] > 9){
                Serial.println("Ekstra move for next motor!");
                Move_motor(j+1,one_turn);
                Stop_motor(j+1);

                //EXTRA LOOK FOR EXTRA TURN
                for(int k=0; k<digit-j; k++){
                    if(hard_state_int[5-j-k]==9){
                        Move_motor(j+k+2,one_turn);
                        Stop_motor(j+k+2);
                    }
                    else
                        break;                    
                }
            }
        }

        //NEGATIVE TURN
        else if(turn<0){
            Serial.println("NEGATIVE TURN!!");
            turn = turn + 10;

            Serial.print(j);
            Serial.print("th Motor will turn by ");
            Serial.println(turn);

            //CLASSICAL TURN
            for(turn;turn>0;turn--){
                Move_motor(j,one_turn);
                Stop_motor(j);
            }
            hard_state = hard_state + digit_turn[6-j]*pow(10,j-1);

            //EXTRA TURN
            if(hard_state_int[6-j] - digit_turn[6-j] > 9){
                Move_motor(j+1,9*one_turn);
                Stop_motor(j+1);

                //EXTRA LOOK FOR EXTRA TURN
                for(int k=0; k<digit-j; k++){
                    if(hard_state_int[5-j-k] - digit_turn[5-j-k]==9){
                        Move_motor(j+2+k,9*one_turn);
                        Stop_motor(j+2+k);
                    }
                    else
                        break;
                }
            }
        }
    }
}
//--------------------------------------------    EQUALIZER ENDS    --------------------------------------------

//--------------------------------------------    MOVEALL BEGINS    --------------------------------------------
void Move_all(){
    for(int i=6; i>0; i--){
        Move_motor(i,10*one_turn);
        Stop_motor(i);
    }
    Stop_motors();
}
//--------------------------------------------    MOVEALL ENDS    --------------------------------------------

//--------------------------------------------    MULTIFY ENDS    --------------------------------------------