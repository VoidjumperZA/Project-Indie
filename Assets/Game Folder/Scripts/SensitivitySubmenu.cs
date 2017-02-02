using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SensitivitySubmenu : MonoBehaviour {
    [SerializeField]
    private Image[] UpArrows;

    [SerializeField]
    private Image[] DownArrows;

    [SerializeField]
    private Text sensitivityNumberDisplay;

    private PauseScreen pauseScreen;
    private PlayerInput playerInput;
    private int selectedOption;
    private int maxNumberOfOptions;
    private int pauseScreenOwner;

    private bool verticalAxisLock;
    // Use this for initialization
    void Start () {
        pauseScreen = GameObject.Find("Manager").GetComponent<PauseScreen>();
        pauseScreenOwner = pauseScreen.GetPauseScreenOwner();
        playerInput = GameObject.Find("Manager").GetComponent<ActivePlayers>().GetPlayerInMatch(pauseScreenOwner).GetComponent<PlayerInput>();
        selectedOption = (int)playerInput.GetPlayerSensitivity() - 1;
        maxNumberOfOptions = 9;
        verticalAxisLock = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        pauseScreenOwner = pauseScreen.GetPauseScreenOwner();
        verticalControls();
        animateText();
    }

    private void verticalControls()
    {
        if (InputManager.UIVertical(pauseScreenOwner) > 0 && verticalAxisLock == false)
        {
            verticalAxisLock = true;
            UpArrows[0].gameObject.SetActive(true);
            selectedOption++;
            //Debug.Log("in vertical controls DOWN, selected option is " + selectedOption);
            if (selectedOption > maxNumberOfOptions)
            {
                selectedOption = maxNumberOfOptions;
            }
            playerInput.SetPlayerSensitivity(selectedOption + 1);
        }
        if (InputManager.UIVertical(pauseScreenOwner) < 0 && verticalAxisLock == false)
        {
            verticalAxisLock = true;
            DownArrows[0].gameObject.SetActive(true);
            selectedOption--;
            //Debug.Log("in vertical controls UP, selected option is " + selectedOption);
            if (selectedOption < 0)
            {
                selectedOption = 0;
            }
            playerInput.SetPlayerSensitivity(selectedOption + 1);
        }
        else if (InputManager.UIVertical(pauseScreenOwner) == 0)
        {
            verticalAxisLock = false;
            UpArrows[0].gameObject.SetActive(false);
            DownArrows[0].gameObject.SetActive(false);
        }
    }

    private void animateText()
    {
        sensitivityNumberDisplay.text = playerInput.GetPlayerSensitivity() + "";
    }

    private void toggleButtonImage(Image[] pImageArray, Image[] pAlternateArray, bool pState)
    {
        pImageArray[0].gameObject.SetActive(pState);
        pImageArray[1].gameObject.SetActive(!pState);
        pAlternateArray[0].gameObject.SetActive(!pState);
        pAlternateArray[1].gameObject.SetActive(pState);

    }
}
