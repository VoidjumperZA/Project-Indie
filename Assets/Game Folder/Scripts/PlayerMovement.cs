using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    //DESIGNER Inspector values, once chosen a nice value, these can be replaced with "hard coded" values
    [SerializeField]
    private float _maxMovementSpeed;
    [SerializeField]
    private float _accelerationSpeed;

    //Could be changed for an enum later on, for now we can use a Boolean
    private bool _ballPosession = false;

    //Need to test this (rigidBody)
    private Rigidbody _rigidBody;

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        if(_rigidBody.velocity.magnitude < _maxMovementSpeed)
        {
            _rigidBody.AddRelativeForce(InputManager.Movement() * _accelerationSpeed);
        }

        //print("velocity: " + _rigidBody.velocity.magnitude);
    }
}
