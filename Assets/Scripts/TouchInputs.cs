
using UnityEngine;

public class TouchInputs : MonoBehaviour
{
    
    
    private bool isMoving ;
    private bool isNotMoving = true;
    private bool stateOnce = true;

    private bool previousState;
    private bool currentState;
    private int stateCounter;

    public float minSwipeTime;
    public float maxSwipeTime;
    public float minSwipeDistance;


    public float maxDoubleTabTime;
    private float previousTapTime;
    
    private float swipeStartTime;
    private float swipeEndTime;
    private float swipeTime;

    private Vector2 startSwipePosition;
    private Vector2 endSwipePosition;
    private float swipeLength;
    private bool isSwipe;
    private Vector2 distance;
    private Camera cam;
    
    private MovementPlayer movementPlayer;
    private void Start()
    {
        movementPlayer = GameObject.FindWithTag("Player").GetComponent<MovementPlayer>();
        cam = Camera.main;
    }

    void Move()
    {
        movementPlayer.TouchPhaseMove();
        // print("Move");
    }

    void DoubleTap()
    {
        movementPlayer.TouchPhaseDoubleTap();
    }
    void Begin()
    {
        movementPlayer.TouchPhaseBegin();
        // print("begin start");
    }
    void Stationary()
    {
        movementPlayer.TouchPhaseStationary();
        // print("Not move");
    }
    void End()
    {
        movementPlayer.TouchPhaseEnd();
        // print("end of touch");
    }

    void SwipeControl()
    {
        distance = endSwipePosition - startSwipePosition;
        float xDistance = Mathf.Abs(distance.x);
        float yDistance = Mathf.Abs(distance.y);
        if (xDistance > yDistance)
        {
            if (distance.x > 0)
            {
                SwipeRight();
            }else if (distance.x < 0 )
            {
                SwipeLeft();
            }
        }
        else if( yDistance > xDistance)
        {
            if (distance.y > 0)
            {
                SwipeUp();
            }else if (distance.y < 0)
            {
                SwipeDown();
            }
        }
    }
    void SwipeUp()
    {
        movementPlayer.SwipeUp();
        // print("swipe UP");
    }
    void SwipeDown()
    {
        movementPlayer.SwipeDown();
        // print("swipe DOWN");
    }   
    void SwipeLeft()
    {
        movementPlayer.SwipeLeft();
        // print("swipe LEFT");
    }
    void SwipeRight()
    {
        movementPlayer.SwipeRight();
        // print("swipe RIGHT");
    }

    private bool isDouble;
    public void TouchInput()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            Vector3 touchPosition = cam.ScreenToViewportPoint(Input.touches[i].position);

            if (touchPosition.x > 0.5f)
            {
                Touch touch = Input.GetTouch(i);

                if (touch.phase == TouchPhase.Began)
                {
                    float timeSinceLastTap = Time.time - previousTapTime;
                    if (timeSinceLastTap <= maxDoubleTabTime)
                    {
                        //double tap
                        // print("DOUBLE");
                        isDouble = true;
                    }
                    else
                    { 
                        //normal tap  
                        // print("NORMAL");
                        isDouble = false;
                    }
                    previousTapTime = Time.time;
                }
            }
        }
        

        if (isDouble)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Vector3 touchPosition = cam.ScreenToViewportPoint(Input.touches[i].position);
                if (touchPosition.x > 0.5f )
                {
                    DoubleTap();
                }
            }

        }
        else
        { 
            for (int i = 0; i < Input.touchCount; i++) 
            { 
                Vector3 touchPosition = cam.ScreenToViewportPoint(Input.touches[i].position);

             // Debug.DrawLine(Vector3.zero, touchPosition, Color.magenta);
              // Debug.Log(touchPosition.x);

              if (touchPosition.x > 0.5f  ) 
              {
                Touch touch = Input.GetTouch(i);
               
                //touch phase controls
                if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Stationary)
                {
                    isMoving = true;
                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    isMoving = true;
                    isNotMoving = false;
                    
                    Move();
                    
                }
                else
                {
                    isMoving = false;
                }
                
                if (!isMoving && isNotMoving)
                {
                    Stationary();
                }

                previousState = currentState;
                currentState = isMoving;
                if (previousState != currentState)
                {
                    stateCounter++;
                }
                if (stateCounter % 2== 0)
                {
                    //swipe
                    isSwipe = false;
                    swipeEndTime = Time.time;
                    endSwipePosition = touch.position;
                    swipeTime = swipeEndTime - swipeStartTime;
                    swipeLength = (endSwipePosition - startSwipePosition).magnitude;
                    
                    if (swipeTime > minSwipeTime && swipeTime < maxSwipeTime && swipeLength > minSwipeDistance)
                    {
                        SwipeControl();
                    }

                    End();
                        
                    isNotMoving = true;
                    stateOnce = true;  
                    stateCounter = 0;
                }
                else
                {
                    if (stateOnce)
                    {
                        //swipe
                        if (isSwipe == false)
                        {
                            swipeStartTime = Time.time;
                            startSwipePosition = touch.position;
                            isSwipe = true;
                        }
                        Begin();
                        
                        stateOnce = false;
                    }
                }
                
              }

            }//end of forloop
        }
      
    }
    
}
