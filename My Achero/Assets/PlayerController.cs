using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    public GameObject laserPrefab;

    public float speed = 5f;
    private float nextFire;
    public float fireRate = 3f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        PlayerControll();
    }
    private void PlayerControll()
    {
        float xAxis = Input.GetAxis("Horizontal");
        float zAxis = Input.GetAxis("Vertical");

        Vector3 pos = transform.position;
        pos.x += xAxis * speed * Time.deltaTime;
        pos.z += zAxis * speed * Time.deltaTime;
        transform.position = pos;

        if (xAxis == 0 && zAxis == 0 && GameObject.FindGameObjectsWithTag("Enemy").Length != 0)
        {
            if (Time.time > nextFire)
            {
                nextFire = Time.time + fireRate;
                Instantiate(laserPrefab, transform.position + new Vector3(0, 0, 1f), Quaternion.identity);
            }
        }
    }
}
