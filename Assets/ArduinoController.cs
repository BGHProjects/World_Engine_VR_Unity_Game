using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class ArduinoController : MonoBehaviour
{
	private float AmountToMove;
	SerialPort serial = new SerialPort("COM5", 9600);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
	if(!serial.IsOpen)
	{
		serial.Open();
	}

	int data = int.Parse(serial.ReadLine());
	AmountToMove = 2;
	MoveObject(data);
        
    }

    void MoveObject(int direction)
	{
		if(direction == 1)
		{
			transform.Translate(Vector3.left*AmountToMove, Space.World);
		}

		if(direction == 2)
		{
			transform.Translate(Vector3.right*AmountToMove, Space.World);
		}
	}


}
