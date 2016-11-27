using UnityEngine;
using System.Collections;

public class PlayerProperties : MonoBehaviour
{
    //DESIGNER Inspector values, once chosen a nice value, these can be replaced with "hard coded" values
    [SerializeField]
    private float _MovementSpeed;
    [SerializeField]
    private float _throwingForce;
    [SerializeField]
    private float _throwRotationAddition;
    [SerializeField]
    private bool _flashThrowBeforeFlash;
    [SerializeField]
    private float _flashThrowingForce;
    [SerializeField]
    private float _flashThrowRotationAddition;
    [SerializeField]
    private float _columnMovementCooldownValue;
    [SerializeField]
    private float _flashCooldownValue;
    [SerializeField]
    private float _flashDistance;
    [SerializeField]
    private float _addedGravity;

    void Start()
    {
        //Standard values

        //Movement Speed:                   30
        //Throwing Force:                   1900
        //Throw Rotation Addition:          25
        //Flash Throw Before Flash:         true
        //Flash Throwing Force:             1900
        //Flash Throw Rotation Addition:    45
        //Column Movement Cooldown:         120
        //Flash Cooldown Value:             120
        //Flash Distance:                   20
        //Added gravity:                    1180000

        //For Dom where to start after dinner: making states/booleans in PlayerInput, Fmod
    }

    public float GetMovementSpeed()
    {
        return _MovementSpeed;
    }

    public float GetThrowingForce()
    {
        return _throwingForce;
    }

    public float GetThrowRotationAddition()
    {
        return _throwRotationAddition;
    }

    public bool GetFlashThrowBeforeFlash()
    {
        return _flashThrowBeforeFlash;
    }

    public float GetFlashThrowingForce()
    {
        return _flashThrowingForce;
    }

    public float GetFlashThrowRotationAddition()
    {
        return _flashThrowRotationAddition;
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

    public float GetAddedGravity()
    {
        return _addedGravity;
    }
}
