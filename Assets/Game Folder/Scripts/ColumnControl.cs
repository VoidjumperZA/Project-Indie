using UnityEngine;
using System.Collections;

public class ColumnControl : MonoBehaviour
{
    [SerializeField]
    private GameObject unmovingColumn;

    [SerializeField]
    private float columnDisplacementSize;

    [SerializeField]
    private float columnMovementAccelerationSpeed;

    [SerializeField]
    private float columnMovementMaxSpeed;

    [SerializeField]
    private float columnResettingSpeed;

    private GameObject selectedColumn;
    private ColumnProperties columnProperties;
    private float baseYValue;
    private bool columnRising;
    private bool columnLowering;
    private float columnSpeed;

    // Use this for initialization
    void Start()
    {
        columnRising = false;
        columnLowering = false;

        baseYValue = unmovingColumn.transform.position.y;      
    }

    // Update is called once per frame
    void Update()
    {
        raycasting();
        detectColumnControlInput();
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
            selectedColumn = raycastHit.collider.gameObject;
            columnProperties = selectedColumn.GetComponent<ColumnProperties>();
        }
    }

    //wait for imput from lowering / raising column buttons
    private void detectColumnControlInput()
    {
        //if we've pressed the button, we have a column selected, it isn't already being controlled and it isn't a Static type column
        if (InputManager.RaiseColumn() > 0 && selectedColumn != null && isColumnMoving() == false && columnProperties.GetColumnType() != 1)
        {
            Debug.Log("My pos before moving is: " + selectedColumn.transform.position + " while the baseY is: " + baseYValue);
            columnRising = true;
        }
        if (InputManager.LowerColumn() > 0 && selectedColumn != null && isColumnMoving() == false && columnProperties.GetColumnType() != 1)
        {
            Debug.Log("My pos before moving is: " + selectedColumn.transform.position + " while the baseY is: " + baseYValue);
            columnLowering = true;
        }
    }

    //if columnLowering or columnRising has been turned on, start moving the column
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
    }

    private bool isColumnMoving()
    {
        //if the column is moving in any direction mark it as moving
        if (columnRising == true || columnLowering == true)
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
        columnRising = false;
        columnLowering = false;
    }

    //actually move the column
    private void moveColumn(float pPolarity)
    {
        //if column is not yet at the height of it's end position
        if(baseYValue + Mathf.Abs(selectedColumn.transform.position.y) + (baseYValue / 2) < columnDisplacementSize && isColumnMoving() == true) 
        {
            //increase the speed of the column to give it natural acceleration
            columnSpeed += columnMovementAccelerationSpeed;
            if (columnSpeed >= columnMovementMaxSpeed)
            {
                columnSpeed = columnMovementMaxSpeed;
            }
            selectedColumn.transform.Translate(0, pPolarity * columnSpeed, 0);
            //Debug.Log("Speed: " + columnSpeed + "lossyScale: " + selectedColumn.transform.lossyScale.y + ", localScale: " + selectedColumn.transform.localScale.y);
        }
        else
        {
            //stop the column moving, which should deactive both columnRising and columnLowering
            columnHalted();
            columnSpeed = 0.0f;
            columnProperties.ResetColumn(columnResettingSpeed, columnDisplacementSize, baseYValue, (pPolarity * -1));
        }
    }
}
