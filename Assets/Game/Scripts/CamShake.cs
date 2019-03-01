using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamShake : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve _xCurve = default;
    [SerializeField]
    private float _time = 1.0f;
    [SerializeField]
    private float _curveScale = 1.0f;

    private float _curTime = -1.0f;

    private Vector3 _direction;
    
    
    public void Play(Vector3 dir)
    {
        _direction = dir;
        _curTime = _time;
    }

    private void LateUpdate()
    {
        if(_curTime > 0.0f)
        {
            _curTime -= Time.deltaTime;

            float t = Mathf.Clamp01(1.0f - _curTime / _time);

            var pos = transform.localPosition;
            var curDir = _direction * _xCurve.Evaluate(t) * _curveScale;
            pos += curDir;
            transform.localPosition = pos;
        }
    }
}
