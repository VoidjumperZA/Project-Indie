using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowFocusBall : MonoBehaviour {
    private GameObject ball;
	// Use this for initialization
	void Start () {
        ball = GameObject.FindGameObjectWithTag("Ball");
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(ball.transform);
	}
}
