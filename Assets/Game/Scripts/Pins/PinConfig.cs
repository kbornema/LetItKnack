using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PinConfig : ScriptableObject
{
    public float Mass = 1.0f;
    public float PitchScale = 1.0f;

    public bool HasResetHead = false;
    public float SlipSpeed = 0.0f;

    public Sprite PinSprite = default;
    public Color PinColor = Color.white;
    public PhysicsMaterial2D PhysicsMat = default;
}
