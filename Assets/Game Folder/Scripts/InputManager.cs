using UnityEngine;
using System.Collections;

public static class InputManager
{
    //Joysticks and WASD movement
    public static float MovementHorizontal()
    {
        float result = 0.0f;
        result += Input.GetAxis("J_MovmentHorizontal");
        result += Input.GetAxis("K_MovmentHorizontal");
        return Mathf.Clamp(result, -1.0f, 1.0f);
    }

    public static float MovementVertical()
    {
        float result = 0.0f;
        result += Input.GetAxis("J_MovmentVertical");
        result += Input.GetAxis("K_MovmentVertical");
        return Mathf.Clamp(result, -1.0f, 1.0f);
    }

    public static Vector3 Movement()
    {
        return new Vector3(MovementHorizontal(), 0, MovementVertical());
    }

    //Triggers / ColumnControl
    public static float RaiseColumn()
    {
        float result = 0.0f;
        result += Input.GetAxis("J_RaiseColumn");
        result += Input.GetAxis("K_RaiseColumn");
        return Mathf.Clamp(result, -1.0f, 1.0f);
    }

    public static float LowerColumn()
    {
        float result = 0.0f;
        result += Input.GetAxis("J_LowerColumn");
        result += Input.GetAxis("K_LowerColumn");
        return Mathf.Clamp(result, -1.0f, 1.0f);
    }


    //Buttons
    public static bool FlashButton()
    {
        return Input.GetButtonDown("FlashButton");
    }

    public static bool ThrowButton()
    {
        return Input.GetButtonDown("ThrowButton");
    }

}
