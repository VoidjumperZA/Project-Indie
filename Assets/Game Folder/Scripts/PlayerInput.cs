using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerInput : MonoBehaviour
{
    //Receiving object references through inspector
    [SerializeField]
    private Camera _playerCamera;

    //Class instances
    private PlayerMovement _playerMovement;
    private PlayerActions _playerActions;
    private PlayerCamera _cameraScript;
    private PlayerProperties _playerProperties;

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
    private float _spawnHeight;

    //Cooldown variables
    private float _flashTimeStamp;
    private float _columnMovementTimeStamp;

    private void Start()
    {
        //Getting class instances/components of objects. Need to be checked later for conventions
        _playerMovement = GetComponent<PlayerMovement>();
        _playerActions = GetComponent<PlayerActions>();
        _cameraScript = _playerCamera.GetComponent<PlayerCamera>();
        _playerProperties = GameObject.Find("Manager").GetComponent<PlayerProperties>();
        //Setting cooldown values
        _flashTimeStamp = Time.time;
        _columnMovementTimeStamp = Time.time;

        /*
=======

        _spawnHeight = transform.position.y;

>>>>>>> 098abcf377c8abc46ce44183c1c615611bc67f7d
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


        //pizza
        _playerActions.SetCameraAndRaycastPos(_playerCamera, _raycastPos);
        MatchStatistics.AssignPlayerToTeam(playerID, temp_TeamID);


        //---------------------------------------------------------------------------
        //                                  NOTE
        //---------------------------------------------------------------------------

        //replace the temp_TeamID with data read in through the UI. This is also as
        //player 2 and player 1 could be on the same team while 3 and 4 are on the 
        //opposite

        //Somehow read the team, probably assigned through UI before the match starts
<<<<<<< HEAD
        MatchStatistics.AssignPlayerToTeam(playerID, temp_TeamID);*/
    }

    private void Update()
    {
        //Send input to the PlayerCamera script
        _cameraScript.MoveCamera(InputManager.CameraHorizontal(playerID), InputManager.CameraVertical(playerID) * cameraPolarity);
        //Send input to the PlayerMovement script
        _playerMovement.Move(InputManager.Movement(playerID).normalized, _playerProperties.GetMovementSpeed());

        faceButtonCheck(InputManager.FlashButton(playerID), ref flashAxisLock, "Flash");
        faceButtonCheck(InputManager.ThrowButton(playerID), ref throwAxisLock, "Throw");
        faceButtonCheck(InputManager.InvertButton(playerID), ref invertAxisLock, "Inverse");
        faceButtonCheck(InputManager.PauseButton(playerID), ref pauseAxisLock, "Pause");
        faceButtonCheck(InputManager.RaiseColumn(playerID), ref raiseAxisLock, "RaiseColumn");
        faceButtonCheck(InputManager.LowerColumn(playerID), ref lowerAxisLock, "LowerColumn");

        //Notes:
        //Try to make a delegate for faceButtonCheck

        //DONE
        //Add raise and lower column to faceButtonCheck
        //Move raise and lower column to PlayerActions
        //faceButtonCheck(InputManager.PauseButton(playerID), ref flashAxisLock, "Pause"); <---- using ref flashAxisLock?
        //move raycasting columns to mouseHandler()?
        //make sure flash will not happent when standing still + maybe only forward

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
                    if (_flashTimeStamp <= Time.time && flashDirectionCheck() == true)
                    {
                        _flashTimeStamp = Time.time + _playerProperties.GetFlashCooldownValue();
                        _playerMovement.Flash(InputManager.Movement(playerID).normalized, _playerProperties.GetFlashDistance(), _spawnHeight, _playerProperties.GetFlashThrowingForce(), _playerProperties.GetFlashThrowRotationAddition(), _ballPosession, _playerProperties.GetFlashThrowBeforeFlash());
                        FMODUnity.RuntimeManager.PlayOneShot(flashSound, _cameraScript.gameObject.transform.position);
                    }
                    break;
                case "Throw":
                    if (_ballPosession)
                    {
                        _playerActions.Throw(_cameraScript.transform.forward);
                        FMODUnity.RuntimeManager.PlayOneShot(ballShootSound, _cameraScript.gameObject.transform.position);
                    }
                    break;
                case "Inverse":
                    //This will be removed once we have the options screen done I think...
                    executeInverse();
                    break;
                case "Pause":
                    executePause();
                    break;
                case "RaiseColumn":
                    if (_columnMovementTimeStamp <= Time.time)
                    {
                        _columnMovementTimeStamp = Time.time + _playerProperties.GetColumnMovementCooldownValue();
                        _playerActions.MoveColumn("Raise", playerID);
                    }
                    break;
                case "LowerColumn":
                    if (_columnMovementTimeStamp <= Time.time)
                    {
                        _columnMovementTimeStamp = Time.time + _playerProperties.GetColumnMovementCooldownValue();
                        _playerActions.MoveColumn("Lower", playerID);
                    }
                    break;
            }
        }
        if (pButtonPressed == 0)
        {
            lockAxis(ref pAxisLock, false);
        }
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

    public void AssignPlayerID(int pPlayerID)
    {
        playerID = pPlayerID;
    }
    
    public void SetRaycastPosition(Vector3 pRaycastPos)
    {
        _raycastPos = pRaycastPos;
    }

    private bool flashDirectionCheck()
    {
        Vector3 forward = transform.forward;
        Vector3 flashDirection = transform.TransformDirection(InputManager.Movement(playerID).normalized);

        return (flashDirection.magnitude > 0 && Vector3.Dot(forward, flashDirection) >= 0);
    }
}