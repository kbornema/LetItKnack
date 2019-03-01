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

    [SerializeField]
    private CanvasGroup _endScreenGroup = default;
    [SerializeField]
    private TMPro.TextMeshProUGUI _endText = default;

    [SerializeField]
    private float _endFadeInTime = 1.0f;
    [SerializeField]
    private AnimationCurve _fadeInCurve = default;

    [SerializeField]
    private float _fadeDelay = 0.5f;

    [SerializeField]
    private Sfx _victorySfx = default;

    [SerializeField]
    private List<GameObject> _hudObjects = default;
    private bool _hudVisible = true;

    private void Awake()
    {
        _tutorialToggle.onValueChanged.AddListener(OnTutorialToggleValueChanged);
        GameManager.Instance.OnLevelCompletedEvent.AddListener(OnLevelCompleted);
        GameManager.Instance.OnGameFinishedEvent.AddListener(OnGameFinished);

        _endScreenGroup.alpha = 0.0f;
        _endScreenGroup.gameObject.SetActive(false);
    }

    private void Start()
    {
        OnLevelCompleted(GameManager.Instance);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            ShowHud(!_hudVisible);

        if (GameManager.Instance.GameIsOver)
            return;

        float time = GameManager.Instance.TotalTimeNeeded;

        int seconds = (int)time % 60;
        int minutes = (int)time / 60;

        if(seconds != _currentSeconds || minutes != _currentMinutes)
        {
            _currentSeconds = seconds;
            _currentMinutes = minutes;

            _timerText.text = "Time: " + GetTimeText();
        }
    }

    private string GetTimeText()
    {
        return _currentMinutes.ToString("00") + ":" + _currentSeconds.ToString("00");
    }

    private void OnTutorialToggleValueChanged(bool newVal)
    {
        _text.enabled = newVal;
    }

    private void OnLevelCompleted(GameManager arg0)
    {
        if(_endScreenGroup.gameObject.activeSelf)
        {
            _endScreenGroup.alpha = 0.0f;
            _endScreenGroup.gameObject.SetActive(false);
        }

        if (_timerStartText && arg0.CurrentLevel > 0)
        {
            _timerStartText.SetActive(false);
        }

        var text = arg0.GetHelpText();
        _text.text = text;
    }   

    private void OnGameFinished(GameManager arg0)
    {   
        StartCoroutine(EndRoutine());
    }

    public void ShowHud(bool val)
    {
        _hudVisible = val;

        foreach (var v in _hudObjects)
        {
            v.SetActive(val);
        }
    }

    private IEnumerator EndRoutine()
    {
        _endScreenGroup.alpha = 0.0f;
        _endScreenGroup.gameObject.SetActive(true);

        string finalText = string.Format(_finalText.text, GetTimeText());
        _endText.text = finalText;

        GameManager.Instance.PlaySound(_victorySfx);

        yield return new WaitForSeconds(_fadeDelay);

        float curTime = 0.0f;

        while(curTime < _endFadeInTime)
        {
            float t = Mathf.Clamp01(curTime / _endFadeInTime);

            _endScreenGroup.alpha = _fadeInCurve.Evaluate(t);

            curTime += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
    }
}
