using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dray : MonoBehaviour, IFacingMover, IKeyMaster
{
    public enum eMode { idle, move, attack, transition }

    [Header("Set in Inspactor")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float attackDuration = 0.25f;//Продолжительность атаки в секундах
    [SerializeField] private float attackDelay = 0.5f;//Задержка между атаками
    [SerializeField] private float transitionDelay = 0.5f;//Задержка перехода между комнатами

    [Header("Set Dynamically")]
    [SerializeField] private int dirHeld = -1; //Направление соответсвующее удериваемой клавиши
    [SerializeField] private int numKeys = 0;
    private int facing = 1; //Направление движения Дрей
    private eMode mode = eMode.idle;

    private float timeAtkDone = 0;
    private float timeAtkNext = 0;

    private float transitionDone = 0;
    private Vector2 transitionPos;

    public eMode Mode => mode;

    private Rigidbody rigid;
    private Animator anim;
    private InRoom inRm;

    private Vector3[] directions = new Vector3[] {
        Vector3.right, Vector3.up, Vector3.left, Vector3.down};
    private KeyCode[] keys = new KeyCode[] {
        KeyCode.RightArrow, KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.DownArrow};
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        inRm = GetComponent<InRoom>();
    }
    private void Update()
    {
        if (mode == eMode.transition)
        {
            rigid.velocity = Vector3.zero;
            anim.speed = 0;
            roomPos = transitionPos;//Оставить Дрея на месте
            if (Time.time < transitionDone) return;
            //Следующая строка выполнится, только если Time.time>=transitionDone
            mode = eMode.idle;
        }
        //------Обработка ввода с клавиатуры и управление режимами eMode------
        dirHeld = -1;
        for (int i = 0; i < keys.Length; i++)
        {
            if (Input.GetKey(keys[i])) dirHeld = i;
        }
        //Нажата клавиша атаки
        if (Input.GetKeyDown(KeyCode.Z) && Time.time >= timeAtkNext)
        {
            mode = eMode.attack;
            timeAtkDone = Time.time + attackDuration;
            timeAtkNext = Time.time + attackDelay;
        }
        //Завершить атаку, если время истекло
        if (Time.time >= timeAtkDone)
            mode = eMode.idle;

        //Выбрать правильный режим. если Дрей не атакует
        if (mode != eMode.attack)
        {
            if (dirHeld == -1)
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
    private void LateUpdate()
    {
        //Получиь координаты узла сетки, с размером ячейки в половину единицыб ближайнего к данному персонажу
        Vector2 rPos = GetRoomPosOnGrid(0.5f);//Размер ячейки в пол-единицы
        //Персонаж находится на плите с дверью?
        int doorNum;
        for (doorNum = 0; doorNum < 4; doorNum++)
        {
            if (rPos == InRoom.DOORS[doorNum]) break;
        }
        if (doorNum > 3 || doorNum != facing) return;
        //Перейти в следующию комнату
        Vector2 rm = roomNum;
        switch (doorNum)
        {
            case 0:
                rm.x += 1;
                break;
            case 1:
                rm.y += 1;
                break;
            case 2:
                rm.x -= 1;
                break;
            case 3:
                rm.y -= 1;
                break;
        }
        //Проверить, можно ли выполнить переход в комнату rm
        if (rm.x >= 0 && rm.x <= InRoom.MAX_RM_X)
        {
            if (rm.y >= 0 && rm.y <= InRoom.MAX_RM_Y)
            {
                roomNum = rm;
                transitionPos = InRoom.DOORS[(doorNum + 2) % 4];
                roomPos = transitionPos;
                mode = eMode.transition;
                transitionDone = Time.time + transitionDelay;
            }
        }
    }
    //Реализация интерфейса IFacingMover
    public int GetFacing() => facing;
    public bool moving => (mode == eMode.move);
    public float GetSpeed() => speed;
    public float gridMult => inRm.GridMult;
    public Vector2 roomPos
    {
        get { return inRm.roomPos; }
        set { inRm.roomPos = value; }
    }
    public Vector2 roomNum
    {
        get { return inRm.roomNum; }
        set { inRm.roomNum = value; }
    }
    public Vector2 GetRoomPosOnGrid(float mult = -1) => inRm.GetRoomPosOnGrid(mult);

    //Реализация интерфейса IKeyMaster
    public int keyCount
    {
        get { return numKeys; }
        set { numKeys = value; }
    }
}
