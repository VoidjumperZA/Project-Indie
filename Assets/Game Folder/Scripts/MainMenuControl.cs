using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuControl : MonoBehaviour
{
    [SerializeField]
    private Canvas[] playMenuSubsections;

    [SerializeField]
    private GameObject pentagram;

    [SerializeField]
    private float pentagramRotationSpeed;

    private float pentagramRotationAngle;
    private float pentagramAngleToReach;
    private bool pentagramShouldRotate;
    // Use this for initialization
    void Start()
    {
        pentagramShouldRotate = false;
        pentagramAngleToReach = 360 / playMenuSubsections.Length;
    }

    // Update is called once per frame
    void Update()
    {
        if (pentagramShouldRotate == true)
        {
            pentagram.transform.Rotate(0, 0, pentagramRotationSpeed);
            pentagramRotationAngle += pentagramRotationSpeed;

            if (pentagramRotationAngle >= pentagramAngleToReach)
            {
                pentagramRotationAngle = 0.0f;
                pentagramShouldRotate = false;
            }
        }
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
        playMenuSubsections[pMenuLevel].gameObject.SetActive(true);
        playMenuSubsections[pMenuLevel - 1].gameObject.SetActive(false);

        pentagramRotationAngle = 0.0f;
        pentagramShouldRotate = true;
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
}
