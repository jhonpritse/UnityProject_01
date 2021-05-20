using Cinemachine;
using UnityEngine;

namespace Player.Scripts
{
    public class StateTrackerPlayer : MonoBehaviour
    {

        #region Variables

        private float switchModeCounter; public float SwitchModeCounter
        {
            get => switchModeCounter;
            set => switchModeCounter = value;
        }
        [SerializeField]
        [Range(1, 10)]
        private  float switchModeTime;

        private bool canFlyMode; public bool CanFlyMode
        {
            get => canFlyMode;
            set => canFlyMode = value;
        }
        private int health = 2; public int Health
        {
            get => health;
            set => health = value;
        }
        private int walkingModeHealth = 2; public int WalkingModeHealth
        {
            get => walkingModeHealth;
            set => walkingModeHealth = value;
        }
        private int flyingModeHealth = 1; public int FlyingModeHealth
        {
            get => flyingModeHealth;
            set => flyingModeHealth = value;
        }

    
        [SerializeField]
        private GameObject ringMode;
        private Color ringColor;
        [Range(0,10)] 
        [SerializeField]
        private float ringAlphaModifier;
        [Range(0,10)] 
        [SerializeField]
        private float ringSizeModifier;
        [Range(5,15)] 
        [SerializeField]
        private float ringMaxSize;
        [Range(-5,5)]
        [SerializeField]
        private float ringMinSize;
    
        private bool isDamage; public bool IsDamage
        {
            get => isDamage;
            set => isDamage = value;
        }
    
        private bool canBeDamage = true;
    
        [Range(0,1.5f)]
        [SerializeField]
        private float damageResetTime;
        private float damageTimeCounter;
    
        [SerializeField]  
        private  GameObject hitParticles;
        [SerializeField]  
        private  CinemachineVirtualCamera cam;
        private float fovMinSize;
        private float fovMaxSize;
        [SerializeField] 
        [Range(1, 180)]
        private float fovSizeModifier;
        private float fov ;
        #endregion
    
        void Start()
        {
            ringColor = ringMode.GetComponent<SpriteRenderer>().color;
            fov = cam.m_Lens.FieldOfView;
            fovMinSize = fov;
            fovMaxSize = fov + 25;
        
            switchModeCounter = switchModeTime;
            damageTimeCounter = damageResetTime;
        }

        void Update()
        {
            HealthTracker();
            CamZoomWhenFly();
            RingTracker();
        }
   
        void CamZoomWhenFly()
        {
            if (canFlyMode)
            {
                if (cam.m_Lens.FieldOfView < fovMaxSize)
                {
                    fov += fovSizeModifier * Time.deltaTime;
                }
                else fov = fovMaxSize;
                cam.m_Lens.FieldOfView = fov;
            }
            else
            {
                if (cam.m_Lens.FieldOfView > fovMinSize)
                {
                    fov -= fovSizeModifier * Time.deltaTime;
                }
                else fov = fovMinSize;
                cam.m_Lens.FieldOfView = fov;
            }
        }
        void HealthTracker()
        {
            if (health == walkingModeHealth)
            {
                canFlyMode = false;
            }else if (health == flyingModeHealth)
            {
                FlyMode();
                canFlyMode = true;
            }else DeathState();
        
            if (isDamage)
            {
                damageTimeCounter -= Time.deltaTime;
                if (canBeDamage)
                {
                    health -= 1;
                    Instantiate(hitParticles, transform.position, Quaternion.identity);
                    canBeDamage = false;
                }
                if (damageTimeCounter <= 0)
                {
                    canBeDamage = true;
                    isDamage = false;
                    damageTimeCounter = damageResetTime;
                }
            
            }
        
        }
    
        void RingTracker()
        {
            float ringSize = (Mathf.Lerp(ringMinSize, ringMaxSize, switchModeCounter/ringSizeModifier));
            ringMode.transform.localScale = new Vector3(ringMinSize + ringSize, ringMinSize + ringSize);
            float ringAlpha = (Mathf.Lerp(0f, 1, switchModeCounter/ringAlphaModifier));
            ringColor.a = 1-ringAlpha;
            ringMode.GetComponent<SpriteRenderer>().color = ringColor;
        }
    
        private void FlyMode()
        {
            if (canFlyMode)
            {
                switchModeCounter -= Time.deltaTime;
                if (switchModeCounter <= 0)
                {
                    switchModeCounter = switchModeTime;
                    health++;
                }
            }
        }
    
        void DeathState()
        {
            print("GAME OVER");
        }
    
    }
}
