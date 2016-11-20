﻿using UnityEngine;
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
    private float raycastPos = 1.0f;

    private void Start()
    {
        _update += mouseHandler;
        _update += raycastingColumn;
        _update += raiseLowerCheck;
        _update += flashCheck;
        _update += movementHandler;

        _movement = GetComponent<PlayerMovement>();
        _cameraScript = playerCamera.GetComponent<PlayerCamera>();
        columnControl = columnControlManager.GetComponent<ColumnControl>();

        switch(gameObject.tag)
        {
            case "Player_1":
                playerID = 1;
                raycastPos = 4.0f;
                break;
            case "Player_2":
                playerID = 2;
                raycastPos = 1.3f;
                break;
            case "Player_3":
                playerID = 3;
                break;
            case "Player_4":
                playerID = 4;
                break;
        }
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

    private void flashCheck()
    {
        //Just for testing
        if (InputManager.P1_FlashButton())
        {
            print("P1Flashing");
        }
        if (InputManager.P2_FlashButton())
        {
            print("P2Flashing");
        }
    }

    private void raycastingColumn()
    {
         //handles the raycast selecting the right column

        RaycastHit raycastHit;
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(playerCamera.pixelWidth / 2.0f, playerCamera.pixelHeight / 4.0f, 0));
        
        //Debug.Log("playerCamera.pixelHeight: " + playerCamera.pixelWidth / 2.0f + ", " + playerCamera.pixelHeight / 2.0f);
        Debug.Log("screen / cam: " + Screen.width / (playerCamera.pixelWidth / 2.0f) + ", " + Screen.height / (playerCamera.pixelHeight / 2.0f));
        //^ This prints (2, 4) which should be the correct position. However it seems  to get a point MUCH lower than this

        if (Physics.Raycast(ray, out raycastHit))
        {
            if (raycastHit.collider.gameObject.tag == "Column")
            {
                _selectedColumn = raycastHit.collider.gameObject;
                _columnProperties = _selectedColumn.GetComponent<ColumnProperties>();
               // columnControl.UpdateSelectedColumn(_selectedColumn, _columnProperties);
            }
        }
    }


    private void raiseLowerCheck()
    {
        if(InputManager.RaiseColumn(playerID) > 0)
        {
            columnControl.AttemptRaise(playerID, _selectedColumn, _columnProperties);
        }
        if (InputManager.LowerColumn(playerID) > 0)
        {
            columnControl.AttemptLower(playerID, _selectedColumn, _columnProperties);
        }
    }
}