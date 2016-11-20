using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ColumnControl : MonoBehaviour
{
    [SerializeField]
    private GameObject _unmovingColumn;

    [SerializeField]
    private float _columnDisplacementSize;

    [SerializeField]
    private float _columnMovementAccelerationSpeed;

    [SerializeField]
    private float _columnMovementMaxSpeed;

    [SerializeField]
    private float _columnResettingSpeed;

    private GameObject _selectedColumn;
    private ColumnProperties _columnProperties;

    private float _baseYValue;
    private bool _columnRising;
    private bool _columnLowering;
    private float _columnSpeed;

    // Use this for initialization
    private void Start()
    {
        _columnRising = false;
        _columnLowering = false;

        _baseYValue = _unmovingColumn.transform.position.y;              
    }

    // Update is called once per frame
    private void Update()
    {

    }

   /// <summary>
   /// Updates the selectedColumn and columnProperties for the give column.
   /// </summary>
   /// <param name="pSelectedColumn"></param>
   /// <param name="pColumnProperties"></param>
    public void UpdateSelectedColumn(GameObject pSelectedColumn, ColumnProperties pColumnProperties)
    {
        _selectedColumn = pSelectedColumn;
        _columnProperties = pColumnProperties;
    }

    /// <summary>
    /// Will try to raise the targetted column, succeding if the conditions to allow such a raise are met.
    /// </summary>
    /// <param name="pPlayerID"></param>
    /// <param name="pSelectedColumn"></param>
    /// <param name="pColumnProperties"></param>
    public void AttemptRaise(int pPlayerID, GameObject pSelectedColumn, ColumnProperties pColumnProperties)
    {
        if (pSelectedColumn != null /*&& isColumnMoving() == false*/ && pColumnProperties.GetColumnType() != 1 && pColumnProperties.GetColumnStatus() == 0)
        {
            Debug.Log("Column Type: " + pColumnProperties.GetColumnType());
            //Debug.Log("My pos before moving is: " + _selectedColumn.transform.position + " while the baseY is: " + _baseYValue);
            setSelectedColumnPropertiesDataValues(pColumnProperties);
            pColumnProperties.columnStatus = ColumnProperties.ColumnStatus.Locked;
            pColumnProperties.ToggleColumnRising(true);
        }
    }

    /// <summary>
    /// Will try to lower the targetted column, succeding if the conditions to allow such a lower are met.
    /// </summary>
    /// <param name="pPlayerID"></param>
    /// <param name="pSelectedColumn"></param>
    /// <param name="pColumnProperties"></param>
    public void AttemptLower(int pPlayerID, GameObject pSelectedColumn, ColumnProperties pColumnProperties)
    {
        if (pSelectedColumn != null /*&& isColumnMoving() == false*/ && pColumnProperties.GetColumnType() != 1 && pColumnProperties.GetColumnStatus() == 0)
        {
            setSelectedColumnPropertiesDataValues(pColumnProperties);
            pColumnProperties.columnStatus = ColumnProperties.ColumnStatus.Locked;
            pColumnProperties.ToggleColumnLowering(true);
        }
    }

    private void setSelectedColumnPropertiesDataValues(ColumnProperties _columnProperties)
    {
        _columnProperties.SetDataValues(_baseYValue, _columnDisplacementSize, _columnMovementAccelerationSpeed, _columnMovementMaxSpeed, _columnResettingSpeed);
    }

    /*
    //wait for imput from lowering / raising column buttons
    private void detectColumnControlInput()
    {
        //if we've pressed the button, we have a column selected, it isn't already being controlled and it isn't a Static type column
        if (InputManager.RaiseColumn(1) > 0 && selectedColumn != null && isColumnMoving() == false && columnProperties.GetColumnType() != 1)
        {
            Debug.Log("My pos before moving is: " + selectedColumn.transform.position + " while the baseY is: " + baseYValue);
            columnRising = true;
        }
        if (InputManager.LowerColumn(1) > 0 && selectedColumn != null && isColumnMoving() == false && columnProperties.GetColumnType() != 1)
        {
            Debug.Log("My pos before moving is: " + selectedColumn.transform.position + " while the baseY is: " + baseYValue);
            columnLowering = true;
        }
    }*/

    //if columnLowering or columnRising has been turned on, start moving the column
    /*
    private void updateColumnPosition()
    {
        if (columnRising == true)
        {
            moveColumn(1.0f);
        }
        if (columnLowering == true)
        {
            moveColumn(-1.0f);
        }
    }*/
    /*
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
    }*/
    /*
    private void columnHalted()
    {
        _columnRising = false;
        _columnLowering = false;
    }*/
    /*
    //actually move the column
    private void moveColumn(float pPolarity)
    {
        //if column is not yet at the height of it's end position
        if(_baseYValue + Mathf.Abs(_selectedColumn.transform.position.y) + (_baseYValue / 2) < _columnDisplacementSize && IsColumnMoving() == true) 
        {
            //increase the speed of the column to give it natural acceleration
            _columnSpeed += _columnMovementAccelerationSpeed;
            if (_columnSpeed >= _columnMovementMaxSpeed)
            {
                _columnSpeed = _columnMovementMaxSpeed;
            }
            _selectedColumn.transform.Translate(0, pPolarity * _columnSpeed, 0);
            //Debug.Log("Speed: " + columnSpeed + "lossyScale: " + selectedColumn.transform.lossyScale.y + ", localScale: " + selectedColumn.transform.localScale.y);
        }
        else
        {
            //stop the column moving, which should deactive both columnRising and columnLowering
            columnHalted();
            _columnSpeed = 0.0f;
            _columnProperties.ResetColumn(_columnResettingSpeed, _columnDisplacementSize, _baseYValue, (pPolarity * -1));
        }
    }*/
}
