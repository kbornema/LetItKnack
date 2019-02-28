using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinSlot_Color : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _spriteRenderer = default;
    [SerializeField]
    private PinSlot _slot = default;

    private float _curAlpha = 1.0f;

    private void Start()
    {
        _slot.OnSlotCanBeLocked.AddListener(OnCanBeLocked);
        _slot.OnSlotCanBeNotLocked.AddListener(OnCanNotBeLocked);

        _slot.GetPin().PinStateChangedEvent.AddListener(OnPinStateChanged);
        
    }

    private void OnPinStateChanged(Pin.Args arg0)
    {       
        if(arg0.ThePin.GetState() == Pin.State.Locked)
        {
            _curAlpha = 0.5f;
            SetColor();
        }
        else
        {
            _curAlpha = 1.0f;
            SetColor();
        }
    }

    private void OnCanBeLocked(PinSlot arg0)
    {
        if(arg0.GetMode() == PinSlot.Mode.Lock)
        {
            SetColor(Color.green);
        }

        else
        {
            SetColor(Color.red);
        }
    }

    private void OnCanNotBeLocked(PinSlot arg0)
    {
        if (arg0.GetMode() == PinSlot.Mode.Lock)
        {
            SetColor(Color.yellow);
        }

        else
        {
            SetColor(Color.red);
        }
    }

    private void SetColor(Color col)
    {
        col.a = _curAlpha;
        _spriteRenderer.color = col;
    }

    private void SetColor()
    {
        SetColor(_spriteRenderer.color);
    }
}
