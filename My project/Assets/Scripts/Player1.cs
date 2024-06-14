using System;
using UnityEngine;
using System.Collections.Generic;

public class Player1 : MonoBehaviour
{
    public static Player1 Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }
    private void OnValidate()
    {
        Instance = this;
    }

    [SerializeField] private float acceleration = 1f;
    [SerializeField] private float deceleration = 1f;
    [SerializeField] private float maximumSpeed = 5f;
    [SerializeField] private float burnSpeed = 12f;

    [SerializeField] private float jumpPower = 1f;
    [SerializeField] private float fastTurnTime = 0.1f;

    [SerializeField] private float coyoteTime = .05f;
    [SerializeField] private float repeatedJumpForbidTime = .05f;
    [SerializeField] private float jumpBufferTime = .05f;
    [SerializeField] private float burnTime = .05f;
    [SerializeField] private float airBurnForbidTime = .5f;

    private float targetSpeed;
    private bool fastTurning = false;
    private float momentumBuilt = 0f;
    private float turningVelocity;
    private float fastTurningSmoothDampVelocity;

    private bool jumpRequest = false;
    private float jumpRequestTime;
    private float lastJumpTime = 0f;
    private float lastGroundedTime;

    private float lastAirTime = 0f;
    private float lastFireOnTime = Mathf.NegativeInfinity;
    private readonly List<Removable> touching = new();
    
    private bool movementDisabled = false;

    private const float DISTANCE_TO_GROUND = 0.9f;
    private const float NEGLIGIBLE_DIFFERENCE = 0.01f;
    private const int WORLD_PLATFORM_LAYER = 8;

    // events for visuals
    public event EventHandler OnPlayer1Jump;
    public event EventHandler OnPlayer1BurnOn;
    public event EventHandler OnPlayer1BurnOff;

    private void Start()
    {
        PlayerInput.Instance.OnPlayer1Jump += PlayerInput_OnPlayer1Jump;
        PlayerInput.Instance.OnPlayer1ReverseJump += PlayerInput_OnPlayer1ReverseJump;
    }
    private void PlayerInput_OnPlayer1Jump(object sender, System.EventArgs e)
    {
        if (GravityNormal())
        {
            jumpRequest = true;
            jumpRequestTime = Time.time;
        }
    }
    private void PlayerInput_OnPlayer1ReverseJump(object sender, System.EventArgs e)
    {
        if (!GravityNormal())
        {
            jumpRequest = true;
            jumpRequestTime = Time.time;
        }
    }

    public void DisableMovement()
    {
        movementDisabled = true;
    }
    public void EnableMovement()
    {
        movementDisabled = false;
    }

    private bool GravityNormal()
    {
        return GetComponent<Rigidbody2D>().gravityScale > 0f;
    }
    private float TimeSince(float timePoint)
    {
        return Time.time - timePoint;
    }

    private void Update()
    {
        if (IsGrounded()) lastGroundedTime = Time.time;
        else lastAirTime = Time.time;

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
                OnPlayer1Jump?.Invoke(this, EventArgs.Empty); //for visuals
            }
        }

        if (movementDisabled)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0f, GetComponent<Rigidbody2D>().velocity.y);
        }
        else
        {
            targetSpeed = maximumSpeed * PlayerInput.Instance.GetPlayer1MovementValue();

            if (fastTurning && targetSpeed == 0f)
            {
                fastTurning = false; //cancelled fast turning
            }
            else if (fastTurning && Mathf.Abs(turningVelocity - momentumBuilt) < NEGLIGIBLE_DIFFERENCE)
            {
                fastTurning = false; //completed fast turning
            }
            else if (!fastTurning && targetSpeed * GetComponent<Rigidbody2D>().velocity.x < 0f)
            {
                fastTurning = true; //start fast turning
                turningVelocity = GetComponent<Rigidbody2D>().velocity.x;
                momentumBuilt = -GetComponent<Rigidbody2D>().velocity.x;
            }

            if (fastTurning)
            {
                turningVelocity = Mathf.SmoothDamp(turningVelocity, momentumBuilt, ref fastTurningSmoothDampVelocity, fastTurnTime);
                GetComponent<Rigidbody2D>().velocity = new Vector2(turningVelocity, GetComponent<Rigidbody2D>().velocity.y);
            }

            // actual acceleration and deceleration
            float currentSpeed = GetComponent<Rigidbody2D>().velocity.x;
            if (currentSpeed < targetSpeed)
            {
                float maxAcceleration = acceleration * Time.deltaTime;
                currentSpeed = Mathf.Min(targetSpeed, currentSpeed + maxAcceleration);
            }
            else if (currentSpeed > targetSpeed)
            {
                float maxDeceleration = deceleration * Time.deltaTime;
                currentSpeed = Mathf.Max(targetSpeed, currentSpeed - maxDeceleration);
            }
            GetComponent<Rigidbody2D>().velocity = new Vector2(currentSpeed, GetComponent<Rigidbody2D>().velocity.y);

            if (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > burnSpeed) FireOn();
            else FireOff();
        }
    }

    public bool IsGrounded()
    {
        int worldPlatformLayer = 1 << WORLD_PLATFORM_LAYER;
        Vector2 leftRayOrigin = new(transform.position.x - GetComponent<CapsuleCollider2D>().size.x / 2, transform.position.y);
        Vector2 rightRayOrigin = new(transform.position.x + GetComponent<CapsuleCollider2D>().size.x / 2, transform.position.y);
        Vector2 centerRayOrigin = new(transform.position.x, transform.position.y);

        bool normalGravity = GetComponent<Rigidbody2D>().gravityScale > 0f;

        bool leftRayHit = Physics2D.Raycast(leftRayOrigin, normalGravity ? Vector2.down : Vector2.up, DISTANCE_TO_GROUND, worldPlatformLayer);
        bool rightRayHit = Physics2D.Raycast(rightRayOrigin, normalGravity ? Vector2.down : Vector2.up, DISTANCE_TO_GROUND, worldPlatformLayer);
        bool centerRayHit = Physics2D.Raycast(centerRayOrigin, normalGravity ? Vector2.down : Vector2.up, DISTANCE_TO_GROUND, worldPlatformLayer);

        return leftRayHit || rightRayHit || centerRayHit;
    }

    private void FireOff()
    {
        OnPlayer1BurnOff?.Invoke(this, EventArgs.Empty);
    }
    private void FireOn()
    {
        OnPlayer1BurnOn?.Invoke(this, EventArgs.Empty);
        lastFireOnTime = Time.time;

        if (!IsGrounded() || (IsGrounded() && (TimeSince(lastAirTime) > airBurnForbidTime))) 
        { 
            while (touching.Count > 0) //foreach doesn't work for removing list elements
            {
                touching[0].Burn();
                touching.RemoveAt(0);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Removable>(out var removable))
        {
            if (TimeSince(lastFireOnTime) < burnTime && (!IsGrounded() || TimeSince(lastAirTime) > airBurnForbidTime)) removable.Burn();
            else touching.Add(removable);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Removable>(out var removable) && touching.Contains(removable))
        {
            touching.Remove(removable);
        }
    }
}
