
using UnityEngine;

public class TouchInputs : MonoBehaviour
{
    
    
    private bool isMoving ;
    private bool stateOnce = true;

    private bool previousState;
    private bool currentState;
    private int stateCounter;

    public float minSwipeTime;
    public float maxSwipeTime;
    public float minSwipeDistance;

    private float swipeStartTime;
    private float swipeEndTime;
    public float swipeTime;

    private Vector2 startSwipePosition;
    private Vector2 endSwipePosition;
    private float swipeLength;
    private bool isSwipe;
    public Vector2 distance;
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
    }
    void Begin()
    {
        movementPlayer.TouchPhaseBegin();
    }
    void Stationary()
    {
        movementPlayer.TouchPhaseStationary(); 
    }
    void End()
    {
        movementPlayer.TouchPhaseEnd();
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
                Swipe();
            }else if (distance.x < 0 )
            {
                SwipeLeft();
                Swipe();
            }
        }
        else if( yDistance > xDistance)
        {
            if (distance.y > 0)
            {
                SwipeUp();
                Swipe();
            }else if (distance.y < 0)
            {
                SwipeDown();
                Swipe();
            }
        }
    }
    void SwipeUp()
    {
        movementPlayer.SwipeUp();
    }
    void SwipeDown()
    {
        movementPlayer.SwipeDown();
    }   
    void SwipeLeft()
    {
        movementPlayer.SwipeLeft();
    }
    void SwipeRight()
    {
        movementPlayer.SwipeRight();
    }

    void Swipe()
    {
        movementPlayer.Swipe();
    }
    
    public void TouchInput()
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
                    if (Input.touchCount > 0)
                    {
                        //swipe
                        if (isSwipe == false)
                        {
                            swipeStartTime = Time.time;
                            startSwipePosition = touch.position;
                            isSwipe = true;
                        }

                    }
                   
                    isMoving = true;
                    Move();
                }
                else
                {
                    isMoving = false;
                }
                
                previousState = currentState;
                currentState = isMoving;
                
                if (previousState != currentState)
                {
                    stateCounter++;
                }
                
                if (stateCounter % 2== 0)
                {
                    if (Input.touchCount > 0)
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
                    }
                    
                    End();
                    stateOnce = true;  
                    stateCounter = 0;
                }
                else
                {
                    if (stateOnce)
                    {
                        Begin();
                        stateOnce = false;
                    }
                }
                
                if (isMoving)
                {
                    Stationary();
                }
                

            }
            
            
        }
    }

    
    
}
