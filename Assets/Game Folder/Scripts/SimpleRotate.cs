using UnityEngine;
using System.Collections;

public class SimpleRotate : MonoBehaviour
{
    [SerializeField]
    private bool overrideAutomaticRotation;

    [SerializeField]
    private float newRotationSpeed;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
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
        transform.Rotate(0, newRotationSpeed, 0);
    }
}
