
// Joystick Variables
const int SW_pin = 2; // digital pin connected to switch output
const int X_pin = 0; // analog pin connected to X output
const int Y_pin = 1; // analog pin connected to Y output

//Buttons Variables (with potentiometer on the left)
int leftButtonPin = 8;
int rightButtonPin = 9;

void setup() {
  //Joystick Setup
  pinMode(SW_pin, INPUT);
  digitalWrite(SW_pin, HIGH);
  

  //Buttons Setup
  pinMode(leftButtonPin, INPUT_PULLUP);
  pinMode(rightButtonPin, INPUT_PULLUP);

  Serial.begin(9600);
}

void loop() {
  //Joystick Output
  /*
  Serial.print("Switch:  ");
  Serial.print(digitalRead(SW_pin));
  Serial.print("\n");
  Serial.print("X-axis: ");
  Serial.print(analogRead(X_pin));
  Serial.print("\n");
  Serial.print("Y-axis: ");
  Serial.println(analogRead(Y_pin));
  Serial.print("\n\n");
  delay(1000);
  */
  /*
  //Button Output
  if (digitalRead(rightButtonPin) == HIGH)
  {
    Serial.println("Right Button ON");
  }

  if (digitalRead(leftButtonPin) == HIGH)
  {
    Serial.println("Left Button ON");
  }
  */
  int analogValue = analogRead(A4);
  Serial.println(analogValue);

  
}
