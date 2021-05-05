
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
    
    [SerializeField]
    private float checkFeetRadius;   
    [SerializeField]
    private float checkHeadRadius;
    [SerializeField]
    private LayerMask ground;
    [SerializeField]
    private float jumpTime;    
    private float jumpTimeCounter;

    private StateTrackerPlayer stateTrackerPlayer;
    
    private float horizontal;
    private float vertical;
    
    private FloatingJoystick joystick;
    private TouchInputs touchInputs;

    private bool facingRight;
    #endregion
    
    void  OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(headPos.position, checkHeadRadius);
        Gizmos.DrawWireSphere(feetPos.position, checkFeetRadius);
    }

    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        touchInputs =GameObject.FindWithTag("Player").GetComponent<TouchInputs>();
        stateTrackerPlayer = GameObject.FindWithTag("Player").GetComponent<StateTrackerPlayer>();
        joystick = GameObject.FindWithTag("GameController").GetComponent<FloatingJoystick>();
        gravity = rb.gravityScale;

    }
    
    private void Update()
    {
        TouchInput();
        FaceDirection();
    }

    void FaceDirection()
    {
        if (horizontal > 0 && facingRight == false)
        {
            Flip();
        }else if (horizontal < 0 && facingRight)
        {
            Flip();
        }
    }
    void Flip()
    {
        var transform1 = transform;
        var localScale = transform1.localScale;
        localScale = new Vector3(-localScale.x, localScale.y, 1);
        transform1.localScale = localScale;
        facingRight = !facingRight;
    }
    void TouchInput()
    {
      
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkFeetRadius, ground);
     
        if (Input.touchCount >0)
            touchInputs.TouchInput();
            
    }
  

    
    void FixedUpdate() 
    {
        Move();
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
      
        if (isGrounded)
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            Jump(); 
        }
    } 
    public void TouchPhaseStationary()
    {    
        if (isJumping && jumpTimeCounter > 0)
        {
            Jump();
            jumpTimeCounter -= Time.deltaTime;
        }
        else isJumping = false;
        if (Physics2D.OverlapCircle(headPos.position, checkHeadRadius, ground))
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
    
    void Jump()
    {
        if (stateTrackerPlayer.CanFlyMode == false)
        {
            rb.velocity = Vector2.up * jumpForce; 
        }
    }


}
