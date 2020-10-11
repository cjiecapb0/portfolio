using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AppleTree : MonoBehaviour
{
    [Header("Set in Inspector")]
    public GameObject applePrefab;//Шаблон для создания яблок
    public float speed = 1f;//Скорость движение яблони
    public float leftAndRightEdge = 10f;//Расстояние, на котором должно изменяться направление движения яблони
    public float chanceToChangeDirections = 0.1f;//Вероятность случайного изменение направления движения
    public float secondsBetweenAppleDrops = 1f;//Частота создания экземпляров яблок
    void Start()
    {
        //Сбрасывает яблоки раз в секунду
        Invoke("DropApple", 2f);
    }
    void DropApple()
    {
        GameObject apple = Instantiate<GameObject>(applePrefab);
        apple.transform.position = transform.position;
        Invoke("DropApple", secondsBetweenAppleDrops);
    }
    void Update()
    {
        //Простое перемещение
        Vector3 pos = transform.position;
        pos.x += speed * Time.deltaTime;
        transform.position = pos;
        //Изменение направления
        if (pos.x < -leftAndRightEdge)
            speed = Mathf.Abs(speed);
        else if (pos.x > leftAndRightEdge)
            speed = -Mathf.Abs(speed);
    }
    void FixedUpdate()
    {
        if (Random.value < chanceToChangeDirections)
            speed *= -1;
    }
}
