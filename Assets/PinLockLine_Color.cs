using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinLockLine_Color : MonoBehaviour
{
    [SerializeField]
    private PinLockLine _lockLine = default;
    [SerializeField]
    private SpriteRenderer _spriteRenderer = default;

    private void Start()
    {
        _lockLine.OnHasLockablePinsChangedEvent.AddListener(OnStateChanged);
    }

    private void OnStateChanged(PinLockLine.Args arg0)
    {
        if(_lockLine.HasLockablePins)
        {
            _spriteRenderer.color = Color.green;
        }

        else
        {
            _spriteRenderer.color = Color.yellow;
        }
    }
}
