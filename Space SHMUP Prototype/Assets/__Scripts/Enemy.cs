using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Enemy : MonoBehaviour
{
    [Header("Set in Inspector")]
    public float speed = 10f;//скорость в м/с
    public float fireRate = 0.3f;//секунд между выстрелами(не испольуется)
    public float health = 10;
    public int score = 100;//очки за уничтожние этого корабля
    private BoundsCheck bnbCheck;
    private void Awake()
    {
        bnbCheck = GetComponent<BoundsCheck>();
    }
    //это свойство: метод действующий как поле
    public Vector3 pos
    {
        get
        {
            return (this.transform.position);
        }
        set
        {
            this.transform.position = value;
        }
    }
    private void Update()
    {
        Move();
        if (bnbCheck != null && bnbCheck.offDown)
                //Корабль за нижней границей, поэтому его нужно уничтожить
                Destroy(gameObject);

    }

    public virtual void Move()
    {
        Vector3 tempPos = pos;
        tempPos.y -= speed * Time.deltaTime;
        pos = tempPos;
    }
    private void OnCollisionEnter(Collision coll)
    {
        GameObject otherGo = coll.gameObject;
        if (otherGo.tag == "ProjectileHero")
        {
            Destroy(otherGo);//уничтожить снаряд
            Destroy(gameObject);//уничтожить игровой объект
        }
        else
            print("Enemy hit by non-ProjectileHero: " + otherGo.name);
    }
}
