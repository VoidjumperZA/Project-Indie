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
    private Image[] PlayerProfiles;

    [SerializeField]
    private Text playerTextName;

    [SerializeField]
    private float wheelRotationSpeed;

    private GameObject[] hexagonalTiles;
    private int tileLightUpCounter;

    private int pauseScreenOwner;
    private int numberOfOptions;
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
    private ActivePlayers activePlayers;

    // Use this for initialization
    void Start()
    {
        tileLightUpCounter = 0;
        numberOfOptions = submenus.Length;
        hexagonalTiles = GameObject.FindGameObjectsWithTag("HexagonalPauseTile");
        activePlayers = GameObject.Find("Manager").GetComponent<ActivePlayers>();
        hideHexagonalArray();
        matchInit = GameObject.Find("Manager").GetComponent<MatchInitialisation>();
        wheelShouldRotate = false;
        wheelAngleToReach = 360 / numberOfOptions;

        selectedOption = 0;
        submenus[selectedOption].SetActive(true);

        for (int i = 0; i < PlayerProfiles.Length; i++)
        {
            PlayerProfiles[i].GetComponent<Image>().enabled = false;
        }
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
      ShuffleHexagonalArray(hexagonalTiles);
    }

    public IEnumerator LightUpHexagonalArray(bool pState)
    {
        //change alpha values of all tiles
        while (tileLightUpCounter < hexagonalTiles.Length)
        {
            //int activeTile = Random.Range(0, hexagonalTiles.Length);
            Color randomAlphaColour = hexagonalTiles[tileLightUpCounter].GetComponent<Image>().color;
            randomAlphaColour.a = Random.Range(0.3f, 1.0f);
            hexagonalTiles[tileLightUpCounter].GetComponent<Image>().color = randomAlphaColour;
            hexagonalTiles[tileLightUpCounter].GetComponent<Image>().enabled = pState;
            tileLightUpCounter++;
            yield return new WaitForSeconds(0.004f);
        }
        //randomly activate certain hexes
        /*
        while(tileLightUpCounter < (int)(hexagonalTiles.Length * 0.66f))
        {
            hexagonalTiles[Random.Range(0, hexagonalTiles.Length)].GetComponent<Image>().enabled = pState;
            tileLightUpCounter++;
            yield return new WaitForSeconds(0.04f);
        }*/

    }

    public void ShuffleHexagonalArray(GameObject[] hexArray)
    {
        int n = hexArray.Length;
        while (n > 1)
        {
            int k = Random.Range(0, n--);
            GameObject temp = hexArray[n];
            hexArray[n] = hexArray[k];
            hexArray[k] = temp;
        }
    }


    public void DisplayPauseScreenOwner()
    {
        playerTextName.text = "Player " + pauseScreenOwner + "  -  ";
        PlayerProfiles[activePlayers.GetPlayerNumberFromID(pauseScreenOwner) - 1].GetComponent<Image>().enabled = true;
    }

    public void  HidePauseScreenOwner()
    {
        PlayerProfiles[activePlayers.GetPlayerNumberFromID(pauseScreenOwner) - 1].GetComponent<Image>().enabled = false;
    }

    //disable all hexagons
    private void hideHexagonalArray()
    {
      for (int i = 0; i < hexagonalTiles.Length; i++)
      {
          hexagonalTiles[i].GetComponent<Image>().enabled = false;
      }        
    }

    private void checkNavigationButtons()
    {
        if (InputManager.UIHorizontal(pauseScreenOwner) != 0 && navAxisLock == false)
        {
            navAxisLock = true;

            if (InputManager.UIHorizontal(pauseScreenOwner) < 0)
            {
                wheelPolarity = -1;
            }
            else if (InputManager.UIHorizontal(pauseScreenOwner) > 0)
            {
                wheelPolarity = 1;
            }

            wheelShouldRotate = true;
        }
        if (InputManager.PauseButton(pauseScreenOwner) == 0)
        {
            navAxisLock = false;
        }
    }

    public void DisableActiveSubmenu()
    {
        submenus[selectedOption].SetActive(false);
    }

    //move the pause wheel based on option selected
    private void animateWheel()
    {
        //toggle old option off and start rotating the wheel
        submenus[selectedOption].SetActive(false);
        pauseWheel.transform.Rotate(0, 0, wheelRotationSpeed * wheelPolarity);
        wheelRotationAngle += (wheelRotationSpeed * wheelPolarity);

        //rotate all the buttons in the opposite direction to wheel rotation, to ensure they remain straight up
        for (int i = 0; i < visualButtons.Length; i++)
        {
            visualButtons[i].transform.Rotate(0, 0, (-wheelRotationSpeed * wheelPolarity));
            //rotate on the other axes based on controller input?
        }

        //if we've rotated sufficiently
        if (wheelRotationAngle >= wheelAngleToReach)
        {
            wheelRotationAngle = 0.0f;
            wheelShouldRotate = false;
            selectedOption += (wheelPolarity * -1);
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
        else if (wheelRotationAngle <= -wheelAngleToReach)
        {
            wheelRotationAngle = 0.0f;
            wheelShouldRotate = false;
            selectedOption += (wheelPolarity * -1);
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
        pauseScreen.SetActive(pState);
        if (pState == true)
        {
            selectedOption = 0;
            Cursor.visible = true;
            submenus[selectedOption].SetActive(true);
        }
        else
        {
            pauseWheel.transform.Rotate(0, 0, (360 / numberOfOptions) * selectedOption);
            for (int i = 0; i < visualButtons.Length; i++)
            {
                visualButtons[i].transform.Rotate(0, 0, -(360 / numberOfOptions) * selectedOption);
                //rotate on the other axes based on controller input?
            }
            selectedOption = 0;
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
