using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MyMessageListener : MonoBehaviour {

	public float barrenTransparency;
	public float lavaTransparency;
	public float atmosTransparency;

	public GameObject lavaPlanet;
	public GameObject barrenPlanet;
	public GameObject atmosphere;
	public GameObject highResPlanet;

	public Color barrenColor;
	public Color lavaColor;
	public Color atmosColor;

	public Slider SurfaceTemp;
	public Slider LiquidWater;
	public Slider LifeSuppotingChemicals;
	public Slider AtmosphericDensity;

	public float displaceFactor;

	public Text Timer;

	Boolean gameWon = false;
	Boolean gameLost = false;
	public float timeToDisplay = 5;
	Boolean timerIsRunning = true;

	public SteamVR_TrackedObject rightTrackedObj;
	public SteamVR_TrackedObject leftTrackedObj;

	bool maxPoint = false;
	bool minPoint = false;
	int increment = 0;
	float maxThresh = 0.8f;
	float minThresh = 0.2f;

	float rightThresh = -0.7f;
	float leftThresh = 0.4f;
	bool RrightPoint = false;
	bool RleftPoint = false;
	bool LrightPoint = false;
	bool LleftPoint = false;

	bool lTrigDown = false;
	bool rTrigDown = false;

	public GameObject fader;
	public Animator animator;
	private Boolean input = false;
	private string levelToLoad;


	// Use this for initialization
	void Start () {


}


// Update is called once per frame
void Update () {

		if (input == true)
		{
			StartCoroutine(Fading());
		}

		GetHighResPlanetDisplacements();

		//planet spins
		lavaPlanet.transform.Rotate(Vector3.up * Time.deltaTime * 5.0f);
		barrenPlanet.transform.Rotate(Vector3.up * Time.deltaTime * 5.0f);
		atmosphere.transform.Rotate(Vector3.up * Time.deltaTime * 7.0f);

		PlanetViveControls();

		if (timerIsRunning)
        {
			timeToDisplay -= Time.deltaTime;
			TimeRemaining(timeToDisplay);
		}

		PlanetFade(lavaPlanet, lavaColor, lavaTransparency);
		PlanetFade(barrenPlanet, barrenColor, barrenTransparency);
		PlanetFade(atmosphere, atmosColor, atmosTransparency);

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

	void PressingMotion()
    {
		SteamVR_Controller.Device rightDevice = SteamVR_Controller.Input((int)rightTrackedObj.index);
		SteamVR_Controller.Device leftDevice = SteamVR_Controller.Input((int)leftTrackedObj.index);

		float dist = Vector3.Distance(rightTrackedObj.transform.position, leftTrackedObj.transform.position);

		if(dist > maxThresh)
        {
			maxPoint = true;
			return;
        }

		if(maxPoint)
        {
			if (dist < minThresh)
			{
				minPoint = true;

				if (maxPoint && minPoint)
				{
					//0.03f
					displaceFactor += 0.25f;
					LifeSuppotingChemicals.value -= 0.25f;

					rightDevice.TriggerHapticPulse(1000);
					leftDevice.TriggerHapticPulse(1000);

					maxPoint = false;
					minPoint = false;
					return;
				}
			}
		}
	}

void RightTwist()
    {

		if (rightTrackedObj.transform.rotation[2] <= rightThresh)
        {
			RrightPoint = true;
		}

		if(RrightPoint)
        {
			if (rightTrackedObj.transform.rotation[2] >= leftThresh)
			{
				RleftPoint = true;

				if (RrightPoint && RleftPoint)
                {
					//0.03f
					barrenTransparency += 0.25f;
					SurfaceTemp.value -= 0.25f;

					RrightPoint = false;
					RleftPoint = false;
					return;
                }
			}
		}
	}

	void LeftTwist()
    {
		if (leftTrackedObj.transform.rotation[2] >= leftThresh)
		{
			LleftPoint = true;
		}


		if(LleftPoint)
        {
			if (leftTrackedObj.transform.rotation[2] <= rightThresh)
			{
				LrightPoint = true;

				if (LrightPoint && LleftPoint)
				{
					//0.03f
					barrenTransparency -= 0.25f;
					LiquidWater.value -= 0.25f;
					LrightPoint = false;
					LleftPoint = false;
					return;
				}
			}
		}
	}

	void AlternatingTriggers()
    {
		SteamVR_Controller.Device rightDevice = SteamVR_Controller.Input((int)rightTrackedObj.index);
		SteamVR_Controller.Device leftDevice = SteamVR_Controller.Input((int)leftTrackedObj.index);

		if(leftDevice.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
        {
			lTrigDown = true;
        }

		if (rightDevice.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
        {
			rTrigDown = true;
        }

		if (lTrigDown && rTrigDown)
		{
			//0.01
			atmosTransparency += 0.25f;
			AtmosphericDensity.value -= 0.25f;
			lTrigDown = false;
			rTrigDown = false;
			return;
		}

	}


	void TimeRemaining(float timeToDisplay)
    {
		if(!gameWon)
        {
			if(timeToDisplay > 0.0f)
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
				Timer.text = "Planet Not Habitable, YOU LOSE\nSqueeze both Grip Buttons \nto Return to Main Menu";
				gameLost = true;
            }
			
		}
		
	}


	void PlanetFade(GameObject planet, Color colour, float transparency)
	{
		colour = planet.GetComponent<Renderer>().material.color;
		colour.a = transparency;
		planet.GetComponent<Renderer>().material.color = colour;
	}

	void GetHighResPlanetDisplacements()
    {
		GameObject mesh = highResPlanet.transform.Find("mesh").gameObject;
		mesh.GetComponent<Renderer>().sharedMaterial.SetFloat("_DisplaceFactor", displaceFactor);
	}

	void PlanetViveControls()
    {
		if (!gameLost)
        {
			//Win Condition
			if (AtmosphericDensity.value <= 0.0f)
			{
				gameWon = true;
				Timer.color = new Color(0, 1, 0);
				Timer.text = "HABITABILITY ACHIEVED\nSqueeze both Grip Buttons \nto Return to Main Menu";
			}

			if (LiquidWater.value <= 0.0f)
			{
				barrenPlanet.SetActive(false);
			}

			if (SurfaceTemp.value <= 0.0f)
			{
				lavaPlanet.SetActive(false);
			}

			//1st Stage
			if(barrenTransparency < 1.0f)
            {
				RightTwist();
            }

			//2nd Stage
			if (SurfaceTemp.value <= 0.0f)
			{
				if (barrenTransparency > 0.0f)
				{
					LeftTwist();
				}
			}

			//3rd Life Stage
			if (LiquidWater.value <= 0.0f)
			{
				if (displaceFactor < 1.0f)
				{
					PressingMotion();
				}
			}

			//Final Stage
			if (LifeSuppotingChemicals.value <= 0.0f)
			{
				if (atmosTransparency < 1.0f)
				{
					AlternatingTriggers();
				}
			}

		}
    }


	// Invoked when a line of data is received from the serial device.

	void OnMessageArrived(string msg)
	{
		UnityEngine.Debug.Log("Arrived: " + msg);


		if (msg == "Left button pressed")
		{
			UnityEngine.Debug.Log(msg);

		}
		else if (msg == "Right button pressed")
		{
			UnityEngine.Debug.Log(msg);

		}
		else if (msg.StartsWith("P")) //Reads in the Potentiometer reading
		{
			UnityEngine.Debug.Log(msg);

		}
	}


// Invoked when a connect/disconnect event occurs. The parameter 'success'
// will be 'true' upon connection, and 'false' upon disconnection or
// failure to connect.
void OnConnectionEvent(bool success)
{
	UnityEngine.Debug.Log(success ? "Device connected" : "Device disconnected");
}





}
