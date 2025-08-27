#include <Arduino.h>

void setup()
{
  Serial.begin(9600);
  Serial.println("READY"); // handshake message
}

void loop()
{
  if (Serial.available() > 0)
  {
    char c = Serial.read();
    Serial.print("Odebrano: ");
    Serial.println(c);
  }
}
