using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float _movementSpeed;

    //Could be changed for an enum later on, for now we can use a Boolean
    private bool _ballPosession = false;

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
        if(_rigidBody.velocity.magnitude < _movementSpeed)
        {
            _rigidBody.AddForce(InputManager.Movement() * 10);
        }
    }
}
