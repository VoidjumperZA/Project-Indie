﻿using UnityEngine;
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
    private float _baseYValue;

    private float _polarity;
    private float _movementDelta = 0.0f;

    private float _columnSpeed;

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

    public void SetDataValues(float pBaseYValue, float pColumnDisplacementSize, float pColumnMovementAccelerationSpeed, float pColumnMovementMaxSpeed, float pColumnResettingSpeed)
    {
        _baseYValue = pBaseYValue;
        _columnDisplacementSize = pColumnDisplacementSize;
        _columnMovementAccelerationSpeed = pColumnMovementAccelerationSpeed;
        _columnMovementMaxSpeed = pColumnMovementMaxSpeed;
        _columnResettingSpeed = pColumnResettingSpeed;
    }


    public void ResetColumn(float pColumnResettingSpeed, float pColumnDisplacementSize, float pBaseYValue, float pPolarity)
    {
        _columnResettingSpeed = pColumnResettingSpeed;
        _columnDisplacementSize = pColumnDisplacementSize;
        _baseYValue = pBaseYValue;
        _polarity = pPolarity;
        _atBaseLevel = false;
    }

    private void grindToArenaLevel()
    {
        if(_movementDelta < _columnDisplacementSize)
        //if (Mathf.Abs(gameObject.transform.position.y) + (columnDisplacementSize / 2) > baseYValue)
        {
            gameObject.transform.Translate(0, _polarity * _columnResettingSpeed, 0);
            _movementDelta += _columnResettingSpeed;
        }
        else
        {
            //Debug.Log("My pos before .set is: " + gameObject.transform.position + " while the baseY value I was given is:" + _baseYValue);
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, _baseYValue, gameObject.transform.position.z);                   
            //Debug.Log("My pos after .set is: " + gameObject.transform.position + " while the baseY value I was given is:" + _baseYValue);
            _movementDelta = 0.0f;
            _atBaseLevel = true;
            columnStatus = ColumnStatus.Free;
        }

    }

    //actually move the column
    private void moveColumn(float pPolarity)
    {
        //if column is not yet at the height of it's end position
        if (_baseYValue + Mathf.Abs(gameObject.transform.position.y) + (_baseYValue / 2) < _columnDisplacementSize && IsColumnMoving() == true)
        {
            //increase the speed of the column to give it natural acceleration
            _columnSpeed += _columnMovementAccelerationSpeed;
            if (_columnSpeed >= _columnMovementMaxSpeed)
            {
                _columnSpeed = _columnMovementMaxSpeed;
            }
            gameObject.transform.Translate(0, pPolarity * _columnSpeed, 0);
            //Debug.Log("Speed: " + columnSpeed + "lossyScale: " + selectedColumn.transform.lossyScale.y + ", localScale: " + selectedColumn.transform.localScale.y);
        }
        else
        {
            //stop the column moving, which should deactive both columnRising and columnLowering
            columnHalted();
            _columnSpeed = 0.0f;
            ResetColumn(_columnResettingSpeed, _columnDisplacementSize, _baseYValue, (pPolarity * -1));
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
}
