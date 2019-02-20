using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PinLockLine : MonoBehaviour
{
    private Dictionary<PinSlot, PinSlot> _collidingPins = new Dictionary<PinSlot, PinSlot>();
    private List<PinSlot> _lockedPins = new List<PinSlot>();

    public int NumLockedPins { get { return _lockedPins.Count; } }

    private bool _hasLockablePins = false;
    public bool HasLockablePins { get { return _hasLockablePins; } }

    public MyEvent<Args> OnHasLockablePinsChangedEvent = new MyEvent<Args>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var pinSlot = collision.GetComponent<PinSlot>();

        if(pinSlot)
        {
            if(_collidingPins.ContainsKey(pinSlot))
            {
                Debug.LogWarning("Already in?");
            }
            else
            {
                _collidingPins.Add(pinSlot, pinSlot);
                pinSlot.GetPin().SetSlotIsOnPinLine(true);
                OnCheckLockablePins();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var pinSlot = collision.GetComponent<PinSlot>();

        if (pinSlot)
        {
            if (_collidingPins.ContainsKey(pinSlot))
            {
                pinSlot.GetPin().SetSlotIsOnPinLine(false);
                _collidingPins.Remove(pinSlot);
                OnCheckLockablePins();
            }
            else
            {
                Debug.LogWarning("Not in?");
            }
        }
    }

    public void TryLockPins()
    {
        int numLocked = 0;

        foreach (var p in _collidingPins.Keys)
        {
            if (p.GetPin().TryLock())
            {
                _lockedPins.Add(p);
                numLocked++;
            }
        }

        //if(numLocked > 0)
        OnCheckLockablePins();
    }

    public void UnlockAll()
    {
        foreach (var p in _lockedPins)
            p.GetPin().TryUnlock();

        _lockedPins.Clear();

        OnCheckLockablePins();
    }

    private void OnCheckLockablePins()
    {
        var tmpHasLockablePins = CheckHasLockablePins();

        if(tmpHasLockablePins != _hasLockablePins)
        {
            _hasLockablePins = tmpHasLockablePins;
            OnHasLockablePinsChangedEvent.Invoke(new Args(this));
        }
    }

    private bool CheckHasLockablePins()
    {
        foreach (var pinSlot in _collidingPins.Keys)
        {
            var pin = pinSlot.GetPin();

            if(pin.GetState() == Pin.State.Free)
                return true;
        }

        return false;
    }

    public struct Args
    {
        public PinLockLine LockLine;

        public Args(PinLockLine line)
        {
            LockLine = line;
        }
    }
}
