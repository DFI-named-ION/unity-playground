using TMPro;
using UnityEngine;

public class MiniGame : MonoBehaviour
{
    private int _currentScore = 0;
    private int _currentValue = 0;
    private bool _isCurrentValueEven = false;
    private bool _isQuestionValueEven = false;

    public GameObject TimerOutput;
    public GameObject NumberOutput;
    public GameObject OddEvenOutput;
    public GameObject ScoreOutput;

    public float RoundDurationSeconds = 20f;

    private float _timeLeft;
    private bool _isRunning;

    public System.Action<int> OnGameFinished;

    public void StartGame()
    {
        _currentScore = 0;
        _timeLeft = RoundDurationSeconds;
        _isRunning = true;

        GenerateNext();
        UpdateInfo();
        UpdateTimerUI();
    }

    private void Update()
    {
        if (!_isRunning) return;

        _timeLeft -= Time.deltaTime;
        if (_timeLeft <= 0f)
        {
            _timeLeft = 0f;
            _isRunning = false;
            UpdateTimerUI();
            OnTimeUp();
            return;
        }

        UpdateTimerUI();
    }

    public void OnButtonYesClick()
    {
        if (!_isRunning)
            return;

        //Debug.LogError($"current: {_currentValue}, is: {(_isCurrentValueEven ? "even" : "odd")}, " +
        //    $"question: is {(_isQuestionValueEven ? "even" : "odd")}, give point?: {_isCurrentValueEven == _isQuestionValueEven}");

        if (_isCurrentValueEven == _isQuestionValueEven)
            _currentScore++;

        GenerateNext();
        UpdateInfo();
    }

    public void OnButtonNoClick()
    {
        if (!_isRunning)
            return;

        //Debug.LogError($"current: {_currentValue}, is: {(_isCurrentValueEven ? "even" : "odd")}, " +
        //    $"question: is {(_isQuestionValueEven ? "even" : "odd")}, give point?: {_isCurrentValueEven == _isQuestionValueEven}");

        if (_isCurrentValueEven != _isQuestionValueEven)
            _currentScore++;

        GenerateNext();
        UpdateInfo();
    }

    private void UpdateInfo()
    {
        NumberOutput.GetComponent<TextMeshProUGUI>().text = _currentValue.ToString();
        OddEvenOutput.GetComponent<TextMeshProUGUI>().text = _isQuestionValueEven ? "even" : "odd";
        ScoreOutput.GetComponent<TextMeshProUGUI>().text = _currentScore.ToString();
    }

    private void UpdateTimerUI()
    {
        if (TimerOutput == null) return;
        TimerOutput.GetComponent<TextMeshProUGUI>().text = Mathf.CeilToInt(_timeLeft).ToString();
    }

    private void GenerateNext()
    {
        _currentValue = Random.Range(-100, 100 + 1);
        _isCurrentValueEven = _currentValue % 2 == 0;
        _isQuestionValueEven = Random.Range(1, 2 + 1) % 2 == 0;
    }

    private void OnTimeUp()
    {
        OnGameFinished?.Invoke(_currentScore);
    }
}