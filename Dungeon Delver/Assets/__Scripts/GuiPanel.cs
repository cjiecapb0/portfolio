﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuiPanel : MonoBehaviour
{
    [SerializeField] private Dray dray;
    [SerializeField] private Sprite healthEmpty;
    [SerializeField] private Sprite healthHalf;
    [SerializeField] private Sprite healthFull;

    private Text keyCountText;
    private List<Image> healthImages;

    private void Start()
    {
        //Счетчик ключей
        Transform trans = transform.Find("Key Count");
        keyCountText = trans.GetComponent<Text>();

        //Индикатор уровня здоровья
        Transform healthPanel = transform.Find("Health Panel");
        healthImages = new List<Image>();
        if (healthPanel != null)
        {
            for (int i = 0; i < 20; i++)
            {
                trans = healthPanel.Find("H_" + i);
                if (trans == null) break;
                healthImages.Add(trans.GetComponent<Image>());
            }
        }
    }
    private void Update()
    {
        //Показать количество ключей
        keyCountText.text = dray.keyCount.ToString();

        //Показать уровень здоровья
        int health = dray.Health;
        for (int i = 0; i < healthImages.Count; i++)
        {
            if (health > 1)
                healthImages[i].sprite = healthFull;
            else if (health == 1)
                healthImages[i].sprite = healthHalf;
            else
                healthImages[i].sprite = healthEmpty;
            health -= 2;
        }
    }
}
