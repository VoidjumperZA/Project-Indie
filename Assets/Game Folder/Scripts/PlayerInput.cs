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

    private enum PlayerState
    {
        NORMAL,
        CARRYINGBALL
    }

    private PlayerState _state = PlayerState.NORMAL;

    private delegate void InputUpdate();
    private event InputUpdate _update;

    private PlayerMovement _movement;
    private PlayerActions playerActions;
    private ColumnControl columnControl;
    private PlayerCamera _cameraScript;
    private PlayerProperties _playerproperties;

    //VYTAUTAS' FMOD IMPLEMENTATION BEGINS

    public string flashSound = "event:/Flash";
    public string ballShootSound = "event:/BallShoot";
    private FMOD_StudioEventEmitter _eventemitter;

    //VYTAUTAS' FMOD IMPLEMENTATION ENDS

    private GameObject _selectedColumn;
    private ColumnProperties _columnProperties;
    private Pentagram selectedPentagram;

    private int playerID;
    private int temp_TeamID;
    private int cameraPolarity = 1;
    private Vector3 _raycastPos;

    private bool flashAxisLock = false;
    private bool throwAxisLock = false;
    private bool raiseAxisLock = false;
    private bool lowerAxisLock = false;
    private bool invertAxisLock = false;
    private bool pauseAxisLock = false;
    private bool scoreboardAxisLock = false;

    private bool _flashAvailable = true;
    private bool _columnMovementAvailable = true;
    private float _flashCounter = 0.0f;
    private float _columnMovementCounter = 0.0f;

    private void Start()
    {
        _update += mouseHandler;
        _update += raycastingColumn;
        _update += raiseLowerCheck;
        _update += movementHandler;
        _update += flashCooldownResetter;
        _update += columnMovementCooldownResetter;        

        _movement = GetComponent<PlayerMovement>();
        playerActions = GetComponent<PlayerActions>();
        _cameraScript = playerCamera.GetComponent<PlayerCamera>();
        columnControl = columnControlManager.GetComponent<ColumnControl>();
        _playerproperties = GameObject.Find("Manager").GetComponent<PlayerProperties>();

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

    private void PlayerInput__update()
    {
        throw new System.NotImplementedException();
    }

    private void Update()
    {
        _update();

        //I would put these on the delegate but I'm not sure how to do that with parameters
        faceButtonCheck(InputManager.FlashButton(playerID), ref flashAxisLock, "Flash", _flashAvailable);
        faceButtonCheck(InputManager.ThrowButton(playerID), ref throwAxisLock, "Throw");
        faceButtonCheck(InputManager.InvertButton(playerID), ref invertAxisLock, "Inverse");
        faceButtonCheck(InputManager.PauseButton(playerID), ref flashAxisLock, "Pause");
    }

    private void mouseHandler()
    {
        _cameraScript.MoveCamera(InputManager.CameraHorizontal(playerID), InputManager.CameraVertical(playerID) * cameraPolarity);
    }

    private void movementHandler()
    {
        _movement.Move(InputManager.Movement(playerID));
    }
    
    //check if input is calling for the player to flash, then execute
    private void flashCheck()
    {
        //Maybe make a difference vector and translate for the trail effect possibly?
        if (InputManager.FlashButton(playerID) > 0 && flashAxisLock == false && _flashAvailable == true)
        {
            //lock the flash
            lockAxis(ref flashAxisLock, true);

           
        }
        if (InputManager.FlashButton(playerID) == 0)
        {
            lockAxis(ref flashAxisLock, false);
        }
    }


    private void faceButtonCheck(float pButtonPressed, ref bool pAxisLock, string pActionName, bool pOptionalVariable = true)
    {
        if (pButtonPressed > 0 && pAxisLock == false && pOptionalVariable == true)
        {
            lockAxis(ref pAxisLock, true);

            //execute the appropriate method for the call
            switch(pActionName)
            {
                case "Flash":
                    executeFlash();
                    break;
                case "Throw":
                    executeThrow();
                    break;
                case "Inverse":
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

    //actual flash method and calculation
    private void executeFlash()
    {
        _flashAvailable = false;

        //get our current column and the next one we could possibly land on
        GameObject currentColumn = null;
        GameObject possibleNextColumn = null;

        //fire a ray to select our current column
        Ray firstRay = new Ray(transform.position, -transform.up);
        RaycastHit firstHitInfo;
        if (Physics.Raycast(firstRay, out firstHitInfo))
        {
            currentColumn = firstHitInfo.collider.gameObject;
        }

        //store the positions of where we'll be after a flash
        Vector3 afterFlashFailPosition = transform.position + (transform.TransformVector(InputManager.Movement(playerID)) * playerActions.GetFlashDistance());
        Vector3 afterFlashSucceedPosition = afterFlashFailPosition;
        afterFlashSucceedPosition.y = columnControl.GetGroundFloorYValue();

        //fire a ray to find our next possible column
        Ray secondRay = new Ray(afterFlashFailPosition, -transform.up);
        RaycastHit secondHitInfo;
        if (Physics.Raycast(secondRay, out secondHitInfo))
        {
            possibleNextColumn = secondHitInfo.collider.gameObject;
        }

        if (currentColumn == possibleNextColumn)
        {
            FMODUnity.RuntimeManager.PlayOneShot(flashSound, _cameraScript.gameObject.transform.position);
            Vector3 deltaToPlayer = playerCamera.transform.position - gameObject.transform.position;
            playerCamera.GetComponent<PlayerCamera>().ToggleSmoothFollow(true, deltaToPlayer, playerCamera.transform.position);
            playerActions.Flash(afterFlashFailPosition);
            print("fail");
        }
        else
        {
            FMODUnity.RuntimeManager.PlayOneShot(flashSound, _cameraScript.gameObject.transform.position);
            Vector3 deltaToPlayer = playerCamera.transform.position - gameObject.transform.position;
            playerCamera.GetComponent<PlayerCamera>().ToggleSmoothFollow(true, deltaToPlayer, playerCamera.transform.position);
            playerActions.Flash(afterFlashSucceedPosition);
            print("succeed");
        }
        if (_state == PlayerState.CARRYINGBALL)
        {
            playerActions.Throw(transform.forward, true);
            Able2Throw(false);
        }
    }

    //actual throw method and calculation
    private void executeThrow()
    {
        playerActions.Throw(_cameraScript.gameObject.transform.forward, false); //why do we use camera script here?
        FMODUnity.RuntimeManager.PlayOneShot(ballShootSound, _cameraScript.gameObject.transform.position);
        Able2Throw(false);
    }

    private void executeInverse()
    {
        cameraPolarity *= -1;
    }

    private void executePause()
    {
        PauseScreen pauseScreen = GameObject.Find("Manager").GetComponent<PauseScreen>();
        pauseScreen.DisplayPauseScreen(!pauseScreen.IsPauseScreenActive(), playerID);
    }

    //check if input is calling for the player to throw, then execute
    private void throwCheck()
    {
        if (InputManager.ThrowButton(playerID) > 0 && throwAxisLock == false)
        {
            lockAxis(ref throwAxisLock, true);

        }
        if (InputManager.ThrowButton(playerID) == 0)
        {
            lockAxis(ref throwAxisLock, false);
        }
    }

    private void inversionCheck()
    {
        if (InputManager.InvertButton(playerID) > 0 && invertAxisLock == false)
        {
            lockAxis(ref invertAxisLock, true);
            
        }
        if (InputManager.InvertButton(playerID) == 0)
        {
            lockAxis(ref invertAxisLock, false);
        }
    }

    private void pauseCheck()
    {
        if (InputManager.PauseButton(playerID) > 0 && pauseAxisLock == false)
        {
            lockAxis(ref pauseAxisLock, true);
            
        }
        if (InputManager.PauseButton(playerID) == 0)
        {
            lockAxis(ref pauseAxisLock, false);
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
        //raise column
        if (InputManager.RaiseColumn(playerID) > 0 && raiseAxisLock == false && _columnMovementAvailable == true)
        {
            _columnMovementAvailable = false;
            columnControl.AttemptRaise(playerID, _selectedColumn, _columnProperties);
            lockAxis(ref raiseAxisLock, true);
        }
        if (InputManager.RaiseColumn(playerID) == 0)
        {
            lockAxis(ref raiseAxisLock, false);
        }

        //lower column
        if (InputManager.LowerColumn(playerID) > 0 && lowerAxisLock == false && _columnMovementAvailable == true)
        {
            _columnMovementAvailable = false;
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

    public void Able2Throw(bool pBool)
    {
        if (pBool == false)
        {
            //_update -= throwCheck;
            _state = PlayerState.NORMAL;
        }
        else
        {
            //_update += throwCheck;
            _state = PlayerState.CARRYINGBALL;
        }
    }

    private void flashCooldownResetter()
    {
        if(_flashAvailable == false)
        {
            _flashCounter++;
        }
        if(_flashCounter >= _playerproperties.GetFlashCooldownValue())
        {
            _flashAvailable = true;
            _flashCounter = 0.0f;
        }
    }

    private void columnMovementCooldownResetter()
    {
        if (_columnMovementAvailable == false)
        {
            _columnMovementCounter++;
        }
        if (_columnMovementCounter >= _playerproperties.GetColumnMovementCooldownValue())
        {
            _columnMovementAvailable = true;
            _columnMovementCounter = 0.0f;
        }
    }
}