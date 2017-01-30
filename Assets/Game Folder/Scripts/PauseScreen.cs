using UnityEngine;
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
    private GameObject hexagonalTilesParentObject;

    [SerializeField]
    private Image[] visualButtons;

    [SerializeField]
    private GameObject[] submenus;

    [SerializeField]
    private float wheelRotationSpeed;

    private GameObject[] hexagonalTiles;
    private int tileLightUpCounter;

    private int pauseScreenOwner;
    private int numberOfOptions = 3;
    private int selectedOption = 0;

    //Animation
    private float wheelRotationAngle;
    private float wheelAngleToReach;
    private bool wheelShouldRotate;
    private int wheelPolarity;

    /*this exists here as PlayerInput locks down on pause, and as it's all in faceButtonCheck I can't make
    pause exempt from the lock. Or maybe I could. But I'm an idiot*/
    private bool pauseAxisLock;
    private bool navAxisLock;
    private bool acceptAxisLock;

    MatchInitialisation matchInit;

    // Use this for initialization
    void Start()
    {
        tileLightUpCounter = 0;
        hexagonalTiles = GameObject.FindGameObjectsWithTag("HexagonalPauseTile");
        hideHexagonalArray();
        matchInit = GameObject.Find("Manager").GetComponent<MatchInitialisation>();

        wheelShouldRotate = false;
        wheelAngleToReach = 360 / numberOfOptions;

        selectedOption = 0;
        submenus[selectedOption].SetActive(true);

        //StartCoroutine(lightUpHexagonalArray());
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Selected Option = " + selectedOption);
        if (IsPauseScreenActive() == true)
        {
            checkNavigationButtons();
            if (wheelShouldRotate == true)
            {
                animateWheel();
            }
            toggleHUD(false);

            //dismissPauseScreen();
            matchInit.ToggleFullscreenCam(matchInit.GetGameCamera(0), true);
        }
        else
        {
            tileLightUpCounter = 9999;
            hideHexagonalArray();
            toggleHUD(true);
            matchInit.ToggleFullscreenCam(matchInit.GetGameCamera(0), false);
        }
    }

    public void SetCrosshairs(Image[] pCrosshairs)
    {
        Crosshairs = pCrosshairs;
    }

    public void ResetLightUpCounter()
    {
      tileLightUpCounter = 0;
    }

    public IEnumerator LightUpHexagonalArray(bool pState)
    {
        while(tileLightUpCounter < (int)(hexagonalTiles.Length * 0.66f))
        {
            hexagonalTiles[Random.Range(0, hexagonalTiles.Length)].GetComponent<Image>().enabled = pState;
            tileLightUpCounter++;
            yield return new WaitForSeconds(0.04f);
        }

    }

    private void hideHexagonalArray()
    {
      for (int i = 0; i < hexagonalTiles.Length; i++)
      {
          hexagonalTiles[i].GetComponent<Image>().enabled = false;
      }
    }

    private void checkNavigationButtons()
    {
        if (InputManager.MovementHorizontal(pauseScreenOwner) != 0 && navAxisLock == false)
        {
            //Debug.Log("Getting nav controls");
            navAxisLock = true;

            if (InputManager.MovementHorizontal(pauseScreenOwner) < 0)
            {
                wheelPolarity = -1;
            }
            else if (InputManager.MovementHorizontal(pauseScreenOwner) > 0)
            {
                wheelPolarity = 1;
            }

            if (InputManager.MovementVertical(pauseScreenOwner) < 0)
            {
                //do something
            }
            else if (InputManager.MovementVertical(pauseScreenOwner) > 0)
            {
                //do something
            }

            wheelShouldRotate = true;
          //  DisplayPauseScreen(false, 0);
        }
        if (InputManager.PauseButton(pauseScreenOwner) == 0)
        {
            navAxisLock = false;
        }
    }

    //move the pause wheel based on option selected
    private void animateWheel()
    {
        submenus[selectedOption].SetActive(false);
        pauseWheel.transform.Rotate(0, 0, wheelRotationSpeed * wheelPolarity);
        wheelRotationAngle += (wheelRotationSpeed * wheelPolarity);

        for (int i = 0; i < visualButtons.Length; i++)
        {
            visualButtons[i].transform.Rotate(0, 0, (-wheelRotationSpeed * wheelPolarity));
            //rotate on the other axes based on controller input?
        }

        if (wheelRotationAngle >= wheelAngleToReach)
        {
            wheelRotationAngle = 0.0f;
            wheelShouldRotate = false;
            selectedOption -= wheelPolarity * 1;
            if (selectedOption < 0 || selectedOption > numberOfOptions - 1)
            {
                selectedOption = numberOfOptions - 1;
            }
            if (selectedOption > numberOfOptions - 1)
            {
                selectedOption = 0;
            }
            submenus[selectedOption].SetActive(true);
        }
        else if (wheelRotationAngle <= -wheelAngleToReach)
        {
            wheelRotationAngle = 0.0f;
            wheelShouldRotate = false;
            selectedOption += wheelPolarity * 1;
            if (selectedOption < 0)
            {
                selectedOption = numberOfOptions - 1;
            }
            if (selectedOption > numberOfOptions - 1)
            {
                selectedOption = 0;
            }
            submenus[selectedOption].SetActive(true);
        }
    }

    public void DisplayPauseScreen(bool pState, int pPlayerID)
    {
        selectedOption = 0;
        pauseScreen.SetActive(pState);
        if (pState == true)
        {
            Cursor.visible = true;
        }
        else
        {
            Cursor.visible = false;
        }

        pauseScreenOwner = pPlayerID;
    }

    /// <summary>
    /// Whether the pause screen is active. This also means that the game is paused and not just that the canvas is enabled.
    /// </summary>
    /// <returns></returns>
    public bool IsPauseScreenActive()
    {
        return pauseScreen.activeSelf;
    }

    //returns who started the pause screen
    public int GetPauseScreenOwner()
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

    private void dismissPauseScreen()
    {
        if (InputManager.PauseButton(pauseScreenOwner) > 0 && pauseAxisLock == false)
        {
            Debug.Log("Unpaused.");
            pauseAxisLock = true;
            DisplayPauseScreen(false, 0);
        }
        if(InputManager.PauseButton(pauseScreenOwner) == 0)
        {
            pauseAxisLock = false;
        }
    }
}
