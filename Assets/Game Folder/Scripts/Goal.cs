using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour {
    [SerializeField]
    private int teamOwnershipID;
    private int opposingTeamID;

    // Use this for initialization
    void Start () {
        if (teamOwnershipID == 1)
        {
            opposingTeamID = 2;
        }
        else if(teamOwnershipID == 2)
        {
            opposingTeamID = 1;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public int GetTeamOwnershipID()
    {
        return teamOwnershipID;
    }

    public int GetOpposingTeamID()
    {
        return opposingTeamID;
    }
}
