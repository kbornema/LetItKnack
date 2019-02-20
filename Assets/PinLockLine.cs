using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinLockLine : MonoBehaviour
{
    private Dictionary<PinSlot, PinSlot> _collidingPins = new Dictionary<PinSlot, PinSlot>();
    private List<PinSlot> _lockedPins = new List<PinSlot>();

    [SerializeField]
    private SpriteRenderer _feedbackSprite = default;

    // Update is called once per frame
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            TryLockPins();
        }

        if(Input.GetMouseButtonDown(1))
        {
            UnlockAll();
        }
    }

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
                pinSlot.OnCanBeLocked();
                _collidingPins.Add(pinSlot, pinSlot);
                CheckPins();
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
                pinSlot.OnCanNotBeLocked();
                _collidingPins.Remove(pinSlot);
                CheckPins();
            }
            else
            {
                Debug.LogWarning("Not in?");
            }
        }
    }

    private void TryLockPins()
    {
        foreach (var p in _collidingPins.Keys)
        {
            if (p.GetPin().TryLock())
            {
                _lockedPins.Add(p);
                CheckPins();
            }
        }
    }

    private void UnlockAll()
    {
        foreach (var p in _lockedPins)
            p.GetPin().TryUnlock();

        _lockedPins.Clear();
        CheckPins();
    }

    private void CheckPins()
    {
        foreach(var pinSlot in _collidingPins.Keys)
        {
            var pin = pinSlot.GetPin();
            
            if(pin.GetState() == Pin.State.Free)
            {
                Feedback(Color.green);
                return;
            }
        }

        Feedback(Color.yellow);
    }

    private void Feedback(Color color)
    {
        _feedbackSprite.color = color;
    }
}
