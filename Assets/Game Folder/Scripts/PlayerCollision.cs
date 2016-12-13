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
            playerInput.SetInAir(false);
        }
    }

    private void OnTriggerEnter(Collider pCol)
    {
        if (pCol.gameObject.tag == "UpperBoundary" || pCol.gameObject.tag == "LowerBoundary")
        {
            if (detectColumnBelow() == true)
            {
                if (pCol.gameObject.tag == "UpperBoundary")
                {
                    MatchStatistics.AddPlayerSquished(columnProperties.GetOwnerID());

                    GameObject ball = GameObject.FindGameObjectWithTag("Ball");
                    GameObject columnUser = GameObject.Find("Manager").GetComponent<ActivePlayers>().GetPlayerInMatch(columnProperties.GetOwnerID());

                    Vector3 deltaFromBallToColumnUser = columnUser.transform.position - ball.transform.position;

                    if (ball.GetComponent<Ball>().getCurrentOwner() == gameObject)
                    {
                        playerActions.Throw(deltaFromBallToColumnUser.normalized, PlayerActions.ThrowType.DEATH);
                    }
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
