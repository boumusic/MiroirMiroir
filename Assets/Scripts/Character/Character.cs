using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;
using Mirror;
using TMPro;

public class Character : NetworkBehaviour
{
    [Header("Components")]
    public CharacterSettings m;
    public Rigidbody body;
    public CharacterAnimator animator;
    public CharacterColor color;
    public MessageBubble messageBubble;
    public TextMeshPro nameText;
    public Transform aimTarget;

    [Header("Debug")]
    public RaycastHitSerializer hitSerializer;

    [TextArea]
    public string debug;

    #region State
    private StateMachine<CharacterState> stateMachine;
    public CharacterState CurrentState => stateMachine != null ? stateMachine.State : CharacterState.Grounded;
    #endregion

    [Header("Settings")]
    public float rotationSpeed = 1f;
    public float speed;

    private Player player;

    public Vector3 Velocity => body.velocity;

    private void Awake()
    {
        stateMachine = StateMachine<CharacterState>.Initialize(this);
        stateMachine.ManualUpdate = true;
        stateMachine.ChangeState(CharacterState.Falling);
    }

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
        stateMachine.UpdateManually();
        HorizontalMovement();
        Sight();
        hitSerializer.UpdateHits(hitGrounds);
    }

    private void FixedUpdate()
    {
        ApplyVelocity();
    }

    private void OnDrawGizmos()
    {
        DrawBox();

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
    private float horizAccel = 0f;

    private float currentAcceleration => isGrounded ? m.groundedAccel : m.aerialAccel;
    private float currentDeceleration => isGrounded ? m.groundedDecel : m.aerialDecel;

    private void HorizontalMovement()
    {
        if (horizAxis == 0)
            horizAccel /= currentDeceleration;
        else
            horizAccel += horizAxis * Time.deltaTime * currentAcceleration;

        horizAccel = Mathf.Clamp(horizAccel, -1, 1);

        SetHorizontalVelocity(horizAccel * m.maxRunSpeed);

        float targetRot = horizAxis == 0 ? 180 : horizAxis * 90f;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(targetRot, Vector3.up), m.rotationSpeed * Time.deltaTime);

        animator.Run(Mathf.Abs(horizAxis) >= 0.01f);
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

    #region Grounded

    private bool isGrounded => CurrentState == CharacterState.Grounded;
    private void Grounded_Enter()
    {
        SetVerticalVelocity(0);
        animator.Land();
    }

    private void Grounded_Update()
    {
        SnapToGround();
        if (!CastGround())
        {
            stateMachine.ChangeState(CharacterState.Falling);
        }
    }

    #endregion

    #region Jumping

    private float jumpProgress = 0f;
    private bool CanJump => CurrentState == CharacterState.Grounded;
    public void StartJump()
    {
        if (CanJump)
        {
            stateMachine.ChangeState(CharacterState.Jumping);

        }
    }

    private void Jumping_Enter()
    {
        jumpProgress = 0f;
        animator.Jump(false);
    }

    private void Jumping_Update()
    {
        jumpProgress += Time.deltaTime / m.jumpDuration;
        SetVerticalVelocity(m.jumpStrength * m.jumpCurve.Evaluate(jumpProgress));
        if(jumpProgress > 0.3f && downButton)
        {
            stateMachine.ChangeState(CharacterState.Falling);
        }

        if (jumpProgress > 1f)
        {
            stateMachine.ChangeState(CharacterState.Falling);
        }
    }

    #endregion

    #region Falling

    private float initialFallVelocity;
    private float fallProgress;
    private void Falling_Enter()
    {
        fallProgress = 0f;
        initialFallVelocity = body.velocity.y;
    }

    private void Falling_Update()
    {
        fallProgress += Time.deltaTime / m.timeToReachMaxFall;
        float fallStrength = m.maxFallStrength;
        if (downButton) fallStrength *= m.fallSpeedDownMultiplier;
        SetVerticalVelocity(fallStrength * m.fallCurve.Evaluate(fallProgress));
        if (fallProgress > 1f) fallProgress = 1f;

        if (CastGround())
        {
            SnapToGround();
            stateMachine.ChangeState(CharacterState.Grounded);
        }
    }

    private void SnapToGround()
    {
        if (hitGrounds[0].point != Vector3.zero)
        {
            body.position = new Vector3(body.position.x, hitGrounds[0].point.y, body.position.z);
            Debug.Log("Snap Ground");
        }
    }
    #endregion

    #region Cast

    private RaycastHit[] hitGrounds;
    public Vector3 FeetOrigin => transform.position + Vector3.up * m.castGroundOrigin;
    public Vector3 CastBox => new Vector3(m.castBoxWidth, m.castBoxWidth, m.castBoxWidth);
    public bool CastGround()
    {
        hitGrounds = Physics.BoxCastAll(FeetOrigin, CastBox, -Vector3.up, Quaternion.identity, m.groundRaycastDown, m.groundMask, QueryTriggerInteraction.Ignore);

        return hitGrounds.Length > 0;
    }

    private void DrawBox()
    {
        BoxCastDrawer.DrawBoxCastBox(FeetOrigin, CastBox, Quaternion.identity, -Vector3.up, m.groundRaycastDown, Color.red);
    }

    #endregion

    #region Down

    private bool isCrouched = false;
    private bool downButton = false;
    public void UpdateDown(bool downButton)
    {
        this.downButton = downButton;
        isCrouched = downButton;

        if (CurrentState != CharacterState.Grounded) isCrouched = false;
        animator.Bool("isCrouched", isCrouched);
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

public enum CharacterState
{
    Grounded,
    Jumping,
    Falling
}
