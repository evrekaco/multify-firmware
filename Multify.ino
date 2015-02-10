// This #include statement was automatically added by the Spark IDE.
#include "sd-card-library/sd-card-library.h"
#include "HttpClient/HttpClient.h"
#include "application.h"
#include "math.h"

SYSTEM_MODE(SEMI_AUTOMATIC);
int initial=1;

int one_turn=20;
int speed=1000;

long int hard_state=0;
int hard_state_int[6]={0,0,0,0,0,0};
char hard_state_read[6]={0,0,0,0,0,0};
int server_state=0;

int digit_turn[6]={0,0,0,0,0,0};
int turn=0;
int digit=0;

File ssid;
File state;

// SOFTWARE SPI pin configuration - modify as required
const uint8_t chipSelect = A2;    // Also used for HARDWARE SPI setup
const uint8_t mosiPin = A5;
const uint8_t misoPin = A4;
const uint8_t clockPin = A3;

byte server[] = { 160, 153, 72, 200 }; //multify server ip address

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
    digitalWrite(D7,HIGH);
    
    //'Fix Me' Initialization
    Spark.function("Changer", Change_state);
    Serial.begin(9600);
    //SD card Initialization
    SD.begin(chipSelect);
}
//--------------------------------------------    SETUP ENDS    --------------------------------------------

//--------------------------------------------    LOOP BEGINS    --------------------------------------------
void loop() {
    if(initial==1){
        Spark.connect();
        initial=0;
    }
    //First check hard_state
    SD_read();
    //Second check check-in count (sever_state)
    Server_Check();
    delay(100);
    
    //Wait for a succesfull http response
    while(server_state==0){
        digitalWrite(D7,LOW);
        Server_Check();
        digitalWrite(D7,HIGH);
        delay(100);
    }
    
    
    //After a successive http response compare the server and hard states
    while(hard_state==server_state)
    {
        digitalWrite(D7,LOW);
        Server_Check();
        digitalWrite(D7,HIGH);
        delay(100);
    }
    
    //Turn the motors after a check-in received
    Serial.println("----------TURN MOTORS!!");
    Equalizer(hard_state,server_state);
    //Update the SD Card
    SD_write(hard_state);
}
//--------------------------------------------    LOOP ENDS    --------------------------------------------

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
    request.ip=server;
    request.port = 80;
    request.path = "/adminMultify/webservice.php?method=foursquare&device_id=54ff74066678574917170667&format=html"; //Spark No: 2

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
            
            Move_motor(j,one_turn*10);
            
            //CLASSICAL TURN
            Move_motor(j,turn*one_turn);
            Stop_motor(j);
            //Increase hard_state according to the motor index
            //For ex: for the 3^th motor(j=2) increase hard_state by 100=(10^2) for each turn
            hard_state=hard_state + turn*pow(10,j-1);
            
            //EXTRA TURN
            if(digit_turn[6-j] + hard_state_int[6-j] > 9){
                Serial.println("Ekstra move for next motor!");
                Move_motor(j+1,one_turn*11);
                Stop_motor(j+1);

                //EXTRA LOOK FOR EXTRA TURN
                for(int k=0; k<digit-j; k++){
                    if(hard_state_int[5-j-k]==9){
                        Move_motor(j+k+2,one_turn);
                        Stop_motor(j+k+2);
                    }
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
            Move_motor(j,turn*one_turn);
            Stop_motor(j);
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

//--------------------------------------------    MULTIFY ENDS    --------------------------------------------
