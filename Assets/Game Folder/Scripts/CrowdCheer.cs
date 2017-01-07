using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CrowdCheer : MonoBehaviour
{
    private GameObject[] crowdMembers;
    private Rigidbody[] crowdMemberRigidbodies;
    private bool[] crowdMemberInAir;
    private List<Rigidbody> inAirBodies = new List<Rigidbody>();
    // Use this for initialization
    void Start()
    {
        crowdMembers = GameObject.FindGameObjectsWithTag("CrowdMember");
        for (int i = 0; i < crowdMembers.Length; i++)
        {
            crowdMemberRigidbodies[i] = crowdMembers[i].GetComponent<Rigidbody>();
            crowdMemberInAir[i] = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < inAirBodies.Count; i++)
        {
            //if (inAirBodies[i].collider.)
            //{

           // }
        }

        int randomMember = Random.Range(0, crowdMembers.Length);
        crowdMemberInAir[randomMember] = true;
        Vector3 force = crowdMembers[randomMember].transform.up * 15000;
        crowdMemberRigidbodies[randomMember].AddRelativeForce(force, ForceMode.Impulse);
        inAirBodies.Add(crowdMemberRigidbodies[randomMember]);
    }
}
