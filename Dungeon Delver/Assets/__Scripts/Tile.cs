﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [Header("Set Dynamically")]
    public int x;
    public int y;
    public int tileNum;

    private BoxCollider bColl;
    private void Awake()
    {
        bColl = GetComponent<BoxCollider>();
    }
    public void SetTile(int eX, int eY, int eTileNum = -1)
    {
        x = eX;
        y = eY;
        transform.localPosition = new Vector3(x, y, 0);
        gameObject.name = x.ToString("D3") + "x" + y.ToString("D3");
        if (eTileNum == -1)
            eTileNum = TileCamera.GET_MAP(x, y);
        else
            TileCamera.SET_MAP(x, y, eTileNum);//Заменить плитку, если необходимо
        tileNum = eTileNum;
        GetComponent<SpriteRenderer>().sprite = TileCamera.SPRITES[tileNum];
        SetCollider();
    }
    private void SetCollider()
    {
        //Получить информацию о коллайдера из Collider DelverCollisions.txt
        bColl.enabled = true;
        char c = TileCamera.COLLISIONS[tileNum];
        switch (c)
        {
            case 'S'://Вся плитка
                bColl.center = Vector3.zero;
                bColl.size = Vector3.one;
                break;
            case 'W'://Верхняя половина
                bColl.center = new Vector3(0, 0.25f, 0);
                bColl.size = new Vector3(1f, 0.5f, 1f);
                break;
            case 'A'://Левая половина
                bColl.center = new Vector3(-0.25f, 0, 0);
                bColl.size = new Vector3(0.5f, 1f, 1f);
                break;
            case 'D'://правая половина
                bColl.center = new Vector3(0.25f, 0, 0);
                bColl.size = new Vector3(0.5f, 1f, 1f);
                break;
            //vvvvvvvvvvvvvv-------Дополнительные коды--------vvvvvvvvvvvv
            case 'Q'://Левая верхняя четверть
                bColl.center = new Vector3(-0.25f, 0.25f, 0);
                bColl.size = new Vector3(0.5f, 0.5f, 1);
                break;
            case 'E'://Правая верхняя четверть
                bColl.center = new Vector3(0.25f, 0.25f, 0);
                bColl.size = new Vector3(0.5f, 0.5f, 1);
                break;
            case 'Z'://Левая нижняя четверть
                bColl.center = new Vector3(-0.25f, -0.25f, 0);
                bColl.size = new Vector3(0.5f, 0.5f, 1);
                break;
            case 'X'://Нижняя половина
                bColl.center = new Vector3(0, -0.25f, 0);
                bColl.size = new Vector3(1f, 0.5f, 1);
                break;
            case 'C'://Правая нижняя четверть
                bColl.center = new Vector3(0.25f, -0.25f, 0);
                bColl.size = new Vector3(0.5f, 0.5f, 1);
                break;
            //^^^^^^^^^^-------Дополнительные коды--------^^^^^^^^^^^
            default: //Все остальное: _,|, и др.
                bColl.enabled = false;
                break;
        }
    }
}
