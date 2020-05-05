using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class Character : NetworkBehaviour
{
    [Header("Components")]
    public CharacterSettings m;
    public Rigidbody body;
    public Animator animator;
    public NetworkAnimator nAnimator;
    public CharacterColor color;
    public MessageBubble messageBubble;
    public TextMeshPro nameText;
    public Transform aimTarget;

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

        HorizontalMovement();
        Sight();
    }

    private void FixedUpdate()
    {
        ApplyVelocity();
    }

    #region Input

    public void InputHorizontal(float horiz)
    {
        horizAxis = horiz;
    }
    
    #endregion

    #region Movement

    private Vector3 velocity;

    private float horizAxis;
    private void HorizontalMovement()
    {
        float xVelocity = horizAxis * m.maxRunSpeed;
        SetHorizontalVelocity(xVelocity);

        float targetRot = horizAxis == 0 ? 180 : horizAxis * 90f;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(targetRot, Vector3.up), m.rotationSpeed * Time.deltaTime);

        animator.SetBool("isRunning", Mathf.Abs(horizAxis) >= 0.01f);
    }

    private void SetHorizontalVelocity(float x)
    {
        velocity = new Vector3(x, velocity.y, 0f);
    }

    private void SetVerticalVelocity(float y)
    {
        velocity = new Vector3(velocity.x, y, 0f);
    }

    private void ApplyVelocity()
    {
        body.velocity = velocity;
    }

    #endregion

    #region Crouch

    private bool isCrouched = false;
    public void UpdateCrouch(bool crouched)
    {
        isCrouched = crouched;
        animator.SetBool("isCrouched", isCrouched);
    }

    #endregion

    #region Sight

    [Header("Sight")]
    public float sightSmoothness = 0.2f;
    public AnimationCurve sightCurveZ;
    private bool useSight;
    private Vector3 currentVelocitySight;
    
    public void InputSight(bool useSight)
    {
        this.useSight = useSight;
    }

    private void Sight()
    {
        float y = Utility.Interpolate(-3, 6f, 0, Screen.height, Input.mousePosition.y);
        float z = sightCurveZ.Evaluate(Utility.Interpolate(0, 1, 0, Screen.height, Input.mousePosition.y));
        float x = Utility.Interpolate(1, -1, 0, Screen.width, Input.mousePosition.x) * 3f;
        Vector3 target = new Vector3(x, y, z);

        if (!useSight) target = new Vector3(0, 2.5f, 1);
        aimTarget.transform.localPosition = Vector3.SmoothDamp(aimTarget.transform.localPosition, target, ref currentVelocitySight, sightSmoothness);

    }

    #endregion

    #region Visuals

    public void DisplayBubble(string text)
    {
        messageBubble.In(text);
    }
    
    #endregion
}
