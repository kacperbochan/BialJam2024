using UnityEngine;

public class Player1 : MonoBehaviour
{
    public static Player1 Instance { get; private set; }

    [SerializeField] private float jumpPower = 1f;
    [SerializeField] private float horizontalForceScale = 1f;
    [SerializeField] private float fastTurnTime = 0.1f;
    [SerializeField] private float distanceToGround = 0.1f;
    [SerializeField] private float mediumSpeed = 6f;
    [SerializeField] private float highSpeed = 12f;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color lowSpeedColor = Color.white;
    [SerializeField] private Color mediumSpeedColor = new(1f, 1f, .5f);
    [SerializeField] private Color highSpeedColor = new(1f, .8f, 0f);
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
    private bool canBurn = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        PlayerInput.Instance.OnPlayer1Jump += PlayerInput_OnPlayer1Jump;
    }

    private void PlayerInput_OnPlayer1Jump(object sender, System.EventArgs e)
    {
        jumpRequest = true;
        jumpRequestTime = Time.time;
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
                GetComponent<Rigidbody2D>().velocity = new Vector3(velocity.x, jumpPower, velocity.z);
                lastJumpTime = Time.time;
                jumpRequest = false;
            }
            else if (Time.time - jumpRequestTime > rebounceTime)
            {
                jumpRequest = false;
            }
        }

        float horizontalForce = horizontalForceScale * PlayerInput.Instance.GetPlayer1MovementValue();

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
            if (Mathf.Abs(momentumBuilt) > highSpeed) FireHigh();
            else if (Mathf.Abs(momentumBuilt) > mediumSpeed) FireMedium();
            else FireLow();
        }
        else
        {
            GetComponent<ConstantForce2D>().force = new Vector2(horizontalForce, 0f);
            if (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > highSpeed) FireHigh();
            else if (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > mediumSpeed) FireMedium();
            else FireLow();
        }
        //Debug.Log("velocity = " + GetComponent<Rigidbody2D>().velocity.x + ", fastTurning = " + fastTurning + ", IsGrounded = " + IsGrounded());
    }

    private bool IsGrounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, distanceToGround, 1 << WORLD_PLATFORM_LAYER);
    }

    private void FireLow()
    {
        spriteRenderer.color = lowSpeedColor;
        canBurn = false;
    }
    private void FireMedium()
    {
        spriteRenderer.color = mediumSpeedColor;
        canBurn = false;
    }
    private void FireHigh()
    {
        spriteRenderer.color = highSpeedColor;
        canBurn = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (canBurn && collision.gameObject.GetComponent)
    }
}
