using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class InputManager
{
    //Joysticks and WASD movement
    /// <summary>
    /// Float value between 0 and 1 if the axis is detecting input. Non-zero value confirms input.
    /// </summary>
    /// <param name="pPlayerID"></param>
    /// <returns></returns>
    public static float MovementHorizontal(int pPlayerID)
    {
        return determineMovementAxes(pPlayerID, "MovementHorizontal");
    }

    /// <summary>
    /// Float value between 0 and 1 if the axis is detecting input. Non-zero value confirms input.
    /// </summary>
    /// <param name="pPlayerID"></param>
    /// <returns></returns>
    public static float MovementVertical(int pPlayerID)
    {
        return determineMovementAxes(pPlayerID, "MovementVertical");
    }

    /// <summary>
    /// /// Vector3 float values for both the horizontal and vertical axes input. Non-zero value confirms input.
    /// </summary>
    /// <param name="pPlayerID"></param>
    /// <returns></returns>
    public static Vector3 Movement(int pPlayerID)
    {
        return new Vector3(MovementHorizontal(pPlayerID), 0, MovementVertical(pPlayerID));
    }

    //Mouse camera movement
    /// <summary>
    /// Float value between 0 and 1 if the axis is detecting input. Non-zero value confirms input.
    /// </summary>
    /// <param name="pPlayerID"></param>
    /// <returns></returns>
    public static float CameraHorizontal(int pPlayerID)
    {
        return determineMovementAxes(pPlayerID, "CameraHorizontal");

    }

    /// <summary>
    /// Float value between 0 and 1 if the axis is detecting input. Non-zero value confirms input.
    /// </summary>
    /// <param name="pPlayerID"></param>
    /// <returns></returns>
    public static float CameraVertical(int pPlayerID)
    {
        return determineMovementAxes(pPlayerID, "CameraVertical");
    }

    /// <summary>
    /// Vector3 float values for both the horizontal and vertical axes input. Non-zero value confirms input.
    /// </summary>
    /// <param name="pPlayerID"></param>
    /// <returns></returns>
    public static Vector3 CameraMovement(int pPlayerID)
    {
        return new Vector3(CameraHorizontal(pPlayerID), 0, CameraVertical(pPlayerID));
    }

    public static float UIHorizontal(int pPlayerID)
    {
        return determineMovementAxes(pPlayerID, "UIHorizontal");
    }

    /// <summary>
    /// Float value between 0 and 1 if the axis is detecting input. Non-zero value confirms input.
    /// </summary>
    /// <param name="pPlayerID"></param>
    /// <returns></returns>
    public static float UIVertical(int pPlayerID)
    {
        return determineMovementAxes(pPlayerID, "UIVertical");
    }

    //Triggers / ColumnControl
    /// <summary>
    /// Float value between 0 and 1 if the axis is detecting input. Non-zero value confirms input.
    /// </summary>
    /// <param name="pPlayerID"></param>
    /// <returns></returns>
    public static float RaiseColumn(int pPlayerID)
    {
        return determineMovementAxes(pPlayerID, "RaiseColumn");
    }

    /// <summary>
    /// Float value between 0 and 1 if the axis is detecting input. Non-zero value confirms input.
    /// </summary>
    /// <param name="pPlayerID"></param>
    /// <returns></returns>
    public static float LowerColumn(int pPlayerID)
    {
        return determineMovementAxes(pPlayerID, "LowerColumn");
    }

    /// <summary>
    /// Float value between 0 and 1 if the axis is detecting input. Non-zero value confirms input.
    /// </summary>
    /// <param name="pPlayerID"></param>
    /// <returns></returns>
    public static float FlashButton(int pPlayerID)
    {
        return determineMovementAxes(pPlayerID, "FlashButton");
    }

    /// <summary>
    /// Float value between 0 and 1 if the axis is detecting input. Non-zero value confirms input.
    /// </summary>
    /// <param name="pPlayerID"></param>
    /// <returns></returns>
    public static float AcceptButton(int pPlayerID)
    {
        return determineMovementAxes(pPlayerID, "AcceptButton");
    }

    /// <summary>
    /// Float value between 0 and 1 if the axis is detecting input. Non-zero value confirms input.
    /// </summary>
    /// <param name="pPlayerID"></param>
    /// <returns></returns>
    public static float ThrowButton(int pPlayerID)
    {
        return determineMovementAxes(pPlayerID, "ThrowButton");
    }

    /// <summary>
    /// Float value between 0 and 1 if the axis is detecting input. Non-zero value confirms input.
    /// </summary>
    /// <param name="pPlayerID"></param>
    /// <returns></returns>
    public static float JumpButton(int pPlayerID)
    {
        return determineMovementAxes(pPlayerID, "JumpButton");
    }

    //QUICK FIX: PLEASE DELETE
    public static float InvertButton(int pPlayerID)
    {
        return determineMovementAxes(pPlayerID, "InvertButton");
    }

    public static float PauseButton(int pPlayerID)
    {
        return determineMovementAxes(pPlayerID, "PauseButton");

    }

    //internal method that handles axis input detection 
    private static float determineMovementAxes(int pPlayerID, string pAxisName)
    {
        //saves us the trouble of fulling out five separate parametres for each controller and the keyboard
        List<string> axisNames = new List<string>();

        string keyboardAxis = "1_K_" + pAxisName;
        axisNames.Add(keyboardAxis);

        for (int i = 0; i < 4; i++)
        {
            string joystickAxis = "" + (i + 1) + "_J_" + pAxisName;
            axisNames.Add(joystickAxis);
        }

        float p1_result = 0.0f;
        p1_result += Input.GetAxis(axisNames[0]);
        p1_result += Input.GetAxis(axisNames[1]);

        float p2_result = 0.0f;
        p2_result += Input.GetAxis(axisNames[2]);

        float p3_result = 0.0f;
        p3_result += Input.GetAxis(axisNames[3]);

        float p4_result = 0.0f;
        p4_result += Input.GetAxis(axisNames[4]);

        return returnClampedAxis(pPlayerID, p1_result, p2_result, p3_result, p4_result);
    }

    //internal method for clamping axes to 1, in the case of both keyboard and joystick input simultaenously
    //our axis value would become twice as large
    private static float returnClampedAxis(int pPlayerID, float pP1_Result, float pP2_Result, float pP3_Result, float pP4_Result)
    {
        switch (pPlayerID)
        {
            case 1:
                return Mathf.Clamp(pP1_Result, -1.0f, 1.0f);
                break;
            case 2:
                return Mathf.Clamp(pP2_Result, -1.0f, 1.0f);
                break;
            case 3:
                return Mathf.Clamp(pP3_Result, -1.0f, 1.0f);
                break;
            case 4:
                return Mathf.Clamp(pP4_Result, -1.0f, 1.0f);
                break;
            default:
                return Mathf.Clamp(pP1_Result, -1.0f, 1.0f);
                break;
        }
    }
}
