using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddScore : MonoBehaviour
{
    public static int level;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        level = PlayerPrefs.GetInt("LEVEL");
        Score.SCORE += level;
        Score.TOTAL += level;
        PlayerPrefs.SetInt("TOTAL", Score.TOTAL);
        Destroy(this.gameObject);
    }
}
