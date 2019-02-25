using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMoveDown : MonoBehaviour
{
    [SerializeField]
    private Pin _pin = default;

    private bool _shouldMove = false;
    private float _slipSpeed = 0.0f;

    private void Awake()
    {
        if (!enabled)
            return;

        _pin.PinStateChangedEvent.AddListener(OnStateChanged);
    }

    private void FixedUpdate()
    {
        if (!enabled)
            return;

        if(_shouldMove)
            _pin.Move(new Vector2(0.0f, -_slipSpeed));
    }

    private void OnStateChanged(Pin.Args arg0)
    {
        if (!enabled)
            return;

        if (arg0.ThePin.GetState() == Pin.State.Locked)
        {
            _shouldMove = true;
        }
        else
        {
            _shouldMove = false;
        }
    }

    public void SetSpeed(float slipSpeed)
    {
        _slipSpeed = slipSpeed;

        if (_slipSpeed <= 0.0f)
            enabled = false;
    }
}
