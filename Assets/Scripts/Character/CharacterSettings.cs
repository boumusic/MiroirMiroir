using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CharacterSettings : ScriptableObject
{
    [Header("Run")]
    public float maxRunSpeed = 20f;
    public float rotationSpeed = 20f;
    public float groundedAccel = 5;
    public float groundedDecel = 5;

    [Header("Jump")]
    public float jumpStrength = 20f;
    public AnimationCurve jumpCurve;
    public float jumpDuration = 0.7f;
    public float aerialAccel = 2f;
    public float aerialDecel = 2f;

    [Header("Falling")]
    public float maxFallStrength = 20f;
    public float timeToReachMaxFall = 0.7f;
    public AnimationCurve fallCurve;
    public float fallSpeedDownMultiplier = 2f;

    [Header("Cast")]
    public float castGroundOrigin = 0.5f;
    public float groundRaycastDown = 0.5f;
    public float castBoxWidth = 1;
    public LayerMask groundMask;
}
