using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReleasePinsOnTrigger : MonoBehaviour
{
    [SerializeField]
    private Sfx _hitSfx = default;

    [SerializeField]
    private ParticleSystem _hitParticles = default;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager.Instance.PlaySound(_hitSfx);
        GameManager.Instance.PinLockLine.UnlockAll();

        _hitParticles.Play();
    }
}
