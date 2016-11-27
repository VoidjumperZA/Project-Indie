using UnityEngine;
using System.Collections;

public class PlayerActions : MonoBehaviour
{
    private PlayerProperties playerProperties;

    private Vector3 _spawnPosition;
    private Quaternion _spawnRotation;
    private Rigidbody _rigidBody;

    private bool applyGravity = false;

    //Columns implementation
    private ColumnControl _columnControl;
    private GameObject _selectedColumn;
    private ColumnProperties _columnProperties;
    private Pentagram selectedPentagram;

    private void Start()
    {
        playerProperties = GameObject.Find("Manager").GetComponent<PlayerProperties>();

        _spawnPosition = transform.position;
        _spawnRotation = transform.rotation;

        _rigidBody = gameObject.GetComponent<Rigidbody>();

        //Columns implementation
        _columnControl = GameObject.Find("Manager").GetComponent<ColumnControl>();
        //attemtpraise and attemptLower
    }

    public void MoveColumn(string pDirection, int pPlayerID)
    {
        if (pDirection == "Up") { _columnControl.AttemptRaise(pPlayerID, _selectedColumn, _columnProperties); }
        else if (pDirection == "Down") { _columnControl.AttemptLower(pPlayerID, _selectedColumn, _columnProperties); }
    }

    //raycast gameobjects and if they're columns, set them as the selected column
    public void RaycastingColumn()
    {
        //RaycastHit raycastHit;
        //Ray ray; = _playerCamera.ScreenPointToRay(_raycastPos);

        //if (Physics.Raycast(ray, out raycastHit))
        //{
        //    if (raycastHit.collider.gameObject.tag == "Column")
        //    {
        //        _selectedColumn = raycastHit.collider.gameObject;
        //        _columnProperties = _selectedColumn.GetComponent<ColumnProperties>();

        //        if (_columnProperties.GetColumnType() == 0)
        //        {
        //            //if we have a pentagram already, and it's not the same one we're targeting, switch the old one off
        //            Pentagram newSelectedPenta = _selectedColumn.GetComponentInChildren(typeof(Pentagram), true) as Pentagram;
        //            if (selectedPentagram != null && selectedPentagram != newSelectedPenta)
        //            {
        //                selectedPentagram.TogglePentagram(false);
        //            }

        //            selectedPentagram = newSelectedPenta;
        //            if (selectedPentagram.IsPentagramActive() != true)
        //            {
        //                selectedPentagram.TogglePentagram(true, gameObject.transform);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        _selectedColumn = null;
        //        _columnProperties = null;
        //        if (selectedPentagram != null)
        //        {
        //            selectedPentagram.TogglePentagram(false);
        //        }
        //    }
        //}
    }

    private void Update()
    {
        if (applyGravity == true)
        {
            applyAddedGravity();
        }
    }

    //Dont think this should be here... maybe Movement??? it is kind of a movement
    public void Respawn()
    {
        transform.position = _spawnPosition;
        transform.rotation = _spawnRotation;
        _rigidBody.velocity = Vector3.zero;

        GameObject ball = GameObject.FindGameObjectWithTag("Ball");
        ball.GetComponent<Ball>().TogglePossession(false);
    }

    //Player states should be in PlayerInput
    public void ToggleAddedGravity(bool pState)
    {
        applyGravity = pState;
    }

    //Move this to PlayerMovment??
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
}
