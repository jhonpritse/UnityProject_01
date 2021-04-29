
using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    EnemyLogic enemyLogic;

    #region Variables

    public float knockBackDuration;
    public float knockBackPower;

    
    

    #endregion
    void Start()
    {
        enemyLogic = new EnemyLogic();

    }

   
 
    private void OnTriggerEnter2D(Collider2D player)
    {
        if (player.gameObject.tag.Equals("Player"))
        {
            player.GetComponent<StateTrackerPlayer>().IsDamage = true;
            if (   player.GetComponent<StateTrackerPlayer>().Health > 1)
            {
                enemyLogic.DestroyEnemy(gameObject);
                player.GetComponent<AbilitiesPlayer>().setBomb = true;
            }
            enemyLogic.KnockBack(knockBackDuration, knockBackPower, player.gameObject, gameObject);
        }
    }
}
