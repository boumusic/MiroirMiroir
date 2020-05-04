using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class MessageBubble : NetworkBehaviour
{
    public Animator animator;
    public TextMeshPro text;
    
    public float lifeTime = 2f;

    public void In(string message)
    {
        animator.SetBool("isIn", true);
        StartCoroutine(LifeTime());
        CmdChangeText(message);
        UpdateText(message);
    }
    
    private void UpdateText(string message)
    {
        text.text = message;
    }

    [Command]
    public void CmdChangeText(string message)
    {
        UpdateText(message);
        RpcChangeText(message);
    }

    [ClientRpc]
    public void RpcChangeText(string message)
    {
        UpdateText(message);
    }

    public IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(lifeTime);
        animator.SetBool("isIn", false);
    }
}
