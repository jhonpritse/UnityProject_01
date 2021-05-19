
using System;
using Saving_Game_Data;
using UnityEngine;


public class MovementPlayer : MonoBehaviour
{
    #region Variables
    
    private Rigidbody2D rb;
    [Range(1, 15)]
    [SerializeField] private float speed;
    [Range(1, 15)]
    [SerializeField] private float flySpeed;

    [SerializeField]
    [Range(1, 25)]
    private float jumpForce;
    private float gravity;
    
    private bool isGrounded;
    // private bool isJumping;

    [SerializeField]
    private Transform feetPos;
    // public Transform headPos;
    [SerializeField]
    private Transform frontPos;
    
    [SerializeField]
    private float checkFeetRadius;   
    // [SerializeField]
    // private float checkHeadRadius;
    [SerializeField]
    private float checkFrontRadius;   
    [SerializeField]
    private LayerMask groundLayer;

    [SerializeField]
    private int doubleJumpAmount;
    private bool canDoubleJump;
    private int doubleJumpStart;


    private StateTrackerPlayer stateTrackerPlayer;

    private float horizontal;
    private float vertical;

    private FloatingJoystick joystick;
    private TouchInputs touchInputs;

    private bool facingRight;
    private bool isTouchingFront;
    private bool isWallSliding;
    private bool isWallJumping;

    [SerializeField]
    [Range(0, 5)]
    private float wallSlidingSpeed;
    [SerializeField]
    [Range(1, 25)]
    private float xWallForce;
    [SerializeField]
    [Range(1, 25)]
    private float yWallForce;
    [SerializeField]
    private float wallJumpTime;

    [SerializeField]
    [Range(500, 1500)]
    private float dashForce;

    private bool canDash; public bool CanDash
    {
        get => canDash;
        set => canDash = value;
    }
    [SerializeField]
    [Range(0,  60)]
    private float dashExpireTime;
    private float dashTimeStart;
    private int dashAmount; public int DashAmount
    {
        get => dashAmount;
        set => dashAmount = value;
    }
    
    [SerializeField]
    private int dashMaxAmount; public int DashMaxAmount
    {
        get => dashMaxAmount;
        set => dashMaxAmount = value;
    }

    private const string Key = "MovementPlayerFirstAwake";
    #endregion

    // void  OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireSphere(headPos.position, checkHeadRadius);
    //     Gizmos.color = Color.green;
    //     Gizmos.DrawWireSphere(feetPos.position, checkFeetRadius);
    //     Gizmos.color = Color.magenta;
    //     Gizmos.DrawWireSphere(frontPos.position, checkFrontRadius);
    // }
    private void OnEnable()
    {
        if (PlayerPrefs.HasKey(Key))
        {
            LoadMovementPlayerData();
        }
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt(Key , 1);
        SaveMovementPlayerData();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        touchInputs =GameObject.FindWithTag("Player").GetComponent<TouchInputs>();
        stateTrackerPlayer = GameObject.FindWithTag("Player").GetComponent<StateTrackerPlayer>();
        joystick = GameObject.FindWithTag("GameController").GetComponent<FloatingJoystick>();
        gravity = rb.gravityScale;
        doubleJumpStart = doubleJumpAmount;
        dashTimeStart = dashExpireTime;
    }

    private void Update()
    {
        TouchInput();
    }

    void FixedUpdate() 
    {
        Move();
        WallSliding();
        WallJumping();
    }

    // void JoystickLimiter()
    // {
    //     if (stateTrackerPlayer.CanFlyMode )
    //     {
    //         joystick.AxisOptions = AxisOptions.Both;
    //        
    //     }
    //     else
    //     {
    //         joystick.AxisOptions = AxisOptions.Horizontal; 
    //     }
    // }

    void TouchInput()
    {
        // JoystickLimiter();
        FaceDirection();
        WallTouching();
        DoubleJumpSetter();
        DashSetter();
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkFeetRadius, groundLayer);
        if (Input.touchCount >0) touchInputs.TouchInputControl();
    }

    void DashSetter()
    {
        if (doubleJumpAmount == 0) canDash = true;
        else canDash = false;

        if (dashAmount > 0)
        {
            dashExpireTime -= Time.deltaTime;
            if (dashExpireTime  <= 0)
            {
                dashAmount--;
                dashExpireTime = dashTimeStart;
            }
        }
        
    }
    void DoubleJumpSetter()
    {
        if (isGrounded || isWallSliding)
        {
            doubleJumpAmount = doubleJumpStart;
        }
        if (isGrounded || !isGrounded&&!isTouchingFront)
        {
            canDoubleJump = true;
        }else if (!isGrounded && isTouchingFront)
        {
            canDoubleJump = false;
        }
    }
    
    void WallTouching()
    {
        isTouchingFront = Physics2D.OverlapCircle(frontPos.position, checkFrontRadius, groundLayer);

        if (isTouchingFront && isGrounded == false && horizontal != 0 &&rb.velocity.y <0) isWallSliding = true;
        else isWallSliding = false;
      
    }
    void WallSliding()
    {
        if (isWallSliding)
        {
            var velocity = rb.velocity;
            rb.velocity = new Vector2(velocity.x, Mathf.Clamp(velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
    }
    void WallJumping()
    {
        if (isWallJumping)
        {
            rb.velocity = new Vector2(xWallForce * -horizontal, yWallForce);
        }
    }

    void SetWallJumpingToFalse()
    {
        isWallJumping = false;
    }
    void FaceDirection()
    {
        if (horizontal > 0 && facingRight == false) Flip();
        else if (horizontal < 0 && facingRight) Flip();
        
    }
    void Flip()
    {
        var transform1 = transform;
        var localScale = transform1.localScale;
        localScale = new Vector3(-localScale.x, localScale.y, 1);
        transform1.localScale = localScale;
        facingRight = !facingRight;
    }
    
    void Move() 
    {
        //State Control Manager
        if (stateTrackerPlayer.CanFlyMode)
        {
            rb.gravityScale = 0;
            horizontal = joystick.Horizontal;
            vertical = joystick.Vertical;
            rb.velocity = new Vector2( horizontal * flySpeed, vertical * flySpeed);
        }
        else
        {
            rb.gravityScale = gravity;
            horizontal = joystick.Horizontal;
            rb.velocity = new Vector2( horizontal * speed, rb.velocity.y);
        }
    }
    
    #region Touch Phases
    public void TouchPhaseBegin()
    {
       Jump();
    } 
    public void TouchPhaseStationary()
    {
    
    } 
    public void TouchPhaseEnd()
    {    
     
    }
    public void TouchPhaseMove()
    {
      
    }
    public void TouchPhaseDoubleTap()
    {
      
    }
    #endregion
   
    #region Swipes Phase
    public void SwipeUp()
    {
        
    }   
    public void SwipeDown()
    {
        
    }
    public void SwipeLeft()
    {

    }
    public void SwipeRight()
    {
        
    }
    public void Swipe()
    {
        Dash();
    }
    
    #endregion

    #region Buttons Controls
    public void ButtonJump()
    {
        Jump();
    }
    public void ButtonDash()
    {
        Dash();
    }
    #endregion

    void Jump()
    {
        //for wall jumping 
        if (isWallSliding && isWallJumping == false)
        {
            isWallJumping = true;
            canDoubleJump = false;
            Invoke(nameof(SetWallJumpingToFalse) , wallJumpTime);
        }
        //for jumping
        if (isGrounded)
        {
            Jumping();
        }
        else if (!isGrounded && doubleJumpAmount > 0 && canDoubleJump )
        {
            //double jumping
            doubleJumpAmount--;
            Jumping();
        }
    }
    void Jumping()
    {
        if (stateTrackerPlayer.CanFlyMode == false)
        {
            rb.velocity = Vector2.up * jumpForce;
        }
    }
    void Dash()
    {
        if (canDash && dashAmount > 0)
        {
            if (facingRight)
                rb.AddForce(Vector2.right * (dashForce * 10 * Time.deltaTime) );
            else rb.AddForce(Vector2.left * (dashForce * 10 * Time.deltaTime));
            dashAmount--;
        }
    
    }
    
    
    //saving Data
    void SaveMovementPlayerData()
    {
        SavingSystem.SaveMovementData(this);
    }
    void LoadMovementPlayerData()
    {
        MovementPlayerData data = SavingSystem.LoadMovementData();
        //Settable variables
       //TODO set variables
    }

}
