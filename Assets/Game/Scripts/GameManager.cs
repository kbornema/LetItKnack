using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    [SerializeField]
    private GameplaySettings _gameplaySettings = default;

    public bool UpdateSettings = true;

    private List<Pin> _pins = new List<Pin>();

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
        _pins.Add(pin);
        pin.ApplySettings(_gameplaySettings);
    }

    private void ApplyPinSettings()
    {
        foreach (var pin in _pins)
            pin.ApplySettings(_gameplaySettings);
    }
}
