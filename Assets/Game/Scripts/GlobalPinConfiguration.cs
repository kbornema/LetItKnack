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
    private List<Transform> _pinPositions = default;

    [SerializeField]
    private List<LevelDescription> _levels = default;
    public int NumLevels { get { return _levels.Count; } }

    public string GetHelpText(int curLevel)
    {
        var text = _levels[curLevel].TextToDisplay;

        if (text)
            return text.text;

        return "";
    }

    public bool HasLevel(int level)
    {
        if (level >= _levels.Count)
            return false;

        return true;
    }

    public void Spawn(int levelIndex)
    {
        var levelDesc = _levels[levelIndex];
        var pinDesc = levelDesc.Pins;

        int numSpawned = 0;

        for (int i = 0; i < pinDesc.Count; i++)
        {
            if(pinDesc[i])
            {
                var instance = Instantiate(_pinPrefab, _pinPositions[i].position, Quaternion.identity, _pinRoot);

                var pos = _pinPositions[i].localPosition;
                instance.InitPin(i, pinDesc[i], _height, pos);
                numSpawned++;
            }
        }

        if(numSpawned == 0)
        {
            Debug.LogError("Need pins!");
        }
    }


}
