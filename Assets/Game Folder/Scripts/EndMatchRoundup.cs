using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndMatchRoundup : MonoBehaviour
{
    [SerializeField]
    private GameObject[] winPositions;

    private PlayerInput[] playerInputs;
    private ActivePlayers activePlayers;
    // Use this for initialization
    void Start()
    {
        activePlayers = GameObject.Find("Manager").GetComponent<ActivePlayers>();
        for (int i = 0; i < activePlayers.GetPlayersInMatchArraySize(); i++)
        {
            playerInputs[i] = activePlayers.GetPlayerInMatch(i).GetComponent<PlayerInput>();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DisplayPodium(int pWinningTeam)
    {
        //
        for (int i = 0; i < playerInputs.Length; i++)
        {
            playerInputs[i].SetControlsDisabled(true);
        }

        //
        int numberOfPlayersToPosition;
        if (pWinningTeam == 1)
        {
            numberOfPlayersToPosition = LobbySettings.GetTeam_1PlayerCount();
        }
        else
        {
            numberOfPlayersToPosition = LobbySettings.GetTeam_2PlayerCount();
        }

        //
        for (int i = 0; i < activePlayers.GetPlayersInMatchArraySize(); i++)
        {
            if (MatchStatistics.GetTeamIDofPlayer(i) == pWinningTeam) 
            {
                activePlayers.GetPlayerInMatch(i).transform.position = winPositions[i].transform.position;
                activePlayers.GetPlayerInMatch(i).transform.rotation = winPositions[i].transform.rotation;
            }
        }
    }
}
