using System;
using UnityEngine;
using Pathfinding;

public class EnemyPathFindingAI: MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Transform enemySprite;
    [Range(0f, 60f)]
    [SerializeField] private float speed;
    [Range(0f, 2.5f)]
    [SerializeField] private float nextWaypointDistance;



    private Path path;
    private int currentWaypoint;
    [SerializeField] private bool reachedEndOfPath;
    
    
    private Seeker seeker;
    private Rigidbody2D rb;
    private float distance;
    private void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
 
      InvokeRepeating("UpdatePath", 0f, 0.5f);
    }
    
    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.yellow;
    //     Gizmos.DrawWireSphere(transform.position, currentWaypoint);
    //     Gizmos.color = Color.green;
    //     Gizmos.DrawWireSphere(transform.position, path.vectorPath.Count);
    // }

    void UpdatePath()
    {
        if (seeker.IsDone()) 
            seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    void OnPathComplete(Path _path)
    {
        if (!_path.error)
        {
            path = _path;
            currentWaypoint = 0;
        }
    }

    void PathRefresh()
    {
        if (path == null) return;
  
        if (currentWaypoint >= path.vectorPath.Count)
        {
            // reachedEndOfPath = true;
            return;
        }
        // else {reachedEndOfPath = false;}
        
        MoveEnemy();
    }

    void MoveEnemy()
    {
        var position = rb.position;
        Vector2 direction = (((Vector2) path.vectorPath[currentWaypoint]) - position).normalized;
        Vector2 enemyForce = direction * ((speed *10) * Time.deltaTime);
        
        rb.AddForce(enemyForce);
        FlipEnemy(enemyForce);
        
        distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
     
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    void FlipEnemy(  Vector2 enemyForce )
    {
        if (enemyForce.x >= 0.01f) enemySprite.localScale = new Vector3(-1, 1, 1);
        else if (enemyForce.x <= -0.01f) enemySprite.localScale = new Vector3(1, 1, 1);
    }
    private void FixedUpdate()
    {
        PathRefresh();
    }
}