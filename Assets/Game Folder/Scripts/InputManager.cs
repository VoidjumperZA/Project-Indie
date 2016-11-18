using UnityEngine;
using System.Collections;

public static class InputManager
{
    //Joysticks and WASD movement
    public static float P1_MovementHorizontal()
    {
        float result = 0.0f;
        result += Input.GetAxis("J_MovementHorizontal");
        result += Input.GetAxis("K_MovementHorizontal");
        return Mathf.Clamp(result, -1.0f, 1.0f);
    }

    public static float P1_MovementVertical()
    {
        float result = 0.0f;
        result += Input.GetAxis("1_J_MovementVertical");
        result += Input.GetAxis("1_K_MovementVertical");
        return Mathf.Clamp(result, -1.0f, 1.0f);
    }

    public static Vector3 P1_Movement()
    {
        return new Vector3(P1_MovementHorizontal(), 0, P1_MovementVertical());
    }

    //Mouse camera movement
    public static float P1_CameraHorizontal()
    {
        float result = 0.0f;
        result += Input.GetAxis("1_J_CameraHorizontal");
        result += Input.GetAxis("1_M_CameraHorizontal");
        return Mathf.Clamp(result, -1.0f, 1.0f);
    }

    public static float P1_CameraVertical()
    {
        float result = 0.0f;
        result += Input.GetAxis("1_J_CameraVertical");
        result += Input.GetAxis("1_M_CameraVertical");
        return Mathf.Clamp(result, -1.0f, 1.0f);
    }

    public static Vector3 P1_CameraMovement()
    {
        return new Vector3(P1_CameraHorizontal(), 0, P1_CameraVertical());
    }

    //Triggers / ColumnControl
    public static float P1_RaiseColumn()
    {
        float result = 0.0f;
        result += Input.GetAxis("1_J_RaiseColumn");
        result += Input.GetAxis("1_K_RaiseColumn");
        return Mathf.Clamp(result, -1.0f, 1.0f);
    }

    public static float P1_LowerColumn()
    {
        float result = 0.0f;
        result += Input.GetAxis("1_J_LowerColumn");
        result += Input.GetAxis("1_K_LowerColumn");
        return Mathf.Clamp(result, -1.0f, 1.0f);
    }

    //Buttons
    public static bool P1_FlashButton()
    {
        return Input.GetButtonDown("1_FlashButton");
    }

    public static bool P1_ThrowButton()
    {
        return Input.GetButtonDown("1_ThrowButton");
    }

    //-----------------------------------------------
    //                  PLAYER 2
    //-----------------------------------------------

    //Joysticks and WASD movement
    public static float P2_MovementHorizontal()
    {
        float result = 0.0f;
        result += Input.GetAxis("2_J_MovementHorizontal");
        return Mathf.Clamp(result, -1.0f, 1.0f);
    }

    public static float P2_MovementVertical()
    {
        float result = 0.0f;
        result += Input.GetAxis("2_J_MovementVertical");
        return Mathf.Clamp(result, -1.0f, 1.0f);
    }

    public static Vector3 P2_Movement()
    {
        return new Vector3(P2_MovementHorizontal(), 0, P2_MovementVertical());
    }

    //Mouse camera movement
    public static float P2_CameraHorizontal()
    {
        float result = 0.0f;
        result += Input.GetAxis("2_J_CameraHorizontal");
        return Mathf.Clamp(result, -1.0f, 1.0f);
    }

    public static float P2_CameraVertical()
    {
        float result = 0.0f;
        result += Input.GetAxis("2_J_CameraVertical");
        return Mathf.Clamp(result, -1.0f, 1.0f);
    }

    public static Vector3 P2_CameraMovement()
    {
        return new Vector3(P2_CameraHorizontal(), 0, P2_CameraVertical());
    }

    //Triggers / ColumnControl
    public static float P2_RaiseColumn()
    {
        float result = 0.0f;
        result += Input.GetAxis("2_J_RaiseColumn");
        return Mathf.Clamp(result, -1.0f, 1.0f);
    }

    public static float P2_LowerColumn()
    {
        float result = 0.0f;
        result += Input.GetAxis("2_J_LowerColumn");
        return Mathf.Clamp(result, -1.0f, 1.0f);
    }

    //Buttons
    public static bool P2_FlashButton()
    {
        return Input.GetButtonDown("2_FlashButton");
    }

    public static bool P2_ThrowButton()
    {
        return Input.GetButtonDown("2_ThrowButton");
    }
}
