using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CampaignTutorialManager : MonoBehaviour
{

    public SteamVR_TrackedObject rightTrackedObj;
    public SteamVR_TrackedObject leftTrackedObj;

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
        SteamVR_Controller.Device rightDevice = SteamVR_Controller.Input((int)rightTrackedObj.index);

        if (rightDevice.GetTouch(SteamVR_Controller.ButtonMask.Trigger))
        {
            rightDevice.TriggerHapticPulse(1000);
            FadeToLevel("Campaign");
        }

        if (input == true)
        {
            StartCoroutine(Fading());
        }

        ReturnToMainMenu();
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
