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
    private SpriteRenderer _spriteRenderer = default;
    [SerializeField]
    private SpringJoint2D _springJoint = default;
    [SerializeField]
    private List<Collider2D> _collider = default;

    [SerializeField]
    private GameObject _upperColliderObj = default;

    [SerializeField]
    private Sfx _sfx = default;
    [SerializeField]
    private float _sfxCooldown = 0.25f;
    [SerializeField]
    private AutoMoveDown _moveDown = default;
    private float _lastSfx;

    private float _xCoord;
    private float _mass = 1.0f;

    private float _pitchScale = 1.0f;

    private GameplaySettings _settings;

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

    public void InitPin(PinConfig config, float height)
    {
        _moveDown.SetSpeed(config.SlipSpeed);

        _upperColliderObj.SetActive(config.HasResetHead);

        _pitchScale = config.PitchScale;

        _mass = config.Mass;

        for (int i = 0; i < _collider.Count; i++)
            _collider[i].sharedMaterial = config.PhysicsMat;

        _spriteRenderer.sprite = config.PinSprite;
        _spriteRenderer.color = config.PinColor;

        Vector2 anchorPos = transform.position;
        anchorPos.y += height;
        _springJoint.connectedAnchor = anchorPos;
    }

    public void ApplySettings(GameplaySettings gameplaySettings)
    {
        _settings = gameplaySettings;
        ApplySettings();
    }

    private void ApplySettings()
    {
        _rigidbody.mass = _mass * _settings.PinMassScale;
        _rigidbody.gravityScale = _settings.PinGravityScale;
    }

    public bool TryLock()
    {   
        if (_state == State.Free)
        {
            _springJoint.enabled = false;
            //_rigidbody.simulated = false;
            _rigidbody.gravityScale = 0.0f;
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
            _springJoint.enabled = true;
            //will re-enable gravity:
            ApplySettings();
            // _rigidbody.simulated = true;
            _rigidbody.velocity = Vector2.zero;
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

    public void Move(Vector2 dir)
    {
        _rigidbody.position += dir * Time.fixedDeltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(Time.time - _lastSfx >= _sfxCooldown)
        {
            GameManager.Instance.PlaySound(_sfx, _pitchScale);
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
