using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private GameObject owner;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {

    }

    private void OnCollisionEnter(Collision pCollision)
    {
        PlayerMovement movement = pCollision.gameObject.GetComponent<PlayerMovement>();
        if (movement != null)
        {
            transform.SetParent(movement.transform);
            changePlayerState(movement);
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.useGravity = false;
            //_rigidbody.freezeRotation = false;
            transform.localPosition = new Vector3(0, 1, 0);

            //VOID
            owner = movement.gameObject;
        }
    }

    private void changePlayerState(PlayerMovement pMovement)
    {
        print("HELLO ITS A ME MARIO");
    }

    private void score()
    {

    }
}
