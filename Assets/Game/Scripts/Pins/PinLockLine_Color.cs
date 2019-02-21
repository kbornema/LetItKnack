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
        var lockablePins = arg0.LockLine.LockablePins;

        var failSlot = lockablePins.Find(x => x.GetMode() == PinSlot.Mode.Fail);
        
        if (_lockLine.HasLockablePins)
        {
            if(failSlot != null)
                _spriteRenderer.color = Color.red;

            else
                _spriteRenderer.color = Color.green;
        }

        else
        {
            _spriteRenderer.color = Color.yellow;
        }
    }
}
