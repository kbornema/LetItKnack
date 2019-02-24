using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pin : MonoBehaviour
{
    public enum State { Free, Locked }

    [SerializeField]
    private Rigidbody2D _rigidbody = default;
    
    private State _state = State.Free;
    public State GetState() { return _state; }

    public bool IsOnPinLine { get { return _slotOnPinLine != null; } }

    private PinSlot _slotOnPinLine;
    public PinSlot SlotOnPinLine { get { return _slotOnPinLine; } }

    public GenericEvent<Args> PinStateChangedEvent = new GenericEvent<Args>();
    public GenericEvent<Args> PinOnLineChangedEvent = new GenericEvent<Args>();

    [SerializeField]
    private SpringJoint2D _springJoint = default;

    [SerializeField]
    private List<Collider2D> _collider = default;

    [SerializeField]
    private Sfx _sfx = default;
    [SerializeField]
    private float _sfxCooldown = 0.25f;
    private float _lastSfx;

    private float _xCoord;
    private float _mass = 1.0f;

    private void Reset()
    {
        if (!_rigidbody)
            _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Awake()
    {
        _xCoord = transform.position.x;
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

    public void InitPin(float mass, float height)
    {
        _mass = mass;

        Vector2 anchorPos = transform.position;
        anchorPos.y += height;
        _springJoint.connectedAnchor = anchorPos;
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

    public void SetSlotIsOnPinLine(PinSlot pinSlot)
    {
        if(_slotOnPinLine != pinSlot)
        {
            _slotOnPinLine = pinSlot;
            PinOnLineChangedEvent.Invoke(new Args(this));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(Time.time - _lastSfx >= _sfxCooldown)
        {
            GameManager.Instance.PlaySound(_sfx);
            _lastSfx = Time.time;
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
