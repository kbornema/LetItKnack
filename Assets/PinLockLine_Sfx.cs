using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinLockLine_Sfx : MonoBehaviour
{
    [SerializeField]
    private Sfx _sfx = default;
    [SerializeField]
    private PinLockLine _line = default;

    public void Awake()
    {
        _line.OnLockedPinEvent.AddListener(OnPinLocked);
        
    }

    private void OnPinLocked(PinLockLine.Args arg0)
    {
        GameManager.Instance.PlaySound(_sfx);
    }
}
