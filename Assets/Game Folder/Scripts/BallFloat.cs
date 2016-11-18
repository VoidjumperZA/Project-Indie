using UnityEngine;
using System.Collections;

public class BallFloat : MonoBehaviour
{
    private Rigidbody _rigidBody;
    private bool _inside = false;

    private void Start()
    {

    }

    private void FixedUpdate()
    {
        if(_inside)
        {
            _rigidBody.AddForce(0, 20, 0);
        }
    }

    private void OnTriggerEnter(Collider pCollider)
    {
        if (pCollider.gameObject.name == "Ball_Test")
        {
            print("enter");
            _inside = true;
            _rigidBody = pCollider.GetComponent<Rigidbody>();
        }
    }

    private void OnTriggerExit(Collider pCollider)
    {
        if (pCollider.name == "Ball_Test")
        {
            print("exit");
            _inside = false;
        }
    }
}
