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
    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite isLife, nonLife;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;
    private void Update()
    {
        coinText.text = player.Coins.ToString();
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].sprite = player.CurHP > i ? isLife : nonLife;
        }
    }
    public void ReloadLvl()
    {
        Time.timeScale = 1f;
        player.enabled = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
        player.enabled = true;
        winScreen.SetActive(true);
    }    
    public void Lose()
    {
        Time.timeScale = 0f;
        player.enabled = true;
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
