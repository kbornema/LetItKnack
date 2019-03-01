using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    [SerializeField]
    private float _forceFactor = 1.0f;
    [SerializeField]
    private Picklock _target = default;

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
        var gameManager = GameManager.Instance;

        if (!gameManager.GameIsOver)
            _mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);

        if (!gameManager.IsInWinRoutine)
        {
            if (GameManager.InDebugMode)
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                    gameManager.StepLevel(-1);

                else if (Input.GetKeyDown(KeyCode.RightArrow))
                    gameManager.StepLevel(1);
            }

            if (!gameManager.GameIsOver)
            {
                if (Input.GetMouseButtonDown(0))
                    gameManager.TryLockPins();

                else if (Input.GetMouseButtonDown(1))
                    gameManager.UnlockAllPins();
            }
        }
    }

    private void FixedUpdate()
    {

        Vector2 mouseDir = _mousePos - _target.GetPosition();
        float distance = Mathf.Clamp(mouseDir.magnitude, _minDistance, _maxDistance);
        Vector2 dir = mouseDir.normalized;
        float force = distance * distance * _forceFactor;
        _target.AddForce(dir, force);

    }
}
