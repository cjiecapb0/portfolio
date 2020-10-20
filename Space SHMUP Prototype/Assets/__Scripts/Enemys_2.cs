using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemys_2 : Enemy
{
    [Header("Set in Inapector: Enemys_2")]
    //Определяют, насколько ярко будет выражен синусоидальный характер движения
    public float sinEccentricity = 0.6f;
    public float lifeTime = 10;
    [Header("Set Dynaically: Enemys_2")]
    public Vector3 p0;
    public Vector3 p1;
    public float birthTime;
    void Start()
    {
        //Выбрать случайюную точку на левой границе экрана
        p0 = Vector3.zero;
        p0.x = -bnbCheck.camWidth - bnbCheck.radius;
        p0.y = Random.Range(-bnbCheck.camHeight, bnbCheck.camHeight);
        //Выбрать случайную точку на правой границе экрана
        p1 = Vector3.zero;
        p1.x = bnbCheck.camWidth + bnbCheck.radius;
        p1.y = Random.Range(-bnbCheck.camHeight, bnbCheck.camHeight);
        //Случайно поменять начальную  конечную точку местами
        if (Random.value > 0.5f)
        {
            p0.x *= -1;
            p1.x *= -1;
        }
        //Записать в birthTime текущее время
        birthTime = Time.time;
    }
    public override void Move()
    {
        //Кривые Безье вычисляются на основе значения u между 0 и 1
        float u = (Time.time - birthTime) / lifeTime;
        //Если u>1, значит, корабл существует дольше, чем lifeTime
        if (u > 1)
        {   //Этот экземпляр замершил свой жизненый цикл
            Destroy(this.gameObject);
            return;
        }
        //Скорректировать u добавление значения кривой, изменяющейся посинусоиде
        u = u + sinEccentricity * (Mathf.Sin(u * Mathf.PI * 2));
        //Интерполировать местоположение между двумя точками
        pos = (1 - u) * p0 + u * p1;
    }
}
