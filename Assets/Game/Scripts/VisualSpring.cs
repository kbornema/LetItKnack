using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualSpring : MonoBehaviour
{
    [SerializeField]
    private GameObject _partPrefab = default;
    [SerializeField]
    private Transform _partRoot = default;
    [SerializeField]
    private int _numParts = 6;
    [SerializeField]
    private float _minSpacing = 0.0f;
    [SerializeField]
    private float _maxSpacing = 2.0f;
    [SerializeField]
    private Transform _upperAnchor = default;
    [SerializeField]
    private Transform _lowerAnchor = default;
    [SerializeField]
    private SpriteRenderer _innerPart = default;
    [SerializeField]
    private float _innerPartScaleFactor = 0.5f;

    private List<GameObject> _partInstances;

    public Transform ObjToWatch = default;

    private void Awake()
    {
        _partInstances = new List<GameObject>();

        for (int i = 0; i < _numParts; i++)
        {
            var curInstance = Instantiate(_partPrefab, _partRoot);
            curInstance.transform.localPosition = Vector3.zero;
            _partInstances.Add(curInstance);
        }
    }

    private void LateUpdate()
    {
        if (ObjToWatch)
        {
            float upperY = _upperAnchor.position.y;
            float lowerY = _lowerAnchor.position.y;
            float curY = ObjToWatch.position.y;

            float t = Mathf.InverseLerp(upperY, lowerY, curY);

            float curSpacing = Mathf.Lerp(_minSpacing, _maxSpacing, t);

            float totalSpace = 0.0f;

            for (int i = 0; i < _partInstances.Count; i++)
            {
                float tmpSpacing = i * curSpacing;
                totalSpace += Mathf.Abs(tmpSpacing);
                var curPos = _partInstances[i].transform.position;
                curPos.y = upperY + tmpSpacing;
                _partInstances[i].transform.position = curPos;
            }

            _innerPart.size = new Vector2(0.5f, totalSpace * _innerPartScaleFactor);
        }
    }
}
