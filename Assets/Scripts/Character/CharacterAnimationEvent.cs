using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationEvent : MonoBehaviour
{
    [Header("Particles")]
    public ParticleSystem footsteps;
    public ParticleSystem jump;
    public ParticleSystem land;

    public void Footstep()
    {
        AudioManager.Instance.PlaySFX("Footstep01");
        footsteps.Play();
    }

    public void Land()
    {
        AudioManager.Instance.PlaySFX("Land01");
        land.Play();
    }

    public void Jump()
    {
        AudioManager.Instance.PlaySFX("Jump01");
        jump.Play();
    }
}
