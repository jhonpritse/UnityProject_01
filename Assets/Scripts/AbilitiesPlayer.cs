using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitiesPlayer : MonoBehaviour
{
  

    [SerializeField]  private Transform destructibleParent; 
    [SerializeField] private float explodeDistance;

    [HideInInspector]
    public bool setBomb;

    void Update()
    {
        if (setBomb)
        {
            Explode();
        }

    }

 

    void Explode()
    {
        for (int i = 0; i < destructibleParent.childCount; i++)
        {
            Transform destructibleObjects = destructibleParent.GetChild(i).gameObject.transform;
            Transform playerTransform = gameObject.GetComponent<Transform>();
            float distance = Vector3.Distance(destructibleObjects.position, playerTransform.position);
            if (distance <= explodeDistance)
            {
           
                DestroyObjects(destructibleParent.GetChild(i).gameObject);
            } 
        }
    }

    private void OnDrawGizmosSelected()
    {
        // for (int i = 0; i < destructibleParent.childCount; i++)
        // {
        //     Transform destructibleObjects = destructibleParent.GetChild(i).gameObject.transform;
        //     Transform playerTransform = gameObject.GetComponent<Transform>();
        //     float distance = Vector3.Distance(destructibleObjects.position, playerTransform.position);
        //     Gizmos.color = Color.red;
        //     var position = playerTransform.transform.position;
        //     Gizmos.DrawLine(position, destructibleObjects.transform.position);
        //     Gizmos.DrawWireSphere(position, explodeDistance);
        // }
    }

    void DestroyObjects(GameObject objects)
    {
        setBomb = false;
        Destroy(objects);
    }
}
