using UnityEngine;
using System.Collections;

public class PossessedHexes : MonoBehaviour
{
    [SerializeField]
    private float columnRandomSelectionCooldown;

    private float timestamp;
    private GameObject[] columns;
    private ColumnControl columnControl;
    
    // Use this for initialization
    void Start()
    {
        columns = GameObject.FindGameObjectsWithTag("Column");
        columnControl = GameObject.Find("Manager").GetComponent<ColumnControl>();
        timestamp = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        randomlyMoveColumn();
    }

    private void randomlyMoveColumn()
    {
        if (timestamp <= Time.time)
        {
            timestamp += columnRandomSelectionCooldown;

            int randomColumnNumber = Random.Range(0, columns.Length);
            int raiseOrLower = Random.Range(1, 2);
            switch (raiseOrLower)
            {
                //the gamefield will use the ID of 10
                case 1:
                    columnControl.AttemptRaise(10, columns[randomColumnNumber], columns[randomColumnNumber].GetComponent<ColumnProperties>());
                    break;
                case 2:
                    columnControl.AttemptLower(10, columns[randomColumnNumber], columns[randomColumnNumber].GetComponent<ColumnProperties>());
                    break;
            }
        }
    }

    public void ModifyCooldownValue(float pModifier)
    {
        columnRandomSelectionCooldown *= pModifier;
    }
}
