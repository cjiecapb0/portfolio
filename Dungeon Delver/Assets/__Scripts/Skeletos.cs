using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeletos : Enemy,IFacingMover
{
    [Header("Set in Inspector")]
    [SerializeField] private int speed = 2;
    [SerializeField] private float timeThinkMin = 1f;
    [SerializeField] private float timeThinkMax = 4f;
    [Header("Set Dynamically: Skeletos")]
    [SerializeField] private int facing = 0;
    [SerializeField] private float timeNextDecision = 0;

    private InRoom inRm;
    protected override void Awake()
    {
        base.Awake();
        inRm = GetComponent<InRoom>();
    }
    private void Update()
    {
        if (Time.time >= timeNextDecision)
            DecideDirection();
        //Поле Rigid унаследовано от класса Enemy и инциализируется в Enemy.Awake()
        rigid.velocity = directions[facing] * speed;
    }
    private void DecideDirection()
    {
        facing = Random.Range(0, 4);
        timeNextDecision = Time.time + Random.Range(timeThinkMin, timeThinkMax);
    }
    //Реализация интерфейса IFacingMover
    public int GetFacing() => facing;
    public bool moving => true;
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
}
