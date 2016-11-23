using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    private PlayerProperties playerProperties;
    private float _actualSpeed;

    //Could be changed for an enum later on, for now we can use a Boolean
    private bool _ballPosession = false;

    private Rigidbody _rigidBody;

    private void Start()
    {
        playerProperties = GameObject.Find("Manager").GetComponent<PlayerProperties>();
        _rigidBody = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Move the player in a given direction while applying acceleration.
    /// </summary>
    /// <param name="pDirection"></param>
    public void Move(Vector3 pDirection)
    {
        if (pDirection.magnitude == 0)
        {
            _actualSpeed = 0.0f;
        }
        else
        {
            _actualSpeed += playerProperties.GetAccelerationSpeed();
            _actualSpeed = Mathf.Min(playerProperties.GetMaxMovementSpeed(), _actualSpeed);
        }
        transform.Translate(pDirection * _actualSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Release the ball in the direction aimed, putting the ball back into play.
    /// </summary>
    /// <param name="pDirection"></param>
    public void Throw(Vector3 pDirection, bool pForced)
    {
        //get the ball's gameObject
        GameObject ball = GameObject.FindGameObjectWithTag("Ball");
        Rigidbody ballRigidbody = ball.GetComponent<Rigidbody>();
        ball.GetComponent<Ball>().TogglePossession(false);
        if (pForced == false)
        {
            pDirection = Quaternion.AngleAxis(-playerProperties.GetThrowRotationAddition(), transform.right) * pDirection;
        }
        else
        {
            pDirection = Quaternion.AngleAxis(-playerProperties.GetForcedThrowRotationAddition(), transform.right) * pDirection;
        }
        ballRigidbody.AddForce(pDirection * playerProperties.GetThrowingForce());
    }

    public void Throw(Vector3 pDirection)
    {
        //get the ball's gameObject
        GameObject ball = GameObject.FindGameObjectWithTag("Ball");
        Rigidbody ballRigidbody = ball.GetComponent<Rigidbody>();
        ball.GetComponent<Ball>().TogglePossession(false);
        ballRigidbody.AddForce(pDirection * playerProperties.GetThrowingForce());
    }

    /// <summary>
    /// Teleport the player forward a short amount while also throwing the ball ahead of them.
    /// </summary>
    public void Flash(Vector3 pPosition)
    {
        //raycast down and look which column you are hitting, then look at the position after the flash, raycast down and see if it is another column
        transform.position = pPosition;
    }

    public float GetFlashDistance()
    {
        return playerProperties.GetFlashDistance();
    }
}
