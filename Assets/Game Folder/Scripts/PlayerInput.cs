using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerInput : MonoBehaviour
{
    //Receiving object references through inspector
    [SerializeField]
    private Camera _playerCamera;

    //Delegates
    private delegate void InputUpdate();
    private event InputUpdate _update;

    //Class instances
    private ColumnControl _columnControl;
    private PlayerMovement _playerMovement;
    private PlayerActions _playerActions;
    private PlayerCamera _cameraScript;
    private PlayerProperties _playerProperties;

    //Need to be checked if needed.....
    private GameObject _selectedColumn;
    private ColumnProperties _columnProperties;
    private Pentagram selectedPentagram;

    //Vytautas's' FMOD implementation
    public string flashSound = "event:/Flash";
    public string ballShootSound = "event:/BallShoot";
    private FMOD_StudioEventEmitter _eventemitter;

    //Booleans for locking axis, enabling OnButtonRelease behaviour-like
    private bool flashAxisLock = false;
    private bool throwAxisLock = false;
    private bool raiseAxisLock = false;
    private bool lowerAxisLock = false;
    private bool invertAxisLock = false;
    private bool pauseAxisLock = false;
    private bool scoreboardAxisLock = false;

    //Individual player stats
    private int playerID;
    private int temp_TeamID;
    private int cameraPolarity = 1;
    private Vector3 _raycastPos;
    private bool _ballPosession = false;

    //Cooldown values
    private float _flashTimeStamp;
    private float _columnMovementTimeStamp;

    private void Start()
    {
        //Adding functions to the _update delegate
        _update += mouseHandler;
        _update += raycastingColumn;
        _update += raiseLowerCheck;
        _update += movementHandler;
        //Getting class instances/components of objects. Need to be checked later for conventions
        _playerMovement = GetComponent<PlayerMovement>();
        _playerActions = GetComponent<PlayerActions>();
        _cameraScript = _playerCamera.GetComponent<PlayerCamera>();
        _playerProperties = GameObject.Find("Manager").GetComponent<PlayerProperties>();
        _columnControl = GameObject.Find("Manager").GetComponent<ColumnControl>();

        _flashTimeStamp = Time.time;
        _columnMovementTimeStamp = Time.time;

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

    //Where is this for?
    private void PlayerInput__update()
    {
        throw new System.NotImplementedException();
    }

    private void Update()
    {
        _update();

        //I would put these on the delegate but I'm not sure how to do that with parameters
        faceButtonCheck(InputManager.FlashButton(playerID), ref flashAxisLock, "Flash");
        faceButtonCheck(InputManager.ThrowButton(playerID), ref throwAxisLock, "Throw");
        faceButtonCheck(InputManager.InvertButton(playerID), ref invertAxisLock, "Inverse");
        faceButtonCheck(InputManager.PauseButton(playerID), ref flashAxisLock, "Pause");

        print("PlayerID: " + playerID + ", ballPosession:  " + _ballPosession);
    }

    private void mouseHandler()
    {
        _cameraScript.MoveCamera(InputManager.CameraHorizontal(playerID), InputManager.CameraVertical(playerID) * cameraPolarity);
    }

    private void movementHandler()
    {
        _playerMovement.Move(InputManager.Movement(playerID), _playerProperties.GetMovementSpeed());
    }

    private void faceButtonCheck(float pButtonPressed, ref bool pAxisLock, string pActionName)
    {
        if (pButtonPressed > 0 && pAxisLock == false)
        {
            lockAxis(ref pAxisLock, true);

            //execute the appropriate method for the call
            switch (pActionName)
            {
                case "Flash":
                    //executeFlash();
                    if (_flashTimeStamp <= Time.time)
                    {
                        _flashTimeStamp = Time.time + _playerProperties.GetFlashCooldownValue();
                        _playerMovement.Flash(InputManager.Movement(playerID), _playerProperties.GetFlashDistance(), _columnControl.GetGroundFloorYValue(), _playerProperties.GetFlashThrowingForce(), _playerProperties.GetFlashThrowRotationAddition(), _ballPosession, _playerProperties.GetFlashThrowBeforeFlash());
                    }
                    //Either set _flashAvailable to false here or try to use ref bool pOptionalVariable, for now leave the cooldown issue for later since need to make another reset method first.
                    break;
                case "Throw":
                    //executeThrow();
                    if (_ballPosession) { _playerActions.Throw(_cameraScript.transform.forward); }
                    break;
                case "Inverse":
                    //This will be removed once we have the options screen done I think...
                    executeInverse();
                    break;
                case "Pause":
                    executePause();
                    break;
            }
        }
        if (pButtonPressed == 0)
        {
            lockAxis(ref pAxisLock, false);
        }
    }

    //Can be removed if PlayerMovement's Flash works 100% with FMOD, this is basically reference material.
    private void executeFlash()
    {
        //_flashAvailable = false;

        ////get our current column and the next one we could possibly land on
        //GameObject currentColumn = null;
        //GameObject possibleNextColumn = null;

        ////fire a ray to select our current column
        //Ray firstRay = new Ray(transform.position, -transform.up);
        //RaycastHit firstHitInfo;
        //if (Physics.Raycast(firstRay, out firstHitInfo))
        //{
        //    currentColumn = firstHitInfo.collider.gameObject;
        //}

        ////Create the positions of where we'll be after a flash
        //Vector3 afterFlashFailPosition = transform.position + (transform.TransformVector(InputManager.Movement(playerID)) * playerActions.GetFlashDistance());
        //Vector3 afterFlashSucceedPosition = afterFlashFailPosition;
        //afterFlashSucceedPosition.y = columnControl.GetGroundFloorYValue();

        ////fire a ray to find our next possible column
        //Ray secondRay = new Ray(afterFlashFailPosition, -transform.up);
        //RaycastHit secondHitInfo;
        //if (Physics.Raycast(secondRay, out secondHitInfo))
        //{
        //    possibleNextColumn = secondHitInfo.collider.gameObject;
        //}

        //if (currentColumn == possibleNextColumn)
        //{
        //    FMODUnity.RuntimeManager.PlayOneShot(flashSound, _cameraScript.gameObject.transform.position);
        //    Vector3 deltaToPlayer = playerCamera.transform.position - gameObject.transform.position;
        //    playerCamera.GetComponent<PlayerCamera>().ToggleSmoothFollow(true, deltaToPlayer, playerCamera.transform.position);
        //    playerActions.Flash(afterFlashFailPosition);
        //}
        //else
        //{
        //    FMODUnity.RuntimeManager.PlayOneShot(flashSound, _cameraScript.gameObject.transform.position);
        //    Vector3 deltaToPlayer = playerCamera.transform.position - gameObject.transform.position;
        //    playerCamera.GetComponent<PlayerCamera>().ToggleSmoothFollow(true, deltaToPlayer, playerCamera.transform.position);
        //    playerActions.Flash(afterFlashSucceedPosition);
        //}
        //if (_state == PlayerState.CARRYINGBALL)
        //{
        //    playerActions.Throw(transform.forward, true);
        //    Able2Throw(false);
        //}
    }
    //Can be removed if PlayerActions's Throw works 100% with FMOD, this is basically reference material.
    private void executeThrow()
    {
        //playerActions.Throw(_cameraScript.gameObject.transform.forward);
        //FMODUnity.RuntimeManager.PlayOneShot(ballShootSound, _cameraScript.gameObject.transform.position);
        //Able2Throw(false);
    }

    //This will be removed once we have the options screen done I think...
    private void executeInverse()
    {
        cameraPolarity *= -1;
    }

    //Implemented by Josh, leave it for now
    private void executePause()
    {
        PauseScreen pauseScreen = GameObject.Find("Manager").GetComponent<PauseScreen>();
        pauseScreen.DisplayPauseScreen(!pauseScreen.IsPauseScreenActive(), playerID);
    }

    //raycast gameobjects and if they're columns, set them as the selected column
    private void raycastingColumn()
    {
        RaycastHit raycastHit;
        Ray ray = _playerCamera.ScreenPointToRay(_raycastPos);

        if (Physics.Raycast(ray, out raycastHit))
        {
            if (raycastHit.collider.gameObject.tag == "Column")
            {
                _selectedColumn = raycastHit.collider.gameObject;
                _columnProperties = _selectedColumn.GetComponent<ColumnProperties>();

                if (_columnProperties.GetColumnType() == 0)
                {
                    //if we have a pentagram already, and it's not the same one we're targeting, switch the old one off
                    Pentagram newSelectedPenta = _selectedColumn.GetComponentInChildren(typeof(Pentagram), true) as Pentagram;
                    if (selectedPentagram != null && selectedPentagram != newSelectedPenta)
                    {
                        selectedPentagram.TogglePentagram(false);
                    }

                    selectedPentagram = newSelectedPenta;
                    if (selectedPentagram.IsPentagramActive() != true)
                    {
                        selectedPentagram.TogglePentagram(true, gameObject.transform);
                    }
                }
            }
            else
            {
                _selectedColumn = null;
                _columnProperties = null;
                if (selectedPentagram != null)
                {
                    selectedPentagram.TogglePentagram(false);
                }
            }
        }
    }

    //check if input is calling for the player to raise or lower a column, then execute
    private void raiseLowerCheck()
    {
        //For now this if statement is here, need to wait untill its in faceButtonCheck
        if (_columnMovementTimeStamp <= Time.time)
        {
            _columnMovementTimeStamp = Time.time + _playerProperties.GetColumnMovementCooldownValue();
            //raise column
            if (InputManager.RaiseColumn(playerID) > 0 && raiseAxisLock == false)
            {
                _columnControl.AttemptRaise(playerID, _selectedColumn, _columnProperties);
                lockAxis(ref raiseAxisLock, true);
            }
            if (InputManager.RaiseColumn(playerID) == 0)
            {
                lockAxis(ref raiseAxisLock, false);
            }

            //lower column
            if (InputManager.LowerColumn(playerID) > 0 && lowerAxisLock == false)
            {
                _columnControl.AttemptLower(playerID, _selectedColumn, _columnProperties);
                lockAxis(ref lowerAxisLock, true);
            }
            if (InputManager.LowerColumn(playerID) == 0)
            {
                lockAxis(ref lowerAxisLock, false);
            }
        }
    }

    private void lockAxis(ref bool pAxisToLock, bool pState)
    {
        pAxisToLock = pState;
    }
    public void SetBallPosession(bool pBool)
    {
        _ballPosession = pBool;
    }

    //Not sure if these should be in PlayerInput at all, same for the values ofcourse

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