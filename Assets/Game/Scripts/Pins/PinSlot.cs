using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinSlot : MonoBehaviour
{
    [SerializeField]
    private Pin _pin = default;
    public Pin GetPin() { return _pin; }
}
