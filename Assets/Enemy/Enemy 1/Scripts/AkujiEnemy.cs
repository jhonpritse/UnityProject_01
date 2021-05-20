


using System;
using UnityEngine;
using Pathfinding;
using Player.Scripts;
using UnityEditor;
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
      

        [SerializeField] private Transform[] patrolPosition;

        private Transform waypointParent;
        private int wayPoint;
        private bool isNewPoint;
        [SerializeField] private EnemyDataObject enemyData;
        #endregion
    
   
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, enemyData.detectionRange);
            waypointParent = GameObject.Find("WayPoints").gameObject.transform;
            if (isSpawned)
            {
             
                if (EditorApplication.isPlaying && patrolPosition.Length > 0 )
                {
                    for (int i = 0; i < enemyData.amountPatrolPoints; i++)
                    {
                        Vector2 point1;
                        Vector2 point2;
                  
                        point1 = patrolPosition[i].position;
                 
                        if (i+1 == enemyData.amountPatrolPoints)
                        {
                      
                            point2 = patrolPosition[0].position;
                        }
                        else
                        {
                     
                            point2 = patrolPosition[i+1].position;
                        }
               
                        Gizmos.color = new Color(0, 1, 0.5f, .25f);
                        Gizmos.DrawLine(point1, point2);
                        Gizmos.color = new Color(1, 1 ,0.5f, .5f);
                        Gizmos.DrawSphere(point1, .5f);
                    
                    } 
                }
                else
                {
                    for (int i = 0; i < enemyData.amountPatrolPoints; i++)
                    {
                        Transform point1;
                        Transform point2;
                        string patrolName=gameObject.name +"_"+ (i+1) + "_PatrolPoints";
                   
                        point1 =waypointParent.transform.Find(patrolName).transform;
                 
                        if (i+1 == enemyData.amountPatrolPoints)
                        {
                            patrolName=gameObject.name +"_"+ (1) + "_PatrolPoints";
                            point2 = waypointParent.transform.Find(patrolName).transform;
                        }
                        else
                        {
                            patrolName=gameObject.name +"_"+ (i+2) + "_PatrolPoints";
                            point2 =  waypointParent.transform.Find(patrolName).transform;
                        }
               
                        Gizmos.color = new Color(0, 1, 0.5f, .25f);
                        Gizmos.DrawLine(point1.position, point2.position);
                        Gizmos.color = new Color(1, 1 ,0.5f, .5f);
                        Gizmos.DrawSphere(point1.position, .3f);
                    
                    }     
                }

             
            }

        }
        void Awake()
        {
            SetUpComponents();
            SetUpPatrolPoints();

        }

     

        void SetUpPatrolPoints()
        {
            isNewPoint = true;
            wayPoint = 0;
            waypointParent = GameObject.Find("WayPoints").gameObject.transform;
            for (int i = 0; i < enemyData.amountPatrolPoints; i++)
            {
                string pointName=gameObject.name +"_"+ (i+1) + "_PatrolPoints";
                GameObject point = waypointParent.transform.Find(pointName).gameObject;
            
                patrolPosition[i] = point.transform;
                
            }
            
           
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
        
        public void SpawnPatrolPoints()
        {
            waypointParent = GameObject.Find("WayPoints").gameObject.transform;
            for (int i = 0; i < enemyData.amountPatrolPoints; i++)
            {
                GameObject patrolPoints = Instantiate(enemyData.patrolPoints,transform.position , Quaternion.identity);
                patrolPoints.transform.parent = waypointParent;
                patrolPoints.name = gameObject.name +"_"+  (i+1) + "_PatrolPoints";
            }
            isSpawned = true;
        }
        public void RemovePatrolPoints()
       {
           for (int i = 0; i < enemyData.amountPatrolPoints; i++)
           {
               string patrolName=gameObject.name +"_"+  (i+1) + "_PatrolPoints";
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
      
        }

        void PatrolState()
        {
            // if (patrolPosition.Length <= enemyData.amountPatrolPoints)
            // {
            //     for (int i = 0; i < enemyData.amountPatrolPoints; i++)
            //     {
            //         string patrolName=i+1 + "_"+ gameObject.name+ "_PatrolPoints";
            //         patrolPosition[i] = waypointParent.transform.Find(patrolName).gameObject.transform;
            //     }
            // }
            //
            // transform.position = Vector3.MoveTowards(transform.position, patrolPosition[0],enemyData.speed  * Time.deltaTime);
            
            // InvokeRepeating(nameof(StartPathFind), 0f, .5f);
            StartPathFind();
        }

        void StartPathFind()
        {
            // int wayPoint = Random.Range(0, patrolPosition.Length);
             epf.StartPathFind(patrolPosition[wayPoint].transform);
        }
        private void OnTriggerEnter2D(Collider2D objCollider2D)
        {
            HitPlayerLogic(objCollider2D);
            HitPatrolPointLogic(objCollider2D);
        }
        private void OnTriggerExit2D(Collider2D objCollider2D)
        {
            HitExitPatrolPointLogic(objCollider2D);
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
        void HitPatrolPointLogic(Collider2D _object)
        {
            if (_object.CompareTag("PatrolPoints") && isNewPoint)
            {
                var nextPoint = wayPoint + 1;
                wayPoint = nextPoint == patrolPosition.Length ? 0 : nextPoint;
                isNewPoint = false;
            }
        }
        void HitExitPatrolPointLogic(Collider2D _object)
        {
            if (_object.CompareTag("PatrolPoints"))
            {
                isNewPoint = true;
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

