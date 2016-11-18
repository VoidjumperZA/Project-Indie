using UnityEngine;
using System.Collections;

public class ColumnProperties : MonoBehaviour
{
    private enum ColumnType { Dynamic, Static };
    public enum ColumnStatus { Free, Locked };

    [SerializeField]
    private ColumnType columnType;

    [HideInInspector]
    public ColumnStatus columnStatus;

    private bool atBaseLevel = true;
    private float columnResettingSpeed;
    private float columnDisplacementSize;
    private float baseYValue;
    private float polarity;
    private float movementDelta = 0.0f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (atBaseLevel == false)
        {
            GrindToArenaLevel();
        }
    }

    public void ResetColumn(float pColumnResettingSpeed, float pColumnDisplacementSize, float pBaseYValue, float pPolarity)
    {
        columnResettingSpeed = pColumnResettingSpeed;
        columnDisplacementSize = pColumnDisplacementSize;
        baseYValue = pBaseYValue;
        polarity = pPolarity;
        atBaseLevel = false;
    }


    private void GrindToArenaLevel()
    {
        if(movementDelta < columnDisplacementSize)
        //if (Mathf.Abs(gameObject.transform.position.y) + (columnDisplacementSize / 2) > baseYValue)
        {
            gameObject.transform.Translate(0, polarity * columnResettingSpeed, 0);
            movementDelta += columnResettingSpeed;
        }
        else
        {
            Debug.Log("My pos before .set is: " + gameObject.transform.position + " while the baseY value I was given is:" + baseYValue);
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, baseYValue, gameObject.transform.position.z);                   
            Debug.Log("My pos after .set is: " + gameObject.transform.position + " while the baseY value I was given is:" + baseYValue);
            movementDelta = 0.0f;
            atBaseLevel = true;
            //Debug.Log("I actually reached here!");
        }

    }

    public int GetColumnType()
    {
        return (int)columnType;
    }
}
