using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerInput : MonoBehaviour
{
    [SerializeField]
    private bool _switchStatementOn;

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
    private bool jumpAxisLock = false;
    private bool flashAxisLock = false;
    private bool throwAxisLock = false;
    private bool raiseAxisLock = false;
    private bool lowerAxisLock = false;
    private bool invertAxisLock = false;
    private bool pauseAxisLock = false;
    private bool scoreboardAxisLock = false;

    //Individual player stats
    private int _playerID;
    private int _temp_TeamID;
    private int _cameraPolarity;
    private float _spawnHeight;
    private float _manaPoints;
    private bool _ballPosession;
    private Vector3 _raycastPos;

    //Cooldown variables
    private float _jumpTimeStamp;
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
        _jumpTimeStamp = Time.time;
        _flashTimeStamp = Time.time;
        _columnMovementTimeStamp = Time.time;
        //Setting individual player values
        _cameraPolarity = 1;
        _spawnHeight = transform.position.y;
        _manaPoints = _playerProperties.GetStartingManaValue();
        _ballPosession = false;
        //Sending the right camera object and raycast position to _playerActions, so it doesn't need an instance of _playerInput

        if (_switchStatementOn)
        {
            //Temporarily switch statement
            switch (gameObject.tag)
            {
                case "Player_1":
                    _playerID = 1;
                    _temp_TeamID = 1;
                    _raycastPos = new Vector3(Screen.width * 0.25f, Screen.height * 0.25f, 0.0f);
                    break;
                case "Player_2":
                    _playerID = 2;
                    _temp_TeamID = 1;
                    _raycastPos = new Vector3(Screen.width * 0.75f, Screen.height * 0.25f, 0.0f);
                    break;
                case "Player_3":
                    _playerID = 3;
                    _temp_TeamID = 2;
                    _raycastPos = new Vector3(Screen.width * 0.25f, Screen.height * 0.75f, 0.0f);
                    break;
                case "Player_4":
                    _playerID = 4;
                    _temp_TeamID = 2;
                    _raycastPos = new Vector3(Screen.width * 0.75f, Screen.height * 0.75f, 0.0f);
                    break;
            }
            _playerActions.SetCameraAndRaycastPos(_playerCamera, _raycastPos);
        }
    }

    private void Update()
    {
        //Send input to the PlayerCamera script
        _cameraScript.MoveCamera(InputManager.CameraHorizontal(_playerID), InputManager.CameraVertical(_playerID) * _cameraPolarity);
        //Send input to the PlayerMovement script
        _playerMovement.Move(InputManager.Movement(_playerID).normalized, _playerProperties.GetMovementSpeed());
        //faceButtonCheck methods which basically acts as activate on button release
        //faceButtonCheck(InputManager.JumpButton(_playerID), ref jumpAxisLock, "Jump");
        faceButtonCheck(InputManager.FlashButton(_playerID), ref flashAxisLock, "Flash");
        faceButtonCheck(InputManager.ThrowButton(_playerID), ref throwAxisLock, "Throw");
        faceButtonCheck(InputManager.InvertButton(_playerID), ref invertAxisLock, "Inverse");
        faceButtonCheck(InputManager.PauseButton(_playerID), ref pauseAxisLock, "Pause");
        faceButtonCheck(InputManager.RaiseColumn(_playerID), ref raiseAxisLock, "RaiseColumn");
        faceButtonCheck(InputManager.LowerColumn(_playerID), ref lowerAxisLock, "LowerColumn");
    }

    private void faceButtonCheck(float pButtonPressed, ref bool pAxisLock, string pActionName)
    {
        if (pButtonPressed > 0 && pAxisLock == false)
        {
            lockAxis(ref pAxisLock, true);

            //execute the appropriate method for the call
            switch (pActionName)
            {
                case "Jump":
                    //Add a bool for grounded
                    if (_jumpTimeStamp <= Time.time)
                    {
                        _jumpTimeStamp = Time.time + _playerProperties.GetJumpCooldownValue();
                        _playerMovement.Jump(_playerProperties.GetJumpForce());
                    }
                    break;
                case "Flash":
                    if (_flashTimeStamp <= Time.time && _manaPoints >= _playerProperties.GetFlashManaCost() && flashDirectionCheck() == true)
                    {
                        _flashTimeStamp = Time.time + _playerProperties.GetFlashCooldownValue();
                        _manaPoints -= _playerProperties.GetFlashManaCost();
                        _playerMovement.Flash(InputManager.Movement(_playerID).normalized, _playerProperties.GetFlashDistance(), _spawnHeight, _playerProperties.GetFlashThrowingForce(), _playerProperties.GetFlashThrowRotationAddition(), _ballPosession, _playerProperties.GetFlashThrowBeforeFlash());
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
                        _playerActions.MoveColumn("Raise", _playerID);
                    }
                    break;
                case "LowerColumn":
                    if (_columnMovementTimeStamp <= Time.time)
                    {
                        _columnMovementTimeStamp = Time.time + _playerProperties.GetColumnMovementCooldownValue();
                        _playerActions.MoveColumn("Lower", _playerID);
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
        _cameraPolarity *= -1;
    }
    //Implemented by Josh, leave it for now
    private void executePause()
    {
        PauseScreen pauseScreen = GameObject.Find("Manager").GetComponent<PauseScreen>();
        pauseScreen.DisplayPauseScreen(!pauseScreen.IsPauseScreenActive(), _playerID);
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
        return _playerID;
    }

    public void AssignPlayerID(int pPlayerID)
    {
        _playerID = pPlayerID;
    }

    public void SetRaycastPosition(Vector3 pRaycastPos)
    {
        _raycastPos = pRaycastPos;
        _playerActions = GetComponent<PlayerActions>();
        _playerActions.SetCameraAndRaycastPos(_playerCamera, _raycastPos);
    }

    private bool flashDirectionCheck()
    {
        Vector3 forward = transform.forward;
        Vector3 flashDirection = transform.TransformDirection(InputManager.Movement(_playerID).normalized);

        return (flashDirection.magnitude > 0 && Vector3.Dot(forward, flashDirection) >= 0);
    }

    public void AddManaPoints()
    {
        _manaPoints = Mathf.Min(_manaPoints + _playerProperties.GetManaValueOnPickUp(), _playerProperties.GetMaxManaValue());
        print("_manaPoints: " + _manaPoints);
    }
}