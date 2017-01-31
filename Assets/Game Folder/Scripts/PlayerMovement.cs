using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float _distanceToGround;
    [SerializeField]
    private LayerMask _ground;

    private Rigidbody _rigidBody;
    private Vector3 _newVelocity;
    private Rigidbody _ballRigidbody;
    private Ball _ballScript;
    private PlayerCamera _playerCamera;
    private bool _flashThrowBeforeFlash;

    private float _characterWidth, _characterDepth;

    private void Start()
    {
        _distanceToGround = 0.1f;
        _ground = 1;
        _rigidBody = GetComponent<Rigidbody>();
        _newVelocity = Vector3.zero;
        GameObject ball = GameObject.FindGameObjectWithTag("Ball");
        _ballRigidbody = ball.GetComponent<Rigidbody>();
        _ballScript = ball.GetComponent<Ball>();

        MeshRenderer renderer = GetComponent<MeshRenderer>();
        _distanceToGround += renderer.bounds.extents.y;
        _characterWidth = renderer.bounds.extents.x * 0.9f;
        _characterDepth = renderer.bounds.extents.z * 0.9f;
    }

    /// <summary>
    /// Move the player in a given direction.
    /// </summary>
    /// <param name="pDirection"></param>
    public void Move(Vector3 pDirection, float pMovementSpeed)
    {
        Vector3 characterSpaceVelocity = transform.TransformDirection(pDirection) * pMovementSpeed;
        _newVelocity.x = characterSpaceVelocity.x;
        _newVelocity.z = characterSpaceVelocity.z;
        //_rigidBody.MovePosition(transform.position + velocity);
    }

    public void Jump(float pJumpForce)
    {
        //Vector3 force = transform.up * pJumpForce;
        //_rigidBody.AddRelativeForce(force, ForceMode.Impulse);
        _newVelocity.y = pJumpForce;
    }

    public void ApplyVelocity()
    {
        //print(_newVelocity);
        _rigidBody.velocity = _newVelocity;
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

        if (pFlashThrowBeforeFlash == true && pBallPosession == true)
        {
            flashThrow(pFlashThrowingForce, pFlashThrowRotationAddition);
        }

        //Do we flash on the same column? yes-> afterFlashFailPosition, no-> afterFlashSucceedPosition 
        transform.position = currentColumn == possibleNextColumn ? afterFlashFailPosition : afterFlashSucceedPosition;

        //Note: For some reason using _rigidbody.moveposition sometimes did not flash at all

        //if (currentColumn == possibleNextColumn)
        //{
        //    _rigidBody.MovePosition(afterFlashFailPosition);
        //    _rigidBody.position = afterFlashFailPosition;
        //}
        //else
        //{
        //    _rigidBody.MovePosition(afterFlashSucceedPosition);
        //    _rigidBody.position = afterFlashSucceedPosition;
        //}

        if (pFlashThrowBeforeFlash == false && pBallPosession == true)
        {
            _ballRigidbody.MovePosition(transform.position + _ballScript.GetBallOffset());
            flashThrow(pFlashThrowingForce, pFlashThrowRotationAddition);
        }
    }

    private void flashThrow(float pFlashThrowingForce, float pFlashThrowRotationAddition)
    {
        Vector3 Direction = Quaternion.AngleAxis(-pFlashThrowRotationAddition, transform.right) * transform.forward;
        _ballScript.TogglePossession(false);
        _ballRigidbody.AddForce(Direction * pFlashThrowingForce);
    }

    public void ApplyGravity(float pAddedGravity)
    {
        //_rigidBody.AddRelativeForce(new Vector3(0.0f, -pAddedGravity, 0.0f) * Time.deltaTime);
        _newVelocity.y -= pAddedGravity;
    }

    public void ResetGravity()
    {
        //print("RESET");
        _newVelocity.y = 0.0f;
    }

    public bool Grounded()
    {
        Ray[] rays = new Ray[5];
        rays[0] = new Ray(transform.position + transform.TransformVector(new Vector3(_characterWidth, 0, _characterDepth)), Vector3.down);
        rays[1] = new Ray(transform.position + transform.TransformVector(new Vector3(_characterWidth, 0, -_characterDepth)), Vector3.down);
        rays[2] = new Ray(transform.position + transform.TransformVector(new Vector3(-_characterWidth, 0, _characterDepth)), Vector3.down);
        rays[3] = new Ray(transform.position + transform.TransformVector(new Vector3(-_characterWidth, 0, -_characterDepth)), Vector3.down);
        rays[4] = new Ray(transform.position, Vector3.down);

        for (int rayIndex = 0; rayIndex < rays.Length; rayIndex++)
        {
            Debug.DrawRay(rays[rayIndex].origin, rays[rayIndex].direction, Color.red);
            if (Physics.Raycast(rays[rayIndex], _distanceToGround, _ground))
                return true;
        }
        return false;
    }
}
