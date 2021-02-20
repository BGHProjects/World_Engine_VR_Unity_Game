using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreativeListener : MonoBehaviour
{

	public GameObject CreativePlanet;

	public float cloudRed;
	public float cloudGreen;
	public float cloudBlue;
	public float noCloud = 0.0f;
	float red = 1.0f;
	float green = 1.0f;
	float blue = 1.0f;

	float pred = 1.0f;
	float pgreen = 1.0f;
	float pblue = 1.0f;

	public Color planetColour;
	public Color cloudColour;

	Boolean cloudsEnabled = false;

	public Material[] starters;

	int index = 0;

	public SteamVR_TrackedObject rightTrackedObj;
	public SteamVR_TrackedObject leftTrackedObj;

	float maxThresh = 0.8f;
	float minThresh = 0.2f;
	bool maxPoint = false;
	bool minPoint = false;

	float rightThresh = -0.7f;
	float leftThresh = 0.4f;
	bool RrightPoint = false;
	bool RleftPoint = false;
	bool LrightPoint = false;
	bool LleftPoint = false;

	int[] colours = { 1, 2, 3 };
	int whichColour;
	int colourIndex = 0;

	public Text colourText;
	public Text cloudText;

	public GameObject fader;
	public Animator animator;
	private Boolean input = false;
	private string levelToLoad;

	// Start is called before the first frame update
	void Start()
    {

	}

    // Update is called once per frame
    void Update()
    {
		if (input == true)
		{
			StartCoroutine(Fading());
		}

		UIUpdate();

		PlanetControls();
		PlanetColour();
		CloudControls();

		//Planet Rotation
		CreativePlanet.transform.Rotate(Vector3.up * Time.deltaTime * 5.0f);

		//STYLES FOR THE PLAYER PLANET
		//Style of the Planet
		MeshRenderer mesh = CreativePlanet.GetComponent<MeshRenderer>();
		mesh.material = starters[index];

		//Colour of the Planet
		planetColour = new Color(pred, pgreen, pblue);
		CreativePlanet.GetComponent<Renderer>().sharedMaterial.SetColor("_TextureColor", planetColour);

		//Clouds of the Planet
		cloudColour = new Color(cloudRed, cloudGreen, cloudBlue);
		CreativePlanet.GetComponent<Renderer>().sharedMaterial.SetColor("_CloudColor", cloudColour);

		ReturnToMainMenu();

	}

	IEnumerator Fading()
	{
		yield return new WaitForSeconds(1);
		SceneManager.LoadScene(levelToLoad);
	}

	public void FadeToLevel(string levelName)
	{
		levelToLoad = levelName;
		animator.SetTrigger("FadeOut");
		input = true;
	}

	void ReturnToMainMenu()
	{
		SteamVR_Controller.Device rightDevice = SteamVR_Controller.Input((int)rightTrackedObj.index);
		SteamVR_Controller.Device leftDevice = SteamVR_Controller.Input((int)leftTrackedObj.index);

		if (rightDevice.GetTouchDown(SteamVR_Controller.ButtonMask.Grip) && leftDevice.GetTouchDown(SteamVR_Controller.ButtonMask.Grip))
		{
			FadeToLevel("Main_Menu");
		}
	}

	void UIUpdate()
    {
		if (cloudsEnabled)
		{
			cloudText.text = "Clouds Enabled";

			if (colours[colourIndex] == 1)
            {
				colourText.text = "Red - " + cloudRed.ToString();
            }
			else if (colours[colourIndex] == 2)
			{
				colourText.text = "Green - " + cloudGreen.ToString();
			}
			else if (colours[colourIndex] == 3)
			{
				colourText.text = "Blue - " + cloudBlue.ToString();
			}
		}
		else
        {
			cloudText.text = "";

			if (colours[colourIndex] == 1)
			{
				colourText.text = "Red - " + pred.ToString();
			}
			else if (colours[colourIndex] == 2)
			{
				colourText.text = "Green - " + pgreen.ToString();
			}
			else if (colours[colourIndex] == 3)
			{
				colourText.text = "Blue - " + pblue.ToString();
			}
		}
    }

	void PlanetColour()
    {
		SteamVR_Controller.Device rightDevice = SteamVR_Controller.Input((int)rightTrackedObj.index);
		SteamVR_Controller.Device leftDevice = SteamVR_Controller.Input((int)leftTrackedObj.index);

		if(!cloudsEnabled)
        {

			//Left Hand Increases the Colour Value
			if (leftDevice.GetTouch(SteamVR_Controller.ButtonMask.Trigger))
            {

				//Red
				if (colours[colourIndex] == 1)
                {

					if (pred < 1.0f)
					{
						pred += 0.005f;
					}
				}
				//Green
				else if(colours[colourIndex] == 2)
                {

					if (pgreen < 1.0f)
					{
						pgreen += 0.005f;
					}
				}
				//Blue
				else if(colours[colourIndex] == 3)
                {

					if (pblue < 1.0f)
					{
						pblue += 0.005f;
					}
				}
            }
			//Right Hand Decreases the Colour Value
			else if (rightDevice.GetTouch(SteamVR_Controller.ButtonMask.Trigger))
            {

				//Red
				if (colours[colourIndex] == 1)
				{
					if (pred > 0.0f)
					{
						pred -= 0.005f;
					}
				}
				//Green
				else if (colours[colourIndex] == 2)
				{
					if (pgreen > 0.0f)
					{
						pgreen -= 0.005f;
					}
				}
				//Blue
				else if (colours[colourIndex] == 3)
				{
					if (pblue > 0.0f)
					{
						pblue -= 0.005f;
					}
				}
			}
        }
		//Change the Colour Values of the Clouds
		else
        {
			cloudRed = red;
			cloudGreen = green;
			cloudBlue = blue;

			//Left Hand Increases the Colour Value
			if (leftDevice.GetTouch(SteamVR_Controller.ButtonMask.Trigger))
			{
				//Red
				if (colours[colourIndex] == 1)
				{
					if (red < 1.0f)
					{
						red += 0.005f;
					}
				}
				//Green
				else if (colours[colourIndex] == 2)
				{
					if (green < 1.0f)
					{
						green += 0.005f;
					}
				}
				//Blue
				else if (colours[colourIndex] == 3)
				{
					if (blue < 1.0f)
					{
						blue += 0.005f;
					}
				}
			}
			//Right Hand Decreases the Colour Value
			else if (rightDevice.GetTouch(SteamVR_Controller.ButtonMask.Trigger))
			{
				//Red
				if (colours[colourIndex] == 1)
				{
					if (red > 0.0f)
					{
						red -= 0.005f;
					}
				}
				//Green
				else if (colours[colourIndex] == 2)
				{
					if (green > 0.0f)
					{
						green -= 0.005f;
					}
				}
				//Blue
				else if (colours[colourIndex] == 3)
				{
					if (blue > 0.0f)
					{
						blue -= 0.005f;
					}
				}
			}
		}

	}

	void CloudControls()
    {
		if (!cloudsEnabled)
        {
			cloudRed = noCloud;
			cloudGreen = noCloud;
			cloudBlue = noCloud;

		}
		
		//Pressing Motion to Toggle the Clouds
		float dist = Vector3.Distance(rightTrackedObj.transform.position, leftTrackedObj.transform.position);

		if (dist > maxThresh)
		{
			maxPoint = true;
			return;
		}

		if (maxPoint)
		{
			if (dist < minThresh)
			{
				minPoint = true;

				if (maxPoint && minPoint)
				{
					if (!cloudsEnabled)
					{
						cloudsEnabled = true;
						UnityEngine.Debug.Log("Clouds are enabled");
					}
					else
					{
						cloudsEnabled = false;
						UnityEngine.Debug.Log("Clouds are disabled");
					}

					maxPoint = false;
					minPoint = false;
					return;
				}
			}
		}
	}

	void PlanetControls()
    {
		//Twisting Motions are used to switch between planet presets
		//Grip buttons are for cycling through red, green and blue

		//Left Twist
		if (leftTrackedObj.transform.rotation[2] >= leftThresh)
		{
			LleftPoint = true;
		}
		
		if (LleftPoint)
		{
			if (leftTrackedObj.transform.rotation[2] <= rightThresh)
			{
				LrightPoint = true;

				if (LrightPoint && LleftPoint)
				{
					if (index == starters.Length - 1)
					{
						index = 0;
					}
					else
					{
						index++;
					}

					LrightPoint = false;
					LleftPoint = false;
					return;
				}
			}
		}

		//Right Twist
		if (rightTrackedObj.transform.rotation[2] <= rightThresh)
		{
			RrightPoint = true;
		}

		if (RrightPoint)
		{
			if (rightTrackedObj.transform.rotation[2] >= leftThresh)
			{
				RleftPoint = true;

				if (RrightPoint && RleftPoint)
				{
					if (index == 0)
					{
						index = starters.Length - 1;
					}
					else
					{
						index--;
					}

					RrightPoint = false;
					RleftPoint = false;
					return;
				}
			}
		}

		//Cycling through the colours
		SteamVR_Controller.Device rightDevice = SteamVR_Controller.Input((int)rightTrackedObj.index);
		SteamVR_Controller.Device leftDevice = SteamVR_Controller.Input((int)leftTrackedObj.index);

		//Grip Buttons
		if (leftDevice.GetTouchDown(SteamVR_Controller.ButtonMask.Grip))
		{
			if (colourIndex == colours.Length - 1)
			{
				colourIndex = 0;
			}
			else
			{
				colourIndex++;
			}
		}

		if (rightDevice.GetTouchDown(SteamVR_Controller.ButtonMask.Grip))
		{
			if (colourIndex == 0)
			{
				colourIndex = colours.Length - 1;
			}
			else
			{
				colourIndex--;
			}
		}



	}

	void OnMessageArrived(string msg)
	{
		UnityEngine.Debug.Log("Arrived: " + msg);
	}


	// Invoked when a connect/disconnect event occurs. The parameter 'success'
	// will be 'true' upon connection, and 'false' upon disconnection or
	// failure to connect.
	void OnConnectionEvent(bool success)
	{
		UnityEngine.Debug.Log(success ? "Device connected" : "Device disconnected");
	}
}
