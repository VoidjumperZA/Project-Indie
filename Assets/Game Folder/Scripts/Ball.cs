using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{
    [SerializeField]
    private Vector3 ballOffset;

    private Rigidbody _rigidbody;
    private GameObject lastOwner;
    private GameObject currentOwner;
    private PlayerInput temp_PlayerInputForPlayerID;
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
            temp_PlayerInputForPlayerID = movement.gameObject.GetComponent<PlayerInput>();
            TogglePossession(true);
            //transform.SetParent(movement.transform);
                //transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                
                //transform.localPosition = new Vector3(0, 1, 0);

        }
        if (pCollision.gameObject.tag == "T1_Goal")
        {
            MatchStatistics.AddPlayerGoal(temp_PlayerInputForPlayerID.GetPlayerID());
            Debug.Log("GAME SCORE: " + MatchStatistics.GetMatchGoals().x + " | " + MatchStatistics.GetMatchGoals().y);
        }
    }

    //
    /// <summary>
    /// Reset the ball to the arena's centre and call a game timer.
    /// </summary>
    public void ResetToCentre()
    {
        transform.position = centrePosition;
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
            currentOwner = null;
        }
    }

    private void moveWithToPlayer()
    {
        if (inPossession == true)
        {
            transform.position = currentOwner.transform.position;
            transform.Translate(ballOffset);
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
