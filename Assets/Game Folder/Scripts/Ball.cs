using UnityEngine;
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

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        centrePosition = transform.position;
        inPossession = false;
        
        //This should definitely be deleted. I've only put it on here because this script is called once
        //it should be called from our lobby 
        MatchStatistics.IntialiseGoalTracking();
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
            currentOwner = movement.gameObject;
            currentOwnerID = movement.gameObject.GetComponent<PlayerInput>();

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

            TogglePossession(true);

        }
        Goal goalScript = pCollision.gameObject.GetComponent<Goal>();
        if (goalScript != null)
        {
            Debug.Log("Yes hello, this is Goal?");

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
                    MatchStatistics.AddUnattributedGoal(goalScript.GetTeamOwnershipID());
                }
                //give the opposing team a goal with the last opposing player to have touched it the credit
                else
                {
                    MatchStatistics.AddPlayerGoal(lastOwnerOfOtherTeamID.GetPlayerID());
                }
            }
            Debug.Log("GAME SCORE: " + MatchStatistics.GetMatchGoals().x + " | " + MatchStatistics.GetMatchGoals().y);
            ResetToCentre();
        }
        //A little fix for ResetToCentre()
        _rigidbody.freezeRotation = false;
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
        if (pState == true)
        {
            inPossession = true;
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.useGravity = false;
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
}
