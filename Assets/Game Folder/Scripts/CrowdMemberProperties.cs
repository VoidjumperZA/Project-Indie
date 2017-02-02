using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdMemberProperties : MonoBehaviour {
    private bool grounded;
	// Use this for initialization
	void Start () {
        grounded = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetGroundedState(bool pState)
    {
        grounded = pState;
    }

    public bool GetGroundedState()
    {
        return grounded;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Bleacher")
        {
            Debug.Log("Back to being grounded :(");
            grounded = true;
        }
    }
}
