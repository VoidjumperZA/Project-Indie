using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody _rigidBody;
    private Rigidbody _ballRigidbody;
    private Ball _ballScript;
    private bool _flashThrowBeforeFlash;

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        GameObject ball = GameObject.FindGameObjectWithTag("Ball");
        _ballRigidbody = ball.GetComponent<Rigidbody>();
        _ballScript = ball.GetComponent<Ball>();
    }

    /// <summary>
    /// Move the player in a given direction.
    /// </summary>
    /// <param name="pDirection"></param>
    public void Move(Vector3 pDirection, float pMovementSpeed)
    {
        Vector3 movement = transform.TransformDirection(pDirection) * pMovementSpeed * Time.deltaTime;
        _rigidBody.MovePosition(transform.position + movement);
    }


    //NOTES: Flashing works, looks kinda oke actually, not sure if we need the smoothcamera, need feedback on this.
    //Fmod needs to be implemented, for now focus on other stuff, fix this later.
    //We need: - int PlayerID, float groundFloorValue, bool pBallposession
    public void Flash(Vector3 pFlashDirection, float pFlashDistance, float pSpawnHeight, float pFlashThrowingForce, float pFlashThrowRotationAddition, bool pBallPosession, bool pFlashThrowBeforeFlash)
    {
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

        //Create the positions of where we'll be after a flash
        Vector3 afterFlashFailPosition = transform.position + (transform.TransformVector(pFlashDirection * pFlashDistance));
        Vector3 afterFlashSucceedPosition = afterFlashFailPosition;
        afterFlashSucceedPosition.y = pSpawnHeight;

        //fire a ray to find our next possible column
        Ray secondRay = new Ray(afterFlashFailPosition, -transform.up);
        RaycastHit secondHitInfo;
        if (Physics.Raycast(secondRay, out secondHitInfo))
        {
            possibleNextColumn = secondHitInfo.collider.gameObject;
        }

        if (_flashThrowBeforeFlash && pBallPosession)
        {
            flashThrow(pFlashThrowingForce, pFlashThrowRotationAddition);
        }

        if (currentColumn == possibleNextColumn)
        {
            //Smooth camera movement from Josh
            //Vector3 deltaToPlayer = playerCamera.transform.position - gameObject.transform.position;
            //playerCamera.GetComponent<PlayerCamera>().ToggleSmoothFollow(true, deltaToPlayer, playerCamera.transform.position);

            transform.position = afterFlashFailPosition;
        }
        else
        {
            //Smooth camera movement from Josh
            //Vector3 deltaToPlayer = playerCamera.transform.position - gameObject.transform.position;
            //playerCamera.GetComponent<PlayerCamera>().ToggleSmoothFollow(true, deltaToPlayer, playerCamera.transform.position);

            transform.position = afterFlashSucceedPosition;
        }

        if(!_flashThrowBeforeFlash && pBallPosession)
        {
            flashThrow(pFlashThrowingForce ,pFlashThrowRotationAddition);
        }
    }

    private void flashThrow(float pFlashThrowingForce ,float pFlashThrowRotationAddition)
    {
        Vector3 Direction = Quaternion.AngleAxis(-pFlashThrowRotationAddition, transform.right) * transform.forward;
        _ballScript.TogglePossession(false);
        _ballRigidbody.AddForce(Direction * pFlashThrowingForce);
    }
}
