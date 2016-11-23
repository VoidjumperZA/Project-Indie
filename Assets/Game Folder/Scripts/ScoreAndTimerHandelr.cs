using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreAndTimerHandelr : MonoBehaviour
{
    [SerializeField]
    private Text _team1_scoreText;
    [SerializeField]
    private Text _team2_scoreText;
    [SerializeField]
    private Text _timerText;

    private void Start()
    {

    }

    private void Update()
    {
        _team1_scoreText.text = "Team1: " + MatchStatistics.GetGoalsForTeam(1).ToString();
        _team2_scoreText.text = "Team2: " + MatchStatistics.GetGoalsForTeam(2).ToString();
        //_timerText.text = "Time: " + ((int)Time.time).ToString();
        _timerText.text = transform2Clock((int)Time.time);
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

        return "Time: " + minutesString + ":" + secondsString;
    }
}
