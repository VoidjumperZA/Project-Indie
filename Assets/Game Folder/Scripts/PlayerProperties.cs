using UnityEngine;
using System.Collections;

public class PlayerProperties : MonoBehaviour
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
    [SerializeField]
    private float _forcedThrowRotationAddition;
    [SerializeField]
    private float _columnMovementCooldownValue;
    [SerializeField]
    private float _flashCooldownValue;
    [SerializeField]
    private float _flashDistance;

    // Use this for initialization
    void Start()
    {
        //30
        //0.5
        //1300
        //25
        //30

     
    }

    // Update is called once per frame
    void Update()
    {

    }

    public float GetMaxMovementSpeed()
    {
        return _maxMovementSpeed;
    }

    public float GetAccelerationSpeed()
    {
        return _accelerationSpeed;
    }

    public float GetThrowingForce()
    {
        return _throwingForce;
    }

    public float GetThrowRotationAddition()
    {
        return _throwRotationAddition;
    }

    public float GetForcedThrowRotationAddition()
    {
        return _forcedThrowRotationAddition;
    }

    public float GetColumnMovementCooldownValue()
    {
        return _columnMovementCooldownValue;
    }

    public float GetFlashCooldownValue()
    {
        return _flashCooldownValue;
    }

    public float GetFlashDistance()
    {
        return _flashDistance;
    }
}
