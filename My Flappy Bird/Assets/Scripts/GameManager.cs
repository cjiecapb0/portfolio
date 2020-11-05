using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Object.DontDestroyOnLoad
public class GameManager : MonoBehaviour
{
    public GameObject gameOver;
    public GameObject pause;
    public GameObject pauseButton;
    public GameObject CanvasScore;
    private void Start()
    {
        Time.timeScale = 1;
    }
    public void GameOver()
    {
        gameOver.SetActive(true);
        Time.timeScale = 0;
        CanvasScore.SetActive(false);
    }
    public void StartGame()
    {
        SceneManager.LoadScene("Scene_1");
       // CanvasScore.SetActive(true);
    }
    public void Shop()
    {
        SceneManager.LoadScene("Scene_2");
    }
    public void Menu()
    {
        SceneManager.LoadScene("Scene_0");
    }
    public void Pause()
    {
        pause.SetActive(true);
        Time.timeScale = 0;
        pauseButton.SetActive(false);

    }
    public void Play()
    {
        pause.SetActive(false);
        pauseButton.SetActive(true);
        Time.timeScale = 1;
    }
    public void UpLevel()
    {
        if (Score.TOTAL >= 10)
        {
            Level.LEVEL++;
            Score.TOTAL -= 10;
            PlayerPrefs.SetInt("LEVEL", Level.LEVEL);
            PlayerPrefs.SetInt("TOTAL", Score.TOTAL);
        }

    }
}
