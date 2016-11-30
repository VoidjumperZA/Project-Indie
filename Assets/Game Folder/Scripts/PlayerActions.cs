using UnityEngine;
using System.Collections;

public class PlayerActions : MonoBehaviour
{
    [SerializeField]
    private GameObject selectionPentagramGameObject;

    private Pentagram selectionPentagram;
    private PlayerProperties playerProperties;

    private Vector3 _spawnPosition;
    private Quaternion _spawnRotation;
    private Rigidbody _rigidBody;

    private bool applyGravity = false;

    //Columns implementation
    private ColumnControl _columnControl;
    private GameObject _selectedColumn;
    private ColumnProperties _columnProperties;

    private Camera _camera;
    private Vector3 _raycastPos;

    private Ball _ballScript;
    private Rigidbody _ballRigidbody;

    public enum ThrowType { NORMAL, FORCED, DEATH }

    private void Start()
    {
        playerProperties = GameObject.Find("Manager").GetComponent<PlayerProperties>();
        selectionPentagram = selectionPentagramGameObject.GetComponent<Pentagram>();

        _spawnPosition = transform.position;
        _spawnRotation = transform.rotation;

        _rigidBody = gameObject.GetComponent<Rigidbody>();

        _columnControl = GameObject.Find("Manager").GetComponent<ColumnControl>();
        GameObject ball = GameObject.FindGameObjectWithTag("Ball");
        _ballScript = ball.GetComponent<Ball>();
        _ballRigidbody = ball.GetComponent<Rigidbody>();
    }


    private void Update()
    {
        RaycastingColumn();
    }

    public void MoveColumn(string pRaiseOrLower, int pPlayerID)
    {
        if (pRaiseOrLower == "Raise") { _columnControl.AttemptRaise(pPlayerID, _selectedColumn, _columnProperties); }
        else if (pRaiseOrLower == "Lower") { _columnControl.AttemptLower(pPlayerID, _selectedColumn, _columnProperties); }
    }

    //raycast gameobjects and if they're columns, set them as the selected column
    public void RaycastingColumn()
    {
        RaycastHit raycastHit;
        Ray ray = _camera.ScreenPointToRay(_raycastPos);

        if (Physics.Raycast(ray, out raycastHit))
        {
            if (raycastHit.collider.gameObject.tag == "Column")
            {
                _selectedColumn = raycastHit.collider.gameObject;
                _columnProperties = _selectedColumn.GetComponent<ColumnProperties>();

                if (_columnProperties.GetColumnType() == 0)
                {
                    //if we have a pentagram already, and it's not the same one we're targeting, switch the old one off
                    //Pentagram newSelectedPenta = _selectedColumn.GetComponentInChildren(typeof(Pentagram), true) as Pentagram;
                    //if (selectionPentagram != null && selectionPentagram != newSelectedPenta)
                    //{
                    //    selectionPentagram.TogglePentagram(false);
                    //}

                    //selectionPentagram = newSelectedPenta;
                    //if (selectionPentagram.IsPentagramActive() != true)
                    //{
                    //selectionPentagram.TogglePentagram(true, gameObject.transform);
                    //}
                    selectionPentagram.TogglePentagram(true, gameObject.transform);
                    selectionPentagram.MovePentagram(_selectedColumn.transform);
                }
                else
                {
                    selectionPentagram.TogglePentagram(false);
                }
            }
            else
            {
                _selectedColumn = null;
                _columnProperties = null;
                if (selectionPentagram != null)
                {
                    selectionPentagram.TogglePentagram(false);
                }
            }
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

    public void Throw(Vector3 pDirection, ThrowType pType)
    {
        _ballScript.TogglePossession(false);

        float rotation = 0.0f;
        float force = 0.0f;

        switch(pType)
        {
            case ThrowType.NORMAL:
                rotation = playerProperties.GetThrowRotationAddition();
                force = playerProperties.GetThrowingForce();
                break;
            case ThrowType.FORCED:
                rotation = playerProperties.GetForcedThrowRotationAddition();
                force = playerProperties.GetForcedThrowingForce();
                break;
            case ThrowType.DEATH:
                rotation = 0.0f;
                //Might need a special one for DeathThrow
                force = playerProperties.GetThrowingForce();
                break;
        }
        print("pDirection in Throw before: " + pDirection);
        pDirection = Quaternion.AngleAxis(-rotation, gameObject.transform.right) * pDirection;
        print("pDirection in Throw after: " + pDirection);
        _ballRigidbody.AddForce(pDirection * force);
    }

    public void SetCameraAndRaycastPos(Camera pCamera, Vector3 pRaycastPos)
    {
        _camera = pCamera;
        _raycastPos = pRaycastPos;
    }
}
