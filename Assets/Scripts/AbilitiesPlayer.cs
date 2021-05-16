using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitiesPlayer : MonoBehaviour
{
  

    [SerializeField]
    private Transform destructibleParent; 
    [SerializeField]
    [Range(1, 10)]
    private float explodeDistance;

    private bool isExplode; public bool IsExplode
    {
        get => isExplode;
        set => isExplode = value;
    }
    
    void Update()
    {
        if (isExplode)
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
        isExplode = false;
    }
    
    private void OnDrawGizmosSelected()
    {
        // for (int i = 0; i < destructibleParent.childCount; i++)
        // {
        //     Transform destructibleObjects = destructibleParent.GetChild(i).gameObject.transform;
        //     Transform playerTransform = gameObject.GetComponent<Transform>();
        //     float distance = Vector3.Distance(destructibleObjects.position, playerTransform.position);
        //     Gizmos.color = Color.green;
        //     var position = playerTransform.transform.position;
        //
        //     // Gizmos.DrawWireSphere(position, explodeDistance);
        //     
        //     if (distance <= explodeDistance)
        //     {
        //         Gizmos.DrawLine(position, destructibleObjects.transform.position);
        //     } 
        //     
        // }
    }

    void DestroyObjects(GameObject objects)
    {
        Destroy(objects);
    }
}
