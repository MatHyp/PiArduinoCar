#include <Arduino.h>

void setup()
{

  pinMode(8, OUTPUT);
  digitalWrite(8, LOW);

  pinMode(9, OUTPUT);
  digitalWrite(9, LOW);
  Serial.begin(9600);
  Serial.println("READY"); // handshake message
}

void loop()
{
  if (Serial.available() > 0)
  {
    char c = Serial.read(); // Read one byte
    Serial.println(c);      // Echo back to Python

    // Blink LED only if the byte is 'D'
    switch (c)
    {
    case 'D':
      digitalWrite(8, HIGH);
      delay(100);
      digitalWrite(8, LOW);
      break;

    case 'A':
      digitalWrite(9, HIGH);
      delay(100);
      digitalWrite(9, LOW);
      break;
    }
  }
}
