using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    static public Hero S;//одиночка
    [Header("Set in Inspector")]
    //поля, управляющие движением коробля
    public float speed = 30;
    public float rollMult = -45;
    public float pitchMult = 30;
    public float gameRestartDelay = 2f;
    public GameObject projetilePrefab;
    public float projetileSpeed = 40;
    [Header("Set Dynamically")]
    [SerializeField]
    private float _shieldLevel = 1;
    //эта переменная хранит ссылку на последний столкнувшийся игровой объект
    private GameObject lastTriggerGo = null;
    //Объявление нового делегата типа WeaponFireDelegate
    public delegate void WeaponFireDelegate();
    //Создать поле типа WeaponFireDelegate с именем fireDelegate.
    public WeaponFireDelegate fireDelegate;
    private void Awake()
    {
        if (S == null)
            S = this;//сохранить ссылку на одиночку
        else
            Debug.LogError("Hero.Awake() - Attemted to assign second Hero.S");
        fireDelegate += TempFire;
    }
    private void Update()
    {
        //извлечь информацию из класса Input
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");
        //изменить transform.position, опираясь на информацию по осям
        Vector3 pos = transform.position;
        pos.x += xAxis * speed * Time.deltaTime;
        pos.y += yAxis * speed * Time.deltaTime;
        transform.position = pos;
        //повернуть корабль, чтобы придать ощущение динамики
        transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0);
        //произвести выстрел из всех видов оружия вызовом fireDelegate
        //сначала проверить нажатие клавиши: Axis("Jump")
        //Затем убедиться , что значение fireDelegate не равно null, что бы избежать ошибки
        if (Input.GetAxis("Jump") == 1 && fireDelegate != null)
            fireDelegate();
    }
    public void TempFire()
    {
        GameObject projGO = Instantiate<GameObject>(projetilePrefab);
        projGO.transform.position = transform.position;
        Rigidbody rigidB = projGO.GetComponent<Rigidbody>();

        Projetile proj = projGO.GetComponent<Projetile>();
        proj.type = WeaponType.blaster;
        float tSpeed = Main.GetWeaponDefinition(proj.type).velocity;
        rigidB.velocity = Vector3.up * tSpeed;
    }
    private void OnTriggerEnter(Collider other)
    {
        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject;
        //print("Triggered: " + go.name);
        //гарантировать невозможность повторного столкновения с тем же объектом
        if (go == lastTriggerGo)
            return;
        lastTriggerGo = go;
        if (go.tag == "Enemy")
        {
            shieldLevel--;
            Destroy(go);
        }
        else
            print("Triggered by non-Emery: " + go.name);
    }
    public float shieldLevel
    {
        get
        {
            return (_shieldLevel);
        }
        set
        {
            _shieldLevel = Mathf.Min(value, 4);
            if (value < 0)
                Destroy(this.gameObject);
            //сообщить объекту Main.S о необходимости перезапустить игру
            Main.S.DelayedRestart(gameRestartDelay);
        }
    }
}
