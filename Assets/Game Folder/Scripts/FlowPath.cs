using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowPath : MonoBehaviour
{
    //objects will not flow in the y direction,
    //enable if you want them to flow up or downwards in 3D space
    [SerializeField]
    private bool disgardHeight;

    //either being repulsed or attracted to a marker
    [SerializeField]
    private bool invertVector;

    //calculate difference vector every frame, useful
    //if marker's are moving
    [SerializeField]
    private bool activeTargetTracking;

    //either continue with the previous velocity or draw to a stop
    [SerializeField]
    private bool stopAtLastMarker;

    //modifies how fast an object flows between markers
    [SerializeField]
    private float flowSpeed = 1;

    //if your object has a rigidbody, add it's mass to the calculation
    //default mass: 
    [SerializeField]
    private bool FactorInRBMass;

    //if your object has a rigidbody, add it's drag to the calculation
    [SerializeField]
    private bool FactorInRBDrag;

    //if your object has a rigidbody, add it's angular drag to the calculation
    [SerializeField]
    private bool FactorInRBAngularDrag;

    //create vectors
    Vector3 forceScale;
    Vector3 targetPos;
    Vector3 differenceVec;

    private int waypointIndex = 0;

    //create lists
    List<Transform> markerList;

    //grab components
    private Rigidbody rigidBod;
    private Transform target;
    private FlowMarkerManager markerManager;
    // Use this for initialization

    void Awake()
    {
        rigidBod = GetComponent<Rigidbody>();

        //set target to our target's position
        markerManager = GameObject.FindGameObjectWithTag("FlowMarkerManager").GetComponent<FlowMarkerManager>();
        target = markerManager.GetWaypointTransform(waypointIndex);
        targetPos = target.position;
        differenceVec = transform.position - targetPos;
        modifyVector();
        markerManager.Report();
    }

    void Start()
    {
        targetPos = target.position;
        differenceVec = transform.position - targetPos;
        modifyVector();
    }

    // Update is called once per frame
    void Update()
    {
        flowToMarker();
    }

    //grabs our next marker in the list if our object passes over the current target
    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "FlowMarker")
        {
            //Debug.Log("Entered a flow marker. Increasing flow index.");
            waypointIndex++;
        }
        //Debug.Log("Marker list size is: " + markerManager.GetWaypointList().Length + " while our waypoint index is: " + waypointIndex);
        if (markerManager.GetWaypointList().Length > waypointIndex)
        {
            target = markerManager.GetWaypointTransform(waypointIndex);

            //for non-active tracking, calculate vectors only on reaching a marker:
            //update our target position
            targetPos = target.position;
            //find the difference between our current position and our target's
            differenceVec = transform.position - targetPos;
            modifyVector();
            //Debug.Log("Assigning a new target marker.");
        }
        else
        {
            Debug.Log("Reached the end of our list of markers.");
            if (stopAtLastMarker == true)
            {
                differenceVec = new Vector3(0, 0, 0);
            }
        }
    }

    //moves our object smoothly towards or away from our marker
    private void flowToMarker()
    {
        //get the difference vector once, unless we want to calculate every frame
        if (activeTargetTracking == true)
        {
            //update our target position
            targetPos = target.position;
            //find the difference between our current position and our target's
            differenceVec = transform.position - targetPos;
            modifyVector();
        }

        //apply a constant force mimicking a river's flow
        rigidBod.AddForce(differenceVec, ForceMode.Force);
    }

    //scales, flips and modifies the vector based on properties or options
    private void modifyVector()
    {
        //generic option to ignore y value if you're on a static plane
        if (disgardHeight == true)
        {
            differenceVec.y = 0;
        }
        //generic option to flip the vector if you want to move away from an object instead of towards it
        if (invertVector == true)
        {
            differenceVec = -differenceVec;
        }

        //scale our flow speed to flow faster or slower
        if (flowSpeed != 1)
        {
            differenceVec.Scale(new Vector3(flowSpeed, flowSpeed, flowSpeed));
        }

        if (FactorInRBMass == true)
        {
            factorRigidBodyProperty(rigidBod.mass, 10);
        }

        if (FactorInRBDrag == true)
        {
            factorRigidBodyProperty(rigidBod.drag);
        }

        if (FactorInRBAngularDrag == true)
        {
            factorRigidBodyProperty(rigidBod.angularDrag);
        }
    }

    private void factorRigidBodyProperty(float pFactorType, int pDivident = 1)
    {
        //scale for the weight of the rigidbody
        Vector3 physicsAccomodation = new Vector3(pFactorType / pDivident, pFactorType / pDivident, pFactorType / pDivident);
        differenceVec.Scale(physicsAccomodation);
    }
}
