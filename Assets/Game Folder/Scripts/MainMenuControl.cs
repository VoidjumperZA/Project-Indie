using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuControl : MonoBehaviour
{
    [SerializeField]
    private Canvas[] playMenuSubsections;

    [SerializeField]
    private Canvas[] MenuSections;

    [SerializeField]
    private GameObject pentagram;

    [SerializeField]
    private float pentagramRotationSpeed;

    [SerializeField]
    private GameObject[] visualButtons;

    private float pentagramRotationAngle;
    private float pentagramAngleToReach;
    private bool pentagramShouldRotate;
    private int currentSubsection;
    private int currentMenuSection;
    private int previousMenuSection;
    private bool dismissSplashAxisLock;
    private AudioSource[] buttonSounds;
    // Use this for initialization
    void Start()
    {
        currentSubsection = 0;
        currentMenuSection = 0;
        dismissSplashAxisLock = false;
        pentagramShouldRotate = false;
        pentagramAngleToReach = 360 / playMenuSubsections.Length;
        buttonSounds = GameObject.Find("Button Sounds Audio Source").GetComponents<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        listenForPressToBeginPrompt();
        if (pentagramShouldRotate == true)
        {
            animateWheel();
        }
    }

    private void animateWheel()
    {
        pentagram.transform.Rotate(0, 0, pentagramRotationSpeed);
        pentagramRotationAngle += pentagramRotationSpeed;

        for (int i = 0; i < visualButtons.Length; i++)
        {
            visualButtons[i].transform.Rotate(0, 0, -pentagramRotationSpeed);
        }

        if (pentagramRotationAngle >= pentagramAngleToReach)
        {
            pentagramRotationAngle = 0.0f;
            pentagramShouldRotate = false;
        }    
    }

    public void QuickTest()
    {

    }

    public void SetTeam_1PlayerCount(int pTeam1Count)
    {
        Debug.Log("Team 1 Player Count is now " + pTeam1Count);
        LobbySettings.SetTeamPlayerCount(1, pTeam1Count);
        Debug.Log("Total Number of Players is: " + LobbySettings.GetNumberOfPlayers());
    }

    public void SetTeam_2PlayerCount(int pTeam2Count)
    {
        Debug.Log("Team 2 Player Count is now " + pTeam2Count);
        LobbySettings.SetTeamPlayerCount(2, pTeam2Count);
        Debug.Log("Total Number of Players is: " + LobbySettings.GetNumberOfPlayers());
    }

    public void ResetNumberOfPlayers()
    {
        LobbySettings.ResetNumberOfPlayers();
    }

    public void moveToNextSubsection(int pMenuLevel)
    {
        currentSubsection = pMenuLevel;
        playMenuSubsections[pMenuLevel].gameObject.SetActive(true);
        playMenuSubsections[pMenuLevel - 1].gameObject.SetActive(false);

        pentagramRotationAngle = 0.0f;
        pentagramShouldRotate = true;
    }

    public void ReturnToPreviousSubsection()
    {
        currentSubsection--;
        if (currentSubsection < 0)
        {
            currentSubsection = 0;
            MoveToNextSection(0);
        }
        else
        {     
            playMenuSubsections[currentSubsection].gameObject.SetActive(false);
            playMenuSubsections[currentSubsection - 1].gameObject.SetActive(true);

            pentagramRotationAngle = 0.0f;
            pentagramShouldRotate = true;
        }
    }


    public void MoveToNextSection(int pNextSection)
    {
        //if we go to custom game options
        if (pNextSection == 2)
        {
            MenuSections[currentMenuSection].gameObject.SetActive(false);
            playMenuSubsections[currentSubsection].gameObject.SetActive(true);
        }
        MenuSections[currentMenuSection].gameObject.SetActive(false);
        MenuSections[pNextSection].gameObject.SetActive(true);
        previousMenuSection = currentMenuSection;
        currentMenuSection = pNextSection;
    }

    public void ReturnToPreviousMenuSection()
    {
        MenuSections[currentMenuSection].gameObject.SetActive(false);
        MenuSections[previousMenuSection].gameObject.SetActive(true);
        int temp = previousMenuSection;
        previousMenuSection = currentMenuSection;
        currentMenuSection = temp;
    }

    public void SetArenaName(string pArenaName)
    {
        LobbySettings.SetArena(pArenaName);
    }

    public void SetCooldownModifier(float pModifier)
    {
        LobbySettings.SetCooldownModifier(pModifier);
    }

    public void SetCooldownModifierInverse(float pInverse)
    {
        LobbySettings.SetCooldownModifierInverse(pInverse);
    }

    public void SetMatchTimeInMinutes(int pDuration)
    {
        LobbySettings.SetMatchTimeInMinutes(pDuration);
        Debug.Log("MainMenuControl giving a value of " + pDuration + " to LobbySettings.");
    }

    public void SetGoalsToWin(int pGoalsToWin)
    {
        LobbySettings.SetGoalsToWin(pGoalsToWin);
    }

    public void UsePossessedHexes(bool pState)
    {
        LobbySettings.UsePossessedHexes(pState);
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(LobbySettings.GetSceneToLoad());
    }

    public void ReloadMainMenu()
    {

    }

    public void PlayHoverSound()
    {
        buttonSounds[0].Play();
    }

    public void PlayClickSound()
    {
        buttonSounds[1].Play();
    }

    private void listenForPressToBeginPrompt()
    {
        if (InputManager.AcceptButton(1) > 0 && dismissSplashAxisLock == false && currentMenuSection == 0)
        {
            dismissSplashAxisLock = true;
            PlayClickSound();
            MoveToNextSection(1);
        }
        if (InputManager.AcceptButton(1) == 0)
        {
            dismissSplashAxisLock = false;
        }
    }

    public void SelectQuickPlay()
    {
        SetTeam_1PlayerCount(1);
        SetTeam_2PlayerCount(1);
        string[] arenaNames = { "Sunlight", "Moonbeam" };
        SetArenaName(arenaNames[Random.Range(0, arenaNames.Length)]);
        SetCooldownModifier(1);
        SetCooldownModifierInverse(1);
        SetMatchTimeInMinutes(5);
        SetGoalsToWin(5);
        UsePossessedHexes(false);
        LoadScene();
    }

}
