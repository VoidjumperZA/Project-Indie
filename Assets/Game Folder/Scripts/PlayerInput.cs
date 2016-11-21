using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerInput : MonoBehaviour
{
    //Mandatory
    [SerializeField]
    private Camera playerCamera;
    [SerializeField]
    private GameObject columnControlManager;
    [SerializeField]
    private Image crosshairs;

    private delegate void InputUpdate();
    private event InputUpdate _update;

    private PlayerMovement _movement;
    private ColumnControl columnControl;
    private PlayerCamera _cameraScript;

    private GameObject _selectedColumn;
    private ColumnProperties _columnProperties;

    private int playerID;
    private int temp_TeamID;
    private Vector3 _raycastPos;

    private bool flashAxisLock = false;
    private bool throwAxisLock = false;
    private bool raiseAxisLock = false;
    private bool lowerAxisLock = false;

    private void Start()
    {
        _update += mouseHandler;
        _update += raycastingColumn;
        _update += raiseLowerCheck;
        _update += flashCheck;
        _update += throwCheck;
        _update += movementHandler;

        _movement = GetComponent<PlayerMovement>();
        _cameraScript = playerCamera.GetComponent<PlayerCamera>();
        columnControl = columnControlManager.GetComponent<ColumnControl>();

        //assign the player and ID based on his tag
        switch (gameObject.tag)
        {
            case "Player_1":
                playerID = 1;
                temp_TeamID = 1;
                _raycastPos = new Vector3(Screen.width * 0.5f, Screen.height * 0.75f, 0.0f);                
                break;
            case "Player_2":
                playerID = 2;                
                temp_TeamID = 2;
                _raycastPos = new Vector3(Screen.width * 0.5f, Screen.height * 0.25f, 0.0f);
                break;
            case "Player_3":
                playerID = 3;
                break;
            case "Player_4":
                playerID = 4;
                break;
                
        }
        //---------------------------------------------------------------------------
        //                                  NOTE
        //---------------------------------------------------------------------------

        //replace the temp_TeamID with data read in through the UI. This is also as
        //player 2 and player 1 could be on the same team while 3 and 4 are on the 
        //opposite

        //Somehow read the team, probably assigned through UI before the match starts
        MatchStatistics.AssignPlayerToTeam(playerID, temp_TeamID);
    }

    private void Update()
    {
        _update();
    }

    private void mouseHandler()
    {
        _cameraScript.MoveCamera(InputManager.CameraHorizontal(playerID), InputManager.CameraVertical(playerID));
    }

    private void movementHandler()
    {
        _movement.Move(InputManager.Movement(playerID));
    }

    //check if input is calling for the player to flash, then execute
    private void flashCheck()
    {
        if (InputManager.FlashButton(playerID) > 0 && flashAxisLock == false)
        {
            lockAxis(ref flashAxisLock, true);
            print("P" + playerID + " is flashing.");
        }
        if (InputManager.FlashButton(playerID) == 0)
        {
            lockAxis(ref flashAxisLock, false);
        }
    }

    //check if input is calling for the player to throw, then execute
    private void throwCheck()
    {
        if (InputManager.ThrowButton(playerID) > 0 && throwAxisLock == false)
        {
            lockAxis(ref throwAxisLock, true);
            print("P" + playerID + " is throwing.");
            _movement.Throw(_cameraScript.gameObject.transform.forward);
        }
        if (InputManager.ThrowButton(playerID) == 0)
        {
            lockAxis(ref throwAxisLock, false);
        }
    }

    //raycast gameobjects and if they're columns, set them as the selected column
    private void raycastingColumn()
    {
        RaycastHit raycastHit;
        Ray ray = playerCamera.ScreenPointToRay(_raycastPos); //Fixed it :D

        if (Physics.Raycast(ray, out raycastHit))
        {
            if (raycastHit.collider.gameObject.tag == "Column")
            {
                _selectedColumn = raycastHit.collider.gameObject;
                _columnProperties = _selectedColumn.GetComponent<ColumnProperties>();
            }
        }
    }

    //check if input is calling for the player to raise or lower a column, then execute
    private void raiseLowerCheck()
    {
        //raise column
        if (InputManager.RaiseColumn(playerID) > 0 && raiseAxisLock == false)
        {
            columnControl.AttemptRaise(playerID, _selectedColumn, _columnProperties);
            lockAxis(ref raiseAxisLock, true);
        }
        if (InputManager.RaiseColumn(playerID) == 0)
        {
            lockAxis(ref raiseAxisLock, false);
        }

        //lower column
        if (InputManager.LowerColumn(playerID) > 0 && lowerAxisLock == false)
        {
            columnControl.AttemptLower(playerID, _selectedColumn, _columnProperties);
            lockAxis(ref lowerAxisLock, true);
        }
        if (InputManager.LowerColumn(playerID) == 0)
        {
            lockAxis(ref lowerAxisLock, false);
        }
    }

    private void lockAxis(ref bool pAxisToLock, bool pState)
    {
        pAxisToLock = pState;
    }

    /// <summary>
    /// Int attached to the PlayerInput script spesificing which player this instance is listed as.
    /// </summary>
    /// <returns></returns>
    public int GetPlayerID()
    {
        return playerID;
    }

    /// <summary>
    /// Int attached to the PlayerInput script spesificing on which team this player belongs.
    /// </summary>
    /// <returns></returns>
    public int GetPlayerTeam()
    {
        return temp_TeamID;
    }
}