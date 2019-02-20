using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sfx
{
    [SerializeField]
    private AudioClip _clip = default;

    [SerializeField, Range(0.0f, 1.0f)]
    private float _volumeMin = 1.0f;
    [SerializeField, Range(0.0f, 1.0f)]
    private float _volumeMax = 1.0f;

    [SerializeField, Range(-3.0f, 3.0f)]
    private float _pitchMin = 1.0f;
    [SerializeField, Range(-3.0f, 3.0f)]
    private float _pitchMax = 1.0f;

    public bool HasClip { get { return _clip != null; } }

    public void Fill(AudioSource audio)
    {
        audio.clip = _clip;
        audio.volume = UnityEngine.Random.Range(_volumeMin, _volumeMax);
        audio.pitch = UnityEngine.Random.Range(_pitchMin, _pitchMax);
    }
}
