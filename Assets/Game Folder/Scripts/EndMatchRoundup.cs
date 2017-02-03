using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class EndMatchRoundup : MonoBehaviour
{
    [SerializeField]
    private GameObject[] winPositions;

    [SerializeField]
    private GameObject cameraPosition;

    [SerializeField]
    private Camera gameCamera;

    private PlayerInput[] playerInputs;
    private ActivePlayers activePlayers;
    private MatchInitialisation matchInit;
    private PauseScreen pauseScreen;
    // Use this for initialization
    void Start()
    {
        activePlayers = GameObject.Find("Manager").GetComponent<ActivePlayers>();
        playerInputs = new PlayerInput[activePlayers.GetPlayersInMatchArraySize()];
        matchInit = GameObject.Find("Manager").GetComponent<MatchInitialisation>();
        pauseScreen = GameObject.Find("Manager").GetComponent<PauseScreen>();
        for (int i = 0; i < activePlayers.GetPlayersInMatchArraySize(); i++)
        {
            Debug.Log("Trying to get active Player " + i);
            playerInputs[i] = activePlayers.GetPlayerInMatch(i + 1).GetComponent<PlayerInput>();
        }

        //switch off their renderers
        for (int i = 0; i < winPositions.Length; i++)
        {
            winPositions[i].gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
        cameraPosition.gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DisplayPodium(int pWinningTeam)
    {
        //fade to black here


        //
        for (int i = 0; i < playerInputs.Length; i++)
        {
            Debug.Log("Trying to switch off Player " + i + " looping " + (playerInputs.Length - 1) + " though");
            playerInputs[i].SetControlsDisabled(true);
        }

        //
        int numberOfPlayersToPosition;
        if (pWinningTeam == 1)
        {
            numberOfPlayersToPosition = LobbySettings.GetTeam_1PlayerCount();
        }
        else if (pWinningTeam == 2)
        {
            numberOfPlayersToPosition = LobbySettings.GetTeam_2PlayerCount();
        }
        else
        {
            numberOfPlayersToPosition = activePlayers.GetPlayersInMatchArraySize();
        }

        //
        for (int i = 0; i < activePlayers.GetPlayersInMatchArraySize(); i++)
        {
            if (pWinningTeam != 0)
            {
                Debug.Log("Winning Team is " + pWinningTeam);
                Debug.Log("We are look at Player " + (i + 1) + " who has a TeamID of " + MatchStatistics.GetTeamIDofPlayer(i + 1));
                if (MatchStatistics.GetTeamIDofPlayer(i + 1) == pWinningTeam)
                {
                    activePlayers.GetPlayerInMatch(i + 1).transform.position = winPositions[i].transform.position;
                    activePlayers.GetPlayerInMatch(i + 1).transform.rotation = winPositions[i].transform.rotation;
                }
            }
            else
            {
                Debug.Log("Game is Draw");
                activePlayers.GetPlayerInMatch(i + 1).transform.position = winPositions[i].transform.position;
                activePlayers.GetPlayerInMatch(i + 1).transform.rotation = winPositions[i].transform.rotation;
            }

            
        }

        //grab the game camera, switch off it's blur, stop it rotating and set it to the camera pos and rot
        pauseScreen.toggleHUD(false);
        matchInit.ToggleFullscreenCam(gameCamera, true);
        gameCamera.gameObject.GetComponent<BlurOptimized>().enabled = false;
        gameCamera.GetComponentInParent<SimpleRotate>().enabled = false;
        gameCamera.transform.position = cameraPosition.transform.position;
        gameCamera.transform.rotation = cameraPosition.transform.rotation;
    }
}
