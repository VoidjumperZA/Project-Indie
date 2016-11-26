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

}
