using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Basket : MonoBehaviour
{
    [Header("Set Dynamically")]
    public Text scoreGT;
    void Start()
    {
        GameObject scoreGO = GameObject.Find("ScoreCounter");//Получить ссылку на игровой обьект ScoreCounter
        scoreGT = scoreGO.GetComponent<Text>();//получить компонент Text этого игрового обьекта
        scoreGT.text = "0";// Установить начальное число очков равным 0
    }
    void Update()
    {
        Vector3 mousePos2D = Input.mousePosition;//Получить текущие координаты указателя мши на экране из Input        
        mousePos2D.z = -Camera.main.transform.position.z;//Координат Z камеры определяет, как далеко в трехмерном простанстве находится указатель мыши
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);//Преобразовать точку на двумерной плокости экрана в трехмерные координаты игры
        //Переместить корзину вдоль оси Х в координату Х указателя мыши
        Vector3 pos = this.transform.position;
        pos.x = mousePos3D.x;
        this.transform.position = pos;
    }
    void OnCollisionEnter(Collision coll)
    {
        GameObject collidedWith = coll.gameObject;
        if (collidedWith.tag == "Apple")
            Destroy(collidedWith);
        int score = int.Parse(scoreGT.text);//преобразовать в целое число
        score += 100;//Добавить очки
        scoreGT.text = score.ToString();//Преобразовать обратно в строку и вывести на экран
        if (score > HighScore.score)
            HighScore.score = score;// Запоминает достижение
    }
}
