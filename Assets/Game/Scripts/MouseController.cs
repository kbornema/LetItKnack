using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    [SerializeField]
    private float _forceFactor = 1.0f;
    [SerializeField]
    private Rigidbody2D _body = default;

    [SerializeField]
    private float _minDistance = 0.01f;
    [SerializeField]
    private float _maxDistance = 1.0f;
    [SerializeField]
    private Camera _cam = default;

    private Vector2 _mousePos;

    // Update is called once per frame
    private void Update()
    {
        _mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
    }

    private void FixedUpdate()
    {
        Vector2 mouseDir = _mousePos - _body.position;
        float distance = Mathf.Clamp(mouseDir.magnitude, _minDistance, _maxDistance);
        Vector2 dir = mouseDir.normalized;
        float force = distance * distance * _forceFactor;
        _body.AddForce(dir * force, ForceMode2D.Force);
    }
}
