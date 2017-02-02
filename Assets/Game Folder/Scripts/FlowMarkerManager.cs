using UnityEngine;
using System.Collections;

public class FlowMarkerManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] waypointList;

    [SerializeField]
    private bool disableMarkerRenderers;

    private MeshRenderer[] markerRenderers;

    // Use this for initialization
    void Start()
    {
        markerRenderers = new MeshRenderer[waypointList.Length];
        //if spesified, disable the renderers for all markers
        if (disableMarkerRenderers == true)
        {
            for (int i = 0; i < waypointList.Length; i++)
            {
                markerRenderers[i] = waypointList[i].GetComponent<MeshRenderer>();
            }

            foreach (MeshRenderer markerRenderer in markerRenderers)
            {
                markerRenderer.enabled = false;
            }
        }        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public GameObject[] GetWaypointList()
    {
        return waypointList;
    }

    public Transform[] GetWaypointTransformList()
    {
        Transform[] transformArray = new Transform[waypointList.Length];
        for (int i = 0; i < waypointList.Length; i++)
        {
            transformArray[i] = waypointList[i].transform;
        }
        return transformArray;
    }

    public GameObject GetWaypoint(int pIndex)
    {
        return waypointList[pIndex];
    }

    public Transform GetWaypointTransform(int pIndex)
    {
        //Debug.Log("Getting waypoint transform: " + waypointList[pIndex].transform);
        return waypointList[pIndex].transform;
    }

    public void Report()
    {
        //Debug.Log("I am active.");
    }

}
