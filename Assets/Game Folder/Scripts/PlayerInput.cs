using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour
{
    //Mandatory
    [SerializeField]
    private PlayerCamera _camera;

    private PlayerMovement _movement;


    private void Start()
    {
        _movement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        mouseHandler();
        movementHandler();
        flashCheck();
    }

    private void mouseHandler()
    {
        _camera.MoveCamera(InputManager.P1_CameraHorizontal(), InputManager.P1_CameraVertical());
    }

    private void movementHandler()
    {

    }

    private void flashCheck()
    {

    }
}