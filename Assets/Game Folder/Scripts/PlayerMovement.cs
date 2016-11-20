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

    private float _actualSpeed;

    //Could be changed for an enum later on, for now we can use a Boolean
    private bool _ballPosession = false;

    private Rigidbody _rigidBody;

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 pDirection)
    {
        if(pDirection.magnitude == 0)
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

    public void Throw(Vector3 pDirection)
    {
        print("movement throw");
        Transform ball = transform.FindChild("Ball_Test");
        ball.SetParent(GameObject.Find("TestBench").transform);
        Rigidbody ballRigidbody = ball.GetComponent<Rigidbody>();
        ballRigidbody.useGravity = true;
        ballRigidbody.AddForce(pDirection * _throwingForce);
    }

    public void Flash()
    {

    }
}
