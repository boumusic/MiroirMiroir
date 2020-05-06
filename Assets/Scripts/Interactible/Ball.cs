using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Ball : Interactible
{
    public float strength = 10f;
    public Rigidbody body;
    public NetworkAnimator nAnimator;
    public Animator animator;
    public ParticleSystem ps;

    public override void Interacted(Character character)
    {
        base.Interacted(character);
        Vector3 dir = character.Velocity.normalized + Vector3.up;
        if (CanBeInteractedWith)
        {
            body.AddForce(dir.normalized * strength, ForceMode.Impulse);
            Debug.Log("Bounce " + dir);
            nAnimator.SetTrigger("Bounce");
            ps.Play();
        }
    }
}
