using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    //DESIGNER Inspector values, once chosen a nice value, these can be replaced with "hard coded" values
    [SerializeField]
    private float _maxMovementSpeed;
    [SerializeField]
    private float _accelerationSpeed;
    [SerializeField]
    private float _throwingForce;
    [SerializeField]
    private float _throwRotationAddition;

    private float _actualSpeed;

    //Could be changed for an enum later on, for now we can use a Boolean
    private bool _ballPosession = false;

    private Rigidbody _rigidBody;

    private void Start()
    {
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
            _actualSpeed += _accelerationSpeed;
            _actualSpeed = Mathf.Min(_maxMovementSpeed, _actualSpeed);
        }
        transform.Translate(pDirection * _actualSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Release the ball in the direction aimed, putting the ball back into play.
    /// </summary>
    /// <param name="pDirection"></param>
    public void Throw(Vector3 pDirection)
    {
        //get the ball's gameObject
        GameObject ball = GameObject.FindGameObjectWithTag("Ball");
        Rigidbody ballRigidbody = ball.GetComponent<Rigidbody>();
        ball.GetComponent<Ball>().TogglePossession(false);
        pDirection = Quaternion.AngleAxis(-_throwRotationAddition, transform.right) * pDirection;
        ballRigidbody.AddForce(pDirection * _throwingForce);
    }

    /// <summary>
    /// Teleport the player forward a short amount while also throwing the ball ahead of them.
    /// </summary>
    public void Flash()
    {

    }
}
