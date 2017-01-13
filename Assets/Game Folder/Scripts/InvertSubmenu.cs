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
    private int selectedOption;
    private int maxNumberOfOptions;

	// Use this for initialization
	void Start ()
    {
        pauseScreen = GameObject.Find("Manager").GetComponent<PauseScreen>();
        selectedOption = 0;
        maxNumberOfOptions = 1; 

    }
	
	// Update is called once per frame
	void Update ()
    {
        verticalControls();
        animateButtons();
    }

    private void verticalControls()
    {
        if (InputManager.MovementVertical(pauseScreen.GetPauseScreenOwner()) < 0)
        {
            selectedOption++;
            if (selectedOption > maxNumberOfOptions)
            {
                selectedOption = 0;
            }
        }
        if (InputManager.MovementVertical(pauseScreen.GetPauseScreenOwner()) > 0)
        {
            selectedOption--;
            if (selectedOption < 0)
            {
                selectedOption = maxNumberOfOptions;
            }
        }
    }

    private void animateButtons()
    {
        if (selectedOption == 0)
        {
            toggleButtonImage(OnSelectButtons, OffSelectButtons, true);
            toggleButtonImage(OnDisplayIcons, OffDisplayIcons, true);
        }
        else if (selectedOption == 1)
        {
            toggleButtonImage(OnSelectButtons, OffSelectButtons, false);
            toggleButtonImage(OnDisplayIcons, OffDisplayIcons, false);
        }
    }

    private void toggleButtonImage(Image[] pImageArray, Image[] pAlternateArray, bool pState)
    {
            pImageArray[0].enabled = pState;
            pImageArray[1].enabled = !pState;
            pAlternateArray[0].enabled = !pState;
            pAlternateArray[1].enabled = pState;

    }
}
