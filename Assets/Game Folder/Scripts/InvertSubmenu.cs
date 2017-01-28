using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvertSubmenu : MonoBehaviour
{
    [SerializeField]
    private Image[] OnSelectButtons;

    [SerializeField]
    private Image[] OffSelectButtons;

    [SerializeField]
    private Image[] OnDisplayIcons;

    [SerializeField]
    private Image[] OffDisplayIcons;

    private PauseScreen pauseScreen;
    private PlayerInput playerInput;
    private int selectedOption;
    private int maxNumberOfOptions;
    private int pauseScreenOwner;

    private bool verticalAxisLock;
    private bool acceptAxisLock;

    // Use this for initialization
    void Start ()
    {
        pauseScreen = GameObject.Find("Manager").GetComponent<PauseScreen>();
        pauseScreenOwner = pauseScreen.GetPauseScreenOwner();
        playerInput = GameObject.Find("Manager").GetComponent<ActivePlayers>().GetPlayerInMatch(pauseScreenOwner).GetComponent<PlayerInput>();
        selectedOption = 0;
        maxNumberOfOptions = 1;
        verticalAxisLock = false;
        acceptAxisLock = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        verticalControls();
        acceptControls();
        animateButtons();
       // Debug.Log("selected option is " + selectedOption);
    }

    private void verticalControls()
    {
        if (InputManager.MovementVertical(pauseScreenOwner) < 0 && verticalAxisLock == false)
        {
            verticalAxisLock = true;
            selectedOption++;
            //Debug.Log("in vertical controls DOWN, selected option is " + selectedOption);
            if (selectedOption > maxNumberOfOptions)
            {
                selectedOption = 0;
            }
        }
        if (InputManager.MovementVertical(pauseScreenOwner) > 0 && verticalAxisLock == false)
        {
            verticalAxisLock = true;
            selectedOption--;
            //Debug.Log("in vertical controls UP, selected option is " + selectedOption);
            if (selectedOption < 0)
            {
                selectedOption = maxNumberOfOptions;
            }
        }
        else if (InputManager.MovementVertical(pauseScreenOwner) == 0)
        {
            verticalAxisLock = false;
        }
    }

    private void acceptControls()
    {
        if (InputManager.AcceptButton(pauseScreenOwner) > 0 && acceptAxisLock == false)
        {
            Debug.Log("Pressed Enter");
            acceptAxisLock = true;
            if (selectedOption == 0)
            {
                playerInput.ExecuteInverse(true);
                Debug.Log("invert toggled");
            }
            else
            {
                playerInput.ExecuteInverse(false);
                Debug.Log("invert toggled");
            }
        }
        else if (InputManager.AcceptButton(pauseScreenOwner) == 0)
        {
            acceptAxisLock = false;
        }
    }

    private void animateButtons()
    {
        if (selectedOption == 0)
        {
            toggleButtonImage(OnSelectButtons, OffSelectButtons, true);
        }
        else if (selectedOption == 1)
        {
            toggleButtonImage(OnSelectButtons, OffSelectButtons, false);
        }

        if (playerInput.GetInverseState() == 1)
        {            
            toggleButtonImage(OnDisplayIcons, OffDisplayIcons, false);
        }
        else
        {
            toggleButtonImage(OnDisplayIcons, OffDisplayIcons, true);
        }
    }

    private void toggleButtonImage(Image[] pImageArray, Image[] pAlternateArray, bool pState)
    {
            pImageArray[0].gameObject.SetActive(pState);
            pImageArray[1].gameObject.SetActive(!pState);
            pAlternateArray[0].gameObject.SetActive(!pState);
            pAlternateArray[1].gameObject.SetActive(pState);

    }
}
