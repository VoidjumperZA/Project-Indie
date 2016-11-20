using UnityEngine;
using System.Collections;

public static class InputManager
{
    //Joysticks and WASD movement
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

    public static Vector3 Movement(int pPlayerID)
    {
        return new Vector3(MovementHorizontal(pPlayerID), 0, MovementVertical(pPlayerID));
    }

    //Mouse camera movement
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

    public static Vector3 CameraMovement(int pPlayerID)
    {
        return new Vector3(CameraHorizontal(pPlayerID), 0, CameraVertical(pPlayerID));
    }

    //Triggers / ColumnControl
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
    /*
    //Buttons
    public static bool P1_FlashButton()
    {
        return Input.GetButtonDown("1_FlashButton");
    }

    public static bool P1_ThrowButton()
    {
        return Input.GetButtonDown("1_ThrowButton");
    }*/

    /*
    //Buttons
    public static bool P2_FlashButton()
    {
        return Input.GetButtonDown("2_J_FlashButton");
    }

    public static bool P2_ThrowButton()
    {
        return Input.GetButtonDown("2_J_ThrowButton");
    }*/

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
