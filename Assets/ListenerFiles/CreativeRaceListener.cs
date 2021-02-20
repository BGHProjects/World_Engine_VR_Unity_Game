using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.SceneManagement;

public class CreativeRaceListener : MonoBehaviour
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

	public GameObject planetToCopy;

	public float planetRed;
	public float planetBlue;
	public float planetGreen;

	public float copyCloudRed;
	public float copyCloudBlue;
	public float copyCloudGreen;

	public Color copyPlanetColour;
	public Color copyCloudColour;

	Boolean areThereClouds = false;

	System.Random copyPlanetColourValue = new System.Random();
	System.Random whichStarter = new System.Random();
	System.Random cloudPicker = new System.Random();

	public Material[] starters;
	public Material[] copyStarters;

	int index = 0;
	int copyIndex = 0;

	public Text Timer;
	Boolean gameFinished = false;
	Boolean gameOver = false;
	public float timeToDisplay = 5;
	Boolean timerIsRunning = true;

	public Text PlanetCounter;
	int planetCounter = 0;

	Boolean matched = false;

	Boolean redFound = false;
	Boolean blueFound = false;
	Boolean greenFound = false;
	Boolean cloudRedFound = false;
	Boolean cloudGreenFound = false;
	Boolean cloudBlueFound = false;
	Boolean planetFound = false;

	public List<Material> completedPlanets = new List<Material>();
	public List<float> coloursFromPlanet = new List<float>();
	public List<List<float>> completedColours = new List<List<float>>();

	public GameObject completedPlanet;
	System.Random planetLocation = new System.Random();
	Boolean gameDone = false;

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

	float valueChange = 0.005f;

	public GameObject fader;
	public Animator animator;
	private Boolean input = false;
	private string levelToLoad;


	// Start is called before the first frame update
	void Start()
    {
		SetPlanet();
	}

    // Update is called once per frame
    void Update()
    {
		if (input == true)
		{
			StartCoroutine(Fading());
		}

		PlanetControls();
		PlanetColour();
		CloudControls();
		UIUpdate();

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


		//STYLES FOR THE PLANET TO COPY
		//Copy Planet Colour
		copyPlanetColour = new Color(planetRed, planetGreen, planetBlue);
		planetToCopy.GetComponent<Renderer>().sharedMaterial.SetColor("_TextureColor", copyPlanetColour);

		//Copy Planet Clouds
		copyCloudColour = new Color(copyCloudRed, copyCloudGreen, copyCloudBlue);
		planetToCopy.GetComponent<Renderer>().sharedMaterial.SetColor("_CloudColor", copyCloudColour);

		//Miscellaneous UI Details
		if (timerIsRunning)
		{
			timeToDisplay -= Time.deltaTime;
			TimeRemaining(timeToDisplay);
		}

		PlanetCounter.text = ("Planets Terraformed: " + planetCounter);

		MatchCheck();

		if(redFound && blueFound && greenFound && cloudRedFound && cloudBlueFound && cloudGreenFound && planetFound)
        {
			planetCounter++;
			completedPlanets.Add(CreativePlanet.GetComponent<MeshRenderer>().material);
			UnityEngine.Debug.Log(completedPlanets.Count);

			List<float> coloursFromPlanet = new List<float>();

			coloursFromPlanet.Add(pred);
			coloursFromPlanet.Add(pgreen);
			coloursFromPlanet.Add(pblue);
			coloursFromPlanet.Add(cloudRed);
			coloursFromPlanet.Add(cloudGreen);
			coloursFromPlanet.Add(cloudBlue);

			completedColours.Add(coloursFromPlanet);

			SetPlanet();
			redFound = false;
			greenFound = false;
			blueFound = false;
			cloudRedFound = false;
			cloudGreenFound = false;
			cloudBlueFound = false;
			planetFound = false;
		}
		else
        {

		}

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
				colourText.text = "Red Selected";

				if (cloudRedFound)
                {
					colourText.color = new Color(0, 1, 0);
					return;
                }
				else
                {
					colourText.color = new Color(1, 1, 1);
					return;
				}
			}
			else if (colours[colourIndex] == 2)
			{
				colourText.text = "Green Selected";

				if (cloudGreenFound)
				{
					colourText.color = new Color(0, 1, 0);
					return;
				}
				else
				{
					colourText.color = new Color(1, 1, 1);
					return;
				}
			}
			else if (colours[colourIndex] == 3)
			{
				colourText.text = "Blue Selected";

				if (cloudBlueFound)
				{
					colourText.color = new Color(0, 1, 0);
					return;
				}
				else
				{
					colourText.color = new Color(1, 1, 1);
					return;
				}
			}
		}
		else
		{
			cloudText.text = "";

			if (colours[colourIndex] == 1)
			{
				colourText.text = "Red Selected";

				if (redFound)
				{
					colourText.color = new Color(0, 1, 0);
					return;
				}
				else
				{
					colourText.color = new Color(1, 1, 1);
					return;
				}
			}
			else if (colours[colourIndex] == 2)
			{
				colourText.text = "Green Selected";

				if (greenFound)
				{
					colourText.color = new Color(0, 1, 0);
					return;
				}
				else
				{
					colourText.color = new Color(1, 1, 1);
					return;
				}

			}
			else if (colours[colourIndex] == 3)
			{
				colourText.text = "Blue Selected";

				if (blueFound)
				{
					colourText.color = new Color(0, 1, 0);
					return;
				}
				else
				{
					colourText.color = new Color(1, 1, 1);
					return;
				}
			}
		}
	}

	void ShowCompletedPlanets()
    {
		//Clear UI
		CreativePlanet.SetActive(false);
		planetToCopy.SetActive(false);

		if(gameOver && !gameDone)
        {
			List<GameObject> planets = new List<GameObject>();

			

			for (int i = 0; i < completedPlanets.Count; i++)
            {

				//Add the correct material
				MeshRenderer mesh = completedPlanet.GetComponent<MeshRenderer>();
				mesh.material = completedPlanets.ElementAt(i);


				//Add the correct colours
				Color planetColour = new Color( (completedColours.ElementAt(i)).ElementAt(0), completedColours.ElementAt(i).ElementAt(1), completedColours.ElementAt(i).ElementAt(2));
				completedPlanet.GetComponent<Renderer>().sharedMaterial.SetColor("_TextureColor", planetColour);

				//Add the correct cloud colours
				Color cloudColor = new Color(completedColours.ElementAt(i).ElementAt(3), completedColours.ElementAt(i).ElementAt(4), completedColours.ElementAt(i).ElementAt(5));
				completedPlanet.GetComponent<Renderer>().sharedMaterial.SetColor("_CloudColor", cloudColor);

				//IOnstantiate the completed planet
				Instantiate(completedPlanet, new Vector3(planetLocation.Next(-5, 5), planetLocation.Next(2, 8), planetLocation.Next(-5, 5)), Quaternion.identity);

				planets.Add(completedPlanet);
			}

			for (int i = 0; i < planets.Count; i++)
            {
				planets[i].AddComponent<TerraRotator>();
			}

			gameDone = true;

		}
    }

	void MatchCheck()
    {

			//Planet Colour Check
			if ((Math.Round(pred, 1) == Math.Round(planetRed, 1)))
            {
				redFound = true;
            }

			if ((Math.Round(pblue, 1) == Math.Round(planetBlue, 1)))
			{
				blueFound = true;
			}

			if ((Math.Round(pgreen, 1) == Math.Round(planetGreen, 1)))
			{
				greenFound = true;
			}

		//Cloud Colour Check
		if ((Math.Round(cloudRed, 1) == Math.Round(copyCloudRed, 1)))
		{
			cloudRedFound = true;
		}

		if ((Math.Round(cloudBlue, 1) == Math.Round(copyCloudBlue, 1)))
		{
			cloudBlueFound = true;
		}

		if ((Math.Round(cloudGreen, 1) == Math.Round(copyCloudGreen, 1)))
		{
			cloudGreenFound = true;
		}



		//Planet Starter Check
		if (index == copyIndex)
			{
				planetFound = true;
			}

		
    }

	void TimeRemaining(float timeToDisplay)
	{
		if (!gameFinished)
		{
			if (timeToDisplay > 0.0f)
			{
				float minutes = Mathf.FloorToInt(timeToDisplay / 60);
				float seconds = Mathf.FloorToInt(timeToDisplay % 60);
				Timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
			}
			else
			{
				timeToDisplay = 0.0f;
				timerIsRunning = false;
				Timer.color = new Color(1, 0, 0);
				Timer.text = "No Time Remaining\nSqueeze both Grip Buttons \nto Return to Main Menu";
				gameOver = true;
				ShowCompletedPlanets();
			}

		}

	}

	void RandomSelector(GameObject planetLayer, System.Random random, Material[] materials)
	{
		int index = whichStarter.Next(0, materials.Length);
		MeshRenderer mesh = planetLayer.GetComponent<MeshRenderer>();
		mesh.material = materials[index];
		copyIndex = index;
	}

	void SetPlanet()
	{
		//Reset planet variables for ending

		//Select starting layer
		RandomSelector(planetToCopy, whichStarter, copyStarters);

		//Randomize colour values
		planetRed = (float)copyPlanetColourValue.NextDouble();

		if(planetRed < 0.2f)
        {
			planetRed += 0.2f;
        }

		planetGreen = (float)copyPlanetColourValue.NextDouble();

		if (planetGreen < 0.2f)
		{
			planetGreen += 0.2f;
		}

		planetBlue = (float)copyPlanetColourValue.NextDouble();

		if (planetBlue < 0.2f)
		{
			planetBlue += 0.2f;
		}

		int clouds = cloudPicker.Next(0, 2);

		if (clouds == 0)
		{
			areThereClouds = false;
		}
		else
		{
			areThereClouds = true;
		}

		if (areThereClouds)
		{
			copyCloudRed = (float)copyPlanetColourValue.NextDouble();

			if (copyCloudRed < 0.2f)
			{
				copyCloudRed += 0.2f;
			}

			copyCloudGreen = (float)copyPlanetColourValue.NextDouble();

			if (copyCloudGreen < 0.2f)
			{
				copyCloudGreen += 0.2f;
			}

			copyCloudBlue = (float)copyPlanetColourValue.NextDouble();

			if (copyCloudBlue < 0.2f)
			{
				copyCloudBlue += 0.2f;
			}

		}
		else
		{
			copyCloudRed = noCloud;
			copyCloudGreen = noCloud;
			copyCloudBlue = noCloud;
		}

	}

	void PlanetColour()
	{
		SteamVR_Controller.Device rightDevice = SteamVR_Controller.Input((int)rightTrackedObj.index);
		SteamVR_Controller.Device leftDevice = SteamVR_Controller.Input((int)leftTrackedObj.index);

		if (!cloudsEnabled)
		{

			//Left Hand Increases the Colour Value
			if (leftDevice.GetTouch(SteamVR_Controller.ButtonMask.Trigger))
			{

				//Red
				if (colours[colourIndex] == 1)
				{
					if(!redFound)
                    {
						if (pred < 1.0f)
						{
							pred += valueChange;
						}
					}
				}
				//Green
				else if (colours[colourIndex] == 2)
				{
					if(!greenFound)
                    {
						if (pgreen < 1.0f)
						{
							pgreen += valueChange;
						}
					}
					
				}
				//Blue
				else if (colours[colourIndex] == 3)
				{
					if(!blueFound)
                    {
						if (pblue < 1.0f)
						{
							pblue += valueChange;
						}
					}
					
				}
			}
			//Right Hand Decreases the Colour Value
			else if (rightDevice.GetTouch(SteamVR_Controller.ButtonMask.Trigger))
			{

				//Red
				if (colours[colourIndex] == 1)
				{
					if(!redFound)
                    {
						if (pred > 0.0f)
						{
							pred -= valueChange;
						}
					}
				}
				//Green
				else if (colours[colourIndex] == 2)
				{
					if(!greenFound)
                    {
						if (pgreen > 0.0f)
						{
							pgreen -= valueChange;
						}
					}
				}
				//Blue
				else if (colours[colourIndex] == 3)
				{
					if(!blueFound)
                    {
						if (pblue > 0.0f)
						{
							pblue -= valueChange;
						}
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
					if(!cloudRedFound)
                    {
						if (red < 1.0f)
						{
							red += valueChange;
						}
					}
				}
				//Green
				else if (colours[colourIndex] == 2)
				{
					if(!cloudGreenFound)
                    {
						if (green < 1.0f)
						{
							green += valueChange;
						}
					}
				}
				//Blue
				else if (colours[colourIndex] == 3)
				{
					if(!cloudBlueFound)
                    {
						if (blue < 1.0f)
						{
							blue += valueChange;
						}
					}
				}
			}
			//Right Hand Decreases the Colour Value
			else if (rightDevice.GetTouch(SteamVR_Controller.ButtonMask.Trigger))
			{
				//Red
				if (colours[colourIndex] == 1)
				{
					if(!cloudRedFound)
                    {
						if (red > 0.0f)
						{
							red -= valueChange;
						}
					}
				}
				//Green
				else if (colours[colourIndex] == 2)
				{
					if(!cloudGreenFound)
                    {
						if (green > 0.0f)
						{
							green -= valueChange;
						}
					}
				}
				//Blue
				else if (colours[colourIndex] == 3)
				{
					if(!cloudBlueFound)
                    {
						if (blue > 0.0f)
						{
							blue -= valueChange;
						}
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
		if(!planetFound)
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
