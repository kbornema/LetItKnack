using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool InDebugMode = false;

    private static GameManager _instance;
    public static GameManager Instance => _instance;

    [SerializeField]
    private int _targetFps = 60;
    [SerializeField]
    private PinLockLine _pinLine = default;
    public PinLockLine PinLockLine => _pinLine;
    [SerializeField]
    private GameplaySettings _gameplaySettings = default;
    [SerializeField]
    private GlobalPinConfiguration _configuration = default;
    [SerializeField]
    private AudioSource _audioPrefab = default;
    [SerializeField]
    private float _winDelay = 0.5f;
    [SerializeField]
    private float _feedbackWinDelay = 0.25f;
    [SerializeField]
    private Sfx _wonSfx = default;

    [SerializeField]
    private ParticleSystem _levelWonParticles = default;
    [SerializeField]
    private List<VisualSpring> _springs = default;
    [SerializeField]
    private ParticleSystem _lockParticles = default;
    [SerializeField]
    private float _wrongClickPenality = 0.25f;
    [SerializeField]
    private Sfx _wrongClickSfx = default;

    [Header("Debug")]
    [SerializeField]
    private List<Pin> _allPins = new List<Pin>();
    public bool GameIsOver = false;

    public bool ShouldCountTime = true;

    [SerializeField]
    private float _totalTimeNeeded = 0.0f;
    public float TotalTimeNeeded { get { return _totalTimeNeeded; } }

    [SerializeField]
    private int _curLevel = 0;

    public int CurrentLevel { get { return _curLevel; } }
    public int MaxLevels { get { return _configuration.NumLevels; } }

    public GenericEvent<GameManager> OnLevelCompletedEvent = new GenericEvent<GameManager>();
    public GenericEvent<GameManager> OnGameFinishedEvent = new GenericEvent<GameManager>();

    [HideInInspector]
    public bool IsInWinRoutine;
    private Queue<AudioSource> _unusedAudio = new Queue<AudioSource>();

    private void Awake()
    {
        Application.targetFrameRate = _targetFps;

        _instance = this;

        if (Application.isEditor)
            InDebugMode = true;

        for (int i = 0; i < _springs.Count; i++)
            _springs[i].gameObject.SetActive(false);
    }

    private void Start()
    {
        _pinLine.OnLockedPinEvent.AddListener(OnLockedPin);
        _configuration.Spawn(_curLevel);
    }
    
    public GameplaySettings GetSettings()
    {
        return _gameplaySettings;
    }

    private void OnLockedPin(PinLockLine.Args arg0)
    {
        if(_pinLine.NumLockedPins == _allPins.Count && !IsInWinRoutine)
        {
            if (!_configuration.HasLevel(_curLevel + 1))
            {
                GameIsOver = true;
                OnGameFinishedEvent.Invoke(this);
            }

            else
                StartCoroutine(WonRoutine());
        }
    }

    private IEnumerator WonRoutine()
    {
        IsInWinRoutine = true;  

        yield return new WaitForSeconds(_feedbackWinDelay);

        PlaySound(_wonSfx);
        _levelWonParticles.Play();

        yield return new WaitForSeconds(_winDelay);

        StepLevel(1);

        yield return null;

        IsInWinRoutine = false;
    }

    public void StepLevel(int delta)
    {
        GameIsOver = false;
        _curLevel = Mathf.Clamp(_curLevel + delta, 0, MaxLevels - 1);
        PrepareLevel();
    }

    private void PrepareLevel()
    {
        for (int i = 0; i < _springs.Count; i++)
            _springs[i].gameObject.SetActive(false);

        OnLevelCompletedEvent.Invoke(this);

        for (int i = _allPins.Count - 1; i >= 0; i--)
            Destroy(_allPins[i].gameObject);

        _pinLine.Clear();
        _allPins.Clear();

        _configuration.Spawn(_curLevel);

        if (!ShouldCountTime)
            ShouldCountTime = true;
    }

    private void Update()
    {
        if(ShouldCountTime)
            _totalTimeNeeded += Time.deltaTime;
    }

    public void RegisterPin(Pin pin)
    {
        var pinUp = pin.GetUpperTransform();

        var springIndex = _allPins.Count;
        var spring = _springs[pin.PinIndex];
        spring.gameObject.SetActive(true);
        spring.ObjToWatch = pinUp;

        _allPins.Add(pin);
        pin.ApplySettings(_gameplaySettings);
    }

    public void TryLockPins()
    {
        if(!_pinLine.TryLockPins())
        {
            int numPins = 0;
            foreach(var pin in _allPins)
            {
                if(pin.GetState() == Pin.State.Locked)
                {
                    pin.Move(new Vector2(0.0f, -_wrongClickPenality));
                    pin.PlayMoveParticles();

                    numPins++;
                }
            }

            if(numPins > 0)
                PlaySound(_wrongClickSfx);
        }

        else
        {
            _lockParticles.Play();
        }
    }

    public void UnlockAllPins()
    {
        GameIsOver = false;
        _pinLine.UnlockAll();
    }

    public void PlaySound(Sfx sfx, float pitchScale = 1.0f)
    {
        if (sfx.HasClip)
        {
            AudioSource audio = null;

            if(_unusedAudio.Count > 0)
            {
                audio = _unusedAudio.Dequeue();
                audio.gameObject.SetActive(true);
            }

            else
            {
                audio  = Instantiate(_audioPrefab);
            }

            sfx.Fill(audio);    
            audio.pitch = Mathf.Clamp(audio.pitch * pitchScale, -3.0f, 3.0f);
            audio.Play();
        }
    }

    public void SfxFinished(AudioSource audio)
    {
        audio.gameObject.SetActive(false);
        _unusedAudio.Enqueue(audio);
    }

    private void ApplyPinSettings()
    {
        foreach (var pin in _allPins)
            pin.ApplySettings(_gameplaySettings);
    }

    public string GetHelpText()
    {
        return _configuration.GetHelpText(_curLevel);
    }
}
