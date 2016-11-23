using UnityEngine;
using System.Collections;

public class PlayerCollision : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerActions playerActions;
    private ColumnProperties columnProperties;
    // Use this for initialization
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerActions = GetComponent<PlayerActions>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision pCol)
    {
        if (pCol.gameObject.tag == "UpperBoundary" || pCol.gameObject.tag == "LowerBoundary")
        {
            //raycast down, if there's a column find who owned it
            if (detectColumnBelow() == true)
            {
                if (pCol.gameObject.tag == "UpperBoundary")
                {
                    MatchStatistics.AddPlayerSquished(columnProperties.GetOwnerID());                    
                }
                else if (pCol.gameObject.tag == "LowerBoundary")
                {
                    MatchStatistics.AddPlayerDropped(columnProperties.GetOwnerID());
                }
            }
            MatchStatistics.AddPlayerDeath(playerInput.GetPlayerID());
            playerActions.Respawn();

        }
    }

    //raycast downwards, if there is a column beneath you save it's properties
    private bool detectColumnBelow()
    {
        RaycastHit raycastHit;
        Ray ray = new Ray(gameObject.transform.position, new Vector3(0, -1, 0));

        if (Physics.Raycast(ray, out raycastHit))
        {
            if (raycastHit.collider.gameObject.tag == "Column")
            {
                columnProperties = gameObject.GetComponent<ColumnProperties>();
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
