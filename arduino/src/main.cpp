#include <Arduino.h>

void setup() {
  Serial.begin(9600); // start komunikacji szeregowej
}

void loop() {
  if (Serial.available() > 0) {
    char c = Serial.read();  
    Serial.print("Odebrano: ");
    Serial.println(c);
  }
}
