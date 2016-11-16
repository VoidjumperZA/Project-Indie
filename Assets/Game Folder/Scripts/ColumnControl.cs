using UnityEngine;
using System.Collections;

public class ColumnControl : MonoBehaviour
{
    [SerializeField]
    private GameObject unmovingColumn;

    [SerializeField]
    private float columnDisplacementSize;

    [SerializeField]
    private float columnMovementMaxSpeed;

    private GameObject selectedColumn;
    private float baseYValue;
    private bool columnRising;
    private bool columnLowering;

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
        Debug.Log("columnMoving: " + isColumnMoving() + ", columnRising: " + columnRising + ", columnLowering: " + columnLowering);
    }

    //handles the raycast selecting the right column
    private void raycasting()
    {
        RaycastHit raycastHit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out raycastHit) && isColumnMoving() == false)
        {
            selectedColumn = raycastHit.collider.gameObject;
        }
    }

    //wait for imput from lowering / raising column buttons
    private void detectColumnControlInput()
    {
        if (InputManager.RaiseColumn() > 0 && selectedColumn != null && isColumnMoving() == false)
        {
            columnRising = true;
        }
        if (InputManager.LowerColumn() > 0 && selectedColumn != null && isColumnMoving() == false)
        {
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
        if (selectedColumn.transform.position.y < pPolarity * (unmovingColumn.transform.position.y + (pPolarity * columnDisplacementSize)) && isColumnMoving() == true)
        {
            selectedColumn.transform.Translate(0, pPolarity * columnMovementMaxSpeed, 0);
        }
        else
        {
            //stop the column moving, which should deactive both columnRising and columnLowering
            columnHalted();
        }
    }
}
