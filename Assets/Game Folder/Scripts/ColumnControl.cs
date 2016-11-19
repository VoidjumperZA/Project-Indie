using UnityEngine;
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
        raycasting();
        //detectColumnControlInput();
        updateColumnPosition();
        isColumnMoving();
        //Debug.Log("columnMoving: " + isColumnMoving() + ", columnRising: " + columnRising + ", columnLowering: " + columnLowering);
    }

    //handles the raycast selecting the right column
    private void raycasting()
    {
        RaycastHit raycastHit;
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2.0f, Screen.height / 2.0f, 0));

        if (Physics.Raycast(ray, out raycastHit) && isColumnMoving() == false)
        {
            _selectedColumn = raycastHit.collider.gameObject;
            _columnProperties = _selectedColumn.GetComponent<ColumnProperties>();
        }
    }

    public void AttemptRaise(int pPlayerID)
    {
        if (_selectedColumn != null && isColumnMoving() == false && _columnProperties.GetColumnType() != 1)
        {
            Debug.Log("My pos before moving is: " + _selectedColumn.transform.position + " while the baseY is: " + _baseYValue);
            _columnRising = true;
        }
    }

    public void AttemptLower(int pPlayerID)
    {
        if (_selectedColumn != null && isColumnMoving() == false && _columnProperties.GetColumnType() != 1)
        {
            Debug.Log("My pos before moving is: " + _selectedColumn.transform.position + " while the baseY is: " + _baseYValue);
            _columnLowering = true;
        }
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

    private bool isColumnMoving()
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

    //actually move the column
    private void moveColumn(float pPolarity)
    {
        //if column is not yet at the height of it's end position
        if(_baseYValue + Mathf.Abs(_selectedColumn.transform.position.y) + (_baseYValue / 2) < _columnDisplacementSize && isColumnMoving() == true) 
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
    }
}
