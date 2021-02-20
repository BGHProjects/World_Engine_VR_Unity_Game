using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class NewCampaignManager : MonoBehaviour
{

    public GameObject planet;

    public SteamVR_TrackedObject rightTrackedObj;
    public SteamVR_TrackedObject leftTrackedObj;

    GradientColorKey[] colorKey;
    GradientColorKey[] otherColorKey;
    GradientAlphaKey[] alphaKey;

    Gradient gradient;
    Gradient newGradient = new Gradient();
    Gradient otherGradient = new Gradient();

    // Start is called before the first frame update
    void Start()
    {
        gradient = planet.GetComponent<Planet>().colourSettings.biomeColourSettings.biomes[0].gradient;
    }

    // Update is called once per frame
    void Update()
    {
        //Test Alpha Settings
        alphaKey = new GradientAlphaKey[2];
        alphaKey[0].alpha = 1.0f;
        alphaKey[0].time = 0.0f;
        alphaKey[1].alpha = 1.0f;
        alphaKey[1].time = 1.0f;

        SteamVR_Controller.Device rightDevice = SteamVR_Controller.Input((int)rightTrackedObj.index);
        SteamVR_Controller.Device leftDevice = SteamVR_Controller.Input((int)leftTrackedObj.index);

        //UnityEngine.Debug.Log(colorKey);

        if (rightDevice.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            //Test Gradient Settings
            colorKey = new GradientColorKey[2];
            colorKey[0].color = Color.red;
            colorKey[0].time = 0.0f;
            colorKey[1].color = Color.blue;
            colorKey[1].time = 1.0f;

            gradient.SetKeys(colorKey, alphaKey);

            planet.GetComponent<Planet>().colourGenerator.UpdateSettings(planet.GetComponent<Planet>().colourSettings);

            UnityEngine.Debug.Log("Right Trigger Triggered");
        }

        if (leftDevice.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
        {

            otherColorKey = new GradientColorKey[2];
            otherColorKey[0].color = Color.green;
            otherColorKey[0].time = 0.0f;
            otherColorKey[1].color = Color.red;
            otherColorKey[1].time = 1.0f;


            gradient.SetKeys(otherColorKey, alphaKey);


            UnityEngine.Debug.Log("Left Trigger Triggered");
        }
    }
}
