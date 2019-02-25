using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalPinConfiguration : MonoBehaviour
{
    [SerializeField]
    private float _height = 3.0f;
    [SerializeField]
    private Pin _pinPrefab = default;
    [SerializeField]
    private Transform _pinRoot = default;
    [SerializeField]
    private List<Configuration> _configuration = default;

    public void Spawn(int num)
    {       
        var configuration = _configuration[num - 1];

        var settings = GameManager.Instance.GetSettings();

        for (int i = 0; i < num; i++)
        {
            var instance = Instantiate(_pinPrefab, configuration.Positions[i].position, Quaternion.identity, _pinRoot);
            instance.InitPin(settings.GetRandConfig(), _height);
        }
    }

    [System.Serializable]
    private class Configuration
    {
        public List<Transform> Positions = new List<Transform>();
    }
}
