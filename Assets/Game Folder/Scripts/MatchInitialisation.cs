using UnityEngine;
using System.Collections;

public class MatchInitialisation : MonoBehaviour
{
    [SerializeField]
    private Camera[] gameCameras;

    private int teamID = 1;
    private Vector3[] raycastPositions = new Vector3[4];
    // Use this for initialization
    void Awake()
    {
        MatchStatistics.IntialiseGoalTracking();
        Cursor.visible = false;

        setCameraDimensions();
        assignPlayersToTeams();
        assignPlayerIDsAndRaycasts();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void setCameraDimensions()
    {
        //-------------------------------------------
        //                  CAMERA 0
        //-------------------------------------------

        // 1v1 | 1v2  -  Top Half
        if (LobbySettings.GetTeam_1PlayerCount() == 1 && LobbySettings.GetTeam_2PlayerCount() < 3)
        {
            gameCameras[0].rect = new Rect(0, 0.5f, 1, 0.5f);
            raycastPositions[0] = new Vector3(Screen.width * 0.5f, Screen.height * 0.75f, 0.0f);
            Debug.Log("Setting Camera 0 for 1v1 | 1v2 - Top Half");
        }
        // 1v3 | 2v2  -  Top Left
        else
        {
            gameCameras[0].rect = new Rect(0, 0.5f, 0.5f, 0.5f);
            raycastPositions[0] = new Vector3(Screen.width * 0.25f, Screen.height * 0.75f, 0.0f);
            Debug.Log("Setting Camera 0 for 1v3 | 2v2 - Top Left, screen width is " + Screen.width + " and screen height is " + Screen.height);
        }

        //-------------------------------------------
        //                  CAMERA 1
        //-------------------------------------------

        // 1v1  -  Bottom Half
        if (LobbySettings.GetTeam_1PlayerCount()  == 1 && LobbySettings.GetTeam_2PlayerCount() == 1)
        {
            gameCameras[1].rect = new Rect(0, 0, 1, 0.5f);
            raycastPositions[1] = new Vector3(Screen.width * 0.5f, Screen.height * 0.25f, 0.0f);
            Debug.Log("Setting Camera 1 for 1v1 - Bottom Half");
        }

        // 1v2  -  Bottom Left
        if (LobbySettings.GetTeam_1PlayerCount() == 1 && LobbySettings.GetTeam_2PlayerCount() == 2)
        {
            gameCameras[1].rect = new Rect(0, 0, 0.5f, 0.5f);
            raycastPositions[1] = new Vector3(Screen.width * 0.25f, Screen.height * 0.25f, 0.0f);
            Debug.Log("Setting Camera 1 for 1v2 - Bottom Left");
        }

        // 1v3  -  Top Right
        if (LobbySettings.GetTeam_1PlayerCount() == 1 && LobbySettings.GetTeam_2PlayerCount() == 3)
        {
            gameCameras[1].rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
            raycastPositions[1] = new Vector3(Screen.width * 0.75f, Screen.height * 0.75f, 0.0f);
            Debug.Log("Setting Camera 1 for 1v3 - Top Right");
        }

        // 2v2  -  Top Right
        if (LobbySettings.GetTeam_1PlayerCount() == 2 && LobbySettings.GetTeam_2PlayerCount() == 2)
        {
            gameCameras[1].rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
            raycastPositions[1] = new Vector3(Screen.width * 0.75f, Screen.height * 0.75f, 0.0f);
            Debug.Log("Setting Camera 1 for 2v2 - Top Right");
        }

        //-------------------------------------------
        //                  CAMERA 2
        //-------------------------------------------

        // 1v1  -  Not Displaying
        if (LobbySettings.GetTeam_1PlayerCount() == 1 && LobbySettings.GetTeam_2PlayerCount() == 1)
        {
            gameCameras[2].rect = new Rect(0, 0, 0, 0);
            raycastPositions[2] = new Vector3(0, 0, 0.0f);
            Debug.Log("Setting Camera 2 for 1v1 - Not Displaying");
        }

        // 1v2  -  Bottom Right
        if (LobbySettings.GetTeam_1PlayerCount() == 1 && LobbySettings.GetTeam_2PlayerCount() == 2)
        {
            gameCameras[2].rect = new Rect(0.5f, 0, 0.5f, 0.5f);
            raycastPositions[2] = new Vector3(Screen.width * 0.75f, Screen.height * 0.25f, 0.0f);
            Debug.Log("Setting Camera 2 for 1v2 - Bottom Right");
        }

        // 1v3  -  Bottom Left
        if (LobbySettings.GetTeam_1PlayerCount() == 1 && LobbySettings.GetTeam_2PlayerCount() == 3)
        {
            gameCameras[2].rect = new Rect(0, 0, 0.5f, 0.5f);
            raycastPositions[2] = new Vector3(Screen.width * 0.25f, Screen.height * 0.25f, 0.0f);
            Debug.Log("Setting Camera 2 for 1v3 - Bottom Left");
        }

        // 2v2  -  Bottom Left
        if (LobbySettings.GetTeam_1PlayerCount() == 2 && LobbySettings.GetTeam_2PlayerCount() == 2)
        {
            gameCameras[2].rect = new Rect(0, 0, 0.5f, 0.5f);
            raycastPositions[2] = new Vector3(Screen.width * 0.25f, Screen.height * 0.25f, 0.0f);
            Debug.Log("Setting Camera 2 for 2v2 - Bottom Left");
        }

        //-------------------------------------------
        //                  CAMERA 3
        //-------------------------------------------

        // 1v1  -  Not Displaying
        if (LobbySettings.GetTeam_1PlayerCount() == 1 && LobbySettings.GetTeam_2PlayerCount() == 1)
        {
            gameCameras[3].rect = new Rect(0, 0, 0, 0);
            raycastPositions[3] = new Vector3(0, 0, 0.0f);
            Debug.Log("Setting Camera 3 for 1v1 - Not Displaying");
        }

        // 1v2  -  Not Displaying
        if (LobbySettings.GetTeam_1PlayerCount() == 1 && LobbySettings.GetTeam_2PlayerCount() == 2)
        {
            gameCameras[3].rect = new Rect(0, 0, 0, 0);
            raycastPositions[3] = new Vector3(0, 0, 0.0f);
            Debug.Log("Setting Camera 3 for 1v2 - Not Displaying");
        }

        // 1v3  -  Bottom Right
        if (LobbySettings.GetTeam_1PlayerCount() == 1 && LobbySettings.GetTeam_2PlayerCount() == 3)
        {
            gameCameras[3].rect = new Rect(0.5f, 0, 0.5f, 0.5f);
            raycastPositions[3] = new Vector3(Screen.width * 0.75f, Screen.height * 0.25f, 0.0f);
            Debug.Log("Setting Camera 3 for 1v3 - Bottom Right");
        }

        // 2v2  -  Bottom Right
        if (LobbySettings.GetTeam_1PlayerCount() == 2 && LobbySettings.GetTeam_2PlayerCount() == 2)
        {
            gameCameras[3].rect = new Rect(0.5f, 0, 0.5f, 0.5f);
            raycastPositions[3] = new Vector3(Screen.width * 0.75f, Screen.height * 0.25f, 0.0f);
            Debug.Log("Setting Camera 3 for 2v2 - Bottom Right");
        }
    }

    private void assignPlayersToTeams()
    {
        for (int i = 0; i < LobbySettings.GetNumberOfPlayers(); i++)
        {
            if (i + 1 <= LobbySettings.GetTeam_1PlayerCount())
            {
                teamID = 1;
            }
            else
            {
                teamID = 2;
            }
            MatchStatistics.AssignPlayerToTeam(i + 1, teamID);
            Debug.Log("Assigning ID " + (i + 1) + " to Team " + teamID + ".");
        }
    }

    private void assignPlayerIDsAndRaycasts()
    {
        string tagName = "Player_";
        Debug.Log("In AssignPlayerIDandRaycast.");       
        for (int i = 0; i < LobbySettings.GetNumberOfPlayers(); i++)
        {
            PlayerInput currentPlayerInput = GameObject.FindGameObjectWithTag("" + tagName + (i + 1).ToString()).GetComponent<PlayerInput>();
            Debug.Log("Assigning object with tag '" + tagName + (i + 1) + "' the ID of " + (i + 1));
            currentPlayerInput.AssignPlayerID(i + 1);
            currentPlayerInput.SetRaycastPosition(raycastPositions[i]);
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
