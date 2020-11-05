using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public static int SCORE;
    public static int TOTAL;
    private Text textScore;
    private Text textTotal;
    public Text score;
    public Text total;

    private void Start()
    {
        textScore = GetComponent<Text>();
        textTotal = total.GetComponent<Text>();
        SCORE = 0;
        if (PlayerPrefs.HasKey("TOTAL"))
            TOTAL = PlayerPrefs.GetInt("TOTAL");
        else TOTAL = 0;
    }
    private void Update()
    {
        textScore.text = SCORE.ToString();
        score.text = $"Score:{SCORE}";
        total.text = $"Total coins:{TOTAL}";
    }
}
