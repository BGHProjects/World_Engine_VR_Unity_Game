using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerraRotator : MonoBehaviour
{
	float timeCounter;
	float xVal;
	float zVal;

	System.Random speed = new System.Random();

	// Start is called before the first frame update
	void Start()
	{
		 xVal = transform.position.x;
		 zVal = transform.position.z;

		if(zVal == 0.0f)
        {
			zVal++;
        }

		if (xVal == 0.0f)
		{
			xVal++;
		}
	}

	// Update is called once per frame
	void Update()
	{
		timeCounter += Time.deltaTime * (float)(speed.NextDouble() + 1);
		//UnityEngine.Debug.Log(timeCounter);

		transform.position = new Vector3((float)Mathf.Sin(timeCounter) * xVal, transform.position.y, (float)Mathf.Cos(timeCounter) * zVal);
		//(float)Mathf.Sin(timeCounter) *
		//(float)Mathf.Cos(timeCounter) *
	}
}
