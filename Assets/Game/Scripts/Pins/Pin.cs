using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pin : MonoBehaviour
{
    public enum State { Free, Locked }

    [SerializeField]
    private Rigidbody2D _rigidbody = default;
    public Rigidbody2D GetPinBody() { return _rigidbody; }
    
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
    public SpringJoint2D GetSpringJoint() { return _springJoint; }
    [SerializeField]
    private List<Collider2D> _collider = default;

    [SerializeField]
    private List<GameObject> _upperColliderObj = default;
    [SerializeField]
    private Transform _slotPos = default;

    [SerializeField]
    private Sfx _sfx = default;
    [SerializeField]
    private float _sfxCooldown = 0.25f;
    [SerializeField]
    private AutoMoveDown _moveDown = default;
    [SerializeField]
    private Transform _upperPos = default;
    private float _lastSfx;

    private float _xCoord;
    private float _mass = 1.0f;

    private float _pitchScale = 1.0f;

    private GameplaySettings _settings;

    private float _lockedTime;
    public float LockedTime { get { return _lockedTime; } }

    [SerializeField]
    private ParticleSystem _lockedParticles = default;
    [SerializeField]
    private ParticleSystem _slippedParticles = default;

    public int PinIndex = -1;

    public Vector3 LocalDefaultPos;

    private void Reset()
    {
        if (!_rigidbody)
            _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Awake()
    {
        _xCoord = transform.localPosition.x;
    }

    private void Start()
    {
        GameManager.Instance.RegisterPin(this);
    }

    private void LateUpdate()
    {
        var pos = transform.localPosition;

        if(pos.x != _xCoord)
        {
            pos.x = _xCoord;
            transform.localPosition = pos;
        }
    }

    public void InitPin(int pinIndex, PinConfig config, float height, Vector3 localPos)
    {
        LocalDefaultPos = localPos;
        ResetPosition();
        _xCoord = localPos.x;

        PinIndex = pinIndex;
        _moveDown.SetSpeed(config.SlipSpeed);

        for (int i = 0; i < _upperColliderObj.Count; i++)
        {
            _upperColliderObj[i].SetActive(config.HasResetHead);
        }


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

    public bool TryLock(PinLockLine line)
    {   
        if (_state == State.Free)
        {
            _lockedParticles.Play();

            _rigidbody.gravityScale = 0.0f;
            _rigidbody.velocity = Vector2.zero;

            Vector2 offset = transform.position - _slotPos.position;
            Vector2 curPos = transform.position;
            curPos.y = line.transform.position.y + offset.y;
            transform.localPosition = curPos;
            _rigidbody.position = transform.position;

            _lockedTime = Time.time;
            _springJoint.enabled = false;

            _state = State.Locked;
            PinStateChangedEvent.Invoke(new Args(this));
            return true;
        }

        return false;
    }

    public Transform GetUpperTransform()
    {
        return _upperPos;
    }

    public bool TryUnlock()
    {
        if (_state == State.Locked)
        {
            _springJoint.enabled = true;
            ApplySettings();
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
        Vector2 pos = (Vector2)transform.localPosition + dir;

        transform.localPosition = pos;
        _rigidbody.position = transform.position;
    }

    public void ResetPosition()
    {
        transform.localPosition = LocalDefaultPos;
        _rigidbody.position = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(Time.time - _lastSfx >= _sfxCooldown)
        {
            GameManager.Instance.PlaySound(_sfx, _pitchScale);
            _lastSfx = Time.time;
        }
    }

    public void PlayMoveParticles()
    {
        _slippedParticles.Play();
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
