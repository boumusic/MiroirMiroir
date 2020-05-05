using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationEvent : MonoBehaviour
{
    public void Footstep()
    {
        AudioManager.Instance.PlaySFX("Footstep01");
    }
}
