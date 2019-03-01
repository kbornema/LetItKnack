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
    [SerializeField]
    private float _alpha = 1.0f;

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
                SetColor(Color.red);

            else
                SetColor(Color.green);
        }

        else
        {
            SetColor(new Color(1.0f, 1.0f, 0.0f));
            
        }
    }

    private void SetColor(Color c)
    {
        c.a = _alpha;
        _spriteRenderer.color = c;
    }
}
