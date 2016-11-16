using UnityEngine;
using System.Collections;

public class ColumnControl : MonoBehaviour
{
    [SerializeField]
    private GameObject unmovingColumn;

    [SerializeField]
    private float columnDisplacementSize;

    [SerializeField]
    private float columnMovementSpeed;

    private GameObject selectedColumn;
    private float baseYValue;
    private bool columnRising;
    private bool columnLowering;
    private bool columnMoving; //is the column moving at all

    // Use this for initialization
    void Start()
    {
        columnRising = false;
        columnLowering = false;
        columnMoving = false;

        baseYValue = unmovingColumn.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        raycasting();
        detectColumnControlInput();
        updateColumnPosition();
        isColumnMoving();
    }

    //handles the raycast selecting the right column
    private void raycasting()
    {
        RaycastHit raycastHit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out raycastHit) && columnMoving == false)
        {
            selectedColumn = raycastHit.collider.gameObject;
        }
    }

    //wait for imput from lowering / raising column buttons
    private void detectColumnControlInput()
    {
        if (InputManager.RaiseColumn() > 0 && selectedColumn != null && columnMoving == false)
        {
            columnRising = true;
        }
        if (InputManager.LowerColumn() > 0 && selectedColumn != null && columnMoving == false)
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

    private void isColumnMoving()
    {
        //if the column is moving in any direction mark it as moving
        if (columnRising == true || columnLowering == true)
        {
            columnMoving = true;
        }
        else
        {
            columnMoving = false;
        }

        //if the column gets called to stop, make sure both bools also stop
        if (columnMoving == false)
        {
            columnRising = false;
            columnLowering = false;
        }
    }

    //actually move the column
    private void moveColumn(float pPolarity)
    {
        //if column is not yet at the height of it's end position
        if (selectedColumn.transform.position.y < unmovingColumn.transform.position.y + (pPolarity * columnDisplacementSize))
        {
            selectedColumn.transform.Translate(0, pPolarity * columnMovementSpeed, 0);
            Debug.Log("MOVING " + columnMovementSpeed + " amount(" + pPolarity + " / " + pPolarity * columnMovementSpeed + ")");
            Debug.Log("selected column y: " + selectedColumn.transform.position.y);
        }
        else
        {
            //stop the column moving, which should deactive both columnRising and columnLowering
            columnMoving = false;
        }
    }
}
