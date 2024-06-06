using System;
using System.Collections;
using UnityEngine;

public class Player2 : MonoBehaviour
{
    public static Player2 Instance { get; private set; }

    [SerializeField] private float jumpPower = 1f;
    [SerializeField] private float horizontalForceScale = 1f;
    [SerializeField] private float fastTurnTime = 0.1f;
    [SerializeField] private float distanceToGround = 0.1f;
    [SerializeField] private float rebounceTime = .2f;
    [SerializeField] private float coyoteTime = .2f;
    private bool fastTurning = false;
    private float momentumBuilt = 0f;
    private float turningVelocity;
    private float smoothDampVelocity;
    private const float NEGLIGIBLE_DIFFERENCE = 0.01f;
    private const int WORLD_PLATFORM_LAYER = 8;
    private bool jumpRequest = false;
    private float jumpRequestTime;
    private float lastJumpTime = 0f;
    private float lastGroundedTime;
    private BuildTrigger buildTrigger = null;
    private GravityFlipTrigger gravityFlipTrigger = null;
    public event EventHandler OnBuild;
    public event EventHandler OnGravityFlip;
    private float gravityFlipTime = Mathf.NegativeInfinity;
    private float capsuleWidth;
    [SerializeField] private float gravityFlipCooldown = 1f;
    private bool movementDisabled = false;

    private void Awake()
    {
        Instance = this;
    }
    private void OnValidate()
    {
        Instance = this;
    }

    private void Start()
    {
        PlayerInput.Instance.OnPlayer2Jump += PlayerInput_OnPlayer2Jump;
        PlayerInput.Instance.OnPlayer2ReverseJump += PlayerInput_OnPlayer2ReverseJump;
        PlayerInput.Instance.OnPlayer2Create += PlayerInput_OnPlayer2Create;
        PlayerInput.Instance.OnPlayer2GravityFlip += PlayerInput_OnPlayer2GravityFlip;

        CapsuleCollider2D capsuleCollider = GetComponent<CapsuleCollider2D>();
        capsuleWidth = capsuleCollider.size.x;
    }

    private void PlayerInput_OnPlayer2Create(object sender, System.EventArgs e)
    {
        bool builtSomething = false;
        if (buildTrigger != null) foreach(Removable removable in buildTrigger.removables)
        {
            if (!removable.built)
            {
                removable.Build();
                builtSomething = true;
            }
        }
        if (builtSomething) OnBuild?.Invoke(this, EventArgs.Empty);
    }
    
    private void PlayerInput_OnPlayer2GravityFlip(object sender, System.EventArgs e)
    {
        if (gravityFlipTrigger != null && Time.time - gravityFlipTime > gravityFlipCooldown)
        {
            foreach (Rigidbody2D rigidbody2D in FindObjectsByType<Rigidbody2D>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            {
                rigidbody2D.gravityScale *= -1;
            }
            OnGravityFlip?.Invoke(this, EventArgs.Empty);
            gravityFlipTime = Time.time;
            StartCoroutine(DisableInputWhileCooldown());
            if (GetComponent<Rigidbody2D>().gravityScale < 0f)
            {
                MusicManager.Instance.GravityFlipOn();
            }
            else
            {
                MusicManager.Instance.GravityFlipOff();
            }
        }
        //Debug.Log("player 2 gravity flip");
    }

    private IEnumerator DisableInputWhileCooldown()
    {
        DisableMovement();
        Player1.Instance.DisableMovement();
        while (true)
        {
            if (IsGrounded() && Player1.Instance.IsGrounded()) break;
            yield return null;
        }
        EnableMovement();
        Player1.Instance.EnableMovement();
    }

    private void PlayerInput_OnPlayer2Jump(object sender, System.EventArgs e)
    {
        if (GetComponent<Rigidbody2D>().gravityScale > 0f)
        {
            jumpRequest = true;
            jumpRequestTime = Time.time;
        }
    }
    private void PlayerInput_OnPlayer2ReverseJump(object sender, System.EventArgs e)
    {
        if (GetComponent<Rigidbody2D>().gravityScale < 0f)
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

    private void Update()
    {
        if (IsGrounded()) lastGroundedTime = Time.time;

        if (jumpRequest)
        {
            //Debug.Log("lastGroundedTime = " + lastGroundedTime + ", current time = " + Time.time + ", coyoteTime = " + coyoteTime);
            if (Time.time - lastGroundedTime < coyoteTime && Time.time - lastJumpTime > coyoteTime)
            {
                // perform actual jump
                Vector3 velocity = GetComponent<Rigidbody2D>().velocity;
                GetComponent<Rigidbody2D>().velocity = new Vector3(velocity.x, jumpPower * Mathf.Sign(GetComponent<Rigidbody2D>().gravityScale), velocity.z);
                lastJumpTime = Time.time;
                jumpRequest = false;
            }
            else if (Time.time - jumpRequestTime > rebounceTime)
            {
                jumpRequest = false;
            }
        }

        if (!movementDisabled)
        {
            float horizontalForce = horizontalForceScale * PlayerInput.Instance.GetPlayer2MovementValue();

            if (fastTurning && horizontalForce == 0f)
            {
                fastTurning = false;
                //Debug.Log("no input, disabling fastTurning");
            }
            else if (fastTurning && Mathf.Abs(turningVelocity - momentumBuilt) < NEGLIGIBLE_DIFFERENCE)
            {
                fastTurning = false;
                //Debug.Log("built back momentum, disabling fastTurning");
            }
            else if (!fastTurning && horizontalForce * GetComponent<Rigidbody2D>().velocity.x < 0f)
            {
                fastTurning = true;
                turningVelocity = GetComponent<Rigidbody2D>().velocity.x;
                momentumBuilt = -GetComponent<Rigidbody2D>().velocity.x;
                //Debug.Log("movement and input inverse, enabling fastTurning");
            }

            if (fastTurning)
            {
                turningVelocity = Mathf.SmoothDamp(turningVelocity, momentumBuilt, ref smoothDampVelocity, fastTurnTime);
                GetComponent<Rigidbody2D>().velocity = new Vector2(turningVelocity, GetComponent<Rigidbody2D>().velocity.y);
            }
            else
            {
                GetComponent<ConstantForce2D>().force = new Vector2(horizontalForce, 0f);
            }
            //Debug.Log("velocity = " + GetComponent<Rigidbody2D>().velocity.x + ", fastTurning = " + fastTurning + ", IsGrounded = " + IsGrounded());
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0f, GetComponent<Rigidbody2D>().velocity.y);
            GetComponent<ConstantForce2D>().force = new Vector2(0f, 0f);
        }
    }

    private bool IsGrounded()
    {

        int worldPlatformLayer = 1 << WORLD_PLATFORM_LAYER;
        Vector2 leftRayOrigin = new Vector2(transform.position.x - capsuleWidth / 2, transform.position.y);
        Vector2 rightRayOrigin = new Vector2(transform.position.x + capsuleWidth / 2, transform.position.y);
        Vector2 centerRayOrigin = new Vector2(transform.position.x, transform.position.y);

        bool normalGravity = GetComponent<Rigidbody2D>().gravityScale > 0f;

        bool leftRayHit = Physics2D.Raycast(leftRayOrigin, normalGravity ? Vector2.down : Vector2.up, distanceToGround, worldPlatformLayer);
        bool rightRayHit = Physics2D.Raycast(rightRayOrigin, normalGravity ? Vector2.down : Vector2.up, distanceToGround, worldPlatformLayer);
        bool centerRayHit = Physics2D.Raycast(centerRayOrigin, normalGravity ? Vector2.down : Vector2.up, distanceToGround, worldPlatformLayer);

        return leftRayHit || rightRayHit || centerRayHit;
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
