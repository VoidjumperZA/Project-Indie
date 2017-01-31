using UnityEngine;
using System.Collections;

public class SimpleRotate : MonoBehaviour
{
    [SerializeField]
    private bool overrideAutomaticRotation;

    [SerializeField]
    private float customMaxRotationSpeedX;

    [SerializeField]
    private float customMaxRotationSpeedY;

    [SerializeField]
    private float customMaxRotationSpeedZ;

    [Header("Random Range")]

    [SerializeField]
    private bool randomiseSpeed;

    [SerializeField]
    private float customMinRotationSpeedX;

    [SerializeField]
    private float customMinRotationSpeedY;

    [SerializeField]
    private float customMinRotationSpeedZ;

    private float internalRotSpeedX;
    private float internalRotSpeedY;
    private float internalRotSpeedZ;
    
    // Use this for initialization
    void Start()
    {
        if (randomiseSpeed == true)
        {
            internalRotSpeedX = Random.Range(customMaxRotationSpeedX, customMinRotationSpeedX);
            internalRotSpeedY = Random.Range(customMaxRotationSpeedY, customMinRotationSpeedY);
            internalRotSpeedZ = Random.Range(customMaxRotationSpeedZ, customMinRotationSpeedZ);
        }
        else
        {
            internalRotSpeedX = customMaxRotationSpeedX;
            internalRotSpeedY = customMaxRotationSpeedY;
            internalRotSpeedZ = customMaxRotationSpeedZ;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (overrideAutomaticRotation == false)
        {
            rotateAutomatically();
        }
        else
        {
            rotateWithValue();
        }
    }

    //mainly used for pentagrams, which get created at run time and so we cannot give them options
    private void rotateAutomatically()
    {
        transform.Rotate(0, 0, GameObject.Find("Manager").GetComponent<ColumnControl>().GetSelectionPentagramRotationSpeed());
    }

    //this is primarly used to control the game camera's rotation
    private void rotateWithValue()
    {
        
        transform.Rotate(internalRotSpeedX, internalRotSpeedY, internalRotSpeedZ);
    }
}
