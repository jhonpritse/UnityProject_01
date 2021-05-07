
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
    private float jumpForce;

    private float gravity;
    
    private bool isGrounded;
    private bool isJumping;

    public Transform feetPos;
    public Transform headPos;
    public Transform frontPos;
    
    [SerializeField]
    private float checkFeetRadius;   
    [SerializeField]
    private float checkHeadRadius;
    [SerializeField]
    private float checkFrontRadius;   
    [SerializeField]
    private LayerMask groundLayer;
    [SerializeField]
    private float jumpTime;    
    private float jumpTimeCounter;

    private StateTrackerPlayer stateTrackerPlayer;
    
    private float horizontal;
    private float vertical;
    
    private FloatingJoystick joystick;
    private TouchInputs touchInputs;

    private bool facingRight;
    private bool isTouchingFront;
    private bool isWallSliding;
    private bool isWallJumping;
      
    public float wallSlidingSpeed;
    public float xWallForce;
    public float yWallForce;
    public float wallJumpTime;

    public float dashForce;
    private float dashTimeCounter;
    public float dashTime;

    #endregion
    
    void  OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(headPos.position, checkHeadRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(feetPos.position, checkFeetRadius);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(frontPos.position, checkFrontRadius);
    }

    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        touchInputs =GameObject.FindWithTag("Player").GetComponent<TouchInputs>();
        stateTrackerPlayer = GameObject.FindWithTag("Player").GetComponent<StateTrackerPlayer>();
        joystick = GameObject.FindWithTag("GameController").GetComponent<FloatingJoystick>();
        gravity = rb.gravityScale;
        dashTimeCounter = dashTime;

    }
    
    private void Update()
    {
        TouchInput();
        Dash();

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
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkFeetRadius, groundLayer);
        if (Input.touchCount >0) touchInputs.TouchInput();
        
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
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
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
        if (isWallSliding && isWallJumping == false)
        {
            isWallJumping = true;
            Invoke(nameof(SetWallJumpingToFalse) , wallJumpTime);
        }
        
        if (isGrounded)
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            if (touchInputs.swipeTime >  touchInputs.maxSwipeTime)
            {
                Jump(); 
            }
          
        }
    } 
    public void TouchPhaseStationary()
    {    
        if (isJumping && jumpTimeCounter > 0)
        {
            if (touchInputs.swipeTime >  touchInputs.maxSwipeTime)
            {
                Jump(); 
            }
            jumpTimeCounter -= Time.deltaTime;
        }
        else isJumping = false;
        if (Physics2D.OverlapCircle(headPos.position, checkHeadRadius, groundLayer))
        {
            isJumping = false;
        }
    } 
    public void TouchPhaseEnd()
    {    
        isJumping = false;
    }
    public void TouchPhaseMove()
    {
      
    }
    #endregion


    #region Swipes

    public void Swipe()
    {
     
    }
    public void SwipeUp()
    {


    }   
    public void SwipeDown()
    {
  

        
    }

    bool swipeLeft;
    public void SwipeLeft()
    {
        swipeLeft = true;
        
    }
    void DashLeft()
    {
        if (swipeLeft)
        {
            if (dashTime <= 0)
            {
                dashTime = dashTimeCounter;
                swipeLeft = false;
            }
            else
            {
                dashTime -= Time.deltaTime;
                rb.AddForce(Vector2.left * dashForce);
            } 
            if (facingRight)
            {
                Flip();
            }
        }
       
    }

    private bool swipeRight;
    public void SwipeRight()
    {
        swipeRight = true;
    }
    void DashRight()
    { 
        if (swipeRight)
        {
            if (dashTime <= 0)
            {
                dashTime = dashTimeCounter;
                swipeRight = false;
            }
            else
            {
                dashTime -= Time.deltaTime;
                rb.AddForce(Vector2.right * dashForce);
            }

            if (!facingRight)
            {
                Flip();
            }
        }
    }
   #endregion

   void Dash()
   {
       DashLeft();
       DashRight();
   }
    void Jump()
    {
        if (stateTrackerPlayer.CanFlyMode == false)
        {
            rb.velocity = Vector2.up * jumpForce; 
        }
    }


}
