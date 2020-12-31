using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected static Vector3[] directions = new Vector3[] {
        Vector3.right, Vector3.up, Vector3.left, Vector3.down};

    [Header("Set in Inspector: Enemy")]
    [SerializeField] private float maxHealth = 1f;
    [SerializeField] private float knockbackSpeed = 10f;
    [SerializeField] private float knockbackDuration = 0.25f;
    [SerializeField] private float invincibleDuration = 0.5f;
    [SerializeField] private GameObject[] randomItemDrops;
    [SerializeField] private GameObject guaranteedItemDrop = null;

    [Header("Set Dynamically")]
    [SerializeField] private float health;
    [SerializeField] private bool invincible = false;
    [SerializeField] private bool knockback = false;
    public bool Knockback => knockback;

    private float invincibleDone = 0;
    private float knockbackDone = 0;
    private Vector3 knockbackVel;

    protected Animator anim;
    protected Rigidbody rigid;
    protected SpriteRenderer sRend;

    protected virtual void Awake()
    {
        health = maxHealth;
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        sRend = GetComponent<SpriteRenderer>();
    }
    protected virtual void Update()
    {
        //Провверить состояние неуязвимости и необходимость выполнить отскок
        if (invincible && Time.time > invincibleDone) invincible = false;
        sRend.color = invincible ? Color.red : Color.white;
        if (knockback)
        {
            rigid.velocity = knockbackVel;
            if (Time.time < knockbackDone) return;
        }
        anim.speed = 1;
        knockback = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (invincible) return;//Выйти, если Дрей пока неуязвим
        DamageEffect dEf = other.gameObject.GetComponent<DamageEffect>();
        if (dEf == null) return;//Если компонент DamageEffect отсутствует - выйти

        health -= dEf.Damage;//Вычесть величину ущерба из уровня здоровья
        if (health <= 0) Die();
        invincible = true;//Сделать Дрея неуязвимым
        invincibleDone = Time.time + invincibleDuration;

        if (dEf.Knockback)//Выполнить отбрасывание
        { //Определить направление отбрасывания
            Vector3 delta = transform.position = other.transform.root.position;
            if (Mathf.Abs(delta.x) >= Mathf.Abs(delta.y))
            {
                //Отбрасывание по горизонтали
                delta.x = (delta.x > 0) ? 1 : -1;
                delta.y = 0;
            }
            else
            {
                delta.x = 0;
                delta.y = (delta.y > 0) ? 1 : -1;
            }
            //Применить скорость оскока к компоненту Rigidbody
            knockbackVel = delta * knockbackSpeed;
            rigid.velocity = knockbackVel;
            //Установить режим knockback и время прекращения отбрасывания
            knockback = true;
            knockbackDone = Time.time + knockbackDuration;
            anim.speed = 0;

        }
    }
    private void Die()
    {
        GameObject go;
        if (guaranteedItemDrop != null)
        {
            go = Instantiate<GameObject>(guaranteedItemDrop);
            go.transform.position = transform.position;
        }
        else if (randomItemDrops.Length > 0)
        {
            int n = Random.Range(0, randomItemDrops.Length);
            GameObject prefab = randomItemDrops[n];
            if (prefab != null)
            {
                go = Instantiate<GameObject>(prefab);
                go.transform.position = transform.position;
            }
        }
        Destroy(gameObject);
    }
}
