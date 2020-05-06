using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Interactible : NetworkBehaviour
{
    public float interactionCooldown = 0.2f;

    private Character interactor;
    public bool CanBeInteractedWith { get; private set; }

    private void Awake()
    {
        CanBeInteractedWith = true;
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("Stay");
        Character character;
        if (other.TryGetComponent(out character))
        {
            if (interactor == null)
            {
                interactor = character;
                Interacted(character);
                CanBeInteractedWith = false;
                StartCoroutine(Cooldown());
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        Character character;
        if (other.TryGetComponent(out character))
        {
            if (character == interactor) interactor = null;
            StopInteraction(character);
        }
    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(interactionCooldown);
        CanBeInteractedWith = true;
    }

    public virtual void Interacted(Character character)
    {

    }

    public virtual void StopInteraction(Character character)
    {

    }
}
