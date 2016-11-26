using UnityEngine;
using System.Collections;

public class DeathZones : MonoBehaviour
{
    [SerializeField]
    private GameObject upperBoundary;

    [SerializeField]
    private GameObject lowerBoundary;

    // Use this for initialization
    void Start()
    {
        setStartPosOfBoundaries(upperBoundary, 2.3f);
        setStartPosOfBoundaries(lowerBoundary, 1.3f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void setStartPosOfBoundaries(GameObject pZone, float pDistanceOffset)
    {
        ColumnControl columnControl = GetComponent<ColumnControl>();
        //pZone.transform.position = new Vector3(pZone.transform.position.x, pZone.transform.position.y, 0);
        pZone.transform.position = new Vector3(pZone.transform.position.x, 0 + (columnControl.GetUnmovingColumn().GetComponent<MeshRenderer>().bounds.extents.y) * pDistanceOffset, pZone.transform.position.z);
        
    }
}
