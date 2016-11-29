using UnityEngine;
using System.Collections;

public class PlayerCollision : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerActions playerActions;
    private PlayerMovement playerMovement;
    private ColumnProperties columnProperties;

    private float _columnwidth;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerActions = GetComponent<PlayerActions>();
        playerMovement = GetComponent<PlayerMovement>();

        _columnwidth = CalculateColumnWidth();
        print(_columnwidth);
    }

    private void Update()
    {

    }

    private void OnCollisionEnter(Collision pCol)
    {
        if (pCol.gameObject.tag == "Column")
        {
            //playerActions.ToggleAddedGravity(false);
            playerInput.SetInAir(false);
        }
    }

    private void OnCollisionExit(Collision pCol)
    {
        if (pCol.gameObject.tag == "Column")
        {
            //playerActions.ToggleAddedGravity(true);
            playerInput.SetInAir(true);
        }
    }

    private void OnTriggerEnter(Collider pCol)
    {
        if (pCol.gameObject.tag == "UpperBoundary" || pCol.gameObject.tag == "LowerBoundary")
        {
            //Debug.Log("Collided with " + pCol.gameObject.tag);
            //print("detectColumnBelow(): " + detectColumnBelow());
            //raycast down, if there's a column find who owned it
            print(name + " enters UppoerBoundary or LowerBoundary");

            if (detectColumnBelow() == true)
            {
                if (pCol.gameObject.tag == "UpperBoundary")
                {
                    MatchStatistics.AddPlayerSquished(columnProperties.GetOwnerID());

                    GameObject ball = GameObject.FindGameObjectWithTag("Ball");
                    //This GetActivePlayerElement is not working as it should or something.
                    GameObject columnUser = GameObject.Find("Manager").GetComponent<ActivePlayers>().GetActivePlayerElement(columnProperties.GetOwnerID() - 1);

                    Vector3 deltaFromBallToColumnUser = columnUser.transform.position - ball.transform.position;

                    print("BUG TEST: Is this active in the hierarchy? " + columnUser.name + "Optional: columnproperties.getownerID() return: " + columnProperties.GetOwnerID());

                   // get width of column, move the ball width of column amount along deltaFromBallToColumnUser before playerActions.Throw
                    //Vector3 displacementBeforeThrow = deltaFromBallToColumnUser.normalized * _columnwidth;
                    //print("ball position before: " + ball.transform.position);
                    //ball.transform.Translate(displacementBeforeThrow);
                    //print("ball position after: " + ball.transform.position);


                    //This has something to do with line 61. It's not getting the right gameobject, it can get the de-activated player.

                    playerActions.Throw(deltaFromBallToColumnUser.normalized, PlayerActions.ThrowType.DEATH);
                    //playerActions.Throw(new Vector3(1.0f, 0.0f, 0.0f), PlayerActions.ThrowType.DEATH);


                    print("Player_" + playerInput.GetPlayerID() + " has been killed by player_" + columnProperties.GetOwnerID() );
                }
                else if (pCol.gameObject.tag == "LowerBoundary")
                {
                    MatchStatistics.AddPlayerDropped(columnProperties.GetOwnerID());
                }
            }
            MatchStatistics.AddPlayerDeath(playerInput.GetPlayerID());
            playerActions.Respawn();
        }

        if (pCol.gameObject.tag == "Mana")
        {
            playerInput.AddManaPoints();
            columnProperties.hitManaObject();
        }
    }

    //raycast downwards, if there is a column beneath you save it's properties
    private bool detectColumnBelow()
    {
        //RaycastHit raycastHit;
        Ray ray = new Ray(transform.position + (transform.up * 2.0f), -gameObject.transform.up);

        RaycastHit[] raycastHits = Physics.RaycastAll(ray);
        print("amount of raycasthits: " + raycastHits.Length);
        foreach (RaycastHit hit in raycastHits)
        {
            print("hit name: " + hit.collider.name);
            if (hit.collider.tag == "Column")
            {
                print("HALELUJAH NIGGA");
                columnProperties = hit.collider.gameObject.GetComponent<ColumnProperties>();
                return true;
            }
        }
        return false;
    }

    private float CalculateColumnWidth()
    {
        RaycastHit raycastHit;
        Ray ray = new Ray(gameObject.transform.position, -gameObject.transform.up);

        if (Physics.Raycast(ray, out raycastHit))
        {
            if (raycastHit.collider.gameObject.tag == "Column")
            {
                GameObject column = raycastHit.collider.gameObject;
                return (column.GetComponent<MeshRenderer>().bounds.extents.x) * 2.0f;
            }
        }
        return 0.0f;
    }
}
