using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinSlot : MonoBehaviour
{
    [SerializeField]
    private Pin _pin = default;
    public Pin GetPin() { return _pin; }

    [SerializeField]
    private SpriteRenderer _feedbackSprite = default;

    public void OnCanBeLocked()
    {
        _feedbackSprite.color = Color.green;
    }

    public void OnCanNotBeLocked()
    {
        _feedbackSprite.color = Color.yellow;
    }
}
