﻿using UnityEngine;
using System.Collections;

public class SimpleRotate : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, GameObject.Find("Manager").GetComponent<ColumnControl>().GetSelectionPentagramRotationSpeed());
    }
}
