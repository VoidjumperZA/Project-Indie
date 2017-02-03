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
    private Ball _ballscript;

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
    private float _cameraSensitivity;
    private float _sensitivityConstant;
    private float _internalCameraSensitivity;
    private float _spawnHeight;
    private float _manaPoints;
    private bool _ballPosession;
    private bool _grounded;
    private Vector3 _raycastPos;

    //Cooldown variables
    private float _columnMovementTimeStamp;
    private float _holdingBallTime;

    private bool controlsDisabled = false;
    private Animator _animator;

    private void Start()
    {
        //Getting class instances/components of objects. Need to be checked later for conventions
        _playerMovement = GetComponent<PlayerMovement>();
        _playerActions = GetComponent<PlayerActions>();
        _cameraScript = _playerCamera.GetComponent<PlayerCamera>();
        _playerProperties = GameObject.Find("Manager").GetComponent<PlayerProperties>();
        _ballscript = GameObject.FindGameObjectWithTag("Ball").GetComponent<Ball>();
        //Setting cooldown values
        _columnMovementTimeStamp = Time.time;
        //_forcedThrowTimeStamp = Time.time;
        _holdingBallTime = -_playerProperties.GetTimeAdditionOnPickUpBall();
        //Setting individual player values
        _cameraPolarity = 1;
        _cameraSensitivity = 5;
        _sensitivityConstant = 0.25f;
        _internalCameraSensitivity = _sensitivityConstant * _cameraSensitivity;
        _spawnHeight = transform.position.y;
        _manaPoints = _playerProperties.GetStartingManaValue();
        _ballPosession = false;
        //Sending the right camera object and raycast position to _playerActions, so it doesn't need an instance of _playerInput
        _animator = transform.FindChild("Character_animation_exp").GetComponent<Animator>();
    }

    private void Update()
    {
            //limit movement and camera controls to when the pause screen isn't on
            if (GameObject.Find("Manager").GetComponent<PauseScreen>().IsPauseScreenActive() == false)
            {
                //Send input to the PlayerCamera script
                _cameraScript.MoveCamera(InputManager.CameraHorizontal(_playerID) * _internalCameraSensitivity, (InputManager.CameraVertical(_playerID) * _internalCameraSensitivity) * _cameraPolarity);
            }

            //faceButtonCheck methods which basically acts as activate on button release
            faceButtonCheck(InputManager.FlashButton(_playerID), ref flashAxisLock, "Flash");
            faceButtonCheck(InputManager.ThrowButton(_playerID), ref throwAxisLock, "Throw");
            faceButtonCheck(InputManager.InvertButton(_playerID), ref invertAxisLock, "Inverse");
            faceButtonCheck(InputManager.PauseButton(_playerID), ref pauseAxisLock, "Pause");
            faceButtonCheck(InputManager.RaiseColumn(_playerID), ref raiseAxisLock, "RaiseColumn");
            faceButtonCheck(InputManager.LowerColumn(_playerID), ref lowerAxisLock, "LowerColumn");

            forcedThrowHandler();
    }

    private void FixedUpdate()
    {
        if (controlsDisabled == false)
        {
            //Send input to the PlayerMovement script
            _playerMovement.Move(InputManager.Movement(_playerID).normalized, _playerProperties.GetMovementSpeed());
            if (InputManager.Movement(_playerID).magnitude > 0.0f)
            {
                _animator.PlayInFixedTime("Running");
                _animator.speed = 1.0f;
            }
            else
            {
                _animator.PlayInFixedTime("Idle");
                _animator.speed = 1.0f;
            }
        }
        gravityHandler();
        if (controlsDisabled == false)
        {
            faceButtonCheck(InputManager.JumpButton(_playerID), ref jumpAxisLock, "Jump");
        }
        _playerMovement.ApplyVelocity();
    }

    private void faceButtonCheck(float pButtonPressed, ref bool pAxisLock, string pActionName)
    {
        if (pButtonPressed > 0 && pAxisLock == false && controlsDisabled == false)
        {
            lockAxis(ref pAxisLock, true);

            //execute the appropriate method for the call
            switch (pActionName)
            {
                case "Jump":
                    //Add a bool for grounded
                    if (_grounded == true)
                    {
                        _playerMovement.Jump(_playerProperties.GetJumpForce());
                        _animator.PlayInFixedTime("Jump");
                        _animator.speed = 1.0f;
                    }
                    break;
                case "Flash":
                    if (_manaPoints >= _playerProperties.GetFlashManaCost() && flashDirectionCheck() == true)
                    {
                        _manaPoints -= _playerProperties.GetFlashManaCost();
                        _playerMovement.Flash(InputManager.Movement(_playerID).normalized, _playerProperties.GetFlashDistance(), _spawnHeight, _playerProperties.GetFlashThrowingForce(), _playerProperties.GetFlashThrowRotationAddition(), _ballPosession, _playerProperties.GetFlashThrowBeforeFlash());
                        _cameraScript.ActivateSmoothFollowOnFlash(_playerProperties.GetSmoothFollowIncrement(), _playerProperties.GetSmoothFollowClipDistance());
                        FMODUnity.RuntimeManager.PlayOneShot(flashSound, _cameraScript.gameObject.transform.position);
                    }
                    break;
                case "Throw":
                    if (_ballPosession)
                    {
                        _playerActions.Throw(_cameraScript.transform.forward, PlayerActions.ThrowType.NORMAL);
                        FMODUnity.RuntimeManager.PlayOneShot(ballShootSound, _cameraScript.gameObject.transform.position);
                    }
                    break;
                case "Pause":
                    executePause();
                    break;
                case "RaiseColumn":
                    if (_columnMovementTimeStamp <= Time.time && _ballPosession == false)
                    {
                        _columnMovementTimeStamp = Time.time + _playerProperties.GetColumnMovementCooldownValue();
                        _playerActions.MoveColumn("Raise", _playerID);
                    }
                    break;
                case "LowerColumn":
                    if (_columnMovementTimeStamp <= Time.time && _ballPosession == false)
                    {
                        _columnMovementTimeStamp = Time.time + _playerProperties.GetColumnMovementCooldownValue();
                        _playerActions.MoveColumn("Lower", _playerID);
                    }
                    break;
            }
        }
        if (pButtonPressed > 0 && pAxisLock == false)
        {
            lockAxis(ref pAxisLock, true);

            //execute the appropriate method for the call
            switch (pActionName)
            {
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

    //This will be removed once we have the options screen done I think...
    public void ExecuteInverse()
    {
        _cameraPolarity *= -1;
    }

    public void ExecuteInverse(bool pState)
    {
        if (pState == true)
        {
            _cameraPolarity = -1;
        }
        else
        {
            _cameraPolarity = 1;
        }
    }

    public int GetInverseState()
    {
        return _cameraPolarity;
    }

    public void SetPlayerSensitivity(int pIncrement)
    {
        _cameraSensitivity = pIncrement;
        _internalCameraSensitivity = _sensitivityConstant * pIncrement;
    }

    public float GetPlayerSensitivity()
    {
        return _cameraSensitivity;
    }

    public void SetControlsDisabled(bool pState)
    {
        controlsDisabled = pState;
    }

    public bool GetControlsDisabled()
    {
        return controlsDisabled;
    }

    //Implemented by Josh, leave it for now (you better)
    private void executePause()
    {
        PauseScreen pauseScreen = GameObject.Find("Manager").GetComponent<PauseScreen>();
        if (pauseScreen.IsPauseScreenActive() == false)
        {
            SetControlsDisabled(true);
            pauseScreen.DisplayPauseScreen(true, _playerID);
            pauseScreen.DisplayPauseScreenOwner();
            pauseScreen.ResetLightUpCounter();
            StartCoroutine(pauseScreen.LightUpHexagonalArray(true));
        }
        else if (pauseScreen.IsPauseScreenActive() == true && _playerID == pauseScreen.GetPauseScreenOwner())
        {
            SetControlsDisabled(false);
            pauseScreen.HidePauseScreenOwner();
            pauseScreen.DisableActiveSubmenu();
            pauseScreen.DisplayPauseScreen(false, 0);
        }
    }

    private void lockAxis(ref bool pAxisToLock, bool pState)
    {
        pAxisToLock = pState;
    }
    public void SetBallPosession(bool pBool)
    {
        _ballPosession = pBool;
        if (pBool == true)
        {
            //_forcedThrowTimeStamp = Time.time + _playerProperties.GetBallPosessionTime();
            _holdingBallTime += _playerProperties.GetTimeAdditionOnPickUpBall();
        }
    }

    public void SetGrounded(bool pBool)
    {
        _grounded = pBool;
    }

    private void forcedThrowHandler()
    {
        _holdingBallTime = _ballPosession == true ? _holdingBallTime + Time.deltaTime : _holdingBallTime - Time.deltaTime;

        _holdingBallTime = Mathf.Clamp(_holdingBallTime, -_playerProperties.GetTimeAdditionOnPickUpBall(), _playerProperties.GetBallPosessionTime());

        if (_ballPosession)
        {
            _ballscript.SetRotationSpeed(_holdingBallTime / _playerProperties.GetBallPosessionTime());
        }

        if (_holdingBallTime == _playerProperties.GetBallPosessionTime())
        {
            _playerActions.Throw(transform.forward, PlayerActions.ThrowType.FORCED);
            print("_holdingBallTime Has reached its maximum value and Forced Throw has been activated");
        }
    }

    private void gravityHandler()
    {
        _grounded = _playerMovement.Grounded();

        if (_grounded == false)
        {
            _playerMovement.ApplyGravity(_playerProperties.GetAddedGravity());
        }
        else
        {
            _playerMovement.ResetGravity();
        }
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
