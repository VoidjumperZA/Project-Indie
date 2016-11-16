using UnityEngine;
using System.Collections;

public class ColumnControl : MonoBehaviour
{
    [SerializeField]
    private GameObject unmovingColumn;

    private GameObject selectedColumn;
    private float yDeltaValue; 
    private float yIncrementor;
    private float baseYValue; 
    private bool columnMoving;
    // Use this for initialization
    void Start()
    {
        yDeltaValue = 100.0f;
        yIncrementor = 0.5f;
        baseYValue = 0.0f;
        columnMoving = false;
    }

    // Update is called once per frame
    void Update()
    {/*
        if (selectedColumn == null)
        {
           // Debug.Log("Selected Column is null");
        }
        if (selectedColumn != null && selectedColumn.transform.position.y == unmovingColumn.transform.position.y)
        {
            //Debug.Log("EQUAL");
        }*/
        RaycastHit raycastHit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Debug.Log("Good to fucking know this is:" + columnMoving);
        if (Physics.Raycast(ray, out raycastHit) && columnMoving == false)
        {
            Debug.Log("Has selected a column");
            selectedColumn = raycastHit.collider.gameObject;
        }

        //Debug.Log("selected column y: " + selectedColumn.transform.position.y);

        if (Input.GetMouseButtonDown(0) && selectedColumn != null)
        {
            //Debug.Log("You clicked.");
            if (selectedColumn.transform.position.y < unmovingColumn.transform.position.y + yDeltaValue)
            {
                //Debug.Log("Should be moving.");
                columnMoving = true;
                moveColumn(1.0f);
                
            }
        }
        if (Input.GetMouseButtonDown(1) && selectedColumn != null)
        {
            //Debug.Log("You clicked.");
            if (selectedColumn.transform.position.y > unmovingColumn.transform.position.y - yDeltaValue)
            {
                //Debug.Log("Should be moving.");
                columnMoving = true;
                moveColumn(-1.0f);

            }
        }

        if (Input.GetKeyDown(KeyCode.W)) 
        {
            unmovingColumn.transform.Translate(0, yIncrementor, 0);
        }
    }

    private void moveColumn(float pPolarity)
    {
        do
        {
            selectedColumn.transform.Translate(0, pPolarity * yIncrementor, 0);
            Debug.Log("MOVING " + yIncrementor + " amount(" + pPolarity + " / " + pPolarity * yIncrementor + ")");
            Debug.Log("selected column y: " + selectedColumn.transform.position.y);
        }
        while (selectedColumn.transform.position.y < unmovingColumn.transform.position.y + (pPolarity * yDeltaValue));
        columnMoving = false;
        Debug.Log("FINAL POS y: " + selectedColumn.transform.position.y);
    }
}
