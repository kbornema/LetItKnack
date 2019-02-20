using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinState_Color : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _spriteRenderer = default;
    [SerializeField]
    private Pin _pin = default;

    private void Start()
    {
        _pin.PinStateChangedEvent.AddListener(OnPinStateChanged);
        _pin.PinOnLineChangedEvent.AddListener(OnPinStateChanged);
    }

    private void OnPinStateChanged(Pin.Args arg)
    {
        var state = _pin.GetState();

        if (state == Pin.State.Locked)
        {
            _spriteRenderer.color = Color.white;
        }

        else if (_pin.IsOnPinLine)
        {
            _spriteRenderer.color = Color.green;
        }

        else
        {
            _spriteRenderer.color = Color.yellow;
        }

    }
}
