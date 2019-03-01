using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_LevelInfo : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI _text = default;

    private void Awake()
    {
        GameManager.Instance.OnLevelCompletedEvent.AddListener(OnLevelCompleted);
        OnLevelCompleted(GameManager.Instance);
    }

    private void OnLevelCompleted(GameManager arg0)
    {
        _text.text = "Level " + (arg0.CurrentLevel + 1) + "/" + arg0.NumLevels;
    }
}
