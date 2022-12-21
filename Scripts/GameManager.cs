using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //Kill Count
    public float killCount;
    public Text killCountText;

    //Menu
    public GameObject WinMenuPanel;
    public GameObject LoseMenuPanel;
    public GameObject pauseMenu;
    public Text winScoreText,loseScoreText;
    private bool gameIsPaused;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(gameIsPaused == true)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void AddKill()
    {
        killCount++;
        killCountText.text = "Dead Alien: " + killCount;
    }

    public void WinLevel()
    {
        WinMenuPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
        winScoreText.text = "Score: " + killCount;
    }

    public void LoseLevel()
    {
        LoseMenuPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
        loseScoreText.text = "Score: " + killCount;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Level01");
        Time.timeScale = 1;
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Cursor.lockState= CursorLockMode.None;
        Time.timeScale = 0;
        gameIsPaused = true;
    }

    void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        gameIsPaused = false;
    }
}
