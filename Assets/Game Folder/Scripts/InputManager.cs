using UnityEngine;
using System.Collections;

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
        float p1_result = 0.0f;
        p1_result += Input.GetAxis("1_J_MovementHorizontal");
        p1_result += Input.GetAxis("1_K_MovementHorizontal");

        float p2_result = 0.0f;
        p2_result += Input.GetAxis("2_J_MovementHorizontal");

        float p3_result = 0.0f;
        

        float p4_result = 0.0f;

        return returnClampedAxis(pPlayerID, p1_result, p2_result, p3_result, p4_result);
    }

    /// <summary>
    /// Float value between 0 and 1 if the axis is detecting input. Non-zero value confirms input.
    /// </summary>
    /// <param name="pPlayerID"></param>
    /// <returns></returns>
    public static float MovementVertical(int pPlayerID)
    {
        float p1_result = 0.0f;
        p1_result += Input.GetAxis("1_J_MovementVertical");
        p1_result += Input.GetAxis("1_K_MovementVertical");

        float p2_result = 0.0f;
        p2_result += Input.GetAxis("2_J_MovementVertical");


        float p3_result = 0.0f;


        float p4_result = 0.0f;

        return returnClampedAxis(pPlayerID, p1_result, p2_result, p3_result, p4_result);   
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
        float p1_result = 0.0f;
        p1_result += Input.GetAxis("1_J_CameraHorizontal");
        p1_result += Input.GetAxis("1_M_CameraHorizontal");

        float p2_result = 0.0f;
        p2_result += Input.GetAxis("2_J_CameraHorizontal");

        float p3_result = 0.0f;
        float p4_result = 0.0f;

        return returnClampedAxis(pPlayerID, p1_result, p2_result, p3_result, p4_result);
        
    }

    /// <summary>
    /// Float value between 0 and 1 if the axis is detecting input. Non-zero value confirms input.
    /// </summary>
    /// <param name="pPlayerID"></param>
    /// <returns></returns>
    public static float CameraVertical(int pPlayerID)
    {
        float p1_result = 0.0f;
        p1_result += Input.GetAxis("1_J_CameraVertical");
        p1_result += Input.GetAxis("1_M_CameraVertical");

        float p2_result = 0.0f;
        p2_result += Input.GetAxis("2_J_CameraVertical");

        float p3_result = 0.0f;
        float p4_result = 0.0f;

        return returnClampedAxis(pPlayerID, p1_result, p2_result, p3_result, p4_result);
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

    //Triggers / ColumnControl
    /// <summary>
    /// Float value between 0 and 1 if the axis is detecting input. Non-zero value confirms input.
    /// </summary>
    /// <param name="pPlayerID"></param>
    /// <returns></returns>
    public static float RaiseColumn(int pPlayerID)
    {
        float p1_result = 0.0f;
        p1_result += Input.GetAxis("1_J_RaiseColumn");
        p1_result += Input.GetAxis("1_K_RaiseColumn");

        float p2_result = 0.0f;
        p2_result += Input.GetAxis("2_J_RaiseColumn");

        float p3_result = 0.0f;
        float p4_result = 0.0f;

        return returnClampedAxis(pPlayerID, p1_result, p2_result, p3_result, p4_result);
    }

    /// <summary>
    /// Float value between 0 and 1 if the axis is detecting input. Non-zero value confirms input.
    /// </summary>
    /// <param name="pPlayerID"></param>
    /// <returns></returns>
    public static float LowerColumn(int pPlayerID)
    {
        float p1_result = 0.0f;
        p1_result += Input.GetAxis("1_J_LowerColumn");
        p1_result += Input.GetAxis("1_K_LowerColumn");

        float p2_result = 0.0f;
        p2_result += Input.GetAxis("2_J_LowerColumn");

        float p3_result = 0.0f;
        float p4_result = 0.0f;

        return returnClampedAxis(pPlayerID, p1_result, p2_result, p3_result, p4_result);
    }

    /// <summary>
    /// Float value between 0 and 1 if the axis is detecting input. Non-zero value confirms input.
    /// </summary>
    /// <param name="pPlayerID"></param>
    /// <returns></returns>
    public static float FlashButton(int pPlayerID)
    {
        float p1_result = 0.0f;
        p1_result += Input.GetAxis("1_J_FlashButton");
        p1_result += Input.GetAxis("1_K_FlashButton");

        float p2_result = 0.0f;
        p2_result += Input.GetAxis("2_J_FlashButton");

        float p3_result = 0.0f;
        float p4_result = 0.0f;

        return returnClampedAxis(pPlayerID, p1_result, p2_result, p3_result, p4_result);
    }

    /// <summary>
    /// Float value between 0 and 1 if the axis is detecting input. Non-zero value confirms input.
    /// </summary>
    /// <param name="pPlayerID"></param>
    /// <returns></returns>
    public static float ThrowButton(int pPlayerID)
    {
        float p1_result = 0.0f;
        p1_result += Input.GetAxis("1_J_ThrowButton");
        p1_result += Input.GetAxis("1_K_ThrowButton");

        float p2_result = 0.0f;
        p2_result += Input.GetAxis("2_J_ThrowButton");

        float p3_result = 0.0f;
        float p4_result = 0.0f;

        return returnClampedAxis(pPlayerID, p1_result, p2_result, p3_result, p4_result);
    }

    //QUICK FIX: PLEASE DELETE
    public static float InvertButton(int pPlayerID)
    {
        float p1_result = 0.0f;
        p1_result += Input.GetAxis("1_J_InvertButton");
        p1_result += Input.GetAxis("1_K_InvertButton");

        float p2_result = 0.0f;
        p2_result += Input.GetAxis("2_J_InvertButton");

        float p3_result = 0.0f;
        float p4_result = 0.0f;

        return returnClampedAxis(pPlayerID, p1_result, p2_result, p3_result, p4_result);
    }

    public static float PauseButton(int pPlayerID)
    {
        float p1_result = 0.0f;
        p1_result += Input.GetAxis("1_J_PauseButton");
        p1_result += Input.GetAxis("1_K_PauseButton");

        float p2_result = 0.0f;
        p2_result += Input.GetAxis("2_J_PauseButton");

        float p3_result = 0.0f;
        float p4_result = 0.0f;

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
