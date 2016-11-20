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
    private Vector3 _raycastPos;

    private bool flashAxisLock = false;
    private bool throwAxisLock = false;

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

        switch (gameObject.tag)
        {
            case "Player_1":
                playerID = 1;
                _raycastPos = new Vector3(Screen.width * 0.5f, Screen.height * 0.75f, 0.0f);
                break;
            case "Player_2":
                playerID = 2;
                _raycastPos = new Vector3(Screen.width * 0.5f, Screen.height * 0.25f, 0.0f);
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
        if (InputManager.FlashButton(playerID) > 0 && flashAxisLock == false)
        {
            lockAxis(ref flashAxisLock, true);
            print("flash lock is " + flashAxisLock);
            print("P" + playerID + " is flashing.");
        }
        if (InputManager.FlashButton(playerID) == 0)
        {
            lockAxis(ref flashAxisLock, false);
        }
    }

    private void throwCheck()
    {
        if (InputManager.ThrowButton(playerID) > 0)
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


    private void raycastingColumn()
    {
        //handles the raycast selecting the right column

        RaycastHit raycastHit;
        Ray ray = playerCamera.ScreenPointToRay(_raycastPos); //Fixed it :D

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
        if (InputManager.RaiseColumn(playerID) > 0)
        {
            columnControl.AttemptRaise(playerID, _selectedColumn, _columnProperties);
        }
        if (InputManager.LowerColumn(playerID) > 0)
        {
            columnControl.AttemptLower(playerID, _selectedColumn, _columnProperties);
        }
    }

    private void lockAxis(ref bool pAxisToLock, bool pState)
    {
        pAxisToLock = pState;
    }

}