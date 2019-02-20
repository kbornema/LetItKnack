using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pin : MonoBehaviour
{
    public enum State { Free, Locked }

    [SerializeField]
    private Rigidbody2D _rigidbody = default;
    [SerializeField]
    private float _mass = 1.0f;
    [SerializeField]
    private PhysicsMaterial2D _material = default;
    
    private State _state = State.Free;
    public State GetState() { return _state; }

    private bool _isOnPinLine = false;
    public bool IsOnPinLine { get { return _isOnPinLine; } }

    private float _xCoord;

    public MyEvent<Args> PinStateChangedEvent = new MyEvent<Args>();
    public MyEvent<Args> PinOnLineChangedEvent = new MyEvent<Args>();

    private void Reset()
    {
        if (!_rigidbody)
            _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Awake()
    {
        _xCoord = transform.position.x;
        _rigidbody.sharedMaterial = _material;
    }

    private void Start()
    {
        GameManager.Instance.RegisterPin(this);
    }

    private void LateUpdate()
    {
        var pos = transform.position;

        if(pos.x != _xCoord)
        {
            pos.x = _xCoord;
            transform.position = pos;
        }
    }

    public void ApplySettings(GameplaySettings gameplaySettings)
    {
        _rigidbody.mass = _mass * gameplaySettings.PinMassScale;
        _rigidbody.gravityScale = gameplaySettings.PinGravityScale;
    }

    public bool TryLock()
    {   
        if (_state == State.Free)
        {       
            _rigidbody.simulated = false;
            _rigidbody.velocity = Vector2.zero;
            _state = State.Locked;
            PinStateChangedEvent.Invoke(new Args(this));
            return true;
        }

        return false;
    }

    public bool TryUnlock()
    {
        if (_state == State.Locked)
        {
            _rigidbody.simulated = true;
            _state = State.Free;
            PinStateChangedEvent.Invoke(new Args(this));
            return true;
        }

        return false;
    }

    public void SetSlotIsOnPinLine(bool isOnPinLine)
    {
        if(_isOnPinLine != isOnPinLine)
        {
            _isOnPinLine = isOnPinLine;
            PinOnLineChangedEvent.Invoke(new Args(this));
        }
    }

    public struct Args
    {
        public Pin ThePin;

        public Args(Pin p)
        {
            ThePin = p;
        }
    }
}
