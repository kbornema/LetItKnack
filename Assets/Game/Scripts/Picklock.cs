using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Picklock : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D _rigidbody = default;
    [SerializeField]
    private float _maxForceToApply = 150.0f;
    [SerializeField]
    private float _maxVelocity = 1000.0f;

    public void AddForce(Vector2 dir, float force)
    {
        _rigidbody.AddForce(dir * Mathf.Clamp(force, 0.0f, _maxForceToApply), ForceMode2D.Force);
    }

    private void FixedUpdate()
    {
        Vector2 velocity = _rigidbody.velocity;
        float velocityForce = velocity.magnitude;

        if(velocityForce > _maxVelocity)
            _rigidbody.velocity = (velocity / velocityForce) * _maxVelocity;
    }

    public Vector2 GetPosition()
    {
        return _rigidbody.position;
    }
}
