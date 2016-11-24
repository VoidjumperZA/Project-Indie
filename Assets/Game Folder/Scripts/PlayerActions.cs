using UnityEngine;
using System.Collections;

public class PlayerActions : MonoBehaviour
{
    private Vector3 spawnPosition;
    private Quaternion spawnRotation;
    private Rigidbody rigidBody;

    private bool applyGravity = false;
    // Use this for initialization
    void Start()
    {
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
        Debug.Log("test");
        gameObject.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, -GameObject.Find("Manager").GetComponent<PlayerProperties>().GetAddedGravity(), 0) * Time.deltaTime);
    }

}
