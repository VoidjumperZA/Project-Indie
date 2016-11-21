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

    [SerializeField]
    private float _columnTimeInMSUntilReset;

    [SerializeField]
    private float _playerColumnControlCooldown;

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
}
