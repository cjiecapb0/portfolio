using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public AudioSource audio;
    Button[,] buttons;
    Image[] images;
    Lines lines;
    void Start()
    {
        lines = new Lines(ShowBox, PlayCut);
        InitButtons();
        InitImages();
        ShowBox(1, 2, 3);
        lines.Start();
    }
    public void ShowBox(int x, int y, int ball)
    {
        buttons[x, y].GetComponent<Image>().sprite = images[ball].sprite;
    }
    public void PlayCut()
    {
        audio.Play();
    }
    public void Click()
    {
        string name = EventSystem.current.currentSelectedGameObject.name;
        int nr = GetNumber(name);
        int x = nr % Lines.SIZE;
        int y = nr / Lines.SIZE;
        Debug.Log($"clicked + {name} {x} {y}");
        lines.Click(x, y);
    }
    private void InitButtons()
    {
        buttons = new Button[Lines.SIZE, Lines.SIZE];
        for (int nr = 0; nr < Lines.SIZE * Lines.SIZE; nr++)
        {
            buttons[nr % Lines.SIZE, nr / Lines.SIZE] =
                GameObject.Find($"Button ({nr})").GetComponent<Button>();
        }
    }
    private void InitImages()
    {
        images = new Image[Lines.BALLS];
        for (int j = 0; j < Lines.BALLS; j++)
        {
            images[j] = GameObject.Find($"Image ({j})").GetComponent<Image>();
        }
    }
    private int GetNumber(string name)
    {
        Regex regex = new Regex("\\((\\d+)\\)");
        Match match = regex.Match(name);
        if (!match.Success)
            throw new Exception("Unrecognized object name");
        Group group = match.Groups[1];
        string number = group.Value;
        return Convert.ToInt32(number);
    }
}
