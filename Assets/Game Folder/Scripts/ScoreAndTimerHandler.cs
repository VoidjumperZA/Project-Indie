﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreAndTimerHandler : MonoBehaviour
{
    [SerializeField]
    private Image[] _countDownImages;
    [SerializeField]
    private Text _team1_scoreText;
    [SerializeField]
    private Text _team2_scoreText;
    [SerializeField]
    private Text _timerText;

    private int _matchDuration;
    private float _startMatchTimeStamp;
    private float _counter = 0.0f;
    private bool _countingDown = false;

    //FMOD
    public string ding = "event:/TimerDing";
    public string dingLast = "event:/TimerDingLast";
    private PlayerCamera _cameraScript;
    [SerializeField]
    private Camera _playerCamera;
    private PlayerInput[] playerInputs;
    private ActivePlayers activePlayers;
    private EndMatchRoundup endMatchRoundUp;

    private void Start()
    {
        _matchDuration = MatchStatistics.GetMatchTimeInMinutes() * 60;
        _counter = Time.time;

        playerInputs = new PlayerInput[5];
        activePlayers = GameObject.Find("Manager").GetComponent<ActivePlayers>();
        endMatchRoundUp = GameObject.Find("Manager").GetComponent<EndMatchRoundup>();
        _cameraScript = _playerCamera.GetComponent<PlayerCamera>();
        for (int i = 0; i < activePlayers.GetPlayersInMatchArraySize(); i++)
        {
            playerInputs[i] = activePlayers.GetPlayerInMatch(i + 1).GetComponent<PlayerInput>();
        }
        StartCoroutine(CountDown());
    }

    private void Update()
    {
        _team1_scoreText.text = "" + MatchStatistics.GetLifeFireLeft(1).ToString();
        _team2_scoreText.text = "" + MatchStatistics.GetLifeFireLeft(2).ToString();

        if (!_countingDown)
        {
            _counter += Time.deltaTime;
        }

        //int timeInt = (_matchDuration - (int)(Time.time - _counter));
        //print("time: " + timeInt + ", matchDuration: " + _matchDuration + ", Time.time: " + Time.time + ", counter: " + _counter);

        if (_matchDuration - (int)(Time.time - _counter) <= 0)
        {
            float teamOneLifeFire = MatchStatistics.GetLifeFireLeft(1);
            float teamTwoLifeFire = MatchStatistics.GetLifeFireLeft(2);
            if (teamOneLifeFire > teamTwoLifeFire)
            {
                endMatchRoundUp.DisplayPodium(1);
            }
            else if (teamOneLifeFire < teamTwoLifeFire)
            {
                endMatchRoundUp.DisplayPodium(2);
            }
            else
            {
                endMatchRoundUp.DisplayPodium(0);
            }
        }
        _timerText.text = transform2Clock(_matchDuration - (int)(Time.time - _counter));
    }

    public int GetTransform2Clock()
    {
        return _matchDuration - (int)(Time.time - _counter);
    }
    private string transform2Clock(int pTime)
    {
        int minutes = pTime / 60;
        int seconds = pTime % 60;

        string minutesString;
        string secondsString;

        if (minutes < 10) { minutesString = "0" + minutes; }
        else { minutesString = minutes.ToString(); }
        if (seconds < 10) { secondsString = "0" + seconds; }
        else { secondsString = seconds.ToString(); }

        return "" + minutesString + ":" + secondsString;
    }

    public void SetCounting(bool pState)
    {
        _countingDown = pState;
    }

    public bool GetCoutingDown()
    {
        return _countingDown;
    }

    public IEnumerator CountDown()
    {
        _countingDown = false;
        //Make everyone respawn command here maybe?
        for (int i = 0; i < activePlayers.GetPlayersInMatchArraySize(); i++)
        {
            print(playerInputs[i].gameObject.name + playerInputs[i].GetControlsDisabled());
            playerInputs[i].SetControlsDisabled(true);
            print(playerInputs[i].gameObject.name + playerInputs[i].GetControlsDisabled());
        }
        //Start fading effect here maybe?
        for (int i = 0; i < _countDownImages.Length; i++)
        {
            _countDownImages[i].enabled = true;
            yield return new WaitForSeconds(1.0f);
            FMODUnity.RuntimeManager.PlayOneShot(ding, _cameraScript.gameObject.transform.position);
            _countDownImages[i].enabled = false;
        }
        for (int i = 0; i < activePlayers.GetPlayersInMatchArraySize(); i++)
        {
            playerInputs[i].SetControlsDisabled(false);
        }
        _countingDown = true;
        FMODUnity.RuntimeManager.PlayOneShot(dingLast, _cameraScript.gameObject.transform.position);
    }
}
