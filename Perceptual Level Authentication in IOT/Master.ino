#include <SoftwareSerial.h>
#include <AESLib.h>
#include <Base64.h>

SoftwareSerial LeafNode(2,3); //Rx Tx

void aes128_enc_single(const uint8_t* key, void* data);

uint8_t key[] = {0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15};

char data[] = "furkanayaspsswrd";
char encoded[64];
bool connectionstatus = false;
String respond = "";

/*
data :    furkanayaspsswrd
aes128 :  AY}\⸮N⸮?⸮K⸮+⸮(⸮
radix64 : QQNZfVznTvc/80vaK7oouA==   
*/

int o = 1;

void setup(){
  

  Serial.begin(9600);
  LeafNode.begin(9600); 

  Serial.print("Data : ");Serial.println(data); 
  
  Encryption();
  Serial.println("\nData Encrypted..!");
  Serial.print("Data : ");Serial.println(data);
  Encode();
  Serial.println("\nData Encoded..!");
  Serial.print("Data : ");Serial.println(encoded);
//while(!Serial.available()){;}
}


void loop(){
  if(connectionstatus == true){ SendData(); }
  else if(connectionstatus == false){ startConnectionProtocol(); }
  respond = "";
  delay(2000);  
}

void startConnectionProtocol() {  
  Serial.println("\nConnecting...");
   
  while(connectionstatus == false) {
    LeafNode.print("mr");
    
    while (LeafNode.available()) {
      delay(10);
      if (LeafNode.available() >0) {
        char c = LeafNode.read(); //byte
        respond += c;
      }
    }
          
      if(respond == "sr"){ connectionstatus = true; }
      respond = "";
      delay(2000);
  }
}

void readSerialPort(){
  while (Serial.available()){
  delay(10);  
  if (Serial.available() >0){
    char c = Serial.read();  //byte
    respond += c; 
    }
  }
  Serial.flush();
}

void Encryption(){ aes128_enc_single(key, data); }

void Encode(){
  int inputLen = strlen(data);  
  int encodedLen = base64_enc_len(inputLen);
  encoded[encodedLen];  
  base64_encode(encoded, data, inputLen);
  }

void SendData(){ 
  if(o == true){ Serial.println("Sending...");  o = 0;} 

  Serial.println(encoded);
  
  LeafNode.print(encoded); delay(100); 
 // if(LeafNode.available() > 0) {LeafNode.print(encoded); delay(100); }
  }
  
