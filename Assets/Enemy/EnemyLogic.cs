
using System.Runtime.InteropServices;
using UnityEngine;
public class EnemyLogic
{
    
    
    
    public void KnockBack(float kbDuration, float kbPower, GameObject moveAwayFrom, GameObject self )
    {
        float timer = 0;
        while (kbDuration > timer)
        {
            timer += Time.deltaTime;
            Vector2 dir = (moveAwayFrom.transform.position - self.transform.position).normalized;
            self.GetComponent<Rigidbody2D>().AddForce(-dir * kbPower);
        }
    }

    public void DestroyEnemy(GameObject gameObject, float time)
    {
        Object.Destroy(gameObject, time);
    }

    public void PatrolAreaRandom( Transform rb, float speed, Transform[] patrolPoints)
    {
        int wayPoint = Random.Range(0, patrolPoints.Length);
       
        
       
        
    }
    
    public void PatrolAreaIncrement( Transform rb, float speed, Transform[] patrolPoints)
    {
        int wayPoint = Random.Range(0, patrolPoints.Length);
        
        Debug.Log(wayPoint);
        
    }
    
}
