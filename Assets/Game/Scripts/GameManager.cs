using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    public bool UpdateSettings = true;

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
    private List<Pin> _allPins = new List<Pin>();

    private Queue<AudioSource> _unusedAudio = new Queue<AudioSource>();



    [Header("Win")]
    [SerializeField]
    private int _numStartPins = 1;
    [SerializeField]
    private int _numEndPins = 7;
    [SerializeField]
    private float _winDelay = 0.5f;
    [SerializeField]
    private Sfx _wonSfx = default;

    private int _numCurPins;

    [HideInInspector]
    public bool IsInWinRoutine;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        _pinLine.OnLockedPinEvent.AddListener(OnLockedPin);
        _configuration.Spawn(_numStartPins);

        _numCurPins = _numStartPins;
    }
    
    public GameplaySettings GetSettings()
    {
        return _gameplaySettings;
    }

    private void OnLockedPin(PinLockLine.Args arg0)
    {
        if(_pinLine.NumLockedPins == _allPins.Count && !IsInWinRoutine)
        {
            if (_numCurPins >= _numEndPins)
            {
                Debug.Log("GAME IS OVER");
            }

            else
                StartCoroutine(WonRoutine());
        }
    }

    private IEnumerator WonRoutine()
    {
        IsInWinRoutine = true;

        yield return new WaitForSeconds(_winDelay);

        PlaySound(_wonSfx);

        yield return new WaitForSeconds(_winDelay);

        _numCurPins++;

        for (int i = _allPins.Count - 1; i >= 0; i--)
            Destroy(_allPins[i].gameObject);



        _pinLine.Clear();
        _allPins.Clear();

        _configuration.Spawn(_numCurPins);

        yield return null;

        IsInWinRoutine = false;
    }

    private void Update()
    {
        if(UpdateSettings)
            ApplyPinSettings();
    }

    public void RegisterPin(Pin pin)
    {
        _allPins.Add(pin);
        pin.ApplySettings(_gameplaySettings);
    }

    public void TryLockPins()
    {
        _pinLine.TryLockPins();
    }

    public void UnlockAllPins()
    {
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
}
