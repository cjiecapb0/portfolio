using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    public static int LEVEL;
    private Text textLevel;

    private void Start()
    {
        textLevel = GetComponent<Text>();
        if (PlayerPrefs.HasKey("LEVEL"))
            LEVEL = PlayerPrefs.GetInt("LEVEL");
        else
        {
            LEVEL = 1;
            PlayerPrefs.SetInt("LEVEL", Level.LEVEL);
        }
    }
    private void Update()
    {
        textLevel.text = $"Level:{LEVEL}";
    }
}
