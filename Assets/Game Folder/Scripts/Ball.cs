﻿using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{
    [SerializeField]
    private Vector3 ballOffset;

    private Rigidbody _rigidbody;
    private GameObject currentOwner;            //tracks who is currently holding the ball
    private GameObject lastOwner;               //tracks who touched the ball last, can track assists
    private GameObject lastOwnerOfOtherTeam;    //tracks the last enemy to touch the other ball, in the case of own goals
    private PlayerInput currentOwnerID;
    private PlayerInput lastOwnerID;
    private PlayerInput lastOwnerOfOtherTeamID;
    private Vector3 centrePosition;

    private bool inPossession;

    //VYTAUTAS' FMOD IMPLEMENTATION BEGINS
    public string pickSound = "event:/Pick";
    public string goalSound = "event:/Goal";
    //VYTAUTAS' FMOD IMPLEMENTATION ENDS

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        centrePosition = transform.position;
        inPossession = false;
    }

    private void Update()
    {
        moveWithToPlayer();
    }

    private void OnCollisionEnter(Collision pCollision)
    {
        PlayerMovement movement = pCollision.gameObject.GetComponent<PlayerMovement>();
        if (movement != null)
        {
            //This is for the steal ball bug
            if (currentOwner != null)
            {
                currentOwnerID.SetBallPosession(false);
            }
            currentOwner = movement.gameObject;
            currentOwnerID = movement.gameObject.GetComponent<PlayerInput>();

            TogglePossession(true);

            try
            {
                //if we have a last owner and the last owner's team is not the same as the current owner's team
                if (lastOwner != null && MatchStatistics.GetTeamIDofPlayer(currentOwnerID.GetPlayerID()) != MatchStatistics.GetTeamIDofPlayer(lastOwnerID.GetPlayerID()))
                {
                    lastOwnerOfOtherTeam = lastOwner;
                    lastOwnerOfOtherTeamID = lastOwnerID;
                }
            }
            catch
            {
                Debug.Log("lastOwner is null. Possibly as another player has not touched the ball. This is intentional.");
            }

        }
        Goal goalScript = pCollision.gameObject.GetComponent<Goal>();
        if (goalScript != null)
        {
            Debug.Log("Yes hello, this is Goal?");
            FMODUnity.RuntimeManager.PlayOneShot(goalSound, goalScript.gameObject.transform.position);

            //if the goal does not belong to the same team as the player who scored
            if (goalScript.GetTeamOwnershipID() != MatchStatistics.GetTeamIDofPlayer(currentOwnerID.GetPlayerID()))
            {
                MatchStatistics.AddPlayerGoal(currentOwnerID.GetPlayerID());
            }
            else
            {
                //if an own goal is scored when no opposing player has touched the ball, give
                //the opposing team a goal, but no player credit
                if (lastOwnerOfOtherTeamID == null)
                {
                    Debug.Log("Scorer is on team" + MatchStatistics.GetTeamIDofPlayer(currentOwnerID.GetPlayerID()) + " while the goal registering is for team " + goalScript.GetTeamOwnershipID());
                    MatchStatistics.AddUnattributedGoal(goalScript.GetTeamOwnershipID());
                }
                //give the opposing team a goal with the last opposing player to have touched it the credit
                else
                {
                    MatchStatistics.AddPlayerGoal(lastOwnerOfOtherTeamID.GetPlayerID());
                }
            }
            Debug.Log("GAME SCORE: " + MatchStatistics.GetMatchGoals().x + " | " + MatchStatistics.GetMatchGoals().y);

            //Reset all players after a goal has been scored
            for (int i = 0; i < GameObject.Find("Manager").GetComponent<ActivePlayers>().GetActivePlayersArraySize(); i++)
            {
                Debug.Log("i is " + i);
                try
                {
                    GameObject.Find("Manager").GetComponent<ActivePlayers>().GetActivePlayerElement(i).GetComponent<PlayerActions>().Respawn();
                }
                catch
                {
                    Debug.Log("Caught trying to respawn a player that's inactive.");
                }
            }
            ResetToCentre();
        }

        //A little fix for ResetToCentre()
        _rigidbody.freezeRotation = false;
    }


    private void OnTriggerEnter(Collider pCol)
    {
        //This needs to be OnTriggerEnter
        if (pCol.gameObject.tag == "LowerBoundary")
        {
            ResetToCentre();
        }
    }

    //
    /// <summary>
    /// Reset the ball to the arena's centre and call a game timer.
    /// </summary>
    public void ResetToCentre()
    {
        transform.position = centrePosition;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.freezeRotation = true;
    }

    public void TogglePossession(bool pState)
    {
        if (currentOwnerID != null) { currentOwnerID.SetBallPosession(pState); }
        //print("inside Ball script.TogglePosession(), pState = " + pState);
        if (pState == true)
        {
            inPossession = true;
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.useGravity = false;

            FMODUnity.RuntimeManager.PlayOneShot(pickSound, this.gameObject.transform.position);
        }
        else
        {
            inPossession = false;
            _rigidbody.useGravity = true;
            lastOwner = currentOwner;
            lastOwnerID = currentOwnerID;
            currentOwner = null;
        }
    }

    private void moveWithToPlayer()
    {
        if (inPossession == true)
        {
            transform.position = currentOwner.transform.position;
            transform.Translate(ballOffset, Space.World);
        }
    }

    /// <summary>
    /// Will return true if the target is owned by a player.
    /// </summary>
    /// <returns></returns>
    public bool IsInPossession()
    {
        return inPossession;
    }

    public Vector3 GetBallOffset()
    {
        return ballOffset;
    }
}
