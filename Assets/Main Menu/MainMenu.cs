using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public SteamVR_TrackedObject rightTrackedObj;
    public SteamVR_TrackedObject leftTrackedObj;

    public Animator animator;

    private string levelToLoad;

    private Boolean input = false;

    public GameObject fader;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        SteamVR_Controller.Device rightDevice = SteamVR_Controller.Input((int)rightTrackedObj.index);
        SteamVR_Controller.Device leftDevice = SteamVR_Controller.Input((int)leftTrackedObj.index);

        if(input == true)
        {
            StartCoroutine(Fading());
        }


        if (rightDevice.GetTouch(SteamVR_Controller.ButtonMask.Trigger))
        {
            //Creative Mode
            //SceneManager.LoadScene("Creative_Tutorial");
            rightDevice.TriggerHapticPulse(1000);
            FadeToLevel("Creative_Tutorial");
        }
        else if (leftDevice.GetTouch(SteamVR_Controller.ButtonMask.Trigger))
        {
            //Race Mode
            //SceneManager.LoadScene("RaceMode_Tutorial");
            leftDevice.TriggerHapticPulse(1000);
            FadeToLevel("RaceMode_Tutorial");
        }
        else if (rightDevice.GetTouch(SteamVR_Controller.ButtonMask.Grip))
        {
            //Campaign
            rightDevice.TriggerHapticPulse(1000);
            FadeToLevel("Campaign_Tutorial");
        }
        else if (leftDevice.GetTouch(SteamVR_Controller.ButtonMask.Grip))
        {
            //Creative Race Mode
            //SceneManager.LoadScene("CreativeRace_Tutorial");
            leftDevice.TriggerHapticPulse(1000);
            FadeToLevel("CreativeRace_Tutorial");
        }
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
}
