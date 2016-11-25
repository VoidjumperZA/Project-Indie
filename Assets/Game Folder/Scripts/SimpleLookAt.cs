using UnityEngine;
using System.Collections;

public class SimpleLookAt : MonoBehaviour
{
    private Transform player;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.rotation = player.gameObject.transform.rotation;
    }

    public void AssignPlayer(Transform pPlayer)
    {
        player = pPlayer;
    }

}
