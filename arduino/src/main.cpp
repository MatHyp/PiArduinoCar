#include <Arduino.h>

void setup()
{
    pinMode(LED_BUILTIN, OUTPUT); // Ustawienie pinu LED jako wyjście
}

void loop()
{
    digitalWrite(LED_BUILTIN, HIGH); // Włącz diodę
    delay(1000);                     // Czekaj 1 sekundę
    digitalWrite(LED_BUILTIN, LOW);  // Wyłącz diodę
    delay(1000);                     // Czekaj 1 sekundę
}
