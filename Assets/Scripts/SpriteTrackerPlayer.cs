
using UnityEngine;

public class SpriteTrackerPlayer : MonoBehaviour
{
    #region Variables
    
    [SerializeField]
    private GameObject walkMode;
    [SerializeField]
    private  GameObject flyMode;
    private StateTrackerPlayer stateTrackerPlayer ;
    

    #endregion

    void Start()
    {
        stateTrackerPlayer =GameObject.FindWithTag("Player").gameObject.GetComponent<StateTrackerPlayer>();
    }


    void Update()
    {
        if (stateTrackerPlayer.Health == stateTrackerPlayer.WalkingModeHealth)
        {
            walkMode.SetActive(true);
            flyMode.SetActive(false);
        }else if (stateTrackerPlayer.Health == stateTrackerPlayer.FlyingModeHealth)
        {
            walkMode.SetActive(false);
            flyMode.SetActive(true);
        }
        else
        {
            walkMode.SetActive(false);
            flyMode.SetActive(false);
        }
    }

}
