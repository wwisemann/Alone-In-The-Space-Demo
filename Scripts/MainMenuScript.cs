using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    void Update()
    {
        
    }

    public void PlayApp()
    {
        SceneManager.LoadScene("Level01");
    }

    public void QuitApp()
    {
        Application.Quit(); 
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
