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
        setStartPosOfBoundaries(upperBoundary, 1.0f);
        setStartPosOfBoundaries(lowerBoundary, -1.0f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void setStartPosOfBoundaries(GameObject pZone, float pPolarity)
    {
        ColumnControl columnControl = GetComponent<ColumnControl>();
        pZone.transform.position = new Vector3(pZone.transform.position.x, pZone.transform.position.y, 0);
        //pZone.transform.Translate(0, 0, (columnControl.GetColumnDisplacement() / 2) * pPolarity);
    }
}
