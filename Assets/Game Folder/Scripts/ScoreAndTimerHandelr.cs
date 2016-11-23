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
        _team1_scoreText.text = "" + MatchStatistics.GetGoalsForTeam(1).ToString();
        _team2_scoreText.text = "" + MatchStatistics.GetGoalsForTeam(2).ToString();
        _timerText.text = "" + ((int)Time.time).ToString();
    }
}
