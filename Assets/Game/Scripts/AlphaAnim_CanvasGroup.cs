using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaAnim_CanvasGroup : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve _curve = default;
    [SerializeField]
    private CanvasGroup _group = default;
    [SerializeField]
    private float _alphaScale = 1.0f;
    [SerializeField]
    private float _minAlpha = 0.0f;
    [SerializeField]
    private float _maxTime = 1.0f;
    private float _curTime = 0.0f;

    private void Update()
    {
        _curTime += Time.deltaTime;

        float t = _curTime / _maxTime;
        _group.alpha = Mathf.Clamp(_curve.Evaluate(t) * _alphaScale, _minAlpha , 1.0f);
    }
}
