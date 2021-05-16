
using UnityEngine;

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

    
    #endregion
    void Start()
    {
        enemyLogic = new EnemyLogic();
    }

   
 
    private void OnTriggerEnter2D(Collider2D player)
    {
        HitPlayerLogic(player);
    }

    void HitPlayerLogic(Collider2D player)
    {
        if (player.gameObject.tag.Equals("Player"))
        {
            MovementPlayer playerMovementPlayer = player.GetComponent<MovementPlayer>();
            AbilitiesPlayer playerAbilitiesPlayer = player.GetComponent<AbilitiesPlayer>();
            StateTrackerPlayer playerStateTrackerPlayer = player.GetComponent<StateTrackerPlayer>();
            
            playerStateTrackerPlayer.IsDamage = true;
            if (   playerStateTrackerPlayer.Health > 1)
            {
                enemyLogic.DestroyEnemy(gameObject, destroyDelay);
                playerAbilitiesPlayer.IsExplode = true;
                
                if ( playerMovementPlayer.DashAmount <= playerMovementPlayer.DashMaxAmount )
                {
                    playerMovementPlayer.DashAmount++;
                }
            }
            
            enemyLogic.KnockBack(knockBackDuration, knockBackPower, player.gameObject, gameObject);
        }
    }
    
    
}
