using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class Character : NetworkBehaviour
{
    [Header("Components")]
    public Animator animator;
    public NetworkAnimator nAnimator;
    public CharacterColor color;
    public MessageBubble messageBubble;
    public TextMeshPro nameText;

    [Header("Settings")]
    public float rotationSpeed = 1f;
    public float speed;

    private Player player;

    public void Initialize(Player player)
    {
        this.player = player;
    }

    public void UpdateTextName(string text)
    {
        nameText.text = text;
    }

    private void Update()
    {
        if (!hasAuthority) return;

        Movement();
    }   

    public void InputHorizontal(float horiz)
    {
        horizAxis = horiz;
    }

    private float horizAxis;

    private void Movement()
    {
        float xVelocity = horizAxis * Time.deltaTime * speed;
        transform.position += new Vector3(xVelocity, 0f, 0f);

        float targetRot = horizAxis == 0 ? 180 : horizAxis * 90f;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(targetRot, Vector3.up), rotationSpeed * Time.deltaTime);

        animator.SetBool("isRunning", Mathf.Abs(horizAxis) >= 0.01f);
    }

    public void DisplayBubble(string text)
    {
        messageBubble.In(text);
    }
}
