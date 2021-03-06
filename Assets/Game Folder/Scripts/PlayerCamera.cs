﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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
    private float _minVerticalCameraRotation;
    [SerializeField]
    private float _maxVerticalCameraRotation;
    [SerializeField]
    private bool _raycasting;
    [SerializeField]
    private bool _raycastLerp;
    [SerializeField]
    private float _raycastLerpSpeed = 1.0f;

    private Transform _targetCameraHelper;
    private Vector3 _finalCameraPosition;
    private Vector3 _finalCameraLookAt;

    private bool _smoothFollow = false;
    private float _smoothFollowSpeed = 0.0f;
    private float _smoothFollowIncrement = 0.0f;
    private float _smoothFollowClipDistance = 0.0f;

    //[SerializeField]
    //private Image _arrow;
    //private RectTransform _rectTransform;
    //private Camera _camera;
    //private Transform _ballTransform;

    private void Start()
    {
        //DESIGNER Little trick so the Designers can work with integers
        _targetCameraHelper = _target.GetChild(0).GetComponent<Transform>();

        //_rectTransform = _arrow.GetComponent<RectTransform>();
        //_camera = GetComponent<Camera>();
        //_ballTransform = GameObject.FindGameObjectWithTag("Ball").GetComponent<Transform>();

        //updateArrow();

    }

    private void Update()
    {
        //updateArrow();
    }

    private void LateUpdate()
    {
        followTarget();
    }

    //private void updateArrow()
    //{
    //    Vector3 ballScreenPosition = _camera.WorldToScreenPoint(_ballTransform.position);
    //    //print(ballScreenPosition);
    //    //float angle = Vector2.Angle(new Vector2(1.0f, 0.0f), new Vector2(ballScreenPosition.x, ballScreenPosition.y));

    //    //_rectTransform.position = new Vector3(ballScreenPosition.x, ballScreenPosition.y, 0.0f);

    //    Vector2 ballPos = new Vector2(ballScreenPosition.x, ballScreenPosition.y);
    //    Vector2 ballVector = ballPos - new Vector2(_rectTransform.position.x, _rectTransform.position.y);
    //    float angle = Vector2.Angle(new Vector2(1.0f, 0.0f), ballVector);



    //    _rectTransform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);//<--------- HIER

    //}

    private void followTarget()
    {
        //Converting the camera position from the player's local space to the world space position
        _finalCameraPosition = _targetCameraHelper.TransformPoint(_relativeCameraPosition);
        _finalCameraLookAt = _target.TransformPoint(_relativeCameraLookAt);
        //DESIGNER If statement to enable or disable raycasting, have to look which one works better
        if (_raycasting)
        {
            Vector3 cam2Player = _finalCameraPosition - _targetCameraHelper.position;
            float rayLength = cam2Player.magnitude;

            Ray ray = new Ray(_targetCameraHelper.position, cam2Player);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, rayLength))
            {
                //print("NAME: " + hitInfo.collider.name);
                if (_raycastLerp)
                {
                    transform.position = Vector3.Lerp(transform.position, hitInfo.point, _raycastLerpSpeed);
                    transform.LookAt(_finalCameraLookAt);
                    return;
                }
                else
                {
                    transform.position = hitInfo.point;
                    transform.LookAt(_finalCameraLookAt);
                    return;
                }
            }
        }
        //DESIGNER If statement so they can swap between following smoothly and following in a snappy manner through the inspector
        if (!_smoothFollow)
        {
            transform.position = _finalCameraPosition;
        }
        else
        {
            //maybe below Slerp, maybe more, idk have to wait untill Josh is back
            _smoothFollowSpeed += _smoothFollowIncrement;
            transform.position = Vector3.Slerp(transform.position, _finalCameraPosition, _smoothFollowSpeed);
            //print("Distance: " + (_finalCameraPosition - transform.position).magnitude);
            if ((_finalCameraPosition - transform.position).magnitude <= _smoothFollowClipDistance)
            {
                //print("reached him!");
                _smoothFollow = false;
            }
        }
        transform.LookAt(_finalCameraLookAt);
    }

    public void MoveCamera(float pXRotation, float pYRotation)
    {
        _target.transform.Rotate(0, pXRotation * _horizontalCameraRotationSpeed, 0);
        float xRotation = _targetCameraHelper.eulerAngles.x;
        if (xRotation > 90) { xRotation = (360 - xRotation) * -1; }

        if (pYRotation > 0 && xRotation > _minVerticalCameraRotation)
        {
            _targetCameraHelper.transform.Rotate(-pYRotation * _verticalCameraRotionSpeed, 0, 0);
        }
        else if (pYRotation < 0 && xRotation < _maxVerticalCameraRotation)
        {
            _targetCameraHelper.transform.Rotate(-pYRotation * _verticalCameraRotionSpeed, 0, 0);
        }

        //DESIGNER A Drawline to visualise where the camera looks at
        Debug.DrawLine(transform.position, _finalCameraLookAt);
    }

    public void ActivateSmoothFollowOnFlash(float pSmoothFollowIncrement, float pSmoothFollowClipDistance)
    {
        _smoothFollowIncrement = pSmoothFollowIncrement;
        _smoothFollowClipDistance = pSmoothFollowClipDistance;
        _smoothFollow = true;
        //Maybe make this higher
        _smoothFollowSpeed = 0.0f;
    }
}
