using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class VoteToExitSubmenu : MonoBehaviour
{
    [SerializeField]
    private Image[] votersProfiles;

    private ActivePlayers activePlayers;
    private Vector3 originalPositionStack;
    private int totalVotes;
    private int currentVotes;

    private bool voteAxisLock;

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
        voteAxisLock = false;

    }

    // Update is called once per frame
    void Update()
    {
        checkVotes();
    }

    private void checkVotes()
    {
        for (int i = 0; i < totalVotes; i++)
        {
          if (InputManager.AcceptButton(i) > 0 && voteAxisLock == false)
          {
              voteAxisLock = true;
                votersProfiles[i].GetComponent<Image>().enabled = !votersProfiles[i].GetComponent<Image>().enabled;

                if (votersProfiles[i].GetComponent<Image>().enabled == true)
                {
                    currentVotes++;
                    Vector3 newPosition = votersProfiles[i].transform.position;
                    newPosition.x = (votersProfiles[i].rectTransform.rect.width + (votersProfiles[i].rectTransform.rect.width * (1 / votersProfiles[i].rectTransform.rect.width))) * currentVotes;
                    votersProfiles[i].transform.position = newPosition;
                }
                else
                {
                    currentVotes--;
                    votersProfiles[i].transform.position = originalPositionStack;
                }
                
          }
          else if (InputManager.AcceptButton(i) == 0)
          {
              voteAxisLock = false;
          }
        }
    }
}
