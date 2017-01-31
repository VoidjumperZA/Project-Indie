using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class VoteToExitSubmenu : MonoBehaviour
{
    [SerializeField]
    private Image[] votersProfiles;

    private ActivePlayers activePlayers;
    private Vector3 originalPositionStack;
    private int totalVotes;
    private int currentVotes;

    private bool[] voteAxisLock = new bool[4];

    private int pauseScreenOwner;

    // Use this for initialization
    void Start()
    {
        activePlayers = GameObject.Find("Manager").GetComponent<ActivePlayers>();

        for (int i = 0; i < votersProfiles.Length; i++)
        {
            votersProfiles[i].GetComponent<Image>().enabled = false;
        }
        originalPositionStack = votersProfiles[0].transform.position;
        currentVotes = 0;
        totalVotes = activePlayers.GetPlayersInMatchArraySize();
        for (int i = 0; i < voteAxisLock.Length; i++)
        {
            voteAxisLock[i] = false;
        }
      

    }

    // Update is called once per frame
    void Update()
    {
        registerVotes();
        checkVoteCount();
    }

    private void registerVotes()
    {
        for (int i = 0; i < totalVotes; i++)
        {
          if (InputManager.AcceptButton(i + 1) > 0 && voteAxisLock[i] == false)
          {
              voteAxisLock[i] = true;
                //there are five players, IDs are 1-4, so get the number player so they portrait will have the right colour
                int playerNumber = activePlayers.GetPlayerNumberFromID(i + 1) - 1;
                votersProfiles[playerNumber].GetComponent<Image>().enabled = !votersProfiles[playerNumber].GetComponent<Image>().enabled;

                if (votersProfiles[playerNumber].GetComponent<Image>().enabled == true)
                {
                    currentVotes++;
                    Vector3 newPosition = votersProfiles[playerNumber].transform.position;
                    newPosition.x = votersProfiles[playerNumber].transform.position.x + (votersProfiles[playerNumber].rectTransform.rect.width + (votersProfiles[playerNumber].rectTransform.rect.width * (1 / votersProfiles[playerNumber].rectTransform.rect.width))) * currentVotes;
                    votersProfiles[playerNumber].transform.position = newPosition;
                }
                else
                {
                    currentVotes--;
                    votersProfiles[playerNumber].transform.position = originalPositionStack;
                }
                
          }
          else if (InputManager.AcceptButton(i + 1) == 0)
          {
              voteAxisLock[i] = false;
          }
        }
    }

    private void checkVoteCount()
    {
        if (currentVotes == totalVotes)
        {
            SceneManager.LoadScene(0);
        }
    }
}
