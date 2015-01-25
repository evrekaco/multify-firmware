TCPClient client;

byte server[] = { 160, 153, 72, 200 };
int count=0;
int index=0;

void setup()
{
  // Make sure your Serial Terminal app is closed before powering your Core
    Serial.begin(9600);
  // Now open your Serial Terminal, and hit any key to continue
    pinMode(A7,INPUT);
}

void loop()
{
    if(index==0)
    {   while(analogRead(A7)!=0) SPARK_WLAN_Loop(); }
  
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
    if(!client.connected())
    {
        Serial.println();
        Serial.println("disconnected.");
        client.flush();
    }   
   index++;
   delay(500);
}
