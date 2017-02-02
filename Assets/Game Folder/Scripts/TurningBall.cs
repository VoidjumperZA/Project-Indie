using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurningBall : MonoBehaviour
{

    //private GameObject _ring1;
    private Transform _ring2;
    private Transform _ring3;

    private void Start()
    {
        //_ring2 = transform.GetChild(0);
        // _ring3 = transform.GetChild(1);
        _ring2 = transform.FindChild("BallRing_2");
        _ring3 = transform.FindChild("BallRing_3");
          
    }

    private void FixedUpdate()
    {
        _ring2.Rotate(0.0f, 1.0f, 0.0f);
        _ring3.Rotate(1.0f, 0.0f, 0.0f);
    }
}

