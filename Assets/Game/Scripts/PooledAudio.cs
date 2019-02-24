using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledAudio : MonoBehaviour
{
    [SerializeField]
    private AudioSource _audioSource = default;

    // Update is called once per frame
    private void Update()
    {
        if(!_audioSource.isPlaying)
        {
            GameManager.Instance.SfxFinished(_audioSource);
        }
    }
}
