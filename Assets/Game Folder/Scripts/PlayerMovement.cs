using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody _rigidBody;

    private bool _flashThrowBeforeFlash;

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Move the player in a given direction.
    /// </summary>
    /// <param name="pDirection"></param>
    public void Move(Vector3 pDirection, float pMovementSpeed)
    {
        pDirection.Normalize();
        Vector3 movement = transform.TransformDirection(pDirection) * pMovementSpeed * Time.deltaTime;
        _rigidBody.MovePosition(transform.position + movement);
    }


    //NOTES: Flashing works, looks kinda oke actually, not sure if we need the smoothcamera, need feedback on this.
    //Fmod needs to be implemented, for now focus on other stuff, fix this later.
    //We need: - int PlayerID, float groundFloorValue, bool pBallposession
    public void Flash(Vector3 pDirection, float pFlashDistance, float pGroundFloorYValue, bool pBallPosession, bool pFlashThrowBeforeFlash)
    {
        pDirection.Normalize();

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
        Vector3 afterFlashFailPosition = transform.position + (transform.TransformVector(pDirection * pFlashDistance));
        Vector3 afterFlashSucceedPosition = afterFlashFailPosition;
        afterFlashSucceedPosition.y = pGroundFloorYValue;

        //fire a ray to find our next possible column
        Ray secondRay = new Ray(afterFlashFailPosition, -transform.up);
        RaycastHit secondHitInfo;
        if (Physics.Raycast(secondRay, out secondHitInfo))
        {
            possibleNextColumn = secondHitInfo.collider.gameObject;
        }

        if (_flashThrowBeforeFlash && pBallPosession)
        {
            FlashThrow();
        }

        if (currentColumn == possibleNextColumn)
        {
            //FMOD
            //FMODUnity.RuntimeManager.PlayOneShot(flashSound, _cameraScript.gameObject.transform.position);

            //Smooth camera movement from Josh
            //Vector3 deltaToPlayer = playerCamera.transform.position - gameObject.transform.position;
            //playerCamera.GetComponent<PlayerCamera>().ToggleSmoothFollow(true, deltaToPlayer, playerCamera.transform.position);


            //playerActions.Flash(afterFlashFailPosition);
            transform.position = afterFlashFailPosition;
        }
        else
        {
            //FMOD
            //FMODUnity.RuntimeManager.PlayOneShot(flashSound, _cameraScript.gameObject.transform.position);

            //Smooth camera movement from Josh
            //Vector3 deltaToPlayer = playerCamera.transform.position - gameObject.transform.position;
            //playerCamera.GetComponent<PlayerCamera>().ToggleSmoothFollow(true, deltaToPlayer, playerCamera.transform.position);

            //playerActions.Flash(afterFlashSucceedPosition);
            transform.position = afterFlashSucceedPosition;
        }

        if(!_flashThrowBeforeFlash && pBallPosession)
        {
            FlashThrow();
        }

        //if (_state == PlayerState.CARRYINGBALL)
        //{
        //    playerActions.Throw(transform.forward, true);
        //    Able2Throw(false);
        //}
    }

    private void FlashThrow()
    {
        GameObject ball = GameObject.FindGameObjectWithTag("Ball");
        Rigidbody ballRigidbody = ball.GetComponent<Rigidbody>();

        //pDirection = Quaternion.AngleAxis(-playerProperties.GetForcedThrowRotationAddition(), gameObject.transform.right) * pDirection;
        //ballRigidbody.AddForce(pDirection * playerProperties.GetThrowingForce());
    }
}
