using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEffect : MonoBehaviour
{
    [Header("Set in Inspector")]
    [SerializeField] private int damage = 1;
    [SerializeField] private bool knockback = true;

    public int Damage => damage;
    public bool Knockback => knockback;
}
