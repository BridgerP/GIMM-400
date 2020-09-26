using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject gameOver;
    public GameObject pauseMenu;
    public GameObject youWin;
    public Text scoreTextW;
    public Text scoreTextL;

    public Slider mouseSensitivity;
    public playerMove p;

    public GameObject grass;

    public float score;

    bool paused;
    bool game;
    private void Start()
    {
        game = true;
        paused = false;
        pauseMenu.SetActive(false);
        gameOver.SetActive(false);
        youWin.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        if (game)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (paused) unpauseGame();
                else pauseGame();
            }
        }
        
    }
    void pauseGame()
    {
        paused = true;
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
    }
    void unpauseGame()
    {
        p.rotSpeed = (mouseSensitivity.normalizedValue * 250) + 70;
        paused = false;
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void gameEnd()
    {
        score += grass.transform.childCount * 10;
        score /= 2;
        game = false;
        Debug.Log(score);
        scoreTextL.text = "Score: " + score.ToString();
        Time.timeScale = 0;
        gameOver.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }
    public void Win(int health)
    {
        Debug.Log(Time.time);
        score += grass.transform.childCount * 10;
        score += health;
        score /= (Time.time) / 100;
        game = false;
        Debug.Log(score);
        int intScore = (int)score;
        scoreTextW.text = "Score: " + intScore.ToString();
        Time.timeScale = 0.25f;
        youWin.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
    }
    public void exitGame()
    {
        Application.Quit();
    }
    public void retryGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
