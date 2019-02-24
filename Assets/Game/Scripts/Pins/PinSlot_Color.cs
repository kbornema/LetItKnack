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

    private void Start()
    {
        _slot.OnSlotCanBeLocked.AddListener(OnCanBeLocked);
        _slot.OnSlotCanBeNotLocked.AddListener(OnCanNotBeLocked);

        _slot.GetPin().PinStateChangedEvent.AddListener(OnPinStateChanged);
        
    }

    private void OnPinStateChanged(Pin.Args arg0)
    {   
        _spriteRenderer.enabled = (arg0.ThePin.GetState() != Pin.State.Locked);
    }

    private void OnCanBeLocked(PinSlot arg0)
    {
        if(arg0.GetMode() == PinSlot.Mode.Lock)
        {
            _spriteRenderer.color = Color.green;
        }

        else
        {
            _spriteRenderer.color = Color.red;
        }
    }

    private void OnCanNotBeLocked(PinSlot arg0)
    {
        if (arg0.GetMode() == PinSlot.Mode.Lock)
        {
            _spriteRenderer.color = Color.yellow;
        }

        else
        {
            _spriteRenderer.color = Color.red;
        }
    }
}
