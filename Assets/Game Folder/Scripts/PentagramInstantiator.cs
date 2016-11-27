using UnityEngine;
using System.Collections;

public class PentagramInstantiator : MonoBehaviour
{
    [SerializeField]
    private GameObject selectionHexPrefab;

    [SerializeField]
    private float scalingSize;

    [SerializeField]
    private float heightAdjustment;

    // Use this for initialization
    void Start()
    {
        //grab all the columns in the scene
        GameObject[] columns = GameObject.FindGameObjectsWithTag("Column");
        for (int i = 0; i < columns.Length; i++)
        {
            if (columns[i].GetComponent<ColumnProperties>().GetColumnType() == 0 && columns[i].GetComponent<ColumnProperties>().GetColumnStatus() == 0)
            {
                //instantiate our selection pentagram
                GameObject newSelectionHex = Instantiate(selectionHexPrefab);

                //parent it to the column
                newSelectionHex.transform.SetParent(columns[i].transform);

                //scale it larger, zero it's position and translate it to the centre of the column
                newSelectionHex.transform.localScale = new Vector3(scalingSize, scalingSize, scalingSize);
                newSelectionHex.transform.localPosition = new Vector3(0, 0, 0);
                newSelectionHex.transform.Translate(0, columns[i].GetComponent<MeshRenderer>().bounds.extents.y * (2.0f + heightAdjustment), 0);

                //deactive it
                newSelectionHex.SetActive(false);
                //0.037
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
