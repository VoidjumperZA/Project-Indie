using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{
    private Rigidbody _rigidbody;

    private void Start()
    {
       // _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {

    }

    private void OnCollisionEnter(Collision pCollision)
    {
        PlayerMovement movement = pCollision.gameObject.GetComponent<PlayerMovement>();
        if(movement != null)
        {
            transform.SetParent(movement.transform);
            changePlayerState(movement);
            //_rigidbody.velocity = Vector3.zero;
            transform.localPosition = new Vector3(0, 1, 0);

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
