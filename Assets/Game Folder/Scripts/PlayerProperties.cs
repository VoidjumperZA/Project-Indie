using UnityEngine;
using System.Collections;

public class PlayerProperties : MonoBehaviour
{
    //DESIGNER Inspector values, once chosen a nice value, these can be replaced with "hard coded" values
    [SerializeField]
    private float _MovementSpeed;
    [SerializeField]
    private float _jumpForce;
    [SerializeField]
    private float _throwingForce;
    [SerializeField]
    private float _flashThrowingForce;
    [SerializeField]
    private float _forcedThrowingForce;
    [SerializeField]
    private float _throwRotationAddition;
    [SerializeField]
    private float _flashThrowRotationAddition;
    [SerializeField]
    private float _forcedThrowRotationAddition;
    [SerializeField]
    private bool _flashThrowBeforeFlash;
    [SerializeField]
    private float _flashDistance;
    [SerializeField]
    private float _flashManaCost;
    [SerializeField]
    private float _startingManaValue;
    [SerializeField]
    private float _maxManaValue;
    [SerializeField]
    private float _manaValueOnPickUp;
    [SerializeField]
    private float _jumpCooldownValue;
    [SerializeField]
    private float _flashCooldownValue;
    [SerializeField]
    private float _columnMovementCooldownValue;
    [SerializeField]
    private float _addedGravity;
    [SerializeField]
    private float _ballPosessionTime;


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

    public float GetManaValueOnPickUp()
    {
        return _manaValueOnPickUp;
    }

    public float GetMaxManaValue()
    {
        return _maxManaValue;
    }

    public float GetStartingManaValue()
    {
        return _startingManaValue;
    }

    public float GetFlashManaCost()
    {
        return _flashManaCost;
    }

    public float GetJumpForce()
    {
        return _jumpForce;
    }

    public float GetJumpCooldownValue()
    {
        return _jumpCooldownValue;
    }

    public float GetBallPosessionTime()
    {
        return _ballPosessionTime;
    }

    public float GetForcedThrowingForce()
    {
        return _forcedThrowingForce;
    }

    public float GetForcedThrowRotationAddition()
    {
        return _forcedThrowRotationAddition;
    }
}
