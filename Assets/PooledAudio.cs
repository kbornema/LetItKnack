using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledAudio : MonoBehaviour
{
    [SerializeField]
    private AudioSource _audioSource;

    // Update is called once per frame
    void Update()
    {
        if(!_audioSource.isPlaying)
        {
            GameManager.Instance.SfxFinished(_audioSource);
        }
    }
}
