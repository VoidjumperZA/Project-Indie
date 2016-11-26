using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    private PlayerProperties _playerProperties;
    private Rigidbody _rigidBody;

    private void Start()
    {
        _playerProperties = GameObject.Find("Manager").GetComponent<PlayerProperties>();
        _rigidBody = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Move the player in a given direction.
    /// </summary>
    /// <param name="pDirection"></param>
    public void Move(Vector3 pDirection)
    {
        pDirection.Normalize();
        Vector3 movement = transform.TransformDirection(pDirection) * _playerProperties.GetMaxMovementSpeed() * Time.deltaTime;
        _rigidBody.MovePosition(transform.position + movement);
    }
}
