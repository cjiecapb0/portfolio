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
    public float showDamageDuration = 0.1f;//Длительность эффекта попадания в секундах
    public float powerUpDropChance = 1f;//Вероятность сбросить бонус
    [Header("Set Dinamically: Enemy")]
    public Color[] originalColors;
    public Material[] materials;//Все материалы игрового объекта и его потомков
    public bool showingDamage = false;
    public float damageDoneTime;// Время прекращения отображения эффекта
    public bool notifiedOfDestruction = false;
    protected BoundsCheck bnbCheck;
    private void Awake()
    {
        bnbCheck = GetComponent<BoundsCheck>();
        //получить материалы и цвет этого игрового объекта и его потомков
        materials = Utils.GetAllMaterials(gameObject);
        originalColors = new Color[materials.Length];
        for (int i = 0; i < materials.Length; i++)
        {
            originalColors[i] = materials[i].color;
        }
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
        if (showingDamage && Time.time > damageDoneTime)
            UnShowDamage();
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
        GameObject otherGO = coll.gameObject;
        switch (otherGO.tag)
        {
            case "ProjectileHero":
                Projetile p = otherGO.GetComponent<Projetile>();
                //Если вражеский корабль за границами экрана,не наносить повреждений
                if (!bnbCheck.isOnScreen)
                {
                    Destroy(otherGO);
                    break;
                }
                //Празить вражеский корабль
                ShowDamage();
                //Получить разрушающую силу из WEAP_DICT в класс Main.
                health -= Main.GetWeaponDefinition(p.type).damageOnHit;
                if (health <= 0)
                {
                    //Сообщить объекту одиночке Main об уничтожении
                    if (!notifiedOfDestruction)
                        Main.S.ShipDestroyed(this);
                    notifiedOfDestruction = true;
                    //Уничтожить этот вражеский корабль
                    Destroy(this.gameObject);
                }
                Destroy(otherGO);
                break;
            default:
                print("Enemy hit by non-ProjectileHero: " + otherGO.name);
                break;
        }
    }

    private void ShowDamage()
    {
        foreach (Material m in materials)
            m.color = Color.red;
        showingDamage = true;
        damageDoneTime = Time.time + showDamageDuration;
    }

    private void UnShowDamage()
    {
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].color = originalColors[i];
        }
        showingDamage = false;
    }
}
