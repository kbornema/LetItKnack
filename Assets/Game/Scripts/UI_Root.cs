using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Root : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI _text = default;
    [SerializeField]
    private Toggle _tutorialToggle = default;

    [SerializeField]
    private TextInfo _finalText = null;
    [SerializeField]
    private GameObject _timerStartText = default;

    [SerializeField]
    private TMPro.TextMeshProUGUI _timerText = default;

    private int _currentSeconds = 0;
    private int _currentMinutes = 0;

    private void Awake()
    {
        _tutorialToggle.onValueChanged.AddListener(OnTutorialToggleValueChanged);
        GameManager.Instance.OnLevelCompletedEvent.AddListener(OnLevelCompleted);
        GameManager.Instance.OnGameFinishedEvent.AddListener(OnGameFinished);
    }

    private void Start()
    {
        OnLevelCompleted(GameManager.Instance);
    }

    private void Update()
    {
        if (GameManager.Instance.GameIsOver)
            return;

        float time = GameManager.Instance.TotalTimeNeeded;

        int seconds = (int)time % 60;
        int minutes = (int)time / 60;

        if(seconds != _currentSeconds || minutes != _currentMinutes)
        {
            _currentSeconds = seconds;
            _currentMinutes = minutes;

            _timerText.text = "Time: " + _currentMinutes.ToString("00") + ":" + _currentSeconds.ToString("00");
        }
    }

    private void OnTutorialToggleValueChanged(bool newVal)
    {
        _text.enabled = newVal;
    }

    private void OnLevelCompleted(GameManager arg0)
    {
        if (_timerStartText && arg0.CurrentLevel > 0)
        {
            _timerStartText.SetActive(false);
        }

        var text = arg0.GetHelpText();
        _text.text = text;
    }   

    private void OnGameFinished(GameManager arg0)
    {
        _text.text = _finalText.text;
    }
}
