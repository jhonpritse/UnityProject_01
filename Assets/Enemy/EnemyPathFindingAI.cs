using System;
using System.Collections;
using UnityEngine;
using Pathfinding;

public class EnemyPathFindingAI: MonoBehaviour
{
    private Transform target;

    private Transform enemySprite;
    
    private float speed;
    private float nextWaypointDistance;
    

    private Path path;
    private int currentWaypoint;
//  private bool reachedEndOfPath;
    
    
    private Seeker seeker;
    private Rigidbody2D rb;
    private float distance;

    private EnemyDataObject enemyData;
    public EnemyDataObject EnemyData
    {
        set => enemyData = value;
    }

    private void Start()
    {
        seeker = GetComponent<Seeker>();
        
        target = GameObject.FindWithTag("Player").GetComponent<Transform>();
       
        rb = GetComponent<Rigidbody2D>();
        
        GameObject sprite = Instantiate(enemyData.enemySprite, transform.position, Quaternion.identity) as GameObject; 
        sprite.transform.parent = transform;
        enemySprite = enemyData.enemySprite.transform;

        nextWaypointDistance = enemyData.nextWaypointDistance;
        speed = enemyData.speed;
        
    }

  
    public void StartPathFind(Transform transform)
    {
        if (seeker.IsDone()) 
            seeker.StartPath(rb.position, transform.position, OnPathComplete);
    }
    void OnPathComplete(Path _path)
    {
        if (!_path.error)
        {
            path = _path;
            currentWaypoint = 0;
        }
    }

    void PathLogic()
    {
        if (path == null) return;
  
        if (currentWaypoint >= path.vectorPath.Count)
        {
            // reachedEndOfPath = true;
            return;
        }
        // else {reachedEndOfPath = false;}
        
        StartMovement();
    }
    void StartMovement()
    {
        var position = rb.position;
        Vector2 direction = (((Vector2) path.vectorPath[currentWaypoint]) - position).normalized;
        Vector2 enemyForce = direction * ((speed *10) * Time.deltaTime);
        
        rb.AddForce(enemyForce);
        FlipSpite(enemyForce);
        
        distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
     
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }
    void FlipSpite(  Vector2 enemyForce )
    {
        if (enemyForce.x >= 0.01f) enemySprite.localScale = new Vector3(-1, 1, 1);
        else if (enemyForce.x <= -0.01f) enemySprite.localScale = new Vector3(1, 1, 1);
    }
    
    private void FixedUpdate()
    {
        PathLogic();
    }
}