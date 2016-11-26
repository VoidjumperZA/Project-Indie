using UnityEngine;
using System.Collections;

public class ColumnProperties : MonoBehaviour
{
    private enum ColumnType { Dynamic, Static };
    public enum ColumnStatus { Free, Locked };

    [SerializeField]
    private ColumnType _columnType;

    [HideInInspector]
    public ColumnStatus columnStatus;

    private bool _atBaseLevel = true;
    private bool _columnRising = false;
    private bool _columnLowering = false;

    private float _columnDisplacementSize;
    private float _columnMovementAccelerationSpeed;
    private float _columnMovementMaxSpeed;
    private float _columnResettingSpeed;
    private float _columnTimeUntilReset;
    private float _baseYValue;

    private float _polarity;
    private float _resettingMovementDelta = 0.0f;
    private float _orignalMovementDelta = 0.0f;
    private float _columnSpeed;

    private int playerOwnerID;

    //VYTAUTAS' FMOD IMPLEMENTATION BEGINS

    public string hexSound = "event:/Hexagons";
    public FMOD.Studio.EventInstance hexSoundEv;
    public FMOD.Studio.ParameterInstance directionParam;

    //VYTAUTAS' FMOD IMPLEMENTATION ENDS

    private void Start()
    {
        columnStatus = ColumnStatus.Free;
    }

    private void Update()
    {
        updateColumnPosition();
        if (_atBaseLevel == false)
        {
            grindToArenaLevel();
        }
    }

    public void SetDataValues(float pBaseYValue, float pColumnDisplacementSize, float pColumnMovementAccelerationSpeed, float pColumnMovementMaxSpeed, float pColumnResettingSpeed, float pColumnTimeUntilReset, int pPlayerOwnerID)
    {
        _baseYValue = pBaseYValue;
        _columnDisplacementSize = pColumnDisplacementSize;
        _columnMovementAccelerationSpeed = pColumnMovementAccelerationSpeed;
        _columnMovementMaxSpeed = pColumnMovementMaxSpeed;
        _columnResettingSpeed = pColumnResettingSpeed;
        _columnTimeUntilReset = pColumnTimeUntilReset;
        playerOwnerID = pPlayerOwnerID;
    }


    public void ResetColumn(float pColumnResettingSpeed, float pColumnDisplacementSize, float pPolarity)
    {
        _columnResettingSpeed = pColumnResettingSpeed;
        _columnDisplacementSize = pColumnDisplacementSize;
        _polarity = pPolarity;      
        _atBaseLevel = false;

        //FMOD
        hexSoundEv = FMODUnity.RuntimeManager.CreateInstance(hexSound);
        hexSoundEv.getParameter("Direction", out directionParam);
        FMODUnity.RuntimeManager.PlayOneShot(hexSound, gameObject.transform.position);
        //
    }

    private IEnumerator waitUntilGrindToArena(float pPolarity)
    {
        yield return new WaitForSeconds(_columnTimeUntilReset);
        ResetColumn(_columnResettingSpeed, _columnDisplacementSize, (pPolarity * -1));
    }


    private void grindToArenaLevel()
    {
        Debug.Log("movementDelta is " + _resettingMovementDelta + " and my column displacement size is " + _columnDisplacementSize);
        if(_resettingMovementDelta < _columnDisplacementSize)
        {
            gameObject.transform.Translate(0, _polarity * _columnResettingSpeed, 0);
            _resettingMovementDelta += _columnResettingSpeed;
        }
        else
        {
            Debug.Log("My pos before .set is: " + gameObject.transform.position + " while the baseY value I was given is:" + _baseYValue);
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, _baseYValue, gameObject.transform.position.z);                   
            Debug.Log("My pos after .set is: " + gameObject.transform.position + " while the baseY value I was given is:" + _baseYValue);
            _resettingMovementDelta = 0.0f;
            _atBaseLevel = true;
            columnStatus = ColumnStatus.Free;
        }

    }

    //actually move the column
    private void moveColumn(float pPolarity)
    {
        //if column is not yet at the height of it's end position
        //if (_baseYValue + Mathf.Abs(gameObject.transform.position.y) + (_baseYValue / 2) < _columnDisplacementSize && IsColumnMoving() == true)
        if(_orignalMovementDelta <  _columnDisplacementSize && IsColumnMoving() == true)
        {
            //increase the speed of the column to give it natural acceleration
            _columnSpeed += _columnMovementAccelerationSpeed;
            if (_columnSpeed >= _columnMovementMaxSpeed)
            {
                _columnSpeed = _columnMovementMaxSpeed;
            }
            gameObject.transform.Translate(0, pPolarity * _columnSpeed, 0);
            _orignalMovementDelta += _columnSpeed;
        }
        else
        {
            //stop the column moving, which should deactive both columnRising and columnLowering
            _orignalMovementDelta = 0.0f;
            columnHalted();
            _columnSpeed = 0.0f;
            StartCoroutine(waitUntilGrindToArena(pPolarity));            
        }
    }

    private void updateColumnPosition()
    {
        if (_columnRising == true)
        {
            moveColumn(1.0f);
        }
        if (_columnLowering == true)
        {
            moveColumn(-1.0f);
        }
    }

    public bool IsColumnMoving()
    {
        //if the column is moving in any direction mark it as moving
        if (_columnRising == true || _columnLowering == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void columnHalted()
    {
        _columnRising = false;
        _columnLowering = false;
    }

    public int GetColumnType()
    {
        return (int)_columnType;
    }

    public int GetColumnStatus()
    {
        return (int)columnStatus;
    }

    public void ToggleColumnRising(bool pState)
    {
        _columnRising = pState;
    }

    public void ToggleColumnLowering(bool pState)
    {
        _columnLowering = pState;
    }

    public int GetOwnerID()
    {
        return playerOwnerID;
    }
}
