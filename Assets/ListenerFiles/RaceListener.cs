using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RaceListener : MonoBehaviour
{

    public float barrenTransparency;
    public float surfaceTransparency;
    public float atmosTransparency;

    //public GameObject currentPlanet;
    public GameObject surfacePlanet;
    public GameObject barrenPlanet;
    public GameObject atmosphere;
    public GameObject highResPlanet;

    public Color barrenColor;
    public Color surfaceColor;
    public Color atmosColor;

    public Slider SurfaceTemp;
    public Slider LiquidWater;
    public Slider LifeSuppotingChemicals;
    public Slider AtmosphericDensity;

    public float displaceFactor;

    public GameObject ST;
    public GameObject LW;
    public GameObject LSC;
    public GameObject AD;
    public GameObject PA;

    public Text Timer;

    public Material[] surfaceTextures;
    public Material[] barrenTextures;
    public Material[] waterTextures;
    public Material[] lifeTextures;

    public List<Texture> terraformedPlanets;

    Boolean gameWon = false;
    Boolean gameOver = false;
    public float timeToDisplay = 5;
    Boolean timerIsRunning = true;

    System.Random surface = new System.Random();
    System.Random barren = new System.Random();
    System.Random water = new System.Random();
    System.Random life = new System.Random();

    System.Random planetLocation = new System.Random();

    public Text PlanetCounter;
    int planetCounter = 0;
    public GameObject terraformedPlanet;
    Boolean gameDone = false;

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

    public Texture[] displaceMaps;
    System.Random dMapPicker = new System.Random();

    int oldIndex = 0;

    int[] movementOrder = { 1, 2, 3, 4 };
    System.Random movePicker = new System.Random();

    public Text surfaceMove;
    public Text barrenMove;
    public Text waterMove;
    public Text atmosMove;

    public GameObject planetAligner;

    public GameObject fader;
    public Animator animator;
    private Boolean input = false;
    private string levelToLoad;


    // Start is called before the first frame update
    void Start()
    {
        PickMovements();
    }

    // Update is called once per frame
    void Update()
    {
        if (input == true)
        {
            StartCoroutine(Fading());
        }

        GetHighResPlanetDisplacements();
        
        //Restarts the planet after successfully terraformed
        if(AtmosphericDensity.value <=0.0f)
        {
            GameObject mesh = highResPlanet.transform.Find("mesh").gameObject;
            Texture instance = mesh.GetComponent<Renderer>().sharedMaterial.GetTexture("_Tex1");

            terraformedPlanets.Add(instance);

            UnityEngine.Debug.Log(terraformedPlanets.Count);
            UnityEngine.Debug.Log(instance);


            PickMovements();

            SetPlanet();
            planetCounter++;
        }

        //Operates the clock
        if (timerIsRunning)
        {
            timeToDisplay -= Time.deltaTime;
            TimeRemaining(timeToDisplay);
        }

        PlanetCounter.text = ("Planets Terraformed: " + planetCounter);


        surfacePlanet.transform.Rotate(Vector3.up * Time.deltaTime * 5.0f);
        barrenPlanet.transform.Rotate(Vector3.up * Time.deltaTime * 5.0f);
        atmosphere.transform.Rotate(Vector3.up * Time.deltaTime * 7.0f);

        PlanetViveControls();

        PlanetFade(surfacePlanet, surfaceColor, surfaceTransparency);
        PlanetFade(barrenPlanet, barrenColor, barrenTransparency);
        PlanetFade(atmosphere, atmosColor, atmosTransparency);

        ShowTerraformedPlanets();
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

    void PickMovements()
    {
        for (int i = 0; i < movementOrder.Length - 1; i++)
        {
            int j = movePicker.Next(i, movementOrder.Length);
            int temp = movementOrder[i];
            movementOrder[i] = movementOrder[j];
            movementOrder[j] = temp;
        }

        Text[] moves = { surfaceMove, barrenMove, waterMove, atmosMove };

        for (int i = 0; i < movementOrder.Length; i++)
        {
            if(movementOrder[i] == 1)
            {
                moves[i].text = "RIGHT TWIST";
            }
            else if (movementOrder[i] == 2)
            {
                moves[i].text = "LEFT TWIST";
            }
            else if (movementOrder[i] == 3)
            {
                moves[i].text = "PRESS";
            }
            else
            {
                moves[i].text = "TRIGGERS";
            }
        }
    }


    void GetHighResPlanetDisplacements()
    {
        GameObject mesh = highResPlanet.transform.Find("mesh").gameObject;
        mesh.GetComponent<Renderer>().sharedMaterial.SetFloat("_DisplaceFactor", displaceFactor);
    }

    void PressingMotion(ref float change, ref Slider UIValue, int waterPart)
    {
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
                    if(waterPart == 0)
                    {
                        change += 0.25f;
                        UIValue.value -= 0.25f;
                    }
                    else
                    {
                        change -= 0.25f;
                        UIValue.value -= 0.25f;
                    }

                    maxPoint = false;
                    minPoint = false;
                    return;
                }
            }
        }
    }

    void RightTwist(ref float change, ref Slider UIValue, int waterPart)
    {
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
                    if (waterPart == 0)
                    {
                        change += 0.25f;
                        UIValue.value -= 0.25f;
                    }
                    else
                    {
                        change -= 0.25f;
                        UIValue.value -= 0.25f;
                    }

                    RrightPoint = false;
                    RleftPoint = false;
                    return;
                }
            }
        }
    }

    void LeftTwist(ref float change, ref Slider UIValue, int waterPart)
    {
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
                    if (waterPart == 0)
                    {
                        change += 0.25f;
                        UIValue.value -= 0.25f;
                    }
                    else
                    {
                        change -= 0.25f;
                        UIValue.value -= 0.25f;
                    }

                    LrightPoint = false;
                    LleftPoint = false;
                    return;
                }
            }
        }
    }

    void AlternatingTriggers(ref float change, ref Slider UIValue, int waterPart)
    {
        SteamVR_Controller.Device rightDevice = SteamVR_Controller.Input((int)rightTrackedObj.index);
        SteamVR_Controller.Device leftDevice = SteamVR_Controller.Input((int)leftTrackedObj.index);

        if (leftDevice.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            lTrigDown = true;
        }

        if (rightDevice.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            rTrigDown = true;
        }

        if (lTrigDown && rTrigDown)
        {
            if (waterPart == 0)
            {
                change += 0.25f;
                UIValue.value -= 0.25f;
            }
            else
            {
                change -= 0.25f;
                UIValue.value -= 0.25f;
            }

            lTrigDown = false;
            rTrigDown = false;
            return;
        }

    }

    void PlanetViveControls()
    {
        if (!gameOver)
        {

            if (LiquidWater.value <= 0.0f)
            {
                barrenPlanet.SetActive(false);
            }

            if (SurfaceTemp.value <= 0.0f)
            {
                surfacePlanet.SetActive(false);
            }

            //1st Stage
            if (barrenTransparency < 1.0f)
            {
                if (movementOrder[0] == 1)
                {
                    RightTwist(ref barrenTransparency, ref SurfaceTemp, 0);
                }
                else if (movementOrder[0] == 2)
                {
                    LeftTwist(ref barrenTransparency, ref SurfaceTemp, 0);
                }
                else if (movementOrder[0] == 3)
                {
                    PressingMotion(ref barrenTransparency, ref SurfaceTemp, 0);
                }
                else
                {
                    AlternatingTriggers(ref barrenTransparency, ref SurfaceTemp, 0);
                }

            }

            //2nd Stage
            if (SurfaceTemp.value <= 0.0f)
            {
                if (barrenTransparency > 0.0f)
                {
                    if (movementOrder[1] == 1)
                    {
                        RightTwist(ref barrenTransparency, ref LiquidWater, 1);
                    }
                    else if (movementOrder[1] == 2)
                    {
                        LeftTwist(ref barrenTransparency, ref LiquidWater, 1);
                    }
                    else if (movementOrder[1] == 3)
                    {
                        PressingMotion(ref barrenTransparency, ref LiquidWater, 1);
                    }
                    else
                    {
                        AlternatingTriggers(ref barrenTransparency, ref LiquidWater, 1);
                    }
                }
            }

            //3rd Life Stage
            if (LiquidWater.value <= 0.0f)
            {
                if (displaceFactor < 1.0f)
                {
                    if (movementOrder[2] == 1)
                    {
                        RightTwist(ref displaceFactor, ref LifeSuppotingChemicals, 0);
                    }
                    else if (movementOrder[2] == 2)
                    {
                        LeftTwist(ref displaceFactor, ref LifeSuppotingChemicals, 0);
                        
                    }
                    else if (movementOrder[2] == 3)
                    {
                        PressingMotion(ref displaceFactor, ref LifeSuppotingChemicals, 0);
                        
                    }
                    else
                    {
                        AlternatingTriggers(ref displaceFactor, ref LifeSuppotingChemicals, 0);
                    }
                }
            }

            //Final Stage
            if (LifeSuppotingChemicals.value <= 0.0f)
            {
                if (atmosTransparency < 1.0f)
                {
                    if (movementOrder[3] == 1)
                    {
                        RightTwist(ref atmosTransparency, ref AtmosphericDensity, 0);
                        
                    }
                    else if (movementOrder[3] == 2)
                    {
                        LeftTwist(ref atmosTransparency, ref AtmosphericDensity, 0);
                       
                    }
                    else if (movementOrder[3] == 3)
                    {
                        PressingMotion(ref atmosTransparency, ref AtmosphericDensity, 0);
                    }
                    else
                    {
                        AlternatingTriggers(ref atmosTransparency, ref AtmosphericDensity, 0);
                    }
                }
            }

        }
    }

    void RandomSelector(GameObject planetLayer, System.Random random, Material[] materials)
    {
        //int index = random.Next(0, materials.Length);

        //Using the same random value, so that the same
        //random value is randomised, instead of three
        //different values randomising the same way
        int index = surface.Next(0, materials.Length);

        if(index == oldIndex)
        {
            if(index == 0)
            {
                index = 1;
            }
            else
            {
                index++;
            }
        }
        else
        {
            oldIndex = index;
        }

        MeshRenderer mesh = planetLayer.GetComponent<MeshRenderer>();
        mesh.material = materials[index];
    }

    void SetPlanet()
    {

        //Initalise planet layers

        RandomSelector(barrenPlanet, barren, barrenTextures);

        barrenPlanet.SetActive(true);
        surfacePlanet.SetActive(true);

        int index = dMapPicker.Next(0, displaceMaps.Length);

        if (index == oldIndex)
        {
            if (index == 0)
            {
                index = 1;
            }
            else
            {
                index++;
            }
        }
        else
        {
            oldIndex = index;
        }

        GameObject mesh = highResPlanet.transform.Find("mesh").gameObject;
        mesh.GetComponent<Renderer>().sharedMaterial.SetTexture("_Tex1", displaceMaps[index]);

        //Initialise all the colour values

        surfaceTransparency = 1.0f;
        barrenTransparency = 0.0f;
        atmosTransparency = 0.0f;
        displaceFactor = 0.0f;

        SurfaceTemp.value = 1.0f;
        LiquidWater.value = 1.0f;
        LifeSuppotingChemicals.value = 1.0f;
        AtmosphericDensity.value = 1.0f;
    }

    void TimeRemaining(float timeToDisplay)
    {
        if (!gameWon)
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
            }

        }

    }

    void ShowTerraformedPlanets()
    {
        if(gameOver && !gameDone)
        {
            //Cleanse UI
            ST.SetActive(false);
            LW.SetActive(false);
            LSC.SetActive(false);
            AD.SetActive(false);
            PA.SetActive(false);
            planetAligner.SetActive(false);

            displaceFactor = 1.0f;

            List<GameObject> planets = new List<GameObject>();

            for (int i = 0; i < terraformedPlanets.Count; i++)
            {
                //terraformedPlanet.GetComponent<Renderer>().sharedMaterial.SetTexture("_Tex1", terraformedPlanets.ElementAt(i));
                Instantiate(terraformedPlanet, new Vector3(planetLocation.Next(-5, 5), planetLocation.Next(2, 5), planetLocation.Next(-5, 5)), Quaternion.identity);

                planets.Add(terraformedPlanet);
            }

            for (int i = 0; i < planets.Count; i++)
            {
                UnityEngine.Debug.Log(terraformedPlanets.ElementAt(i));
                UnityEngine.Debug.Log(planets[i]);

                planets[i].AddComponent<TerraRotator>();
                //planets[i].GetComponent<Renderer>().material.SetTexture("_Tex1", terraformedPlanets.ElementAt(i));
            }

            gameDone = true;
        }
    }


    void PlanetFade(GameObject planet, Color colour, float transparency)
    {
        colour = planet.GetComponent<Renderer>().material.color;
        colour.a = transparency;
        planet.GetComponent<Renderer>().material.color = colour;

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
