using UnityEngine;
using System.Collections;

public class ActivePlayers : MonoBehaviour
{
    [SerializeField]
    private GameObject[] activePlayerArray;
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

    public GameObject GetActivePlayerElement(int pActiveObjectID)
    {
        Debug.Log("Active Object ID is " + pActiveObjectID);
        return activePlayerArray[pActiveObjectID];
    }
}
