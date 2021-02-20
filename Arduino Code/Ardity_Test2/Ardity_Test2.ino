

//Buttons Variables (with potentiometer on the left)
int leftButtonPin = 8;
int rightButtonPin = 9;

// Joystick Variables
const int SW_pin = 2; // digital pin connected to switch output
const int X_pin = 0; // analog pin connected to X output
const int Y_pin = 1; // analog pin connected to Y output


void setup() {
    // put your setup code here, to run once:
    //Joystick Setup
  pinMode(SW_pin, INPUT);
  digitalWrite(SW_pin, HIGH);
  
  pinMode(leftButtonPin, INPUT_PULLUP);
  pinMode(rightButtonPin, INPUT_PULLUP);
  
  Serial.begin(115200);
}
void loop()
{

   //Button Output
  if (digitalRead(rightButtonPin) == HIGH)
  {
    Serial.println("Right button pressed");
  }

  if (digitalRead(leftButtonPin) == HIGH)
  {
    Serial.println("Left button pressed");
  }

  /*
  int analogValue = analogRead(A4);
  Serial.print("Potentiometer Value: ");
  Serial.println(analogValue);

  Serial.print("Joystick ");
  Serial.print(analogRead(X_pin));
  Serial.print(" ");
  Serial.println(analogRead(Y_pin));
  */
}
