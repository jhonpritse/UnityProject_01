
using System;
using UnityEngine;
using Pathfinding;
using UnityEditor;

public class Enemy1 : MonoBehaviour
{
    EnemyLogic enemyLogic;

    #region Variables

    [SerializeField]
    private  float knockBackDuration;
    [SerializeField]
    private  float knockBackPower;
    [SerializeField]
    [Range(0f, 1.5f)]
    private  float destroyDelay;

    private AIPath aiPath;
    private GameObject player;
    private Vector2 playerPosition;
    private float playerDistance;
    [Range(0f, 15f)]
    [SerializeField] private float searchRange;
    #endregion
    void Start()
    {
        enemyLogic = new EnemyLogic();
        aiPath = GetComponent<AIPath>();
        player = GameObject.FindWithTag("Player").gameObject;
    }

    private void Update()
    {
       SearchForPlayer();
        
    }

    void SearchForPlayer()
    {
        playerPosition = player.GetComponent<Transform>().position;
        playerDistance = Vector2.Distance(transform.position, playerPosition);

        if (playerDistance < searchRange)
        {
           //player is within range of enemy 
           // aiPath.canSearch = true;
           // print("player is with in sight");
        }
        else
        {
            // aiPath.canSearch = false;
            // print("player is NOT seen");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, searchRange);

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
            Invoke(nameof(ScanNewPath), .0005f);
        }
    }

    void ScanNewPath()
    {
        AstarPath astarPath = GameObject.FindWithTag("PathFindingAI").GetComponent<AstarPath>();
        astarPath.Scan();
    }
    
}
