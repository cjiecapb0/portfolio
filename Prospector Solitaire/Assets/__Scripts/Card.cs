using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    [Header("Set Dynamically")]
    public string suit;//Масть карты (C,D,H,S)
    public int rank;//Достоинство карты (1-14)
    public Color color = Color.black;//Цвет значков
    public string colS = "Black";//Имя цвета
    //Этот список хранит все игровые объекты Decorator
    public List<GameObject> decoGOs = new List<GameObject>();
    //Этот список хранит все игровые объекты Pip
    public List<GameObject> pipGOs = new List<GameObject>();
    public GameObject back;//Игровой объект рубашки карты
    public CardDefinition def;//Извлекается из DeckXML.xml
}
[System.Serializable]//Сериализуемый класс доступен дл правки в инспекторе
public class Decorator
{//Этот класс хрнаит инфрмаию из DeckXML о каждом значке на карте
    public string type;//Значок определяющий достоинство карты, имеет type = "pip"
    public Vector3 loc;//Местоположение спрайта на карте
    public bool flip = false;//Признак переворота спрайта по вертикали
    public float scale = 1f;// Маштаб спрайта
}
[System.Serializable]
public class CardDefinition
{//Этот класс хранит информацию о достоинстве карты
    public string face;//Спрайт, изображающий лицевую сторону карты
    public int rank;//Досоинство карты(1-13)
    public List<Decorator> pips = new List<Decorator>();//Значки
}
