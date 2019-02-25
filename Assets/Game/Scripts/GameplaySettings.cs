﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameplaySettings : ScriptableObject
{
    [SerializeField]
    private float _pinGravityScale = 1.0f;
    public float PinGravityScale => _pinGravityScale;
    
    [SerializeField]
    private float _pinMassScale = 1.0f;
    public float PinMassScale => _pinMassScale;

    [SerializeField]
    private List<PinConfig> _pinConfigs = default;

    public PinConfig GetRandConfig()
    {
        return _pinConfigs[UnityEngine.Random.Range(0, _pinConfigs.Count)];
    }
}
