using System;
using System.Collections;
using UnityEngine;

public class Player2 : MonoBehaviour
{
    public static Player2 Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }
    private void OnValidate()
    {
        Instance = this;
    }

    // player 2 uses instant movement (infinity acceleration and deceleration)
    [SerializeField] private float maximumSpeed;

    [SerializeField] private float jumpPower;
    [SerializeField] private float coyoteTime;
    [SerializeField] private float repeatedJumpForbidTime;
    [SerializeField] private float jumpBufferTime;

    private bool jumpRequest = false;
    private float jumpRequestTime;
    private float lastJumpTime = 0f;
    private float lastGroundedTime;

    private bool movementDisabled = false;

    private const float DISTANCE_TO_GROUND = 0.1f;
    private const float NEGLIGIBLE_DIFFERENCE = 0.01f;
    private const int WORLD_PLATFORM_LAYER = 8;

    private BuildTrigger buildTrigger = null;
    private GravityFlipTrigger gravityFlipTrigger = null;
    public event EventHandler OnBuild;
    public event EventHandler OnGravityFlip;

    private void Start()
    {
        PlayerInput.Instance.OnPlayer2Jump += PlayerInput_OnPlayer2Jump;
        PlayerInput.Instance.OnPlayer2ReverseJump += PlayerInput_OnPlayer2ReverseJump;
        PlayerInput.Instance.OnPlayer2Create += PlayerInput_OnPlayer2Create;
        PlayerInput.Instance.OnPlayer2GravityFlip += PlayerInput_OnPlayer2GravityFlip;
    }
    private void PlayerInput_OnPlayer2Jump(object sender, EventArgs e)
    {
        if (GravityNormal())
        {
            jumpRequest = true;
            jumpRequestTime = Time.time;
        }
    }
    private void PlayerInput_OnPlayer2ReverseJump(object sender, EventArgs e)
    {
        if (!GravityNormal())
        {
            jumpRequest = true;
            jumpRequestTime = Time.time;
        }
    }
    private bool GravityNormal()
    {
        return GetComponent<Rigidbody2D>().gravityScale > 0f;
    }
    private float TimeSince(float timePoint)
    {
        return Time.time - timePoint;
    }
    private void PlayerInput_OnPlayer2Create(object sender, EventArgs e)
    {
        bool builtSomething = false;
        if (buildTrigger != null)
        {
            foreach (Removable removable in buildTrigger.removables)
            {
                if (!removable.built)
                {
                    removable.Build();
                    builtSomething = true;
                }
            }
        }
        if (builtSomething) OnBuild?.Invoke(this, EventArgs.Empty); //for sounds
    }
    private void PlayerInput_OnPlayer2GravityFlip(object sender, System.EventArgs e)
    {
        if (gravityFlipTrigger != null && !movementDisabled)
        {
            foreach (Rigidbody2D rigidbody2D in FindObjectsByType<Rigidbody2D>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            {
                rigidbody2D.gravityScale *= -1;
            }
            OnGravityFlip?.Invoke(this, EventArgs.Empty); //for visuals
            StartCoroutine(DisableInputUntilBothGrounded());
            MusicManager.Instance.GravityFlip(!GravityNormal());
        }
    }
    private IEnumerator DisableInputUntilBothGrounded()
    {
        Player1.Instance.DisableMovement();
        DisableMovement();

        while ((!IsGrounded()) || (!Player1.Instance.IsGrounded())) yield return null;

        Player1.Instance.EnableMovement();
        EnableMovement();
    }
    private void DisableMovement()
    {
        movementDisabled = true;
    }
    private void EnableMovement()
    {
        movementDisabled = false;
    }

    private void Update()
    {
        if (IsGrounded()) lastGroundedTime = Time.time;

        if (jumpRequest)
        {
            if (TimeSince(jumpRequestTime) > jumpBufferTime)
            {
                jumpRequest = false;
            }
            else if (TimeSince(lastGroundedTime) < coyoteTime && TimeSince(lastJumpTime) > repeatedJumpForbidTime)
            {
                // perform actual jump
                Vector3 velocity = GetComponent<Rigidbody2D>().velocity;
                GetComponent<Rigidbody2D>().velocity = new Vector3(velocity.x, GravityNormal() ? jumpPower : -jumpPower, velocity.z);
                lastJumpTime = Time.time;
                jumpRequest = false;
            }
        }

        if (movementDisabled)
        {
            // stop abruptly to prevent high jump with gravity flips
            GetComponent<Rigidbody2D>().velocity = new Vector2(0f, GetComponent<Rigidbody2D>().velocity.y);
        }
        else
        {
            // actual velocity set here, simply, instantly, without accelerating and decelerating
            GetComponent<Rigidbody2D>().velocity = new Vector2(
                maximumSpeed * PlayerInput.Instance.GetPlayer2MovementValue(),
                GetComponent<Rigidbody2D>().velocity.y);
        }
    }

    public bool IsGrounded()
    {
        CapsuleCollider2D cc = GetComponent<CapsuleCollider2D>();
        return Physics2D.CapsuleCast(transform.position, cc.size, cc.direction, 0f, GravityNormal() ? Vector2.down : Vector2.up, DISTANCE_TO_GROUND, 1 << WORLD_PLATFORM_LAYER);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<BuildTrigger>() != null)
        {
            buildTrigger = collision.gameObject.GetComponent<BuildTrigger>();
        }
        if (collision.gameObject.GetComponent<GravityFlipTrigger>() != null)
        {
            gravityFlipTrigger = collision.gameObject.GetComponent<GravityFlipTrigger>();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<BuildTrigger>() != null)
        {
            buildTrigger = null;
        }
        if (collision.gameObject.GetComponent<GravityFlipTrigger>() != null)
        {
            gravityFlipTrigger = null;
        }
    }
}
