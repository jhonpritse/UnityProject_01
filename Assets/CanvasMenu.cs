using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject[] pauseMenuObjects;
    [SerializeField] private GameObject[] controlOptionObjects;
    [SerializeField] private GameObject[] settingsMenuObjects;
    [SerializeField] private GameObject[] buttonsControlButtons;






    private bool isButtonControl; public bool IsButtonControl
    {
        get => isButtonControl;
        set => isButtonControl = value;
    }
    
    public void Update()
    {
        if (isButtonControl)
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
    public void QuitApp()
    {
        Application.Quit();
    }

    public   void ReloadTestScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    public void PauseGame()
    {
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

    void HandleTouchWhenPaused()
    {
    
        if (Time.timeScale.Equals(1))
        {
            //show joystick when playing
            GameObject.FindWithTag("GameController").GetComponent<FloatingJoystick>().enabled = true;
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
            //make buttons not be able to be intractable 
            foreach (var buttons in buttonsControlButtons)
            {
                buttons.GetComponent<Button>().interactable = false;
            } 
        }
    }
}
