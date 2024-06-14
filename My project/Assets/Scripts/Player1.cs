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

    [SerializeField] private float acceleration;
    [SerializeField] private float deceleration;
    [SerializeField] private float fastTurnTime; 
    [SerializeField] private float maximumSpeed;
    [SerializeField] private float burnSpeed;
    [SerializeField] private float speedCurveExponent = 3f;
    [SerializeField] private float slopeCompensation;

    [SerializeField] private float jumpPower;
    [SerializeField] private float coyoteTime;
    [SerializeField] private float repeatedJumpForbidTime;
    [SerializeField] private float jumpBufferTime;

    [SerializeField] private float burnTime;
    //[SerializeField] private float airBurnForbidTime = 0.5f;

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
    private float lastBurningTime = Mathf.NegativeInfinity;
    private readonly List<Removable> touching = new();
    
    private bool movementDisabled = false;

    private const float DISTANCE_TO_GROUND = 0.9f;
    private const float NEGLIGIBLE_DIFFERENCE = 0.01f;
    private const int WORLD_PLATFORM_LAYER = 8;

    // events for visuals
    public event EventHandler OnPlayer1Jump;
    public event EventHandler OnPlayer1BurnOn;
    public event EventHandler OnPlayer1BurnOff;

    // DO NOT modify Rigidbody2D's velocity.x directly! use Speed instead. modifying y is fine though
    private float _speed;
    private float Speed
    {
        get => _speed;
        set
        {
            _speed = value;

            // better curve than linear: faster acceleration at start, fine control near max
            float fractionOfMaxSpeed = Speed / maximumSpeed;
            float newFractionOfMaxSpeed = Mathf.Sign(fractionOfMaxSpeed) * (1 - Mathf.Pow(1 - Mathf.Abs(fractionOfMaxSpeed), speedCurveExponent));
            GetComponent<Rigidbody2D>().velocity = new Vector2(maximumSpeed * newFractionOfMaxSpeed, GetComponent<Rigidbody2D>().velocity.y);
        }
    }

    private void Start()
    {
        PlayerInput.Instance.OnPlayer1Jump += PlayerInput_OnPlayer1Jump;
        PlayerInput.Instance.OnPlayer1ReverseJump += PlayerInput_OnPlayer1ReverseJump;
    }
    private void PlayerInput_OnPlayer1Jump(object sender, EventArgs e)
    {
        if (GravityNormal())
        {
            jumpRequest = true;
            jumpRequestTime = Time.time;
        }
    }
    private void PlayerInput_OnPlayer1ReverseJump(object sender, EventArgs e)
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
                GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, GravityNormal() ? jumpPower : -jumpPower);
                lastJumpTime = Time.time;
                jumpRequest = false;
                OnPlayer1Jump?.Invoke(this, EventArgs.Empty); //for visuals
                BurnBelow();
            }
        }

        if (movementDisabled)
        {
            // stop abruptly to prevent high jump with gravity flips
            Speed = 0f;
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
                turningVelocity = Speed;
                momentumBuilt = -Speed;
            }

            if (fastTurning)
            {
                turningVelocity = Mathf.SmoothDamp(turningVelocity, momentumBuilt, ref fastTurningSmoothDampVelocity, fastTurnTime);
                Speed = turningVelocity;
            }

            // actual acceleration and deceleration
            float currentSpeed = Speed;
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
            Speed = currentSpeed;

            // slope handling
            float gravity = GetComponent<Rigidbody2D>().gravityScale * GetComponent<Rigidbody2D>().mass;
            Speed = Speed - GetSlope() * gravity * slopeCompensation;
            
            // are we burning
            HandleFire(Mathf.Abs(Speed) > burnSpeed);
        }
    }

    public bool IsGrounded()
    {
        return Physics2D.BoxCast(transform.position, GetComponent<BoxCollider2D>().size, 0f, GravityNormal() ? Vector2.down : Vector2.up, DISTANCE_TO_GROUND, 1 << WORLD_PLATFORM_LAYER);
    }
    public float GetSlope()
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, GetComponent<BoxCollider2D>().size, 0f, GravityNormal() ? Vector2.down : Vector2.up, DISTANCE_TO_GROUND, 1 << WORLD_PLATFORM_LAYER);
        if (hit) return hit.normal.x;
        else return 0f;
    }

    private bool isBurning = false;
    private void HandleFire(bool isOn) // triggered every frame in Update
    {
        if (isBurning != isOn)
        {
            isBurning = isOn;
            if (isBurning) OnPlayer1BurnOn?.Invoke(this, EventArgs.Empty);
            else OnPlayer1BurnOff?.Invoke(this, EventArgs.Empty);
        }

        if (isBurning) lastBurningTime = Time.time;

        if ((TimeSince(lastBurningTime) < burnTime)   )//   && (!IsGrounded() || (TimeSince(lastAirTime) > airBurnForbidTime)))
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
            touching.Add(removable);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Removable>(out var removable) && touching.Contains(removable))
        {
            touching.Remove(removable);
        }
    }

    private void BurnBelow()
    {
        // this is executed at jump, to make sure we can't continuously bounce off burnable ground when we're burning, even though colliders might not get in contact
        if (isBurning)
        {
            RaycastHit2D cast = Physics2D.BoxCast(transform.position, GetComponent<BoxCollider2D>().size, 0f, GravityNormal() ? Vector2.down : Vector2.up, Mathf.Infinity, 1 << WORLD_PLATFORM_LAYER);
            if (cast && cast.collider.gameObject.TryGetComponent(out Removable removable))
            {
                removable.Burn();
            }
        }
    }
}
