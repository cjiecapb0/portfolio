using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameMode
{
    idle,
    playing,
    levelEnd
}
public class MissionDemolition : MonoBehaviour
{
    static private MissionDemolition S;//скрытый объект одиночка

    [Header("Set in Inspector")]
    public Text uitLevel;// ссылка на объект UIText_Level
    public Text uitShots;// ссылка на объект UIText_Shots
    public Text uitButton;// ссылка на дочерний объект Text в UIButton_View
    public Vector3 castlePos;//местоположение замка
    public GameObject[] castles;// массив замков

    [Header("Set Dynamically")]
    public int level;//текущий уровень
    public int levelMax;//количество уровней
    public int shotsTaken;
    public GameObject castle;//текущий замок
    public GameMode mode = GameMode.idle;
    public string showing = "Show Slingshot";//режим FollowCam
    private void Start()
    {
        S = this;//определить объект одиночку
        level = 0;
        levelMax = castles.Length;
        StartLevel();
    }
    private void StartLevel()
    {   //уничтожить прежний замок если он существует
        if (castle != null)
            Destroy(castle);
        //уничтожить прежние снаряды если они существуют
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (GameObject pTemp in gos)
            Destroy(pTemp);
        //создать новый замок
        castle = Instantiate<GameObject>(castles[level]);
        castle.transform.position = castlePos;
        shotsTaken = 0;
        //переустановить камеру в начальную позицию
        SwitchView("Show Both");
        ProjectileLine.S.Clear();
        //сбросить цель
        Goal.goalMet = false;
        UpdeteGUI();
        mode = GameMode.playing;
    }

    private void UpdeteGUI()
    {
        //показывать данные в элементах ПИ
        uitLevel.text = "Level:" + (level + 1) + " of " + levelMax;
        uitShots.text = "Shots Taken: " + shotsTaken;
    }
    private void Update()
    {
        UpdeteGUI();
        //проверить завершение уровня
        if ((mode == GameMode.playing)&&Goal.goalMet)
        {
            //изменить режим, что бы прекратить проверку завершения уровня
            mode = GameMode.levelEnd;
            //Уменьшить маштаб
            SwitchView("Show Bolt");
            //Начать новый уровень через 2 секунды
            Invoke("NextLevel", 2f);
        }
    }
    void NextLevel()
    {
        level++;
        if (level == levelMax)
            level = 0;
        StartLevel();
    }
    public void SwitchView(string eView = "")
    {
        if (eView == "")
            eView = uitButton.text;
        showing = eView;
        switch (showing)
        {
            case "Show Slingshot":
                FollowCam.POI = null;
                uitButton.text = "Show Castle";
                break;
            case "Show Castle":
                FollowCam.POI = S.castle;
                uitButton.text = "Show Both";
                break;
            case "Show Both":
                FollowCam.POI = GameObject.Find("ViewBoth");
                uitButton.text = "Show Slingshot";
                break;
        }
    }
    //статический метод, позволяющий из любого кода увелиить shotsTaken
    public static void ShotFired()
    {
        S.shotsTaken++;
    }
}
