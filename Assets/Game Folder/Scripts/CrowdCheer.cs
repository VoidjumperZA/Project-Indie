using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CrowdCheer : MonoBehaviour
{
    [SerializeField]
    private float minForceMultiplier;
    [SerializeField]
    private float maxForceMultiplier;
    [SerializeField]
    private float timeBetweenCheersInSeconds;
    private GameObject[] crowdMembers;
    private Rigidbody[] crowdMemberRigidbodies;
    private bool[] crowdMemberInAir;
    private List<Rigidbody> inAirBodies = new List<Rigidbody>();
    // Use this for initialization
    void Start()
    {
        crowdMembers = GameObject.FindGameObjectsWithTag("CrowdMember");
        crowdMemberRigidbodies = new Rigidbody[crowdMembers.Length];
        for (int i = 0; i < crowdMembers.Length; i++)
        {
            crowdMemberRigidbodies[i] = crowdMembers[i].GetComponent<Rigidbody>();
            //crowdMemberInAir[i] = false;
        }        
        StartCoroutine(haveCrowdMemberCheer());
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    private IEnumerator haveCrowdMemberCheer()
    {
        //change alpha values of all tiles
        while (true)
        {
            int randomMember = Random.Range(0, crowdMembers.Length);
            if (crowdMemberRigidbodies[randomMember] == null)
            {
                crowdMembers[randomMember].GetComponent<Rigidbody>();
            }
            CrowdMemberProperties properties = crowdMembers[randomMember].GetComponent<CrowdMemberProperties>();
            if (properties.GetGroundedState() == true)
            {
                // Debug.Log("Got cube " + randomMember + " launching him NOW!");
                Vector3 force = crowdMembers[randomMember].transform.up * Random.Range(minForceMultiplier, maxForceMultiplier);
                crowdMemberRigidbodies[randomMember].AddRelativeForce(force, ForceMode.Impulse);
                properties.SetGroundedState(false);
            }
            yield return new WaitForSeconds(timeBetweenCheersInSeconds);
        }
    }
}
