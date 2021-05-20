using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;


[CreateAssetMenu (fileName = "EnemyBase" , menuName = "Scriptable Objects/Enemy" ) ]

public class EnemyDataObject : ScriptableObject
{
   public string enemyName;
   public string description;
   public GameObject enemySprite;
   


   [ Header ("Enemy")]
   [Range(0, 30)]
   public float detectionRange;
   [Range(0, 2)] [Tooltip("How Long will knockback will be applied")]
   public float knockBackDuration;
   [Range(0, 2)][Tooltip("the delay before Destroy() will be called")]
   public float destroyDelay;
   [Range(0, 50)][Tooltip("The force of the knockback")]
   public float knockBackPower;
   [Range(0, 20)][Tooltip("amount of patrol point the enemy will go to")]
   public int amountPatrolPoints;
   public GameObject patrolPoints;
   [Header("Enemy Path Finding AI Script")]
   [Range(0, 3)]
   public float nextWaypointDistance;
   [Range(0,60)]
   public float speed;



   [Header("Enemy Path Finding AI Script")]
   [Range(-1, 1)]
   public float gravity;
   [Range(0, 5)]
   public float linearDrag; 
   public CollisionDetectionMode2D collisionDetectionMode2D;
   public RigidbodyConstraints2D rigidbodyConstraints2D;
   public PhysicsMaterial2D physicsMaterial2D;


}
