using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#pragma warning disable 0649
public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private RoadSpawner roadSpawner;
    [SerializeField] private Text coinsTxt;
    [SerializeField] private Text pointTxt;
    [SerializeField] private Text highscoreTxt;
    [SerializeField] private Text pointResTxt;
    [SerializeField] private Text highscoreResTxt;

    public float MoveSpeed { get; set; }
    public float Points { get; set; }
    public static int Coins { get; set; }
    public bool CanPlay { get; set; } = true;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("HighScore"))
            Parametrs.HIGHSCORE = PlayerPrefs.GetInt("HighScore");
        else
            PlayerPrefs.SetInt("HighScore", 0);

        if (PlayerPrefs.HasKey("Coins"))
            Coins = PlayerPrefs.GetInt("Coins");
        else
            PlayerPrefs.SetInt("Coins", 0);
    }
    private void Update()
    {
        if (CanPlay)
        {
            pauseButton.SetActive(true);
            MoveSpeed = Parametrs.LVL;
            MoveSpeed += 0.1f * Time.deltaTime;

            Points += MoveSpeed * Time.deltaTime;
            if (Points > Parametrs.HIGHSCORE)
                highscoreTxt.text = "High score: " + (int)Points;
            else
                highscoreTxt.text = "High score: " + Parametrs.HIGHSCORE;
            pointTxt.text = "Score: " + ((int)Points).ToString();
        }
    }
    public void ShowResult()
    {
        pauseButton.SetActive(false);
        resultPanel.SetActive(true);
        if (Points > Parametrs.HIGHSCORE)
        {
            pointResTxt.text = "New record";
            highscoreResTxt.text = "High score: " + (int)Points;
        }
        else
        {
            pointResTxt.text = "Score: " + (int)Points;
            highscoreResTxt.text = "High score: " + (int)Parametrs.HIGHSCORE;
        }

    }
    public void AddCoins(int number)
    {
        Coins += number;
        coinsTxt.text = "Coins: " + Coins;
        PlayerPrefs.SetInt("Coins", Coins);
    }
}
