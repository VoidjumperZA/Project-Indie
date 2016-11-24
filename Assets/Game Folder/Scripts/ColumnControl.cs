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
    private float _columnTimeUntilReset;

    [SerializeField]
    private float _selectionPentagramRotationSpeed;

    private GameObject _selectedColumn;
    private ColumnProperties _columnProperties;

    private float _baseYValue;
    private bool _columnRising;
    private bool _columnLowering;
    private float _columnSpeed;
    private float _halfColumnHeight;

    // Use this for initialization
    private void Start()
    {
        _columnRising = false;
        _columnLowering = false;

        _baseYValue = _unmovingColumn.transform.position.y;
        _halfColumnHeight = GameObject.Find("Column").GetComponent<MeshRenderer>().bounds.extents.y;              
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
            setSelectedColumnPropertiesDataValues(pColumnProperties, pPlayerID);
            pColumnProperties.columnStatus = ColumnProperties.ColumnStatus.Locked;
            pColumnProperties.ToggleColumnRising(true);

            Debug.Log("Column Control column displacement size is: " + _columnDisplacementSize);

            //FMOD
            playFMOD(pSelectedColumn, pColumnProperties, 1);
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
            setSelectedColumnPropertiesDataValues(pColumnProperties, pPlayerID);
            pColumnProperties.columnStatus = ColumnProperties.ColumnStatus.Locked;
            pColumnProperties.ToggleColumnLowering(true);

            //FMOD
            playFMOD(pSelectedColumn, pColumnProperties, 0);
        }
    }

    private void setSelectedColumnPropertiesDataValues(ColumnProperties _columnProperties, int pPlayerID)
    {
        _columnProperties.SetDataValues(_baseYValue, _columnDisplacementSize, _columnMovementAccelerationSpeed, _columnMovementMaxSpeed, _columnResettingSpeed, _columnTimeUntilReset, pPlayerID);
    }

    public float GetColumnDisplacement()
    {
        return _columnDisplacementSize;
    }

    public float GetGroundFloorYValue()
    {
        print("_baseYvalue: " + _baseYValue + "_halfColumnHeight: " + _halfColumnHeight);
        return _baseYValue + (2.0f * _halfColumnHeight);
        //Probuilder placed the origin at the bottom!!!!
    }

    public GameObject GetUnmovingColumn()
    {
        return _unmovingColumn;
    }

    public float GetSelectionPentagramRotationSpeed()
    {
        return _selectionPentagramRotationSpeed;
    }


    private void playFMOD(GameObject pSelectedColumn, ColumnProperties pColumnProperties, int pDirectionParamValue)
    {
        pColumnProperties.hexSoundEv = FMODUnity.RuntimeManager.CreateInstance(pColumnProperties.hexSound);
        pColumnProperties.hexSoundEv.getParameter("Direction", out pColumnProperties.directionParam);
        pColumnProperties.directionParam.setValue(pDirectionParamValue);
        FMODUnity.RuntimeManager.PlayOneShot(pColumnProperties.hexSound, pSelectedColumn.gameObject.transform.position);
    }

}
