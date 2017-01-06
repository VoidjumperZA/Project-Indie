using UnityEngine;
using System.Collections;

public class DeathZones : MonoBehaviour
{
    [SerializeField]
    private GameObject upperBoundary;

    [SerializeField]
    private GameObject lowerBoundary;


    private void Start()
    {
        ColumnControl control = GetComponent<ColumnControl>();

        float columnBaseYValue = control.GetUnmovingColumn().transform.position.y;
        float columnHeight = control.GetUnmovingColumn().GetComponent<MeshRenderer>().bounds.extents.y * 2.0f;
        float distance = control.GetColumnDisplacement();


        setStartPosOfBoundaries(upperBoundary, columnBaseYValue + columnHeight + distance);
        setStartPosOfBoundaries(lowerBoundary, columnBaseYValue + columnHeight - distance);
    }

    private void Update()
    {

    }

    private void setStartPosOfBoundaries(GameObject pZone, float pHeight)
    {
        ColumnControl columnControl = GetComponent<ColumnControl>();
        //pZone.transform.position = new Vector3(pZone.transform.position.x, pZone.transform.position.y, 0);
        pZone.transform.position = new Vector3(pZone.transform.position.x, pHeight , pZone.transform.position.z);
        
    }
}
