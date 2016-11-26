using UnityEngine;
using System.Collections;

public class PlayerActions : MonoBehaviour
{
    private Vector3 spawnPosition;
    private Quaternion spawnRotation;
    private Rigidbody rigidBody;
    private PlayerProperties playerProperties;
    private bool applyGravity = false;
    // Use this for initialization
    void Start()
    {
        playerProperties = GameObject.Find("Manager").GetComponent<PlayerProperties>();

        spawnPosition = transform.position;
        spawnRotation = transform.rotation;

        rigidBody = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (applyGravity == true)
        {
            applyAddedGravity();
        }
    }

    public void Respawn()
    {
        transform.position = spawnPosition;
        transform.rotation = spawnRotation;
        rigidBody.velocity = Vector3.zero;

        GameObject ball = GameObject.FindGameObjectWithTag("Ball");
        ball.GetComponent<Ball>().TogglePossession(false);
    }

    public void ToggleAddedGravity(bool pState)
    {
        applyGravity = pState;
    }

    private void applyAddedGravity()
    {
        gameObject.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, -GameObject.Find("Manager").GetComponent<PlayerProperties>().GetAddedGravity(), 0) * Time.deltaTime);
    }


    /// <summary>
    /// Release the ball in the direction aimed, putting the ball back into play.
    /// </summary>
    /// <param name="pDirection"></param>
    public void Throw(Vector3 pDirection)
    {
        //going to make another function called FlashThrow in PlayerMovement
        //get the ball's gameObject
        GameObject ball = GameObject.FindGameObjectWithTag("Ball");
        Rigidbody ballRigidbody = ball.GetComponent<Rigidbody>();
        ball.GetComponent<Ball>().TogglePossession(false);
        pDirection = Quaternion.AngleAxis(-playerProperties.GetThrowRotationAddition(), gameObject.transform.right) * pDirection;

        ballRigidbody.AddForce(pDirection * playerProperties.GetThrowingForce());
        //pDirection = Quaternion.AngleAxis(-playerProperties.GetForcedThrowRotationAddition(), gameObject.transform.right) * pDirection;
    }

    //Need to change name and determine if this needs to be in PlayerActions
    public void DeathThrow(Vector3 pDirection)
    {
        //get the ball's gameObject
        GameObject ball = GameObject.FindGameObjectWithTag("Ball");
        Rigidbody ballRigidbody = ball.GetComponent<Rigidbody>();
        ball.GetComponent<Ball>().TogglePossession(false);
        ballRigidbody.AddForce(pDirection * playerProperties.GetThrowingForce());
    }

    /// <summary>
    /// Teleport the player forward a short amount while also throwing the ball ahead of them.
    /// </summary>
    public void Flash(Vector3 pPosition)
    {
        //raycast down and look which column you are hitting, then look at the position after the flash, raycast down and see if it is another column
        transform.position = pPosition;
    }

    public float GetFlashDistance()
    {
        return playerProperties.GetFlashDistance();
    }
}
