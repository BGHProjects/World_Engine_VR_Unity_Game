using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{

	public float timeCounter;
	public float speed;
	public float width;
	public float breadth;
	public float height;
	
    // Start is called before the first frame update
    void Start()
    {
	//timeCounter = 0;
	//speed = 1.1f;
	//width = 8;
	//breadth = 8;
        
    }

    // Update is called once per frame
    void Update()
    {
	timeCounter += Time.deltaTime*speed;

	float x = Mathf.Sin(timeCounter)*width;
	float y = height;
	float z = Mathf.Cos(timeCounter)*breadth;

	transform.position = new Vector3(x,y,z);
        
    }
}
