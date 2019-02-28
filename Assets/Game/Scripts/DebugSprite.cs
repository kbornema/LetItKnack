using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugSprite : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    private void Reset()
    {
        if (!_spriteRenderer)
            _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Awake()
    {
        Destroy(_spriteRenderer);
    }
}
