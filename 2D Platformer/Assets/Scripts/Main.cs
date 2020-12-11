using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Text coinText;
    [SerializeField] private Text timeText;
    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite isLife, nonLife;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;
    [SerializeField] private TimeWork timeWork;
    [SerializeField] private float countDown;
    private float timer = 0f;
    private Scene activeScene;

    private void Start()
    {
        activeScene = SceneManager.GetActiveScene();
        if ((int)timeWork == 2)
            timer = countDown;
    }
    private void Update()
    {
        coinText.text = player.Coins.ToString();
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].sprite = player.CurHP > i ? isLife : nonLife;
        }
        if ((int)timeWork == 1)
        {
            timer += Time.deltaTime;
            timeText.text = timer.ToString("F2").Replace(",", ":");
        }
        else if ((int)timeWork == 2)
        {
            timer -= Time.deltaTime;
            //timeText.text = timer.ToString("F2").Replace(",", ":");
            timeText.text = ((int)timer / 60).ToString("D2") + ":" + ((int)timer % 60).ToString("D2");
            if (timer <= 0)
                Lose();
        }
        else
            timeText.gameObject.SetActive(false);
    }
    public void ReloadLvl()
    {
        Time.timeScale = 1f;
        player.enabled = true;
        SceneManager.LoadScene(activeScene.name);
    }
    public void PauseOn()
    {
        Time.timeScale = 0f;
        player.enabled = false;
        pauseScreen.SetActive(true);
    }
    public void PauseOff()
    {
        Time.timeScale = 1f;
        player.enabled = true;
        pauseScreen.SetActive(false);
    }
    public void Win()
    {
        Time.timeScale = 0f;
        player.enabled = false;
        winScreen.SetActive(true);
        if (!PlayerPrefs.HasKey("Lvl") || PlayerPrefs.GetInt("Lvl") < activeScene.buildIndex)
            PlayerPrefs.SetInt("Lvl", activeScene.buildIndex);
        if (PlayerPrefs.HasKey("coins"))
            PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + player.Coins);
        else
            PlayerPrefs.SetInt("coins", player.Coins);
    }
    public void Lose()
    {
        Time.timeScale = 0f;
        player.enabled = false;
        loseScreen.SetActive(true);
    }
    public void MenuLvl()
    {
        SceneManager.LoadScene("Menu");
    }
    public void NextLvl()
    {
        Time.timeScale = 1f;
        player.enabled = true;
        SceneManager.LoadScene(activeScene.buildIndex + 1);
    }
}
public enum TimeWork
{
    None,
    StopWatch,
    Timer
}
