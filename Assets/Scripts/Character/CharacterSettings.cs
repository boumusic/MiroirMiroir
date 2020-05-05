using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CharacterSettings : ScriptableObject
{
    [Header("Run")]
    public float maxRunSpeed = 20f;
    public float rotationSpeed = 20f;
}
