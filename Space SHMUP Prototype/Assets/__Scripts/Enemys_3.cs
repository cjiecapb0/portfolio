using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemys_3 : Enemy
{
    //Траекория движение Enemy_3 вычисляется путём линейной интерполяции кривой Безье по более двум точкам
    [Header("Set in Inspector: Enemy_3")]
    public float lifeTime = 5;
    [Header("Set Dynamicaly: Enemy_3")]
    public Vector3[] points;
    public float birthTime;
    //Метод Start() хорош подходит так как не ипользыется в Enemy
    void Start()
    {
        points = new Vector3[3];
        //Начальная позиция уже определена в Main.SpawnEnemy()
        points[0] = pos;
        //Установить xMin и xMax так же, как это делает Main.SpawnEnemy()
        float xMin = -bnbCheck.camWidth + bnbCheck.radius;
        float xMax = bnbCheck.camWidth - bnbCheck.radius;
        Vector3 v;
        //Случайно выбрать среднию точку нижней границы экрана
        v = Vector3.zero;
        v.x = Random.Range(xMin, xMax);
        v.y = -bnbCheck.camHeight * Random.Range(2.75f, 2);
        points[1] = v;
        //Случайно выбрать конечную точку выше верхней границы экрана
        v = Vector3.zero;
        v.y = pos.y;
        v.x = Random.Range(xMin, xMax);
        points[2] = v;
        //Записать в birthTime текущее время
        birthTime = Time.time;
    }
    public override void Move()
    {
        //Кривые Безье вычисляются на основе значения u между 0 и 1
        float u = (Time.time - birthTime) / lifeTime;
        if (u > 1)
        {
            Destroy(this.gameObject);
            return;
        }
        //Интерполировать кривую Безье по трем точкам
        Vector3 p01, p12;
        u = u - 0.2f * Mathf.Sin(u * Mathf.PI * 2);
        p01 = (1 - u) * points[0] + u * points[1];
        p12 = (1 - u) * points[1] + u * points[2];
        pos = (1 - u) * p01 + u * p12;
    }
}
