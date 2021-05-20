using Asset_Imports.Joystick_Pack.Scripts.Joysticks;
using GameScripts.Saving_Game_Data;
using Player.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameScripts
{
    public class CanvasMenu : MonoBehaviour
    {
        [SerializeField] private GameObject pauseButton;
        [SerializeField] private GameObject dashButton;
    
        [SerializeField] private GameObject[] pauseMenuObjects;
        [SerializeField] private GameObject[] controlOptionObjects;
        [SerializeField] private GameObject[] settingsMenuObjects;
        [SerializeField] private GameObject[] buttonsControlButtons;
        private bool isButtonControl; public bool IsButtonControl
        {
            get => isButtonControl;
            set => isButtonControl = value;
        }

        private MovementPlayer movementPlayer;

        private const string Key = "CanvasMenuFirstAwake";
    
    
    
        private void OnEnable()
        {
            if (SavingSystem.CreatePathIfNull())
            {
                SaveSettings();
            }
            if (PlayerPrefs.HasKey(Key))
            {
                LoadSettingsData();
            }

       
        }
        private void OnDisable()
        {
            PlayerPrefs.SetInt(Key , 1);
        }

        public void Start()
        {
            movementPlayer = GameObject.FindWithTag("Player").GetComponent<MovementPlayer>();
        }

        public void Update()
        {
            HandleButtons();
            HandleDash();
        }

        public void QuitApp()
        {
            Application.Quit();
        }

        public void ReloadTestScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Time.timeScale = 1;
        }
        public void PauseGame()
        {
            SaveSettingsData();
            Time.timeScale = 0;
            foreach (var pauseMenu in pauseMenuObjects)
            {
                pauseMenu.SetActive(true);
            } 
            pauseButton.SetActive(false);
            HandleTouchWhenPaused();
        }
        public void ResumeGame()
        {
            foreach (var pauseMenu in pauseMenuObjects)
            {
                pauseMenu.SetActive(false);
            }
            pauseButton.SetActive(true);
            Time.timeScale = 1;
            HandleTouchWhenPaused();
        }
        public void Settings()
        {
            foreach (var settingsMenu in settingsMenuObjects)
            {
                settingsMenu.SetActive(true);
            }
        }
    
        public void SaveSettings()
        {
            SaveSettingsData();
            foreach (var settingsMenu in settingsMenuObjects)
            {
                settingsMenu.SetActive(false);
            } 
        }
        public void NotSaveSettings()
        {
            LoadSettingsData();
            foreach (var settingsMenu in settingsMenuObjects)
            {
                settingsMenu.SetActive(false);
            } 
        }
    
        public void ChangeControlSettings()
        {
            foreach (var buttons in controlOptionObjects)
            {
                buttons.SetActive(true);
            }
        }
        public void SaveControlSettings()
        {
            foreach (var buttons in controlOptionObjects)
            {
                buttons.SetActive(false);
            }
        }
    
        public void TouchControls()
        {
            isButtonControl = false;
            SaveControlSettings();
        }
        public void ButtonControls()
        {
            isButtonControl = true;
            SaveControlSettings();
        }
    
        void HandleDash()
        {
            if (movementPlayer.DashAmount > 0 )
            {
                dashButton.SetActive(true);
                if (movementPlayer.CanDash)
                {
                    dashButton.GetComponent<Button>().interactable = true;
                }else    dashButton.GetComponent<Button>().interactable = false;
            } else
            {
                dashButton.SetActive(false);
            }
        }
        void HandleButtons()
        {
            if (IsButtonControl)
            {
                foreach (var buttons in buttonsControlButtons)
                {
                    buttons.SetActive(true);
                } 
            }
            else
            {
                foreach (var buttons in buttonsControlButtons)
                {
                    buttons.SetActive(false);
                } 
            }

        }
        void HandleTouchWhenPaused()
        {
            if (Time.timeScale.Equals(1))
            {
                //show joystick when playing
                GameObject.FindWithTag("GameController").GetComponent<FloatingJoystick>().enabled = true;
                GameObject.FindWithTag("Player").GetComponent<MovementPlayer>().enabled = true;
                //make buttons intractable when resumed 
                foreach (var buttons in buttonsControlButtons)
                {
                    buttons.GetComponent<Button>().interactable = true;
                } 
            }
            else
            {
                //hide when paused 
                GameObject.FindWithTag("GameController").GetComponent<FloatingJoystick>().enabled = false;
                GameObject.FindWithTag("Player").GetComponent<MovementPlayer>().enabled = false;
                //make buttons not be able to be intractable 
                foreach (var buttons in buttonsControlButtons)
                {
                    buttons.GetComponent<Button>().interactable = false;
                } 
            }
        }

        void SaveSettingsData()
        {
            SavingSystem.SaveSettings(this);
        }
        void LoadSettingsData()
        {
            SettingsData data = SavingSystem.LoadSettings();
            //Settable variables
            isButtonControl = data.IsButtonControls;
        }
    }
}
