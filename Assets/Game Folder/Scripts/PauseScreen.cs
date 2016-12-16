﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PauseScreen : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseScreen;

    [SerializeField]
    private GameObject HUD;

    //[SerializeField]
    private Image[] Crosshairs;

    [SerializeField]
    private Image pauseWheel;

    [SerializeField]
    private Image[] visualButtons;

    [SerializeField]
    private float wheelRotationSpeed;

    private float pauseScreenOwner;
    MatchInitialisation matchInit; 

    // Use this for initialization
    void Start()
    {
        matchInit = GameObject.Find("Manager").GetComponent<MatchInitialisation>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPauseScreenActive() == true)
        {
            animateWheel();
            toggleHUD(false);
           
            matchInit.ToggleFullscreenCam(matchInit.GetGameCamera(0), true);
        }
        else
        {
            toggleHUD(true);
            matchInit.ToggleFullscreenCam(matchInit.GetGameCamera(0), false);
        }
    }

    public void SetCrosshairs(Image[] pCrosshairs)
    {
        Crosshairs = pCrosshairs;
    }

    private void animateWheel()
    {
        pauseWheel.transform.Rotate(0, 0, wheelRotationSpeed);
        for (int i = 0; i < visualButtons.Length; i++)
        {
            visualButtons[i].transform.Rotate(0, 0, -wheelRotationSpeed);
            //rotate on the other axes based on controller input?
        }
    }

    public void DisplayPauseScreen(bool pState, float pPlayerID)
    {
        pauseScreen.SetActive(pState);
        pauseScreenOwner = pPlayerID;
    }

    public bool IsPauseScreenActive()
    {
        return pauseScreen.activeSelf;
    }

    public float GetPauseScreenOwner()
    {
        return pauseScreenOwner;
    }

    private void toggleHUD(bool pState)
    {
        HUD.SetActive(pState);
        for (int i = 0; i < Crosshairs.Length; i++)
        {
            Crosshairs[i].gameObject.SetActive(pState);
        }
    }
}
