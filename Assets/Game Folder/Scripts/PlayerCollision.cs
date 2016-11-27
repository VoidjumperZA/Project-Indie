using UnityEngine;
using System.Collections;

public class PlayerCollision : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerActions playerActions;
    private PlayerMovement playerMovement;
    private ColumnProperties columnProperties;
    // Use this for initialization
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerActions = GetComponent<PlayerActions>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision pCol)
    {
        if (pCol.gameObject.tag == "UpperBoundary" || pCol.gameObject.tag == "LowerBoundary")
        {
            Debug.Log("Collided with " + pCol.gameObject.tag);
            //raycast down, if there's a column find who owned it
            if (detectColumnBelow() == true)
            {
                if (pCol.gameObject.tag == "UpperBoundary")
                {
                    MatchStatistics.AddPlayerSquished(columnProperties.GetOwnerID());

                    GameObject ball = GameObject.FindGameObjectWithTag("Ball");
                    Vector3 deltaFromBallToColumnUser = new Vector3();
                    GameObject columnUser = GameObject.Find("Manager").GetComponent<ActivePlayers>().GetActivePlayer(columnProperties.GetOwnerID());
                    deltaFromBallToColumnUser = columnUser.transform.position - ball.transform.position;

                    //However, it is still inside the column and trapped on the otehr side of the glass, it first
                    //needs to be moved away (renderer.bounds.extends?) before being thrown. Good luck Dom :)
                    playerActions.DeathThrow(deltaFromBallToColumnUser);                    
                }
                else if (pCol.gameObject.tag == "LowerBoundary")
                {
                    MatchStatistics.AddPlayerDropped(columnProperties.GetOwnerID());
                }
            }
            MatchStatistics.AddPlayerDeath(playerInput.GetPlayerID());
            playerActions.Respawn();

        }

        if (pCol.gameObject.tag == "Column")
        {
            playerActions.ToggleAddedGravity(false);
        }
    }

    void OnCollisionExit(Collision pCol)
    {
        if (pCol.gameObject.tag == "Column")
        {
            playerActions.ToggleAddedGravity(true);          
        }
    }

    private void OnTriggerEnter(Collider pCol)
    {
        if(pCol.gameObject.tag == "Mana")
        {
            playerInput.AddManaPoints();
            print("Touching mana");
            Destroy(pCol.gameObject);
        }
    }

    //raycast downwards, if there is a column beneath you save it's properties
    private bool detectColumnBelow()
    {
        RaycastHit raycastHit;
        Ray ray = new Ray(gameObject.transform.position, -gameObject.transform.up);

        if (Physics.Raycast(ray, out raycastHit))
        {
            if (raycastHit.collider.gameObject.tag == "Column")
            {
                columnProperties = raycastHit.collider.gameObject.GetComponent<ColumnProperties>();
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}
