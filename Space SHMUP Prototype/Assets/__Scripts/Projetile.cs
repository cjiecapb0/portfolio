﻿using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class Projetile : MonoBehaviour
{
    private BoundsCheck bnbCheck;
    private Renderer rend;
    [Header("Set Dynamically")]
    public Rigidbody rigid;
    [SerializeField]
    private WeaponType _type;
    //Это обедоступное свойство маскирует поле _type и обрабатывает
    //операции присваивания ему нового значения
    public WeaponType type
    {
        get
        {
            return (_type);
        }
        set
        {
            SetType(value);
        }
    }
    void Awake()
    {
        bnbCheck = GetComponent<BoundsCheck>();
        rend = GetComponent<Renderer>();
        rigid = GetComponent<Rigidbody>();
    }
    void Update()
    {
        if (bnbCheck.offUp)
        {
            Destroy(gameObject);
        }
    }
    ///<summary>
    ///Изменяет скрытое поле _type и устанавливает цвет этого снаряда,
    ///как определено в WeaponDefinition.
    ///</summary>
    ///<param name="eType">Тип WeaponType использыемого оружия.</param>
    public void SetType (WeaponType eType)
    {
        //Установить _type
        _type = eType;
        WeaponDefinition def = Main.GetWeaponDefinition(_type);
        rend.material.color = def.projectileColor;
    }
}
