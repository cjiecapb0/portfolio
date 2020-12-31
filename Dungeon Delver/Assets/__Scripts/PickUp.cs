using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public enum EType { key,health, grappler}

    public static float COLLIDER_DELAY = 0.5f;
    [Header("Set in Inspector")]
    [SerializeField] private EType itemType;
    public EType ItemType => itemType;
    //Awake() и Activate() деактивирует коллайдер на 0,5 секунды
    private void Awake()
    {
        GetComponent<Collider>().enabled = false;
        Invoke("Activate", COLLIDER_DELAY);
    }
    private void Activate()
    {
        GetComponent<Collider>().enabled = true;
    }
}
