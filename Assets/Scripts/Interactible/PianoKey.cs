using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Note
{
    C5,
    CSharp5,
    D5,
    DSharp5,
    E5
}

public class PianoKey : Interactible
{
    public Note note;
    public Animator animator;
    public ParticleSystem ps;

    public override void Interacted(Character origin)
    {
        base.Interacted(origin);
        AudioManager.Instance.PlaySFX("Piano" + note.ToString().Replace("Sharp", "#"));
        animator.SetBool("pressed", true);
        ps.Play();
    }

    public override void StopInteraction(Character character)
    {
        base.StopInteraction(character);
        animator.SetBool("pressed", false);
    }
}
