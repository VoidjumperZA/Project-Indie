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
    private float _horizontalCameraRotationSpeed;
    [SerializeField]
    private float _verticalCameraRotionSpeed;
    [SerializeField]
    private bool _smoothFollow = true;
    [SerializeField]
    private float _smoothFollowSpeed;

    private Transform _targetCameraHelper;
    private Vector3 _finalCameraPosition;
    private Vector3 _finalCameraLookAt;


    private void Start()
    {
        //DESIGNER Little trick so the Designers can work with integers
        _smoothFollowSpeed /= 100;
        _targetCameraHelper = _target.GetChild(0).GetComponent<Transform>();
    }

    private void Update()
    {
        followTarget();
    }

    private void followTarget()
    {
        //Converting the camera position from the player's local space to the world space position
        _finalCameraPosition = _targetCameraHelper.TransformPoint(_relativeCameraPosition);
        _finalCameraLookAt = _target.TransformPoint(_relativeCameraLookAt);
        //DESIGNER If statement so they can swap between following smoothly and following in a snappy manner through the inspector
        if (!_smoothFollow)
        {
            transform.position = _finalCameraPosition;
        }
        else
        {
            transform.position = Vector3.Slerp(transform.position, _finalCameraPosition, _smoothFollowSpeed);
        }

        transform.LookAt(_finalCameraLookAt);
    }

    public void MoveCamera(float pXRotation, float pYRotation)
    {
        _target.transform.Rotate(0, pXRotation * _horizontalCameraRotationSpeed, 0);
        _targetCameraHelper.transform.Rotate(-pYRotation * _verticalCameraRotionSpeed, 0, 0);
        //DESIGNER A Drawline to visualise where the camera looks at
        Debug.DrawLine(transform.position, _finalCameraLookAt);
        //BUGTEST
        //print("X: " + pXRotation + "and Y: " + pYRotation);
    }
}
