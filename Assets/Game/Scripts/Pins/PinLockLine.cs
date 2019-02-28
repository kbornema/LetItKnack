using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PinLockLine : MonoBehaviour
{
    private Dictionary<PinSlot, PinSlot> _collidingPins = new Dictionary<PinSlot, PinSlot>();

    [SerializeField]
    private List<PinSlot> _lockedPins = new List<PinSlot>();

    public int NumLockedPins { get { return _lockedPins.Count; } }

    private bool _hasLockablePins = false;
    public bool HasLockablePins { get { return _hasLockablePins; } }

    public GenericEvent<Args> OnHasLockablePinsChangedEvent = new GenericEvent<Args>();
    public GenericEvent<Args> OnLockedPinEvent = new GenericEvent<Args>();

    [SerializeField]
    private List<PinSlot> _lockablePins = new List<PinSlot>();
    public List<PinSlot> LockablePins { get { return _lockablePins; } }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var pinSlot = collision.GetComponent<PinSlot>();

        if(pinSlot)
        {
            if(!_collidingPins.ContainsKey(pinSlot))
            {
                pinSlot.CanBeLocked();
                _collidingPins.Add(pinSlot, pinSlot);
                pinSlot.GetPin().SetSlotIsOnPinLine(pinSlot);
                OnCheckLockablePins();
            }
        }
    }

    //TODO: bug: sometimes when exiting at the same frame the lock has been locked, it will unlock automatically
    private void OnTriggerExit2D(Collider2D collision)
    {
        var pinSlot = collision.GetComponent<PinSlot>();

        if (pinSlot)
        {
            if (_collidingPins.ContainsKey(pinSlot))
            {
                float timeDiff = Mathf.Abs(pinSlot.GetPin().LockedTime - Time.time);

                if(timeDiff < Time.fixedDeltaTime)
                    return;

                pinSlot.CanNotBeLocked();
                pinSlot.GetPin().SetSlotIsOnPinLine(null);
                _collidingPins.Remove(pinSlot);

                if(pinSlot.GetPin().GetState() == Pin.State.Locked)
                {
                    _lockedPins.Remove(pinSlot);
                    pinSlot.GetPin().TryUnlock();
                }

                OnCheckLockablePins();
            }
        }
    }

    public bool TryLockPins()
    {
        int numLocked = 0;

        foreach (var p in _collidingPins.Keys)
        {
            if(p.OnUse(this))
            {
                if (p.GetMode() == PinSlot.Mode.Lock)
                {
                    _lockedPins.Add(p);
                    numLocked++;
                }
            }
        }

        if (numLocked > 0)
            OnLockedPinEvent.Invoke(new Args(this));

        OnCheckLockablePins();
        return numLocked > 0;
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
        _lockablePins.Clear();

        foreach (var pinSlot in _collidingPins.Keys)
        {
            var pin = pinSlot.GetPin();

            if(pin.GetState() == Pin.State.Free)
            {
                _lockablePins.Add(pinSlot);
                return true;
            }
        }

        return false;
    }

    public void Clear()
    {
        _collidingPins.Clear();
        _lockablePins.Clear();
        _lockedPins.Clear();
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
