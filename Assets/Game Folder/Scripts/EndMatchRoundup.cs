using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndMatchRoundup : MonoBehaviour
{
    [SerializeField]
    private GameObject[] winPositions;

    private PlayerInput[] playerInputs;
    private ActivePlayers activePlayers;
    // Use this for initialization
    void Start()
    {
        activePlayers = GameObject.Find("Manager").GetComponent<ActivePlayers>();
        for (int i = 0; i < activePlayers.GetPlayersInMatchArraySize(); i++)
        {
            playerInputs[i] = activePlayers.GetPlayerInMatch(i).GetComponent<PlayerInput>();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
