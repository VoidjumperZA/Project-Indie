using UnityEngine;
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
    private bool _counting = false;

    private void Awake()
    {
        
    }

    private void startCountDown()
    {
        print(_countDownImages[1].name);
        
    }

    private void Start()
    {
        _matchDuration = (int)MatchStatistics.GetMatchTimeInMinutes() * 60;
        //StartCoroutine("countDown");
        Debug.Log("ScoreAndTimerHandler got a _matchDuration of " + _matchDuration + " from MatchStatistics.");
        _startMatchTimeStamp = Time.time;
        startCountDown();
    }

    private void Update()
    {
        _team1_scoreText.text = "" + MatchStatistics.GetLifeFireLeft(1).ToString();
        _team2_scoreText.text = "" + MatchStatistics.GetLifeFireLeft(2).ToString();
        //_timerText.text = "Time: " + ((int)Time.time).ToString();
        if (_counting)
        {
            _counter += Time.deltaTime;
        }

        _timerText.text = transform2Clock(_matchDuration - (int)(Time.time - _counter));
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
        _counting = pState;
    }

    private IEnumerator countDown()
    {
        print("INSIDE COUNTDOWN BABEYYYYYYYYYYYYYYYYYYYYYYYY");
        for (int i = 0; i < _countDownImages.Length; i++)
        {
            //_countDownImages[i].enabled = true;
            print(_countDownImages[i].name);
            print("Before");
            yield return new WaitForSeconds(5.0f);
            print("After");
        }
    }
}
