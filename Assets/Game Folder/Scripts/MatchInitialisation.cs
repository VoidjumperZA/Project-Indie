using UnityEngine;
using System.Collections;

public class MatchInitialisation : MonoBehaviour
{
    [SerializeField]
    private Camera[] gameCameras;

    private int teamThreshold = 1;
    // Use this for initialization
    void Start()
    {
        MatchStatistics.IntialiseGoalTracking();
        Cursor.visible = false;

        setCameraDimensions();
        assignPlayersToTeams();
        assignPlayerIDs();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void setCameraDimensions()
    {
        //If Team 1 has 1 player and Team 2 has either 1 or 2 players, but not 3
        // 1v1, 1v2
        if (LobbySettings.GetTeam_1PlayerCount() == 1 && LobbySettings.GetNumberOfPlayers() < 4)
        {
            //T1: 1v1, 1v2
            gameCameras[0].rect.Set(0, 0.5f, 1, 0.5f);
            GameObject.FindGameObjectWithTag("Player_1").GetComponent<PlayerInput>().SetRaycastPosition(new Vector3(Screen.width * 0.5f, Screen.height * 0.25f, 0.0f));
            //T1: 1v1
            if (LobbySettings.GetTeam_2PlayerCount() == 1)
            {
                gameCameras[1].rect.Set(0, 0, 1, 0.5f);
                GameObject.FindGameObjectWithTag("Player_2").GetComponent<PlayerInput>().SetRaycastPosition(new Vector3(Screen.width * 0.5f, Screen.height * 0.75f, 0.0f));
            }
            //T1: 1v2
            else
            {
                gameCameras[1].rect.Set(0, 0, 0.5f, 0.5f);
                GameObject.FindGameObjectWithTag("Player_2").GetComponent<PlayerInput>().SetRaycastPosition(new Vector3(Screen.width * 0.25f, Screen.height * 0.75f, 0.0f));

                gameCameras[2].rect.Set(0.5f, 0, 0.5f, 0.5f);
                GameObject.FindGameObjectWithTag("Player_3").GetComponent<PlayerInput>().SetRaycastPosition(new Vector3(Screen.width * 0.75f, Screen.height * 0.75f, 0.0f));
            }
        }

        //new Vector3(Screen.width * 0.5f, Screen.height * 0.25f, 0.0f);

        //If Team 1 has 1 payer while Team 2 has 3 players
        // 1v3
        if (LobbySettings.GetTeam_1PlayerCount() == 1 && LobbySettings.GetNumberOfPlayers() == 4)
        {
            gameCameras[0].rect.Set(0, 0.5f, 0.5f, 0.5f);
            gameCameras[1].rect.Set(0.5f, 0.5f, 1, 0.5f);
            gameCameras[2].rect.Set(0, 0, 0.5f, 0.5f);
            gameCameras[3].rect.Set(0.5f, 0, 0.5f, 0.5f);

            GameObject.FindGameObjectWithTag("Player_1").GetComponent<PlayerInput>().SetRaycastPosition(new Vector3(Screen.width * 0.25f, Screen.height * 0.25f, 0.0f));
            GameObject.FindGameObjectWithTag("Player_2").GetComponent<PlayerInput>().SetRaycastPosition(new Vector3(Screen.width * 0.75f, Screen.height * 0.25f, 0.0f));
            GameObject.FindGameObjectWithTag("Player_3").GetComponent<PlayerInput>().SetRaycastPosition(new Vector3(Screen.width * 0.25f, Screen.height * 0.75f, 0.0f));
            GameObject.FindGameObjectWithTag("Player_4").GetComponent<PlayerInput>().SetRaycastPosition(new Vector3(Screen.width * 0.75f, Screen.height * 0.75f, 0.0f));
        }

        //If Team 1 has 2 payers while Team 2 also has 2 players
        // 2v2
        if (LobbySettings.GetTeam_1PlayerCount() == 1 && LobbySettings.GetNumberOfPlayers() == 4)
        {
            gameCameras[0].rect.Set(0, 0.5f, 0.5f, 0.5f);
            gameCameras[1].rect.Set(0.5f, 0.5f, 1, 0.5f);
            gameCameras[2].rect.Set(0, 0, 0.5f, 0.5f);
            gameCameras[3].rect.Set(0.5f, 0, 0.5f, 0.5f);

            GameObject.FindGameObjectWithTag("Player_1").GetComponent<PlayerInput>().SetRaycastPosition(new Vector3(Screen.width * 0.25f, Screen.height * 0.25f, 0.0f));
            GameObject.FindGameObjectWithTag("Player_2").GetComponent<PlayerInput>().SetRaycastPosition(new Vector3(Screen.width * 0.75f, Screen.height * 0.25f, 0.0f));
            GameObject.FindGameObjectWithTag("Player_3").GetComponent<PlayerInput>().SetRaycastPosition(new Vector3(Screen.width * 0.25f, Screen.height * 0.75f, 0.0f));
            GameObject.FindGameObjectWithTag("Player_4").GetComponent<PlayerInput>().SetRaycastPosition(new Vector3(Screen.width * 0.75f, Screen.height * 0.75f, 0.0f));
        }
    }

    private void assignPlayersToTeams()
    {
        for (int i = 0; i < LobbySettings.GetNumberOfPlayers(); i++)
        {
            MatchStatistics.AssignPlayerToTeam(i + 1, teamThreshold);
            if (i + 1 >= LobbySettings.GetTeam_1PlayerCount() && teamThreshold == 1)
            {
                teamThreshold += 1;
            }
            Debug.Log("Assigning ID " + (i + 1) + " to Team " + teamThreshold + ".");
        }
    }

    private void assignPlayerIDs()
    {
        string tagName = "Player_";
        for (int i = 0; i < LobbySettings.GetNumberOfPlayers(); i++)
        {
            GameObject.FindGameObjectWithTag("" + tagName + (i + 1)).GetComponent<PlayerInput>().AssignPlayerID(i + 1);
            Debug.Log("Assigning object with tag '" + tagName + (i + 1) + "' the ID of " + (i + 1));
        }
    }


    /// <summary>
    /// Set a camera to use the entire screen based on ID.
    /// </summary>
    /// <param name="pCamID"></param>
    public void SetFullscreenCam(int pCamID)
    {
        gameCameras[pCamID].rect.Set(0, 0, 1, 1);
    }

    /// <summary>
    /// Set a given camera to use the entire screen.
    /// </summary>
    /// <param name="pCam"></param>
    public void SetFullscreenCam(Camera pCam)
    {
        pCam.rect.Set(0, 0, 1, 1);
    }
}
