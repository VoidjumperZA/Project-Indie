using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour
{
    //Mandatory
    [SerializeField]
    private PlayerCamera _camera;

    [SerializeField]
    private GameObject columnControlManager;

    private PlayerMovement _movement;  
    private ColumnControl columnControl;

    private int playerID;

    private void Start()
    {
        _movement = GetComponent<PlayerMovement>();
        columnControl = columnControlManager.GetComponent<ColumnControl>();

        switch(gameObject.tag)
        {
            case "Player_1":
                playerID = 1;
                break;
            case "Player_2":
                playerID = 2;
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
        mouseHandler();
        raiseLowerCheck();
        flashCheck();
    }

    private void FixedUpdate()
    {
        movementHandler();
    }

    private void mouseHandler()
    {
        _camera.MoveCamera(InputManager.CameraHorizontal(playerID), InputManager.CameraVertical(playerID));
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

    private void raiseLowerCheck()
    {
        if(InputManager.RaiseColumn(playerID) > 0)
        {
            columnControl.AttemptRaise(playerID);
        }
        if (InputManager.LowerColumn(playerID) > 0)
        {
            columnControl.AttemptLower(playerID);
        }
    }
}