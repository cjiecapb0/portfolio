using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Coins : MonoBehaviour
{
    private Text textCoins;
    private static int coins;
    private void Start()
    {
        textCoins = GetComponent<Text>();
    }
    private void Update()
    {
        coins = Score.TOTAL;
        textCoins.text = $"Coins:{coins}";
    }
}
