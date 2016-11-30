using UnityEngine;
using System.Collections;

public class CrowdCheer : MonoBehaviour
{
    private GameObject[] crowdMembers;
    private Rigidbody[] crowdMemberRigidbodies;
    // Use this for initialization
    void Start()
    {
        crowdMembers = GameObject.FindGameObjectsWithTag("CrowdMember");
        for (int i = 0; i < crowdMembers.Length; i++)
        {
            crowdMemberRigidbodies[i] = crowdMembers[i].GetComponent<Rigidbody>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        int randomMember = Random.Range(0, crowdMembers.Length);
        Vector3 force = crowdMembers[randomMember].transform.up * 15000;
        crowdMemberRigidbodies[randomMember].AddRelativeForce(force, ForceMode.Impulse);
    }
}
