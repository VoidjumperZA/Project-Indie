using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class VoteToExitSubmenu : MonoBehaviour
{
    [SerializeField]
    private Image[] votersProfiles;
    private int totalVotes;
    private int currentVotes;

    private bool voteAxisLock;

    // Use this for initialization
    void Start()
    {
        currentVotes = 0;
        totalVotes = 4;
        voteAxisLock = false;
    }

    // Update is called once per frame
    void Update()
    {
        checkVotes();
    }

    private void checkVotes()
    {
        /*for (int i = 0; i < length; i++)
        {

        }*/
        if (InputManager.MovementVertical(pauseScreenOwner) > 0 && voteAxisLock == false)
        {
            voteAxisLock = true;

        }
        else if (InputManager.MovementVertical(pauseScreenOwner) == 0)
        {
            voteAxisLock = false;
        }
    }
}
