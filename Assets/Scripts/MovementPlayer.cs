
using UnityEditor;
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
    
    [SerializeField]
    private float checkRadius;
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

    #endregion
    
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
    }
    void TouchInput()
    {
        // isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, ground);
        isGrounded = Physics2D.OverlapBox(feetPos.position, new Vector2(3,1), 0,ground);
        if (Input.touchCount >0)
            touchInputs.TouchInput();
            
    }
  //
  // void  OnDrawGizmosSelected()
  //   {
  //       Gizmos.color = Color.red;
  //       Gizmos.DrawWireCube(feetPos.transform.position, new Vector2(3, 1));
  //   }
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
