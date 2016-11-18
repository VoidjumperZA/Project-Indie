using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{
    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {

    }

    private void OnCollisionEnter(Collision pCollision)
    {

    }

    private void changePlayerState(PlayerMovement pMovement)
    {

    }

    private void score()
    {

    }
}
