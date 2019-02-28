using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinSlot : MonoBehaviour
{
    public enum Mode { Lock, Fail }

    [SerializeField]
    private Pin _pin = default;
    public Pin GetPin() { return _pin; }

    [SerializeField]
    private Mode _mode = Mode.Lock;
    public Mode GetMode() { return _mode; }

    public GenericEvent<PinSlot> OnSlotCanBeLocked = new GenericEvent<PinSlot>();
    public GenericEvent<PinSlot> OnSlotCanBeNotLocked = new GenericEvent<PinSlot>();

    public bool OnUse(PinLockLine line)
    {
        if(_mode == Mode.Lock)
        {
            return _pin.TryLock(line);
        }

        else
        {
            line.UnlockAll();
            return true;
        }
    }

    public void CanBeLocked()
    {
        OnSlotCanBeLocked.Invoke(this);
    }

    public void CanNotBeLocked()
    {
        OnSlotCanBeNotLocked.Invoke(this);
    }
}
