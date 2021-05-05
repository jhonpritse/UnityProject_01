
using UnityEngine;

public class TouchInputs : MonoBehaviour
{
    
    
    private bool isMoving ;
    private bool stateOnce = true;

    private bool previousState;
    private bool currentState;
    private int stateCounter;

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
    public void TouchInput()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            Vector3 touchPosition = cam.ScreenToViewportPoint(Input.touches[i].position);
            // //TODO remove gizmo
             // Debug.DrawLine(Vector3.zero, touchPosition, Color.magenta);
              // Debug.Log(touchPosition.x);
            if (touchPosition.x > 0.5f  )
            {
                Touch touch = Input.GetTouch(i);
                if (touch.phase == TouchPhase.Began || 
                    touch.phase == TouchPhase.Stationary)
                {
                    isMoving = true;
                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    isMoving = true;
                    Move();
                }
                else
                    isMoving = false;
                
                previousState = currentState;
                currentState = isMoving;
                if (previousState != currentState) stateCounter++;
                
                if (stateCounter % 2== 0)
                {
                    End();
                    stateOnce = true;
                }
                else
                {
                    if (stateOnce)
                    {
                        Begin();
                        stateOnce = false;
                    }
                }

                if (isMoving) Stationary();


            }
        }
    }

    
    
}
