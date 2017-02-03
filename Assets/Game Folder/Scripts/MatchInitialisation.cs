using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MatchInitialisation : MonoBehaviour
{
    [SerializeField]
    private Camera[] gameCameras;

    [SerializeField]
    private Image blueCrosshair;

    [SerializeField]
    private Image redCrosshair;

    [SerializeField]
    private GameObject[] teamSpawns;

    private List<Image> allCrosshairs = new List<Image>();

    private string[] locationStrings = { "Top Half", "Bottom Half", "Top Left", "Bottom Left", "Top Right", "Bottom Right", "Not Displaying" };
    private static Dictionary<string, Rect> cameraDictionary = new Dictionary<string, Rect>();
    private static Dictionary<string, Vector3> raycastDictionary = new Dictionary<string, Vector3>();

    private int teamID = 1;
    private Vector3[] raycastPositions = new Vector3[5];
    // Use this for initialization
    void Awake()
    {
        MatchStatistics.IntialiseGoalTracking();
        Cursor.visible = false;

        assignCameraRectsToDictionary();
        assignRaycastVecsToDictionary();

        switchedPossessedHexesOn();

        setCameraDimensions();

        assignPlayersToTeams();

        setCorrectPlayerPositions();

        assignPlayerIDsAndRaycasts();
        passCrosshairsToUIManager();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //store a dictionary of camera positions, tied to a string describing their location
    private void assignCameraRectsToDictionary()
    {
        cameraDictionary.Add(locationStrings[0], new Rect(0, 0.5f, 1, 0.5f));
        cameraDictionary.Add(locationStrings[1], new Rect(0, 0, 1, 0.5f));
        cameraDictionary.Add(locationStrings[2], new Rect(0, 0.5f, 0.5f, 0.5f));
        cameraDictionary.Add(locationStrings[3], new Rect(0, 0, 0.5f, 0.5f));
        cameraDictionary.Add(locationStrings[4], new Rect(0.5f, 0.5f, 0.5f, 0.5f));
        cameraDictionary.Add(locationStrings[5], new Rect(0.5f, 0, 0.5f, 0.5f));
        cameraDictionary.Add(locationStrings[6], new Rect(0, 0, 0, 0));     
    }

    //store a dictionary of our raycast positions, tied to a string describing thier location
    private void assignRaycastVecsToDictionary()
    {
        raycastDictionary.Add(locationStrings[0], new Vector3(Screen.width * 0.5f, Screen.height * 0.75f, 0.0f));
        raycastDictionary.Add(locationStrings[1], new Vector3(Screen.width * 0.5f, Screen.height * 0.25f, 0.0f));
        raycastDictionary.Add(locationStrings[2], new Vector3(Screen.width * 0.25f, Screen.height * 0.75f, 0.0f));
        raycastDictionary.Add(locationStrings[3], new Vector3(Screen.width * 0.25f, Screen.height * 0.25f, 0.0f));
        raycastDictionary.Add(locationStrings[4], new Vector3(Screen.width * 0.75f, Screen.height * 0.75f, 0.0f));
        raycastDictionary.Add(locationStrings[5], new Vector3(Screen.width * 0.75f, Screen.height * 0.25f, 0.0f));
    }

    //simply enable the script attached to the manager which creates the PossessedHexes game mode: with randomly moving hex columns
    private void switchedPossessedHexesOn()
    {
        GameObject.Find("Manager").GetComponent<PossessedHexes>().enabled = LobbySettings.IsPossessedHexes();
    }
    
    //what we do, in a somewhat inelegant fashion possibly, is go through each of the five cameras in this scene (excluding camera 0: the overhead gamecam
    //and find the camera rect. We set it up with the size and width and position we want depending on the number of players and what game type it is
    private void setCameraDimensions()
    {
        string cameraConfiguration = "";
        int cameraToPosition = 0;

        cameraConfiguration = "Not Displaying";
        gameCameras[cameraToPosition].rect = ReturnDictionaryRectValue(cameraDictionary, cameraConfiguration);

        //---------------------------------------------------------------
        //                  CAMERA 1  -  (P1)TEAM 1: PLAYER 1
        //---------------------------------------------------------------

        cameraToPosition += 1;
        int team = 1;

        // 1v1 | 1v2  -  Top Half
        if (LobbySettings.GetTeam_1PlayerCount() == 1 && LobbySettings.GetTeam_2PlayerCount() < 3)
        {
            cameraConfiguration = "Top Half";

            gameCameras[cameraToPosition].rect = ReturnDictionaryRectValue(cameraDictionary, cameraConfiguration);
            raycastPositions[cameraToPosition - 1] = ReturnDictionaryVecValue(raycastDictionary, cameraConfiguration);
            setCrosshairPosition(team, raycastPositions[cameraToPosition - 1]);
            Debug.Log("Setting Camera " + cameraToPosition + " for 1v1 | 1v2 - " + cameraConfiguration);
        }
        // 1v3 | 2v2  -  Top Left        
        else
        {
            cameraConfiguration = "Top Left";

            gameCameras[cameraToPosition].rect = ReturnDictionaryRectValue(cameraDictionary, cameraConfiguration);
            raycastPositions[cameraToPosition - 1] = ReturnDictionaryVecValue(raycastDictionary, cameraConfiguration);
            setCrosshairPosition(team, raycastPositions[cameraToPosition - 1]);
            Debug.Log("Setting Camera " + cameraToPosition + " for 1v3 | 2v2 - " + cameraConfiguration);
        }

        //---------------------------------------------------------------
        //                  CAMERA 2  -  (P2)TEAM 1: PLAYER 2
        //---------------------------------------------------------------
        cameraToPosition += 1;
        team = 1;

        // 1v1  -  Not Displaying
        if (LobbySettings.GetTeam_1PlayerCount() == 1 && LobbySettings.GetTeam_2PlayerCount() == 1)
        {
            cameraConfiguration = "Not Displaying";

            gameCameras[cameraToPosition].rect = ReturnDictionaryRectValue(cameraDictionary, cameraConfiguration);
            raycastPositions[cameraToPosition - 1] = ReturnDictionaryVecValue(raycastDictionary, cameraConfiguration);
            Debug.Log("Setting Camera " + cameraToPosition + " for 1v1 - " + cameraConfiguration);
        }

        // 1v2  -  Not Displaying
        if (LobbySettings.GetTeam_1PlayerCount() == 1 && LobbySettings.GetTeam_2PlayerCount() == 2)
        {
            cameraConfiguration = "Not Displaying";

            gameCameras[cameraToPosition].rect = ReturnDictionaryRectValue(cameraDictionary, cameraConfiguration);
            raycastPositions[cameraToPosition - 1] = ReturnDictionaryVecValue(raycastDictionary, cameraConfiguration);
            Debug.Log("Setting Camera " + cameraToPosition + " for 1v2 - " + cameraConfiguration);
        }

        // 1v3  -  Not Displaying
        if (LobbySettings.GetTeam_1PlayerCount() == 1 && LobbySettings.GetTeam_2PlayerCount() == 3)
        {
            cameraConfiguration = "Not Displaying";

            gameCameras[cameraToPosition].rect = ReturnDictionaryRectValue(cameraDictionary, cameraConfiguration);
            raycastPositions[cameraToPosition - 1] = ReturnDictionaryVecValue(raycastDictionary, cameraConfiguration);
            Debug.Log("Setting Camera " + cameraToPosition + " for 1v3 - " + cameraConfiguration);
        }
        
        // 2v2  -  Top Right
        if (LobbySettings.GetTeam_1PlayerCount() == 2 && LobbySettings.GetTeam_2PlayerCount() == 2)
        {
            cameraConfiguration = "Top Right";

            gameCameras[cameraToPosition].rect = ReturnDictionaryRectValue(cameraDictionary, cameraConfiguration);
            raycastPositions[cameraToPosition - 1] = ReturnDictionaryVecValue(raycastDictionary, cameraConfiguration);
            setCrosshairPosition(team, raycastPositions[cameraToPosition - 1]);
            Debug.Log("Setting Camera " + cameraToPosition + " for 2v2 - " + cameraConfiguration);
        }

        //----------------------------------------------------------------------
        //                  CAMERA 3  -  (P3)TEAM 2: PLAYER 1
        //----------------------------------------------------------------------
        cameraToPosition += 1;
        team = 2;

        // 1v1  -  Bottom Half
        if (LobbySettings.GetTeam_1PlayerCount() == 1 && LobbySettings.GetTeam_2PlayerCount() == 1)
        {
            cameraConfiguration = "Bottom Half";

            gameCameras[cameraToPosition].rect = ReturnDictionaryRectValue(cameraDictionary, cameraConfiguration);
            raycastPositions[cameraToPosition - 1] = ReturnDictionaryVecValue(raycastDictionary, cameraConfiguration);
            setCrosshairPosition(team, raycastPositions[cameraToPosition - 1]);
            Debug.Log("Setting Camera " + cameraToPosition + " for 1v1 - " + cameraConfiguration);
        }

        // 1v2  -  Bottom Left
        if (LobbySettings.GetTeam_1PlayerCount() == 1 && LobbySettings.GetTeam_2PlayerCount() == 2)
        {
            cameraConfiguration = "Bottom Left";

            gameCameras[cameraToPosition].rect = ReturnDictionaryRectValue(cameraDictionary, cameraConfiguration);
            raycastPositions[cameraToPosition - 1] = ReturnDictionaryVecValue(raycastDictionary, cameraConfiguration);
            setCrosshairPosition(team, raycastPositions[cameraToPosition - 1]);
            Debug.Log("Setting Camera " + cameraToPosition + " for 1v2 - " + cameraConfiguration);
        }

        // 1v3  -  Top Right
        if (LobbySettings.GetTeam_1PlayerCount() == 1 && LobbySettings.GetTeam_2PlayerCount() == 3)
        {
            cameraConfiguration = "Top Right";

            gameCameras[cameraToPosition].rect = ReturnDictionaryRectValue(cameraDictionary, cameraConfiguration);
            raycastPositions[cameraToPosition - 1] = ReturnDictionaryVecValue(raycastDictionary, cameraConfiguration);
            setCrosshairPosition(team, raycastPositions[cameraToPosition - 1]);
            Debug.Log("Setting Camera " + cameraToPosition + " for 1v3 - " + cameraConfiguration);
        }

        // 2v2  -  Bottom Left
        if (LobbySettings.GetTeam_1PlayerCount() == 2 && LobbySettings.GetTeam_2PlayerCount() == 2)
        {
            cameraConfiguration = "Bottom Left";

            gameCameras[cameraToPosition].rect = ReturnDictionaryRectValue(cameraDictionary, cameraConfiguration);
            raycastPositions[cameraToPosition - 1] = ReturnDictionaryVecValue(raycastDictionary, cameraConfiguration);
            setCrosshairPosition(team, raycastPositions[cameraToPosition - 1]);
            Debug.Log("Setting Camera " + cameraToPosition + " for 2v2 - " + cameraConfiguration);
        }

        //-----------------------------------------------------------------------
        //                  CAMERA 4 -  (P4)TEAM 2: PLAYER 2
        //-----------------------------------------------------------------------
        cameraToPosition += 1;
        team = 2;

        // 1v1  -  Not Displaying
        if (LobbySettings.GetTeam_1PlayerCount() == 1 && LobbySettings.GetTeam_2PlayerCount() == 1)
        {
            cameraConfiguration = "Not Displaying";

            gameCameras[cameraToPosition].rect = ReturnDictionaryRectValue(cameraDictionary, cameraConfiguration);
            raycastPositions[cameraToPosition - 1] = ReturnDictionaryVecValue(raycastDictionary, cameraConfiguration);
            Debug.Log("Setting Camera " + cameraToPosition + " for 1v1 - " + cameraConfiguration);
        }

        // 1v2  -  Bottom Right
        if (LobbySettings.GetTeam_1PlayerCount() == 1 && LobbySettings.GetTeam_2PlayerCount() == 2)
        {
            cameraConfiguration = "Bottom Right";

            gameCameras[cameraToPosition].rect = ReturnDictionaryRectValue(cameraDictionary, cameraConfiguration);
            raycastPositions[cameraToPosition - 1] = ReturnDictionaryVecValue(raycastDictionary, cameraConfiguration);
            setCrosshairPosition(team, raycastPositions[cameraToPosition - 1]);
            Debug.Log("Setting Camera " + cameraToPosition + " for 1v2 - " + cameraConfiguration);
        }

        // 1v3  -  Bottom Left
        if (LobbySettings.GetTeam_1PlayerCount() == 1 && LobbySettings.GetTeam_2PlayerCount() == 3)
        {
            cameraConfiguration = "Bottom Left";

            gameCameras[cameraToPosition].rect = ReturnDictionaryRectValue(cameraDictionary, cameraConfiguration);
            raycastPositions[cameraToPosition - 1] = ReturnDictionaryVecValue(raycastDictionary, cameraConfiguration);
            setCrosshairPosition(team, raycastPositions[cameraToPosition - 1]);
            Debug.Log("Setting Camera " + cameraToPosition + " for 1v3 - " + cameraConfiguration);
        }

        // 2v2  -  Bottom Right
        if (LobbySettings.GetTeam_1PlayerCount() == 2 && LobbySettings.GetTeam_2PlayerCount() == 2)
        {
            cameraConfiguration = "Bottom Right";

            gameCameras[cameraToPosition].rect = ReturnDictionaryRectValue(cameraDictionary, cameraConfiguration);
            raycastPositions[cameraToPosition - 1] = ReturnDictionaryVecValue(raycastDictionary, cameraConfiguration);
            setCrosshairPosition(team, raycastPositions[cameraToPosition - 1]);
            Debug.Log("Setting Camera " + cameraToPosition + " for 2v2 - " + cameraConfiguration);
        }

        //------------------------------------------------------------------------
        //                  CAMERA 5  -  (P5)TEAM 2: PLAYER 3
        //------------------------------------------------------------------------
        cameraToPosition += 1;
        team = 2;

        // 1v1  -  Not Displaying
        if (LobbySettings.GetTeam_1PlayerCount() == 1 && LobbySettings.GetTeam_2PlayerCount() == 1)
        {
            cameraConfiguration = "Not Displaying";

            gameCameras[cameraToPosition].rect = ReturnDictionaryRectValue(cameraDictionary, cameraConfiguration);
            raycastPositions[cameraToPosition - 1] = ReturnDictionaryVecValue(raycastDictionary, cameraConfiguration);
            Debug.Log("Setting Camera " + cameraToPosition + " for 1v1 - " + cameraConfiguration);
        }

        // 1v2  -  Not Displaying
        if (LobbySettings.GetTeam_1PlayerCount() == 1 && LobbySettings.GetTeam_2PlayerCount() == 2)
        {
            cameraConfiguration = "Not Displaying";

            gameCameras[cameraToPosition].rect = ReturnDictionaryRectValue(cameraDictionary, cameraConfiguration);
            raycastPositions[cameraToPosition - 1] = ReturnDictionaryVecValue(raycastDictionary, cameraConfiguration);
            Debug.Log("Setting Camera " + cameraToPosition + " for 1v2 - " + cameraConfiguration);
        }

        // 1v3  -  Bottom Right
        if (LobbySettings.GetTeam_1PlayerCount() == 1 && LobbySettings.GetTeam_2PlayerCount() == 3)
        {
            cameraConfiguration = "Bottom Right";

            gameCameras[cameraToPosition].rect = ReturnDictionaryRectValue(cameraDictionary, cameraConfiguration);
            raycastPositions[cameraToPosition - 1] = ReturnDictionaryVecValue(raycastDictionary, cameraConfiguration);
            setCrosshairPosition(team, raycastPositions[cameraToPosition - 1]);
            Debug.Log("Setting Camera " + cameraToPosition + " for 1v3 - " + cameraConfiguration);
        }
        /*
        // 2v2  -  Bottom Right
        if (LobbySettings.GetTeam_1PlayerCount() == 2 && LobbySettings.GetTeam_2PlayerCount() == 2)
        {
            gameCameras[3].rect = new Rect(0.5f, 0, 0.5f, 0.5f);
            raycastPositions[3] = new Vector3(Screen.width * 0.75f, Screen.height * 0.25f, 0.0f);
            setCrosshairPosition(2, raycastPositions[3]);
            Debug.Log("Setting Camera 3 for 2v2 - Bottom Right");
        }*/


      
        //gameCameras[cameraToPosition].
    }

    //simply give the players a team ID
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

        //This is a little complex, what we're doing is trying to go through five possible player (Blue 1, Blue 2, Red 1, Red 2 and Red 3) based on the game type (1v1, 1v3, 2v2)
        //and trying to set the players (a total of 4) with an ID of 1-4. To do this we have to loop through the teams, trying to figure out where to start and using some
        //offsets for team two (red) to account for there being anywhere from 1 to 3 of them present

        //         0                          1
        for (int i = 0; i < LobbySettings.GetTeam_1PlayerCount(); i++)
        {
                                                                             // Player_      (  1  )
            PlayerInput currentPlayerInput = GameObject.FindGameObjectWithTag("" + tagName + (i + 1).ToString()).GetComponent<PlayerInput>();
            //                                            Player_1                          1
            Debug.Log("Assigning object with tag '" + tagName + (i + 1) + "' the ID of " + (i + 1));
            //                               (  1  )
            currentPlayerInput.AssignPlayerID(i + 1);
            //                                                     0
            currentPlayerInput.SetRaycastPosition(raycastPositions[i]);

            //add it to the activeplayer's list of players ACTUALLY in the match and enabled
            GameObject.Find("Manager").GetComponent<ActivePlayers>().AddPlayerInMatch(currentPlayerInput.gameObject, i + 1, i + 1);

        }

        //the offset accomodates for the team being anything from a 1v1 to a 2v2, to a 1v3
        int indexOffset = 0;
        if(LobbySettings.GetTeam_1PlayerCount() == 2)
        {
            indexOffset = 1;
        }

        //                         2                                           4
        for (int i = LobbySettings.GetTeam_1PlayerCount(); i < LobbySettings.GetNumberOfPlayers(); i++)
        {
            //                                                                     Player_   (  3  )
            PlayerInput currentPlayerInput = GameObject.FindGameObjectWithTag("" + tagName + (i + 2 - indexOffset).ToString()).GetComponent<PlayerInput>();
            //                                                  (  3  )                       2
            Debug.Log("Assigning object with tag '" + tagName + (i + 2 - indexOffset) + "' the ID of " + (i + 1));
            //                                  2 
            currentPlayerInput.AssignPlayerID(i + 1);
            //                                                     1
            currentPlayerInput.SetRaycastPosition(raycastPositions[i + 1 - indexOffset]);

            //add it to the activeplayer's list of players ACTUALLY in the match and enabled
            GameObject.Find("Manager").GetComponent<ActivePlayers>().AddPlayerInMatch(currentPlayerInput.gameObject, i + 1, (i + 2 - indexOffset));
        }
        
    }

    private void setCrosshairPosition(int pTeamID, Vector3 pPosition)
    {
        Image pNewCrosshair;
        if (pTeamID == 1)
        {
            pNewCrosshair = Instantiate(blueCrosshair);
            Debug.Log("Instantiating a blue crosshair at position: " + pNewCrosshair.transform.position);
        }
        else
        {
            pNewCrosshair = Instantiate(redCrosshair);
            Debug.Log("Instantiating a red crosshair at position: " + pNewCrosshair.transform.position);
        }
        pNewCrosshair.transform.position = pPosition;
        Debug.Log("Crosshair has been repositioned to: " + pNewCrosshair.transform.position);
        pNewCrosshair.transform.SetParent(GameObject.Find("Canvas").transform);
        Debug.Log("Crosshair has been parented and now exists at: " + pNewCrosshair.transform.position);
        allCrosshairs.Add(pNewCrosshair);
        Debug.Log("allCrosshairs now has " + allCrosshairs.Count + " crosshairs.");
    }

    private void passCrosshairsToUIManager()
    {
        GameObject.Find("Manager").GetComponent<PauseScreen>().SetCrosshairs(allCrosshairs.ToArray());
    }

    //activates characters and sets their positions correctly
    private void setCorrectPlayerPositions()
    {
        int teamOneMembersLeftToSpawn = LobbySettings.GetTeam_1PlayerCount();
        int teamTwoMembersLeftToSpawn = LobbySettings.GetTeam_2PlayerCount();

        for (int i = 0; i < LobbySettings.GetTeam_1PlayerCount(); i++)
        {
            //Activate the player
            Debug.Log("There are " + LobbySettings.GetTeam_1PlayerCount() + " players on blue team, and we're activating ActivePlayer element " + i);
            GameObject player = GameObject.Find("Manager").GetComponent<ActivePlayers>().GetActivePlayerElement(i);
            player.gameObject.SetActive(true);
            //player.transform.position = new Vector3(teamSpawns[LobbySettings.Get);

        }

        for (int i = 0; i < LobbySettings.GetTeam_2PlayerCount(); i++)
        {
            //Activate the player
            Debug.Log("There are " + LobbySettings.GetTeam_2PlayerCount() + " players on red team, and we're activating ActivePlayer element " + (i + 2));
            GameObject player = GameObject.Find("Manager").GetComponent<ActivePlayers>().GetActivePlayerElement(i + 2); //plus 2 for max two blue members
            player.gameObject.SetActive(true);
            //player.transform.position = new Vector3(teamSpawns[LobbySettings.Get);

        }
    }

    private Rect ReturnDictionaryRectValue(Dictionary<string, Rect> pDictionary, string pKey)
    {
        Rect value = new Rect();
        pDictionary.TryGetValue(pKey, out value);
        return value;
    }

    private Vector3 ReturnDictionaryVecValue(Dictionary<string, Vector3> pDictionary, string pKey)
    {
        Vector3 value = new Vector3();
        pDictionary.TryGetValue(pKey, out value);
        return value;
    }


    /// <summary>
    /// Set a camera to use the entire screen based on ID.
    /// </summary>
    /// <param name="pCamID"></param>
    public void ToggleFullscreenCam(int pCamID, bool pState)
    {
        for (int i = 0; i < gameCameras.Length; i++)
        {
            gameCameras[i].enabled = !pState;
        }
        if (pState == true)
        {
            gameCameras[pCamID].enabled = pState;
            gameCameras[pCamID].rect = new Rect(0, 0, 1, 1);
        }
        else
        {
            gameCameras[pCamID].enabled = pState;
            gameCameras[pCamID].rect = new Rect(0, 0, 0, 0);
        }
    }

    /// <summary>
    /// Set a given camera to use the entire screen.
    /// </summary>
    /// <param name="pCam"></param>
    public void ToggleFullscreenCam(Camera pCam, bool pState)
    {
       
        for (int i = 0; i < gameCameras.Length; i++)
        {
            gameCameras[i].enabled = !pState;
        }
        if (pState == true)
        {
            Debug.Log("Toggling on");
            pCam.enabled = pState;
            pCam.rect = new Rect(0, 0, 1, 1);        
        }
        else
        {
            pCam.enabled = pState;
            pCam.rect = new Rect(0, 0, 0, 0);
        }
    }

    /// <summary>
    /// Grabs a camera from the manager, listed as PlayerID - 1. Element 5 is the Game Cam.
    /// </summary>
    /// <param name="pCamID"></param>
    /// <returns></returns>
    public Camera GetGameCamera(int pCamID)
    {
        return gameCameras[pCamID];
    }
}
