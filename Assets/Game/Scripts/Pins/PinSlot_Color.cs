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
