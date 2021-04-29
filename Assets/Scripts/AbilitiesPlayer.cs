using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitiesPlayer : MonoBehaviour
{
  

    [SerializeField]  private Transform destructibleParent; 
    [SerializeField] private float explodeDistance;
    private StateTrackerPlayer stateTrackerPlayer;

    void Start()
    {
        stateTrackerPlayer = GetComponent<StateTrackerPlayer>();
    }

    void Update()
    {
     print(stateTrackerPlayer.IsDamage);
        // if (stateTrackerPlayer.IsDamage)
        // {
        //     Explode();
        // }

    }

    void Explode()
    {
        for (int i = 0; i < destructibleParent.childCount; i++)
        {
            Transform destructibleObjects = destructibleParent.GetChild(i).gameObject.transform;
            Transform playerTransform = gameObject.GetComponent<Transform>();
            float distance = Vector3.Distance(destructibleObjects.position, playerTransform.position);
                print(distance + " ===  " + destructibleObjects);
            if (distance <= explodeDistance)
            {
                DestroyObjects(destructibleParent.GetChild(i).gameObject);
            } 
        }
    }

    void DestroyObjects(GameObject objects)
    {
        Destroy(objects);
        //TODO add destroy particles
    }
}
