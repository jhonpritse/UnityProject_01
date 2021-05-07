using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasMenu : MonoBehaviour
{
    public void QuitApp()
    {
        Application.Quit();
    }

    public   void ReloadTestScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
