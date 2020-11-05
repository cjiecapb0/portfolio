using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdFly : MonoBehaviour
{
    public float speed = 6f;
    private Rigidbody2D rb;
    public GameObject birb;
    void Start()
    {
        rb = birb.GetComponent<Rigidbody2D>();
    }
    private void OnMouseDown()
    {
        rb.velocity = Vector2.up * speed;
    }
}
