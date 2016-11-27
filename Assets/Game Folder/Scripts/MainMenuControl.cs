using UnityEngine;
using System.Collections;

public class MainMenuControl : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
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
}
