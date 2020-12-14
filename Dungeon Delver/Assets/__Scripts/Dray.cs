using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dray : MonoBehaviour
{
    public enum eMode { idle, move, attack, transition }

    [Header("Set in Inspactor")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float attackDuration = 0.25f;//Продолжительность атаки в секундах
    [SerializeField] private float attackDelay = 0.5f;//Задержка между атаками

    [Header("Set Dynamically")]
    [SerializeField] private int dirHeld = -1; //Направление соответсвующее удериваемой клавиши
    [SerializeField] private int facing = 1; //Направление движения Дрей
    [SerializeField] private eMode mode = eMode.idle;
    private float timeAtkDone = 0;
    private float timeAtkNext = 0;
    public int Facing => facing;
    public eMode Mode => mode;

    private Rigidbody rigid;
    private Animator anim;

    private Vector3[] directions = new Vector3[] {
        Vector3.right, Vector3.up, Vector3.left, Vector3.down};
    private KeyCode[] keys = new KeyCode[] {
        KeyCode.RightArrow, KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.DownArrow};
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }
    private void Update()
    {   
        //------Обработка ввода с клавиатуры и управление режимами eMode------
        dirHeld = -1;
        for (int i = 0; i < keys.Length; i++)
        {
            if (Input.GetKey(keys[i])) dirHeld = i;
        }
        //Нажата клавиша атаки
        if(Input.GetKeyDown(KeyCode.Z) && Time.time >= timeAtkNext)
        {
            mode = eMode.attack;
            timeAtkDone = Time.time + attackDuration;
            timeAtkNext = Time.time + attackDelay;
        }
        //Завершить атаку, если время истекло
        if (Time.time >= timeAtkDone)
            mode = eMode.idle;

        //Выбрать правильный режим. если Дрей не атакует
        if (mode != eMode.attack) {
            if (dirHeld ==-1)
                mode = eMode.idle;
            else
            {
                facing = dirHeld;
                mode = eMode.move;
            }
        }
        //-----Действие в текущем режиме----
        Vector3 vel = Vector3.zero;
        switch (mode)
        {
            case eMode.attack:
                anim.CrossFade("Dray_Attack_" + facing, 0);
                anim.speed = 0;
                break;
            case eMode.idle:
                anim.CrossFade("Dray_Walk_" + facing, 0);
                anim.speed = 0;
                break;
            case eMode.move:
                vel = directions[dirHeld];
                anim.CrossFade("Dray_Walk_" + facing, 0);
                anim.speed = 1;
                break;
        }
        rigid.velocity = vel * speed;
    }
}
