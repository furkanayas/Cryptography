#include <SoftwareSerial.h>
#include <AESLib.h>
#include <Base64.h>

SoftwareSerial MasterNode(2,3); //Rx Tx

void aes128_dec_single(const uint8_t* key, void* data);

uint8_t key[] = {0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15};

//char data[] = "QQNZfVznTvc/80vaK7oouA==";
char data[40];
String datacontrol = "nil";
bool connectionstatus = false;
String respond = "";

int nonconnectled = 8;
int connectled = 9;

bool au = false;

int o = 1;

void setup(){
  Serial.begin(9600);
  MasterNode.begin(9600);
  pinMode(nonconnectled,OUTPUT);
  pinMode(connectled,OUTPUT);
}

void loop() {
  if(au == false) { digitalWrite(nonconnectled,HIGH); }
  if(connectionstatus == true){ ReadData(); }
  else if(connectionstatus == false){
    Serial.println("Waiting for connection...");
    startConnectionProtocol(); }
  respond = "";

  delay(2000);
}

void startConnectionProtocol(){
  while(connectionstatus == false){
    readSerialPort();     
    if(respond == "mr"){       
      MasterNode.print("sr");
      delay(200);
      connectionstatus = true;      
      }
    respond = "";
    delay(2000);
    }
  }
void readSerialPort()
{
  while(MasterNode.available()){   
  delay(10);
  if(MasterNode.available() > 0){
    char c = MasterNode.read();
    respond = respond + c;
    } 
  }
  MasterNode.flush();
}

void ReadData(){
  readSerialPort();      
  respond.toCharArray(data, sizeof(data));
  int inputLen = strlen(data);  
  int encodedLen = base64_enc_len(inputLen);
  char encoded[encodedLen];  

  base64_decode(encoded, data, inputLen);  
  aes128_dec_single(key,encoded);  

  //Serial.println("++++++");
//Serial.println(encoded);
 //Serial.println(datacontrol);
  if(String(encoded) == datacontrol){

    if (o == true){Serial.println("Leaf Node Authenticate Succesfully..!"); o = 0;}

    au = true;
     digitalWrite(nonconnectled,LOW);
      digitalWrite(connectled,HIGH);  
    Serial.println(encoded);
    }
   datacontrol = encoded;

   // memcpy(encodedcontrol, encoded, encodedLen);
   //   ((strcpy(encodedcontrol, encoded);
   //  encoded.toCharArray(encodedcontrol, encodedLen);
 }




