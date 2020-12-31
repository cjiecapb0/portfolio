using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dray : MonoBehaviour, IFacingMover, IKeyMaster
{
    public enum EMode { idle, move, attack, transition, knockback }

    [Header("Set in Inspactor")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float attackDuration = 0.25f;//Продолжительность атаки в секундах

    [SerializeField] private float attackDelay = 0.5f;//Задержка между атаками
    [SerializeField] private float transitionDelay = 0.5f;//Задержка перехода между комнатами

    [SerializeField] private int maxHealth = 10;
    [SerializeField] private float knockbackSpeed = 10f;
    [SerializeField] private float knockbackDuration = 0.25f;
    [SerializeField] private float invincibleDuration = 0.5f;

    [Header("Set Dynamically")]
    [SerializeField] private int dirHeld = -1; //Направление соответсвующее удериваемой клавиши
    [SerializeField] private int numKeys = 0;
    [SerializeField] private bool invincible = false;
    [SerializeField] private bool hasGrappler = false;
    [SerializeField] private Vector3 lastSafeLoc;
    [SerializeField] private int lastSafeFacing;
    [SerializeField] private int _health;
    public int Health
    {
        get { return _health; }
        set { _health = value; }
    }
    private int facing = 1; //Направление движения Дрей
    private EMode mode = EMode.idle;

    private float timeAtkDone = 0;
    private float timeAtkNext = 0;

    private float transitionDone = 0;
    private Vector2 transitionPos;
    private float knockbackDone = 0;
    private float invincibleDone = 0;
    private Vector3 knockbackVel;

    public EMode Mode => mode;
    public bool HasGrappler => hasGrappler;

    private SpriteRenderer sRend;
    private Rigidbody rigid;
    private Animator anim;
    private InRoom inRm;

    private Vector3[] directions = new Vector3[] {
        Vector3.right, Vector3.up, Vector3.left, Vector3.down};
    private KeyCode[] keys = new KeyCode[] {
        KeyCode.RightArrow, KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.DownArrow};
    private void Awake()
    {
        sRend = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        inRm = GetComponent<InRoom>();
        Health = maxHealth;
        lastSafeLoc = transform.position;//начальная позиция безопасна.
        lastSafeFacing = facing;
    }
    private void Update()
    {
        //Проверить остояние неуязвимости и необходимость выполнить отбрасывание
        if (invincible && Time.time > invincibleDone) invincible = false;
        sRend.color = invincible ? Color.red : Color.white;
        if(mode == EMode.knockback)
        {
            rigid.velocity = knockbackVel;
            if (Time.time < knockbackDone) return;
        }

        if (mode == EMode.transition)
        {
            rigid.velocity = Vector3.zero;
            anim.speed = 0;
            roomPos = transitionPos;//Оставить Дрея на месте
            if (Time.time < transitionDone) return;
            //Следующая строка выполнится, только если Time.time>=transitionDone
            mode = EMode.idle;
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
            mode = EMode.attack;
            timeAtkDone = Time.time + attackDuration;
            timeAtkNext = Time.time + attackDelay;
        }
        //Завершить атаку, если время истекло
        if (Time.time >= timeAtkDone)
            mode = EMode.idle;

        //Выбрать правильный режим. если Дрей не атакует
        if (mode != EMode.attack)
        {
            if (dirHeld == -1)
                mode = EMode.idle;
            else
            {
                facing = dirHeld;
                mode = EMode.move;
            }
        }
        //-----Действие в текущем режиме----
        Vector3 vel = Vector3.zero;
        switch (mode)
        {
            case EMode.attack:
                anim.CrossFade("Dray_Attack_" + facing, 0);
                anim.speed = 0;
                break;
            case EMode.idle:
                anim.CrossFade("Dray_Walk_" + facing, 0);
                anim.speed = 0;
                break;
            case EMode.move:
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
                lastSafeLoc = transform.position;
                lastSafeFacing = facing;
                mode = EMode.transition;
                transitionDone = Time.time + transitionDelay;
            }
        }
    }
    private void OnCollisionEnter(Collision coll)
    {
        if (invincible) return;//Выйти, если Дрей пока неуязвим
        DamageEffect dEf = coll.gameObject.GetComponent<DamageEffect>();
        if (dEf == null) return;//Если компонент DamageEffect отсуствут  - выйти

        Health -= dEf.Damage;//Вычесть величину ущерба из уровня здоровья
        invincible = true;//Сделать Дрея неуязвимым
        invincibleDone = Time.time + invincibleDuration;

        if (dEf.Knockback)//Выполнить отбрасывание
        { //Определить направление отбрасывания
            Vector3 delta = transform.position = coll.transform.position;
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
            mode = EMode.knockback;
            knockbackDone = Time.time + knockbackDuration;

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        PickUp pup = other.GetComponent<PickUp>();
        if (pup == null) return;
        switch (pup.ItemType)
        {
            case PickUp.EType.health:
                Health = Mathf.Min(Health + 2, maxHealth);
                break;
            case PickUp.EType.key:
                keyCount++;
                break;
            case PickUp.EType.grappler:
                hasGrappler = true;
                break;
        }
        Destroy(other.gameObject);
    }
    public void ResetInRoom(int healthLoss = 0)
    {
        transform.position = lastSafeLoc;
        facing = lastSafeFacing;
        Health -= healthLoss;

        invincible = true;//Сделать Дрея неуязвимым
        invincibleDone = Time.time + invincibleDuration;
    }

    //Реализация интерфейса IFacingMover
    public int GetFacing() => facing;
    public bool moving => (mode == EMode.move);
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
