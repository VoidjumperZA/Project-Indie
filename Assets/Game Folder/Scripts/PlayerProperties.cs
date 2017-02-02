using UnityEngine;
using System.Collections;

public class PlayerProperties : MonoBehaviour
{
    //DESIGNER Inspector values, once chosen a nice value, these can be replaced with "hard coded" values
    [Header("General properties")]
    [SerializeField]
    private float _MovementSpeed;
    [SerializeField]
    private float _jumpForce;
    [SerializeField]
    private float _addedGravity;
    [SerializeField]
    private float _ballPosessionTime;
    [SerializeField]
    private float _timeAdditionOnPickUpBall;
    [SerializeField]
    private float _ballCoolOffTime;
    [SerializeField]
    private float _columnMovementCooldownValue;
    [SerializeField]
    private float _manaRespawnTime;
    [Header("Throw properties")]
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
    [Header("Flash properties")]
    [SerializeField]
    private bool _flashThrowBeforeFlash;
    [SerializeField]
    private float _flashDistance;
    [SerializeField]
    private float _smoothFollowIncrement;
    [SerializeField]
    private float _smoothFollowClipDistance;
    [SerializeField]
    private float _flashManaCost;
    [Header("Mana properties")]
    [SerializeField]
    private float _startingManaValue;
    [SerializeField]
    private float _maxManaValue;
    [SerializeField]
    private float _manaValueOnPickUp;

    private void Start()
    {
        ModifyCooldownValues();
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
    public float GetSmoothFollowIncrement()
    {
        return _smoothFollowIncrement;
    }
    public float GetSmoothFollowClipDistance()
    {
        return _smoothFollowClipDistance;
    }
    public float GetTimeAdditionOnPickUpBall()
    {
        return _timeAdditionOnPickUpBall;
    }
    public float GetBallCoolOffTime()
    {
        return _ballCoolOffTime;
    }

    public float GetManaRespawnTime()
    {
        return _manaRespawnTime;
    }

    public void ModifyCooldownValues()
    {
        //if 0.5, move these down  ||  
        _columnMovementCooldownValue *= LobbySettings.GetModifierValues().x;
        _flashManaCost *= LobbySettings.GetModifierValues().x;
        GameObject.Find("Manager").GetComponent<PossessedHexes>().ModifyCooldownValue(LobbySettings.GetModifierValues().x);

        //yet do the inverse to these
        _manaValueOnPickUp *= LobbySettings.GetModifierValues().y;
        _maxManaValue *= LobbySettings.GetModifierValues().y;
    }
}
