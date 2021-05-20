


using System;
using UnityEngine;
using Pathfinding;
using Player.Scripts;
using Random = UnityEngine.Random;


public class AkujiEnemy : MonoBehaviour
    {
        EnemyLogic enemyLogic;

        #region Variables

        private  float knockBackDuration;
        private  float knockBackPower;
        private  float destroyDelay;
        
        private GameObject player;
        private Vector2 playerPosition;
        
        private float playerDistance;
        private float searchRange;
        private bool isHitPlayer;

        private CircleCollider2D cc;
        private EnemyPathFindingAI epf;
        private Rigidbody2D rb;


        [HideInInspector] public bool isSpawned;
      

        private Transform[] patrolPosition;
        
        private Transform waypointParent;
        [SerializeField] private EnemyDataObject enemyData;
        #endregion
    
   
        // private void OnDrawGizmos()
        // {
        //     Gizmos.color = Color.red;
        //     Gizmos.DrawWireSphere(transform.position, enemyData.detectionRange);
        //     if (isSpawned)
        //     {
        //         for (int i = 0; i < enemyData.amountPatrolPoints; i++)
        //         {
        //             GameObject point1;
        //             GameObject point2;
        //            
        //             string patrolName=i+1 + "_"+ gameObject.name+ "_PatrolPoints";
        //             GameObject patrolPoints = waypointParent.transform.Find(patrolName).gameObject;
        //             
        //             point1 =waypointParent.transform.Find(patrolName).gameObject;
        //             
        //             if (i+1 == enemyData.amountPatrolPoints)
        //             {
        //                 point2 = waypointParent.transform.Find(patrolName).gameObject;
        //             }
        //             else
        //             {
        //                  patrolName=i+2 + "_"+ gameObject.name+ "_PatrolPoints";
        //                 point2 =  waypointParent.transform.Find(patrolName).gameObject;
        //             }
        //
        //
        //             Gizmos.color = new Color(0, 1, 0.5f, .15f);
        //             Gizmos.DrawLine(point1.transform.position, point2.transform.position);
        //         }     
        //     }
        //     
        // }
        void Awake()
        {
            SetUpComponents();
         
        }
        void SetUpComponents()
        {
            // gameObject.name = enemyData.enemyName;
            
            gameObject.AddComponent<Seeker>();
            epf = gameObject.AddComponent<EnemyPathFindingAI>();
            epf.EnemyData = enemyData;
            cc = gameObject.AddComponent<CircleCollider2D>();
            cc.isTrigger = true;
        
            enemyLogic = new EnemyLogic();

            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = enemyData.gravity;
            rb.drag = enemyData.linearDrag;
            rb.collisionDetectionMode = enemyData.collisionDetectionMode2D;
            rb.constraints = enemyData.rigidbodyConstraints2D;
            rb.sharedMaterial = enemyData.physicsMaterial2D;
            
            player = GameObject.FindWithTag("Player").gameObject;

            searchRange = enemyData.detectionRange;
            knockBackDuration = enemyData.knockBackDuration;
            knockBackPower = enemyData.knockBackPower;
            destroyDelay = enemyData.destroyDelay;
            
            patrolPosition = new Transform[enemyData.amountPatrolPoints];
            waypointParent = GameObject.Find("WayPoints").gameObject.transform;
        }
        
        public void SetUpPatrolPoints()
        {
            waypointParent = GameObject.Find("WayPoints").gameObject.transform;
            for (int i = 0; i < enemyData.amountPatrolPoints; i++)
            {
                GameObject patrolPoints = Instantiate(enemyData.patrolPoints,transform.position , Quaternion.identity); 
                patrolPoints.transform.parent = waypointParent;
                patrolPoints.name =i+1 + "_"+ gameObject.name+ "_PatrolPoints";
            }
            isSpawned = true;
        }
        public void RemovePatrolPoints()
       {
           for (int i = 0; i < enemyData.amountPatrolPoints; i++)
           {
               string patrolName=i+1 + "_"+ gameObject.name+ "_PatrolPoints";
               GameObject patrolPoints = waypointParent.transform.Find(patrolName).gameObject;
               DestroyImmediate(patrolPoints);
               isSpawned = false;
           }
       }
        
        private void Update()
        {
            SearchForPlayer();
            RefreshPathAIMap();
        }
        
        void SearchForPlayer()
        {
            playerPosition = player.GetComponent<Transform>().position;
            playerDistance = Vector2.Distance(transform.position, playerPosition);

            if (playerDistance < searchRange)
            {
                PlayerInRange();
            }
            else
            {
               PatrolState();
            }
        }

        void PlayerInRange()
        {
        Debug.Log("Player in range " + playerDistance);
        }

        void PatrolState()
        {
            if (patrolPosition.Length <= enemyData.amountPatrolPoints)
            {
                for (int i = 0; i < enemyData.amountPatrolPoints; i++)
                {
                    string patrolName=i+1 + "_"+ gameObject.name+ "_PatrolPoints";
                    patrolPosition[i] = waypointParent.transform.Find(patrolName).gameObject.transform;
                }
            }
            
            InvokeRepeating(nameof(StartPathFind), 0f, 2f);
            
        }

        void StartPathFind()
        {
            int wayPoint = Random.Range(0, patrolPosition.Length);
            epf.StartPathFind(patrolPosition[wayPoint].transform);
        }
        private void OnTriggerEnter2D(Collider2D player)
        {
            HitPlayerLogic(player);
        }
        void HitPlayerLogic(Collider2D _object)
        {
            if (_object.gameObject.tag.Equals("Player"))
            {
                MovementPlayer playerMovementPlayer = _object.GetComponent<MovementPlayer>();
                AbilitiesPlayer playerAbilitiesPlayer = _object.GetComponent<AbilitiesPlayer>();
                StateTrackerPlayer playerStateTrackerPlayer = _object.GetComponent<StateTrackerPlayer>();
            
                playerStateTrackerPlayer.IsDamage = true;
                if (   playerStateTrackerPlayer.Health > 1)
                {
                    enemyLogic.DestroyEnemy(gameObject, destroyDelay);
                
                    playerAbilitiesPlayer.IsExplode = true;
                    GetComponent<EnemyPathFindingAI>().enabled = false;
                
                    if ( playerMovementPlayer.DashAmount <= playerMovementPlayer.DashMaxAmount )
                    {
                        playerMovementPlayer.DashAmount++;
                    }
       
                }
            
                enemyLogic.KnockBack(knockBackDuration, knockBackPower, _object.gameObject, gameObject);
                Invoke(nameof(DelayedPlayerHit), .0005f);
            }
        }

        void DelayedPlayerHit()
        {
            isHitPlayer = true;
            cc.isTrigger = false;
        }
        void ScanNewPath()
        {
            AstarPath astarPath = GameObject.FindWithTag("PathFindingAI").GetComponent<AstarPath>();
            astarPath.Scan();
        }
        void RefreshPathAIMap()
        {
            if (isHitPlayer)
            {
                ScanNewPath();
            }
        }
    
        
        
        
        
   
        
        
        
        
        
        
        
        
        
    }

