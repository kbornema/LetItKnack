using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMoveDown : MonoBehaviour
{
    [SerializeField]
    private Pin _pin = default;

    [SerializeField]
    private GameObject _feedbackObj = default;

    [SerializeField]
    private Transform _moveFeedback = default;

    private bool _shouldMove = false;
    private float _slipSpeed = 0.0f;

    [SerializeField]
    private float _vibratePower = 0.2f;
    [SerializeField]
    private float _vibrateSpeed = 1.0f;
    [SerializeField]
    private ParticleSystem _feedbackParticles = default;

    private float _curVibrateTime = 0.0f;

    [SerializeField]
    private Sfx _vibrateSfx = default;
    [SerializeField]
    private float _vibrateCd = 0.25f;

    private float _lastVibrateSfx;

    private void Awake()
    {
        if (!enabled)
            return;

        _pin.PinStateChangedEvent.AddListener(OnStateChanged);
    }

    private void FixedUpdate()
    {
        if (!enabled || GameManager.Instance.IsInWinRoutine || GameManager.Instance.GameIsOver)
        {   
            if (_feedbackParticles && _feedbackParticles.isPlaying)
                _feedbackParticles.Stop();

            return;
        }

        if(_shouldMove)
        {   
            _pin.Move(new Vector2(0.0f, -_slipSpeed * Time.fixedDeltaTime));

            if(_moveFeedback)
            {
                _curVibrateTime += Time.fixedDeltaTime;
                float localX = Mathf.Sin(_curVibrateTime * _vibrateSpeed) * _vibratePower;
                var pos = _moveFeedback.localPosition;
                pos.x = localX;
                _moveFeedback.localPosition = pos;
            }

            if(Time.time - _lastVibrateSfx >= _vibrateCd)
            {
                _lastVibrateSfx = Time.time;
                GameManager.Instance.PlaySound(_vibrateSfx);
            }
        }
    }

    private void OnStateChanged(Pin.Args arg0)
    {
        if (!enabled)
            return;

        if (arg0.ThePin.GetState() == Pin.State.Locked)
        {
            _shouldMove = true;

            if (_feedbackParticles)
            {   
                _feedbackParticles.Play();
            }
        }
        else
        {
            _shouldMove = false;
            _moveFeedback.localPosition = Vector3.zero;

            if (_feedbackParticles)
                _feedbackParticles.Stop();
        }
    }

    public void SetSpeed(float slipSpeed)
    {
        _slipSpeed = slipSpeed;

        if (_slipSpeed <= 0.0f)
        {
            enabled = false;

            if (_feedbackObj)
                _feedbackObj.SetActive(false);
        }
    }
}
