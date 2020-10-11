using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projetile : MonoBehaviour
{
    private BoundsCheck bnbCheck;
    void Awake()
    {
        bnbCheck = GetComponent<BoundsCheck>();
    }
    void Update()
    {
        if (bnbCheck.offUp)
        {
            Destroy(gameObject);
        }
    }
}
