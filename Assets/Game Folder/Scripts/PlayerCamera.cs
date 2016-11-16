using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour
{
    //Mandatory
    [SerializeField]
    private Transform _target;
    //DESIGNER Inspector values, once chosen a nice value, these can be replaced with "hard coded" values
    [SerializeField]
    private Vector3 _relativeCameraPosition;
    [SerializeField]
    private Vector3 _relativeCameraLookAt;
    [SerializeField]
    private Vector3 _relativeShootingCameraPosition;
    [SerializeField]
    private bool _smoothFollowment = true;
    [SerializeField]
    private float _smoothFollowmentSpeed;

    private Vector3 _finalCameraPosition;


    private void Start()
    {
        //DESIGNER Little trick so the Designers can work with integers
        _smoothFollowmentSpeed /= 100;
    }

    private void Update()
    {
        followTarget();
    }

    private void followTarget()
    {
        //Converting the camera position from the player's local space to the world space position
        _finalCameraPosition = _target.TransformPoint(_relativeCameraPosition);
        //DESIGNER If statement so they can swap between smooth followment and snappy followment through the inspector
        if (!_smoothFollowment)
        {
            transform.position = _finalCameraPosition;
        }
        else
        {
            transform.position = Vector3.Slerp(transform.position, _finalCameraPosition, _smoothFollowmentSpeed);
        }
    }
}
