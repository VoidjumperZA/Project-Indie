using UnityEngine;
using System.Collections;

public class Pentagram : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TogglePentagram(bool pState, Transform pPlayerTransform)
    {
        gameObject.SetActive(pState);

        gameObject.GetComponentInChildren<SimpleLookAt>().AssignPlayer(pPlayerTransform);
    }
}
