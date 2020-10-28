using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Deck : MonoBehaviour
{
    [Header("Set in Inspector")]
    //Масти
    public Sprite suitClub;
    public Sprite suitDamond;
    public Sprite suitHeart;
    public Sprite suitSpade;

    public Sprite[] faceSprites;
    public Sprite[] rankSprites;
    public Sprite cardBack;
    public Sprite cardBackGold;
    public Sprite cardFront;
    public Sprite cardFrontGold;
    //Шаблоны
    public GameObject prefabCard;
    public GameObject prefabSprite;
    [Header("Set Dynamically")]
    public PT_XMLReader xmlr;
    public List<string> cardNames;
    public List<Card> cards;
    public List<Decorator> decorators;
    public List<CardDefinition> cardDefs;
    public Transform deckAnchor;
    public Dictionary<string, Sprite> dictSuits;
    //InitDeck вызывается экземпляром Prospector, когда будет готов
    public void InitDeck(string deckXMLText)
    {
        //Создать точку привязки для все игровых объектов Card в иерархии
        if (GameObject.Find("_Deck") == null)
        {
            GameObject anchorGO = new GameObject("_Deck");
            deckAnchor = anchorGO.transform;
        }
        //Инициализировать словарь со спрайтами значков мастей
        dictSuits = new Dictionary<string, Sprite>()
        {
            {"C",suitClub},
            {"D",suitDamond},
            {"H",suitHeart},
            {"S",suitSpade}
        };
        ReadDeck(deckXMLText);
        MakeCards();
    }
    //ReadDeck читает указынный XML-файл и создает массив экземпляров CardDefinition
    public void ReadDeck(string deckXMLText)
    {
        xmlr = new PT_XMLReader();//Создать новый экземпляр PT_XMLReader
        xmlr.Parse(deckXMLText);// Использовать его для чтения DeckXML
        //Вывод проверичной строки, чтобы показать, как использовать xmlr
        string s = "xml[0] decorator [0] ";
        s += "type=" + xmlr.xml["xml"][0]["decorator"][0].att("type");
        s += " x=" + xmlr.xml["xml"][0]["decorator"][0].att("x");
        s += " y=" + xmlr.xml["xml"][0]["decorator"][0].att("y");
        s += " scale=" + xmlr.xml["xml"][0]["decorator"][0].att("scale");
        //print(s);
        //Прочиайте элементы <decorator> для всех карт
        decorators = new List<Decorator>();//инициализировать
        //Извлечь список PT_XMLHashList всех элементов <decorator> из XML-файла
        PT_XMLHashList xDecos = xmlr.xml["xml"][0]["decorator"];
        Decorator deco;
        for (int i = 0; i < xDecos.Count; i++)
        {
            //Для каждго элемента <decorator> в XML
            deco = new Decorator
            {
                //Скопировать атрибуты из <decorator> в Decorator
                type = xDecos[i].att("type"),
                //deco.flip получит значение true, если атрибут flip содежрит текст "1"
                flip = (xDecos[i].att("flip") == "1"),
                //Получить значение float из строковых атрибутов
                scale = float.Parse(xDecos[i].att("scale"))
            };//создать экземпляр Decorator
            //Vector3 loc инициализируется как [0,0,0], потому нам остается только изменить его
            deco.loc.x = float.Parse(xDecos[i].att("x"));
            deco.loc.y = float.Parse(xDecos[i].att("y"));
            deco.loc.z = float.Parse(xDecos[i].att("z"));
            //Добавить deco в список decorators
            decorators.Add(deco);
        }
        //Прочитать координаты для значков,определяющих достоинство карты
        cardDefs = new List<CardDefinition>();//Инициализировать список карт
                                              //Извлечь список PT_XMLHashList всех элементов <card> из XML-файла
        PT_XMLHashList xCardDefs = xmlr.xml["xml"][0]["card"];
        for (int i = 0; i < xCardDefs.Count; i++)
        {
            //Для каждого элемента  <card> создать экземпляр CardDefinition
            CardDefinition cDef = new CardDefinition
            {
                //Получить значение атрибута и добавить их в cDef
                rank = int.Parse(xCardDefs[i].att("rank"))
            };
            //Извлечь список PT_XMLHashList всех элементов <pip> внутри этого элемента <card>
            PT_XMLHashList xPips = xCardDefs[i]["pip"];
            if (xPips != null)
            {
                for (int j = 0; j < xPips.Count; j++)
                {
                    //Обойти все элемнты <pip>
                    deco = new Decorator
                    {
                        //Элементы <pip> в <card> обрабатываются классом Decorator
                        type = "pip",
                        flip = (xPips[j].att("flip") == "1")
                    };
                    deco.loc.x = float.Parse(xPips[j].att("x"));
                    deco.loc.y = float.Parse(xPips[j].att("y"));
                    deco.loc.z = float.Parse(xPips[j].att("z"));
                    if (xPips[j].HasAtt("scale"))
                    {
                        deco.scale = float.Parse(xPips[j].att("scale"));
                    }
                    cDef.pips.Add(deco);
                }
            }
            //Карты с картинками (Валет,Дама,Король) имеют артибут face
            if (xCardDefs[i].HasAtt("face"))
            {
                cDef.face = xCardDefs[i].att("face");
            }
            cardDefs.Add(cDef);
        }
    }
    //Получает CardDefinition на основе значения достоинства (от 1 до 14 - от туза до короля)
    public CardDefinition GetCardDefinitionByRank(int rnk)
    {
        //Поиск во всех определениях CardDefinition
        foreach (CardDefinition cd in cardDefs)
        {
            //Если достинство совпадает, вернуть это определение
            if (cd.rank == rnk)
            {
                return (cd);
            }
        }
        return (null);
    }
    //создает игровые объекты карт
    public void MakeCards()
    {
        //cardNames будет содержать имена сконструированных карт
        //каждая масть имеет значений достоинтва (например для треф (Clubs): от C1 до С14)
        cardNames = new List<string>();
        string[] letters = new string[] { "C", "D", "H", "S" };
        foreach (string s in letters)
        {
            for (int i = 0; i < 13; i++)
            {
                cardNames.Add(s + (i + 1));
            }
        }
        //Создать список со всеми картами
        cards = new List<Card>();
        //обойти все только что созданные имена карт
        for (int i = 0; i < cardNames.Count; i++)
        {
            //Создать карту и добавить ее в колоду
            cards.Add(MakeCard(i));
        }
    }
    private Card MakeCard(int cNum)
    {
        //создать новый игровой объект с картой
        GameObject cgo = Instantiate(prefabCard) as GameObject;
        //Настроить transform.parent новой карты в соотвесвии с точкой привязки.
        cgo.transform.parent = deckAnchor;
        Card card = cgo.GetComponent<Card>();//Получить компонент Card
        //Эта строка выкладывает карты в аккуратный ряд
        cgo.transform.localPosition = new Vector3((cNum % 13) * 3, cNum / 13 * 4, 0);
        //Настроит основные параметры карты
        card.name = cardNames[cNum];
        card.suit = card.name[0].ToString();
        card.rank = int.Parse(card.name.Substring(1));
        if (card.suit == "D" || card.suit == "H")
        {
            card.colS = "Red";
            card.color = Color.red;
        }
        //получить CardDefinition для этой карты
        card.def = GetCardDefinitionByRank(card.rank);
        AddDecorators(card);
        return card;
    }
    //Следующие скрытые переменные используются вспомогательными методами
    private Sprite _tSp = null;
    private GameObject _tGO = null;
    private SpriteRenderer _tSR = null;
    private void AddDecorators(Card card)
    {
        //Добавить оформление
        foreach (Decorator deco in decorators)
        {
            if (deco.type == "suit")
            {
                //Создать экземпляр игрового объекта спрайта
                _tGO = Instantiate(prefabSprite) as GameObject;
                //Получить ссылку на компонент SpriteRenderer
                _tSR = _tGO.GetComponent<SpriteRenderer>();
                //Установить спрайт масти
                _tSR.sprite = dictSuits[card.suit];
            }
            else
            {
                _tGO = Instantiate(prefabSprite) as GameObject;
                _tSR = _tGO.GetComponent<SpriteRenderer>();
                //получить спрайт для отображения достоинства
                _tSp = rankSprites[card.rank];
                //Установить спрайт достоинства в SpriteRenderer
                _tSR.sprite = _tSp;
                //Установить цвт, соответствующий масти
                _tSR.color = card.color;
            }
            //Поместить спрайты над картой
            _tSR.sortingOrder = 1;
            //Сделать спрайт дочерним по отношению к карте
            _tGO.transform.SetParent(card.transform);
            //Установить localPosition, как определено в DeckXML
            _tGO.transform.localPosition = deco.loc;
            //Перевернуть значок, если неободимо
            if (deco.flip)
            {
                //Эйлеров поворот на 180 относительно оси Z-axis
                _tGO.transform.rotation = Quaternion.Euler(0, 0, 180);
            }
            //Установить масштаб, чтобы уменьшить размер спрайта
            if (deco.scale != 1)
            {
                _tGO.transform.localScale = Vector3.one * deco.scale;
            }
            //Дать имя этому игровому объекту для нагляности
            _tGO.name = deco.type;
            //Добавить этот игровой объект с офорлением в список card.decoGOs
            card.decoGOs.Add(_tGO);
        }
    }
}

