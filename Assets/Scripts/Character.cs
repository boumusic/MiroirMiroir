using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class Character : NetworkBehaviour

{
    [Header("Components")]
    public Animator animator;

    [Header("Settings")]
    public float rotationSpeed = 1f;
    public float speed;

    private void Update()
    {
        if (!hasAuthority) return;

        float horiz = Input.GetAxisRaw("Horizontal");
        float xVelocity = horiz * Time.deltaTime * speed;
        transform.position += new Vector3(xVelocity, 0f, 0f);

        float targetRot = horiz == 0 ? 180 : horiz * 90f;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(targetRot, Vector3.up), rotationSpeed * Time.deltaTime);

        animator.SetBool("isRunning", Mathf.Abs(xVelocity) >= 0.01f);
    }
}
