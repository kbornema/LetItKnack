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
    private AudioSource _audioPrefab = default;
    private List<Pin> _allPins = new List<Pin>();

    private void Awake()
    {
        _instance = this;
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

    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            var newSound = Instantiate(_audioPrefab);
            newSound.clip = clip;
            newSound.Play();
        }
    }

    private void ApplyPinSettings()
    {
        foreach (var pin in _allPins)
            pin.ApplySettings(_gameplaySettings);
    }
}
