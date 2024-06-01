using System;
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
    }

    private void PlayerInput_OnPlayer2Create(object sender, System.EventArgs e)
    {
        if (buildTrigger != null) foreach(Removable removable in buildTrigger.removables)
        {
            if (!removable.built)
            {
                removable.Build();
                OnBuild?.Invoke(this, EventArgs.Empty);
                Debug.Log("player 2 built a platform");
            }
        }
        Debug.Log("player 2 is done building");
    }
    
    private void PlayerInput_OnPlayer2GravityFlip(object sender, System.EventArgs e)
    {
        if (gravityFlipTrigger != null)
        {
            foreach (Rigidbody2D rigidbody2D in FindObjectsByType<Rigidbody2D>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            {
                rigidbody2D.gravityScale *= -1;
                OnGravityFlip?.Invoke(this, EventArgs.Empty);
                if (rigidbody2D.gravityScale < 0f)
                {
                    MusicManager.Instance.GravityFlipOn();
                }
                else
                {
                    MusicManager.Instance.GravityFlipOff();
                }
            }
        }
        //Debug.Log("player 2 gravity flip");
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

    private bool IsGrounded()
    {
        if (GetComponent<Rigidbody2D>().gravityScale > 0f) return Physics2D.Raycast(transform.position, Vector2.down, distanceToGround, 1 << WORLD_PLATFORM_LAYER);
        else return Physics2D.Raycast(transform.position, Vector2.up, distanceToGround, 1 << WORLD_PLATFORM_LAYER);
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
