using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActivePlayers : MonoBehaviour
{
    [SerializeField]
    private GameObject[] activePlayerArray;
    private List<GameObject> playersInMatchArray = new List<GameObject>();
    private Dictionary<int, int> playerNumberToIDDictionary = new Dictionary<int, int>();
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public GameObject[] GetActivePlayersArray()
    {
        return activePlayerArray;
    }

    public int GetActivePlayersArraySize()
    {
        return activePlayerArray.Length;
    }

    //returns a player from the list of all five possible players loaded into the scene
    public GameObject GetActivePlayerElement(int pActiveObjectID)
    {
        Debug.Log("Active Object ID is " + pActiveObjectID);
        return activePlayerArray[pActiveObjectID];
    }

    // Add our player to the list of players particpating in the match (while the other's gameobjects remain inactive).
    // We're also feeding it the player number (that is 1-5) and an ID (1-4: only the players IN the match) and adding that to a dictionary
    public void AddPlayerInMatch(GameObject pMatchPlayer, int pPlayerID, int pPlayerNumber)
    {
        playersInMatchArray.Add(pMatchPlayer);
        playerNumberToIDDictionary.Add(pPlayerID, pPlayerNumber);
    }

    //returns only one of the four particpating players, using their assigned IDs
    public GameObject GetPlayerInMatch(int pPlayerID)
    {
        return playersInMatchArray[pPlayerID - 1];
    }

    //simply returns a total of how many players are in this game (is it 1v1? a 2v2?)
    public int GetPlayersInMatchArraySize()
    {
        return playersInMatchArray.Count;
    }

    public int GetPlayerNumberFromID(int pPlayerID)
    {
        return playerNumberToIDDictionary[pPlayerID];
    }

}
