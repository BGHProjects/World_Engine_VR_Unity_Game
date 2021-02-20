
//Buttons Variables (with potentiometer on the left)
int leftButtonPin = 8;
int rightButtonPin = 9;

int a = 1;
int b = 2;

void setup() {
  // put your setup code here, to run once:
  pinMode(leftButtonPin, INPUT_PULLUP);
  pinMode(rightButtonPin, INPUT_PULLUP);
  
  Serial.begin(9600);
}

void loop() {
  // put your main code here, to run repeatedly:

   //Button Output
  if (digitalRead(rightButtonPin) == HIGH)
  {
    Serial.println(a);
  }

  if (digitalRead(leftButtonPin) == HIGH)
  {
    Serial.println(b);
  }

}
